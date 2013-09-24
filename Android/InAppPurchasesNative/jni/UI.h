#ifndef __UI_H__
#define __UI_H__

#include "CallbacksFetchGamerUUID.h"
#include "CallbacksRequestProducts.h"
#include "CallbacksRequestPurchase.h"
#include "CallbacksRequestReceipts.h"
#include "PluginOuya.h"
#include "TextButton.h"

class UI
{
public:
	
	UI();

	void Initialize(PluginOuya* pluginOuya);

	bool InitUI();

	void Destroy();

	void Resize(int w, int h);

	void Render();

	void HandleInput(int keyCode, int action);

	void SetGamerUUID(const char* gamerUUID);

private:

	PluginOuya* m_pluginOuya;

	bool m_uiInitialized;

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