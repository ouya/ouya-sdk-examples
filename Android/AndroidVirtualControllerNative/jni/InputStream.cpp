#include <android/log.h>
#include <jni.h>

#include "InputStream.h"

#define LOG_TAG "java_io_InputStream"

namespace java_io_InputStream
{
	JNIEnv* InputStream::_env = 0;
	jclass InputStream::_jcInputStream = 0;
	jmethodID InputStream::_mClose = 0;

	int InputStream::InitJNI(JNIEnv* env)
	{
		const char* strInputStreamClass = "java/io/InputStream";
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strInputStreamClass);
		_jcInputStream = env->FindClass(strInputStreamClass);
		if (_jcInputStream)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strInputStreamClass);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strInputStreamClass);
			return JNI_ERR;
		}

		const char* strInputStreamClose = "close";
		_mClose = env->GetMethodID(_jcInputStream, strInputStreamClose, "()V");
		if (_mClose)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strInputStreamClose);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strInputStreamClose);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	InputStream::InputStream()
	{
		_instance = 0;
	}

	InputStream::InputStream(const jobject& inputStream)
	{
		_instance = inputStream;
	}

	jobject InputStream::GetInstance()
	{
		return _instance;
	}

	void InputStream::SetInstance(jobject inputStream)
	{
		_instance = inputStream;
	}

	void InputStream::close()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Close InputStream");
		_env->CallVoidMethod(_instance, _mClose);
	}
}