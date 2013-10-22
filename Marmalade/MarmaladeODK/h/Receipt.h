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

#ifndef __RECEIPT_H__
#define __RECEIPT_H__

#include "JSON.h"

namespace OuyaSDK
{
	class Receipt
	{
	public:
		std::string Currency;
		std::string Gamer;
		std::string GeneratedDate;
		std::string Identifier;
		std::string Uuid;
		std::string PurchaseDate;
		float LocalPrice;
		int PriceInCents;

		void Init();

		void ParseJSON(JSONValue* json);
	};

	// for passing object from extension to application
	class ExtensionReceipt
	{
	public:
		char* Currency;
		char* Gamer;
		char* GeneratedDate;
		char* Identifier;
		char* Uuid;
		char* PurchaseDate;
		float LocalPrice;
		int PriceInCents;

		void Init();
	};
}

#endif