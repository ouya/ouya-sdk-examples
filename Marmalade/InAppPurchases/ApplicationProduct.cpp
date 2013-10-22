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

#include "ApplicationProduct.h"

void ApplicationProduct::ApplicationProduct::Init()
{
	CurrencyCode = "";
	Identifier = "";
	Name = "";
	ProductVersionToBundle = 0;
	LocalPrice = 0;
	PriceInCents = 0;
}

void ApplicationProduct::Copy(const OuyaSDK::ExtensionProduct& product)
{
	if (product.CurrencyCode)
	{
		CurrencyCode = product.CurrencyCode;
	}
	else
	{
		CurrencyCode = "";
	}
	if (product.Identifier)
	{
		Identifier = product.Identifier;
	}
	else
	{
		Identifier = "";
	}
	if (product.Name)
	{
		Name = product.Name;
	}
	else
	{
		Name = "";
	}
	ProductVersionToBundle = product.ProductVersionToBundle;
	LocalPrice = product.LocalPrice;
	PriceInCents = product.PriceInCents;
}