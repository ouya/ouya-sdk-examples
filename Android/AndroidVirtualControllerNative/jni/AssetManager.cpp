#include <android/log.h>
#include <jni.h>

#include "AssetManager.h"

#define LOG_TAG "android_content_res_AssetManager"

namespace android_content_res_AssetManager
{
	JNIEnv* AssetManager::_env = 0;
	jclass AssetManager::_jcAssetManager = 0;

	int AssetManager::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");

		const char* strAssetManagerClass = "android/content/res/AssetManager";
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strAssetManagerClass);
		_jcAssetManager = env->FindClass(strAssetManagerClass);
		if (_jcAssetManager)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerClass);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerClass);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	AssetManager::AssetManager(jobject assetManager)
	{
		_instance = assetManager;
	}

	AssetManager::AssetManager(const AssetManager& assetManager)
	{
		_instance = assetManager._instance;
	}

	AssetManager::~AssetManager()
	{
		_instance = 0;
	}
}