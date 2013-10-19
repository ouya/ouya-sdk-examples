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