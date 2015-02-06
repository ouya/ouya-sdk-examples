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

#include "OuyaSDK_Bitmap.h"
#include "OuyaSDK_BitmapConfig.h"
#include "OuyaSDK_Bundle.h"
#include "OuyaSDK_CallbackSingleton.h"
#include "OuyaSDK_Color.h"
#include "OuyaSDK_InputStream.h"
#include "OuyaSDK_JSONArray.h"
#include "OuyaSDK_JSONObject.h"
#include "OuyaSDK_OuyaContent.h"
#include "OuyaSDK_OuyaMod.h"
#include "OuyaSDK_OuyaModEditor.h"
#include "OuyaSDK_OuyaModScreenshot.h"
#include "OuyaSDK_OutputStream.h"
#include "OuyaSDK_PluginOuya.h"
#include "OuyaSDK_String.h"

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
#define ENABLE_VERBOSE_LOGGING true

using namespace android_graphics_Bitmap;
using namespace android_graphics_Bitmap_Config;
using namespace android_graphics_Color;
using namespace android_os_Bundle;
using namespace java_io_InputStream;
using namespace java_io_OutputStream;
using namespace org_json_JSONArray;
using namespace org_json_JSONObject;
using namespace std;
using namespace tv_ouya_console_api_content_OuyaContent;
using namespace tv_ouya_console_api_content_OuyaMod;
using namespace tv_ouya_console_api_content_OuyaModEditor;
using namespace tv_ouya_console_api_content_OuyaModScreenshot;

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
	jclass PluginOuya::jc_AsyncCppOuyaContentInit = 0;

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

		{
			const char* strClass = "tv/ouya/sdk/unreal/AsyncCppOuyaContentInit";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				jc_AsyncCppOuyaContentInit = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		if (Bitmap::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (Bundle::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (CallbackSingleton::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (Color::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (Config::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (InputStream::InitJNI(jvm) == JNI_ERR)
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

		if (OutputStream::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (OuyaContent::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (OuyaMod::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (OuyaModEditor::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (OuyaModScreenshot::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		if (java_lang_String::String::InitJNI(jvm) == JNI_ERR)
		{
			return JNI_ERR;
		}

		return JNI_OK;
	}

	void PluginOuya::AsyncInitOuyaPlugin(const string& jsonData, CallbacksInitOuyaPlugin* callbacks)
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

	void PluginOuya::AsyncOuyaRequestProducts(const vector<string>& productIds, CallbacksRequestProducts* callbacks)
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

	void PluginOuya::AsyncOuyaRequestPurchase(const string& purchasable, CallbacksRequestPurchase* callbacks)
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

	void PluginOuya::AsyncOuyaContentInit(CallbacksContentInit* callbacks)
	{
		//LOGI("AsyncOuyaContentInit");

		CallbackSingleton::GetInstance()->m_callbacksContentInit = callbacks;

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
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaContentInit, "invoke", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaContentInit, invokeMethod);
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

	void PluginOuya::InitializeContent()
	{
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

		jmethodID method;

		{
			const char* strMethod = "InitializeContent";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "()V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method);
	}

	void PluginOuya::contentDelete(OuyaMod ouyaMod, CallbacksContentDelete* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentDelete = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "contentDelete";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method, ouyaMod.GetInstance());
	}

	void PluginOuya::contentDownload(OuyaMod ouyaMod, CallbacksContentDownload* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentDownload = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "contentDownload";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method, ouyaMod.GetInstance());
	}

	void PluginOuya::contentPublish(OuyaMod ouyaMod, CallbacksContentPublish* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentPublish = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "contentPublish";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method, ouyaMod.GetInstance());
	}

	void PluginOuya::contentUnpublish(OuyaMod ouyaMod, CallbacksContentUnpublish* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentUnpublish = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "contentUnpublish";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method, ouyaMod.GetInstance());
	}

	void PluginOuya::saveOuyaMod(tv_ouya_console_api_content_OuyaModEditor::OuyaModEditor ouyaModEditor,
		OuyaMod ouyaMod,
		CallbacksContentSave* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentSave = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "saveOuyaMod";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod$Editor;Ltv/ouya/console/api/content/OuyaMod;)V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method, ouyaModEditor.GetInstance(), ouyaMod.GetInstance());
	}

	void PluginOuya::getOuyaContentInstalled(CallbacksContentSearchInstalled* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentSearchInstalled = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "getOuyaContentInstalled";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "()V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method);
	}

	void PluginOuya::getOuyaContentPublished(string sortMethod, CallbacksContentSearchPublished* callbacks)
	{
		CallbackSingleton::GetInstance()->m_callbacksContentSearchPublished = callbacks;

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

		jmethodID method;

		{
			const char* strMethod = "getOuyaContentPublished";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ljava/lang/String;)V");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		jstring arg1 = env->NewStringUTF(sortMethod.c_str());
		env->CallStaticVoidMethod(jc_UnrealOuyaPlugin, method, arg1);
		env->DeleteLocalRef(arg1);
	}

	float PluginOuya::getFloat(jobject fFloat)
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}

		if (!jc_UnrealOuyaPlugin)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "jc_UnrealOuyaPlugin is not initialized");
			return 0;
		}

		if (!fFloat)
		{
			return 0;
		}

		jmethodID method;
		{
			const char* strMethod = "getFloat";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ljava/lang/Float;)F");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return 0;
			}
		}

		jobject arg1 = fFloat;
		float result = env->CallStaticFloatMethod(jc_UnrealOuyaPlugin, method, arg1);
		return result;
	}

	vector<Bitmap> PluginOuya::getBitmapArray(jobject listBitmaps)
	{
		vector<Bitmap> results;

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return results;
		}

		if (!jc_UnrealOuyaPlugin)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "jc_UnrealOuyaPlugin is not initialized");
			return results;
		}

		if (!listBitmaps)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "listBitmaps reference is null");
			return results;
		}

		jmethodID method;
		{
			const char* strMethod = "getBitmapArray";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ljava/util/List;)[Landroid/graphics/Bitmap;");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return results;
			}
		}

		jobject arg1 = listBitmaps;
		jobject result = (jarray)env->CallStaticObjectMethod(jc_UnrealOuyaPlugin, method, arg1);
		if (!result)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "getBitmapArray returned null");
			return results;
		}

		jsize length = env->GetArrayLength((jarray)result);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "getBitmapArray returned %d elements", length);
