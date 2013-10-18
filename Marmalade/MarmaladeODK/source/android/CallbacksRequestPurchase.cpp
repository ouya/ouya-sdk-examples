#include "CallbacksRequestPurchase.h"
#include "CallbackSingleton.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include "IwDebug.h"

#include <stdio.h>

void CallbacksRequestPurchase::RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType)
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

void CallbacksRequestPurchase::RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	RegisterCallback(onSuccess, &m_onSuccess, S3E_ODK_CALLBACKS_REQUEST_PURCHASE_ON_SUCCESS);
	RegisterCallback(onFailure, &m_onFailure, S3E_ODK_CALLBACKS_REQUEST_PURCHASE_ON_FAILURE);
	RegisterCallback(onCancel, &m_onCancel, S3E_ODK_CALLBACKS_REQUEST_PURCHASE_ON_CANCEL);
}

void CallbacksRequestPurchase::OnSuccess(const OuyaSDK::Product& product)
{
	//char buffer[1024];
	//sprintf(buffer, "OnSuccess:  %s\r\n", product.Name.c_str());
	//LOGI(buffer);

	//std::string message = "CallbacksRequestPurchase::OnSuccess: ";
	//message.append(product.Identifier);

	//Application::m_ui.SetMessage(message);
}

void CallbacksRequestPurchase::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//LOGI(buffer);
	//Application::m_ui.SetMessage("CallbacksRequestPurchase::OnFailure");
}

void CallbacksRequestPurchase::OnCancel()
{
	//LOGI("OnCancel");
	//Application::m_ui.SetMessage("CallbacksRequestPurchase::OnCancel");
}