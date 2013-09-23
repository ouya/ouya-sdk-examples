#ifndef __PLUGIN_OUYA_H__
#define __PLUGIN_OUYA_H__

#include <nv_and_util/nv_native_app_glue.h>
#include <jni.h>

class PluginOuya
{
public:

	bool Initialized;

	PluginOuya();

	void FindClass(const char* tag, JNIEnv* env, const char* className, jclass* jc);

	void CacheClasses(const char* tag, JNIEnv* env);

	void SetApp(struct android_app* app);

	void Initialize();

	void SetDeveloperId(const char* developerId);

	void AsyncSetDeveloperId();

	void AsyncOuyaFetchGamerUUID(void* onSuccess, void* onFailure, void* onCancel);

	void AsyncOuyaRequestProducts(void* onSuccess, void* onFailure, void* onCancel, void* products);

	void AsyncOuyaRequestPurchase(void* onSuccess, void* onFailure, void* onCancel, const char* purchasable);

	void AsyncOuyaRequestReceipts(void* onSuccess, void* onFailure, void* onCancel);

private:

	struct android_app* m_app;

	const char* m_developerId;

	// cached references
	jclass jc_AsyncCppOuyaSetDeveloperId;
	jclass jc_AsyncCppOuyaFetchGamerUUID;
	jclass jc_AsyncCppOuyaRequestProducts;
	jclass jc_AsyncCppOuyaRequestPurchase;
	jclass jc_AsyncCppOuyaRequestReceipts;
};

#endif