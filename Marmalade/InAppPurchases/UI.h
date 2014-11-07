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

#ifndef __UI_H__
#define __UI_H__

#include "ExtensionProduct.h"
#include "ExtensionReceipt.h"
#include "TextButton.h"
#include "TextLabel.h"

#include <string>
#include <vector>

class ApplicationCallbacksInitOuyaPlugin;
class ApplicationCallbacksRequestGamerInfo;
class ApplicationCallbacksRequestProducts;
class ApplicationCallbacksRequestPurchase;
class ApplicationCallbacksRequestReceipts;

class UI
{
public:
	
	UI();

	bool InitUI();

	void Destroy();

	bool HasUIChanged();

	void Render();

	void HandleInput();

	void SetUsername(const std::string& username);

	void SetGamerUUID(const std::string& uuid);

	void SetMessage(const std::string& message);

	void SetDirections();

	void ClearProducts();

	void ClearReceipts();

	void AddProduct(const OuyaSDK::Product& product);

	void AddReceipt(const OuyaSDK::Receipt& receipt);

	ApplicationCallbacksInitOuyaPlugin* m_callbacksInitOuyaPlugin;
	ApplicationCallbacksRequestGamerInfo* m_callbacksRequestGamerInfo;
	ApplicationCallbacksRequestProducts* m_callbacksRequestProducts;
	ApplicationCallbacksRequestPurchase* m_callbacksRequestPurchase;
	ApplicationCallbacksRequestReceipts* m_callbacksRequestReceipts;

private:

	std::vector<std::string> m_productIds;

	std::vector<OuyaSDK::Product> m_pendingProducts;
	std::vector<TextButton*> m_products;

	std::vector<OuyaSDK::Receipt> m_pendingReceipts;
	std::vector<TextButton*> m_receipts;

	TextButton* m_selectedProduct;

	bool m_uiInitialized;

	bool m_uiChanged;

	TextButton m_uiRequestGamerUUID;
	TextButton m_uiRequestProducts;
	TextButton m_uiRequestPurchase;
	TextButton m_uiRequestReceipts;
	TextButton m_uiPause;
	
	TextLabel m_uiLabelUsername;
	TextLabel m_uiLabelGamerUuid;
	TextLabel m_uiLabelDirections;
	TextLabel m_uiLabelMessage;

	TextButton* m_selectedButton;

	void RenderThreadInitProducts();
	void RenderThreadInitReceipts();
};

#endif