#include "Application.h"
#include "CallbackSingleton.h"
#include "Controller.h"
#include "Product.h"
#include "Receipt.h"
#include "ui.h"

#include <algorithm>

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

#define APP_NAME "inapppurchasenative-ui"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

UI::UI()
{
	m_uiInitialized = false;

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
	}
}

void UI::RenderThreadInitReceipts()
{
	// delay creation of new labels for the rendering thread
	if (m_pendingReceipts.size() > 0)
	{
		for (unsigned int index = 0; index < m_pendingReceipts.size(); ++index)
		{
			char buffer[1024];

			//sprintf(buffer, "Copy receipt %s", m_pendingReceipts[index].Identifier.c_str());
			//LOGI(buffer);

			TextButton* txtReceipt = new TextButton();
			OuyaSDK::Receipt* newReceipt = new OuyaSDK::Receipt(m_pendingReceipts[index]);
			txtReceipt->DataContext = newReceipt;

			//sprintf(buffer, "Setting up receipt ui %s", newReceipt->Identifier.c_str());
			//LOGI(buffer);

			sprintf(buffer, "%s (%.2f)", newReceipt->Identifier.c_str(), newReceipt->LocalPrice);
			txtReceipt->ActiveText = buffer;
			txtReceipt->InactiveText = buffer;

			txtReceipt->Setup(2, 32);

			m_receipts.push_back(txtReceipt);
		}

		m_uiChanged = true;

		m_pendingReceipts.clear();
	}
}

void UI::SetupLabel(std::string* uiLabel, int font, int size, std::string text)
{
	*uiLabel = text;
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

	SetupLabel(&m_uiLabelFetch, 2, 32, "");
	SetupLabel(&m_uiLabelDirections, 2, 32, "Directions:");
	SetupLabel(&m_uiLabelMessage, 2, 32, "Message:");

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

void UI::Resize(int w, int h)
{
	/*
	NVBFTextCursorAlign(m_uiLabelFetch, NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	NVBFTextCursorPos(m_uiLabelFetch, w*4/5, h/6);

	NVBFTextCursorAlign(m_uiLabelDirections, NVBF_ALIGN_LEFT, NVBF_ALIGN_CENTER);
	NVBFTextCursorPos(m_uiLabelDirections, w/5, h*2/3);

	NVBFTextCursorAlign(m_uiLabelMessage, NVBF_ALIGN_LEFT, NVBF_ALIGN_CENTER);
	NVBFTextCursorPos(m_uiLabelMessage, w/5 + 25, h*2/3 + 50);

	m_uiRequestGamerUUID.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestGamerUUID.SetPosition(w*3/5, h/6);

	m_uiRequestProducts.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestProducts.SetPosition(w*2/5, h/6);

	m_uiRequestPurchase.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestPurchase.SetPosition(w*2/5, h/4);

	m_uiRequestReceipts.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestReceipts.SetPosition(w*3/5, h/4);

	m_uiPause.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiPause.SetPosition(w/2, h/2);

	for (int index = 0; index < m_products.size(); ++index)
	{
		m_products[index]->SetAlignment(NVBF_ALIGN_RIGHT, NVBF_ALIGN_CENTER);
		m_products[index]->SetPosition(w*0.45f, h/3 + index * 25);
	}

	for (int index = 0; index < m_receipts.size(); ++index)
	{
		m_receipts[index]->SetAlignment(NVBF_ALIGN_LEFT, NVBF_ALIGN_CENTER);
		m_receipts[index]->SetPosition(w*0.55f, h/3 + index * 25);
	}
	*/
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
	//IwGxPrintString(m_x, m_y, m_text.c_str());
	//NVBFTextRender(m_uiLabelFetch);
	//NVBFTextRender(m_uiLabelDirections);
	//NVBFTextRender(m_uiLabelMessage);

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

void UI::HandleInput(int keyCode, int action)
{
	/*
	if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == OuyaSDK::Controller::BUTTON_MENU)
	{
		if (m_selectedButton &&
			std::find(m_products.begin(), m_products.end(), m_selectedButton) == m_products.end())
		{
			m_selectedButton->SetActive(false);
		}

		m_selectedButton = &m_uiPause;
		m_uiPause.SetActive(true);

		SetDirections();

		LOGI("Key event, hack, regaining focus...\r\n");
		Application::m_app->activity->callbacks->onWindowFocusChanged(Application::m_app->activity, true);
		LOGI("Key event, hack complete***\r\n");
	}

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == OuyaSDK::Controller::BUTTON_DPAD_LEFT)
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

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == OuyaSDK::Controller::BUTTON_DPAD_RIGHT)
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

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == OuyaSDK::Controller::BUTTON_DPAD_UP) //dpad up
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

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == OuyaSDK::Controller::BUTTON_DPAD_DOWN)
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

	else if(action == AMOTION_EVENT_ACTION_UP &&
		keyCode == OuyaSDK::Controller::BUTTON_O)
	{
		if (m_selectedButton)
		{
			LOGI("Executing action");
			if (m_selectedButton == &m_uiRequestGamerUUID)
			{
				SetMessage("Fetching gamer uuid...");
				Application::m_pluginOuya.AsyncOuyaFetchGamerUUID(&m_callbacksFetchGamerUUID);
			}
			if (m_selectedButton == &m_uiRequestProducts)
			{
				SetMessage("Requesting products...");
				Application::m_pluginOuya.AsyncOuyaRequestProducts(&m_callbacksRequestProducts, m_productIds);
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
						Application::m_pluginOuya.AsyncOuyaRequestPurchase(&m_callbacksRequestPurchase, product->Identifier);
					}
				}
			}
			if (m_selectedButton == &m_uiRequestReceipts)
			{
				SetMessage("Requesting receipts...");
				Application::m_pluginOuya.AsyncOuyaRequestReceipts(&m_callbacksRequestReceipts);
			}
		}
	}
	*/
}

void UI::SetGamerUUID(const std::string& gamerUUID)
{
	m_uiLabelFetch = gamerUUID;
}

void UI::SetMessage(const std::string& message)
{
	std::string text = "Message: ";
	text.append(message);
	m_uiLabelMessage = text;
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

		m_uiLabelDirections = text;
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

void UI::AddProduct(OuyaSDK::Product product)
{
	m_pendingProducts.push_back(product);
}

void UI::AddReceipt(OuyaSDK::Receipt receipt)
{
	m_pendingReceipts.push_back(receipt);
}