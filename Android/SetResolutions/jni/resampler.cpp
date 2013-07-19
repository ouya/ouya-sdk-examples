//----------------------------------------------------------------------------------
// File:        android/dynamic_resolution/jni/resampler.cpp
// SDK Version: v10.10 
// Email:       tegradev@nvidia.com
// Site:        http://developer.nvidia.com/
//
// Copyright (c) 2007-2012, NVIDIA CORPORATION.  All rights reserved.
//
// TO  THE MAXIMUM  EXTENT PERMITTED  BY APPLICABLE  LAW, THIS SOFTWARE  IS PROVIDED
// *AS IS*  AND NVIDIA AND  ITS SUPPLIERS DISCLAIM  ALL WARRANTIES,  EITHER  EXPRESS
// OR IMPLIED, INCLUDING, BUT NOT LIMITED  TO, IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL  NVIDIA OR ITS SUPPLIERS
// BE  LIABLE  FOR  ANY  SPECIAL,  INCIDENTAL,  INDIRECT,  OR  CONSEQUENTIAL DAMAGES
// WHATSOEVER (INCLUDING, WITHOUT LIMITATION,  DAMAGES FOR LOSS OF BUSINESS PROFITS,
// BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY LOSS)
// ARISING OUT OF THE  USE OF OR INABILITY  TO USE THIS SOFTWARE, EVEN IF NVIDIA HAS
// BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
//
//
//----------------------------------------------------------------------------------

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>
#include <GLES2/gl2ext.h>

#include <unistd.h>

#include <math.h>
#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include <android/log.h>

#include <nv_shader/nv_shader.h>

#define MODULE "dynamic_resolution_trivial_upsample"
#include "resampler.h"
#include "render_target.h"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 MODULE, __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,   \
											 MODULE, __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,   \
											 MODULE, __VA_ARGS__))

#define dimof(x) (sizeof(x[0])/sizeof(x))

static const GLubyte quadIndices [6] = {  0, 2, 3, 0, 3, 1 };

static const GLfloat quadVertices[8] = { -1.0f,-1.0f,
                                          1.0f,-1.0f,
                                         -1.0f, 1.0f,
                                          1.0f, 1.0f };

static void renderQuad(GLuint prog, GLuint mPosAttr, GLuint mTcAttr, float xscale, float yscale)
{
    const GLfloat quadTcs[] = { 0.0f,0.0f,   xscale,0.0f,   0.0f,yscale, xscale,yscale};

    glVertexAttribPointer(mPosAttr, 2, GL_FLOAT, 0, 0, quadVertices);
    glEnableVertexAttribArray(mPosAttr);
    glVertexAttribPointer(mTcAttr, 2, GL_FLOAT, 0, 0, quadTcs);
    glEnableVertexAttribArray(mTcAttr);

	glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_BYTE, quadIndices);

	glDisableVertexAttribArray(mPosAttr);
	glDisableVertexAttribArray(mTcAttr);
}


Resampler::Resampler(int maxWidth, int maxHeight, int bbWidth, int bbHeight)
    : mTexture(0)
    , mFb(0)
    , mRb(0)
    , mTexWidth(maxWidth)
    , mTexHeight(maxHeight)
    , mRenderWidth(maxWidth)
    , mRenderHeight(maxWidth)
    , mBox(0)
    , mPosAttr(0)
    , mTcAttr(0)
{
    static const char vs[] =
        "\n"
        "attribute vec2 mPosAttr;\n"
        "attribute vec2 mTcAttr;\n"
        "varying vec2 tc;\n"
        "\n"
        "void main()\n"
        "{\n"
        "    gl_Position = vec4(mPosAttr.xy, 0.0, 1.0);\n"
        "    tc = mTcAttr;\n"
        "}\n";

    static const char ps[] =
        "precision lowp float;\n"
        "\n"
        "varying vec2 tc;\n"
        "\n"
        "uniform sampler2D tex0;\n"
        "\n"
        "void main()\n"
        "{\n"
        "    gl_FragColor = texture2D(tex0, tc);\n"
        "}\n";

    mBox = nv_load_program_from_strings((const char*)vs, (const char*)ps);

    glUseProgram(mBox);
    glUniform1i(glGetUniformLocation(mBox, "tex0"), 0);
	mPosAttr = glGetAttribLocation(mBox, "mPosAttr");
	mTcAttr  = glGetAttribLocation(mBox, "mTcAttr");

	initOffscreenBuffers(&mRb, &mTexture, &mFb, maxWidth, maxHeight, maxWidth, maxHeight);
    LOGD("allocated %d x %d render target", mTexWidth, mTexHeight);
}

Resampler::~Resampler()
{
    deleteOffscreenBuffers(&mRb, &mTexture, &mFb);

	if(mBox)
	{
		glDeleteProgram(mBox);
		mBox = 0;
	}
}

// set the FBO as the render target
void Resampler::setOffscreenTarget(int renderWidth, int renderHeight)
{
	mRenderWidth = renderWidth;
	mRenderHeight = renderHeight;
	setOffscreenRender(mFb, renderWidth, renderHeight);
}

// render a quad to the backbuffer with mTexture on it, scaled appropriately
void Resampler::renderToBackbuffer()
{
	glUseProgram(mBox);
	glBindTexture(GL_TEXTURE_2D, mTexture);

	renderQuad(mBox, mPosAttr, mTcAttr, (float)mRenderWidth/mTexWidth, (float)mRenderHeight/mTexHeight);

	glBindTexture(GL_TEXTURE_2D, 0);
	glUseProgram(0);
}

