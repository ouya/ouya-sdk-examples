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

#include "OuyaSDK_Bitmap.h"
#include "OuyaSDK_BitmapConfig.h"

#include <android/log.h>
#include <jni.h>

using namespace android_graphics_Bitmap_Config;

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "android_graphics_Bitmap"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING true

namespace android_graphics_Bitmap
{
	JavaVM* Bitmap::_jvm = 0;
	jclass Bitmap::_jcBitmap = 0;
	jmethodID Bitmap::_jmCreateBitmap = 0;
	jmethodID Bitmap::_jmGetHeight = 0;
	jmethodID Bitmap::_jmGetPixels = 0;
	jmethodID Bitmap::_jmGetWidth = 0;
	jmethodID Bitmap::_jmSetPixel = 0;
	jmethodID Bitmap::_jmSetPixels = 0;

	int Bitmap::InitJNI(JavaVM* jvm)
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
			const char* strClass = "android/graphics/Bitmap";
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Searching for %s", strClass);
#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Found %s", strClass);
#endif
				_jcBitmap = (jclass)env->NewGlobalRef(localRef);
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

	int Bitmap::FindJNI()
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
			const char* strMethod = "createBitmap";
			_jmCreateBitmap = env->GetStaticMethodID(_jcBitmap, strMethod, "(IILandroid/graphics/Bitmap$Config;)Landroid/graphics/Bitmap;");
			if (_jmCreateBitmap)
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
			const char* strMethod = "getHeight";
			_jmGetHeight = env->GetMethodID(_jcBitmap, strMethod, "()I");
			if (_jmGetHeight)
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
			const char* strMethod = "getPixels";
			_jmGetPixels = env->GetMethodID(_jcBitmap, strMethod, "([IIIIIII)V");
			if (_jmGetPixels)
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
			const char* strMethod = "getWidth";
			_jmGetWidth = env->GetMethodID(_jcBitmap, strMethod, "()I");
			if (_jmGetWidth)
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
			const char* strMethod = "setPixel";
			_jmSetPixel = env->GetMethodID(_jcBitmap, strMethod, "(III)V");
			if (_jmSetPixel)
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
			const char* strMethod = "setPixels";
			_jmSetPixels = env->GetMethodID(_jcBitmap, strMethod, "([IIIIIII)V");
			if (_jmSetPixels)
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

	Bitmap::Bitmap(jobject instance)
	{
		_instance = instance;
	}

	jobject Bitmap::GetInstance() const
	{
		return _instance;
	}

	void Bitmap::Dispose() const
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

	Bitmap Bitmap::createBitmap(int width, int height, Config config)
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return Bitmap(0);
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return Bitmap(0);
		}

		if (!_jcBitmap) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jcBitmap is null");
			return Bitmap(0);
		}

		if (!_jmCreateBitmap) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmCreateBitmap is null");
			return Bitmap(0);
		}

		if (!config.GetInstance()) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "config reference is null");
			return Bitmap(0);
		}

		jint arg1 = width;
		jint arg2 = height;
		jobject arg3 = config.GetInstance();
		jobject localRef = env->CallStaticObjectMethod(_jcBitmap, _jmCreateBitmap, arg1, arg2, arg3);
		if (!localRef)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "createBitmap returned null");
			return Bitmap(0);
		}

		jobject globalRef = (jobject)env->NewGlobalRef(localRef);
		env->DeleteLocalRef(localRef);

		return Bitmap(globalRef);
	}

	int Bitmap::getHeight() const
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_instance) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is null");
			return 0;
		}

		if (!_jmGetHeight) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetHeight is null");
			return 0;
		}

		int result = env->CallIntMethod(_instance, _jmGetHeight);
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "getHeight returned %d", result);
#endif
		return result;
	}

	void Bitmap::getPixels(int* pixels, int offset, int stride, int x, int y, int width, int height) const
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

		if (!_jmGetPixels) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetPixels is null");
			return;
		}

		int pixelCount = width*height;

		jintArray arg1 = env->NewIntArray(pixelCount);
		jint arg2 = offset;
		jint arg3 = stride;
		jint arg4 = x;
		jint arg5 = y;
		jint arg6 = width;
		jint arg7 = height;
		env->CallVoidMethod(_instance, _jmGetPixels, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		env->GetIntArrayRegion(arg1, 0, pixelCount, pixels);
		env->DeleteLocalRef(arg1);
	}

	int Bitmap::getWidth() const
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**)&env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_instance) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is null");
			return 0;
		}

		if (!_jmGetWidth) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetWidth is null");
			return 0;
		}

		int result = env->CallIntMethod(_instance, _jmGetWidth);
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "getWidth returned %d", result);
#endif
		return result;
	}

	void Bitmap::setPixel(int x, int y, int color) const
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

		if (!_jmSetPixel) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmSetPixel is null");
			return;
		}

		jint arg1 = x;
		jint arg2 = y;
		jint arg3 = color;
		env->CallVoidMethod(_instance, _jmSetPixel, arg1, arg2, arg3);
	}

	void Bitmap::setPixels(int* pixels, int offset, int stride, int x, int y, int width, int height) const
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

		if (!_jmSetPixels) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmSetPixels is null");
			return;
		}

		int pixelCount = width*height;

		jintArray arg1 = env->NewIntArray(pixelCount);
		env->SetIntArrayRegion(arg1, 0, pixelCount, pixels);
		jint arg2 = offset;
		jint arg3 = stride;
		jint arg4 = x;
		jint arg5 = y;
		jint arg6 = width;
		jint arg7 = height;
		env->CallVoidMethod(_instance, _jmSetPixels, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		env->DeleteLocalRef(arg1);
	}
}
