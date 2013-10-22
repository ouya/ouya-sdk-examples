/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#include "JSON.h"
#include "JSONValue.h"
#include "JsonUtil.h"

#include <android/log.h>
#include <stdio.h>

#define APP_NAME "MarmaladeODK_JsonUtil"

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
		return NULL;
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
		return NULL;
	}
	if (item[fieldName]->IsNumber())
	{
		float field = (float)item[fieldName]->AsNumber();
		return field;
	}

	return 0;
}