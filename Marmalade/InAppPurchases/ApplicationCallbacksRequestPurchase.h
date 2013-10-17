#ifndef __APPLICATTON_CALLBACKS_REQUEST_PURCHASE_H__
#define __APPLICATTON_CALLBACKS_REQUEST_PURCHASE_H__

#include "CallbacksRequestPurchase.h"

class ApplicationCallbacksRequestPurchase : public CallbacksRequestPurchase
{
public:

	virtual void OnSuccess(const OuyaSDK::Product& product);

	virtual void OnFailure(int errorCode, const std::string& errorMessage);

	virtual void OnCancel();
};

#endif