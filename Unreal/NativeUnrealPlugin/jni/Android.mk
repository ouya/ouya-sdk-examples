# Copyright (C) 2010 The Android Open Source Project
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

LOCAL_MODULE    := native-activity
LOCAL_SRC_FILES := \
	LaunchAndroid.cpp \
	AndroidJNI.cpp \
	OuyaSDK_Bitmap.cpp \
	OuyaSDK_BitmapConfig.cpp \
	OuyaSDK_Bundle.cpp \
	OuyaSDK_CallbackSingleton.cpp \
	OuyaSDK_CallbacksContentInit.cpp \
	OuyaSDK_CallbacksContentDelete.cpp \
	OuyaSDK_CallbacksContentDownload.cpp \
	OuyaSDK_CallbacksContentPublish.cpp \
	OuyaSDK_CallbacksContentSave.cpp \
	OuyaSDK_CallbacksContentSearchInstalled.cpp \
	OuyaSDK_CallbacksContentSearchPublished.cpp \
	OuyaSDK_CallbacksContentUnpublish.cpp \
	OuyaSDK_CallbacksInitOuyaPlugin.cpp \
	OuyaSDK_CallbacksRequestGamerInfo.cpp \
	OuyaSDK_CallbacksRequestProducts.cpp \
	OuyaSDK_CallbacksRequestPurchase.cpp \
	OuyaSDK_CallbacksRequestReceipts.cpp \
	OuyaSDK_Color.cpp \
	OuyaSDK_GamerInfo.cpp \
	OuyaSDK_InputStream.cpp \
	OuyaSDK_JSONArray.cpp \
	OuyaSDK_JSONObject.cpp \
	OuyaSDK_OutputStream.cpp \
	OuyaSDK_OuyaContent.cpp \
	OuyaSDK_OuyaController.cpp \
	OuyaSDK_OuyaInputView.cpp \
	OuyaSDK_OuyaMod.cpp \
	OuyaSDK_OuyaModEditor.cpp \
	OuyaSDK_OuyaModScreenshot.cpp \
	OuyaSDK_PluginOuya.cpp \
	OuyaSDK_Product.cpp \
	OuyaSDK_Receipt.cpp \
	OuyaSDK_String.cpp
LOCAL_CFLAGS    := -DPLATFORM_ANDROID -DANDROID -DECLIPSE
LOCAL_LDLIBS    := -llog -landroid -lEGL -lGLESv1_CM
LOCAL_STATIC_LIBRARIES := android_native_app_glue

include $(BUILD_SHARED_LIBRARY)

$(call import-module,android/native_app_glue)
