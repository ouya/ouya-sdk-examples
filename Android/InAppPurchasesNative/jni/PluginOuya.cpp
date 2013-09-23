#include "PluginOuya.h"

#include <android/log.h>
#include "engine.h"

#include <stdio.h>

#define LOG_TAG "PluginOuya"

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
	jc_AsyncCppOuyaSetDeveloperId = NULL;
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
	FindClass(tag, env, "tv/ouya/sdk/android/AsyncCppOuyaSetDeveloperId", &jc_AsyncCppOuyaSetDeveloperId);
	EXCEPTION_RETURN(tag, env);

	FindClass(tag, env, "tv/ouya/sdk/android/AsyncCppOuyaFetchGamerUUID", &jc_AsyncCppOuyaFetchGamerUUID);
	EXCEPTION_RETURN(tag, env);

	FindClass(tag, env, "tv/ouya/sdk/android/AsyncCppOuyaRequestProducts", &jc_AsyncCppOuyaRequestProducts);
	EXCEPTION_RETURN(tag, env);

	FindClass(tag, env, "tv/ouya/sdk/android/AsyncCppOuyaRequestPurchase", &jc_AsyncCppOuyaRequestPurchase);
	EXCEPTION_RETURN(tag, env);

	FindClass(tag, env, "tv/ouya/sdk/android/AsyncCppOuyaRequestReceipts", &jc_AsyncCppOuyaRequestReceipts);
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
		AsyncSetDeveloperId();
		LOGI("plugin_ouya is initialized");
	}
}

void PluginOuya::SetDeveloperId(const char* developerId)
{
	m_developerId = developerId;
}

void PluginOuya::AsyncSetDeveloperId()
{
	if (Initialized)
	{
		return;
	}

	if (!jc_AsyncCppOuyaSetDeveloperId)
	{
		LOGI("jc_AsyncOuyaSetDeveloperId is not initialized");
		return;
	}

	JNIEnv* env = m_app->appThreadEnv;

	LOGI("Allocate DeveloperId String");
	jstring developerIdString = env->NewStringUTF(m_developerId);
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("allocate the object");
	jobject objSetDeveloperId = env->AllocObject(jc_AsyncCppOuyaSetDeveloperId); 
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("get the constructor");
	jmethodID constructSetDeveloperId = env->GetMethodID(jc_AsyncCppOuyaSetDeveloperId, "<init>", "()V");
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("construct the object");
	env->CallVoidMethod(objSetDeveloperId, constructSetDeveloperId);
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("get the invoke method");
	jmethodID invokeSetDeveloperId = env->GetStaticMethodID(jc_AsyncCppOuyaSetDeveloperId, "invoke", "(Ljava/lang/String;)V");
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("execute the invoke method");
	env->CallStaticVoidMethod(jc_AsyncCppOuyaSetDeveloperId, invokeSetDeveloperId, developerIdString);
	EXCEPTION_RETURN(LOG_TAG, env);

	Initialized = true;
}

void PluginOuya::AsyncOuyaFetchGamerUUID(CallbacksFetchGamerUUID* callbacksFetchGamerUUID)
{
	Initialize();

	LOGI("AsyncOuyaFetchGamerUUID");

	JNIEnv* env = m_app->appThreadEnv;

	LOGI("get the invoke method");
	jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaFetchGamerUUID, "invoke", "()V");
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("execute the invoke method");
	env->CallStaticVoidMethod(jc_AsyncCppOuyaFetchGamerUUID, invokeMethod);
	EXCEPTION_RETURN(LOG_TAG, env);
}

void PluginOuya::AsyncOuyaRequestProducts(CallbacksRequestProducts* callbacksRequestProducts)
{
	Initialize();

	LOGI("AsyncOuyaRequestProducts");

	JNIEnv* env = m_app->appThreadEnv;

	LOGI("get the invoke method");
	jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestProducts, "invoke", "()V");
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("execute the invoke method");
	env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestProducts, invokeMethod, (int)callbacksRequestProducts);
	EXCEPTION_RETURN(LOG_TAG, env);
}

void PluginOuya::AsyncOuyaRequestPurchase(CallbacksRequestPurchase* callbacksRequestPurchase, const char* purchasable)
{
	Initialize();

	LOGI("AsyncOuyaRequestPurchase");

	JNIEnv* env = m_app->appThreadEnv;

	LOGI("Allocate purchasable String");
	jstring purchasableString = env->NewStringUTF(purchasable);
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("get the invoke method");
	jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestPurchase, "invoke", "(Ljava/lang/String;)V");
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("execute the invoke method");
	env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestPurchase, invokeMethod, purchasableString);
	EXCEPTION_RETURN(LOG_TAG, env);
}

void PluginOuya::AsyncOuyaRequestReceipts(CallbacksRequestReceipts* callbacksRequestReceipts)
{
	Initialize();

	LOGI("AsyncOuyaRequestReceipts");

	JNIEnv* env = m_app->appThreadEnv;

	LOGI("get the invoke method");
	jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestReceipts, "invoke", "()V");
	EXCEPTION_RETURN(LOG_TAG, env);

	LOGI("execute the invoke method");
	env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestReceipts, invokeMethod);
	EXCEPTION_RETURN(LOG_TAG, env);
}