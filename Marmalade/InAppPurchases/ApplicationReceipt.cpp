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

#include "ApplicationReceipt.h"

void ApplicationReceipt::ApplicationReceipt::Init()
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

void ApplicationReceipt::Copy(const OuyaSDK::ExtensionReceipt& receipt)
{
	if (receipt.Currency)
	{
		Currency = receipt.Currency;
	}
	else
	{
		Currency = "";
	}
	if (receipt.Gamer)
	{
		Gamer = receipt.Gamer;
	}
	else
	{
		Gamer = "";
	}
	if (receipt.GeneratedDate)
	{
		GeneratedDate = receipt.GeneratedDate;
	}
	else
	{
		GeneratedDate = "";
	}
	if (receipt.Identifier)
	{
		Identifier = receipt.Identifier;
	}
	else
	{
		Identifier = "";
	}
	if (receipt.Uuid)
	{
		Uuid = receipt.Uuid;
	}
	else
	{
		Uuid = "";
	}
	if (receipt.PurchaseDate)
	{
		PurchaseDate = receipt.PurchaseDate;
	}
	else
	{
		PurchaseDate = "";
	}
	LocalPrice = receipt.LocalPrice;
	PriceInCents = receipt.PriceInCents;
}