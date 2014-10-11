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
#include "ApplicationCallbacksRequestProducts.h"
#include "CallbackSingleton.h"
#include "CallbacksRequestProducts.h"
#include "ExtensionProduct.h"

#include "IwDebug.h"

#include <stdio.h>

void RequestProductsOnSuccess(s3eRequestProductsSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestProductsOnSuccess(event)"));
	if (event)
	{
		std::vector<OuyaSDK::Product> products;
		for (int index = 0; index < event->m_productsLength; ++index)
		{
			OuyaSDK::ExtensionProduct eProduct = event->m_products[index];
			OuyaSDK::Product product;
			product.Copy(eProduct);
			products.push_back(product);

			//IwTrace(DEFAULT, (product.Name.c_str()));
		}
		Application::m_ui.m_callbacksRequestProducts->OnSuccess(products);
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