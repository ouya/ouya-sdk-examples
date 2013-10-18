#include "Application.h"
#include "ApplicationCallbacksRequestReceipts.h"
#include "CallbacksRequestReceipts.h"

#include "IwDebug.h"

#include <stdio.h>

void RequestReceiptsOnSuccess(s3eRequestReceiptsSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestReceiptsOnSuccess(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestReceipts->OnSuccess(event->m_receipts);
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
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnSuccess");
}

void ApplicationCallbacksRequestReceipts::OnCancel()
{
	//IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnSuccess");
}