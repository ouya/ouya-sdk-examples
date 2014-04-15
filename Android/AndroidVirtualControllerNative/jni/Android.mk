# Copyright (C) 2009 The Android Open Source Project
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE    := AndroidVirtualControllerNative
LOCAL_C_INCLUDES := \
Activity.h \
ApplicationInfo.h \
AssetManager.h \
AudioManager.h \
Bitmap.h \
BitmapFactory.h \
Build.h \
ClassLoader.h \
Context.h \
DexFile.h \
GLUtils.h \
InputDevice.h \
InputStream.h \
JSONArray.h \
JSONObject.h \
MappingParser.h \
OuyaController.h \
SoundPool.h \
String.h \
System.h

LOCAL_SRC_FILES := \
Activity.cpp \
ApplicationInfo.cpp \
AssetManager.cpp \
AudioManager.cpp \
Bitmap.cpp \
BitmapFactory.cpp \
Build.cpp \
ClassLoader.cpp \
Context.cpp \
DexFile.cpp \
GLUtils.cpp \
InputDevice.cpp \
InputStream.cpp \
JSONArray.cpp \
JSONObject.cpp \
main.cpp \
MappingParser.cpp \
OuyaController.cpp \
SoundPool.cpp \
String.cpp \
System.cpp

LOCAL_LDLIBS := -lc -lm -llog -landroid -lEGL -lGLESv1_CM -ljnigraphics -lgcc -lstlport
LOCAL_STATIC_LIBRARIES := android_native_app_glue

LOCAL_PREBUILT_STATIC_JAVA_LIBRARIES := ouya-sdk.jar
include $(BUILD_MULTI_PREBUILT)

include $(BUILD_SHARED_LIBRARY)
