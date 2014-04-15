#include <android/log.h>
#include <jni.h>

#include "ApplicationInfo.h"
#include "AssetManager.h"
#include "Context.h"

#define LOG_TAG "android_content_res_AssetManager"

using namespace android_content_pm_ApplicationInfo;

namespace android_content_Context
{
	JNIEnv* Context::_env = 0;
	jclass Context::_jcContext = 0;
	jmethodID Context::_mGetApplicationContext = 0;
	jmethodID Context::_mGetApplicationInfo = 0;
	jmethodID Context::_mGetAssets = 0;

	int Context::InitJNI(JNIEnv* env)
	{
		{
			const char* strContextClass = "android/content/Context";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strContextClass);
			_jcContext = env->FindClass(strContextClass);
			if (_jcContext)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strContextClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strContextClass);
				return JNI_ERR;
			}
		}

		{
			const char* strContextGetApplicationContext = "getApplicationContext";
			_mGetApplicationContext = env->GetMethodID(_jcContext, strContextGetApplicationContext, "()Landroid/content/Context;");
			if (_mGetApplicationContext)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strContextGetApplicationContext);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strContextGetApplicationContext);
				return JNI_ERR;
			}
		}

		{
			const char* strGetApplicationInfo = "getApplicationInfo";
			_mGetApplicationInfo = env->GetMethodID(_jcContext, strGetApplicationInfo, "()Landroid/content/pm/ApplicationInfo;");
			if (_mGetApplicationInfo)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strGetApplicationInfo);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strGetApplicationInfo);
				return JNI_ERR;
			}
		}

		{
			const char* strContextGetAssets = "getAssets";
			_mGetAssets = env->GetMethodID(_jcContext, strContextGetAssets, "()Landroid/content/res/AssetManager;");
			if (_mGetAssets)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strContextGetAssets);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strContextGetAssets);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	Context::Context(jobject context)
	{
		_instance = context;
	}

	Context Context::getApplicationContext()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return Context(0);
		}

		jobject result = _env->CallObjectMethod(_instance, _mGetApplicationContext);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get application context");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get application context");
		}
		return Context(result);
	}

	ApplicationInfo Context::getApplicationInfo()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return ApplicationInfo(0);
		}

		jobject result = _env->CallObjectMethod(_instance, _mGetApplicationInfo);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get application info");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get application info");
		}
		return ApplicationInfo(result);
	}

	android_content_res_AssetManager::AssetManager Context::getAssets()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return android_content_res_AssetManager::AssetManager(0);
		}

		jobject result = _env->CallObjectMethod(_instance, _mGetAssets);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get assets");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get assets");
		}
		return android_content_res_AssetManager::AssetManager(result);
	}
}