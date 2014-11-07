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

#include "ExtensionProduct.h"
#include "JSONObject.h"

#include <string>


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

	void ExtensionProduct::Copy(const Product& rhs)
	{
		CurrencyCode = rhs.CurrencyCode.c_str();
		Identifier = rhs.Identifier.c_str();
		Name = rhs.Name.c_str();
		ProductVersionToBundle = rhs.ProductVersionToBundle;
		LocalPrice = rhs.LocalPrice;
		PriceInCents = rhs.PriceInCents;
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

	void Product::Copy(const ExtensionProduct& rhs)
	{
		Init();
		if (rhs.CurrencyCode)
		{
			CurrencyCode = rhs.CurrencyCode;
		}
		if (rhs.Identifier)
		{
			Identifier = rhs.Identifier;
		}
		if (rhs.Name)
		{
			Name = rhs.Name;
		}
		ProductVersionToBundle = rhs.ProductVersionToBundle;
		LocalPrice = rhs.LocalPrice;
		PriceInCents = rhs.PriceInCents;
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