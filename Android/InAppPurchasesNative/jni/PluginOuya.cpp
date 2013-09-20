#include "PluginOuya.h"

#include <android/log.h>
#include "engine.h"

#include <stdio.h>

// If we cause an exception in JNI, we print the exception info to
// the log, we clear the exception to avoid a pending-exception
// crash, and we force the function to return.
#define EXCEPTION_RETURN(env) \
	if (env->ExceptionOccurred()) { \
		env->ExceptionDescribe(); \
		env->ExceptionClear(); \
	}

PluginOuya::PluginOuya()
{
	Initialized = false;
	m_developerId = NULL;
	m_jcAsyncOuyaSetDeveloperId = NULL;
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