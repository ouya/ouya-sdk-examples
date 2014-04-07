#include <android/log.h>
#include <jni.h>

#include "Bitmap.h"

#define LOG_TAG "android_graphics_Bitmap"

namespace android_graphics_Bitmap
{
	JNIEnv* Bitmap::_env = 0;
	jclass Bitmap::_jcBitmap = 0;

	int Bitmap::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");

		const char* strBitmapClass = "android/graphics/Bitmap";
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strBitmapClass);
		_jcBitmap = env->FindClass(strBitmapClass);
		if (_jcBitmap)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strBitmapClass);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strBitmapClass);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	Bitmap::Bitmap(jobject bitmap)
	{
		_instance = bitmap;
	}

	jobject Bitmap::GetInstance() const
	{
		return _instance;
	}
}