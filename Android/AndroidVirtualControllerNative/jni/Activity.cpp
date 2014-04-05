#include <android/log.h>
#include <jni.h>

#include "Activity.h"

#define LOG_TAG "android_app_Activity"

namespace android_app_Activity
{
	JNIEnv* Activity::_env = 0;
	jclass Activity::_jcActivity = 0;

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
		_instance = 0;
	}
}