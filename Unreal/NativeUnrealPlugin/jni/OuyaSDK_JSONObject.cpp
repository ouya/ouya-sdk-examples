/*
* Copyright (C) 2012-2015 OUYA, Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#ifndef ECLIPSE
#include "LaunchPrivatePCH.h"
#endif

#include "OuyaSDK_JSONArray.h"
#include "OuyaSDK_JSONObject.h"

#include <android/log.h>
#include <jni.h>

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "org_json_JSONObject"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING false

namespace org_json_JSONObject
{
	JavaVM* JSONObject::_jvm = 0;
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

	int JSONObject::InitJNI(JavaVM* jvm)
	{
		_jvm = jvm;

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return JNI_ERR;
		}

		{
			const char* strClass = "org/json/JSONObject";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strClass);
#endif
				_jcJsonObject = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		return FindJNI();
	}

	int JSONObject::FindJNI()
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return JNI_ERR;
		}

		{
			const char* strMethod = "<init>";
			_mConstruct = env->GetMethodID(_jcJsonObject, strMethod, "()V");
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
			_mConstruct2 = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)V");
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
			_mGetDouble = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)D");
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
			_mGetInt = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)I");
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
			_mGetJsonArray = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Lorg/json/JSONArray;");
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
			_mGetJsonObject = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Lorg/json/JSONObject;");
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
			_mGetString = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
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
			_mHas = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;)Z");
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
			_mPut = env->GetMethodID(_jcJsonObject, strMethod, "(Ljava/lang/String;Ljava/lang/Object;)Lorg/json/JSONObject;");
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
			_mToString = env->GetMethodID(_jcJsonObject, strMethod, "()Ljava/lang/String;");
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

		return JNI_OK;
	}

	JSONObject::JSONObject(jobject jsonObject)
	{
		_instance = jsonObject;
	}

	jobject JSONObject::GetInstance() const
	{
		return _instance;
	}

	void JSONObject::Dispose() const
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (env &&
			_instance)
		{
			env->DeleteGlobalRef(_instance);
		}
	}	
	
	JSONObject::JSONObject()
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Invoke JSONObject()");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		jobject localRef = env->AllocObject(_jcJsonObject);
		if (localRef)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success allocate JSONObject");
#endif
			_instance = env->NewGlobalRef(localRef);
			env->DeleteLocalRef(localRef);
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

		env->CallVoidMethod(_instance, _mConstruct);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
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

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = env->AllocObject(_jcJsonObject);
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

		jstring arg1 = env->NewStringUTF(buffer.c_str());
		env->CallVoidMethod(_instance, _mConstruct2, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct JSONObject");
			return;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success constructed JSONObject");
#endif
		env->DeleteLocalRef(arg1);
	}

	double JSONObject::getDouble(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "double JSONObject::getDouble(const std::string& name) const");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jdouble result = env->CallDoubleMethod(_instance, _mGetDouble, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get double");
			return 0;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success get double: %f", (float)result);
#endif
		env->DeleteLocalRef(arg1);

		return (double)result;
	}

	int JSONObject::getInt(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "int JSONObject::getInt(const std::string& name) const");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jint result = env->CallBooleanMethod(_instance, _mGetInt, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get int");
			return 0;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success get int: %d", result);
#endif
		env->DeleteLocalRef(arg1);

		return result;
	}

	org_json_JSONArray::JSONArray JSONObject::getJSONArray(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "org_json_JSONArray::JSONArray JSONObject::getJSONArray(const std::string& name) const");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return org_json_JSONArray::JSONArray(0);
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jobject result = env->CallObjectMethod(_instance, _mGetJsonArray, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get json array");
			return org_json_JSONArray::JSONArray(0);
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success got json array");
#endif
		env->DeleteLocalRef(arg1);

		return org_json_JSONArray::JSONArray(result);
	}

	JSONObject JSONObject::getJSONObject(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JSONObject JSONObject::getJSONObject(const std::string& name) const");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JSONObject(0);
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jobject result = env->CallObjectMethod(_instance, _mGetJsonObject, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get json object");
			return JSONObject(0);
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success got json object");
#endif
		env->DeleteLocalRef(arg1);

		return JSONObject(result);
	}

	std::string JSONObject::getString(const std::string& name) const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "std::string JSONObject::getString(const std::string& name) const");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return std::string();
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jstring result = (jstring)env->CallObjectMethod(_instance, _mGetString, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get string");
			return std::string();
		}

		env->DeleteLocalRef(arg1);

		if (result)
		{
			const char* nativeString = env->GetStringUTFChars(result, 0);
			std::string retVal = std::string(nativeString);
			env->ReleaseStringUTFChars(result, nativeString);

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

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return false;
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jboolean result = env->CallBooleanMethod(_instance, _mHas, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to has");
			return false;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success has");
#endif
		env->DeleteLocalRef(arg1);

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

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JSONObject(0);
		}

		if (!env)
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

		jstring arg1 = env->NewStringUTF(name.c_str());
		jstring arg2 = env->NewStringUTF(value.c_str());
		jobject retVal = env->CallObjectMethod(_instance, _mPut, arg1, arg2);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to put");
			return JSONObject(0);
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success put");
#endif
		env->DeleteLocalRef(arg1);
		env->DeleteLocalRef(arg2);

		JSONObject result = JSONObject(retVal);
		return result;
	}

	std::string JSONObject::toString() const
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "std::string JSONObject::toString() const");
#endif
		FindJNI();

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return std::string();
		}

		if (!env)
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

		jstring retVal = (jstring)env->CallObjectMethod(_instance, _mToString);
		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to toString");
			return std::string();
		}

		if (retVal)
		{
			const char* nativeString = env->GetStringUTFChars(retVal, 0);
			std::string result = std::string(nativeString);
			env->ReleaseStringUTFChars(retVal, nativeString);

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
