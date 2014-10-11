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
#include "ApplicationCallbacksRequestPurchase.h"
#include "CallbacksRequestPurchase.h"

#include "IwDebug.h"

#include <stdio.h>

void RequestPurchaseOnSuccess(s3eRequestPurchaseSuccessEvent* event)
{
	//IwTrace(DEFAULT, ("void RequestPurchaseOnSuccess(event)"));
	if (event)
	{
		OuyaSDK::ExtensionProduct eProduct = event->m_product;
		OuyaSDK::Product product;
		product.Init();
		product.Copy(eProduct);

		Application::m_ui.m_callbacksRequestPurchase->OnSuccess(product);
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