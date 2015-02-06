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

#ifndef __JAVA_IO_OUTPUTSTREAM_H__
#define __JAVA_IO_OUTPUTSTREAM_H__

#if PLATFORM_ANDROID

#include <jni.h>

namespace java_io_OutputStream
{
	class OutputStream
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static int FindJNI();
		OutputStream(jobject instance);
		jobject GetInstance() const;
		void Dispose() const;
		void close() const;
		void write(signed char buffer[], int length) const;
		void flush() const;
	private:
		static JavaVM* _jvm;
		static jclass _jcOutputStream;
		static jmethodID _jmClose;
		static jmethodID _jmFlush;
		static jmethodID _jmWrite;
		jobject _instance;
	};
}

#endif

#endif
