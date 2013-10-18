#include "CallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include "IwDebug.h"

#include <android/log.h>
#include <stdio.h>

void CallbacksFetchGamerUUID::RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	if (m_onSuccess)
	{
		IwTrace(ODK, ("Unregistering Callback: s3eEdkCallbacksRegister(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);"));

		s3eEdkCallbacksUnRegister(
				S3E_EXT_ODK_HASH,
				S3E_ODK_CALLBACKS_MAX,
				S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS,
				m_onSuccess,
				NULL);
		m_onSuccess = NULL;
	}

	m_onSuccess = onSuccess;
	m_onFailure = onFailure;
	m_onCancel = onCancel;

	IwTrace(ODK, ("Registering Callback: s3eEdkCallbacksRegister(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);"));

	s3eEdkCallbacksRegister(
			S3E_EXT_ODK_HASH,
			S3E_ODK_CALLBACKS_MAX,
			S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS,
			m_onSuccess,
			NULL,
			S3E_FALSE);
}

void CallbacksFetchGamerUUID::OnSuccess(const std::string& gamerUUID)
{
	char buffer[256];
	sprintf(buffer, "OnSuccess:  %s", gamerUUID.c_str());
	IwTrace(ODK, (buffer));

	IwTrace(ODK, ("Invoking Callback: s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);"));

	s3eFetchGamerUuidSuccessEvent event;
	sprintf(event.m_gamerUUID, "%s", gamerUUID.c_str());

	std::string msg = "Sending event.m_gamerUUID=";
	msg.append(event.m_gamerUUID);
	IwTrace(ODK, (msg.c_str()));

	m_dataFetchGamerUuidSuccessEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS, &m_dataFetchGamerUuidSuccessEvent);
}

void CallbacksFetchGamerUUID::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(ODK, (buffer));

	//Application::m_ui.SetMessage("CallbacksFetchGamerUUID::OnFailure");
}

void CallbacksFetchGamerUUID::OnCancel()
{
	IwTrace(ODK, ("OnCancel"));

	//Application::m_ui.SetMessage("CallbacksFetchGamerUUID::OnCancel");
}