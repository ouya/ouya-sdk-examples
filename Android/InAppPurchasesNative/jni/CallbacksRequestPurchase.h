#ifndef __CALLBACKS_REQUEST_PURCHASE_H__
#define __CALLBACKS_REQUEST_PURCHASE_H__

#include <string>

class CallbacksRequestPurchase
{
public:

	void OnSuccess(const std::string& jsonData);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif