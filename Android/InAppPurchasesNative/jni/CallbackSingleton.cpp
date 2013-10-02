#include "CallbackSingleton.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative_callbacks"

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

		m_callbacksFetchGamerUUID = NULL;
		m_callbacksRequestProducts = NULL;
		m_callbacksRequestPurchase = NULL;
		m_callbacksRequestReceipts = NULL;
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
		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess(JNIEnv* env, jobject thiz, jstring gamerUUID)
		{
			LOGI("***********Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess***********");

			std::string strGamerUUID = env->GetStringUTFChars(gamerUUID, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnSuccess: Returned to C: %s", strGamerUUID.c_str());
			//LOGI(buffer);
		
			CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
			if (callback)
			{
				callback->OnSuccess(strGamerUUID);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnFailure***********");
		
			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);
		
			CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnCancel***********");
		
			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksFetchGamerUUID_CallbacksFetchGamerUUIDOnCancel: Returned to C");
			//LOGI(buffer);
		
			CallbacksFetchGamerUUID* callback = CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess***********");
		
			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnSuccess: Returned to C: %s", strJsonData.c_str());
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
		
			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnSuccess(products);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnFailure***********");
		
			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);
		
			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnCancel***********");
		
			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestProducts_CallbacksRequestProductsOnCancel: Returned to C");
			//LOGI(buffer);
		
			CallbacksRequestProducts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestProducts;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess***********");
		
			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnSuccess: Returned to C: %s", strJsonData.c_str());
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
		
			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnSuccess(product);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure***********");
		
			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);
		
			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel***********");
		
			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestPurchase_CallbacksRequestPurchaseOnCancel: Returned to C");
			//LOGI(buffer);
		
			CallbacksRequestPurchase* callback = CallbackSingleton::GetInstance()->m_callbacksRequestPurchase;
			if (callback)
			{
				callback->OnCancel();
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess(JNIEnv* env, jobject thiz, jstring jsonData)
		{
			LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess***********");
		
			std::string strJsonData = env->GetStringUTFChars(jsonData, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnSuccess: Returned to C: %s", strJsonData.c_str());
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
		
			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnSuccess(receipts);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure(JNIEnv* env, jobject thiz, jint errorCode, jstring errorMessage)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure***********");
		
			std::string strErrorMessage = env->GetStringUTFChars(errorMessage, NULL);

			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnFailure: Returned to C: %d %s", errorCode, strGamerUUID.c_str());
			//LOGI(buffer);
		
			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnFailure(errorCode, strErrorMessage);
			}
		}

		JNIEXPORT void JNICALL Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel(JNIEnv* env, jobject thiz)
		{
			//LOGI("***********Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel***********");
		
			//char buffer[256];
			//sprintf(buffer, "Java_tv_ouya_sdk_android_CallbacksRequestReceipts_CallbacksRequestReceiptsOnCancel: Returned to C");
			//LOGI(buffer);
		
			CallbacksRequestReceipts* callback = CallbackSingleton::GetInstance()->m_callbacksRequestReceipts;
			if (callback)
			{
				callback->OnCancel();
			}
		}
	}
}