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

#include "ExtensionReceipt.h"
#include "JSONObject.h"

#include <string>


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

	void ExtensionReceipt::Copy(const Receipt& rhs)
	{
		Currency = rhs.Currency.c_str();
		Gamer = rhs.Gamer.c_str();
		GeneratedDate = rhs.GeneratedDate.c_str();
		Identifier = rhs.Identifier.c_str();
		Uuid = rhs.Uuid.c_str();
		PurchaseDate = rhs.PurchaseDate.c_str();
		LocalPrice = rhs.LocalPrice;
		PriceInCents = rhs.PriceInCents;
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

	void Receipt::Copy(const ExtensionReceipt& rhs)
	{
		Init();
		if (rhs.Currency)
		{
			Currency = rhs.Currency;
		}
		if (rhs.Gamer)
		{
			Gamer = rhs.Gamer;
		}
		if (rhs.GeneratedDate)
		{
			GeneratedDate = rhs.GeneratedDate;
		}
		if (rhs.Identifier)
		{
			Identifier = rhs.Identifier;
		}
		if (rhs.Uuid)
		{
			Uuid = rhs.Uuid;
		}
		if (rhs.PurchaseDate)
		{
			PurchaseDate = rhs.PurchaseDate;
		}
		LocalPrice = rhs.LocalPrice;
		PriceInCents = rhs.PriceInCents;
	}

#if defined(ANDROID)
	void Receipt::ParseJSON(const org_json_JSONObject::JSONObject& jsonObject)
	{
		std::string field;

		field = "currency";
		if (jsonObject.has(field))
		{
			Currency = jsonObject.getString(field);
		}
		
		field = "gamer";
		if (jsonObject.has(field))
		{
			Gamer = jsonObject.getString(field);
		}
		
		field = "generatedDate";
		if (jsonObject.has(field))
		{
			GeneratedDate = jsonObject.getString(field);
		}
		
		field = "identifier";
		if (jsonObject.has(field))
		{
			Identifier = jsonObject.getString(field);
		}
		
		field = "uuid";
		if (jsonObject.has(field))
		{
			Uuid = jsonObject.getString(field);
		}
		
		field = "purchaseDate";
		if (jsonObject.has(field))
		{
			PurchaseDate = jsonObject.getString(field);
		}
		
		field = "localPrice";
		if (jsonObject.has(field))
		{
			LocalPrice = (float)jsonObject.getDouble(field);
		}
		
		field = "priceInCents";
		if (jsonObject.has(field))
		{
			PriceInCents = jsonObject.getInt(field);
		}
	}
#endif
}