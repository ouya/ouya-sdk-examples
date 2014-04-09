#include <android/log.h>
#include <jni.h>

#include "InputStream.h"

#define LOG_TAG "java_io_InputStream"

namespace java_io_InputStream
{
	JNIEnv* InputStream::_env = 0;
	jclass InputStream::_jcInputStream = 0;
	jmethodID InputStream::_mAvailable = 0;
	jmethodID InputStream::_mRead = 0;
	jmethodID InputStream::_mClose = 0;

	int InputStream::InitJNI(JNIEnv* env)
	{
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
		}

		{
			const char* strInputStreamAvilable = "available";
			_mAvailable = env->GetMethodID(_jcInputStream, strInputStreamAvilable, "()I");
			if (_mAvailable)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strInputStreamAvilable);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strInputStreamAvilable);
				return JNI_ERR;
			}
		}

		{
			const char* strInputStreamRead = "read";
			_mRead = env->GetMethodID(_jcInputStream, strInputStreamRead, "([B)I");
			if (_mAvailable)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strInputStreamRead);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strInputStreamRead);
				return JNI_ERR;
			}
		}

		{
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

	int InputStream::available()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		jint result = _env->CallIntMethod(_instance, _mAvailable);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on available: %d", result);
		return result;
	}

	int InputStream::read(jbyte* buffer, int length)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		jbyteArray arg1 =_env->NewByteArray(length);
		jint result = _env->CallIntMethod(_instance, _mRead, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to read");
			return 0;
		}
		_env->GetByteArrayRegion(arg1, 0, length, buffer);
		_env->DeleteLocalRef(arg1);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on read: %d", result);

		return result;
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