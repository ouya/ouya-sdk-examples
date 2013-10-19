#ifndef __APPLICATION_PRODUCT_H__
#define __APPLICATION_PRODUCT_H__

#include "Product.h"

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