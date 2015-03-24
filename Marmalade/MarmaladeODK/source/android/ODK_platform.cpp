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

/*
 * android-specific implementation of the ODK extension.
 * Add any platform-specific functionality here.
 */
/*
 * NOTE: This file was originally written by the extension builder, but will not
 * be overwritten (unless --force is specified) and is intended to be modified.
 */
#include "ODK_internal.h"
#include "CallbackSingleton.h"
#include "CallbacksInitOuyaPlugin.h"
#include "CallbacksRequestGamerInfo.h"
#include "CallbacksRequestProducts.h"
#include "CallbacksRequestPurchase.h"
#include "CallbacksRequestReceipts.h"
#include "JSONArray.h"
#include "JSONObject.h"
#include "OuyaController.h"
#include "PluginOuya.h"

#include <map>

#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include <jni.h>
#include "IwDebug.h"

#include <android/log.h>
#define LOG_TAG "ODK_platform.cpp"

using namespace org_json_JSONObject;
using namespace org_json_JSONArray;
using namespace tv_ouya_console_api_OuyaController;

static OuyaSDK::PluginOuya g_pluginOuya;

// use string to send char* to invoker
static std::string g_tempPluginString;

#define MAX_CONTROLLERS 4

//axis states
static std::vector< std::map<int, float> > g_axis;

//button states
static std::vector< std::map<int, bool> > g_button;
static std::vector< std::map<int, bool> > g_buttonDown;
static std::vector< std::map<int, bool> > g_buttonUp;
static std::vector< std::map<int, bool> > g_lastButtonDown;
static std::vector< std::map<int, bool> > g_lastButtonUp;

void dispatchGenericMotionEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint axis,
	jfloat val)
{
	//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "dispatchGenericMotionEventNative: Device=%d axis=%d val=%f", deviceId, axis, val);
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}
	g_axis[deviceId][axis] = val;
}

void dispatchKeyEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint keyCode,
	jint action)
{
	//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "dispatchKeyEventNative: Device=%d KeyCode=%d Action=%d", deviceId, keyCode, action);
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}

	bool buttonDown = action == 0;

	if (g_button[deviceId][keyCode] != buttonDown)
	{
		g_button[deviceId][keyCode] = buttonDown;
		if (buttonDown)
		{
			g_buttonDown[deviceId][keyCode] = true;
		}
		else
		{
			g_buttonUp[deviceId][keyCode] = true;
		}
	}
}

static JNINativeMethod method_table[] = {
	{ "dispatchGenericMotionEventNative", "(IIF)V", (void *)dispatchGenericMotionEventNative }
};

static int method_table_size = sizeof(method_table) / sizeof(method_table[0]);

static JNINativeMethod method_table2[] = {
	{ "dispatchKeyEventNative", "(III)V", (void *)dispatchKeyEventNative }
};

static int method_table_size2 = sizeof(method_table2) / sizeof(method_table2[0]);

s3eResult ODKInit_platform()
{
    // Get the environment from the pointer
    JNIEnv* env = s3eEdkJNIGetEnv();

	for (int index = 0; index < MAX_CONTROLLERS; ++index)
	{
		g_axis.push_back(std::map<int, float>());
		g_button.push_back(std::map<int, bool>());
		g_buttonDown.push_back(std::map<int, bool>());
		g_buttonUp.push_back(std::map<int, bool>());
		g_lastButtonDown.push_back(std::map<int, bool>());
		g_lastButtonUp.push_back(std::map<int, bool>());
	}

	jclass clazz = env->FindClass("tv/ouya/sdk/marmalade/ODK");
	if (clazz)
	{
		jint ret = env->RegisterNatives(clazz, method_table, method_table_size);
		ret = env->RegisterNatives(clazz, method_table2, method_table_size2);

		const char* strMethod = "nativeLoaded";
		jmethodID nativeLoaded = env->GetStaticMethodID(clazz, strMethod, "()V");
		env->CallStaticVoidMethod(clazz, nativeLoaded);

		env->DeleteLocalRef(clazz);
	}
	else
	{
		IwTrace(ODK, ("Failed to find ODK"));
		goto fail;
	}

	if (JSONArray::InitJNI(env) == JNI_ERR)
	{
		goto fail;
	}

	if (JSONObject::InitJNI(env) == JNI_ERR)
	{
		goto fail;
	}

	if (OuyaController::InitJNI(env) == JNI_ERR)
	{
		goto fail;
	}

	// cache class references
	g_pluginOuya.CacheClasses(env);

    // Add any platform-specific initialization code here
    return S3E_RESULT_SUCCESS;

fail:
    jthrowable exc = env->ExceptionOccurred();
    if (exc)
    {
        env->ExceptionDescribe();
        env->ExceptionClear();
        IwTrace(ODK, ("One or more java methods could not be found"));
    }
    return S3E_RESULT_ERROR;

}

