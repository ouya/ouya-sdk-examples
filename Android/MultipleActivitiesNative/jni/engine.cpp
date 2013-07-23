//----------------------------------------------------------------------------------
// File:        android/dynamic_resolution/jni/engine.cpp
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
#include "engine.h"

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>
#define GL_GLEXT_PROTOTYPES
#include <GLES2/gl2ext.h>

#include <math.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>
#include <nv_bitfont/nv_bitfont.h>
#include <nv_shader/nv_shader.h>

#include "render_target.h"
#include "resampler.h"
#include <time.h>

#define MODULE "dynamic_resolution"

namespace RenderMode
{
    enum
    {
        MODE_SBG = 0,
        MODE_FBO,
        NUM_MODES
    };
    static const char* getTxt(int v)
    {
        switch(v)
        {
            case MODE_SBG: return "MODE_SBG";
            case MODE_FBO: return "MODE_FBO";
            default: return "INVALID_MODE!";
        };
    }
};


// accessory function near the end of the file
static void createGeometry( int triCount, GLuint& vertexBuffer, GLuint& indexBuffer, int& vertCount, int& indexCount, float aspect);

struct Vertex
{
    float x, y, z, w;
    float rotCenterX;
    float rotCenterY;
    float r, g, b, a;
};
#define VERTEX_FLOATS sizeof(Vertex)/sizeof(float)
#define VERTEX_BYTES  sizeof(Vertex)
#define INDEX_BYTES   (sizeof(unsigned short))


Engine::Engine(NvEGLUtil& egl, struct android_app* app)
    : mApp(app)
    , mEgl(egl)
    , mResizePending(false)
    , mGameplayMode(true)
    , mForceRender(4)
    , mTimeVal(0.0)
    , mUiInitialized(false)
    , mFpsText(0)
    , mRenderResourcesInit(false)
    , mVertexCount(-1)
    , mVertexBuffer(0)
    , mIndexCount(-1)
    , mIndexBuffer(0)
    , mProgram(0)
    , mLastTime(mEgl.getSystemTime())
    , mFrameCount(0)
	, mWindowWidth(0)
	, mWindowHeight(0)
	, mMode(RenderMode::MODE_SBG)
    , mAspect(1.0f)
	, mBufferNeedsResize(false)
    , mRenderAreaFrac(1.0f)
    , mRenderWidth(0)
    , mRenderHeight(0)
	, mRequestedWidth(0)
	, mRequestedHeight(0)
    , mResampler(0)
{
    app->userData = this;
    app->onAppCmd = &Engine::handleCmdThunk;
    app->onInputEvent = &Engine::handleInputThunk;

    if(!app->activity->assetManager)
    {
        LOGD("asset manager invalid");
    }
    nv_shader_init(app->activity->assetManager);

    // set the current resolution to 100%
    mHitBtn = sNumResizeSteps-1; // last one.
}

Engine::~Engine()
{
    delete mResampler;

    glDeleteBuffers(1, &mVertexBuffer);
    glDeleteBuffers(1, &mIndexBuffer);
    glDeleteProgram(mProgram);
    NVBFTextFree(mPauseText);
    NVBFTextFree(mFpsText);
    NVBFTextFree(mStatusText);
    NVBFCleanup();
}

bool Engine::initRenderResources()
{
    if(mRenderResourcesInit) return true;

    createGeometry(615, mVertexBuffer, mIndexBuffer, mVertexCount, mIndexCount, mAspect);

    mProgram = nv_load_program("demo_");
    glUseProgram(mProgram);

    const float ROOT_3_OVER_2 = 0.8660254;
    const float ROOT_3_OVER_6 = ROOT_3_OVER_2/3.0;
    const float offsetBuf[] = { 0.5, -ROOT_3_OVER_6,
                               -0.5, -ROOT_3_OVER_6,
                                0.0,  ROOT_3_OVER_2 - ROOT_3_OVER_6 };
    glUniform2fv(glGetUniformLocation(mProgram, "s_vert"), 3, offsetBuf);

    // set states
    glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

    mRenderResourcesInit = true;
    return true;
}

