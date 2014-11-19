#include "OuyaInputView.h"

#include <android/log.h>
#include <jni.h>

#define LOG_TAG "tv_ouya_sdk_OuyaInputView"

#define ENABLE_VERBOSE_LOGGING true

namespace tv_ouya_sdk_OuyaInputView
{
	JavaVM* OuyaInputView::_jvm = 0;
	jclass OuyaInputView::_jcOuyaInputView = 0;
	jmethodID OuyaInputView::_jmGetInstance = 0;

	int OuyaInputView::InitJNI(JavaVM* jvm)
	{
		JNIEnv* env;
		jvm->GetEnv((void**) &env, JNI_VERSION_1_6);

		/*
		JNIEnv* env;
		if (jvm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}
		*/

		{
			const char* strClass = "tv/ouya/sdk/OuyaInputView";
			#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			#endif
			_jcOuyaInputView = env->FindClass(strClass);
			if (_jcOuyaInputView)
			{
				#if ENABLE_VERBOSE_LOGGING
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
				#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}
		_jvm = jvm;
		JNIFind();
		return JNI_OK;
	}

	void OuyaInputView::JNIFind()
	{
		JNIEnv* env;
		_jvm->GetEnv((void**) &env, JNI_VERSION_1_6);

		/*
		JNIEnv* env;
		if (_jvm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}
		*/

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		if (!_jcOuyaInputView)
		{
			return;
		}

		{
			const char* strMethod = "getInstance";
			#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for method %s", strMethod);
			#endif
			_jmGetInstance = env->GetStaticMethodID(_jcOuyaInputView, strMethod, "()Ltv/ouya/sdk/OuyaInputView;");
			if (_jmGetInstance)
			{
				#if ENABLE_VERBOSE_LOGGING
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
				#endif
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			}
		}
	}

	OuyaInputView* OuyaInputView::getInstance()
	{
		JNIEnv* env;
		_jvm->GetEnv((void**) &env, JNI_VERSION_1_6);

		/*
		JNIEnv* env;
		if (_jvm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}
		*/

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_jcOuyaInputView)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jcOuyaInputView is not valid!");
			return 0;
		}

		if (!_jmGetInstance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetInstance is not valid!");
			return 0;
		}

		jobject retVal = env->CallStaticObjectMethod(_jcOuyaInputView, _jmGetInstance);
		if (!retVal)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetInstance returned null!");
			return 0;
		}

		OuyaInputView* result = new OuyaInputView();
		result->_instance = retVal;
		return result;
	}

	jobject OuyaInputView::GetInstance()
	{
		return _instance;
	}
}
