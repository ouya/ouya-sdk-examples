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

#include "OuyaSDK_OutputStream.h"

#include <android/log.h>
#include <jni.h>

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "java_io_OutputStream"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING false

namespace java_io_OutputStream
{
	JavaVM* OutputStream::_jvm = 0;
	jclass OutputStream::_jcOutputStream = 0;
	jmethodID OutputStream::_jmClose = 0;
	jmethodID OutputStream::_jmFlush = 0;
	jmethodID OutputStream::_jmWrite = 0;

	int OutputStream::InitJNI(JavaVM* jvm)
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
			const char* strClass = "java/io/OutputStream";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strClass);
#endif
				_jcOutputStream = (jclass)env->NewGlobalRef(localRef);
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

	int OutputStream::FindJNI()
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

		{
			const char* strMethod = "close";
			_jmClose = env->GetMethodID(_jcOutputStream, strMethod, "()V");
			if (_jmClose)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return JNI_ERR;
			}
		}

		{
			const char* strMethod = "flush";
			_jmFlush = env->GetMethodID(_jcOutputStream, strMethod, "()V");
			if (_jmFlush)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return JNI_ERR;
			}
		}

		{
			const char* strMethod = "write";
			_jmWrite = env->GetMethodID(_jcOutputStream, strMethod, "([B)V");
			if (_jmWrite)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return JNI_ERR;
			}
		}

		return JNI_OK;
	}

	OutputStream::OutputStream(jobject instance)
	{
		_instance = instance;
	}

	jobject OutputStream::GetInstance() const
	{
		return _instance;
	}

	void OutputStream::Dispose() const
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

	void OutputStream::close() const
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		if (!_instance) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is null");
			return;
		}

		if (!_jmClose) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmClose is null");
			return;
		}

		env->CallVoidMethod(_instance, _jmClose);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to close");
		}
	}

	void OutputStream::write(signed char buffer[], int length) const
	{
		if (!buffer)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "buffer is null");
			return;
		}

		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		if (!_instance) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is null");
			return;
		}

		if (!_jmWrite) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmWrite is null");
			return;
		}

		jbyteArray arg1 = env->NewByteArray(length);
		env->SetByteArrayRegion(arg1, 0, length, buffer);

		env->CallVoidMethod(_instance, _jmWrite, arg1);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to write");
		}

		env->DeleteLocalRef(arg1);
	}

	void OutputStream::flush() const
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		if (!_instance) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is null");
			return;
		}

		if (!_jmFlush) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmFlush is null");
			return;
		}

		env->CallVoidMethod(_instance, _jmFlush);

		if (env->ExceptionCheck())
		{
			env->ExceptionDescribe();
			env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to flush");
		}
	}
}
