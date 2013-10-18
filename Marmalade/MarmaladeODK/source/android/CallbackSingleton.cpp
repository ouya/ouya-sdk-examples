#include "CallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"
#include "CallbacksRequestProducts.h"
#include "CallbacksRequestPurchase.h"
#include "CallbacksRequestReceipts.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"

#include <android/log.h>
#include <jni.h>
#include <stdio.h>
#include <string>

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

		m_callbacksFetchGamerUUID = new CallbacksFetchGamerUUID();
		m_callbacksRequestProducts = new CallbacksRequestProducts();
		m_callbacksRequestPurchase = new CallbacksRequestPurchase();
		m_callbacksRequestReceipts = new CallbacksRequestReceipts();
	}

	CallbackSingleton::~CallbackSingleton()
	{
		delete m_callbacksFetchGamerUUID;
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
		//com.ODK.CallbacksFetchGamerUUID.CallbacksFetchGamerUUIDOnSuccess
		JNIEXPORT void JNICALL Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess(JNIEnv* env, jobject thiz, jstring gamerUUID)
		{
			LOGI("***********Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess***********");

			std::string strGamerUUID = env->GetStringUTFChars(gamerUUID, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess: Returned to C: %s", strGamerUUID.c_str());
			//LOGI(buffer);

			//CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
			//if (callback)
			//{
			//	callback->OnSuccess(strGamerUUID);
			//}

			LOGI("Registering Callback: s3eEdkCallbacksRegister(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);");

			s3eFetchGamerUuidSuccessEvent event;
			event.m_gamerUUID = strGamerUUID;

			s3eEdkCallbacksRegister(
					S3E_EXT_ODK_HASH,
					S3E_ODK_CALLBACKS_MAX,
					S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS,
					OuyaSDK::CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID->m_onSuccess,
					&event,
					S3E_FALSE);

			LOGI("Invoking Callback: s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);");

			s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_FETCH_GAMER_UUID_ON_SUCCESS);
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			//CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
			//if (callback)
			//{
			//	callback->OnFailure(errorCode, strErrorMessage);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnCancel: Returned to C");
			//LOGI(buffer);

			//CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
			//if (callback)
			//{
			//	callback->OnCancel();
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess: Returned to C: %s", strJsonData.c_str());
			//LOGI(buffer);

			// Parse example data
			JSONValue* value = JSON::Parse(strJsonData.c_str());

			if (value == NULL)
			{
				LOGI("Parsing JSON Failed");
				return;
			}

			if (!value->IsArray())
			{
				LOGI("Parsing JSON Failed: Not an array");
				return;
			}

			// Retrieve the main object
			JSONArray data = value->AsArray();

			std::vector<Product> products;

			for (unsigned int i = 0; i < data.size(); i++)
			{
				OuyaSDK::Product newProduct;
				newProduct.ParseJSON(data[i]);

				products.push_back(newProduct);
			}

			//CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			//if (callback)
			//{
			//	callback->OnSuccess(products);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			//CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			//if (callback)
			//{
			//	callback->OnFailure(errorCode, strErrorMessage);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestProducts_CallbacksRequestProductsOnCancel: Returned to C");
			//LOGI(buffer);

			//CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			//if (callback)
			//{
			//	callback->OnCancel();
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess: Returned to C: %s", strJsonData.c_str());
			//LOGI(buffer);

			// Parse example data
			JSONValue* value = JSON::Parse(strJsonData.c_str());

			if (value == NULL)
			{
				LOGI("Parsing JSON Failed");
				return;
			}

			if (!value->IsObject())
			{
				LOGI("Parsing JSON Failed: Not an object");
				return;
			}

			// Retrieve the main object
			JSONValue data = value->AsObject();

			OuyaSDK::Product product;
			product.ParseJSON(&data);

			//CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			//if (callback)
			//{
			//	callback->OnSuccess(product);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			//CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			//if (callback)
			//{
			//	callback->OnFailure(errorCode, strErrorMessage);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel: Returned to C");
			//LOGI(buffer);

			//CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			//if (callback)
			//{
			//	callback->OnCancel();
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			LOGI("***********Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess***********");

			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess: Returned to C: %s", strJsonData.c_str());
			//LOGI(buffer);

			//LOGI("Parsing JSON Data");

			// Parse example data
			JSONValue* value = JSON::Parse(strJsonData.c_str());

			if (value == NULL)
			{
				LOGI("Parsing JSON Failed");
				return;
			}

			if (!value->IsArray())
			{
				LOGI("Parsing JSON Failed: Not an array");
				return;
			}

			// Retrieve the main object
			JSONArray data = value->AsArray();

			std::vector<Receipt> receipts;

			for (unsigned int i = 0; i < data.size(); i++)
			{
				OuyaSDK::Receipt newReceipt;
				newReceipt.ParseJSON(data[i]);

				receipts.push_back(newReceipt);
			}

			//CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			//if (callback)
			//{
			//	callback->OnSuccess(receipts);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure***********");

			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);

			//CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			//if (callback)
			//{
			//	callback->OnFailure(errorCode, strErrorMessage);
			//}
		}

		JNIEXPORT void JNICALL Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel***********");

			//char buffer[256];
			//sprintf(buffer, "Java_com_ODK_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel: Returned to C");
			//LOGI(buffer);

			//CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			//if (callback)
			//{
			//	callback->OnCancel();
			//}
		}
	}

	void RegisterNativeMethod(JNIEnv* env, std::string methodName, std::string signature)
	{
		std::string msg = "Registering Native Method: methodName=";
		msg.append(methodName);
		msg.append(" signature=");
		msg.append(signature);
		LOGI(msg.c_str());
	}

	void CallbackSingleton::RegisterNativeMethods()
	{
		JNIEnv* env = s3eEdkJNIGetEnv();

		RegisterNativeMethod(env, "CallbacksFetchGamerUUIDOnSuccess", "(Ljava/lang/String;)V");

		//Register the HelloNative method on the object
		static const JNINativeMethod jnm = { "CallbacksFetchGamerUUIDOnSuccess", "(Ljava/lang/String;)V", (void*)&Java_com_ODK_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess };

		// Find the class using the native
		jclass CallerJavaClass = env->FindClass("com/ODK/CallbacksFetchGamerUUID");
		if (env->RegisterNatives(CallerJavaClass, &jnm, 1))
		{
			LOGI("Could not register native method: CallbacksFetchGamerUUIDOnSuccess");
			return;
		}
	}
}