#include <android/log.h>
#include <jni.h>

#include "GLUtils.h"

#define LOG_TAG "android_opengl_GLUtils"

namespace android_opengl_GLUtils
{
	JNIEnv* GLUtils::_env = 0;
	jclass GLUtils::_jcGlUtils = 0;
	jmethodID GLUtils::_mTexImage2D = 0;

	int GLUtils::InitJNI(JNIEnv* env)
	{
		{
			const char* strGlUtilsClass = "android/opengl/GLUtils";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strGlUtilsClass);
			_jcGlUtils = env->FindClass(strGlUtilsClass);
			if (_jcGlUtils)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strGlUtilsClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strGlUtilsClass);
				return JNI_ERR;
			}
		}

		{
		const char* strGlUtilsTexImage2D = "texImage2D";
			_mTexImage2D = env->GetStaticMethodID(_jcGlUtils, strGlUtilsTexImage2D, "(IILandroid/graphics/Bitmap;I)V");
			if (_mTexImage2D)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strGlUtilsTexImage2D);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strGlUtilsTexImage2D);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	void GLUtils::texImage2D(int target, int level, android_graphics_Bitmap::Bitmap bitmap, int border)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		jobject arg3 = bitmap.GetInstance();
		_env->CallStaticIntMethod(_jcGlUtils, _mTexImage2D, target, level, arg3, border);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on texImage2D");
	}
}