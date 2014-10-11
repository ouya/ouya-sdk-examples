#if defined(ANDROID)
#include <android/log.h>
#include <jni.h>
#endif

#include "JSONArray.h"
#include "JSONObject.h"

#define LOG_TAG "org_json_JSONArray"

#define ENABLE_VERBOSE_LOGGING false

namespace org_json_JSONArray
{
	JNIEnv* JSONArray::_env = 0;
	jclass JSONArray::_jcJsonArray = 0;
	jmethodID JSONArray::_mConstruct = 0;
	jmethodID JSONArray::_mGetInt = 0;
	jmethodID JSONArray::_mGetJsonObject = 0;
	jmethodID JSONArray::_mGetString = 0;
	jmethodID JSONArray::_mLength = 0;

#if defined(ANDROID)

	int JSONArray::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "org/json/JSONArray";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			_jcJsonArray = env->FindClass(strClass);
			if (_jcJsonArray)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	void JSONArray::FindJNI()
	{
		{
			JavaVM* jvm;
			_env->GetJavaVM(&jvm);
			jvm->AttachCurrentThread(&_env, 0);
		}

		{
			const char* strStringConstructor = "<init>";
			_mConstruct = _env->GetMethodID(_jcJsonArray, strStringConstructor, "(Ljava/lang/String;)V");
			if (_mConstruct)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strStringConstructor);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strStringConstructor);
			}
		}

		{
			const char* strJsonObjectGetJsonObject = "getJSONObject";
			_mGetJsonObject = _env->GetMethodID(_jcJsonArray, strJsonObjectGetJsonObject, "(I)Lorg/json/JSONObject;");
			if (_mGetJsonObject)
			{
	#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetJsonObject);
	#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetJsonObject);
			}
		}

		{
			const char* strJsonObjectGetInt = "getInt";
			_mGetInt = _env->GetMethodID(_jcJsonArray, strJsonObjectGetInt, "(I)I");
			if (_mGetInt)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetInt);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetInt);
			}
		}

		{
			const char* strJsonObjectGetString = "getString";
			_mGetString = _env->GetMethodID(_jcJsonArray, strJsonObjectGetString, "(I)Ljava/lang/String;");
			if (_mGetString)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetString);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetString);
			}
		}

		{
			const char* strJsonObjectLength = "length";
			_mLength = _env->GetMethodID(_jcJsonArray, strJsonObjectLength, "()I");
			if (_mLength)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectLength);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectLength);
			}
		}
	}

	JSONArray::JSONArray(jobject jsonArray)
	{
		_instance = jsonArray;
	}

	JSONArray::JSONArray(const std::string& buffer)
	{
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->AllocObject(_jcJsonArray);
		if (_instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success allocate JSONArray");
#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to allocate JSONArray");
			return;
		}

		jstring arg1 = _env->NewStringUTF(buffer.c_str());
		_env->CallVoidMethod(_instance, _mConstruct, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct JSONArray");
			return;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success constructed JSONArray");
#endif
		_env->DeleteLocalRef(arg1);
	}

	org_json_JSONObject::JSONObject JSONArray::getJSONObject(int index) const
	{
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return org_json_JSONObject::JSONObject(0);
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return org_json_JSONObject::JSONObject(0);
		}

		jint arg1 = index;
		jobject result = _env->CallObjectMethod(_instance, _mGetJsonObject, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get json object");
			return org_json_JSONObject::JSONObject(0);
		}

		if (result)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success got json object");
#endif
			return org_json_JSONObject::JSONObject(result);
		}
		else
		{
			return org_json_JSONObject::JSONObject(0);
		}
	}

	int JSONArray::getInt(int index) const
	{
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return 0;
		}

		jint arg1 = index;
		jint result = _env->CallIntMethod(_instance, _mGetString, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get int");
			return 0;
		}
		else
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on get int: %d", result);
#endif
			return (int)result;
		}
	}

	std::string JSONArray::getString(int index) const
	{
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return std::string();
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return std::string();
		}

		jint arg1 = index;
		jstring result = (jstring)_env->CallObjectMethod(_instance, _mGetString, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get string");
			return std::string();
		}
		else
		{
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success on get string");
		}

		if (result)
		{
			const char* nativeString = _env->GetStringUTFChars(result, 0);
			std::string retVal = std::string(nativeString);
			_env->ReleaseStringUTFChars(result, nativeString);

#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Get string: %s", retVal.c_str());
#endif
			return retVal;
		}
		else
		{
			return std::string();
		}
	}

	int JSONArray::length() const
	{
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return 0;
		}

		jint result = _env->CallIntMethod(_instance, _mLength);
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on length: %d", result);
#endif
		return result;
	}
#else
	int JSONArray::InitJNI(JNIEnv* env)
	{
		return 0;
	}
	void JSONArray::FindJNI()
	{
	}
	JSONArray::JSONArray(jobject jsonArray)
	{
	}
	JSONArray::JSONArray(const std::string& json)
	{
	}
	jobject JSONArray::GetInstance() const
	{
		return NULL;
	}
	int JSONArray::length() const
	{
		return 0;
	}
	org_json_JSONObject::JSONObject JSONArray::getJSONObject(int index) const
	{
		return NULL;
	}
	int JSONArray::getInt(int index) const
	{
		return 0;
	}
	std::string JSONArray::getString(int index) const
	{
		return std::string();
	}
#endif
}