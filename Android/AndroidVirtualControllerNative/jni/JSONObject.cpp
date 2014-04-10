#include <android/log.h>
#include <jni.h>

#include "JSONObject.h"

#define LOG_TAG "org_json_JSONObject"

namespace org_json_JSONObject
{
	JNIEnv* JSONObject::_env = 0;
	jclass JSONObject::_jcJsonObject = 0;
	jmethodID JSONObject::_mConstruct = 0;

	int JSONObject::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");

		{
			const char* strClass = "org/json/JSONObject";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcJsonObject = env->FindClass(strClass);
			if (_jcJsonObject)
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
			const char* strStringConstructor = "<init>";
			_mConstruct = env->GetMethodID(_jcJsonObject, strStringConstructor, "(Ljava/lang/String;)V");
			if (_mConstruct)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strStringConstructor);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strStringConstructor);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	JSONObject::JSONObject(jobject jsonObject)
	{
		_instance = jsonObject;
	}

	JSONObject::JSONObject(const std::string& buffer)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "********HERE************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "************************");

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->AllocObject(_jcJsonObject);
		if (_instance)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success allocate JSONObject");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to allocate JSONObject");
			return;
		}

		jstring arg1 = _env->NewStringUTF(buffer.c_str());
		_env->CallVoidMethod(_jcJsonObject, _mConstruct, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct JSONObject");
			return;
		}

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success constructed JSONObject");
		_env->DeleteLocalRef(arg1);
	}
}