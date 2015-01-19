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

#include "OuyaSDK_GamerInfo.h"
#include "OuyaSDK_JSONObject.h"

#include <string>

namespace OuyaSDK
{
	void GamerInfo::Init()
	{
		Username = "";
		Uuid = "";
	}

#if defined(ANDROID)
	void GamerInfo::ParseJSON(const org_json_JSONObject::JSONObject& jsonObject)
	{
		Init();

		std::string field;

		field = "username";
		if (jsonObject.has(field))
		{
			Username = jsonObject.getString(field);
		}

		field = "uuid";
		if (jsonObject.has(field))
		{
			Uuid = jsonObject.getString(field);
		}
	}
#endif
}
