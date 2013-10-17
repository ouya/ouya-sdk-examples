#ifndef __APPLICATTON_CALLBACKS_REQUEST_PRODUCTS_H__
#define __APPLICATTON_CALLBACKS_REQUEST_PRODUCTS_H__

#include "CallbacksRequestProducts.h"
#include "Product.h"

class ApplicationCallbacksRequestProducts : public CallbacksRequestProducts
{
public:

	virtual void OnSuccess(const std::vector<OuyaSDK::Product>& products);

	virtual void OnFailure(int errorCode, const std::string& errorMessage);

	virtual void OnCancel();
};

#endif