void ODKTerminate_platform()
{
    // Add any platform-specific termination code here
}

// get axis value, cast float to int for the application bridge
int OuyaPlugin_getAxis(int deviceId, int axis)
{
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}

	std::map<int, float>::const_iterator search = g_axis[deviceId].find(axis);
	float val = 0.0f;
	if (search != g_axis[deviceId].end())
	{
		val = search->second;
	}
	//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_getAxis: Device=%d axis=%d val=%f", deviceId, axis, val);
	return *(reinterpret_cast<int*>(&val));
}

// check if a button is pressed
bool OuyaPlugin_isPressed(int deviceId, int keyCode)
{
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}

	std::map<int, bool>::const_iterator search = g_button[deviceId].find(keyCode);
	bool val = false;
	if (search != g_button[deviceId].end())
	{
		val = search->second;
	}
	//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_isPressed: Device=%d KeyCode=%d Action=%d", deviceId, keyCode, val);
	return val;
}

// check if a button was down
bool OuyaPlugin_isPressedDown(int deviceId, int keyCode)
{
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}

	std::map<int, bool>::const_iterator search = g_lastButtonDown[deviceId].find(keyCode);
	if (search != g_lastButtonDown[deviceId].end())
	{
		return search->second;
	}
	return false;
}

// check if a button was up
bool OuyaPlugin_isPressedUp(int deviceId, int keyCode)
{
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}

	std::map<int, bool>::const_iterator search = g_lastButtonUp[deviceId].find(keyCode);
	if (search != g_lastButtonUp[deviceId].end())
	{
		return search->second;
	}
	return false;
}

// clear the button state for detecting up and down
void OuyaPlugin_clearButtonStates()
{
	for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId)
	{
		g_lastButtonDown[deviceId].clear();
		g_lastButtonUp[deviceId].clear();
		for (std::map<int, bool>::iterator it = g_buttonDown[deviceId].begin(); it != g_buttonDown[deviceId].end(); ++it)
		{
			int keyCode = it->first;
			g_lastButtonDown[deviceId][keyCode] = g_buttonDown[deviceId][keyCode];
		}
		for (std::map<int, bool>::iterator it = g_buttonUp[deviceId].begin(); it != g_buttonUp[deviceId].end(); ++it)
		{
			int keyCode = it->first;
			g_lastButtonUp[deviceId][keyCode] = g_buttonUp[deviceId][keyCode];
		}
		g_buttonDown[deviceId].clear();
		g_buttonUp[deviceId].clear();
	}
}

const char* OuyaPlugin_getDeviceName(int playerNum)
{
	OuyaController* ouyaController = OuyaController::getControllerByPlayer(playerNum);
	if (NULL != ouyaController)
	{
		g_tempPluginString = ouyaController->getDeviceName();
		ouyaController->Dispose();
	}
	else
	{
		g_tempPluginString = "Unavailable";
	}
	return g_tempPluginString.c_str();
}

void OuyaPlugin_initOuyaPlugin(const char* jsonData, s3eCallback onSuccess, s3eCallback onFailure)
{
	IwTrace(ODK, ("OuyaPlugin_initOuyaPlugin"));

	OuyaSDK::CallbackSingleton::GetInstance()->m_callbacksInitOuyaPlugin->RegisterCallbacks(onSuccess, onFailure);

	g_pluginOuya.AsyncInitOuyaPlugin(jsonData);
}

void OuyaPlugin_asyncOuyaRequestGamerInfo(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	IwTrace(ODK, ("ODK_platform: OuyaPlugin_asyncOuyaRequestGamerInfo"));

	OuyaSDK::CallbackSingleton::GetInstance()->m_callbacksRequestGamerInfo->RegisterCallbacks(onSuccess, onFailure, onCancel);

	g_pluginOuya.AsyncOuyaRequestGamerInfo();
}

void OuyaPlugin_asyncOuyaRequestProducts(const char* productsJson, s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	IwTrace(ODK, ("ODK_platform: OuyaPlugin_asyncOuyaRequestProducts"));

	std::string msg = "OuyaPlugin_asyncOuyaRequestProducts: productsJson=";
	msg.append(productsJson);
	IwTrace(ODK, (msg.c_str()));

	//convert JSON to product id array
	JSONArray jsonArray = JSONArray(productsJson);

	std::vector<std::string> productIds;

	for (int i = 0; i < jsonArray.length(); i++)
	{
		std::string productId = jsonArray.getString(i);
		productIds.push_back(productId);
	}

	OuyaSDK::CallbackSingleton::GetInstance()->m_callbacksRequestProducts->RegisterCallbacks(onSuccess, onFailure, onCancel);

	g_pluginOuya.AsyncOuyaRequestProducts(productIds);
}

