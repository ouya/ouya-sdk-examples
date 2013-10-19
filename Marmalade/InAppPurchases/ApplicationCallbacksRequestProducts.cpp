#include "Application.h"
#include "ApplicationCallbacksRequestProducts.h"
#include "CallbackSingleton.h"
#include "CallbacksRequestProducts.h"

#include "IwDebug.h"

#include <stdio.h>

void RequestProductsOnSuccess(s3eRequestProductsSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestProductsOnSuccess(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestProducts->OnSuccess(event->m_products);
	}
	else
	{
		Application::m_ui.m_callbacksRequestProducts->OnFailure(-1, "Success event is null");
	}
}

void RequestProductsOnFailure(s3eRequestProductsFailureEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestProductsOnFailure(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestProducts->OnFailure(event->m_errorCode, event->m_errorMessage);
	}
	else
	{
		Application::m_ui.m_callbacksRequestProducts->OnFailure(-1, "Failure event is null");
	}
}

void RequestProductsOnCancel(s3eRequestProductsCancelEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestProductsOnCancel(event)"));
	if (event)
	{
		Application::m_ui.m_callbacksRequestProducts->OnCancel();
	}
	else
	{
		Application::m_ui.m_callbacksRequestProducts->OnFailure(-1, "Cancel event is null");
	}
}

s3eCallback ApplicationCallbacksRequestProducts::GetSuccessEvent()
{
	return (s3eCallback)RequestProductsOnSuccess;
}

s3eCallback ApplicationCallbacksRequestProducts::GetFailureEvent()
{
	return (s3eCallback)RequestProductsOnFailure;
}

s3eCallback ApplicationCallbacksRequestProducts::GetCancelEvent()
{
	return (s3eCallback)RequestProductsOnCancel;
}

void ApplicationCallbacksRequestProducts::OnSuccess(const std::vector<OuyaSDK::Product>& products)
{
	//IwTrace(DEFAULT, ("OnSuccess"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestProducts::OnSuccess");

	Application::m_ui.ClearProducts();

	for (unsigned int index = 0; index < products.size(); index++)
	{
		Application::m_ui.AddProduct(products[index]);
	}
}

void ApplicationCallbacksRequestProducts::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//IwTrace(DEFAULT, (buffer));
	
	std::string msg = "ApplicationCallbacksRequestProducts::OnFailure";
	msg.append(" errorMessage=");
	msg.append(errorMessage);
	Application::m_ui.SetMessage(msg);
}

void ApplicationCallbacksRequestProducts::OnCancel()
{
	//IwTrace(DEFAULT, ("OnCancel"));
	
	Application::m_ui.SetMessage("ApplicationCallbacksRequestProducts::OnCancel");
}