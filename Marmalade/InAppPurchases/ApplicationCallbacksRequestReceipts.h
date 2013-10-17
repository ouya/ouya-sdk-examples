#ifndef __APPLICATTON_CALLBACKS_REQUEST_RECEIPTS_H__
#define __APPLICATTON_CALLBACKS_REQUEST_RECEIPTS_H__

#include "CallbacksRequestReceipts.h"
#include "Receipt.h"

class ApplicationCallbacksRequestReceipts : public CallbacksRequestReceipts
{
public:

	virtual void OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts);

	virtual void OnFailure(int errorCode, const std::string& errorMessage);

	virtual void OnCancel();
};

#endif