#include "Application.h"
#include "ApplicationCallbacksRequestPurchase.h"

#include "IwDebug.h"

#include <stdio.h>

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
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestPurchase::OnFailure");
}

void ApplicationCallbacksRequestPurchase::OnCancel()
{
	IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestPurchase::OnCancel");
}