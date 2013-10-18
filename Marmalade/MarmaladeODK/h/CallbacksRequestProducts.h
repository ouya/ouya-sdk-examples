#ifndef __CALLBACKS_REQUEST_PRODUCTS_H__
#define __CALLBACKS_REQUEST_PRODUCTS_H__

#include "Product.h"

#include <string>

class CallbacksRequestProducts
{
public:

	void OnSuccess(const std::vector<OuyaSDK::Product>& products);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif