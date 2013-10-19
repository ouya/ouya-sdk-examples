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

		void Init();

		void ParseJSON(JSONValue* json);
	};

	// for passing object from extension to application
	class ExtensionReceipt
	{
	public:
		char* Currency;
		char* Gamer;
		char* GeneratedDate;
		char* Identifier;
		char* Uuid;
		char* PurchaseDate;
		float LocalPrice;
		int PriceInCents;

		void Init();
	};
}

#endif