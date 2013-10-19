#include "CallbacksRequestReceipts.h"
#include "CallbackSingleton.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include "IwDebug.h"

#include <stdio.h>

OuyaSDK::ExtensionReceipt* CallbacksRequestReceipts::m_receipts = NULL;

void CallbacksRequestReceipts::RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType)
{
	if (*savedCallback)
	{
		//IwTrace(ODK, ("Unregistering Callback"));

		s3eEdkCallbacksUnRegister(
				S3E_EXT_ODK_HASH,
				S3E_ODK_CALLBACKS_MAX,
				callbackType,
				*savedCallback,
				NULL);
		*savedCallback = NULL;
	}

	*savedCallback = callback;

	//IwTrace(ODK, ("Registering Callback"));

	s3eEdkCallbacksRegister(
			S3E_EXT_ODK_HASH,
			S3E_ODK_CALLBACKS_MAX,
			callbackType,
			*savedCallback,
			NULL,
			S3E_FALSE);
}

void CallbacksRequestReceipts::RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	RegisterCallback(onSuccess, &m_onSuccess, S3E_ODK_CALLBACKS_REQUEST_RECEIPTS_ON_SUCCESS);
	RegisterCallback(onFailure, &m_onFailure, S3E_ODK_CALLBACKS_REQUEST_RECEIPTS_ON_FAILURE);
	RegisterCallback(onCancel, &m_onCancel, S3E_ODK_CALLBACKS_REQUEST_RECEIPTS_ON_CANCEL);
}

void CopyStringReceipt(std::string source, char** destination)
{
	const char* str = source.c_str();
	*destination = new char[strlen(str)];
	sprintf(*destination, "%s", str);
}

void CallbacksRequestReceipts::OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts)
{
	IwTrace(ODK, ("OnSuccess"));

	if (receipts.size() > 0)
	{
		IwTrace(ODK, ("Allocating receipts"));
		m_receipts = new OuyaSDK::ExtensionReceipt[receipts.size()];
		IwTrace(ODK, ("Copying receipts"));
		for (unsigned int index = 0; index < receipts.size(); ++index)
		{
			OuyaSDK::Receipt receipt = receipts[index];
			OuyaSDK::ExtensionReceipt eReceipt;
			eReceipt.Init();
			CopyStringReceipt(receipt.Currency, &eReceipt.Currency);
			CopyStringReceipt(receipt.Gamer, &eReceipt.Gamer);
			CopyStringReceipt(receipt.GeneratedDate, &eReceipt.GeneratedDate);
			CopyStringReceipt(receipt.Identifier, &eReceipt.Identifier);
			CopyStringReceipt(receipt.Uuid, &eReceipt.Uuid);
			CopyStringReceipt(receipt.PurchaseDate, &eReceipt.PurchaseDate);
			eReceipt.LocalPrice = receipt.LocalPrice;
			eReceipt.PriceInCents = receipt.PriceInCents;
			m_receipts[index] = eReceipt;
		}
	}
	else
	{
		IwTrace(ODK, ("No products to copy"));
	}

	s3eRequestReceiptsSuccessEvent event;
	event.m_receipts = m_receipts;
	event.m_receiptsLength = receipts.size();

	IwTrace(ODK, ("Invoking callback"));

	m_dataRequestReceiptsSuccessEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_REQUEST_RECEIPTS_ON_SUCCESS, &m_dataRequestReceiptsSuccessEvent);
}

void CallbacksRequestReceipts::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(ODK, (buffer));

	s3eRequestReceiptsFailureEvent event;
	event.m_errorCode = errorCode;
	sprintf(event.m_errorMessage, "%s", errorMessage.c_str());

	m_dataRequestReceiptsFailureEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_REQUEST_RECEIPTS_ON_FAILURE, &m_dataRequestReceiptsFailureEvent);
}

void CallbacksRequestReceipts::OnCancel()
{
	IwTrace(ODK, ("OnCancel"));

	s3eRequestReceiptsCancelEvent event;

	m_dataRequestReceiptsCancelEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_REQUEST_RECEIPTS_ON_CANCEL, &m_dataRequestReceiptsCancelEvent);
}