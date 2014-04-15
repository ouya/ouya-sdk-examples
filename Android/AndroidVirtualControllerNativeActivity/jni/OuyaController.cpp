#include <android/log.h>
#include <jni.h>

#include "ClassLoader.h"
#include "OuyaController.h"

#define LOG_TAG "tv_ouya_console_api_OuyaController"

namespace tv_ouya_console_api_OuyaController
{
	JNIEnv* OuyaController::_env = 0;
	jclass OuyaController::_jcOuyaController = 0;
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
		/*
		if (true)
		{
			return JNI_OK;
		}
		*/

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "***************");

		/*
		{
			java_lang_ClassLoader::ClassLoader classLoader = java_lang_ClassLoader::ClassLoader::getSystemClassLoader();
			std::string className = "tv.ouya.console.api.OuyaController";
			std::string result = classLoader.loadClass(className);
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded %s", result.c_str());
		}
		*/

		{
			const char* strOuyaControllerClass = "tv/ouya/console/api/OuyaController";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strOuyaControllerClass);
			_jcOuyaController = env->FindClass(strOuyaControllerClass);
			if (_jcOuyaController)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strOuyaControllerClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strOuyaControllerClass);
				return JNI_ERR;
			}
		}

		{
			const char* strAxisField = "AXIS_LS_X";
			_jfAxisLsX = env->GetStaticFieldID(_jcOuyaController, strAxisField, "I");
			if (_jfAxisLsX)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAxisField);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAxisField);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
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
}
