#include <android/log.h>
#include <jni.h>

#include "AssetManager.h"

#define LOG_TAG "android_content_res_AssetManager"

namespace android_content_res_AssetManager
{
	JNIEnv* AssetManager::_env = 0;
	jclass AssetManager::_jcAssetManager = 0;
	jmethodID AssetManager::_mList = 0;

	int AssetManager::InitJNI(JNIEnv* env)
	{
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

		const char* strAssetManagerList = "list";
		_mList = env->GetMethodID(_jcAssetManager, strAssetManagerList, "(Ljava/lang/String;)[Ljava/lang/String;");
		if (_mList)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerList);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerList);
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
		if (_instance)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Destroy Object");
			_env->DeleteLocalRef(_instance);
		}
	}

	std::vector<std::string> AssetManager::list(const java_lang_String::String& path) const
	{
		std::vector<std::string> retVal;

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jstring arg1 = path.GetInstance();
		jobject result = _env->CallObjectMethod(_instance, _mList, arg1);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on list path");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to list path");
			return retVal;
		}

		jsize const length = _env->GetArrayLength((jarray)result);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "List returned %d items", length);

		for (jsize index(0); index < length; ++index)
		{
			jstring element = (jstring)_env->GetObjectArrayElement((jobjectArray)result, index);
			const char* nativeString = _env->GetStringUTFChars(element, 0);
			//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, nativeString);
			std::string entry = nativeString;
			retVal.push_back(entry);
			_env->ReleaseStringUTFChars(element, nativeString);
		}

		return retVal;
	}
}