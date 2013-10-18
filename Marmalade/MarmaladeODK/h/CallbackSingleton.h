#ifndef __CALLBACK_SINGLETON_H__
#define __CALLBACK_SINGLETON_H__

#include <string>

class CallbacksFetchGamerUUID;
class CallbacksRequestProducts;
class CallbacksRequestPurchase;
class CallbacksRequestReceipts;

typedef struct s3eFetchGamerUuidSuccessEvent
{
	char m_gamerUUID[256];
} s3eFetchGamerUuidSuccessEvent;

typedef struct s3eFetchGamerUuidFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eFetchGamerUuidFailureEvent;

typedef struct s3eFetchGamerUuidCancelEvent
{
} s3eFetchGamerUuidCancelEvent;

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