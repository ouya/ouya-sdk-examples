#include "Application.h"
#include "ApplicationCallbacksRequestReceipts.h"

#include "IwDebug.h"
#include <stdio.h>

void ApplicationCallbacksRequestReceipts::OnSuccess(const std::vector<OuyaSDK::Receipt>& receipts)
{
	IwTrace(DEFAULT, ("OnSuccess"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnSuccess");

	Application::m_ui.ClearReceipts();

	for (unsigned int index = 0; index < receipts.size(); ++index)
	{
		Application::m_ui.AddReceipt(receipts[index]);
	}
}

void ApplicationCallbacksRequestReceipts::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(DEFAULT, (buffer));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnSuccess");
}

void ApplicationCallbacksRequestReceipts::OnCancel()
{
	IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestReceipts::OnSuccess");
}