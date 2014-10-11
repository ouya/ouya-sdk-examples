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
#include "ApplicationCallbacksSetDeveloperId.h"
#include "ODK.h"

ApplicationCallbacksSetDeveloperId* Application::m_callbacksSetDeveloperId = new ApplicationCallbacksSetDeveloperId();

void Application::Init()
{
	OuyaPlugin_asyncSetDeveloperId("310a8f51-4d6e-4ae5-bda0-b93878e5f5d0",
		Application::m_callbacksSetDeveloperId->GetSuccessEvent(),
		Application::m_callbacksSetDeveloperId->GetFailureEvent());
}
