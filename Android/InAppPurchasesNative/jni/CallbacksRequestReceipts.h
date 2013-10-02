#ifndef __CALLBACKS_REQUEST_RECEIPTS_H__
#define __CALLBACKS_REQUEST_RECEIPTS_H__

#include <string>

class CallbacksRequestReceipts
{
public:

	void OnSuccess(const std::string& jsonData);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif