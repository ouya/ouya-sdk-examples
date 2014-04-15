#include <android/log.h>
#include <jni.h>

#include "JSONArray.h"
#include "JSONObject.h"

#define LOG_TAG "org_json_JSONObject"

#define ENABLE_VERBOSE_LOGGING false

namespace org_json_JSONObject
{
	JNIEnv* JSONObject::_env = 0;
	jclass JSONObject::_jcJsonObject = 0;
	jmethodID JSONObject::_mConstruct = 0;
	jmethodID JSONObject::_mGetDouble = 0;
	jmethodID JSONObject::_mGetInt = 0;
	jmethodID JSONObject::_mGetJsonArray = 0;
	jmethodID JSONObject::_mGetJsonObject = 0;
	jmethodID JSONObject::_mGetString = 0;
	jmethodID JSONObject::_mHas = 0;

	int JSONObject::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "org/json/JSONObject";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			_jcJsonObject = env->FindClass(strClass);
			if (_jcJsonObject)
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

		{
			const char* strStringConstructor = "<init>";
			_mConstruct = env->GetMethodID(_jcJsonObject, strStringConstructor, "(Ljava/lang/String;)V");
			if (_mConstruct)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strStringConstructor);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strStringConstructor);
				return JNI_ERR;
			}
		}

		{
			const char* strJsonObjectGetDouble = "getDouble";
			_mGetDouble = env->GetMethodID(_jcJsonObject, strJsonObjectGetDouble, "(Ljava/lang/String;)D");
			if (_mGetDouble)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetDouble);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetDouble);
				return JNI_ERR;
			}
		}

		{
			const char* strJsonObjectGetInt = "getInt";
			_mGetInt = env->GetMethodID(_jcJsonObject, strJsonObjectGetInt, "(Ljava/lang/String;)I");
			if (_mGetInt)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetInt);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetInt);
				return JNI_ERR;
			}
		}

		{
			const char* strJsonObjectGetJsonArray = "getJSONArray";
			_mGetJsonArray = env->GetMethodID(_jcJsonObject, strJsonObjectGetJsonArray, "(Ljava/lang/String;)Lorg/json/JSONArray;");
			if (_mGetJsonArray)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetJsonArray);
#endif
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
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetJsonObject);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetJsonObject);
				return JNI_ERR;
			}
		}

		{
			const char* strJsonObjectGetString = "getString";
			_mGetString = env->GetMethodID(_jcJsonObject, strJsonObjectGetString, "(Ljava/lang/String;)Ljava/lang/String;");
			if (_mGetString)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectGetString);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectGetString);
				return JNI_ERR;
			}
		}

		{
			const char* strJsonObjectHas = "has";
			_mHas = env->GetMethodID(_jcJsonObject, strJsonObjectHas, "(Ljava/lang/String;)Z");
			if (_mHas)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strJsonObjectHas);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strJsonObjectHas);
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
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->AllocObject(_jcJsonObject);
		if (_instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success allocate JSONObject");
#endif
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

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success constructed JSONObject");
#endif
		_env->DeleteLocalRef(arg1);
	}

	double JSONObject::getDouble(const std::string& name)
	{
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

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jdouble result = _env->CallDoubleMethod(_instance, _mGetDouble, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get double");
			return 0;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get double: %f", (float)result);
#endif
		_env->DeleteLocalRef(arg1);

		return (double)result;
	}

	int JSONObject::getInt(const std::string& name)
	{
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

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jint result = _env->CallBooleanMethod(_instance, _mGetInt, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get int");
			return 0;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get int: %d", result);
#endif
		_env->DeleteLocalRef(arg1);

		return result;
	}

	org_json_JSONArray::JSONArray JSONObject::getJSONArray(const std::string& name)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return org_json_JSONArray::JSONArray(0);
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
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

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success got json array");
#endif
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

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
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

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success got json object");
#endif
		_env->DeleteLocalRef(arg1);

		return JSONObject(result);
	}

	std::string JSONObject::getString(const std::string& name)
	{
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

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jstring result = (jstring)_env->CallObjectMethod(_instance, _mGetString, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get string");
			return std::string();
		}

		_env->DeleteLocalRef(arg1);

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

	bool JSONObject::has(const std::string& name)
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

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jboolean result = _env->CallBooleanMethod(_instance, _mHas, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to has");
			return false;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success has");
#endif
		_env->DeleteLocalRef(arg1);

		return result;
	}
}