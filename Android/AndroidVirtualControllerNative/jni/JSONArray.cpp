#include <android/log.h>
#include <jni.h>

#include "JSONArray.h"

#define LOG_TAG "org_json_JSONArray"

namespace org_json_JSONArray
{
	JNIEnv* JSONArray::_env = 0;
	jclass JSONArray::_jcJsonArray = 0;
	jmethodID JSONArray::_mLength = 0;

	int JSONArray::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");

		{
			const char* strClass = "org/json/JSONArray";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcJsonArray = env->FindClass(strClass);
			if (_jcJsonArray)
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
			const char* strJsonObjectLength = "length";
			_mLength = env->GetMethodID(_jcJsonArray, strJsonObjectLength, "()I");
			if (_mLength)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectLength);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectLength);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	JSONArray::JSONArray(jobject jsonArray)
	{
		_instance = jsonArray;
	}

	int JSONArray::length()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		jint result = _env->CallIntMethod(_instance, _mLength);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on length: %d", result);
		return result;
	}
}