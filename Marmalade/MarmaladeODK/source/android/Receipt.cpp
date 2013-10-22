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
#include "Receipt.h"

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
	void ExtensionReceipt::Init()
	{
		Currency = NULL;
		Gamer = NULL;
		GeneratedDate = NULL;
		Identifier = NULL;
		Uuid = NULL;
		PurchaseDate = NULL;
		LocalPrice = 0;
		PriceInCents = 0;
	}

	void Receipt::Init()
	{
		Currency = "";
		Gamer = "";
		GeneratedDate = "";
		Identifier = "";
		Uuid = "";
		PurchaseDate = "";
		LocalPrice = 0;
		PriceInCents = 0;
	}

	void Receipt::ParseJSON(JSONValue* json)
	{
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

		Currency = JsonUtil::ParseString(item, L"currency");
		Gamer = JsonUtil::ParseString(item, L"gamer");
		GeneratedDate = JsonUtil::ParseString(item, L"generatedDate");
		Identifier = JsonUtil::ParseString(item, L"identifier");
		Uuid = JsonUtil::ParseString(item, L"uuid");
		PurchaseDate = JsonUtil::ParseString(item, L"generatedDate");
		LocalPrice = JsonUtil::ParseFloat(item, L"localPrice");
		PriceInCents = JsonUtil::ParseInt(item, L"priceInCents");

		char buffer[1024];
		sprintf(buffer, "currency=%s gamer=%s generatedDate=%s identifier=%s uuid=%s purchaseDate=%s localPrice=%.2f priceInCents=%d",
			Currency.c_str(),
			Gamer.c_str(),
			GeneratedDate.c_str(),
			Identifier.c_str(),
			Uuid.c_str(),
			PurchaseDate.c_str(),
			LocalPrice,
			PriceInCents);
		LOGI(buffer);
	}
}