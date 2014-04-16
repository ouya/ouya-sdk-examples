#include <android/log.h>
#include <jni.h>

#include "AssetFileDescriptor.h"

#define LOG_TAG "android_content_res_AssetFileDescriptor"

namespace android_content_res_AssetFileDescriptor
{
	JNIEnv* AssetFileDescriptor::_env = 0;
	jclass AssetFileDescriptor::_jcAssetFileDescriptor = 0;
	jmethodID AssetFileDescriptor::_mGetLength = 0;

	int AssetFileDescriptor::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/content/res/AssetFileDescriptor";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcAssetFileDescriptor = env->FindClass(strClass);
			if (_jcAssetFileDescriptor)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strGetLength = "getLength";
			_mGetLength = env->GetMethodID(_jcAssetFileDescriptor, strGetLength, "()J");
			if (_mGetLength)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strGetLength);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strGetLength);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	AssetFileDescriptor::AssetFileDescriptor(jobject assetFileDescriptor)
	{
		_instance = assetFileDescriptor;
	}

	AssetFileDescriptor::AssetFileDescriptor(const AssetFileDescriptor& rhs)
	{
		_instance = rhs._instance;
	}

	jobject AssetFileDescriptor::GetInstance() const
	{
		return _instance;
	}

	long AssetFileDescriptor::getLength()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return false;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return false;
		}

		jlong result = _env->CallLongMethod(_instance, _mGetLength);
		return result;
	}
}