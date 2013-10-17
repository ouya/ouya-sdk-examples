#include "Application.h"
#include "ApplicationCallbacksRequestProducts.h"
#include "CallbackSingleton.h"

#include "IwDebug.h"

#include <stdio.h>

void ApplicationCallbacksRequestProducts::OnSuccess(const std::vector<OuyaSDK::Product>& products)
{
	IwTrace(DEFAULT, ("OnSuccess"));
	
	/*
	Application::m_ui.SetMessage("ApplicationCallbacksRequestProducts::OnSuccess");

	Application::m_ui.ClearProducts();

	for (unsigned int index = 0; index < products.size(); index++)
	{
		Application::m_ui.AddProduct(products[index]);
	}
	*/
}

void ApplicationCallbacksRequestProducts::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(DEFAULT, (buffer));
	
	//Application::m_ui.SetMessage("ApplicationCallbacksRequestProducts::OnFailure");
}

void ApplicationCallbacksRequestProducts::OnCancel()
{
	IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestProducts::OnCancel");
}