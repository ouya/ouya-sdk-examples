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

#ifndef __ANDROID_GRAPHICS_BITMAP_CONFIG_H__
#define __ANDROID_GRAPHICS_BITMAP_CONFIG_H__

#if PLATFORM_ANDROID

#include <jni.h>

namespace android_graphics_Bitmap_Config
{
	class Config
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static int FindJNI();
		Config(jobject instance);
		jobject GetInstance() const;
		void Dispose() const;
		static Config ARGB_8888();
	private:
		static JavaVM* _jvm;
		static jclass _jcConfig;
		static jfieldID _jfARGB_8888;
		jobject _instance;
	};
}

#endif

#endif
