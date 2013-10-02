#ifndef __RECEIPT_H__
#define __RECEIPT_H__

#include "JSON.h"

namespace OuyaSDK
{
	class Receipt
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

		Receipt();

		Receipt(const Receipt& copy);

		void ParseJSON(JSONValue* json);
	};
}

#endif