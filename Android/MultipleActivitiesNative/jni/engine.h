//----------------------------------------------------------------------------------
// File:        android/dynamic_resolution/jni/engine.h
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
#ifndef __ENGINE_H
#define __ENGINE_H

#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>
#include <math.h>
#include <GLES2/gl2.h>
#include <nv_ui/nv_ui_rect.h>


#define APP_NAME "dynamic_resolution"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

// Compute the height and width required to reduce the rendered pixel count by
// the specified fraction.
// This yields a linear control for fps -- halving frac
// doubles framerate

class Resampler;

class Engine
{
public:
	Engine(NvEGLUtil& egl, struct android_app* app);
	~Engine();

	bool isGameplayMode() { return mGameplayMode; }

	void updateFrame(bool interactible, long deltaTime);

	bool launchJavaActivity();

protected:

	void setGameplayMode(bool paused);

	void advanceTime(long time) { mTimeVal += time * 0.001; }

	bool isForcedRenderPending() { return mForceRender > 0; }
	void requestForceRender() { mForceRender = 4; }

    void updateRenderSize();
	bool checkWindowResized();

	bool renderFrame(bool allocateIfNeeded);
    void renderScene();
    void renderText(float fps);

	static void handleCmdThunk(struct android_app* app, int32_t cmd);
	static int32_t handleInputThunk(struct android_app* app, AInputEvent* event);

	bool initRenderResources();
	bool initUI();

    int switchModes(int mode, int buttonIndex);
	void computeSize(float frac, int windowWidth, int& renderWidth, int windowHeight, int& renderHeight);
	void setRenderSize(ANativeWindow* window, int width, int height, int format);
	bool resizeIfNeeded();

	int handleInput(AInputEvent* event);
    bool handleBarInput(int touchType, float xPixels, float yPixels);
	void handleCommand(int cmd);

    void compileBarShaders();
    void renderBar();

    struct android_app* mApp;

	NvEGLUtil& mEgl;

	bool mResizePending;

	bool mGameplayMode;

	int mForceRender;

    double mTimeVal;

	bool mUiInitialized;

	void *mFpsText;
	void *mStatusText;
	void *mPauseText;

    float  mAspect;

    bool   mRenderResourcesInit;
    int    mVertexCount;
    uint32_t mVertexBuffer;
    int    mIndexCount;
    uint32_t mIndexBuffer;
    uint32_t mProgram;

	long mLastTime;
    int mFrameCount;
    float mMean;

	bool mConfigChanged;
	int mWindowWidth;
	int mWindowHeight;

    int mMode;
    bool mBufferNeedsResize;
    
    float mRenderAreaFrac;
    // requested render buffer size
	int	mRequestedWidth;
	int	mRequestedHeight;
    // actual render buffer size
    int   mRenderWidth;
    int   mRenderHeight;

    // FBO rendering related var
    Resampler* mResampler;

    // ui button related variables
    static const int sNumResizeSteps = 6;
    static const int sNumResizePct = 10;
    inline int getPercentForIndex(int index)
    {
    	if (index<0 || index>=sNumResizeSteps)
    		return 100;
		switch (index)
		{
		case 5:
			return 100;
		case 4:
			return 50;
		case 3:
			return 45;
		case 2:
			return 40;
		case 1:
			return 35;
		case 0:
			return 30;
		}
    	return 100 - ((sNumResizeSteps-index-1)*sNumResizePct);
    };

    int mHitBtn;

	NvUIRect mTapZone[sNumResizeSteps];
	void *mTapText[sNumResizeSteps];
};


#endif // __ENGINE_H
