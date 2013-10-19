#include "Application.h"
#include "ApplicationCallbacksRequestPurchase.h"
#include "CallbacksRequestPurchase.h"

#include "IwDebug.h"

#include <stdio.h>

void RequestPurchaseOnSuccess(s3eRequestPurchaseSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestPurchaseOnSuccess(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestPurchase->OnSuccess(event->m_product);
	}
	else
	{
		Application::m_ui.m_callbacksRequestPurchase->OnFailure(-1, "Success event is null");
	}
}

void RequestPurchaseOnFailure(s3eRequestPurchaseFailureEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestPurchaseOnFailure(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestPurchase->OnFailure(event->m_errorCode, event->m_errorMessage);
	}
	else
	{
		Application::m_ui.m_callbacksRequestPurchase->OnFailure(-1, "Failure event is null");
	}
}

void RequestPurchaseOnCancel(s3eRequestPurchaseCancelEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestPurchaseOnCancel(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestPurchase->OnCancel();
	}
	else
	{
		Application::m_ui.m_callbacksRequestPurchase->OnFailure(-1, "Cancel event is null");
	}
}

s3eCallback ApplicationCallbacksRequestPurchase::GetSuccessEvent()
{
	return (s3eCallback)RequestPurchaseOnSuccess;
}

s3eCallback ApplicationCallbacksRequestPurchase::GetFailureEvent()
{
	return (s3eCallback)RequestPurchaseOnFailure;
}

s3eCallback ApplicationCallbacksRequestPurchase::GetCancelEvent()
{
	return (s3eCallback)RequestPurchaseOnCancel;
}

void ApplicationCallbacksRequestPurchase::OnSuccess(const OuyaSDK::Product& product)
{
	IwTrace(DEFAULT, ("OnSuccess"));

	std::string message = "ApplicationCallbacksRequestPurchase::OnSuccess: ";
	message.append(product.Identifier);

	Application::m_ui.SetMessage(message);
}

void ApplicationCallbacksRequestPurchase::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(DEFAULT, (buffer));
	
	std::string msg = "ApplicationCallbacksRequestPurchase::OnFailure";
	msg.append(" errorMessage=");
	msg.append(errorMessage);
	Application::m_ui.SetMessage(msg);
}

void ApplicationCallbacksRequestPurchase::OnCancel()
{
	IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestPurchase::OnCancel");
}