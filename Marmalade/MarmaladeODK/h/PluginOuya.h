#ifndef __PLUGIN_OUYA_H__
#define __PLUGIN_OUYA_H__

#include <string>
#include <vector>

class _jclass;
class CallbacksFetchGamerUUID;
class CallbacksRequestProducts;
class CallbacksRequestPurchase;
class CallbacksRequestReceipts;

typedef struct _JNIEnv JNIEnv;
typedef _jclass* jclass;

namespace OuyaSDK
{
	class PluginOuya
	{
	public:

		PluginOuya();

		void FindClass(JNIEnv* env, const std::string& className, jclass* jc);

		void CacheClasses(JNIEnv* env);

		void SetApp(struct android_app* app);

		void AsyncSetDeveloperId(const std::string& developerId);

		void AsyncOuyaFetchGamerUUID();

		void AsyncOuyaRequestProducts(const std::vector<std::string>& productIds);

		void AsyncOuyaRequestPurchase(const std::string& purchasable);

		void AsyncOuyaRequestReceipts();

	private:

		// cached references
		jclass jc_AsyncCppOuyaSetDeveloperId;
		jclass jc_AsyncCppOuyaFetchGamerUUID;
		jclass jc_AsyncCppOuyaRequestProducts;
		jclass jc_AsyncCppOuyaRequestPurchase;
		jclass jc_AsyncCppOuyaRequestReceipts;
	};
}

#endif