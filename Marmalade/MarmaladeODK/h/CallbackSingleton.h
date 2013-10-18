#ifndef __CALLBACK_SINGLETON_H__
#define __CALLBACK_SINGLETON_H__

#include <string>

class CallbacksFetchGamerUUID;
class CallbacksRequestProducts;
class CallbacksRequestPurchase;
class CallbacksRequestReceipts;

namespace OuyaSDK
{
	class CallbackSingleton
	{
	private:
		static CallbackSingleton* m_instance;

		CallbackSingleton();
		~CallbackSingleton();

	public:

		static CallbackSingleton* GetInstance();

		void RegisterNativeMethods();

		CallbacksFetchGamerUUID* m_callbacksFetchGamerUUID;
		CallbacksRequestProducts* m_callbacksRequestProducts;
		CallbacksRequestPurchase* m_callbacksRequestPurchase;
		CallbacksRequestReceipts* m_callbacksRequestReceipts;
	};
};

#endif