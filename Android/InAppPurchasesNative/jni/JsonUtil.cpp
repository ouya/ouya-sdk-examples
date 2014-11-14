#include "JSON.h"
#include "JSONValue.h"
#include "JsonUtil.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative_Product"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

std::string JsonUtil::ParseString(JSONObject item, const std::wstring& fieldName)
{
	if (item.find(fieldName) == item.end())
	{
		std::string field( fieldName.begin(), fieldName.end() );
		std::string error = "Parsing JSON Failed: Can't find field name: ";
		error.append(field);
		LOGI(error.c_str());
		return "";
	}
	if (item[fieldName]->IsString())
	{
		const std::wstring wstr = item[fieldName]->AsString();

		std::string field( wstr.begin(), wstr.end() );

		//char buffer[1024];
		//sprintf(buffer, "ParseString=%s", field.c_str());
		//LOGI(buffer);

		return field;
	}

	return "";
}

int JsonUtil::ParseInt(JSONObject item, const std::wstring& fieldName)
{
	if (item.find(fieldName) == item.end())
	{
		LOGI("Parsing JSON Failed: Can't find field name");
		return 0;
	}
	if (item[fieldName]->IsNumber())
	{
		int field = (int)item[fieldName]->AsNumber();
		return field;
	}

	return 0;
}

float JsonUtil::ParseFloat(JSONObject item, const std::wstring& fieldName)
{
	if (item.find(fieldName) == item.end())
	{
		LOGI("Parsing JSON Failed: Can't find field name");
		return 0;
	}
	if (item[fieldName]->IsNumber())
	{
		float field = (float)item[fieldName]->AsNumber();
		return field;
	}

	return 0;
}