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

#ifndef __ANDROID_GRAPHICS_BITMAP_H__
#define __ANDROID_GRAPHICS_BITMAP_H__

#if PLATFORM_ANDROID

#include "OuyaSDK_BitmapConfig.h"

#include <jni.h>

namespace android_graphics_Bitmap
{
	class Bitmap
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static int FindJNI();
		Bitmap(jobject instance);
		jobject GetInstance() const;
		void Dispose() const;
		static Bitmap createBitmap(int width, int height, android_graphics_Bitmap_Config::Config config);
		int getHeight() const;
		void getPixels(int* pixels, int offset, int stride, int x, int y, int width, int height) const;
		int getWidth() const;
		void setPixel(int x, int y, int color) const;
		void setPixels(int* pixels, int offset, int stride, int x, int y, int width, int height) const;
	private:
		static JavaVM* _jvm;
		static jclass _jcBitmap;
		static jmethodID _jmCreateBitmap;
		static jmethodID _jmGetHeight;
		static jmethodID _jmGetPixels;
		static jmethodID _jmGetWidth;
		static jmethodID _jmSetPixel;
		static jmethodID _jmSetPixels;
		jobject _instance;
	};
}

#endif

#endif
