#include <android/log.h>
#include <jni.h>

#include "Build.h"

#define LOG_TAG "android_os_Build"

namespace android_os_Build
{
	JNIEnv* Build::_env = 0;
	jclass Build::_jcBuild = 0;
	jfieldID Build::_fModel = 0;

	int Build::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/os/Build";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcBuild = env->FindClass(strClass);
			if (_jcBuild)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strBuildModel = "MODEL";
			_fModel = env->GetStaticFieldID(_jcBuild, strBuildModel, "Ljava/lang/String;");
			if (_fModel)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strBuildModel);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strBuildModel);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}
	
	std::string Build::MODEL()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return std::string();
		}

		jstring result = (jstring)_env->GetStaticObjectField(_jcBuild, _fModel);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get model");
			return 0;
		}

		if (result)
		{
			const char* nativeString = _env->GetStringUTFChars(result, 0);
			std::string retVal = std::string(nativeString);
			_env->ReleaseStringUTFChars(result, nativeString);

			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Get MODEL: %s", retVal.c_str());
			return retVal;
		}
		else
		{
			return std::string();
		}
	}
}