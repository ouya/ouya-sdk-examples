#include "engine.h"

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>
#include <nv_bitfont/nv_bitfont.h>
#include <nv_shader/nv_shader.h>

Engine::Engine(NvEGLUtil& egl, struct android_app* app, PluginOuya* pluginOuya, UI* ui) :
	mEgl(egl)
{
    mApp = app;
	m_pluginOuya = pluginOuya;
	m_ui = ui;

	mResizePending = false;

	mGameplayMode = true;

	mForceRender = 4;

    mTimeVal = 0.0;

    app->userData = this;
	app->onAppCmd = &Engine::handleCmdThunk;
    app->onInputEvent = &Engine::handleInputThunk;

	nv_shader_init(app->activity->assetManager);

	m_pluginOuya->SetApp(app);
	m_pluginOuya->SetDeveloperId("310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
}

Engine::~Engine()
{
	m_ui->Destroy();

	NVBFCleanup();
}

bool Engine::initUI()
{
	m_ui->InitUI();	

	return true;
}

void Engine::setGameplayMode(bool running)
{
	if (mGameplayMode != running)
		requestForceRender();

	mGameplayMode = running;
}

bool Engine::checkWindowResized()
{
	if (mEgl.checkWindowResized())
	{
		mResizePending = true;
		requestForceRender();
		LOGI("Window size change %dx%d", mEgl.getWidth(), mEgl.getHeight()); 
		return true;
	}

	return false;
}

bool Engine::resizeIfNeeded()
{
	if (!mResizePending)
		return false;

	int w = mEgl.getWidth();
	int h = mEgl.getHeight();
	int height = (w > h) ? (h / 16) : (w / 16);

	NVBFSetScreenRes(w, h);

	m_ui->Resize(w, h);	

	mResizePending = false;

	return true;
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

	// set up viewport
	glViewport((GLint)0, (GLint)0, 
		(GLsizei)(mEgl.getWidth()), (GLsizei)(mEgl.getHeight()));

	// clear buffers as necessary
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	// do some game rendering here
	// ...
	
	// start rendering bitfont text overlaid here.
	NVBFTextRenderPrep();
		
	m_ui->Render();

	// done rendering overlaid text.
	NVBFTextRenderDone();

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
			advanceTime(deltaTime);

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

int Engine::handleInput(AInputEvent* event)
{
    //We only handle motion events (touchscreen) and key (button/key) events
	int32_t eventType = AInputEvent_getType(event);

	if (eventType == AINPUT_EVENT_TYPE_MOTION)
	{
		return 1;
	} else if (eventType == AINPUT_EVENT_TYPE_KEY) {
		int32_t code = AKeyEvent_getKeyCode((const AInputEvent*)event);

		int32_t action = AMOTION_EVENT_ACTION_MASK &
							AMotionEvent_getAction((const AInputEvent*)event);

		//char buffer[256];
		//sprintf(buffer, "%d", code);
		//LOGI(buffer);

		m_ui->HandleInput(code, action);
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
        	break;

        case APP_CMD_TERM_WINDOW:
            // The window is being hidden or closed, clean it up.
			mEgl.setWindow(NULL);
			break;

        case APP_CMD_GAINED_FOCUS:
			requestForceRender();
			break;

        case APP_CMD_LOST_FOCUS:
		case APP_CMD_PAUSE:
        	// Move out of gameplay mode if we are in it.  But if we are
			// in another dialog mode, leave it as-is
            if (mGameplayMode)
				setGameplayMode(false);
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
