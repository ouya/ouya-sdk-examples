/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#include "Application.h"
#include "ApplicationCallbacksSetDeveloperId.h"
#include "CallbacksSetDeveloperId.h"

#include "IwDebug.h"

#include <stdio.h>

#define LOG_TAG "ApplicationCallbacksSetDeveloperId"

void SetDeveloperIdOnSuccess(s3eSetDeveloperIdSuccessEvent* event)
{
	//IwTrace(LOG_TAG, ("void SetDeveloperIdOnSuccess(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksSetDeveloperId->OnSuccess();
	}
	else
	{
		Application::m_ui.m_callbacksSetDeveloperId->OnFailure(-1, "Success event is null");
	}
}

void SetDeveloperIdOnFailure(s3eSetDeveloperIdFailureEvent* event)
{
	//IwTrace(LOG_TAG, ("void SetDeveloperIdOnFailure(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksSetDeveloperId->OnFailure(event->m_errorCode, event->m_errorMessage);
	}
	else
	{
		Application::m_ui.m_callbacksSetDeveloperId->OnFailure(-1, "Failure event is null");
	}
}

s3eCallback ApplicationCallbacksSetDeveloperId::GetSuccessEvent()
{
	return (s3eCallback)SetDeveloperIdOnSuccess;
}

s3eCallback ApplicationCallbacksSetDeveloperId::GetFailureEvent()
{
	return (s3eCallback)SetDeveloperIdOnFailure;
}

void ApplicationCallbacksSetDeveloperId::OnSuccess()
{
	//IwTrace(LOG_TAG, ("OnSuccess"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksSetDeveloperId::OnSuccess");

	Application::InitOuyaPlugin();
}

void ApplicationCallbacksSetDeveloperId::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//IwTrace(LOG_TAG, (buffer));
	
	std::string msg = "ApplicationCallbacksSetDeveloperId::OnFailure";
	msg.append(" errorMessage=");
	msg.append(errorMessage);
	Application::m_ui.SetMessage(msg);

	Application::SetDeveloperId();
}
