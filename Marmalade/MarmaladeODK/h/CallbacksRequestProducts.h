#ifndef __CALLBACKS_REQUEST_PRODUCTS_H__
#define __CALLBACKS_REQUEST_PRODUCTS_H__

#include "Product.h"

#include "s3eTypes.h"

#include <string>

typedef struct s3eRequestProductsSuccessEvent
{
	OuyaSDK::ExtensionProduct* m_products;
	int m_productLength;
} s3eRequestProductsSuccessEvent;

typedef struct s3eRequestProductsFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eRequestProductsFailureEvent;

typedef struct s3eRequestProductsCancelEvent
{
} s3eRequestProductsCancelEvent;

class CallbacksRequestProducts
{
public:

	void RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel);

	void OnSuccess(const std::vector<OuyaSDK::Product>& products);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();

private:
	//extension callbacks
	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	s3eCallback m_onCancel;

	//event data sent to callbacks can't be temporary
	s3eRequestProductsSuccessEvent m_dataRequestProductsSuccessEvent;
	s3eRequestProductsFailureEvent m_dataRequestProductsFailureEvent;
	s3eRequestProductsCancelEvent m_dataRequestProductsCancelEvent;

	void RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType);

	//cache the product array
	static OuyaSDK::ExtensionProduct* m_products;
};

#endif