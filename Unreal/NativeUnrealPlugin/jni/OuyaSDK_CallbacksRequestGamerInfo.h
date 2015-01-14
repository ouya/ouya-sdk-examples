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

#ifndef __CALLBACKS_REQUEST_GAMER_INFO_H__
#define __CALLBACKS_REQUEST_GAMER_INFO_H__

#include "OuyaSDK_GamerInfo.h"

#include <string>

class CallbacksRequestGamerInfo
{
public:

	virtual void OnSuccess(const OuyaSDK::GamerInfo& gamerInfo);

	virtual void OnFailure(int errorCode, const std::string& errorMessage);

	virtual void OnCancel();
};

#endif
