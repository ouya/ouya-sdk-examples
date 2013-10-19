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

		void Init();

		void ParseJSON(JSONValue* json);
	};

	// for passing product from extension to application
	class ExtensionProduct
	{
	public:
		char* CurrencyCode;
		char* Identifier;
		char* Name;
		int ProductVersionToBundle;
		float LocalPrice;
		int PriceInCents;

		void Init();
	};
}

#endif