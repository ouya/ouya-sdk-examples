#ifndef __UI_H__
#define __UI_H__

#include "CallbacksFetchGamerUUID.h"
#include "CallbacksRequestProducts.h"
#include "CallbacksRequestPurchase.h"
#include "CallbacksRequestReceipts.h"
#include "PluginOuya.h"
#include "Product.h"
#include "TextButton.h"

#include <string>

class UI
{
public:
	
	UI();

	void Initialize(PluginOuya* pluginOuya);

	bool InitUI();

	void Destroy();

	void Resize(int w, int h);

	bool HasUIChanged();

	void Render();

	void HandleInput(int keyCode, int action);

	void SetGamerUUID(const char* gamerUUID);

	void ClearProducts();

	void AddProduct(Product product);

private:

	std::vector<std::string> m_productIds;

	PluginOuya* m_pluginOuya;

	std::vector<Product> m_pendingProducts;

	std::vector<TextButton*> m_products;

	TextButton* m_selectedProduct;

	bool m_uiInitialized;

	bool m_uiChanged;

	TextButton m_uiRequestGamerUUID;
	TextButton m_uiRequestProducts;
	TextButton m_uiRequestPurchase;
	TextButton m_uiRequestReceipts;
	TextButton m_uiPause;
	void* m_uiLabelFetch;

	TextButton* m_selectedButton;

	CallbacksFetchGamerUUID* m_callbacksFetchGamerUUID;
	CallbacksRequestProducts* m_callbacksRequestProducts;
	CallbacksRequestPurchase* m_callbacksRequestPurchase;
	CallbacksRequestReceipts* m_callbacksRequestReceipts;
};

#endif