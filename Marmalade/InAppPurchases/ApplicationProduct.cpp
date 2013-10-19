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