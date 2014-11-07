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
#include "CallbacksRequestGamerInfo.h"
#include "CallbacksInitOuyaPlugin.h"
#include "CallbacksRequestProducts.h"
#include "CallbacksRequestPurchase.h"
#include "CallbacksRequestReceipts.h"
#include "ExtensionGamerInfo.h"
#include "ExtensionProduct.h"
#include "ExtensionReceipt.h"
#include "JSONArray.h"
#include "JSONObject.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"

#include <android/log.h>
#include <jni.h>
#include <stdio.h>
#include <string>

using namespace org_json_JSONObject;
using namespace org_json_JSONArray;

#define APP_NAME "MarmaladeODK_callbacks"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

namespace OuyaSDK
{
	CallbackSingleton* CallbackSingleton::m_instance = NULL;

	CallbackSingleton::CallbackSingleton()
	{
		CallbackSingleton::m_instance = this;

		m_callbacksInitOuyaPlugin = new CallbacksInitOuyaPlugin();
		m_callbacksRequestGamerInfo = new CallbacksRequestGamerInfo();
		m_callbacksRequestProducts = new CallbacksRequestProducts();
		m_callbacksRequestPurchase = new CallbacksRequestPurchase();
		m_callbacksRequestReceipts = new CallbacksRequestReceipts();
	}

	CallbackSingleton::~CallbackSingleton()
	{
		delete m_callbacksInitOuyaPlugin;
		delete m_callbacksRequestGamerInfo;
		delete m_callbacksRequestProducts;
		delete m_callbacksRequestPurchase;
		delete m_callbacksRequestReceipts;
	}

	CallbackSingleton* CallbackSingleton::GetInstance()
	{
		if (!CallbackSingleton::m_instance)
		{
			CallbackSingleton::m_instance = new CallbackSingleton();
		}

		return CallbackSingleton::m_instance;
	}

