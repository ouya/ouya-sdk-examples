#ifndef __APPLICATTON_CALLBACKS_REQUEST_PURCHASE_H__
#define __APPLICATTON_CALLBACKS_REQUEST_PURCHASE_H__

#include "Product.h"

#include <string>

class ApplicationCallbacksRequestPurchase
{
public:

	void OnSuccess(const OuyaSDK::Product& product);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif