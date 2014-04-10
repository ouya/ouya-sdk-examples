#include <android/log.h>
#include <jni.h>

#include "JSONArray.h"
#include "JSONObject.h"

#define LOG_TAG "org_json_JSONObject"

namespace org_json_JSONObject
{
	JNIEnv* JSONObject::_env = 0;
	jclass JSONObject::_jcJsonObject = 0;
	jmethodID JSONObject::_mConstruct = 0;
	jmethodID JSONObject::_mGetJsonArray = 0;
	jmethodID JSONObject::_mGetJsonObject = 0;

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

		{
			const char* strJsonObjectGetJsonArray = "getJSONArray";
			_mGetJsonArray = env->GetMethodID(_jcJsonObject, strJsonObjectGetJsonArray, "(Ljava/lang/String;)Lorg/json/JSONArray;");
			if (_mGetJsonArray)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetJsonArray);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetJsonArray);
				return JNI_ERR;
			}
		}

		{
			const char* strJsonObjectGetJsonObject = "getJSONObject";
			_mGetJsonObject = env->GetMethodID(_jcJsonObject, strJsonObjectGetJsonObject, "(Ljava/lang/String;)Lorg/json/JSONObject;");
			if (_mGetJsonObject)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetJsonObject);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetJsonObject);
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
		_env->CallVoidMethod(_instance, _mConstruct, arg1);

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

	org_json_JSONArray::JSONArray JSONObject::getJSONArray(const std::string& name)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return org_json_JSONArray::JSONArray(0);
		}

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jobject result = _env->CallObjectMethod(_instance, _mGetJsonArray, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get json array");
			return org_json_JSONArray::JSONArray(0);
		}

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success got json array");
		_env->DeleteLocalRef(arg1);

		return org_json_JSONArray::JSONArray(result);
	}

	JSONObject JSONObject::getJSONObject(const std::string& name)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return JSONObject(0);
		}

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jobject result = _env->CallObjectMethod(_instance, _mGetJsonObject, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get json object");
			return JSONObject(0);
		}

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success got json object");
		_env->DeleteLocalRef(arg1);

		return JSONObject(result);
	}
}