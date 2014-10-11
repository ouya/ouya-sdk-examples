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

#ifndef __APPLICATION_PRODUCT_H__
#define __APPLICATION_PRODUCT_H__

#include "ExtensionProduct.h"

#include <string>

class ApplicationProduct
{
public:
	std::string CurrencyCode;
	std::string Identifier;
	std::string Name;
	int ProductVersionToBundle;
	float LocalPrice;
	int PriceInCents;

	void Init();
	void Copy(const OuyaSDK::ExtensionProduct& product);
};

#endif