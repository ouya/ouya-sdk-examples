#include <android/log.h>
#include <jni.h>

#include "Bitmap.h"

#define LOG_TAG "android_graphics_Bitmap"

namespace android_graphics_Bitmap
{
	JNIEnv* Bitmap::_env = 0;
	jclass Bitmap::_jcBitmap = 0;
	jmethodID Bitmap::_mGetHeight = 0;
	jmethodID Bitmap::_mGetWidth = 0;
	jmethodID Bitmap::_mRecycle = 0;

	int Bitmap::InitJNI(JNIEnv* env)
	{
		{
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
		}

		{
			const char* strBitmapGetHeight = "getHeight";
			_mGetHeight = env->GetMethodID(_jcBitmap, strBitmapGetHeight, "()I");
			if (_mGetHeight)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strBitmapGetHeight);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strBitmapGetHeight);
				return JNI_ERR;
			}
	}

		{
			const char* strBitmapGetWidth = "getWidth";
			_mGetWidth = env->GetMethodID(_jcBitmap, strBitmapGetWidth, "()I");
			if (_mGetWidth)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strBitmapGetWidth);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strBitmapGetWidth);
				return JNI_ERR;
			}
		}

		{
			const char* strBitmapRecycle = "recycle";
			_mRecycle = env->GetMethodID(_jcBitmap, strBitmapRecycle, "()V");
			if (_mRecycle)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strBitmapRecycle);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strBitmapRecycle);
				return JNI_ERR;
			}
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

	int Bitmap::getHeight()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		const jint result = _env->CallIntMethod(_instance, _mGetHeight);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on get height: %d", result);
			return result;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get height");
			return 0;
		}
	}

	int Bitmap::getWidth()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		const jint result = _env->CallIntMethod(_instance, _mGetWidth);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on get width: %d", (int)result);
			return result;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get width");
			return 0;
		}
	}

	void Bitmap::recycle()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_env->CallVoidMethod(_instance, _mRecycle);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on recycle bitmap");
	}
}