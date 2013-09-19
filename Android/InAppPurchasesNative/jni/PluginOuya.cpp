#include "PluginOuya.h"

#include <android/log.h>
#include "engine.h"

#include <stdio.h>

PluginOuya::PluginOuya()
{
	Initialized = false;
}

void PluginOuya::Initialize()
{
	if (!Initialized)
	{
		//ouyaSetDeveloperId(plugin_ouya.developerId);
		LOGI("plugin_ouya is initialized");
	}
}

void PluginOuya::OuyaSetDeveloperId(const char* developerId)
{
	if (Initialized)
	{
		return;
	}

	char buffer[256];
	sprintf(buffer, "OuyaSetDeveloperId (%s)", developerId);
	LOGI(buffer);

	//ouyaSDK.ouyaSetDeveloperId(developerId);

	Initialized = true;
}

void PluginOuya::AsyncOuyaFetchGamerUUID(void* onSuccess, void* onFailure, void* onCancel)
{
	Initialize();

	LOGI("AsyncOuyaFetchGamerUUID");
}

void PluginOuya::AsyncOuyaRequestProducts (void* onSuccess, void* onFailure, void* onCancel, void* products)
{
	Initialize();

	LOGI("AsyncOuyaRequestProducts");
}

void PluginOuya::AsyncOuyaRequestPurchase (void* onSuccess, void* onFailure, void* onCancel, const char* purchasable)
{
	Initialize();

	LOGI("AsyncOuyaRequestPurchase");
}

void PluginOuya::AsyncOuyaRequestReceipts (void* onSuccess, void* onFailure, void* onCancel)
{
	Initialize();

	LOGI("AsyncOuyaRequestReceipts");
}