#include "CallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include "IwDebug.h"

#include <stdio.h>

void CallbacksFetchGamerUUID::RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType)
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

void CallbacksFetchGamerUUID::RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	RegisterCallback(onSuccess, &m_onSuccess, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);
	RegisterCallback(onFailure, &m_onFailure, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_FAILURE);
	RegisterCallback(onCancel, &m_onCancel, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_CANCEL);
}

void CallbacksFetchGamerUUID::OnSuccess(const std::string& gamerUUID)
{
	//char buffer[256];
	//sprintf(buffer, "OnSuccess:  %s", gamerUUID.c_str());
	//IwTrace(ODK, (buffer));

	s3eFetchGamerUuidSuccessEvent event;
	sprintf(event.m_gamerUUID, "%s", gamerUUID.c_str());

	//std::string msg = "Sending event.m_gamerUUID=";
	//msg.append(event.m_gamerUUID);
	//IwTrace(ODK, (msg.c_str()));

	m_dataFetchGamerUuidSuccessEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS, &m_dataFetchGamerUuidSuccessEvent);
}

void CallbacksFetchGamerUUID::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(ODK, (buffer));

	s3eFetchGamerUuidFailureEvent event;
	event.m_errorCode = errorCode;
	sprintf(event.m_errorMessage, "%s", errorMessage.c_str());

	m_dataFetchGamerUuidFailureEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_FAILURE, &m_dataFetchGamerUuidFailureEvent);
}

void CallbacksFetchGamerUUID::OnCancel()
{
	IwTrace(ODK, ("OnCancel"));

	s3eFetchGamerUuidCancelEvent event;

	m_dataFetchGamerUuidCancelEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_CANCEL, &m_dataFetchGamerUuidCancelEvent);
}