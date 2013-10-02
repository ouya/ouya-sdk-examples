#include <stdio.h>

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#include "Application.h"
#include "CallbackSingleton.h"
#include "engine.h"
#include "PluginOuya.h"

#define EXCEPTION_RETURN(env) \
	if (env->ExceptionOccurred()) { \
		env->ExceptionDescribe(); \
		env->ExceptionClear(); \
	}

extern "C"
{
	// JNI OnLoad
	JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* jvm, void* reserved)
	{
		//LOGI("************JNI_OnLoad*************");

		JNIEnv* env;
		jvm->GetEnv((void**) &env, JNI_VERSION_1_6);
		Application::m_pluginOuya.CacheClasses(env);

		return JNI_VERSION_1_6;
	}
}

/**
 * This is the main entry point of a native application that is using
 * android_native_app_glue.  It runs in its own thread, with its own
 * event loop for receiving input events and doing other things.
 */
void android_main(struct android_app* app)
{
	//LOGI("**********android_main***********");
	Application::m_app = app;

	Application::m_pluginOuya.AsyncSetDeveloperId("34eec327-0040-4b01-ace8-23a94e3a8394");

    // Make sure glue isn't stripped.
    app_dummy();

	NvEGLUtil* egl = NvEGLUtil::create();
    if (!egl) 
    {
        // if we have a basic EGL failure, we need to exit immediately; nothing else we can do
        nv_app_force_quit_no_cleanup(app);
        return;
    }

    Engine engine = Engine(*egl);

	long lastTime = egl->getSystemTime();

    // loop waiting for stuff to do.

	while (nv_app_status_running(app))
    {
        // Read all pending events.
        int ident;
        int events;
        struct android_poll_source* source;

        // If not rendering, we will block 250ms waiting for events.
        // If animating, we loop until all events are read, then continue
        // to draw the next frame of animation.
        while ((ident = ALooper_pollAll(((nv_app_status_focused(app) &&
										  engine.isGameplayMode()) ? 1 : 250),
        								NULL,
        								&events,
        								(void**)&source)) >= 0)
        {
            // Process this event. 
            if (source != NULL)
                source->process(app, source);

            // Check if we are exiting.  If so, dump out
            if (!nv_app_status_running(app))
				break;
        }

		long currentTime = egl->getSystemTime();

		// clamp time - it must not go backwards, and we don't
		// want it to be more than a half second to avoid huge
		// delays causing issues.  Note that we do not clamp to above
		// zero because some tools will give us zero delta
		long deltaTime = currentTime - lastTime;
		if (deltaTime < 0)
			deltaTime = 0;
		else if (deltaTime > 500)
			deltaTime = 500;

		lastTime = currentTime;

		// Update the frame, which optionally updates time and animations
		// and renders
		engine.updateFrame(nv_app_status_interactable(app), deltaTime);
    }

    delete egl;
}