bool Engine::initUI()
{
    if (mUiInitialized)
        return true;

    #define NUM_FONTS    2
    static NvBool fontsSplit[NUM_FONTS] = {1,1}; /* all are split */
    static const char *fontFiles[NUM_FONTS] = {
        "courier+lucida_256.dds",
        "utahcond+bold_1024.dds",
    };
    if (NVBFInitialize(NUM_FONTS, (const char**)fontFiles, fontsSplit, 0))
    {
        LOGW("Could not initialize NvBitFont");
        return false;
    }

    mFpsText = NVBFTextAlloc();
    NVBFTextSetFont(mFpsText, 2); // should look up by font file name.
    NVBFTextSetSize(mFpsText, 32);
    NVBFTextSetColor(mFpsText, NV_PC_PREDEF_WHITE);
    NVBFTextSetString(mFpsText, "60fps");

    mStatusText = NVBFTextAlloc();
    NVBFTextSetFont(mStatusText, 2); // should look up by font file name.
    NVBFTextSetSize(mStatusText, 32);
    NVBFTextSetColor(mStatusText, NV_PC_PREDEF_WHITE);
    NVBFTextSetString(mStatusText, "100% of pixels, 1366x720");

	mPauseText = NVBFTextAlloc();
	NVBFTextSetFont(mPauseText, 2); // should look up by font file name.
	NVBFTextSetSize(mPauseText, 32);
    NVBFTextSetColor(mPauseText, NV_PC_PREDEF_WHITE);
	NVBFTextSetShadow(mPauseText, -2, NV_PC_PREDEF_BLACK);
	NVBFTextSetString(mPauseText,
			NVBF_COLORSTR_RED NVBF_STYLESTR_BOLD "Auto-pause:\n" NVBF_STYLESTR_NORMAL
			NVBF_COLORSTR_NORMAL
			"Press back to quit\n"
			"Tap window to resume\n" );
    
    // create the buttons, we'll position in resize
	char tapString[16];
	for (int i=0; i<sNumResizeSteps; i++)
    {
		const int pct = getPercentForIndex(i);
		sprintf(tapString, NVBF_STYLESTR_BOLD "[ %d%% ]", pct);
		mTapText[i] = NVBFTextAlloc();
		NVBFTextSetFont(mTapText[i], 2);
		NVBFTextSetSize(mTapText[i], 32);
		NVBFTextSetColor(mTapText[i], NV_PC_PREDEF_WHITE);
		NVBFTextSetShadow(mTapText[i], -2, NV_PC_PREDEF_BLACK);
		NVBFTextSetString(mTapText[i], tapString );
    }

    // set the current button to selected
    NVBFTextSetMultiplyColor(mTapText[mHitBtn], 
            NV_PACKED_COLOR(50, 255, 50, 255) );

    mUiInitialized = true;

    return true;
}

void Engine::setGameplayMode(bool running)
{
    if (mGameplayMode != running)
        requestForceRender();

    mGameplayMode = running;
}

void Engine::computeSize(float frac, int windowWidth, int& renderWidth, int windowHeight, int& renderHeight)
{
    float f = sqrtf(frac);

    // round down unless at max
    renderWidth  = windowWidth;
    renderHeight = windowHeight;
    if( frac != 1.0f )
    {
        renderWidth  = (int(windowWidth * f))  / 16 * 16;
        renderHeight = (int(windowHeight * f)) / 16 * 16;
    }
}

