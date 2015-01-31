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

#include "OuyaSDK_Bundle.h"
#include "OuyaSDK_CallbackSingleton.h"
#include "OuyaSDK_CallbacksContentDelete.h"
#include "OuyaSDK_CallbacksContentDownload.h"
#include "OuyaSDK_CallbacksContentInit.h"
#include "OuyaSDK_CallbacksContentPublish.h"
#include "OuyaSDK_CallbacksContentSave.h"
#include "OuyaSDK_CallbacksContentSearchInstalled.h"
#include "OuyaSDK_CallbacksContentSearchPublished.h"
#include "OuyaSDK_CallbacksContentUnpublish.h"
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
#include "OuyaSDK_OuyaMod.h"

#include <android/log.h>
#include <jni.h>
#include <stdio.h>
#include <string>

using namespace android_os_Bundle;
using namespace org_json_JSONObject;
using namespace org_json_JSONArray;
using namespace tv_ouya_console_api_content_OuyaMod;

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

CallbackSingleton::CallbackSingleton() {
	CallbackSingleton::m_instance = this;

	m_callbacksContentDelete = NULL;
	m_callbacksContentDownload = NULL;
	m_callbacksContentInit = NULL;
	m_callbacksContentPublish = NULL;
	m_callbacksContentSave = NULL;
	m_callbacksContentSearchInstalled = NULL;
	m_callbacksContentSearchPublished = NULL;
	m_callbacksContentUnpublish = NULL;
	m_callbacksInitOuyaPlugin = NULL;
	m_callbacksRequestGamerInfo = NULL;
	m_callbacksRequestProducts = NULL;
	m_callbacksRequestPurchase = NULL;
	m_callbacksRequestReceipts = NULL;
}

CallbackSingleton::~CallbackSingleton() {
}

CallbackSingleton* CallbackSingleton::GetInstance() {
	if (!CallbackSingleton::m_instance) {
		CallbackSingleton::m_instance = new CallbackSingleton();
	}

	return CallbackSingleton::m_instance;
}

