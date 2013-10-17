#ifndef __CALLBACKS_REQUEST_PURCHASE_H__
#define __CALLBACKS_REQUEST_PURCHASE_H__

#include "Product.h"

#include <string>

class CallbacksRequestPurchase
{
public:

	virtual void OnSuccess(const OuyaSDK::Product& product) = 0;

	virtual void OnFailure(int errorCode, const std::string& errorMessage) = 0;

	virtual void OnCancel() = 0;
};

#endif