// These are the function calls that should be made to set the render target
// size
void Engine::setRenderSize(ANativeWindow* window, int width, int height, int format)
{
	LOGD(">> setRenderSize %d x %d", width, height);
	mRequestedWidth = width;
	mRequestedHeight = height;
    switch( mMode )
    {
        case RenderMode::MODE_SBG:
            ANativeWindow_setBuffersGeometry(window, width, height, format);
            mBufferNeedsResize = false;
        break;
        case RenderMode::MODE_FBO:
        	// only an app doing benchmarking or something would touch _sBG here.
            //ANativeWindow_setBuffersGeometry(window, mWindowWidth, mWindowHeight, format);
        	// for FBO, the request == being set.
            mRenderWidth = width;
            mRenderHeight = height;
        break;
        default:
            LOGD(">>>>>>>>>>>>>>>>>> unknown mode set! %s, %d", __FILE__, __LINE__);
    }
}

void Engine::updateRenderSize()
{
    // reset the perf counter
    mFrameCount = 0;

    // determine what resolution we should render at
    int w, h;
    computeSize(mRenderAreaFrac, mWindowWidth, w, mWindowHeight, h);

    // request the new size (through whichever method we are using...)
    setRenderSize(mApp->window, w, h, mEgl.getFormat());
}

bool Engine::checkWindowResized()
{
	bool resized = false;
	
    if (mEgl.checkWindowResized())
    {
        LOGI("Window resize..."); 
        mResizePending = true;
        requestForceRender();
        // this won't account for rotation, but will
        // avoid downsizing issues with GB/HC and setBufferGeometry.
        int newW = mEgl.getWidth();
        int newH = mEgl.getHeight();
        if (newW>mWindowWidth || newH>mWindowHeight)
        {
        	mWindowWidth = newW;
        	mWindowHeight = newH;
            LOGI("Window size change %dx%d", mWindowWidth, mWindowHeight); 
            resized = true;
            
            if (mRenderWidth==0 && mRenderHeight==0)
            {
    			mRenderWidth = newW;
    			mRenderHeight = newH;
    	        LOGI("> Render surface size initialized: %dx%d", mRenderWidth, mRenderHeight);
            }
        }        
    }

    // don't do this for FBO at this time, we immediately set size for FBO.
    if (mMode == RenderMode::MODE_SBG) 
    {
        mResizePending = true;
        requestForceRender();
		int sw = mEgl.getSurfaceWidth();
		int sh = mEgl.getSurfaceHeight();
		if (sw != mRenderWidth || sh != mRenderHeight)
		{
			mRenderWidth = sw;
			mRenderHeight = sh;
	        LOGI("!!> Render surface size changed: %dx%d", mRenderWidth, mRenderHeight); 
	        resized = true;
		}
    }

    return resized;
}

static bool almostEq(int x, int y, int delta)
{
	return ((x + delta) > y) && ((x - delta) < y);
}

