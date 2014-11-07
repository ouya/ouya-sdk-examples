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
#include "ApplicationCallbacksInitOuyaPlugin.h"
#include "CallbacksInitOuyaPlugin.h"
#include "CallbackSingleton.h"

#include "IwDebug.h"

#include <stdio.h>

void InitOuyaPluginOnSuccess(s3eInitOuyaPluginSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void InitOuyaPluginOnSuccess(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksInitOuyaPlugin->OnSuccess();
	}
	else
	{
		Application::m_ui.m_callbacksInitOuyaPlugin->OnFailure(-1, "Success event is null");
	}
}

void InitOuyaPluginOnFailure(s3eInitOuyaPluginFailureEvent* event)
{
	//IwTrace(DEFAULT, ("void InitOuyaPluginOnFailure(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksInitOuyaPlugin->OnFailure(event->m_errorCode, event->m_errorMessage);
	}
	else
	{
		Application::m_ui.m_callbacksInitOuyaPlugin->OnFailure(-1, "Failure event is null");
	}
}

s3eCallback ApplicationCallbacksInitOuyaPlugin::GetSuccessEvent()
{
	return (s3eCallback)InitOuyaPluginOnSuccess;
}

s3eCallback ApplicationCallbacksInitOuyaPlugin::GetFailureEvent()
{
	return (s3eCallback)InitOuyaPluginOnFailure;
}

void ApplicationCallbacksInitOuyaPlugin::OnSuccess()
{
	//IwTrace(DEFAULT, ("OnSuccess"));

	//char buffer[256];
	//sprintf(buffer, "OnSuccess:  %s", gamerUUID.c_str());
	//IwTrace(DEFAULT, (buffer));

	Application::m_ui.SetMessage("ApplicationCallbacksInitOuyaPlugin::OnSuccess");
}

void ApplicationCallbacksInitOuyaPlugin::OnFailure(int errorCode, const std::string& errorMessage)
{
	//IwTrace(DEFAULT, ("OnFailure"));

	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//IwTrace(DEFAULT, (buffer));

	std::string msg = "ApplicationCallbacksInitOuyaPlugin::OnFailure";
	msg.append(" errorMessage=");
	msg.append(errorMessage);
	Application::m_ui.SetMessage(msg);

	Application::InitOuyaPlugin();
}
