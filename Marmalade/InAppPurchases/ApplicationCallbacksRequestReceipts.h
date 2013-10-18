#ifndef __APPLICATTON_CALLBACKS_REQUEST_RECEIPTS_H__
#define __APPLICATTON_CALLBACKS_REQUEST_RECEIPTS_H__

#include "Receipt.h"

#include <string>
#include <vector>

class ApplicationCallbacksRequestReceipts
{
public:

	s3eCallback GetSuccessEvent();

	s3eCallback GetFailureEvent();

	s3eCallback GetCancelEvent();

	void OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif