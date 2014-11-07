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
#include "ApplicationCallbacksRequestGamerInfo.h"
#include "ApplicationCallbacksRequestProducts.h"
#include "ApplicationCallbacksRequestPurchase.h"
#include "ApplicationCallbacksRequestReceipts.h"
#include "CallbackSingleton.h"
#include "Controller.h"
#include "ODK.h"
#include "ExtensionProduct.h"
#include "ExtensionReceipt.h"
#include "UI.h"

#include <algorithm>
#include <map>
#include <sstream>
#include <string>

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

#include "s3e.h"
#include "IwDebug.h"

UI::UI()
{
	m_uiInitialized = false;

	m_callbacksInitOuyaPlugin = new ApplicationCallbacksInitOuyaPlugin();
	m_callbacksRequestGamerInfo = new ApplicationCallbacksRequestGamerInfo();
	m_callbacksRequestProducts = new ApplicationCallbacksRequestProducts();
	m_callbacksRequestPurchase = new ApplicationCallbacksRequestPurchase();
	m_callbacksRequestReceipts = new ApplicationCallbacksRequestReceipts();

	m_productIds.push_back("long_sword");
	m_productIds.push_back("sharp_axe");
	m_productIds.push_back("cool_level");
	m_productIds.push_back("awesome_sauce");
	m_productIds.push_back("__DECLINED__THIS_PURCHASE");

	m_selectedProduct = NULL;

	m_uiChanged = false;
}

void UI::RenderThreadInitProducts()
{
	// delay creation of new labels for the rendering thread
	if (m_pendingProducts.size() > 0)
	{
		for (unsigned int index = 0; index < m_pendingProducts.size(); ++index)
		{
			char buffer[1024];

			//sprintf(buffer, "Copy product %s", m_pendingProducts[index].Identifier.c_str());
			//LOGI(buffer);

			TextButton* txtProduct = new TextButton();
			OuyaSDK::Product* newProduct = new OuyaSDK::Product(m_pendingProducts[index]);
			txtProduct->DataContext = newProduct;

			//sprintf(buffer, "Setting up product ui %s", newProduct->Identifier.c_str());
			//LOGI(buffer);

			sprintf(buffer, "--> %s (%.2f)", newProduct->Identifier.c_str(), newProduct->LocalPrice);
			txtProduct->ActiveText = buffer;

			sprintf(buffer, "%s (%.2f)", newProduct->Identifier.c_str(), newProduct->LocalPrice);
			txtProduct->InactiveText = buffer;

			txtProduct->Setup(2, 32);

			txtProduct->Right = &m_uiRequestPurchase;

			m_products.push_back(txtProduct);

			if (index == 0)
			{
				m_selectedProduct = m_products[0];
				m_products[0]->SetActive(true);
				m_uiRequestProducts.Left = m_selectedProduct;
				m_uiRequestPurchase.Left = m_selectedProduct;
			}
			else
			{
				m_products[index-1]->Down = m_products[index];
				m_products[index]->Up = m_products[index-1];
			}
		}

		m_uiChanged = true;

		m_pendingProducts.clear();

		const int w = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
		const int h = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

		for (unsigned int index = 0; index < m_products.size(); ++index)
		{
			m_products[index]->SetPosition(w*0.3f, h/3 + index * 25);
		}
	}
}

void UI::RenderThreadInitReceipts()
{
	// delay creation of new labels for the rendering thread
	if (m_pendingReceipts.size() > 0)
	{
		for (unsigned int index = 0; index < m_pendingReceipts.size(); ++index)
		{
			TextButton* txtReceipt = new TextButton();
			OuyaSDK::Receipt* newReceipt = new OuyaSDK::Receipt(m_pendingReceipts[index]);
			txtReceipt->DataContext = newReceipt;

			std::ostringstream buffer;
			buffer << newReceipt->Identifier;
			buffer << " " << newReceipt->LocalPrice;
			buffer << " " << newReceipt->GeneratedDate;
			buffer << " " << newReceipt->PurchaseDate;
			txtReceipt->ActiveText = buffer.str();
			txtReceipt->InactiveText = txtReceipt->ActiveText;

			txtReceipt->Setup(2, 32);

			m_receipts.push_back(txtReceipt);
		}

		m_uiChanged = true;

		m_pendingReceipts.clear();

		const int w = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
		const int h = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

		for (unsigned int index = 0; index < m_receipts.size(); ++index)
		{
			m_receipts[index]->SetPosition(w*0.7f, h/3 + index * 25);
		}
	}
}

bool UI::InitUI()
{
	if (m_uiInitialized)
	{
		RenderThreadInitProducts();
		RenderThreadInitReceipts();
		SetDirections();
		return true;
	}

	m_uiLabelUsername.Setup(300, 280, "");
	m_uiLabelGamerUuid.Setup(300, 300, "");
	m_uiLabelDirections.Setup(300, 300, "Directions:");
	m_uiLabelMessage.Setup(300, 300, "Message:");

	m_uiRequestGamerUUID.Setup(2, 32, "[Get GamerUUID]", "Get GamerUUID");
	m_uiRequestProducts.Setup(2, 32, "[Get Products]", "Get Products");
	m_uiRequestPurchase.Setup(2, 32, "[Purchase]", "Purchase");
	m_uiRequestReceipts.Setup(2, 32, "[Get Receipts]", "Get Receipts");
	m_uiPause.Setup(2, 32, "[Pause]", "Pause");

	m_uiRequestGamerUUID.Down = &m_uiRequestReceipts;
	m_uiRequestGamerUUID.Left = &m_uiRequestProducts;

	m_uiRequestProducts.Down = &m_uiRequestPurchase;
	m_uiRequestProducts.Right = &m_uiRequestGamerUUID;

	m_uiRequestPurchase.Up = &m_uiRequestProducts;
	m_uiRequestPurchase.Right = &m_uiRequestReceipts;

	m_uiRequestReceipts.Left = &m_uiRequestPurchase;
	m_uiRequestReceipts.Up = &m_uiRequestGamerUUID;

	m_uiPause.Up = &m_uiRequestReceipts;

	m_selectedButton = &m_uiPause;
	m_uiPause.SetActive(true);

	const int w = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
    const int h = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

	m_uiLabelUsername.SetPosition(w*4/5, h/6 - 40);
	m_uiLabelGamerUuid.SetPosition(w*4/5, h/6);
	m_uiLabelDirections.SetPosition(w/5, h*2/3);
	m_uiLabelMessage.SetPosition(w/5 + 25, h*2/3 + 50);

	m_uiRequestGamerUUID.SetPosition(w*3/5, h/6);
	m_uiRequestProducts.SetPosition(w*2/5, h/6);
	m_uiRequestPurchase.SetPosition(w*2/5, h/4);
	m_uiRequestReceipts.SetPosition(w*3/5, h/4);
	m_uiPause.SetPosition(w/2, h/2);

	m_uiInitialized = true;

	return true;
}

void UI::Destroy()
{
	m_uiRequestGamerUUID.Destroy();
	m_uiRequestProducts.Destroy();
	m_uiRequestPurchase.Destroy();
	m_uiRequestReceipts.Destroy();
	m_uiPause.Destroy();

	ClearProducts();
	ClearReceipts();
}

bool UI::HasUIChanged()
{
	if (m_uiChanged)
	{
		m_uiChanged = false;
		return true;
	}
	return false;
}

void UI::Render()
{
	m_uiLabelUsername.Render();
	m_uiLabelGamerUuid.Render();
	m_uiLabelDirections.Render();
	m_uiLabelMessage.Render();

	m_uiRequestGamerUUID.Render();
	m_uiRequestProducts.Render();
	m_uiRequestPurchase.Render();
	m_uiRequestReceipts.Render();
	m_uiPause.Render();

	for (unsigned int index = 0; index < m_products.size(); ++index)
	{
		m_products[index]->Render();
	}

	for (unsigned int index = 0; index < m_receipts.size(); ++index)
	{
		m_receipts[index]->Render();
	}
}

void UI::HandleInput()
{
	std::vector<int> released = std::vector<int>();
	for (int playerNum = 0; playerNum < OuyaController_MAX_CONTROLLERS; ++playerNum)
	{
		if (OuyaPlugin_isPressedUp(playerNum, OuyaController_BUTTON_MENU))
		{
			released.push_back(OuyaController_BUTTON_MENU);
		}
		if (OuyaPlugin_isPressedUp(playerNum, OuyaController_BUTTON_DPAD_DOWN))
		{
			released.push_back(OuyaController_BUTTON_DPAD_DOWN);
		}
		if (OuyaPlugin_isPressedUp(playerNum, OuyaController_BUTTON_DPAD_LEFT))
		{
			released.push_back(OuyaController_BUTTON_DPAD_LEFT);
		}
		if (OuyaPlugin_isPressedUp(playerNum, OuyaController_BUTTON_DPAD_RIGHT))
		{
			released.push_back(OuyaController_BUTTON_DPAD_RIGHT);
		}
		if (OuyaPlugin_isPressedUp(playerNum, OuyaController_BUTTON_DPAD_UP))
		{
			released.push_back(OuyaController_BUTTON_DPAD_UP);
		}
		if (OuyaPlugin_isPressedUp(playerNum, OuyaController_BUTTON_O))
		{
			released.push_back(OuyaController_BUTTON_O);
		}
	}
	OuyaPlugin_clearButtonStates();

	if (std::find(released.begin(), released.end(), OuyaController_BUTTON_MENU) != released.end())
	{
		if (m_selectedButton &&
			std::find(m_products.begin(), m_products.end(), m_selectedButton) == m_products.end())
		{
			m_selectedButton->SetActive(false);
		}

		m_selectedButton = &m_uiPause;
		m_uiPause.SetActive(true);

		SetDirections();

		//LOGI("Key event, hack, regaining focus...\r\n");
		//Application::m_app->activity->callbacks->onWindowFocusChanged(Application::m_app->activity, true);
		//LOGI("Key event, hack complete***\r\n");
	}

	if (std::find(released.begin(), released.end(), OuyaController_BUTTON_DPAD_LEFT) != released.end())
	{
		if (m_selectedButton &&
			m_selectedButton->Left)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Left;
			m_selectedButton->SetActive(true);

			SetDirections();
		}
	}

	if (std::find(released.begin(), released.end(), OuyaController_BUTTON_DPAD_RIGHT) != released.end())
	{
		if (m_selectedButton &&
			m_selectedButton->Right)
		{
			if (std::find(m_products.begin(), m_products.end(), m_selectedButton) == m_products.end())
			{
				m_selectedButton->SetActive(false);
			}
			m_selectedButton = m_selectedButton->Right;
			m_selectedButton->SetActive(true);

			SetDirections();
		}
	}

	if (std::find(released.begin(), released.end(), OuyaController_BUTTON_DPAD_UP) != released.end())
	{
		if (m_selectedButton &&
			m_selectedButton->Up)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Up;
			m_selectedButton->SetActive(true);

			if (std::find(m_products.begin(), m_products.end(), m_selectedButton) != m_products.end())
			{
				m_selectedProduct = m_selectedButton;
				m_uiRequestProducts.Left = m_selectedProduct;
				m_uiRequestPurchase.Left = m_selectedProduct;
			}

			SetDirections();
		}
	}

	if (std::find(released.begin(), released.end(), OuyaController_BUTTON_DPAD_DOWN) != released.end())
	{
		if (m_selectedButton &&
			m_selectedButton->Down)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Down;
			m_selectedButton->SetActive(true);

			if (std::find(m_products.begin(), m_products.end(), m_selectedButton) != m_products.end())
			{
				m_selectedProduct = m_selectedButton;
				m_uiRequestProducts.Left = m_selectedProduct;
				m_uiRequestPurchase.Left = m_selectedProduct;
			}

			SetDirections();
		}
	}

	if (std::find(released.begin(), released.end(), OuyaController_BUTTON_O) != released.end())
	{
		if (m_selectedButton)
		{
			if (m_selectedButton == &m_uiRequestGamerUUID)
			{

				SetMessage("Fetching gamer uuid...");
				OuyaPlugin_asyncOuyaRequestGamerInfo(
					Application::m_ui.m_callbacksRequestGamerInfo->GetSuccessEvent(),
					Application::m_ui.m_callbacksRequestGamerInfo->GetFailureEvent(),
					Application::m_ui.m_callbacksRequestGamerInfo->GetCancelEvent());
			}
			if (m_selectedButton == &m_uiRequestProducts)
			{
				ClearProducts();

				SetMessage("Requesting products...");
				
				// prepare json
				std::string productsJson = "[";

				int index = 0;
				for (std::vector<std::string>::iterator iter = m_productIds.begin(); iter != m_productIds.end(); ++iter)
				{
					std::string productId = *iter;
					if (index == 0)
					{
						productsJson.append("\"");
					}
					else
					{
						productsJson.append(", \"");
					}
					productsJson.append(productId);
					productsJson.append("\"");
					++index;
				}

				productsJson.append("]");

				IwTrace(DEFAULT, (productsJson.c_str()));

				OuyaPlugin_asyncOuyaRequestProducts(productsJson.c_str(),
					Application::m_ui.m_callbacksRequestProducts->GetSuccessEvent(),
					Application::m_ui.m_callbacksRequestProducts->GetFailureEvent(),
					Application::m_ui.m_callbacksRequestProducts->GetCancelEvent());
			}
			if (m_selectedButton == &m_uiRequestPurchase)
			{
				if (NULL == m_selectedProduct)
				{
					SetMessage("No product selected.");
				}
				else
				{
					OuyaSDK::Product* product = (OuyaSDK::Product*)m_selectedProduct->DataContext;
					if (NULL == product)
					{
						SetMessage("No product selected.");
					}
					else
					{
						SetMessage("Requesting purchase...");
						OuyaPlugin_asyncOuyaRequestPurchase(product->Identifier.c_str(),
							Application::m_ui.m_callbacksRequestPurchase->GetSuccessEvent(),
							Application::m_ui.m_callbacksRequestPurchase->GetFailureEvent(),
							Application::m_ui.m_callbacksRequestPurchase->GetCancelEvent());
					}
				}
			}
			if (m_selectedButton == &m_uiRequestReceipts)
			{
				ClearReceipts();

				SetMessage("Requesting receipts...");
				OuyaPlugin_asyncOuyaRequestReceipts(
					Application::m_ui.m_callbacksRequestReceipts->GetSuccessEvent(),
					Application::m_ui.m_callbacksRequestReceipts->GetFailureEvent(),
					Application::m_ui.m_callbacksRequestReceipts->GetCancelEvent());
			}
		}
	}
}

void UI::SetUsername(const std::string& username)
{
	m_uiLabelUsername.SetText(username);
}

void UI::SetGamerUUID(const std::string& uuid)
{
	m_uiLabelGamerUuid.SetText(uuid);
}

void UI::SetMessage(const std::string& message)
{
	std::string text = "Message: ";
	text.append(message);
	m_uiLabelMessage.SetText(text);
}

void UI::SetDirections()
{
	if (m_selectedButton)
	{
		std::string text;
		if (std::find(m_products.begin(), m_products.end(), m_selectedButton) != m_products.end())
		{
			text = "Select a product with the DPAD (up/down) | DPAD (right) Return to Purchase";
		}
		else if (m_selectedButton == &m_uiRequestGamerUUID)
		{
			text = "Press (O): to fetch the gamer uuid | DPAD (left) Get Products | DPAD (down) Get Receipts";
		}
		else if (m_selectedButton == &m_uiRequestProducts)
		{
			if (m_products.size() > 0)
			{
				text = "Press (O): Get products | DPAD (left) Select a product | DPAD (down) Purchase | DPAD (right) Fetch gamer uuid";
			}
			else
			{
				text = "Press (O): Get products | DPAD (down) Purchase | DPAD (right) Fetch gamer uuid";
			}
		}
		else if (m_selectedButton == &m_uiRequestPurchase)
		{
			if (m_products.size() > 0)
			{
				text = "Press (O): to purchase products | DPAD (left) Select a product | DPAD (up) Get products | DPAD (right) Get receipts";
			}
			else
			{
				text = "DPAD (up) Get products | DPAD (right) Get receipts";
			}
		}
		else if (m_selectedButton == &m_uiRequestReceipts)
		{
			text = "Press (O): to get receipts | DPAD (left) Purchase a product  | DPAD (up) Fetch gamer uuid";
		}
		else if (m_selectedButton == &m_uiPause)
		{
			text = "Pause detected | DPAD (up) Get receipts";
		}
		else
		{
			return;
		}

		m_uiLabelDirections.SetText(text);
	}
}

void UI::ClearProducts()
{
	for (unsigned int index = 0; index < m_products.size(); ++index)
	{
		TextButton* txtProduct = m_products[index];
		txtProduct->Destroy();
		OuyaSDK::Product* product = (OuyaSDK::Product*)txtProduct->DataContext;
		if (product)
		{
			delete product;
		}
		delete txtProduct;
	}
	m_products.clear();

	m_uiRequestProducts.Left = NULL;
	m_uiRequestPurchase.Left = NULL;

	m_selectedProduct = NULL;
}

void UI::ClearReceipts()
{
	for (unsigned int index = 0; index < m_receipts.size(); ++index)
	{
		TextButton* txtReceipt = m_receipts[index];
		txtReceipt->Destroy();
		OuyaSDK::Receipt* receipt = (OuyaSDK::Receipt*)txtReceipt->DataContext;
		if (receipt)
		{
			delete receipt;
		}
		delete txtReceipt;
	}
	m_receipts.clear();
}

void UI::AddProduct(const OuyaSDK::Product& product)
{
	m_pendingProducts.push_back(product);
}

void UI::AddReceipt(const OuyaSDK::Receipt& receipt)
{
	m_pendingReceipts.push_back(receipt);
}