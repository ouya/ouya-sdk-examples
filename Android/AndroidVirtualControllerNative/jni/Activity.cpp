#include <android/log.h>
#include <jni.h>

#include "Activity.h"

#define LOG_TAG "android_app_Activity"

namespace android_app_Activity
{
	JNIEnv* Activity::_env = 0;
	jclass Activity::_jcActivity = 0;
	jmethodID Activity::_mGetApplicationContext = 0;

	int Activity::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");

		const char* strActivityClass = "android/app/Activity";
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strActivityClass);
		_jcActivity = env->FindClass(strActivityClass);
		if (_jcActivity)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strActivityClass);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strActivityClass);
			return JNI_ERR;
		}

		const char* strActivityGetApplicationContext = "getApplicationContext";
		_mGetApplicationContext = env->GetMethodID(_jcActivity, strActivityGetApplicationContext, "()Landroid/content/Context;");
		if (_mGetApplicationContext)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strActivityGetApplicationContext);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strActivityGetApplicationContext);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	Activity::Activity(jobject activity)
		: Context::Context(activity)
	{
		_instance = activity;
	}

	Activity::Activity(const Activity& activity) :
		Context::Context(activity)
	{
		_instance = activity._instance;
	}

	Activity::~Activity()
	{
		if (_instance)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Destroy Object");
			_env->DeleteLocalRef(_instance);
		}
	}

	android_content_Context::Context Activity::getApplicationContext()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return android_content_Context::Context(0);
		}

		jobject result = _env->CallObjectMethod(_instance, _mGetApplicationContext);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get application context");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get application context");
		}
		return android_content_Context::Context(result);
	}
}