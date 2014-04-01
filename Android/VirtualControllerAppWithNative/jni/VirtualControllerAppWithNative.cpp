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
#include <stdio.h>
#include <string.h>
#include <jni.h>

#include <android/log.h>

#define LOG_TAG "VirtualControllerAppWithNative.cpp"

/* See the corresponding Java source
 * file located at:
 *
 *   src/tv/ouya/examples/android/virtualcontrollerappwithnative/OuyaActivity.java
 */

void dispatchGenericMotionEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint axis,
	jfloat val)
{
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Device=%d axis=%d val=%f", deviceId, axis, val);
}

void dispatchKeyEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint keyCode,
	jint action)
{
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Device=%d KeyCode=%d Action=%d", deviceId, keyCode, action);
}

static JNINativeMethod method_table[] = {
	{ "dispatchGenericMotionEventNative", "(IIF)V", (void *) dispatchGenericMotionEventNative }
};

static int method_table_size = sizeof(method_table) / sizeof(method_table[0]);

static JNINativeMethod method_table2[] = {
	{ "dispatchKeyEventNative", "(III)V", (void *) dispatchKeyEventNative }
};

static int method_table_size2 = sizeof(method_table2) / sizeof(method_table2[0]);

jint JNI_OnLoad(JavaVM* vm, void* reserved)
{
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JNI_OnLoad");

    JNIEnv* env;
    if (vm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK) {
    	return JNI_ERR;
    }

    jclass clazz = env->FindClass("tv/ouya/examples/android/virtualcontrollerappwithnative/OuyaActivity");
    if (clazz)
    {
    	jint ret = env->RegisterNatives(clazz, method_table, method_table_size);
    	ret = env->RegisterNatives(clazz, method_table2, method_table_size2);
    	env->DeleteLocalRef(clazz);
    }
    else
    {
    	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Failed to find OuyaActivity");
    	return JNI_ERR;
    }

    return JNI_VERSION_1_6;
}
