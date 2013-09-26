#include "CallbacksRequestProducts.h"
#include "CallbackSingleton.h"
#include "JSON.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#include <iostream>

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
	sprintf(buffer, "CallbacksRequestProducts::OnSuccess:  %s\r\n", jsonData);
	LOGI(buffer);

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
		if (!data[i]->IsObject())
		{
			LOGI("Parsing JSON Failed: Not an object item");
			continue;
		}
		JSONObject item = data[i]->AsObject();
		if (item.find(L"identifier") == item.end())
		{
			LOGI("Parsing JSON Failed: Can't find identifier");
			continue;
		}
		if (item[L"identifier"]->IsString())
		{
			const wchar_t* wstr = item[L"identifier"]->AsString().c_str();

			///*
			//char id[512];
			//std::wcsrtombs(id, &wstr, 512, 0);
			//sprintf(id, "id=%s\r\n", str);
			//LOGI(id);
			//*/
		}
	}

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