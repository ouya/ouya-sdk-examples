#ifndef __CALLBACKS_REQUEST_RECEIPTS_H__
#define __CALLBACKS_REQUEST_RECEIPTS_H__

#include "Receipt.h"

#include <string>

class CallbacksRequestReceipts
{
public:

	virtual void OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts) = 0;

	virtual void OnFailure(int errorCode, const std::string& errorMessage) = 0;

	virtual void OnCancel() = 0;
};

#endif