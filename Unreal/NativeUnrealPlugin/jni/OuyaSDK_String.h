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

#ifndef __JAVA_LANG_STRING_H__
#define __JAVA_LANG_STRING_H__

#if PLATFORM_ANDROID

#include <string>

#include <jni.h>

namespace java_lang_String
{
	class String
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static int FindJNI();
		String(const std::string& val);
		String(jstring instance);
		jstring GetInstance() const;
		void Dispose() const;
		int* getBytes(int& length) const;
	private:
		static JavaVM* _jvm;
		static jclass _jcString;
		static jmethodID _jmGetBytes;
		jstring _instance;
	};
}

#endif

#endif
