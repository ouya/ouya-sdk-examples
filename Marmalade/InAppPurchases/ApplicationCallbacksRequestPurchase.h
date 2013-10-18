#ifndef __APPLICATTON_CALLBACKS_REQUEST_PURCHASE_H__
#define __APPLICATTON_CALLBACKS_REQUEST_PURCHASE_H__

#include "Product.h"

#include <string>

class ApplicationCallbacksRequestPurchase
{
public:

	s3eCallback GetSuccessEvent();

	s3eCallback GetFailureEvent();

	s3eCallback GetCancelEvent();

	void OnSuccess(const OuyaSDK::Product& product);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif