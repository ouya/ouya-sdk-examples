#include "CallbacksFetchGamerUUID.h"
#include "CallbackSingleton.h"

#include <android/log.h>
#include <stdio.h>

#define APP_NAME "MarmaladeODK_CallbacksFetchGamerUUID"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

void CallbacksFetchGamerUUID::OnSuccess(const std::string& gamerUUID)
{
	char buffer[256];
	sprintf(buffer, "OnSuccess:  %s", gamerUUID.c_str());
	LOGI(buffer);

	//Application::m_ui.SetMessage("CallbacksFetchGamerUUID::OnSuccess");
	//Application::m_ui.SetGamerUUID(gamerUUID);
}

void CallbacksFetchGamerUUID::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	LOGI(buffer);

	//Application::m_ui.SetMessage("CallbacksFetchGamerUUID::OnFailure");
}

void CallbacksFetchGamerUUID::OnCancel()
{
	LOGI("OnCancel");

	//Application::m_ui.SetMessage("CallbacksFetchGamerUUID::OnCancel");
}