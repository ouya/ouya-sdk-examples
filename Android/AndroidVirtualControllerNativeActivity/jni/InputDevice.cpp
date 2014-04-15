#include <android/log.h>
#include <jni.h>

#include "InputDevice.h"

#define LOG_TAG "android_view_InputDevice"

namespace android_view_InputDevice
{
	JNIEnv* InputDevice::_env = 0;
	jclass InputDevice::_jcInputDevice = 0;
	jmethodID InputDevice::_mGetDevice = 0;
	jmethodID InputDevice::_mGetName = 0;

	int InputDevice::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/view/InputDevice";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcInputDevice = env->FindClass(strClass);
			if (_jcInputDevice)
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
			const char* strInputDeviceGetDevice = "getDevice";
			_mGetDevice = env->GetStaticMethodID(_jcInputDevice, strInputDeviceGetDevice, "(I)Landroid/view/InputDevice;");
			if (_mGetDevice)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strInputDeviceGetDevice);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strInputDeviceGetDevice);
				return JNI_ERR;
			}
		}

		{
			const char* strInputDeviceGetName = "getName";
			_mGetName = env->GetMethodID(_jcInputDevice, strInputDeviceGetName, "()Ljava/lang/String;");
			if (_mGetName)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strInputDeviceGetName);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strInputDeviceGetName);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	InputDevice::InputDevice(jobject inputDevice)
	{
		_instance = inputDevice;
	}

	jobject InputDevice::GetInstance()
	{
		return _instance;
	}

	InputDevice InputDevice::getDevice(int id)
	{
		InputDevice retVal = InputDevice(0);
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jint arg1 = id;
		jobject result = _env->CallStaticObjectMethod(_jcInputDevice, _mGetDevice, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get device");
			return retVal;
		}
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success get device");
			retVal = InputDevice(result);
			return retVal;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed get device");
			return retVal;
		}
	}

	std::string InputDevice::getName()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return std::string();
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return std::string();
		}

		jstring result = (jstring)_env->CallObjectMethod(_instance, _mGetName);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get name");
			return std::string();
		}

		if (result)
		{
			const char* nativeString = _env->GetStringUTFChars(result, 0);
			std::string retVal = std::string(nativeString);
			_env->ReleaseStringUTFChars(result, nativeString);

			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Get name: %s", retVal.c_str());
			return retVal;
		}
		else
		{
			return std::string();
		}
	}
}
