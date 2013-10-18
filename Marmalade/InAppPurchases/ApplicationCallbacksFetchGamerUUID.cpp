#include "Application.h"
#include "ApplicationCallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"

#include "IwDebug.h"

#include <stdio.h>

void FetchGamerUuidOnSuccess(s3eFetchGamerUuidSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void FetchGamerUuidOnSuccess(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksFetchGamerUUID->OnSuccess(event->m_gamerUUID);
	}
	else
	{
		Application::m_ui.m_callbacksFetchGamerUUID->OnFailure(-1, "Success event is null");
	}
}

void FetchGamerUuidOnFailure(s3eFetchGamerUuidFailureEvent* event)
{
	//IwTrace(DEFAULT, ("void FetchGamerUuidOnFailure(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksFetchGamerUUID->OnFailure(event->m_errorCode, event->m_errorMessage);
	}
	else
	{
		Application::m_ui.m_callbacksFetchGamerUUID->OnFailure(-1, "Failure event is null");
	}
}

void FetchGamerUuidOnCancel(s3eFetchGamerUuidSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void FetchGamerUuidOnCancel(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksFetchGamerUUID->OnCancel();
	}
	else
	{
		Application::m_ui.m_callbacksFetchGamerUUID->OnFailure(-1, "Cancel event is null");
	}
}

s3eCallback ApplicationCallbacksFetchGamerUUID::GetSuccessEvent()
{
	return (s3eCallback)FetchGamerUuidOnSuccess;
}

s3eCallback ApplicationCallbacksFetchGamerUUID::GetFailureEvent()
{
	return (s3eCallback)FetchGamerUuidOnFailure;
}

s3eCallback ApplicationCallbacksFetchGamerUUID::GetCancelEvent()
{
	return (s3eCallback)FetchGamerUuidOnCancel;
}

void ApplicationCallbacksFetchGamerUUID::OnSuccess(const std::string& gamerUUID)
{
	//IwTrace(DEFAULT, ("OnSuccess"));

	//char buffer[256];
	//sprintf(buffer, "OnSuccess:  %s", gamerUUID.c_str());
	//IwTrace(DEFAULT, (buffer));

	Application::m_ui.SetMessage("ApplicationCallbacksFetchGamerUUID::OnSuccess");
	Application::m_ui.SetGamerUUID(gamerUUID);
}

void ApplicationCallbacksFetchGamerUUID::OnFailure(int errorCode, const std::string& errorMessage)
{
	//IwTrace(DEFAULT, ("OnFailure"));

	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//IwTrace(DEFAULT, (buffer));

	std::string msg = "ApplicationCallbacksFetchGamerUUID::OnFailure";
	msg.append(" errorMessage=");
	msg.append(errorMessage);
	Application::m_ui.SetMessage(msg);
}

void ApplicationCallbacksFetchGamerUUID::OnCancel()
{
	//IwTrace(DEFAULT, ("OnCancel"));

	Application::m_ui.SetMessage("ApplicationCallbacksFetchGamerUUID::OnCancel");
}