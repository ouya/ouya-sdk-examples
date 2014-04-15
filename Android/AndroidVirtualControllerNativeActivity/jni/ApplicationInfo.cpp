#include <android/log.h>
#include <jni.h>

#include "ApplicationInfo.h"

#define LOG_TAG "android_content_pm_ApplicationInfo"

namespace android_content_pm_ApplicationInfo
{
	JNIEnv* ApplicationInfo::_env = 0;
	jclass ApplicationInfo::_jcApplicationInfo = 0;
	jfieldID ApplicationInfo::_fSourceDir = 0;

	int ApplicationInfo::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/content/pm/ApplicationInfo";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcApplicationInfo = env->FindClass(strClass);
			if (_jcApplicationInfo)
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
			const char* strSourceDir = "sourceDir";
			_fSourceDir = env->GetFieldID(_jcApplicationInfo, strSourceDir, "Ljava/lang/String;");
			if (_fSourceDir)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strSourceDir);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strSourceDir);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	ApplicationInfo::ApplicationInfo(jobject applicationInfo)
	{
		_instance = applicationInfo;
	}	

	jobject ApplicationInfo::GetInstance() const
	{
		return _instance;
	}

	std::string ApplicationInfo::sourceDir()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return "";
		}

		jstring result = (jstring)_env->GetObjectField(_instance, _fSourceDir);

		const char* nativeString = _env->GetStringUTFChars(result, 0);
		std::string retVal = nativeString;
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "sourceDir=%s", retVal.c_str());
		_env->ReleaseStringUTFChars(result, nativeString);

		return retVal;
	}
}