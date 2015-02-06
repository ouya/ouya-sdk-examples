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

#ifndef __TV_OUYA_CONSOLE_API_CONTENT_OUYACONTENT_H__
#define __TV_OUYA_CONSOLE_API_CONTENT_OUYACONTENT_H__

#if PLATFORM_ANDROID

#include "OuyaSDK_OuyaMod.h"

#include <jni.h>

namespace tv_ouya_console_api_content_OuyaContent
{
	class OuyaContent
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static int FindJNI();
		OuyaContent(jobject instance);
		jobject GetInstance() const;
		void Dispose() const;

		static OuyaContent getInstance();
		tv_ouya_console_api_content_OuyaMod::OuyaMod create();
		bool isInitialized() const;
	private:
		static JavaVM* _jvm;
		static jclass _jcOuyaContent;
		static jmethodID _jmCreate;
		static jmethodID _jmGetInstance;
		static jmethodID _jmIsInitialized;
		jobject _instance;
	};
}

#endif

#endif
