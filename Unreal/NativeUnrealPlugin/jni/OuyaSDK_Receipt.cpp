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

#include "OuyaSDK_Receipt.h"
#include "OuyaSDK_JSONObject.h"

#include <string>


namespace OuyaSDK
{
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
