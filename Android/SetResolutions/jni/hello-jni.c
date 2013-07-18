/*
 * Copyright (C) 2009 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
#include <android_native_app_glue.h>
#include <string.h>
#include <jni.h>
#include <android/log.h>

#define  LOG_TAG    "setresolutions"
#define  LOGI(...)  __android_log_print(ANDROID_LOG_INFO,LOG_TAG,__VA_ARGS__)
#define  LOGW(...)  __android_log_print(ANDROID_LOG_WARN,LOG_TAG,__VA_ARGS__)
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)

/* This is a trivial JNI example where we use a native method
 * to return a new VM String. See the corresponding Java source
 * file located at:
 *
 *   apps/samples/hello-jni/project/src/com/example/hellojni/HelloJni.java
 */
jstring
Java_tv_ouya_android_setresolutions_SetResolutions_stringFromJNI( JNIEnv* env,
                                                  jobject thiz,
                                                  jobject surface)
{
	/*
	ANativeWindow* window = ANativeWindow_fromSurface(env, surface);
	if (window != NULL)
	{
		//LOGW("FOUND ANativeWindow");
		//ANativeWindow_setBuffersGeometry(wnd, 0, 0, format)
	}
	else
	{
		//LOGW("FAIL ANativeWindow");
	}
	*/

	/*
	ANativeWindow_Buffer buffer;
	if (ANativeWindow_lock(window, &buffer, NULL) < 0)
	{
		LOGW("Unable to lock window buffer");

		ANativeWindow_unlockAndPost(window);
	}
	*/
    return (*env)->NewStringUTF(env, "Hello from JNI !");
}
