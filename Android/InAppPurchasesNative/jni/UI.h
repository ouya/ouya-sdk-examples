#ifndef __UI_H__
#define __UI_H__

#include "CallbacksFetchGamerUUID.h"
#include "CallbacksRequestProducts.h"
#include "CallbacksRequestPurchase.h"
#include "CallbacksRequestReceipts.h"
#include "PluginOuya.h"
#include "Product.h"
#include "Receipt.h"
#include "TextButton.h"

#include <string>

class Engine;

class UI
{
public:
	
	UI();

	void Initialize(PluginOuya* pluginOuya);

	void SetEngine(Engine* engine);

	bool InitUI();

	void Destroy();

	void Resize(int w, int h);

	bool HasUIChanged();

	void Render();

	void HandleInput(int keyCode, int action);

	void SetGamerUUID(std::string gamerUUID);

	void SetMessage(std::string message);

	void ClearProducts();

	void ClearReceipts();

	void AddProduct(Product product);

	void AddReceipt(Receipt receipt);

private:

	Engine* m_engine;

	std::vector<std::string> m_productIds;

	PluginOuya* m_pluginOuya;

	std::vector<Product> m_pendingProducts;
	std::vector<TextButton*> m_products;

	std::vector<Receipt> m_pendingReceipts;
	std::vector<TextButton*> m_receipts;

	TextButton* m_selectedProduct;

	bool m_uiInitialized;

	bool m_uiChanged;

	TextButton m_uiRequestGamerUUID;
	TextButton m_uiRequestProducts;
	TextButton m_uiRequestPurchase;
	TextButton m_uiRequestReceipts;
	TextButton m_uiPause;
	void* m_uiLabelFetch;

	void* m_uiLabelDirections;
	void* m_uiLabelMessage;

	TextButton* m_selectedButton;

	CallbacksFetchGamerUUID* m_callbacksFetchGamerUUID;
	CallbacksRequestProducts* m_callbacksRequestProducts;
	CallbacksRequestPurchase* m_callbacksRequestPurchase;
	CallbacksRequestReceipts* m_callbacksRequestReceipts;

	void SetupLabel(void** uiLabel, int font, int size, std::string text);
	void DestroyLabel(void** uiLabel);

	void RenderThreadInitProducts();
	void RenderThreadInitReceipts();
};

#endif