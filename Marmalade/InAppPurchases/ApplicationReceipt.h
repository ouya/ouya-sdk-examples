#ifndef __APPLICATION_RECEIPT_H__
#define __APPLICATION_RECEIPT_H__

#include "Receipt.h"

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