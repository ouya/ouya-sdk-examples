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

#include "OuyaSDK_Bundle.h"

#include <android/log.h>
#include <jni.h>

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "android_os_Bundle"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING false

namespace android_os_Bundle
{
	JavaVM* Bundle::_jvm = 0;
	jclass Bundle::_jcBundle = 0;

	int Bundle::InitJNI(JavaVM* jvm)
	{
		_jvm = jvm;

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return JNI_ERR;
		}

		{
			const char* strClass = "android/os/Bundle";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strClass);
#endif
				_jcBundle = (jclass)env->NewGlobalRef(localRef);
				env->DeleteLocalRef(localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		return FindJNI();
	}

	int Bundle::FindJNI()
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return JNI_ERR;
		}

		return JNI_OK;
	}

	Bundle::Bundle(jobject instance)
	{
		_instance = instance;
	}

	jobject Bundle::GetInstance() const
	{
		return _instance;
	}

	void Bundle::Dispose() const
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (env &&
			_instance)
		{
			env->DeleteGlobalRef(_instance);
		}
	}
}
