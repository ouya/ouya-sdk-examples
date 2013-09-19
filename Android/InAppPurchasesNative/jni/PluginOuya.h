#ifndef __PLUGIN_OUYA_H__
#define __PLUGIN_OUYA_H__

class PluginOuya
{
public:

	bool Initialized;

	PluginOuya();

	void Initialize();

	void OuyaSetDeveloperId(const char* developerId);

	void AsyncOuyaFetchGamerUUID(void* onSuccess, void* onFailure, void* onCancel);

	void AsyncOuyaRequestProducts (void* onSuccess, void* onFailure, void* onCancel, void* products);

	void AsyncOuyaRequestPurchase (void* onSuccess, void* onFailure, void* onCancel, const char* purchasable);

	void AsyncOuyaRequestReceipts (void* onSuccess, void* onFailure, void* onCancel);
};

#endif