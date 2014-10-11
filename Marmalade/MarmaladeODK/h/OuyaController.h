#ifndef __TV_OUYA_CONSOLE_API_OUYACONTROLLER_OUYACONTROLLER_H__
#define __TV_OUYA_CONSOLE_API_OUYACONTROLLER_OUYACONTROLLER_H__

#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include <jni.h>

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace tv_ouya_console_api_OuyaController
{
	class OuyaController
	{
	public:
		static int InitJNI(JNIEnv* env);
		void Dispose();

		static const int AXIS_LS_X();
		static const int AXIS_LS_Y();
		static const int AXIS_RS_X();
		static const int AXIS_RS_Y();
		static const int AXIS_L2();
		static const int AXIS_R2();
		static const int BUTTON_O();
		static const int BUTTON_U();
		static const int BUTTON_Y();
		static const int BUTTON_A();
		static const int BUTTON_L1();
		static const int BUTTON_L3();
		static const int BUTTON_R1();
		static const int BUTTON_R3();
		static const int BUTTON_MENU();
		static const int BUTTON_DPAD_UP();
		static const int BUTTON_DPAD_RIGHT();
		static const int BUTTON_DPAD_DOWN();
		static const int BUTTON_DPAD_LEFT();

		static OuyaController* getControllerByPlayer(int playerNum);
		static void showCursor(bool visible);

		const std::string& getDeviceName();

	private:
		static JNIEnv* _env;
		static jclass _jcOuyaController;
		static jmethodID _jmGetButtonData;
		static jmethodID _jmGetControllerByPlayer;
		static jmethodID _jmGetDeviceName;
		static jmethodID _jmShowCursor;
		static jfieldID _jfAxisLsX;
		static jfieldID _jfAxisLsY;
		static jfieldID _jfAxisRsX;
		static jfieldID _jfAxisRsY;
		static jfieldID _jfAxisL2;
		static jfieldID _jfAxisR2;
		static jfieldID _jfButtonO;
		static jfieldID _jfButtonU;
		static jfieldID _jfButtonY;
		static jfieldID _jfButtonA;
		static jfieldID _jfButtonL1;
		static jfieldID _jfButtonL3;
		static jfieldID _jfButtonR1;
		static jfieldID _jfButtonR3;
		static jfieldID _jfButtonMenu;
		static jfieldID _jfButtonDpadUp;
		static jfieldID _jfButtonDpadRight;
		static jfieldID _jfButtonDpadDown;
		static jfieldID _jfButtonDpadLeft;
		jobject _instance;
		std::string _deviceName;

		static void FindJNI();

		void SetInstance(jobject instance);
	};
}

#endif