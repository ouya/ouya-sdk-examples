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
#include "ApplicationCallbacksRequestReceipts.h"
#include "CallbacksRequestReceipts.h"
#include "ExtensionReceipt.h"

#include "IwDebug.h"

#include <stdio.h>

void RequestReceiptsOnSuccess(s3eRequestReceiptsSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestReceiptsOnSuccess(event)"));
	if (event)
	{
		std::vector<OuyaSDK::Receipt> receipts;
		for (int index = 0; index < event->m_receiptsLength; ++index)
		{
			OuyaSDK::ExtensionReceipt eReceipt = event->m_receipts[index];
			OuyaSDK::Receipt receipt;
			receipt.Init();
			receipt.Copy(eReceipt);
			receipts.push_back(receipt);

			IwTrace(DEFAULT, (receipt.Identifier.c_str()));
		}
		Application::m_ui.m_callbacksRequestReceipts->OnSuccess(receipts);
	}
	else
	{
		Application::m_ui.m_callbacksRequestReceipts->OnFailure(-1, "Success event is null");
	}
}

void RequestReceiptsOnFailure(s3eRequestReceiptsFailureEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestReceiptsOnFailure(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestReceipts->OnFailure(event->m_errorCode, event->m_errorMessage);
	}
	else
	{
		Application::m_ui.m_callbacksRequestReceipts->OnFailure(-1, "Failure event is null");
	}
}

void RequestReceiptsOnCancel(s3eRequestReceiptsCancelEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestReceiptsOnCancel(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestReceipts->OnCancel();
	}
	else
	{
		Application::m_ui.m_callbacksRequestReceipts->OnFailure(-1, "Cancel event is null");
	}
}

s3eCallback ApplicationCallbacksRequestReceipts::GetSuccessEvent()
{
	return (s3eCallback)RequestReceiptsOnSuccess;
}

s3eCallback ApplicationCallbacksRequestReceipts::GetFailureEvent()
{
	return (s3eCallback)RequestReceiptsOnFailure;
}

s3eCallback ApplicationCallbacksRequestReceipts::GetCancelEvent()
{
	return (s3eCallback)RequestReceiptsOnCancel;
}

void ApplicationCallbacksRequestReceipts::OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts)
{
	//IwTrace(DEFAULT, ("OnSuccess"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnSuccess");

	Application::m_ui.ClearReceipts();

	for (unsigned int index = 0; index < receipts.size(); ++index)
	{
		Application::m_ui.AddReceipt(receipts[index]);
	}
}

void ApplicationCallbacksRequestReceipts::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//IwTrace(DEFAULT, (buffer));
	
	std::string msg = "ApplicationCallbacksRequestReceipts::OnFailure";
	msg.append(" errorMessage=");
	msg.append(errorMessage);
	Application::m_ui.SetMessage(msg);
}

void ApplicationCallbacksRequestReceipts::OnCancel()
{
	//IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnCancel");
}