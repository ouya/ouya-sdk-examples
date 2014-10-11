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

#ifndef __APPLICATION_RECEIPT_H__
#define __APPLICATION_RECEIPT_H__

#include "ExtensionReceipt.h"

#include <string>

class ApplicationReceipt
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
	void Copy(const OuyaSDK::ExtensionReceipt& receipt);
};

#endif