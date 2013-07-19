//----------------------------------------------------------------------------------
// File:        android/dynamic_resolution/jni/render_target.h
// SDK Version: v10.00 
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

#ifndef __RENDER_TARGET__
#define __RENDER_TARGET__

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

// invoke before calling the resampler
inline void setBackbufferRender(int width, int height)
{
	glBindFramebuffer(GL_FRAMEBUFFER, 0);
	glViewport(0, 0, width, height);
}

inline void setOffscreenRender(GLuint fb, int renderWidth, int renderHeight)
{
	glBindFramebuffer(GL_FRAMEBUFFER, fb);
	glViewport(0, 0, renderWidth, renderHeight);
}

inline bool initOffscreenBuffers(GLuint* rb, GLuint* texture, GLuint* fb, int offscreenWidth, int offscreenHeight, int backbufferWidth, int backbufferHeight)
{
    // create a texture to hold our color data
	glGenTextures(1, texture);
    glGenFramebuffers(1, fb);
    glGenRenderbuffers(1, rb);

	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, *texture);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, offscreenWidth, offscreenHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, 0);
	glBindTexture(GL_TEXTURE_2D, 0);

    // create fbobject
    glBindFramebuffer(GL_FRAMEBUFFER, *fb);

    // create depth buffer
    glBindRenderbuffer(GL_RENDERBUFFER, *rb);
    glRenderbufferStorage( GL_RENDERBUFFER, GL_DEPTH_COMPONENT16, offscreenWidth, offscreenHeight);
    glBindRenderbuffer(GL_RENDERBUFFER, 0);

    // attach texture to fbobject
    glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, *texture, 0);

    // attach rb to framebuffer
    glFramebufferRenderbuffer( GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, *rb );

    // set the rendering back to backbuffer to preserve "incoming state"
    setBackbufferRender(backbufferWidth, backbufferHeight);
}

inline void deleteOffscreenBuffers(GLuint* rb, GLuint* texture, GLuint* fb)
{
    glDeleteRenderbuffers(1, rb);
    glDeleteTextures(1, texture);
    glDeleteFramebuffers(1, fb);
}

#endif //__RENDER_TARGET__
