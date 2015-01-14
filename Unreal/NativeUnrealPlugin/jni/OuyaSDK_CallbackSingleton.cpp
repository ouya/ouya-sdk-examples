/*
* Copyright (C) 2012-2014 OUYA, Inc.
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

#include "OuyaSDK_CallbackSingleton.h"
#include "OuyaSDK_CallbacksRequestGamerInfo.h"
#include "OuyaSDK_CallbacksInitOuyaPlugin.h"
#include "OuyaSDK_CallbacksRequestProducts.h"
#include "OuyaSDK_CallbacksRequestPurchase.h"
#include "OuyaSDK_CallbacksRequestReceipts.h"
#include "OuyaSDK_GamerInfo.h"
#include "OuyaSDK_Product.h"
#include "OuyaSDK_Receipt.h"
#include "OuyaSDK_JSONArray.h"
#include "OuyaSDK_JSONObject.h"

#include <android/log.h>
#include <jni.h>
#include <stdio.h>
#include <string>

using namespace org_json_JSONObject;
using namespace org_json_JSONArray;

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "OuyaSDK_CallbackSingleton"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING true

namespace OuyaSDK
{
	CallbackSingleton* CallbackSingleton::m_instance = NULL;

	CallbackSingleton::CallbackSingleton()
	{
		CallbackSingleton::m_instance = this;

		m_callbacksInitOuyaPlugin = NULL;
		m_callbacksRequestGamerInfo = NULL;
		m_callbacksRequestProducts = NULL;
		m_callbacksRequestPurchase = NULL;
		m_callbacksRequestReceipts = NULL;
	}

	CallbackSingleton::~CallbackSingleton()
	{
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
		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess(JNIEnv* env, jobject thiz)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***********Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess***********");
#endif

			CallbacksInitOuyaPlugin* callback = CallbackSingleton::GetInstance()->m_callbacksInitOuyaPlugin;
			if (callback)
			{
				callback->OnSuccess();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksInitOuyaPlugin* callback = CallbackSingleton::GetInstance()->m_callbacksInitOuyaPlugin;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		//com.ODK.CallbacksRequestGamerInfo.CallbacksRequestGamerInfoOnSuccess
		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***********Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess***********");
#endif

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess: Returned to C: %s", strJsonData.c_str());
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

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestGamerInfo* callback = CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestGamerInfo* callback = CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess: Returned to C: %s", strJsonData.c_str());
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

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess: Returned to C: %s", strJsonData.c_str());
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

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel: Returned to C");
			//LOGI(buffer);

			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***********Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess***********");
#endif

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess: Returned to C: %s", strJsonData.c_str());
#endif

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

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel: Returned to C");
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
		///*
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Registering Native Method: %s className=%s signature=%s",
			methodName.c_str(),
			className.c_str(),
			signature.c_str());
#endif
		//*/

		//Register the Native method on the object
		*savedNativeMethod = { methodName.c_str(), signature.c_str(), method};

		// Find the class using the native
		jclass CallerJavaClass = env->FindClass(className.c_str());
		if (env->RegisterNatives(CallerJavaClass, savedNativeMethod, 1))
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Could not register native method: %s",
				methodName.c_str());
#endif
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

	int CallbackSingleton::InitJNI(JavaVM* jvm)
	{
		JNIEnv* env;
		if (jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return JNI_ERR;
		}

		//
		// Register Native Callbacks for InitOuyaPlugin
		//

		RegisterNativeMethod(env, "CallbacksInitOuyaPluginOnSuccess", "tv/ouya/sdk/unreal/CallbacksInitOuyaPlugin", "()V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess, &g_nativeCallbacksInitOuyaPluginOnSuccess);

		RegisterNativeMethod(env, "CallbacksInitOuyaPluginOnFailure", "tv/ouya/sdk/unreal/CallbacksInitOuyaPlugin", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure, &g_nativeCallbacksInitOuyaPluginOnFailure);

		//
		// Register Native Callbacks for RequestGamerInfo
		//

		RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnSuccess", "tv/ouya/sdk/unreal/CallbacksRequestGamerInfo", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess, &g_nativeCallbacksRequestGamerInfoOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnFailure", "tv/ouya/sdk/unreal/CallbacksRequestGamerInfo", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure, &g_nativeCallbacksRequestGamerInfoOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnCancel", "tv/ouya/sdk/unreal/CallbacksRequestGamerInfo", "()V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel, &g_nativeCallbacksRequestGamerInfoOnCancel);

		//
		// Register Native Callbacks for RequestProducts
		//

		RegisterNativeMethod(env, "CallbacksRequestProductsOnSuccess", "tv/ouya/sdk/unreal/CallbacksRequestProducts", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess, &g_nativeCallbacksRequestProductsOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestProductsOnFailure", "tv/ouya/sdk/unreal/CallbacksRequestProducts", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnFailure, &g_nativeCallbacksRequestProductsOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestProductsOnCancel", "tv/ouya/sdk/unreal/CallbacksRequestProducts", "()V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnCancel, &g_nativeCallbacksRequestProductsOnCancel);

		//
		// Register Native Callbacks for RequestPurchase
		//

		RegisterNativeMethod(env, "CallbacksRequestPurchaseOnSuccess", "tv/ouya/sdk/unreal/CallbacksRequestPurchase", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess, &g_nativeCallbacksRequestPurchaseOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestPurchaseOnFailure", "tv/ouya/sdk/unreal/CallbacksRequestPurchase", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure, &g_nativeCallbacksRequestPurchaseOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestPurchaseOnCancel", "tv/ouya/sdk/unreal/CallbacksRequestPurchase", "()V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel, &g_nativeCallbacksRequestPurchaseOnCancel);

		//
		// Register Native Callbacks for RequestReceipts
		//

		RegisterNativeMethod(env, "CallbacksRequestReceiptsOnSuccess", "tv/ouya/sdk/unreal/CallbacksRequestReceipts", "(Ljava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess, &g_nativeCallbacksRequestReceiptsOnSuccess);

		RegisterNativeMethod(env, "CallbacksRequestReceiptsOnFailure", "tv/ouya/sdk/unreal/CallbacksRequestReceipts", "(ILjava/lang/String;)V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure, &g_nativeCallbacksRequestReceiptsOnFailure);

		RegisterNativeMethod(env, "CallbacksRequestReceiptsOnCancel", "tv/ouya/sdk/unreal/CallbacksRequestReceipts", "()V",
			(void*)&Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel, &g_nativeCallbacksRequestReceiptsOnCancel);

		return JNI_OK;
	}
}
