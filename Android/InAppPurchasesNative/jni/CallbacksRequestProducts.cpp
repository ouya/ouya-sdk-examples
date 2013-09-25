#include "CallbacksRequestProducts.h"
#include "CallbackSingleton.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative_CallbacksRequestProducts"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

void CallbacksRequestProducts::OnSuccess(const char* jsonData)
{
	char buffer[1024];
	sprintf(buffer, "CallbacksRequestProducts::OnSuccess:  %s", jsonData);
	LOGI(buffer);

	LOGI("*******************");
	LOGI("*******************");
	LOGI("*******************");
	LOGI("*******************");
}

void CallbacksRequestProducts::OnFailure(int errorCode, const char* errorMessage)
{
}

void CallbacksRequestProducts::OnCancel()
{
}