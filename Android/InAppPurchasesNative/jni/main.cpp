#include <stdio.h>

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#include "CallbackSingleton.h"
#include "engine.h"
#include "PluginOuya.h"

Engine* g_engine = 0;
PluginOuya g_pluginOuya;
UI g_ui;
JNIEnv* g_env = 0;
JavaVM* g_jvm = 0;

#define EXCEPTION_RETURN(env) \
	if (env->ExceptionOccurred()) { \
		env->ExceptionDescribe(); \
		env->ExceptionClear(); \
	}

// package tv.ouya.sdk.android;
// public class OuyaNativeActivity extends NativeActivity
//
// looks for
//
// tv.ouya.sdk.android.OuyaNativeActivity.hookJNI
//
// Native method not found: tv.ouya.sdk.android.OuyaNativeActivity.hookJNI:()V
//
//
//
// No implementation found for native Ltv/ouya/sdk/android/OuyaNativeActivity;.hookJNI:()V
//
//
extern "C"
{
	// JNI OnLoad
	JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* jvm, void* reserved)
	{
		LOGI("************JNI_OnLoad*************");

		g_jvm = jvm;

		if (jvm)
		{
			LOGI("JVM IS VALID");
		}
		else
		{
			LOGI("JVM IS INVALID");
		}

		JNIEnv* env;
		jvm->GetEnv((void**) &env, JNI_VERSION_1_6);
		//jvm->AttachCurrentThread(&env, NULL);

		g_pluginOuya.CacheClasses("JNI_OnLoad", env);

		//spawn thread? Check for access?

		return JNI_VERSION_1_6;
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_OuyaNativeActivity_hookJNI(JNIEnv* env, jobject thiz)
	{
		return;

		/*

		LOGI("***********Java_tv_ouya_sdk_android_OuyaNativeActivity_hookJNI***********");

		g_pluginOuya.CheckJava("Java_tv_ouya_sdk_android_OuyaNativeActivity_hookJNI", env);

		*/
	}
}

/**
 * This is the main entry point of a native application that is using
 * android_native_app_glue.  It runs in its own thread, with its own
 * event loop for receiving input events and doing other things.
 */
void android_main(struct android_app* app)
{
	LOGI("**********android_main***********");

	g_pluginOuya.SetDeveloperId("34eec327-0040-4b01-ace8-23a94e3a8394");
	g_pluginOuya.SetApp(app);

	//g_jvm->AttachCurrentThread(&app->appThreadEnv, NULL);
	//g_pluginOuya.CheckJava("android_main", app->appThreadEnv);

	//JNIEnv* env;
	//g_jvm->GetEnv((void**) &env, JNI_VERSION_1_6);
	//g_jvm->AttachCurrentThread(&env, NULL);
	//g_pluginOuya.CheckJava("android_main", env);	

	//JavaVM* jvm;
	//app->appThreadEnv->GetJavaVM(&jvm);
	//jvm->AttachCurrentThread(&app->appThreadEnv, NULL);
	g_ui.Initialize(&g_pluginOuya);
	g_pluginOuya.Initialize();
	CallbackSingleton::GetInstance()->Initialize(&g_ui);
	CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID = new CallbacksFetchGamerUUID();
	CallbackSingleton::GetInstance()->m_callbacksRequestProducts = new CallbacksRequestProducts();
	CallbackSingleton::GetInstance()->m_callbacksRequestPurchase = new CallbacksRequestPurchase();
	CallbackSingleton::GetInstance()->m_callbacksRequestReceipts = new CallbacksRequestReceipts();
	

    // Make sure glue isn't stripped.
    app_dummy();

	NvEGLUtil* egl = NvEGLUtil::create();
    if (!egl) 
    {
        // if we have a basic EGL failure, we need to exit immediately; nothing else we can do
        nv_app_force_quit_no_cleanup(app);
        return;
    }

    g_engine = new Engine(*egl, app, &g_pluginOuya, &g_ui);

	g_ui.SetEngine(g_engine);

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
										  g_engine->isGameplayMode()) ? 1 : 250),
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
		g_engine->updateFrame(nv_app_status_interactable(app), deltaTime);
    }

	delete g_engine;
	g_engine = NULL;
    delete egl;
}
