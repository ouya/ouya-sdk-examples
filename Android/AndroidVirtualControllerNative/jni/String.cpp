#include <android/log.h>
#include <jni.h>

#include "String.h"

#define LOG_TAG "java_lang_String"

namespace java_lang_String
{
	JNIEnv* String::_env = 0;
	jclass String::_jcString = 0;
	jmethodID String::_mConstruct = 0;

	int String::InitJNI(JNIEnv* env)
	{
		{
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
		}

		{
			const char* strStringConstructor = "<init>";
			_mConstruct = env->GetMethodID(_jcString, strStringConstructor, "([B)V");
			if (_mConstruct)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strStringConstructor);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strStringConstructor);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	String::String(jstring objString)
	{
		_instance = objString;
	}

	String::String(const std::string& buffer)
	{
		_instance = _env->NewStringUTF(buffer.c_str());
	}

	String::String(jbyte* buffer, int length)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->NewStringUTF("");
		jbyteArray arg1 = _env->NewByteArray(length);
		_env->SetByteArrayRegion(arg1, 0, length, buffer);
		_env->CallObjectMethod(_instance, _mConstruct, arg1);
		_env->DeleteLocalRef(arg1);

		const char* nativeString = _env->GetStringUTFChars(_instance, 0);
		_strInstance = nativeString;
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Length=%d contents=%s", _strInstance.size(), _strInstance.c_str());
		_env->ReleaseStringUTFChars(_instance, nativeString);
	}

	jstring String::GetInstance() const
	{
		return _instance;
	}

	const std::string& String::ToString()
	{
		return _strInstance;
	}
}