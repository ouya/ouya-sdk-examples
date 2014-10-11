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

#ifndef __CALLBACKS_REQUEST_GAMER_INFO_H__
#define __CALLBACKS_REQUEST_GAMER_INFO_H__

#include "ExtensionGamerInfo.h"

#include "s3eTypes.h"

#include <string>

typedef struct s3eRequestGamerInfoSuccessEvent
{
	OuyaSDK::ExtensionGamerInfo m_gamerInfo;
} s3eRequestGamerInfoSuccessEvent;

typedef struct s3eRequestGamerInfoFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eRequestGamerInfoFailureEvent;

typedef struct s3eRequestGamerInfoCancelEvent
{
} s3eRequestGamerInfoCancelEvent;

class CallbacksRequestGamerInfo
{
public:

	void RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel);

	void OnSuccess(const OuyaSDK::GamerInfo& gamerInfo);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();

private:
	//extension callbacks
	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	s3eCallback m_onCancel;

	//event data sent to callbacks can't be temporary
	s3eRequestGamerInfoSuccessEvent m_dataRequestGamerInfoSuccessEvent;
	s3eRequestGamerInfoFailureEvent m_dataRequestGamerInfoFailureEvent;
	s3eRequestGamerInfoCancelEvent m_dataRequestGamerInfoCancelEvent;

	void RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType);
};

#endif