#endif

		for (jsize index(0); index < length; ++index)
		{
			jobject localRef = (jobject)env->GetObjectArrayElement((jobjectArray)result, index);
			jobject globalRef = (jobject)env->NewGlobalRef(localRef);
			env->DeleteLocalRef(localRef);
			Bitmap bitmap = Bitmap(globalRef);
			results.push_back(bitmap);
		}

		env->DeleteLocalRef(result);

		return results;
	}

	vector<OuyaModScreenshot> PluginOuya::getOuyaModScreenshotArray(jobject listOuyaModScreenshots)
	{
		vector<OuyaModScreenshot> results;

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return results;
		}

		if (!jc_UnrealOuyaPlugin)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "jc_UnrealOuyaPlugin is not initialized");
			return results;
		}

		if (!listOuyaModScreenshots)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "listOuyaModScreenshots reference is null");
			return results;
		}

		jmethodID method;
		{
			const char* strMethod = "getOuyaModScreenshotArray";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ljava/util/List;)[Ltv/ouya/console/api/content/OuyaModScreenshot;");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return results;
			}
		}

		jobject arg1 = listOuyaModScreenshots;
		jobject result = (jarray)env->CallStaticObjectMethod(jc_UnrealOuyaPlugin, method, arg1);
		if (!result)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "getOuyaModScreenshotArray returned null");
			return results;
		}

		jsize length = env->GetArrayLength((jarray)result);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "getBitmapArray returned %d elements", length);
#endif

		for (jsize index(0); index < length; ++index)
		{
			jobject localRef = (jobject)env->GetObjectArrayElement((jobjectArray)result, index);
			jobject globalRef = (jobject)env->NewGlobalRef(localRef);
			env->DeleteLocalRef(localRef);
			OuyaModScreenshot ouyaModScreenshot = OuyaModScreenshot(globalRef);
			results.push_back(ouyaModScreenshot);
		}

		env->DeleteLocalRef(result);

		return results;
	}

	vector<string> PluginOuya::getStringArray(jobject listStrings)
	{
		vector<string> results;

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return results;
		}

		if (!jc_UnrealOuyaPlugin)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "jc_UnrealOuyaPlugin is not initialized");
			return results;
		}

		jmethodID method;
		{
			const char* strMethod = "getStringArray";
			method = env->GetStaticMethodID(jc_UnrealOuyaPlugin, strMethod, "(Ljava/util/List;)[Ljava/lang/String;");
			if (method)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return results;
			}
		}

		jobject arg1 = listStrings;
		jobject result = (jarray)env->CallStaticObjectMethod(jc_UnrealOuyaPlugin, method, arg1);
		if (!result)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "getStringArray returned null");
			return results;
		}

		jsize length = env->GetArrayLength((jarray)result);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "getStringArray returned %d elements", length);
#endif

		for (jsize index(0); index < length; ++index)
		{
			jstring localRef = (jstring)env->GetObjectArrayElement((jobjectArray)result, index);
			const char* nativeString = env->GetStringUTFChars(localRef, 0);
			std::string element = nativeString;
			env->ReleaseStringUTFChars(localRef, nativeString);
			env->DeleteLocalRef(localRef);
			results.push_back(element);
		}

		env->DeleteLocalRef(result);

		return results;
	}
}
