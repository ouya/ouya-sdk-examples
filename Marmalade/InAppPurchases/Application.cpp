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

#include "Application.h"
#include "ApplicationCallbacksInitOuyaPlugin.h"
#include "JSONArray.h"
#include "JSONObject.h"
#include "ODK.h"

#include "IwDebug.h"

using namespace org_json_JSONArray;
using namespace org_json_JSONObject;

UI Application::m_ui = UI();

void Application::InitOuyaPlugin()
{
	Application::m_ui.SetMessage("Initialize OUYA Plugin...");

	int jsonArray = OuyaPlugin_JSONArray_Construct();

	int index = 0;
	int jsonObject = OuyaPlugin_JSONObject_Construct();

	OuyaPlugin_JSONObject_Put(jsonObject, "key", "tv.ouya.developer_id");
	OuyaPlugin_JSONObject_Put(jsonObject, "value", "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
	//IwTrace(Sanity, (OuyaPlugin_JSONObject_ToString(jsonObject)));

	OuyaPlugin_JSONArray_Put(jsonArray, index, jsonObject);	

	std::string jsonData = OuyaPlugin_JSONArray_ToString(jsonArray);
	//IwTrace(Sanity, (jsonData.c_str()));

	OuyaPlugin_initOuyaPlugin(jsonData.c_str(),
		Application::m_ui.m_callbacksInitOuyaPlugin->GetSuccessEvent(),
		Application::m_ui.m_callbacksInitOuyaPlugin->GetFailureEvent());
}