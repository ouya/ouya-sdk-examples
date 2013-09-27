#include "JSON.h"
#include "JSONValue.h"
#include "JsonUtil.h"
#include "Product.h"

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

void Product::ParseJSON(JSONValue* json)
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

	CurrencyCode = JsonUtil::ParseString(item, L"currencyCode");
	Identifier = JsonUtil::ParseString(item, L"identifier");
	Name = JsonUtil::ParseString(item, L"name");
	ProductVersionToBundle = JsonUtil::ParseInt(item, L"productVersionToBundle");
	LocalPrice = JsonUtil::ParseFloat(item, L"localPrice");
	PriceInCents = JsonUtil::ParseInt(item, L"priceInCents");

	char buffer[1024];
	sprintf(buffer, "currencyCode=%s identifier=%s productVersionToBundle=%d localPrice=%.2f priceInCents=%d",
		CurrencyCode.c_str(),
		Identifier.c_str(),
		Name.c_str(),
		ProductVersionToBundle,
		LocalPrice,
		PriceInCents);
	LOGI(buffer);
}