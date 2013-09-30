#include "CallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative_CallbacksFetchGamerUUID"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

void CallbacksFetchGamerUUID::OnSuccess(std::string gamerUUID)
{
	char buffer[256];
	sprintf(buffer, "CallbacksFetchGamerUUID::OnSuccess:  %s", gamerUUID.c_str());
	LOGI(buffer);

	CallbackSingleton::GetInstance()->GetUI()->SetGamerUUID(gamerUUID);
}

void CallbacksFetchGamerUUID::OnFailure(int errorCode, std::string errorMessage)
{
	char buffer[256];
	sprintf(buffer, "CallbacksFetchGamerUUID::OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	LOGI(buffer);
}

void CallbacksFetchGamerUUID::OnCancel()
{
	LOGI("CallbacksFetchGamerUUID::OnCancel");
}