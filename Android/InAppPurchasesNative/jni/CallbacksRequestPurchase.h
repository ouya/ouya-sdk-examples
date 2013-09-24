#ifndef __CALLBACKS_REQUEST_PURCHASE_H__
#define __CALLBACKS_REQUEST_PURCHASE_H__

class CallbacksRequestPurchase
{
public:

	void OnSuccess();

	void OnFailure(int errorCode, char* errorMessage);

	void OnCancel();
};

#endif