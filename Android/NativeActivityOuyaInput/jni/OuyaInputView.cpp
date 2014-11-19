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
	jmethodID OuyaInputView::_jmJavaDispatchKeyEvent = 0;

	int OuyaInputView::InitJNI(JavaVM* jvm)
	{
		_jvm = jvm;

		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return JNI_ERR;
		}

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

		JNIFind();
		return JNI_OK;
	}

	void OuyaInputView::JNIFind()
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return;
		}

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

		{
			const char* strMethod = "javaDispatchKeyEvent";
			#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for method %s", strMethod);
			#endif
			_jmJavaDispatchKeyEvent = env->GetMethodID(_jcOuyaInputView, strMethod, "(JJIIIIIIII)Z");
			if (_jmJavaDispatchKeyEvent)
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
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return 0;
		}

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

	bool OuyaInputView::javaDispatchKeyEvent(long long downTime, long long eventTime, int action, int code,
		int repeat, int metaState, int deviceId, int scancode, int flags, int source)
	{
		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return false;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return false;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is not valid!");
			return false;
		}

		if (!_jmJavaDispatchKeyEvent)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmJavaDispatchKeyEvent is not valid!");
			return false;
		}

		return env->CallBooleanMethod(_instance, _jmJavaDispatchKeyEvent,
			downTime, eventTime, action, code,
			repeat, metaState, deviceId, scancode, flags, source);
	}

	bool OuyaInputView::javaDispatchGenericMotionEvent(
		long long downTime,
		long long eventTime,
		int action,
		int pointerCount,
		int metaState,
		int buttonState,
		float xPrecision,
		float yPrecision,
		int deviceId,
		int edgeFlags,
		int source,
		int flags,
		long long* pointerPropertiesId,
		int* pointerPropertiesToolType,
		float* pointerCoordsOrientation,
		float* pointerCoordsPressure,
		float* pointerCoordsSize,
		float* pointerCoordsToolMajor,
		float* pointerCoordsToolMinor,
		float* pointerCoordsTouchMajor,
		float* pointerCoordsTouchMinor,
		float* pointerCoordsX,
		float* pointerCoordsY,
		int axisCount,
		int* axisIndexes,
		float* axisValues)
	{
		return false;

		/*
		JNIEnv* env;
		if (_jvm->GetEnv((void**) &env, JNI_VERSION_1_6) != JNI_OK) {
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get JNI environment!");
			return false;
		}

		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return false;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is not valid!");
			return false;
		}

		if (!_jmJavaDispatchKeyEvent)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmJavaDispatchKeyEvent is not valid!");
			return false;
		}

		return env->CallBooleanMethod(_instance, _jmJavaDispatchKeyEvent,
			downTime, eventTime, action, code,
			repeat, metaState, deviceId, scancode, flags, source);
		*/
	}

	jobject OuyaInputView::GetInstance()
	{
		return _instance;
	}
}
