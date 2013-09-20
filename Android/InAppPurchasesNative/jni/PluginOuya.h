#ifndef __PLUGIN_OUYA_H__
#define __PLUGIN_OUYA_H__

#include <nv_and_util/nv_native_app_glue.h>
#include <jni.h>

class PluginOuya
{
public:

	bool Initialized;

	jclass m_jcAsyncOuyaSetDeveloperId;

	PluginOuya();

	void SetApp(struct android_app* app);

	void Initialize();

	void SetDeveloperId(const char* developerId);

	void AsyncOuyaFetchGamerUUID(void* onSuccess, void* onFailure, void* onCancel);

	void AsyncOuyaRequestProducts (void* onSuccess, void* onFailure, void* onCancel, void* products);

	void AsyncOuyaRequestPurchase (void* onSuccess, void* onFailure, void* onCancel, const char* purchasable);

	void AsyncOuyaRequestReceipts (void* onSuccess, void* onFailure, void* onCancel);

private:

	struct android_app* m_app;

	const char* m_developerId;

	void OuyaSetDeveloperId();
};

#endif