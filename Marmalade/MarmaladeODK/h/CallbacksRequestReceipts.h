#ifndef __CALLBACKS_REQUEST_RECEIPTS_H__
#define __CALLBACKS_REQUEST_RECEIPTS_H__

#include "Receipt.h"

#include "s3eTypes.h"

#include <string>

typedef struct s3eRequestReceiptsSuccessEvent
{
	OuyaSDK::ExtensionReceipt* m_receipts;
	int m_receiptsLength;
} s3eRequestReceiptsSuccessEvent;

typedef struct s3eRequestReceiptsFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eRequestReceiptsFailureEvent;

typedef struct s3eRequestReceiptsCancelEvent
{
} s3eRequestReceiptsCancelEvent;

class CallbacksRequestReceipts
{
public:

	void RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel);

	void OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();

private:
	//extension callbacks
	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	s3eCallback m_onCancel;

	//event data sent to callbacks can't be temporary
	s3eRequestReceiptsSuccessEvent m_dataRequestReceiptsSuccessEvent;
	s3eRequestReceiptsFailureEvent m_dataRequestReceiptsFailureEvent;
	s3eRequestReceiptsCancelEvent m_dataRequestReceiptsCancelEvent;

	void RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType);

	//cache the receipt array
	static OuyaSDK::ExtensionReceipt* m_receipts;
};

#endif