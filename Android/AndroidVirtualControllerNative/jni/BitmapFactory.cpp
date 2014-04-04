#include <android/log.h>
#include <jni.h>

#include "BitmapFactory.h"

#define LOG_TAG "android_graphics_BitmapFactory"

namespace android_graphics_BitmapFactory
{
	JNIEnv* BitmapFactory::Options::_env = 0;
	jclass BitmapFactory::Options::_jcOptions = 0;
	jmethodID BitmapFactory::Options::_mOptionsConstruct = 0;
	jfieldID BitmapFactory::Options::_jbInScaled = 0;
	jobject BitmapFactory::Options::_instance = 0;

	int BitmapFactory::Options::InitJNI(JNIEnv* env)
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "****************");

		const char* strOptionsClass = "android/graphics/BitmapFactory$Options";
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strOptionsClass);
		_jcOptions = env->FindClass(strOptionsClass);
		if (_jcOptions)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strOptionsClass);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strOptionsClass);
			return JNI_ERR;
		}

		const char* strOptionsConstruct = "<init>";
		_mOptionsConstruct = env->GetMethodID(_jcOptions, strOptionsConstruct, "()V");
		if (_mOptionsConstruct)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strOptionsConstruct);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strOptionsConstruct);
			return JNI_ERR;
		}

		const char* strOptionsInScaled = "inScaled";
		_jbInScaled = env->GetFieldID(_jcOptions, strOptionsInScaled, "Z");
		if (_jbInScaled)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strOptionsInScaled);
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strOptionsInScaled);
			return JNI_ERR;
		}

		_env = env;

		return JNI_OK;
	}

	BitmapFactory::Options::Options()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->NewObject(_jcOptions, _mOptionsConstruct);
		if (_instance)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Created Object");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to create Object");
			return;
		}

		_env->CallVoidMethod(_instance, _mOptionsConstruct);
	}

	BitmapFactory::Options::~Options()
	{
		if (_instance)
		{
			_env->DeleteLocalRef(_instance);
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Destroyed Object");
		}
	}

	bool BitmapFactory::Options::get_inScaled()
	{
		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is invalid");
			return false;
		}
		bool result = _env->GetBooleanField(_instance, _jbInScaled);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "get Object.inScaled=true");
		}
		else
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "get Object.inScaled=false");
		}
		return result;
	}

	void BitmapFactory::Options::set_inScaled(bool inScaled)
	{
		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is invalid");
			return;
		}

		_env->SetBooleanField(_instance, _jbInScaled, inScaled);
		if (inScaled)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "set Object.inScaled=true");
		}
		else
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "set Object.inScaled=false");
		}
	}
}