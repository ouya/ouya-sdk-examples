#include "Application.h"
#include "ApplicationCallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"

#include "IwDebug.h"

#include <stdio.h>

void ApplicationCallbacksFetchGamerUUID::OnSuccess(const std::string& gamerUUID)
{
	IwTrace(DEFAULT, ("OnSuccess"));

	//char buffer[256];
	//sprintf(buffer, "OnSuccess:  %s", gamerUUID.c_str());
	//IwTrace(DEFAULT, ("OnSuccess"));

	Application::m_ui.SetMessage("ApplicationCallbacksFetchGamerUUID::OnSuccess");
	Application::m_ui.SetGamerUUID(gamerUUID);
}

void ApplicationCallbacksFetchGamerUUID::OnFailure(int errorCode, const std::string& errorMessage)
{
	IwTrace(DEFAULT, ("OnFailure"));

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
	IwTrace(DEFAULT, ("OnCancel"));

	//Application::m_ui.SetMessage("ApplicationCallbacksFetchGamerUUID::OnCancel");
}