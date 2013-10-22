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
#include "Product.h"

#include <android/log.h>
#include <stdio.h>

#define APP_NAME "MarmaladeODK_Product"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))
namespace OuyaSDK
{
	void ExtensionProduct::Init()
	{
		CurrencyCode = NULL;
		Identifier = NULL;
		Name = NULL;
		ProductVersionToBundle = 0;
		LocalPrice = 0;
		PriceInCents = 0;
	}

	void Product::Init()
	{
		CurrencyCode = "";
		Identifier = "";
		Name = "";
		ProductVersionToBundle = 0;
		LocalPrice = 0;
		PriceInCents = 0;
	}

	void Product::ParseJSON(JSONValue* json)
	{
		Init();

		if (!json)
		{
			LOGI("Parsing JSON Failed: Invalid Argument");
			return;
		}

		if (!json->IsObject())
		{
			LOGI("Parsing JSON Failed: Not an object item");
			return;
		}

		JSONObject item = json->AsObject();

		CurrencyCode = JsonUtil::ParseString(item, L"currencyCode");
		Identifier = JsonUtil::ParseString(item, L"identifier");
		Name = JsonUtil::ParseString(item, L"name");
		ProductVersionToBundle = JsonUtil::ParseInt(item, L"productVersionToBundle");
		LocalPrice = JsonUtil::ParseFloat(item, L"localPrice");
		PriceInCents = JsonUtil::ParseInt(item, L"priceInCents");

		char buffer[1024];
		sprintf(buffer, "currencyCode=%s identifier=%s name=%s productVersionToBundle=%d localPrice=%.2f priceInCents=%d",
			CurrencyCode.c_str(),
			Identifier.c_str(),
			Name.c_str(),
			ProductVersionToBundle,
			LocalPrice,
			PriceInCents);
		LOGI(buffer);
	}
}