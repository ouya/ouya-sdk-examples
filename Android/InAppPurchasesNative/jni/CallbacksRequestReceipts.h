#ifndef __CALLBACKS_REQUEST_RECEIPTS_H__
#define __CALLBACKS_REQUEST_RECEIPTS_H__

class CallbacksRequestReceipts
{
public:

	void OnSuccess(const char* jsonData);

	void OnFailure(int errorCode, char* errorMessage);

	void OnCancel();
};

#endif