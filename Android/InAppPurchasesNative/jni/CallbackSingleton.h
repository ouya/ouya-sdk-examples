#ifndef __CALLBACK_SINGLETON_H__
#define __CALLBACK_SINGLETON_H__

#include "UI.h"

class CallbacksFetchGamerUUID;

class CallbackSingleton
{
private:
	static CallbackSingleton* m_instance;
	UI* m_ui;

	CallbackSingleton();

public:

	static CallbackSingleton* GetInstance();

	void Initialize(UI* ui);

	UI* GetUI();

	CallbacksFetchGamerUUID* m_callbacksFetchGamerUUID;
	CallbacksRequestProducts* m_callbacksRequestProducts;
	CallbacksRequestPurchase* m_callbacksRequestPurchase;
	CallbacksRequestReceipts* m_callbacksRequestReceipts;
};

#endif