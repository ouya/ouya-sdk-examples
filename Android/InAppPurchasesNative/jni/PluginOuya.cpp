#include "Application.h"
#include "CallbackSingleton.h"
#include "PluginOuya.h"

#include <android/log.h>

#include <string>
#include <vector>

#define APP_NAME "inapppurchasesnative_PluginOuya"

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
		Initialized = false;
		m_developerId = "";
		jc_AsyncCppOuyaSetDeveloperId = NULL;
		jc_AsyncCppOuyaFetchGamerUUID = NULL;
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
		FindClass(env, "tv/ouya/sdk/android/AsyncCppOuyaSetDeveloperId", &jc_AsyncCppOuyaSetDeveloperId);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/android/AsyncCppOuyaFetchGamerUUID", &jc_AsyncCppOuyaFetchGamerUUID);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/android/AsyncCppOuyaRequestProducts", &jc_AsyncCppOuyaRequestProducts);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/android/AsyncCppOuyaRequestPurchase", &jc_AsyncCppOuyaRequestPurchase);
		EXCEPTION_RETURN(env);

		FindClass(env, "tv/ouya/sdk/android/AsyncCppOuyaRequestReceipts", &jc_AsyncCppOuyaRequestReceipts);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::Initialize()
	{
		if (!Initialized)
		{
			AsyncSetDeveloperId();
			//LOGI("plugin_ouya is initialized");
		}
	}

	void PluginOuya::SetDeveloperId(const std::string& developerId)
	{
		m_developerId = developerId;
	}

	void PluginOuya::AsyncSetDeveloperId()
	{
		if (Initialized)
		{
			return;
		}

		if (!jc_AsyncCppOuyaSetDeveloperId)
		{
			LOGI("jc_AsyncOuyaSetDeveloperId is not initialized");
			return;
		}

		JNIEnv* env = Application::m_app->appThreadEnv;

		if (!env)
		{
			LOGI("JNIEnv is invalid");
			return;
		}

		//std::string buffer = "Developer Id: ";
		//buffer.append(m_developerId);
		//LOGI(buffer.c_str());

		//LOGI("Allocate DeveloperId String");
		jstring developerIdString = env->NewStringUTF(m_developerId.c_str());
		EXCEPTION_RETURN(env);

		//LOGI("allocate the object");
		jobject objSetDeveloperId = env->AllocObject(jc_AsyncCppOuyaSetDeveloperId); 
		EXCEPTION_RETURN(env);

		//LOGI("get the constructor");
		jmethodID constructSetDeveloperId = env->GetMethodID(jc_AsyncCppOuyaSetDeveloperId, "<init>", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("construct the object");
		env->CallVoidMethod(objSetDeveloperId, constructSetDeveloperId);
		EXCEPTION_RETURN(env);

		//LOGI("get the invoke method");
		jmethodID invokeSetDeveloperId = env->GetStaticMethodID(jc_AsyncCppOuyaSetDeveloperId, "invoke", "(Ljava/lang/String;)V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaSetDeveloperId, invokeSetDeveloperId, developerIdString);
		EXCEPTION_RETURN(env);

		Initialized = true;
	}

	void PluginOuya::AsyncOuyaFetchGamerUUID(CallbacksFetchGamerUUID* callbacksFetchGamerUUID)
	{
		CallbackSingleton::GetInstance()->m_callbacksFetchGamerUUID = callbacksFetchGamerUUID;

		Initialize();

		//set the callback singleton here

		//LOGI("AsyncOuyaFetchGamerUUID");

		JNIEnv* env = Application::m_app->appThreadEnv;

		if (!env)
		{
			LOGI("JNIEnv is invalid");
			return;
		}

		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaFetchGamerUUID, "invoke", "()V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaFetchGamerUUID, invokeMethod);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestProducts(CallbacksRequestProducts* callbacksRequestProducts, const std::vector<std::string>& productIds)
	{
		CallbackSingleton::GetInstance()->m_callbacksRequestProducts = callbacksRequestProducts;

		Initialize();

		//LOGI("AsyncOuyaRequestProducts");

		JNIEnv* env = Application::m_app->appThreadEnv;

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
		for (int i = 0; i < productIds.size(); ++i) {
			env->SetObjectArrayElement(products, i, env->NewStringUTF(productIds[i].c_str()));
		}


		//LOGI("get the invoke method");
		jmethodID invokeMethod = env->GetStaticMethodID(jc_AsyncCppOuyaRequestProducts, "invoke", "([Ljava/lang/String;)V");
		EXCEPTION_RETURN(env);

		//LOGI("execute the invoke method");
		env->CallStaticVoidMethod(jc_AsyncCppOuyaRequestProducts, invokeMethod, products);
		EXCEPTION_RETURN(env);
	}

	void PluginOuya::AsyncOuyaRequestPurchase(CallbacksRequestPurchase* callbacksRequestPurchase, const std::string& purchasable)
	{
		CallbackSingleton::GetInstance()->m_callbacksRequestPurchase = callbacksRequestPurchase;

		Initialize();

		//LOGI("AsyncOuyaRequestPurchase");

		JNIEnv* env = Application::m_app->appThreadEnv;

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

	void PluginOuya::AsyncOuyaRequestReceipts(CallbacksRequestReceipts* callbacksRequestReceipts)
	{
		CallbackSingleton::GetInstance()->m_callbacksRequestReceipts = callbacksRequestReceipts;

		Initialize();

		//LOGI("AsyncOuyaRequestReceipts");

		JNIEnv* env = Application::m_app->appThreadEnv;

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