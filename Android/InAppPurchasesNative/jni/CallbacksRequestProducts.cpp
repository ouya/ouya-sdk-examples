#include "Application.h"
#include "CallbacksRequestProducts.h"
#include "CallbackSingleton.h"
#include "JSON.h"
#include "Product.h"

#include <stdio.h>
#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

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

void CallbacksRequestProducts::OnSuccess(const std::string& jsonData)
{
	//char buffer[1024];
	//sprintf(buffer, "OnSuccess:  %s\r\n", jsonData.c_str());
	//LOGI(buffer);
	Application::m_ui.SetMessage("CallbacksRequestProducts::OnSuccess");

	Application::m_ui.ClearProducts();

	//LOGI("Parsing JSON Data");

	// Parse example data
	JSONValue* value = JSON::Parse(jsonData.c_str());

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

	for (unsigned int i = 0; i < data.size(); i++)
	{
		OuyaSDK::Product newProduct;
		newProduct.ParseJSON(data[i]);

		Application::m_ui.AddProduct(newProduct);
	}
}

void CallbacksRequestProducts::OnFailure(int errorCode, const std::string& errorMessage)
{
	//char buffer[256];
	//sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	//LOGI(buffer);
	Application::m_ui.SetMessage("CallbacksRequestProducts::OnFailure");
}

void CallbacksRequestProducts::OnCancel()
{
	//LOGI("OnCancel");
	Application::m_ui.SetMessage("CallbacksRequestProducts::OnCancel");
}