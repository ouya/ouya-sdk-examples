#ifndef __CALLBACKS_REQUEST_PURCHASE_H__
#define __CALLBACKS_REQUEST_PURCHASE_H__

#include "Product.h"

#include "s3eTypes.h"

#include <string>

typedef struct s3eRequestPurchaseSuccessEvent
{
	OuyaSDK::Product m_product;
} s3eRequestPurchaseSuccessEvent;

typedef struct s3eRequestPurchaseFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eRequestPurchaseFailureEvent;

typedef struct s3eRequestPurchaseCancelEvent
{
} s3eRequestPurchaseCancelEvent;

class CallbacksRequestPurchase
{
public:

	void RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel);

	void OnSuccess(const OuyaSDK::Product& product);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();

private:
	//extension callbacks
	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	s3eCallback m_onCancel;

	//event data sent to callbacks can't be temporary
	s3eRequestPurchaseSuccessEvent m_dataRequestPurchaseSuccessEvent;
	s3eRequestPurchaseFailureEvent m_dataRequestPurchaseFailureEvent;
	s3eRequestPurchaseCancelEvent m_dataRequestPurchaseCancelEvent;

	void RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType);
};

#endif