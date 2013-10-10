#include "CallbacksRequestPurchase.h"
#include "CallbackSingleton.h"

#include <android/log.h>
#include <stdio.h>

#define APP_NAME "inapppurchasesnative_CallbacksRequestPurchase"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

void CallbacksRequestPurchase::OnSuccess(const OuyaSDK::Product& product)
{
	//char buffer[1024];
	//sprintf(buffer, "OnSuccess:  %s\r\n", product.Name.c_str());
	//LOGI(buffer);

	//std::string message = "CallbacksRequestPurchase::OnSuccess: ";
	//message.append(product.Identifier);

	//Application::m_ui.SetMessage(message);
}

void CallbacksRequestPurchase::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//LOGI(buffer);
	//Application::m_ui.SetMessage("CallbacksRequestPurchase::OnFailure");
}

void CallbacksRequestPurchase::OnCancel()
{
	//LOGI("OnCancel");
	//Application::m_ui.SetMessage("CallbacksRequestPurchase::OnCancel");
}