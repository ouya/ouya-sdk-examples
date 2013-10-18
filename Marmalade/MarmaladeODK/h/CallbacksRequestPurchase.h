#ifndef __CALLBACKS_REQUEST_PURCHASE_H__
#define __CALLBACKS_REQUEST_PURCHASE_H__

#include "Product.h"

#include <string>

class CallbacksRequestPurchase
{
public:

	void OnSuccess(const OuyaSDK::Product& product);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif