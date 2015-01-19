/*
* Copyright (C) 2012-2015 OUYA, Inc.
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
#ifndef ECLIPSE
#include "LaunchPrivatePCH.h"
#endif

#include "OuyaSDK_Product.h"
#include "OuyaSDK_JSONObject.h"

#include <string>


namespace OuyaSDK
{
	void Product::Init()
	{
		CurrencyCode = "";
		Identifier = "";
		Name = "";
		ProductVersionToBundle = 0;
		LocalPrice = 0;
		PriceInCents = 0;
	}

#if defined(ANDROID)
	void Product::ParseJSON(const org_json_JSONObject::JSONObject& jsonObject)
	{
		Init();

		std::string field;

		field = "currencyCode";
		if (jsonObject.has(field))
		{
			CurrencyCode = jsonObject.getString(field);
		}

		field = "identifier";
		if (jsonObject.has(field))
		{
			Identifier = jsonObject.getString(field);
		}

		field = "name";
		if (jsonObject.has(field))
		{
			Name = jsonObject.getString(field);
		}
		
		field = "productVersionToBundle";
		if (jsonObject.has(field))
		{
			ProductVersionToBundle = jsonObject.getInt(field);
		}
		
		field = "localPrice";
		if (jsonObject.has(field))
		{
			LocalPrice = jsonObject.getDouble(field);
		}
		
		field = "priceInCents";
		if (jsonObject.has(field))
		{
			PriceInCents = jsonObject.getInt(field);
		}
	}
#endif
}
