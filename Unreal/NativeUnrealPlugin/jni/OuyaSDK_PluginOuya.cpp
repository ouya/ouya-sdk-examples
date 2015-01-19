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

#include "OuyaSDK_CallbackSingleton.h"
#include "OuyaSDK_JSONArray.h"
#include "OuyaSDK_JSONObject.h"
#include "OuyaSDK_PluginOuya.h"

#include <jni.h>
#include <android/log.h>

#include <string>
#include <vector>

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "tv_ouya_sdk_OuyaPlugin"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING false

using namespace org_json_JSONArray;
using namespace org_json_JSONObject;

// If we cause an exception in JNI, we print the exception info to
// the log, we clear the exception to avoid a pending-exception
// crash, and we force the function to return.
#define EXCEPTION_RETURN(env) \
	if (env->ExceptionOccurred()) { \
		env->ExceptionDescribe(); \
		env->ExceptionClear(); \
		return; \
	}

namespace OuyaSDK
{
	JavaVM* PluginOuya::_jvm = 0;
	jclass PluginOuya::jc_IUnrealOuyaActivity = 0;
	jclass PluginOuya::jc_UnrealOuyaPlugin = 0;
	jclass PluginOuya::jc_AsyncCppInitOuyaPlugin = 0;
	jclass PluginOuya::jc_AsyncCppOuyaRequestGamerInfo = 0;
	jclass PluginOuya::jc_AsyncCppOuyaRequestProducts = 0;
	jclass PluginOuya::jc_AsyncCppOuyaRequestPurchase = 0;
	jclass PluginOuya::jc_AsyncCppOuyaRequestReceipts = 0;

	int PluginOuya::InitJNI(JavaVM* jvm)
	{
		_jvm = jvm;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/IUnrealOuyaActivity";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_IUnrealOuyaActivity = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/UnrealOuyaPlugin";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_UnrealOuyaPlugin = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/AsyncCppInitOuyaPlugin";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_AsyncCppInitOuyaPlugin = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/AsyncCppOuyaRequestGamerInfo";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_AsyncCppOuyaRequestGamerInfo = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/AsyncCppOuyaRequestProducts";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_AsyncCppOuyaRequestProducts = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/AsyncCppOuyaRequestPurchase";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_AsyncCppOuyaRequestPurchase = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClass = "tv/ouya/sdk/unreal/AsyncCppOuyaRequestReceipts";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_AsyncCppOuyaRequestReceipts = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		if (CallbackSingleton::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (JSONArray::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (JSONObject::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		return JNI_OK;
	}

	void PluginOuya::AsyncInitOuyaPlugin(const std::string& jsonData, CallbacksInitOuyaPlugin* callbacks)
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "AsyncInitOuyaPlugin");
#endif

		CallbackSingleton::GetInstance()->m_callbacksInitOuyaPlugin = callbacks;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		if (!jc_AsyncCppInitOuyaPlugin)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "jc_AsyncCppInitOuyaPlugin is not initialized");
			return;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "allocate the object");
#endif
		jobject objInitOuyaPlugin = env->AllocObject(jc_AsyncCppInitOuyaPlugin);
		EXCEPTION_RETURN(env);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "get the constructor");
#endif
		jmethodID constructor = env->GetMethodID(jc_AsyncCppInitOuyaPlugin, "<init>", "()V");
		EXCEPTION_RETURN(env);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "construct the object");
#endif
		env->CallVoidMethod(objInitOuyaPlugin, constructor);
		EXCEPTION_RETURN(env);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "get the invoke method");
#endif
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppInitOuyaPlugin, "invoke", "(Ljava/lang/String;)V");
		EXCEPTION_RETURN(env);

		jstring jsonString = env->NewStringUTF(jsonData.c_str());
		EXCEPTION_RETURN(env);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "execute the invoke method");
#endif
		env->CallStaticVoidMethod(jc_AsyncCppInitOuyaPlugin, invokeMethod, jsonString);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestGamerInfo(CallbacksRequestGamerInfo* callbacks)
	{
		//LOGI("AsyncOuyaRequestGamerInfo");

		CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo = callbacks;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestGamerInfo, "invoke", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestGamerInfo, invokeMethod);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestProducts(const std::vector<std::string>& productIds, CallbacksRequestProducts* callbacks)
	{
		//LOGI("AsyncOuyaRequestProducts");

		CallbackSingleton::GetInstance()->m_callbacksRequestProducts = callbacks;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		//LOGI("get string class");

		// Get a class reference for java.lang.String
		jclass classString = env->FindClass("java/lang/String");

		//LOGI("create array");
		jobjectArray products = env->NewObjectArray(productIds.size(), classString, NULL);

		//LOGI("populate items");
		for (unsigned int i = 0; i < productIds.size(); ++i) {
			env->SetObjectArrayElement(products, i, env->NewStringUTF(productIds[i].c_str()));
		}


		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestProducts, "invoke", "([Ljava/lang/String;)V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestProducts, invokeMethod, products);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestPurchase(const std::string& purchasable, CallbacksRequestPurchase* callbacks)
	{
		//LOGI("AsyncOuyaRequestPurchase");

		CallbackSingleton::GetInstance()->m_callbacksRequestPurchase = callbacks;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		//LOGI("Allocate purchasable String");
		jstring purchasableString = env->NewStringUTF(purchasable.c_str());
		EXCEPTION_RETURN(env);

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestPurchase, "invoke", "(Ljava/lang/String;)V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestPurchase, invokeMethod, purchasableString);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestReceipts(CallbacksRequestReceipts* callbacks)
	{
		//LOGI("AsyncOuyaRequestReceipts");

		CallbackSingleton::GetInstance()->m_callbacksRequestReceipts = callbacks;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestReceipts, "invoke", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestReceipts, invokeMethod);
		EXCEPTION_RETURN(env);
	}

	bool PluginOuya::updateSafeArea(float amount)
	{
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

		jmethodID method;

		{
			const char* strMethod = "updateSafeArea";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(F)Z");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return false;
			}
		}

		return env->CallStaticBooleanMethod(jc_UnrealOuyaPlugin, method, amount);
	}

	bool PluginOuya::shutdown()
	{
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

		jmethodID method;

		{
			const char* strMethod = "shutdown";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "()Z");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return false;
			}
		}

		return env->CallStaticBooleanMethod(jc_UnrealOuyaPlugin, method);
	}
}
