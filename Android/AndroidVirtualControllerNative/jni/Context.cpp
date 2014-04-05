#include <android/log.h>
#include <jni.h>

#include "AssetManager.h"
#include "Context.h"

#define LOG_TAG "android_content_res_AssetManager"

namespace android_content_Context
{
	JNIEnv* Context::_env = 0;
	jclass Context::_jcContext = 0;
	jmethodID Context::_mGetApplicationContext = 0;
	jmethodID Context::_mGetAssets = 0;

	int Context::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");

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

		const char* strActivityGetApplicationContext = "getApplicationContext";
		_mGetApplicationContext = env->GetMethodID(_jcContext, strActivityGetApplicationContext, "()Landroid/content/Context;");
		if (_mGetApplicationContext)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strActivityGetApplicationContext);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strActivityGetApplicationContext);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	Context::Context(jobject context)
	{
		_instance = context;
	}

	Context::Context(const Context& context)
	{
		_instance = context._instance;
	}

	Context::~Context()
	{
		_instance = 0;
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