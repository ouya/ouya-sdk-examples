#include "CallbackSingleton.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative_callbacks"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

CallbackSingleton* CallbackSingleton::m_instance = NULL;

CallbackSingleton::CallbackSingleton()
{
	CallbackSingleton::m_instance = this;

	m_callbacksFetchGamerUUID = NULL;
	m_callbacksRequestProducts = NULL;
	m_callbacksRequestPurchase = NULL;
	m_callbacksRequestReceipts = NULL;
}

CallbackSingleton* CallbackSingleton::GetInstance()
{
	if (!CallbackSingleton::m_instance)
	{
		CallbackSingleton::m_instance = new CallbackSingleton();
	}
	
	return CallbackSingleton::m_instance;
}

void CallbackSingleton::Initialize(UI* ui)
{
	m_ui = ui;
}

UI* CallbackSingleton::GetUI()
{
	return m_ui;
}

extern "C"
{
	JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess(JNIEnv* env, jobject thiz, jstring gamerUUID)
	{
		//LOGI("***********Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess***********");
		
		const char* strGamerUUID = env->GetStringUTFChars(gamerUUID, NULL);

		//char buffer[256];
		//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess: Returned to C: %s", strGamerUUID);
		//LOGI(buffer);
		
		CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
		if (callback)
		{
			callback->OnSuccess(strGamerUUID);
		}
	}
}