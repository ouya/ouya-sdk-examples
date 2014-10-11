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

#ifndef __CALLBACKS_SET_DEVELOPER_ID_H__
#define __CALLBACKS_SET_DEVELOPER_ID_H__

#include "s3eTypes.h"

#include <string>

typedef struct s3eSetDeveloperIdSuccessEvent
{
	char m_gamerUUID[256];
} s3eSetDeveloperIdSuccessEvent;

typedef struct s3eSetDeveloperIdFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eSetDeveloperIdFailureEvent;

class CallbacksSetDeveloperId
{
public:

	void RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure);

	void OnSuccess();

	void OnFailure(int errorCode, const std::string& errorMessage);

private:
	//extension callbacks
	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	
	//event data sent to callbacks can't be temporary
	s3eSetDeveloperIdSuccessEvent m_dataSetDeveloperIdSuccessEvent;
	s3eSetDeveloperIdFailureEvent m_dataSetDeveloperIdFailureEvent;

	void RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType);
};

#endif