/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
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

#include "CallbackSingleton.h"
#include "PluginOuya.h"

#include "s3eEdk.h"
#include "s3eEdk_android.h"

#include <android/log.h>

#include <string>
#include <vector>

#define APP_NAME "MarmaladeODK_PluginOuya"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

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
	PluginOuya::PluginOuya()
	{
		jc_IMarmaladeOuyaActivity = NULL;
		jc_AsyncCppInitOuyaPlugin = NULL;
		jc_AsyncCppOuyaRequestGamerInfo = NULL;
		jc_AsyncCppOuyaRequestProducts = NULL;
		jc_AsyncCppOuyaRequestPurchase = NULL;
		jc_AsyncCppOuyaRequestReceipts = NULL;
	}

	void PluginOuya::FindClass(JNIEnv* env, const std::string& className, jclass* jc)
	{
		//std::string buffer;
		//buffer.append("Cache class: ");
		//buffer.append(className);
		//LOGI(buffer.c_str());

		jclass localRef = env->FindClass(className.c_str());
		(*jc) = (jclass)env->NewGlobalRef(localRef);

		//buffer = "";
		//buffer.append("Cached class: ");
		//buffer.append(className);
		//LOGI(buffer.c_str());
	}

	void PluginOuya::CacheClasses(JNIEnv* env)
	{
		FindClass(env, "tv/ouya/sdk/marmalade/IMarmaladeOuyaActivity", &jc_IMarmaladeOuyaActivity);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/marmalade/AsyncCppInitOuyaPlugin", &jc_AsyncCppInitOuyaPlugin);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/marmalade/AsyncCppOuyaRequestGamerInfo", &jc_AsyncCppOuyaRequestGamerInfo);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/marmalade/AsyncCppOuyaRequestProducts", &jc_AsyncCppOuyaRequestProducts);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/marmalade/AsyncCppOuyaRequestPurchase", &jc_AsyncCppOuyaRequestPurchase);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/marmalade/AsyncCppOuyaRequestReceipts", &jc_AsyncCppOuyaRequestReceipts);
		EXCEPTION_RETURN(env);

		CallbackSingleton::GetInstance()->RegisterNativeMethods();
	}

	void PluginOuya::AsyncInitOuyaPlugin(const std::string& jsonData)
	{
		//LOGI("AsyncInitOuyaPlugin");

		if (!jc_AsyncCppInitOuyaPlugin)
		{
			LOGI("jc_AsyncCppInitOuyaPlugin is not initialized");
			return;
		}

		JNIEnv* env = s3eEdkJNIGetEnv();

		if (!env)
		{
			LOGI("JNIEnv is invalid");
			return;
		}

		//LOGI("allocate the object");
		jobject objInitOuyaPlugin = env->AllocObject(jc_AsyncCppInitOuyaPlugin);
		EXCEPTION_RETURN(env);

		//LOGI("get the constructor");
		jmethodID constructor = env->GetMethodID(jc_AsyncCppInitOuyaPlugin, "<init>", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("construct the object");
		env->CallVoidMethod(objInitOuyaPlugin, constructor);
		EXCEPTION_RETURN(env);

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppInitOuyaPlugin, "invoke", "(Ljava/lang/String;)V");
		EXCEPTION_RETURN(env);

		jstring jsonString = env->NewStringUTF(jsonData.c_str());
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppInitOuyaPlugin, invokeMethod, jsonString);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestGamerInfo()
	{
		//LOGI("AsyncOuyaRequestGamerInfo");

		JNIEnv* env = s3eEdkJNIGetEnv();

		if (!env)
		{
			LOGI("JNIEnv is invalid");
			return;
		}

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestGamerInfo, "invoke", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestGamerInfo, invokeMethod);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestProducts(const std::vector<std::string>& productIds)
	{
		//LOGI("AsyncOuyaRequestProducts");

		JNIEnv* env = s3eEdkJNIGetEnv();

		if (!env)
		{
			LOGI("JNIEnv is invalid");
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

	void PluginOuya::AsyncOuyaRequestPurchase(const std::string& purchasable)
	{
		//LOGI("AsyncOuyaRequestPurchase");

		JNIEnv* env = s3eEdkJNIGetEnv();

		if (!env)
		{
			LOGI("JNIEnv is invalid");
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

	void PluginOuya::AsyncOuyaRequestReceipts()
	{
		//LOGI("AsyncOuyaRequestReceipts");

		JNIEnv* env = s3eEdkJNIGetEnv();

		if (!env)
		{
			LOGI("JNIEnv is invalid");
			return;
		}

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestReceipts, "invoke", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestReceipts, invokeMethod);
		EXCEPTION_RETURN(env);
	}	
}
