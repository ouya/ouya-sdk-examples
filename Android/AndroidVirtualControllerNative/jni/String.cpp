#include <android/log.h>
#include <jni.h>

#include "String.h"

#define LOG_TAG "java_lang_String"

namespace java_lang_String
{
	JNIEnv* String::_env = 0;
	jclass String::_jcString = 0;

	int String::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");

		const char* strStringClass = "java/lang/String";
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strStringClass);
		_jcString = env->FindClass(strStringClass);
		if (_jcString)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strStringClass);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strStringClass);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	String::String(jstring objString)
	{
		_instance = objString;
	}

	String::String(const char* bytes)
	{
		if (_instance)
		{
			_env->DeleteLocalRef(_instance);
		}

		if (bytes)
		{
			_instance = _env->NewStringUTF(bytes);
		}
	}

	jstring String::GetInstance() const
	{
		return _instance;
	}
}