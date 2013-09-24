#ifndef __CALLBACKS_REQUEST_RECEIPTS_H__
#define __CALLBACKS_REQUEST_RECEIPTS_H__

class CallbacksRequestReceipts
{
public:

	void OnSuccess();

	void OnFailure(int errorCode, char* errorMessage);

	void OnCancel();
};

#endif