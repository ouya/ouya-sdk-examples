#include "JSON.h"
#include "JSONValue.h"
#include "JsonUtil.h"
#include "Receipt.h"

#include <android/log.h>
#include <stdio.h>

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
namespace OuyaSDK
{
	Receipt::Receipt()
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

	Receipt::Receipt(const Receipt& copy)
	{
		Currency = copy.Currency;
		Gamer = copy.Gamer;
		GeneratedDate = copy.GeneratedDate;
		Identifier = copy.Identifier;
		Uuid = copy.Uuid;
		PurchaseDate = copy.PurchaseDate;
		LocalPrice = copy.LocalPrice;
		PriceInCents = copy.PriceInCents;
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