	extern "C"
	{
		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess(JNIEnv* env, jobject thiz)
		{
			LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess***********");

			CallbacksInitOuyaPlugin* callback = CallbackSingleton::GetInstance()->m_callbacksInitOuyaPlugin;
			if (callback)
			{
				callback->OnSuccess();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksInitOuyaPlugin* callback = CallbackSingleton::GetInstance()->m_callbacksInitOuyaPlugin;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		//com.ODK.CallbacksRequestGamerInfo.CallbacksRequestGamerInfoOnSuccess
		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess: Returned to C: %s", strJsonData.c_str());
			//LOGI(buffer);

			JSONObject jsonObject = JSONObject(strJsonData.c_str());

			// Parse example data
			OuyaSDK::GamerInfo newGamerInfo;
			newGamerInfo.ParseJSON(jsonObject);

			CallbacksRequestGamerInfo* callback = CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo;
			if (callback)
			{
				callback->OnSuccess(newGamerInfo);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestGamerInfo* callback = CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestGamerInfo* callback = CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess: Returned to C: %s", strJsonData.c_str());
			//LOGI(buffer);

			JSONArray jsonArray = JSONArray(strJsonData.c_str());

			std::vector<Product> products;

			for (int i = 0; i < jsonArray.length(); i++)
			{
				OuyaSDK::Product newProduct;
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				newProduct.ParseJSON(jsonObject);

				products.push_back(newProduct);
			}

			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnSuccess(products);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess: Returned to C: %s", strJsonData.c_str());
			//LOGI(buffer);

			JSONObject jsonObject = JSONObject(strJsonData.c_str());

			OuyaSDK::Product product;
			product.ParseJSON(jsonObject);

			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnSuccess(product);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			std::string buffer = "Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess: Returned to C: ";
			buffer.append(strJsonData);
			LOGI(buffer.c_str());

			//LOGI("Parsing JSON Data");

			JSONArray jsonArray = JSONArray(strJsonData.c_str());

			std::vector<Receipt> receipts;

			for (int i = 0; i < jsonArray.length(); i++)
			{
				OuyaSDK::Receipt newReceipt;
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				newReceipt.ParseJSON(jsonObject);

				receipts.push_back(newReceipt);
			}

			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnSuccess(receipts);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnCancel();
			}
		}
	}

	void RegisterNativeMethod(JNIEnv* env, std::string methodName, std::string className, std::string signature, void* method, JNINativeMethod* savedNativeMethod)
	{
		std::string msg = "Registering Native Method: ";
		msg.append(methodName);
		msg.append(" className=");
		msg.append(className);
		msg.append(" signature=");
		msg.append(signature);
		LOGI(msg.c_str());

		//Register the Native method on the object
		*savedNativeMethod = { methodName.c_str(), signature.c_str(), method};

		// Find the class using the native
		jclass CallerJavaClass = env->FindClass(className.c_str());
		if (env->RegisterNatives(CallerJavaClass, savedNativeMethod, 1))
		{
			std::string error = "Could not register native method: ";
			error.append(methodName);
			LOGI(error.c_str());
			return;
		}
	}

	//
	// Native Callbacks for InitOuyaPlugin
	//

	JNINativeMethod g_nativeCallbacksInitOuyaPluginOnSuccess;
	JNINativeMethod g_nativeCallbacksInitOuyaPluginOnFailure;

	//
	// Native Callbacks for RequestGamerInfo
	//

	JNINativeMethod g_nativeCallbacksRequestGamerInfoOnSuccess;
	JNINativeMethod g_nativeCallbacksRequestGamerInfoOnFailure;
	JNINativeMethod g_nativeCallbacksRequestGamerInfoOnCancel;

	//
	// Native Callbacks for RequestProducts
	//

	JNINativeMethod g_nativeCallbacksRequestProductsOnSuccess;
	JNINativeMethod g_nativeCallbacksRequestProductsOnFailure;
	JNINativeMethod g_nativeCallbacksRequestProductsOnCancel;

	//
	// Native Callbacks for RequestPurchase
	//

	JNINativeMethod g_nativeCallbacksRequestPurchaseOnSuccess;
	JNINativeMethod g_nativeCallbacksRequestPurchaseOnFailure;
	JNINativeMethod g_nativeCallbacksRequestPurchaseOnCancel;

	//
	// Native Callbacks for RequestReceipts
	//

	JNINativeMethod g_nativeCallbacksRequestReceiptsOnSuccess;
	JNINativeMethod g_nativeCallbacksRequestReceiptsOnFailure;
	JNINativeMethod g_nativeCallbacksRequestReceiptsOnCancel;

	void CallbackSingleton::RegisterNativeMethods()
	{
		JNIEnv* env = s3eEdkJNIGetEnv();

		//
		// Register Native Callbacks for InitOuyaPlugin
		//

		RegisterNativeMethod(env, "CallbacksInitOuyaPluginOnSuccess", "tv/ouya/sdk/marmalade/CallbacksInitOuyaPlugin", "()V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess, &g_nativeCallbacksInitOuyaPluginOnSuccess);

		RegisterNativeMethod(env, "CallbacksInitOuyaPluginOnFailure", "tv/ouya/sdk/marmalade/CallbacksInitOuyaPlugin", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure, &g_nativeCallbacksInitOuyaPluginOnFailure);

		//
		// Register Native Callbacks for RequestGamerInfo
		//

		RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnSuccess", "tv/ouya/sdk/marmalade/CallbacksRequestGamerInfo", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess, &g_nativeCallbacksRequestGamerInfoOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnFailure", "tv/ouya/sdk/marmalade/CallbacksRequestGamerInfo", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure, &g_nativeCallbacksRequestGamerInfoOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnCancel", "tv/ouya/sdk/marmalade/CallbacksRequestGamerInfo", "()V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel, &g_nativeCallbacksRequestGamerInfoOnCancel);

		//
		// Register Native Callbacks for RequestProducts
		//

		RegisterNativeMethod(env, "CallbacksRequestProductsOnSuccess", "tv/ouya/sdk/marmalade/CallbacksRequestProducts", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess, &g_nativeCallbacksRequestProductsOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestProductsOnFailure", "tv/ouya/sdk/marmalade/CallbacksRequestProducts", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnFailure, &g_nativeCallbacksRequestProductsOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestProductsOnCancel", "tv/ouya/sdk/marmalade/CallbacksRequestProducts", "()V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestProducts_CallbacksRequestProductsOnCancel, &g_nativeCallbacksRequestProductsOnCancel);

		//
		// Register Native Callbacks for RequestPurchase
		//

		RegisterNativeMethod(env, "CallbacksRequestPurchaseOnSuccess", "tv/ouya/sdk/marmalade/CallbacksRequestPurchase", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess, &g_nativeCallbacksRequestPurchaseOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestPurchaseOnFailure", "tv/ouya/sdk/marmalade/CallbacksRequestPurchase", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure, &g_nativeCallbacksRequestPurchaseOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestPurchaseOnCancel", "tv/ouya/sdk/marmalade/CallbacksRequestPurchase", "()V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel, &g_nativeCallbacksRequestPurchaseOnCancel);

		//
		// Register Native Callbacks for RequestReceipts
		//

		RegisterNativeMethod(env, "CallbacksRequestReceiptsOnSuccess", "tv/ouya/sdk/marmalade/CallbacksRequestReceipts", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess, &g_nativeCallbacksRequestReceiptsOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestReceiptsOnFailure", "tv/ouya/sdk/marmalade/CallbacksRequestReceipts", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure, &g_nativeCallbacksRequestReceiptsOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestReceiptsOnCancel", "tv/ouya/sdk/marmalade/CallbacksRequestReceipts", "()V",
			(void*)&Java_tv_ouya_sdk_marmalade_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel, &g_nativeCallbacksRequestReceiptsOnCancel);
	}
}
