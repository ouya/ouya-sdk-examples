#ifndef __PRODUCT_H__
#define __PRODUCT_H__

#include "JSON.h"

namespace OuyaSDK
{
	class Product
	{
	public:
		std::string CurrencyCode;
		std::string Identifier;
		std::string Name;
		int ProductVersionToBundle;
		float LocalPrice;
		int PriceInCents;

		Product();

		void ParseJSON(JSONValue* json);
	};
}

#endif