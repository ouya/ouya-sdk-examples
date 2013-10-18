#ifndef __CALLBACKS_REQUEST_RECEIPTS_H__
#define __CALLBACKS_REQUEST_RECEIPTS_H__

#include "Receipt.h"

#include <string>

class CallbacksRequestReceipts
{
public:

	void OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif