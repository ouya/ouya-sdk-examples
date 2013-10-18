#include "CallbacksRequestReceipts.h"
#include "CallbackSingleton.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include "IwDebug.h"

#include <stdio.h>

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

void CallbacksRequestReceipts::OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts)
{
	//LOGI("OnSuccess);
	//Application::m_ui.SetMessage("CallbacksRequestReceipts::OnSuccess");

	//Application::m_ui.ClearReceipts();

	//for (int index = 0; index < receipts.size(); ++index)
	//{
	//	Application::m_ui.AddReceipt(receipts[index]);
	//}
}

void CallbacksRequestReceipts::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//LOGI(buffer);
	//Application::m_ui.SetMessage("CallbacksRequestReceipts::OnSuccess");
}

void CallbacksRequestReceipts::OnCancel()
{
	//LOGI("OnCancel");
	//Application::m_ui.SetMessage("CallbacksRequestReceipts::OnSuccess");
}