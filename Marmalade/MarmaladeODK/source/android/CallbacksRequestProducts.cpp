#include "CallbacksRequestProducts.h"
#include "CallbackSingleton.h"

#include <android/log.h>
#include <stdio.h>

#define APP_NAME "inapppurchasesnative_CallbacksRequestProducts"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

void CallbacksRequestProducts::OnSuccess(const std::vector<OuyaSDK::Product>& products)
{
	//char buffer[1024];
	//sprintf(buffer, "OnSuccess:  %s\r\n", jsonData.c_str());
	//LOGI(buffer);
	//Application::m_ui.SetMessage("CallbacksRequestProducts::OnSuccess");

	//Application::m_ui.ClearProducts();

	//LOGI("Parsing JSON Data");

	//for (int index = 0; index < products.size(); index++)
	//{
	//	Application::m_ui.AddProduct(products[index]);
	//}
}

void CallbacksRequestProducts::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//LOGI(buffer);
	//Application::m_ui.SetMessage("CallbacksRequestProducts::OnFailure");
}

void CallbacksRequestProducts::OnCancel()
{
	//LOGI("OnCancel");
	//Application::m_ui.SetMessage("CallbacksRequestProducts::OnCancel");
}