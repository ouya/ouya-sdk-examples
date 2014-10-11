#include <android/log.h>
#include <jni.h>

#include "OuyaController.h"

#define LOG_TAG "tv_ouya_console_api_OuyaController"
#define VERBOSE_LOGGING false

namespace tv_ouya_console_api_OuyaController
{
	JNIEnv* OuyaController::_env = 0;
	jclass OuyaController::_jcOuyaController = 0;
	jmethodID OuyaController::_jmGetButtonData = 0;
	jmethodID OuyaController::_jmGetControllerByPlayer = 0;
	jmethodID OuyaController::_jmGetDeviceName = 0;
	jmethodID OuyaController::_jmShowCursor = 0;
	jfieldID OuyaController::_jfAxisLsX = 0;
	jfieldID OuyaController::_jfAxisLsY = 0;
	jfieldID OuyaController::_jfAxisRsX = 0;
	jfieldID OuyaController::_jfAxisRsY = 0;
	jfieldID OuyaController::_jfAxisL2 = 0;
	jfieldID OuyaController::_jfAxisR2 = 0;
	jfieldID OuyaController::_jfButtonO = 0;
	jfieldID OuyaController::_jfButtonU = 0;
	jfieldID OuyaController::_jfButtonY = 0;
	jfieldID OuyaController::_jfButtonA = 0;
	jfieldID OuyaController::_jfButtonL1 = 0;
	jfieldID OuyaController::_jfButtonL3 = 0;
	jfieldID OuyaController::_jfButtonR1 = 0;
	jfieldID OuyaController::_jfButtonR3 = 0;
	jfieldID OuyaController::_jfButtonMenu = 0;
	jfieldID OuyaController::_jfButtonDpadUp = 0;
	jfieldID OuyaController::_jfButtonDpadRight = 0;
	jfieldID OuyaController::_jfButtonDpadDown = 0;
	jfieldID OuyaController::_jfButtonDpadLeft = 0;

	int OuyaController::InitJNI(JNIEnv* env)
	{
		{
			const char* strOuyaControllerClass = "tv/ouya/console/api/OuyaController";
			if (VERBOSE_LOGGING)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strOuyaControllerClass);
			}
			_jcOuyaController = env->FindClass(strOuyaControllerClass);
			if (_jcOuyaController)
			{
				if (VERBOSE_LOGGING)
				{
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strOuyaControllerClass);
				}
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strOuyaControllerClass);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	void OuyaController::FindJNI()
	{
		{
			JavaVM* jvm;
			_env->GetJavaVM(&jvm);
			jvm->AttachCurrentThread(&_env, 0);
		}

		{
			const char* strMethod = "getButtonData";
			_jmGetButtonData = _env->GetStaticMethodID(_jcOuyaController, strMethod, "(I)Ltv/ouya/console/api/OuyaController$ButtonData;");
			if (_jmGetButtonData)
			{
				if (VERBOSE_LOGGING)
				{
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
				}
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		{
			const char* strMethod = "getControllerByPlayer";
			_jmGetControllerByPlayer = _env->GetStaticMethodID(_jcOuyaController, strMethod, "(I)Ltv/ouya/console/api/OuyaController;");
			if (_jmGetControllerByPlayer)
			{
				if (VERBOSE_LOGGING)
				{
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
				}
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		{
			const char* strMethod = "getDeviceName";
			_jmGetDeviceName = _env->GetMethodID(_jcOuyaController, strMethod, "()Ljava/lang/String;");
			if (_jmGetDeviceName)
			{
				if (VERBOSE_LOGGING)
				{
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
				}
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}

		{
			const char* strMethod = "showCursor";
			_jmShowCursor = _env->GetStaticMethodID(_jcOuyaController, strMethod, "(Z)V");
			if (_jmShowCursor)
			{
				if (VERBOSE_LOGGING)
				{
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
				}
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
				return;
			}
		}
	}

	void OuyaController::SetInstance(jobject instance)
	{
		_instance = instance;
	}

	void OuyaController::Dispose()
	{
		if (_instance)
		{
			_env->DeleteLocalRef(_instance);
			_instance = 0;
		}
	}

	const int OuyaController::AXIS_LS_X()
	{
		return 0;
	}

	const int OuyaController::AXIS_LS_Y()
	{
		return 1;
	}

	const int OuyaController::AXIS_RS_X()
	{
		return 11;
	}

	const int OuyaController::AXIS_RS_Y()
	{
		return 14;
	}

	const int OuyaController::AXIS_L2()
	{
		return 17;
	}

	const int OuyaController::AXIS_R2()
	{
		return 18;
	}

	const int OuyaController::BUTTON_O()
	{
		return 96;
	}

	const int OuyaController::BUTTON_U()
	{
		return 99;
	}

	const int OuyaController::BUTTON_Y()
	{
		return 100;
	}

	const int OuyaController::BUTTON_A()
	{
		return 97;
	}

	const int OuyaController::BUTTON_L1()
	{
		return 102;
	}

	const int OuyaController::BUTTON_L3()
	{
		return 106;
	}

	const int OuyaController::BUTTON_R1()
	{
		return 103;
	}

	const int OuyaController::BUTTON_R3()
	{
		return 107;
	}

	const int OuyaController::BUTTON_MENU()
	{
		return 82;
	}

	const int OuyaController::BUTTON_DPAD_UP()
	{
		return 19;
	}

	const int OuyaController::BUTTON_DPAD_RIGHT()
	{
		return 22;
	}

	const int OuyaController::BUTTON_DPAD_DOWN()
	{
		return 20;
	}

	const int OuyaController::BUTTON_DPAD_LEFT()
	{
		return 21;
	}

	OuyaController* OuyaController::getControllerByPlayer(int playerNum)
	{
		FindJNI();

		if (!_jcOuyaController)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jcOuyaController is not initialized");
			return 0;
		}

		if (!_jmGetControllerByPlayer)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetControllerByPlayer is not initialized");
			return 0;
		}

		jint arg1 = playerNum;
		jobject retVal = _env->CallStaticObjectMethod(_jcOuyaController, _jmGetControllerByPlayer, arg1);
		if (!retVal)
		{
			// May return null if controller isn't connected
			//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "getControllerByPlayer returned null");
			return 0;
		}

		OuyaController* result = new OuyaController();
		result->SetInstance(retVal);
		return result;
	}

	void OuyaController::showCursor(bool visible)
	{
		FindJNI();

		if (!_jcOuyaController)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jcOuyaController is not initialized");
			return;
		}

		if (!_jmShowCursor)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmShowCursor is not initialized");
			return;
		}

		_env->CallStaticVoidMethod(_jcOuyaController, _jmShowCursor, visible);
	}

	const std::string& OuyaController::getDeviceName()
	{
		FindJNI();

		_deviceName = "Unavailable";

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_instance is not initialized");
			return _deviceName;
		}

		if (!_jmGetDeviceName)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jmGetDeviceName is not initialized");
			return _deviceName;
		}

		jstring retVal = (jstring)_env->CallObjectMethod(_instance, _jmGetDeviceName);
		if (!retVal)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "getDeviceName returned null");
			return _deviceName;
		}

		const char* nativeString = _env->GetStringUTFChars(retVal, 0);
		_deviceName = nativeString;
		_env->ReleaseStringUTFChars(retVal, nativeString);
		_env->DeleteLocalRef(retVal);

		return _deviceName;
	}
}
