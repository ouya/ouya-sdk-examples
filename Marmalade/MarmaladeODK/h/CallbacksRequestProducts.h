#ifndef __CALLBACKS_REQUEST_PRODUCTS_H__
#define __CALLBACKS_REQUEST_PRODUCTS_H__

#include "Product.h"

#include <string>

class CallbacksRequestProducts
{
public:

	virtual void OnSuccess(const std::vector<OuyaSDK::Product>& products) = 0;

	virtual void OnFailure(int errorCode, const std::string& errorMessage) = 0;

	virtual void OnCancel() = 0;
};

#endif