//----------------------------------------------------------------------------------
// File:        android/dynamic_resolution/jni/resampler.h
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

#ifndef __Resampler__
#define __Resampler__

class Resampler
{
public:
    Resampler(int maxWidth, int maxHeight, int bbWidth, int bbHeight);
    ~Resampler();

    // call this before rendering your scene, you can vary the renderWidth and renderHeight
    // varying these without regard to the aspect ratio can yield weird results
    // renderHeight = renderWidth * screenHeight / screenWidth is an example of goodness.
    void setOffscreenTarget(int renderWidth, int renderHeight);

    // call this after rendering your scene and setting the backbuffer to the current render target
    void renderToBackbuffer();

protected:
    GLuint mTexture, mFb, mRb;
    int32_t mTexWidth;
    int32_t mTexHeight;
    int32_t mRenderWidth; // must be <= texWidth
    int32_t mRenderHeight;// must be <= texHeight

    GLuint mBox;

    GLuint mPosAttr;
    GLuint mTcAttr;
};


#endif //__Resampler__
