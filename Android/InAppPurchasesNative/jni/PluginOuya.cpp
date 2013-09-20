#include "PluginOuya.h"

#include <android/log.h>
#include "engine.h"

#include <stdio.h>

// If we cause an exception in JNI, we print the exception info to
// the log, we clear the exception to avoid a pending-exception
// crash, and we force the function to return.
#define EXCEPTION_RETURN(tag, env) \
	if (env->ExceptionOccurred()) { \
		env->ExceptionDescribe(); \
		env->ExceptionClear(); \
		char buffer[256]; \
		sprintf(buffer, "************Error occurred in %s", tag); \
		LOGI(buffer); \
		return; \
	}

PluginOuya::PluginOuya()
{
	Initialized = false;
	m_developerId = NULL;
	jc_AsyncOuyaSetDeveloperId = NULL;
}

void PluginOuya::FindClass(const char* tag, JNIEnv* env, const char* className, jclass* jc)
{
	char buffer[256];
	sprintf(buffer, "Find %s", className);
	__android_log_print(ANDROID_LOG_INFO, "PluginOuya", buffer);
	jclass localRef = env->FindClass(className);
	(*jc) = (jclass)env->NewGlobalRef(localRef);
}

void PluginOuya::CacheClasses(const char* tag, JNIEnv* env)
{
	FindClass(tag, env, "tv/ouya/sdk/android/AsyncOuyaSetDeveloperId", &jc_AsyncOuyaSetDeveloperId);
	EXCEPTION_RETURN(tag, env);
}

void PluginOuya::CheckJava(const char* tag, JNIEnv* env)
{
	if (!jc_AsyncOuyaSetDeveloperId)
	{
		LOGI("jc_AsyncOuyaSetDeveloperId is not initialized");
		return;
	}

	LOGI("Allocate DeveloperId String");
	jstring developerIdString = env->NewStringUTF("310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
	EXCEPTION_RETURN(tag, env);

	LOGI("allocate the object");
	jobject objSetDeveloperId = env->AllocObject(jc_AsyncOuyaSetDeveloperId); 
	EXCEPTION_RETURN(tag, env);

	LOGI("get the constructor");
	jmethodID constructSetDeveloperId = env->GetMethodID(jc_AsyncOuyaSetDeveloperId, "<init>", "()V");
	EXCEPTION_RETURN(tag, env);

	LOGI("construct the object");
	env->CallVoidMethod(objSetDeveloperId, constructSetDeveloperId);
	EXCEPTION_RETURN(tag, env);

	LOGI("get the invoke method");
	jmethodID invokeSetDeveloperId = env->GetStaticMethodID(jc_AsyncOuyaSetDeveloperId, "invoke", "(Ljava/lang/String;)V");
	EXCEPTION_RETURN(tag, env);

	LOGI("execute the invoke method");
	env->CallStaticVoidMethod(jc_AsyncOuyaSetDeveloperId, invokeSetDeveloperId, developerIdString);
	EXCEPTION_RETURN(tag, env);
}

void PluginOuya::SetApp(struct android_app* app)
{
	m_app = app;
}

void PluginOuya::Initialize()
{
	if (!Initialized)
	{
		OuyaSetDeveloperId();
		LOGI("plugin_ouya is initialized");
	}
}

void PluginOuya::SetDeveloperId(const char* developerId)
{
	m_developerId = developerId;
}

void PluginOuya::OuyaSetDeveloperId()
{
	if (Initialized)
	{
		return;
	}

	CheckJava("OuyaSetDeveloperId", m_app->appThreadEnv);

	/*

	char buffer[256];
	sprintf(buffer, "OuyaSetDeveloperId (%s)", m_developerId);
	LOGI(buffer);

	//LOGI("Allocate String");
	jstring actionString = m_app->appThreadEnv->NewStringUTF(m_developerId);
	EXCEPTION_RETURN(m_app->appThreadEnv);

	LOGI("Find AsyncOuyaSetDeveloperId");
	//jclass jcAsyncOuyaSetDeveloperId = m_app->appThreadEnv->FindClass("tv/ouya/sdk/android/AsyncOuyaSetDeveloperId");
	//EXCEPTION_RETURN(m_app->appThreadEnv);

	if (m_jcAsyncOuyaSetDeveloperId)
	{
		LOGI("+++++++++++++++++++++");
		LOGI("+++++++++++++++++++++");
		LOGI("+++++++++++++++++++++");
		LOGI("+++++++++++++++++++++");

		// Get the initializer/constructor for Uri
		jmethodID methodSetDeveloperId = m_app->appThreadEnv->GetMethodID(m_jcAsyncOuyaSetDeveloperId, "<init>", "()V");
		EXCEPTION_RETURN(m_app->appThreadEnv);
	}
	*/

	//ouyaSDK.ouyaSetDeveloperId(developerId);

	Initialized = true;
}

void PluginOuya::AsyncOuyaFetchGamerUUID(void* onSuccess, void* onFailure, void* onCancel)
{
	Initialize();

	LOGI("AsyncOuyaFetchGamerUUID");
}

void PluginOuya::AsyncOuyaRequestProducts (void* onSuccess, void* onFailure, void* onCancel, void* products)
{
	Initialize();

	LOGI("AsyncOuyaRequestProducts");
}

void PluginOuya::AsyncOuyaRequestPurchase (void* onSuccess, void* onFailure, void* onCancel, const char* purchasable)
{
	Initialize();

	LOGI("AsyncOuyaRequestPurchase");
}

void PluginOuya::AsyncOuyaRequestReceipts (void* onSuccess, void* onFailure, void* onCancel)
{
	Initialize();

	LOGI("AsyncOuyaRequestReceipts");
}