extern "C" {
	/// CallbacksInitOuyaPlugin

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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksInitOuyaPlugin is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksInitOuyaPlugin is not set");
		}
	}

	/// CallbacksRequestGamerInfo

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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestGamerInfo is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestGamerInfo is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestGamerInfo is not set");
		}
	}

	/// CallbacksRequestProducts

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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestProducts is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestProducts is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestProducts is not set");
		}
	}

	/// CallbacksRequestPurchase

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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestPurchase is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestPurchase is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestPurchase is not set");
		}
	}

	/// CallbacksRequestReceipts

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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestReceipts is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestReceipts is not set");
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
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksRequestReceipts is not set");
		}
	}

	/// CallbacksContentDelete

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentDelete_CallbacksContentDeleteOnDeleteFailed(JNIEnv* env, jobject thiz, jobject ouyaMod, jint code, jstring reason)
	{
		std::string strReason = env->GetStringUTFChars(reason, NULL);

		CallbacksContentDelete* callback = CallbackSingleton::GetInstance()->m_callbacksContentDelete;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnDeleteFailed(newOuyaMod, code, strReason);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentDelete is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentDelete_CallbacksContentDeleteOnDeleted(JNIEnv* env, jobject thiz, jobject ouyaMod)
	{
		CallbacksContentDelete* callback = CallbackSingleton::GetInstance()->m_callbacksContentDelete;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnDeleted(newOuyaMod);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentDelete is not set");
		}
	}

	/// CallbacksContentDownload

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentDownload_CallbacksContentDownloadOnProgress(JNIEnv* env, jobject thiz, jobject ouyaMod, jint progress)
	{
		CallbacksContentDownload* callback = CallbackSingleton::GetInstance()->m_callbacksContentDownload;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnProgress(newOuyaMod, progress);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentDownload is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentDownload_CallbacksContentDownloadOnFailed(JNIEnv* env, jobject thiz, jobject ouyaMod)
	{
		CallbacksContentDownload* callback = CallbackSingleton::GetInstance()->m_callbacksContentDownload;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnFailed(newOuyaMod);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentDownload is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentDownload_CallbacksContentDownloadOnComplete(JNIEnv* env, jobject thiz, jobject ouyaMod)
	{
		CallbacksContentDownload* callback = CallbackSingleton::GetInstance()->m_callbacksContentDownload;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnComplete(newOuyaMod);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentDownload is not set");
		}
	}

	/// CallbacksContentInit

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentInit_CallbacksContentInitOnInitialized(JNIEnv* env, jobject thiz)
	{
		CallbacksContentInit* callback = CallbackSingleton::GetInstance()->m_callbacksContentInit;
		if (callback)
		{
			callback->OnInitialized();
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentInit is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentInit_CallbacksContentInitOnDestroyed(JNIEnv* env, jobject thiz)
	{
		CallbacksContentInit* callback = CallbackSingleton::GetInstance()->m_callbacksContentInit;
		if (callback)
		{
			callback->OnDestroyed();
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentInit is not set");
		}
	}

	/// CallbacksContentSave

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentSave_CallbacksContentSaveOnError(JNIEnv* env, jobject thiz, jobject ouyaMod, jint code, jstring reason)
	{
		std::string strReason = env->GetStringUTFChars(reason, NULL);

		CallbacksContentSave* callback = CallbackSingleton::GetInstance()->m_callbacksContentSave;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnError(newOuyaMod, code, strReason);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentSave is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentSave_CallbacksContentSaveOnSuccess(JNIEnv* env, jobject thiz, jobject ouyaMod)
	{
		CallbacksContentSave* callback = CallbackSingleton::GetInstance()->m_callbacksContentSave;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnSuccess(newOuyaMod);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentSave is not set");
		}
	}

	/// CallbacksContentPublish

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentPublish_CallbacksContentPublishOnError(JNIEnv* env, jobject thiz, jobject ouyaMod, jint code, jstring reason, jobject bundle)
	{
		std::string strReason = env->GetStringUTFChars(reason, NULL);

		CallbacksContentPublish* callback = CallbackSingleton::GetInstance()->m_callbacksContentPublish;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			Bundle newBundle = Bundle(bundle);
			callback->OnError(newOuyaMod, code, strReason, newBundle);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentPublish is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentPublish_CallbacksContentPublishOnSuccess(JNIEnv* env, jobject thiz, jobject ouyaMod)
	{
		CallbacksContentPublish* callback = CallbackSingleton::GetInstance()->m_callbacksContentPublish;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnSuccess(newOuyaMod);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentPublish is not set");
		}
	}

	/// CallbacksContentSearchInstalled

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentSearchInstalled_CallbacksContentSearchInstalledOnError(JNIEnv* env, jobject thiz, jint code, jstring reason)
	{
		std::string strReason = env->GetStringUTFChars(reason, NULL);

		CallbacksContentSearchInstalled* callback = CallbackSingleton::GetInstance()->m_callbacksContentSearchInstalled;
		if (callback)
		{
			callback->OnError(code, strReason);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentSearchInstalled is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentSearchInstalled_CallbacksContentSearchInstalledOnResults(JNIEnv* env, jobject thiz, jobjectArray ouyaMods, jint count)
	{
		CallbacksContentSearchInstalled* callback = CallbackSingleton::GetInstance()->m_callbacksContentSearchInstalled;
		if (callback)
		{
			std::vector<OuyaMod> newOuyaMods;
			jsize length = env->GetArrayLength(ouyaMods);
			for (jsize index(0); index < length; ++index)
			{
				jobject localRef = (jobject)env->GetObjectArrayElement(ouyaMods, index);
				jobject globalRef = (jobject)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
				OuyaMod newOuyaMod = OuyaMod(globalRef);
				newOuyaMods.push_back(newOuyaMod);

			}
			callback->OnResults(newOuyaMods, count);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentSearchInstalled is not set");
		}
	}

	/// CallbacksContentSearchPublished

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentSearchPublished_CallbacksContentSearchPublishedOnError(JNIEnv* env, jobject thiz, jint code, jstring reason)
	{
		std::string strReason = env->GetStringUTFChars(reason, NULL);

		CallbacksContentSearchPublished* callback = CallbackSingleton::GetInstance()->m_callbacksContentSearchPublished;
		if (callback)
		{
			callback->OnError(code, strReason);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentSearchPublished is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentSearchPublished_CallbacksContentSearchPublishedOnResults(JNIEnv* env, jobject thiz, jobjectArray ouyaMods, jint count)
	{
		CallbacksContentSearchPublished* callback = CallbackSingleton::GetInstance()->m_callbacksContentSearchPublished;
		if (callback)
		{
			std::vector<OuyaMod> newOuyaMods;
			jsize length = env->GetArrayLength(ouyaMods);
			for (jsize index(0); index < length; ++index)
			{
				jobject localRef = (jobject)env->GetObjectArrayElement(ouyaMods, index);
				jobject globalRef = (jobject)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
				OuyaMod newOuyaMod = OuyaMod(globalRef);
				newOuyaMods.push_back(newOuyaMod);

			}
			callback->OnResults(newOuyaMods, count);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentSearchPublished is not set");
		}
	}

	/// CallbacksContentUnpublish

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentUnpublish_CallbacksContentUnpublishOnError(JNIEnv* env, jobject thiz, jobject ouyaMod, jint code, jstring reason)
	{
		std::string strReason = env->GetStringUTFChars(reason, NULL);

		CallbacksContentUnpublish* callback = CallbackSingleton::GetInstance()->m_callbacksContentUnpublish;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnError(newOuyaMod, code, strReason);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentUnpublish is not set");
		}
	}

	JNIEXPORT void JNICALL Java_tv_ouya_sdk_unreal_CallbacksContentUnpublish_CallbacksContentUnpublishOnSuccess(JNIEnv* env, jobject thiz, jobject ouyaMod)
	{
		CallbacksContentUnpublish* callback = CallbackSingleton::GetInstance()->m_callbacksContentUnpublish;
		if (callback)
		{
			OuyaMod newOuyaMod = OuyaMod(ouyaMod);
			callback->OnSuccess(newOuyaMod);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "CallbacksContentUnpublish is not set");
		}
	}
}

void RegisterNativeMethod(JNIEnv* env, std::string methodName,
	std::string className, std::string signature, void* method,
	JNINativeMethod* savedNativeMethod) {
///*
#if ENABLE_VERBOSE_LOGGING
__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG,
		"Registering Native Method: %s className=%s signature=%s",
		methodName.c_str(), className.c_str(), signature.c_str());
#endif
//*/

//Register the Native method on the object
*savedNativeMethod = {methodName.c_str(), signature.c_str(), method};

// Find the class using the native
jclass CallerJavaClass = env->FindClass(className.c_str());
if (env->RegisterNatives(CallerJavaClass, savedNativeMethod, 1)) {
#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_ERROR, LOG_TAG,
			"Could not register native method: %s", methodName.c_str());
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

//
// Native Callbacks for ContentDelete
//

JNINativeMethod g_nativeCallbacksContentDeleteOnDeleteFailed;
JNINativeMethod g_nativeCallbacksContentDeleteOnDeleted;

//
// Native Callbacks for ContentDownload
//

JNINativeMethod g_nativeCallbacksContentDownloadOnProgress;
JNINativeMethod g_nativeCallbacksContentDownloadOnFailed;
JNINativeMethod g_nativeCallbacksContentDownloadOnComplete;

//
// Native Callbacks for ContentInit
//

JNINativeMethod g_nativeCallbacksContentInitOnInitialized;
JNINativeMethod g_nativeCallbacksContentInitOnDestroyed;

//
// Native Callbacks for ContentSave
//

JNINativeMethod g_nativeCallbacksContentSaveOnError;
JNINativeMethod g_nativeCallbacksContentSaveOnSuccess;

//
// Native Callbacks for ContentPublish
//

JNINativeMethod g_nativeCallbacksContentPublishOnError;
JNINativeMethod g_nativeCallbacksContentPublishOnSuccess;

//
// Native Callbacks for ContentSearchInstalled
//

JNINativeMethod g_nativeCallbacksContentSearchInstalledOnError;
JNINativeMethod g_nativeCallbacksContentSearchInstalledOnResults;

//
// Native Callbacks for ContentSearchPublished
//

JNINativeMethod g_nativeCallbacksContentSearchPublishedOnError;
JNINativeMethod g_nativeCallbacksContentSearchPublishedOnResults;

//
// Native Callbacks for ContentUnpublish
//

JNINativeMethod g_nativeCallbacksContentUnpublishOnError;
JNINativeMethod g_nativeCallbacksContentUnpublishOnSuccess;

int CallbackSingleton::InitJNI(JavaVM* jvm) {
JNIEnv* env;
if (jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
	__android_log_print(ANDROID_LOG_ERROR, LOG_TAG,
			"Failed to get JNI environment!");
	return JNI_ERR;
}

if (!env) {
	__android_log_print(ANDROID_LOG_ERROR, LOG_TAG,
			"JNI must be initialized with a valid environment!");
	return JNI_ERR;
}

//
// Register Native Callbacks for InitOuyaPlugin
//

RegisterNativeMethod(env, "CallbacksInitOuyaPluginOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksInitOuyaPlugin", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnSuccess,
		&g_nativeCallbacksInitOuyaPluginOnSuccess);

RegisterNativeMethod(env, "CallbacksInitOuyaPluginOnFailure",
		"tv/ouya/sdk/unreal/CallbacksInitOuyaPlugin", "(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksInitOuyaPlugin_CallbacksInitOuyaPluginOnFailure,
		&g_nativeCallbacksInitOuyaPluginOnFailure);

//
// Register Native Callbacks for RequestGamerInfo
//

RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksRequestGamerInfo", "(Ljava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnSuccess,
		&g_nativeCallbacksRequestGamerInfoOnSuccess);

RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnFailure",
		"tv/ouya/sdk/unreal/CallbacksRequestGamerInfo",
		"(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnFailure,
		&g_nativeCallbacksRequestGamerInfoOnFailure);

RegisterNativeMethod(env, "CallbacksRequestGamerInfoOnCancel",
		"tv/ouya/sdk/unreal/CallbacksRequestGamerInfo", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestGamerInfo_CallbacksRequestGamerInfoOnCancel,
		&g_nativeCallbacksRequestGamerInfoOnCancel);

//
// Register Native Callbacks for RequestProducts
//

RegisterNativeMethod(env, "CallbacksRequestProductsOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksRequestProducts", "(Ljava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess,
		&g_nativeCallbacksRequestProductsOnSuccess);

RegisterNativeMethod(env, "CallbacksRequestProductsOnFailure",
		"tv/ouya/sdk/unreal/CallbacksRequestProducts", "(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnFailure,
		&g_nativeCallbacksRequestProductsOnFailure);

RegisterNativeMethod(env, "CallbacksRequestProductsOnCancel",
		"tv/ouya/sdk/unreal/CallbacksRequestProducts", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestProducts_CallbacksRequestProductsOnCancel,
		&g_nativeCallbacksRequestProductsOnCancel);

//
// Register Native Callbacks for RequestPurchase
//

RegisterNativeMethod(env, "CallbacksRequestPurchaseOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksRequestPurchase", "(Ljava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess,
		&g_nativeCallbacksRequestPurchaseOnSuccess);

RegisterNativeMethod(env, "CallbacksRequestPurchaseOnFailure",
		"tv/ouya/sdk/unreal/CallbacksRequestPurchase", "(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure,
		&g_nativeCallbacksRequestPurchaseOnFailure);

RegisterNativeMethod(env, "CallbacksRequestPurchaseOnCancel",
		"tv/ouya/sdk/unreal/CallbacksRequestPurchase", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel,
		&g_nativeCallbacksRequestPurchaseOnCancel);

//
// Register Native Callbacks for RequestReceipts
//

RegisterNativeMethod(env, "CallbacksRequestReceiptsOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksRequestReceipts", "(Ljava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess,
		&g_nativeCallbacksRequestReceiptsOnSuccess);

RegisterNativeMethod(env, "CallbacksRequestReceiptsOnFailure",
		"tv/ouya/sdk/unreal/CallbacksRequestReceipts", "(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure,
		&g_nativeCallbacksRequestReceiptsOnFailure);

RegisterNativeMethod(env, "CallbacksRequestReceiptsOnCancel",
		"tv/ouya/sdk/unreal/CallbacksRequestReceipts", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel,
		&g_nativeCallbacksRequestReceiptsOnCancel);

//
// Register Native Callbacks for ContentDelete
//

RegisterNativeMethod(env, "CallbacksContentDeleteOnDeleteFailed",
		"tv/ouya/sdk/unreal/CallbacksContentDelete",
		"(Ltv/ouya/console/api/content/OuyaMod;ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentDelete_CallbacksContentDeleteOnDeleteFailed,
		&g_nativeCallbacksContentDeleteOnDeleteFailed);

RegisterNativeMethod(env, "CallbacksContentDeleteOnDeleted",
		"tv/ouya/sdk/unreal/CallbacksContentDelete",
		"(Ltv/ouya/console/api/content/OuyaMod;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentDelete_CallbacksContentDeleteOnDeleted,
		&g_nativeCallbacksContentDeleteOnDeleted);

//
// Register Native Callbacks for ContentDownload
//

RegisterNativeMethod(env, "CallbacksContentDownloadOnProgress",
		"tv/ouya/sdk/unreal/CallbacksContentDownload",
		"(Ltv/ouya/console/api/content/OuyaMod;I)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentDownload_CallbacksContentDownloadOnProgress,
		&g_nativeCallbacksContentDownloadOnProgress);

RegisterNativeMethod(env, "CallbacksContentDownloadOnFailed",
		"tv/ouya/sdk/unreal/CallbacksContentDownload",
		"(Ltv/ouya/console/api/content/OuyaMod;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentDownload_CallbacksContentDownloadOnFailed,
		&g_nativeCallbacksContentDownloadOnFailed);

RegisterNativeMethod(env, "CallbacksContentDownloadOnComplete",
		"tv/ouya/sdk/unreal/CallbacksContentDownload",
		"(Ltv/ouya/console/api/content/OuyaMod;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentDownload_CallbacksContentDownloadOnComplete,
		&g_nativeCallbacksContentDownloadOnComplete);

//
// Register Native Callbacks for ContentInit
//

RegisterNativeMethod(env, "CallbacksContentInitOnInitialized",
		"tv/ouya/sdk/unreal/CallbacksContentInit", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentInit_CallbacksContentInitOnInitialized,
		&g_nativeCallbacksContentInitOnInitialized);

RegisterNativeMethod(env, "CallbacksContentInitOnDestroyed",
		"tv/ouya/sdk/unreal/CallbacksContentInit", "()V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentInit_CallbacksContentInitOnDestroyed,
		&g_nativeCallbacksContentInitOnDestroyed);

//
// Register Native Callbacks for ContentPublish
//

RegisterNativeMethod(env, "CallbacksContentPublishOnError",
		"tv/ouya/sdk/unreal/CallbacksContentPublish",
		"(Ltv/ouya/console/api/content/OuyaMod;ILjava/lang/String;Landroid/os/Bundle;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentPublish_CallbacksContentPublishOnError,
		&g_nativeCallbacksContentPublishOnError);

RegisterNativeMethod(env, "CallbacksContentPublishOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksContentPublish",
		"(Ltv/ouya/console/api/content/OuyaMod;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentPublish_CallbacksContentPublishOnSuccess,
		&g_nativeCallbacksContentPublishOnSuccess);

//
// Register Native Callbacks for ContentSave
//

RegisterNativeMethod(env, "CallbacksContentSaveOnError",
		"tv/ouya/sdk/unreal/CallbacksContentSave",
		"(Ltv/ouya/console/api/content/OuyaMod;ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentSave_CallbacksContentSaveOnError,
		&g_nativeCallbacksContentSaveOnError);

RegisterNativeMethod(env, "CallbacksContentSaveOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksContentSave",
		"(Ltv/ouya/console/api/content/OuyaMod;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentSave_CallbacksContentSaveOnSuccess,
		&g_nativeCallbacksContentSaveOnSuccess);

//
// Register Native Callbacks for ContentSearchInstalled
//

RegisterNativeMethod(env, "CallbacksContentSearchInstalledOnError",
		"tv/ouya/sdk/unreal/CallbacksContentSearchInstalled",
		"(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentSearchInstalled_CallbacksContentSearchInstalledOnError,
		&g_nativeCallbacksContentSearchInstalledOnError);

RegisterNativeMethod(env, "CallbacksContentSearchInstalledOnResults",
		"tv/ouya/sdk/unreal/CallbacksContentSearchInstalled",
		"([Ltv/ouya/console/api/content/OuyaMod;I)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentSearchInstalled_CallbacksContentSearchInstalledOnResults,
		&g_nativeCallbacksContentSearchInstalledOnResults);

//
// Register Native Callbacks for ContentSearchPublished
//

RegisterNativeMethod(env, "CallbacksContentSearchPublishedOnError",
		"tv/ouya/sdk/unreal/CallbacksContentSearchPublished",
		"(ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentSearchPublished_CallbacksContentSearchPublishedOnError,
		&g_nativeCallbacksContentSearchPublishedOnError);

RegisterNativeMethod(env, "CallbacksContentSearchPublishedOnResults",
		"tv/ouya/sdk/unreal/CallbacksContentSearchPublished",
		"([Ltv/ouya/console/api/content/OuyaMod;I)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentSearchPublished_CallbacksContentSearchPublishedOnResults,
		&g_nativeCallbacksContentSearchPublishedOnResults);

//
// Register Native Callbacks for ContentUnpublish
//

RegisterNativeMethod(env, "CallbacksContentUnpublishOnError",
		"tv/ouya/sdk/unreal/CallbacksContentUnpublish",
		"(Ltv/ouya/console/api/content/OuyaMod;ILjava/lang/String;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentUnpublish_CallbacksContentUnpublishOnError,
		&g_nativeCallbacksContentUnpublishOnError);

RegisterNativeMethod(env, "CallbacksContentUnpublishOnSuccess",
		"tv/ouya/sdk/unreal/CallbacksContentUnpublish",
		"(Ltv/ouya/console/api/content/OuyaMod;)V",
		(void*) &Java_tv_ouya_sdk_unreal_CallbacksContentUnpublish_CallbacksContentUnpublishOnSuccess,
		&g_nativeCallbacksContentUnpublishOnSuccess);

//
// DONE
//

return JNI_OK;
}
}
