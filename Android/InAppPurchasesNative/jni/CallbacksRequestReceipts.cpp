#include "CallbacksRequestReceipts.h"
#include "CallbackSingleton.h"
#include "JSON.h"
#include "Receipt.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative_CallbacksRequestReceipts"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

void CallbacksRequestReceipts::OnSuccess(const char* jsonData)
{
	char buffer[1024];
	sprintf(buffer, "CallbacksRequestReceipts::OnSuccess:  %s\r\n", jsonData);
	LOGI(buffer);

	CallbackSingleton::GetInstance()->GetUI()->ClearReceipts();

	LOGI("Parsing JSON Data");

	// Parse example data
	JSONValue* value = JSON::Parse(jsonData);

	if (value == NULL)
	{
		LOGI("Parsing JSON Failed");
		return;
	}

	if (!value->IsArray())
	{
		LOGI("Parsing JSON Failed: Not an array");
		return;
	}

	// Retrieve the main object
	JSONArray data = value->AsArray();

	for (unsigned int i = 0; i < data.size(); i++)
	{
		Receipt newReceipt;
		newReceipt.ParseJSON(data[i]);

		CallbackSingleton::GetInstance()->GetUI()->AddReceipt(newReceipt);
	}
}

void CallbacksRequestReceipts::OnFailure(int errorCode, char* errorMessage)
{
}

void CallbacksRequestReceipts::OnCancel()
{
}