#ifndef __CALLBACKS_REQUEST_PURCHASE_H__
#define __CALLBACKS_REQUEST_PURCHASE_H__

#include <string>

class CallbacksRequestPurchase
{
public:

	void OnSuccess(std::string jsonData);

	void OnFailure(int errorCode, std::string errorMessage);

	void OnCancel();
};

#endif