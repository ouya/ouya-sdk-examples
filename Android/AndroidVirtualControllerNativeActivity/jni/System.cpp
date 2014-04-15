#include <android/log.h>
#include <jni.h>

#include "System.h"

#define LOG_TAG "java_lang_System"

namespace java_lang_System
{
	JNIEnv* System::_env = 0;
	jclass System::_jcSystem = 0;
	jmethodID System::_mLoadLibrary = 0;

	int System::InitJNI(JNIEnv* env)
	{
		{
			const char* strSystemClass = "java/lang/System";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strSystemClass);
			_jcSystem = env->FindClass(strSystemClass);
			if (_jcSystem)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strSystemClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strSystemClass);
				return JNI_ERR;
			}
		}

		{
			const char* strSystemLoadLibrary = "loadLibrary";
			_mLoadLibrary = env->GetStaticMethodID(_jcSystem, strSystemLoadLibrary, "(Ljava/lang/String;)V");
			if (_mLoadLibrary)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strSystemLoadLibrary);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strSystemLoadLibrary);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	void System::loadLibrary(const std::string& libName)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		jstring arg1 = _env->NewStringUTF(libName.c_str());
		_env->CallStaticVoidMethod(_jcSystem, _mLoadLibrary, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to load library");
		}
		else
		{
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success on load library");
		}
		_env->DeleteLocalRef(arg1);
	}
}