#ifndef __UI_H__
#define __UI_H__

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

private:

	PluginOuya* m_pluginOuya;

	bool m_uiInitialized;

	TextButton m_uiRequestGamerUUID;
	TextButton m_uiRequestProducts;
	TextButton m_uiRequestPurchase;
	TextButton m_uiRequestReceipts;
	TextButton m_uiPause;

	TextButton* m_selectedButton;

};

#endif