bool Engine::resizeIfNeeded()
{
    if (!mResizePending)
        return false;

    int w = mWindowWidth;
    int h = mWindowHeight;
    int height = (w > h) ? (h / 16) : (w / 16);

    NVBFSetScreenRes(w, h);
    
    if (mFpsText)
    {
        NVBFTextSetSize(mFpsText, height);
        NVBFTextCursorAlign(mFpsText, NVBF_ALIGN_LEFT, NVBF_ALIGN_TOP);
        NVBFTextCursorPos(mFpsText, 10, 10);
    }
    if (mStatusText)
    {
        NVBFTextSetSize(mStatusText, height);
        NVBFTextCursorAlign(mStatusText, NVBF_ALIGN_LEFT, NVBF_ALIGN_TOP);
        NVBFTextCursorPos(mStatusText, 10, 10 + height + 5);
    }
	if (mPauseText)
	{
		NVBFTextSetSize(mPauseText, height);
		NVBFTextCursorAlign(mPauseText, NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
		NVBFTextCursorPos(mPauseText, w/2, h/2);
	}

    // position buttons
    // NOTE: this code needs to be in screen pixels, because incoming touches
    //       are in screen pixels
    float screenBtnSpace = (w/2.0f);
    float screenBtnW     = screenBtnSpace/(float)sNumResizeSteps;
    float screenBtnH     = 64.0f;
    float screenLeft     = (w - screenBtnSpace)/2.0f; // left edge, centered space
    float screenTop      = h - w/16.0f;
	for (int i=0; i<sNumResizeSteps; i++)
	{
		mTapZone[i].set(screenLeft, screenTop, screenBtnW, screenBtnH);
		screenLeft += screenBtnW;
    }

    // resize text
    // NOTE: this code needs to be in current render target size units.
    //
    //       on HC mEgl.getWidth/Height return the current size that was set
    //       via ANativeWindow_setBuffersGeometry
    //
    //       on ICS the two functions will return the size of the window
    //       invariantly
    float btnSpace = (w/2.0f);
    float btnW = btnSpace/(float)sNumResizeSteps;
    float btnH = 64.0f;
    float left = (w - btnSpace)/2.0f; // left edge, centered space
    float top = h - w/16.0f;
	for (int i=0; i<sNumResizeSteps; i++)
	{
        NVBFTextSetSize(mTapText[i], height);
		NVBFTextCursorAlign(mTapText[i], NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
		NVBFTextCursorPos(mTapText[i], left + btnW/2.0f, top + btnH/2.0f - height/2.0f);
		mTapZone[i].set(left, top, btnW, btnH);
		left += btnW;
	}

    // update the render string in case render size change came through.
    const char *base = "%dx%d, %2.0f%% of pixels\n%s";
    char tmp[256];
    sprintf(tmp, base,
            mRenderWidth,
            mRenderHeight,
            100.0f * mRenderAreaFrac,
            "setBuffersGeometry");
    NVBFTextSetString(mStatusText, tmp);

    if (mBufferNeedsResize)
    {
    	mBufferNeedsResize = false;
    	if (mMode == RenderMode::MODE_SBG) // right now, only do something for SBG mode 
		{
    		if (mRequestedWidth>0 && mRequestedHeight>0)
			{
				LOGI("!!> Attemping to reset geometry: %dx%d", mRequestedWidth, mRequestedHeight); 
				ANativeWindow_setBuffersGeometry(mApp->window, mRequestedWidth, mRequestedHeight, mEgl.getFormat());
				// if it works, it'll cause another resize...
			}
		}
    }
    
    mResizePending = false;

    return true;
}

void Engine::renderScene()
{
    glUseProgram(mProgram);

    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);

    glEnable (GL_BLEND);
    glBlendFunc (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
    
    {
        glUniform1f(glGetUniformLocation(mProgram, "aspect"), mAspect);
        
        nv_set_attrib_by_name(mProgram, "pos_attr", 4, GL_FLOAT, 0, VERTEX_BYTES, 0 );
        nv_set_attrib_by_name(mProgram, "rot_center_attr", 2, GL_FLOAT, 0, VERTEX_BYTES, (void*)(sizeof(float)*4) );
        nv_set_attrib_by_name(mProgram, "col_attr", 4, GL_FLOAT, 0, VERTEX_BYTES, (void*)(sizeof(float)*6) );
        glDrawElements(GL_TRIANGLES, mIndexCount, GL_UNSIGNED_SHORT, 0);
        nv_flush_tracked_attribs(); // clear attrib binds.
    }

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
    glUseProgram(0);
}

void Engine::renderText(float fps)
{
    NVBFTextRenderPrep();
        
    // button text
    if (mGameplayMode)
    {
        // render buttons
		for (int i=0; i<sNumResizeSteps; i++) 
		{
			NVBFTextRender(mTapText[i]);
		}
        
        // render fps text if it's valid
        if (mFpsText && fps < 10000.0f)
        {
            const char *base = "%5.1ffps";
            char tmp[256];
            sprintf(tmp, base, fps);
            NVBFTextSetString(mFpsText, tmp);
            NVBFTextRender(mFpsText);
        }

        NVBFTextRender(mStatusText);
    }
    else //!mGameplayMode
    {
    	NVBFTextRender(mPauseText);
    }

    // done rendering overlaid text.
    NVBFTextRenderDone();
}


bool Engine::renderFrame(bool allocateIfNeeded)
{
    if (!mEgl.isReadyToRender(allocateIfNeeded))
        return false;

    if (!initUI())
    {
        LOGW("Could not initialize UI - assets may be missing!");
        ANativeActivity_finish(mApp->activity);
        return false;
    }

    resizeIfNeeded();
    mAspect = (mRenderWidth / (float)mRenderHeight);

    if (!initRenderResources())
    {
        LOGW("Could not initialize render resources !");
        ANativeActivity_finish(mApp->activity);
        return false;
    }
    
    // gather statistics
    ++mFrameCount;
    float avgFPS = FLT_MAX;
    if (mGameplayMode)
    {
        // capture timing information
        long currentTime = mEgl.getSystemTime();
        float fps = 1.0f / (0.001f * (currentTime - mLastTime));
        mLastTime = currentTime;

        // define when to start collecting mean/stddev
        static const int startStats = 5;
        const int numSamples = mFrameCount - startStats + 1;
        if( numSamples >= 1 )
        {
            // use a recurrence relation to compute running mean, should reduce
            // error when using floats
            if( numSamples == 1 )
            {
                mMean = fps;
            }
            else if( numSamples > 1 )
            {
                float prevMean = mMean;
                mMean = prevMean + (fps - prevMean) / numSamples;
            }
            avgFPS = mMean;
        }
    }

	if (mMode == RenderMode::MODE_FBO)
	{
	    if (NULL==mResampler)
	    {
	        // this has to happen after resize if needed because width & height aren't
	        // valid until AFTER.
	    	// this should happen once when window init size is set or rotated.
	    	
	    	// really want to set these to whatever maximum buffer size you need,
	    	// and NOT the window size.  we just use window size as this sample
	    	// dynamically resizes.  Normal apps would pick a max size and be done
	    	// most likely...
	    	int maxW = mWindowWidth;
	    	int maxH = mWindowHeight;
	        mResampler = new Resampler(maxW, maxH, mRenderWidth, mRenderHeight);
	    }
	    else
	    {
	    	// make sure we have the right size in the resampler.
	    	// right now this sets viewport too...
	    	mResampler->setOffscreenTarget(mRenderWidth, mRenderHeight);
	    }
	}

    // clear buffers as necessary
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    // set the target rendering size based upon known values, not requested ones...
    glViewport(0, 0, mRenderWidth, mRenderHeight);

    renderScene();

	if(mMode == RenderMode::MODE_FBO)
	{
		setBackbufferRender(mWindowWidth, mWindowHeight);
		mResampler->renderToBackbuffer();
	}

    // start rendering bitfont text overlaid here.
    renderText(avgFPS);

    if (mForceRender > 0)
        mForceRender--;

    mEgl.swap();

    return true;
}

void Engine::updateFrame(bool interactible, long deltaTime)
{
    if (interactible)
    {
        // Each frame, we check to see if the window has resized.  While the
        // various events we get _should_ cover this, in practice, it appears
        // that the safest move across all platforms and OSes is to check at 
        // the top of each frame
        checkWindowResized();

        // Time stands still when we're auto-paused, and we don't
        // automatically render
        if (mGameplayMode)
        {
            // This will try to set up EGL if it isn't set up
            // When we first set up EGL completely, we also load our GLES resources
            // If these are already set up or we succeed at setting them all up now, then
            // we go ahead and render.
            renderFrame(true);
        }
        else if (isForcedRenderPending()) // forced rendering when needed for UI, etc
        {
            renderFrame(true);
        }
    }
    else
    {
        // Even if we are not interactible, we may be visible, so we
        // HAVE to do any forced renderings if we can.  We must also
        // check for resize, since that may have been the point of the
        // forced render request in the first place!
        if (isForcedRenderPending() && mEgl.isReadyToRender(false)) 
        {
            checkWindowResized();
            renderFrame(false);
        }
    }
}

// If we cause an exception in JNI, we print the exception info to
// the log, we clear the exception to avoid a pending-exception
// crash, and we force the function to return.
#define EXCEPTION_RETURN(env) \
	if (env->ExceptionOccurred()) { \
		env->ExceptionDescribe(); \
		env->ExceptionClear(); \
		return false; \
	}

bool Engine::launchJavaActivity()
{
	LOGI(">launchJavaActivity");

	// Find the Intent class
	jclass intentClass = mApp->appThreadEnv->FindClass("android/content/Intent");
	EXCEPTION_RETURN(mApp->appThreadEnv);

	jstring actionString = mApp->appThreadEnv->NewStringUTF("tv.ouya.examples.android.multipleactivitiesnative.ACTION1");
	EXCEPTION_RETURN(mApp->appThreadEnv);

	// Get the initializer/constructor for Uri
	jmethodID newIntent = mApp->appThreadEnv->GetMethodID(intentClass, "<init>", 
		"()V");
	EXCEPTION_RETURN(mApp->appThreadEnv);

	// Call Intent intent = new Intent(actionString);  
	// Three stages: alloc object, then call init on it (init is the constructor)
	// then call the set functions
	// We SHOULD be able to set the action string and Action in <init>; it works on HC and ICS,
	// but GB fails.  So this longer code is safer on GB.
	jobject intent = mApp->appThreadEnv->AllocObject(intentClass); 
	EXCEPTION_RETURN(mApp->appThreadEnv);

	mApp->appThreadEnv->CallVoidMethod(intent, newIntent);
	EXCEPTION_RETURN(mApp->appThreadEnv);

	jmethodID setAction = mApp->appThreadEnv->GetMethodID(intentClass, "setAction", 
		"(Ljava/lang/String;)Landroid/content/Intent;");
	EXCEPTION_RETURN(mApp->appThreadEnv);

	// Set the action
	mApp->appThreadEnv->CallObjectMethod(intent, setAction, actionString);
	EXCEPTION_RETURN(mApp->appThreadEnv);

	// startActivity(launchBrowser);  
	jclass activityClass = mApp->appThreadEnv->FindClass("android/app/Activity");
	EXCEPTION_RETURN(mApp->appThreadEnv);

	jmethodID startActivity = mApp->appThreadEnv->GetMethodID(activityClass, 
		"startActivity", "(Landroid/content/Intent;)V");
	EXCEPTION_RETURN(mApp->appThreadEnv);

	mApp->appThreadEnv->CallVoidMethod(mApp->appThreadThis, startActivity, intent);
	EXCEPTION_RETURN(mApp->appThreadEnv);

	return true;
}

int Engine::switchModes(int mode, int buttonIndex )
{
    mMode = mode;
    mRenderAreaFrac = getPercentForIndex(buttonIndex)/100.0f;
    updateRenderSize();
    mHitBtn = buttonIndex;

    // update text vars that are dependent on "mode"
    for (int i=0; i<sNumResizeSteps; i++) 
    {
        NVBFTextSetMultiplyColor(mTapText[i], mHitBtn==i ?
                NV_PACKED_COLOR(50, 255, 50, 255) :		
                NV_PACKED_COLOR(255, 255, 255, 255) );
    }

    const char *base = "%dx%d, %2.0f%% of pixels\n%s";
    char tmp[256];
    sprintf(tmp, base,
            mRenderWidth,
            mRenderHeight,
            100.0f * mRenderAreaFrac,
            mMode==RenderMode::MODE_FBO?"FBO":"setBuffersGeometry");
    NVBFTextSetString(mStatusText, tmp);
}

int Engine::handleInput(AInputEvent* event)
{
    //We only handle motion events (touchscreen) and key (button/key) events
    int32_t eventType = AInputEvent_getType(event);

    if (eventType == AINPUT_EVENT_TYPE_MOTION)
    {
        int32_t action = AMOTION_EVENT_ACTION_MASK &
                         AMotionEvent_getAction((const AInputEvent*)event);

		// for now, just handle down as button press.
        if (action == AMOTION_EVENT_ACTION_DOWN)
        {
			int32_t mx = AMotionEvent_getX(event, 0);
			int32_t my = AMotionEvent_getY(event, 0);

			if (mGameplayMode)
			{
				// check if a tap in one of our text boxes
				for (int i=0; i<sNumResizeSteps; i++)
                {
					if (mTapZone[i].inside(mx, my))
					{
                        switchModes(mMode, i);
						break;
					}
                }
			}
			
			// switch to gameplay mode on any tap down on the screen.
            setGameplayMode(true);
        }

        return 1;
    }
    else if (eventType == AINPUT_EVENT_TYPE_KEY)
    {
        int32_t code = AKeyEvent_getKeyCode((const AInputEvent*)event);

		int32_t action = AMOTION_EVENT_ACTION_MASK &
                         AMotionEvent_getAction((const AInputEvent*)event);

		if (action == AMOTION_EVENT_ACTION_UP &&
			code == 21) //dpad left
		{
			if (sNumResizeSteps > 0)
			{
				mHitBtn = (mHitBtn+sNumResizeSteps-1) % sNumResizeSteps;
				switchModes(mMode, mHitBtn);
			}
		}

		else if (action == AMOTION_EVENT_ACTION_UP &&
			code == 22) //dpad right
		{
			if (sNumResizeSteps > 0)
			{
				mHitBtn = (mHitBtn+1) % sNumResizeSteps;
				switchModes(mMode, mHitBtn);
			}
		}

		else if (action == AMOTION_EVENT_ACTION_UP &&
			code == 19) //dpad up
		{
			mMode = RenderMode::MODE_SBG;
			switchModes(mMode, mHitBtn);
		}

		else if (action == AMOTION_EVENT_ACTION_UP &&
			code == 20) //dpad down
		{
			mMode = RenderMode::MODE_FBO;
			switchModes(mMode, mHitBtn);
		}

		else if(action == AMOTION_EVENT_ACTION_UP &&
			code == 23) //button O
		{
			launchJavaActivity();
		}

		//LOGI("Key: %d", code); 

        // if we are in gameplay mode, we eat the back button and move into
        // pause mode.  If we are already in pause mode, we allow the back
        // button to be handled by the OS, which means we'll be shut down
        if ((code == AKEYCODE_BACK) && mGameplayMode)
        {
            setGameplayMode(false);
            return 1;
        }
    }

    return 0;
}


void Engine::handleCommand(int cmd)
{
    switch (cmd)
    {
        // The window is being shown, get it ready.
        // Note that on ICS, the EGL size will often be correct for the new size here
        // But on HC it will not be.  We need to defer checking the new res until the
        // first render with the new surface!
        case APP_CMD_INIT_WINDOW:
        case APP_CMD_WINDOW_RESIZED:
            mEgl.setWindow(mApp->window);
			requestForceRender();
			if (mRequestedWidth>0 && mRequestedHeight>0)
				mBufferNeedsResize = true; // queue up a resize.
			break;

		case APP_CMD_TERM_WINDOW:
            // The window is being hidden or closed, clean it up.
            mEgl.setWindow(NULL);
            mBufferNeedsResize = false; // just in case...
            break;

        case APP_CMD_GAINED_FOCUS:
            requestForceRender();
            break;

        case APP_CMD_LOST_FOCUS:
        case APP_CMD_PAUSE:
            // Move out of gameplay mode if we are in it.  But if we are
            // in the paused mode, exit
            if (mGameplayMode)
                setGameplayMode(false);
            //else
            //    ANativeActivity_finish(mApp->activity);
    
            requestForceRender();
            break;

        // ICS does not appear to require this, but on GB phones,
        // not having this causes rotation changes to be delayed or
        // ignored when we're in a non-rendering mode like autopause.
        // The forced renders appear to allow GB to process the rotation
        case APP_CMD_CONFIG_CHANGED:
            requestForceRender();
            break;
    }
}

/**
 * Process the next input event.
 */
int32_t Engine::handleInputThunk(struct android_app* app, AInputEvent* event)
{
    Engine* engine = (Engine*)app->userData;
    if (!engine)
        return 0;

    return engine->handleInput(event);
}

/**
 * Process the next main command.
 */
void Engine::handleCmdThunk(struct android_app* app, int32_t cmd)
{
    Engine* engine = (Engine*)app->userData;
    if (!engine)
        return;

    engine->handleCommand(cmd);
}

static float fRand()
{
    return ((float)rand()/(float)RAND_MAX * 2.0f) - 1.0f;
}

static void createGeometry( int triCount, GLuint& vertexBuffer, GLuint& indexBuffer, int& vertCount, int& indexCount, float aspect)
{
    Vertex* vertices = 0;
    GLushort* indices = 0;

    // tweak triCount
    static const int rows = 5;
    int trisPerRow = triCount / rows;
    triCount = rows * trisPerRow;

    vertCount  = triCount * 3;
    indexCount = triCount * 3;

    // fill vertex buffer
    glDeleteBuffers(1, &vertexBuffer);
    glGenBuffers(1, &vertexBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
    glBufferData(GL_ARRAY_BUFFER, vertCount * VERTEX_BYTES, 0, GL_STATIC_DRAW);
    vertices = (Vertex*)glMapBufferOES( GL_ARRAY_BUFFER, GL_WRITE_ONLY_OES );
    Vertex* curVert = vertices;

    const float xinset = 0.3f;
    const float yinset = 0.4f;
    const float dx = (1.0f - xinset - (xinset - 1.0f)) / trisPerRow;
    const float dy = (1.0f - yinset - (yinset - 1.0f)) / (rows * aspect);

    // seed the random number generator to get reproducible results
    srand(0);

    float y = (yinset - 1.0f) / aspect;
    for(int row = 0; row < rows; ++row)
    {
        float x = xinset - 1.0f;
        for(int col = 0; col < trisPerRow; ++col)
        {
            for(int i = 0; i < 3; ++i)
            {
                curVert->x = x;
                curVert->y = y;
                curVert->z = i;
                curVert->w = 0.2f;
                // putting xy center of tri in here
                curVert->rotCenterX = x;
                curVert->rotCenterY = y;
                curVert->r = fRand();
                curVert->g = fRand();
                curVert->b = fRand();
                curVert->a = fRand() * 0.25f;
                ++curVert;

            }
            x += dx;
        }
        y += dy;
    }

    glUnmapBufferOES(GL_ARRAY_BUFFER);
    
    // fill index buffer
    glDeleteBuffers(1, &indexBuffer);
    glGenBuffers(1, &indexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indexCount * INDEX_BYTES, 0, GL_STATIC_DRAW);
    indices  = (unsigned short*)glMapBufferOES( GL_ELEMENT_ARRAY_BUFFER, GL_WRITE_ONLY_OES );
    GLushort* curIndex = indices;
    int curIdx = 0;
    for(int i = 0; i < indexCount; ++i)
    {
        *curIndex++ = curIdx++;
    }
    glUnmapBufferOES(GL_ELEMENT_ARRAY_BUFFER);

    #undef VERTEX_FLOATS
    #undef VERTEX_BYTES
    #undef INDEX_BYTES
}