void OuyaPlugin_asyncOuyaRequestPurchase(const char* purchasable, s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	IwTrace(ODK, ("ODK_platform: OuyaPlugin_asyncOuyaRequestPurchase"));

	std::string msg = "OuyaPlugin_asyncOuyaRequestPurchase: purchasable=";
	msg.append(purchasable);
	IwTrace(ODK, (msg.c_str()));

	OuyaSDK::CallbackSingleton::GetInstance()->m_callbacksRequestPurchase->RegisterCallbacks(onSuccess, onFailure, onCancel);

	g_pluginOuya.AsyncOuyaRequestPurchase(purchasable);
}

void OuyaPlugin_asyncOuyaRequestReceipts(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	IwTrace(ODK, ("ODK_platform: OuyaPlugin_asyncOuyaRequestReceipts"));

	OuyaSDK::CallbackSingleton::GetInstance()->m_callbacksRequestReceipts->RegisterCallbacks(onSuccess, onFailure, onCancel);

	g_pluginOuya.AsyncOuyaRequestReceipts();
}

static int g_refCountJSONObject = 0;
static std::map<int, JSONObject*> g_refJSONObject = std::map<int, JSONObject*>();

int OuyaPlugin_JSONObject_Construct()
{
	JSONObject* jsonObject = new JSONObject();
	g_refJSONObject[g_refCountJSONObject] = jsonObject;
	int result = g_refCountJSONObject;
	++g_refCountJSONObject;
	return result;
}

void OuyaPlugin_JSONObject_Put(int jsonObject, const char* name, const char* value)
{
#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONObject_Put: jsonObject=%d", jsonObject);
#endif
	std::map<int, JSONObject*>::const_iterator search = g_refJSONObject.find(jsonObject);
	if (search != g_refJSONObject.end())
	{
		JSONObject* instance = search->second;
		if (instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONObject_Put JSONObject reference is valid");
#endif
			instance->put(name, value);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONObject_Put JSONObject reference is invalid");
		}
	}
	else
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONObject_Put failed to find JSONObject reference");
	}
}

const char* OuyaPlugin_JSONObject_ToString(int jsonObject)
{
#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONObject_ToString: jsonObject=%d", jsonObject);
#endif
	std::map<int, JSONObject*>::const_iterator search = g_refJSONObject.find(jsonObject);
	if (search != g_refJSONObject.end())
	{
		JSONObject* instance = search->second;
		if (instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONObject_ToString JSONObject reference is valid");
#endif
			g_tempPluginString = instance->toString();
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONObject_ToString jsonData=%s", g_tempPluginString.c_str());
#endif
			return g_tempPluginString.c_str();
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONObject_ToString JSONObject reference is invalid");
		}
	}
	else
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONObject_ToString failed to find JSONObject reference");
	}

	return "";
}

static int g_refCountJSONArray = 0;
static std::map<int, JSONArray*> g_refJSONArray = std::map<int, JSONArray*>();

int OuyaPlugin_JSONArray_Construct()
{
	JSONArray* jsonArray = new JSONArray();
	g_refJSONArray[g_refCountJSONArray] = jsonArray;
	int result = g_refCountJSONArray;
	++g_refCountJSONArray;
	return result;
}

void OuyaPlugin_JSONArray_Put(int jsonArray, int index, int jsonObject)
{
	std::map<int, JSONArray*>::const_iterator searchJSONArray = g_refJSONArray.find(jsonArray);
	if (searchJSONArray == g_refJSONArray.end())
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONArray_Put JSONArray reference is invalid");
		return;
	}

	std::map<int, JSONObject*>::const_iterator searchJSONObject = g_refJSONObject.find(jsonObject);
	if (searchJSONObject == g_refJSONObject.end())
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONArray_Put JSONObject reference is invalid");
		return;
	}

	JSONArray* instanceJSONArray = searchJSONArray->second;
	if (!instanceJSONArray)
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONArray_Put JSONArray instance is invalid");
		return;
	}

	JSONObject* instanceJSONObject = searchJSONObject->second;
	if (!instanceJSONObject)
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONArray_Put JSONObject instance is invalid");
		return;
	}

	instanceJSONArray->put(index, instanceJSONObject->GetInstance());
}

const char* OuyaPlugin_JSONArray_ToString(int jsonArray)
{
#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONArray_ToString: jsonArray=%d", jsonArray);
#endif
	std::map<int, JSONArray*>::const_iterator search = g_refJSONArray.find(jsonArray);
	if (search != g_refJSONArray.end())
	{
		JSONArray* instance = search->second;
		if (instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONArray_ToString JSONArray reference is valid");
#endif
			g_tempPluginString = instance->toString();
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "OuyaPlugin_JSONArray_ToString jsonData=%s", g_tempPluginString.c_str());
#endif
			return g_tempPluginString.c_str();
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONArray_ToString JSONArray reference is invalid");
		}
	}
	else
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "OuyaPlugin_JSONArray_ToString failed to find JSONArray reference");
	}

	return "";
}
