#include "JSONArray.h"
#include "JSONObject.h"

#include <android/log.h>
#include <jni.h>

#define LOG_TAG "org_json_JSONObject"

#define ENABLE_VERBOSE_LOGGING false

namespace org_json_JSONObject
{
	JNIEnv* JSONObject::_env = 0;
	jclass JSONObject::_jcJsonObject = 0;
	jmethodID JSONObject::_mConstruct = 0;
	jmethodID JSONObject::_mConstruct2 = 0;
	jmethodID JSONObject::_mGetDouble = 0;
	jmethodID JSONObject::_mGetInt = 0;
	jmethodID JSONObject::_mGetJsonArray = 0;
	jmethodID JSONObject::_mGetJsonObject = 0;
	jmethodID JSONObject::_mGetString = 0;
	jmethodID JSONObject::_mHas = 0;
	jmethodID JSONObject::_mPut = 0;
	jmethodID JSONObject::_mToString = 0;

	int JSONObject::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "org/json/JSONObject";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = (jclass)env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strClass);
#endif
				_jcJsonObject = localRef;
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

	void JSONObject::FindJNI()
	{
		{
			JavaVM* jvm;
			_env->GetJavaVM(&jvm);
			jvm->AttachCurrentThread(&_env, 0);
		}

		{
			const char* strMethod = "<init>";
			_mConstruct = _env->GetMethodID(_jcJsonObject, strMethod, "()V");
			if (_mConstruct)
			{
	#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
	#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "<init>";
			_mConstruct2 = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)V");
			if (_mConstruct2)
			{
	#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
	#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "getDouble";
			_mGetDouble = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)D");
			if (_mGetDouble)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "getInt";
			_mGetInt = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)I");
			if (_mGetInt)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "getJSONArray";
			_mGetJsonArray = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Lorg/json/JSONArray;");
			if (_mGetJsonArray)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "getJSONObject";
			_mGetJsonObject = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Lorg/json/JSONObject;");
			if (_mGetJsonObject)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "getString";
			_mGetString = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
			if (_mGetString)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "has";
			_mHas = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Z");
			if (_mHas)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "put";
			_mPut = _env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;Ljava/lang/Object;)Lorg/json/JSONObject;");
			if (_mPut)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}

		{
			const char* strMethod = "toString";
			_mToString = _env->GetMethodID(_jcJsonObject, strMethod, "()Ljava/lang/String;");
			if (_mToString)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}
	}

	JSONObject::JSONObject(jobject jsonObject)
	{
		_instance = jsonObject;
	}

	jobject JSONObject::GetInstance() const
	{
		return _instance;
	}
	
	JSONObject::JSONObject()
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Invoke JSONObject()");
#endif
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->AllocObject(_jcJsonObject);
		if (_instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success allocate JSONObject");
#endif
			_instance = _env->NewGlobalRef(_instance);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to allocate JSONObject");
			return;
		}

		if (!_mConstruct)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mConstruct is not valid!");
			return;
		}

		_env->CallVoidMethod(_instance, _mConstruct);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct JSONObject");
			return;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success constructed JSONObject");
#endif
	}

	JSONObject::JSONObject(const std::string& buffer)
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Invoke JSONObject(const std::string& buffer)");
#endif
		FindJNI();

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->AllocObject(_jcJsonObject);
		if (_instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success allocate JSONObject");
#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to allocate JSONObject");
			return;
		}

		if (!_mConstruct2)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mConstruct2 is not valid!");
			return;
		}

		jstring arg1 = _env->NewStringUTF(buffer.c_str());
		_env->CallVoidMethod(_instance, _mConstruct2, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct JSONObject");
			return;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success constructed JSONObject");
#endif
		_env->DeleteLocalRef(arg1);
	}

	double JSONObject::getDouble(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "double JSONObject::getDouble(const std::string& name) const");
#endif
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

		if (!_mGetDouble)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mGetDouble is not valid!");
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
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success get double: %f", (float)result);
#endif
		_env->DeleteLocalRef(arg1);

		return (double)result;
	}

	int JSONObject::getInt(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "int JSONObject::getInt(const std::string& name) const");
#endif
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

		if (!_mGetInt)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mGetInt is not valid!");
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
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success get int: %d", result);
#endif
		_env->DeleteLocalRef(arg1);

		return result;
	}

	org_json_JSONArray::JSONArray JSONObject::getJSONArray(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "org_json_JSONArray::JSONArray JSONObject::getJSONArray(const std::string& name) const");
#endif
		FindJNI();

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

		if (!_mGetJsonArray)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mGetJsonArray is not valid!");
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
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success got json array");
#endif
		_env->DeleteLocalRef(arg1);

		return org_json_JSONArray::JSONArray(result);
	}

	JSONObject JSONObject::getJSONObject(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JSONObject JSONObject::getJSONObject(const std::string& name) const");
#endif
		FindJNI();

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

		if (!_mGetJsonObject)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mGetJsonObject is not valid!");
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
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success got json object");
#endif
		_env->DeleteLocalRef(arg1);

		return JSONObject(result);
	}

	std::string JSONObject::getString(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "std::string JSONObject::getString(const std::string& name) const");
#endif
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

		if (!_mGetString)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mGetString is not valid!");
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
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Get string: %s", retVal.c_str());
#endif
			return retVal;
		}
		else
		{
			return std::string();
		}
	}

	bool JSONObject::has(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "bool JSONObject::has(const std::string& name) const");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "has name=%s", name.c_str());
#endif
		FindJNI();

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

		if (!_mHas)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mHas is not valid!");
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
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success has");
#endif
		_env->DeleteLocalRef(arg1);

		return result;
	}

	JSONObject JSONObject::put(const std::string& name, const std::string& value) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JSONObject JSONObject::put(const std::string& name, const std::string& value) const");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "put name=%s", name.c_str());
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "put value=%s", value.c_str());
#endif
		FindJNI();

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

		if (!_mPut)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mPut is not valid!");
			return JSONObject(0);
		}

		jstring arg1 = _env->NewStringUTF(name.c_str());
		jstring arg2 = _env->NewStringUTF(value.c_str());
		jobject retVal = _env->CallObjectMethod(_instance, _mPut, arg1, arg2);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to put");
			return JSONObject(0);
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success put");
#endif
		_env->DeleteLocalRef(arg1);
		_env->DeleteLocalRef(arg2);

		JSONObject result = JSONObject(retVal);
		return result;
	}

	std::string JSONObject::toString() const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "std::string JSONObject::toString() const");
#endif
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

		if (!_mToString)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_mToString is not valid!");
			return std::string();
		}

		jstring retVal = (jstring)_env->CallObjectMethod(_instance, _mToString);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to toString");
			return std::string();
		}

		if (retVal)
		{
			const char* nativeString = _env->GetStringUTFChars(retVal, 0);
			std::string result = std::string(nativeString);
			_env->ReleaseStringUTFChars(retVal, nativeString);

#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "toString: %s", result.c_str());
#endif
			return result;
		}
		else
		{
			return std::string();
		}
	}
}
