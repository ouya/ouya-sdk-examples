#ifndef __TV_OUYA_SDK_OUYA_INPUT_VIEW_H__
#define __TV_OUYA_SDK_OUYA_INPUT_VIEW_H__

#include <jni.h>
#include <map>
#include <vector>

class AInputEvent;

namespace tv_ouya_sdk_OuyaInputView
{
	class OuyaInputView
	{
	public:
		static int InitJNI(JNIEnv* env);

		// extract the original key event in native code
		static bool dispatchKeyEvent(JNIEnv* env, AInputEvent* keyEvent);

		// extract the original motion event in native code
		static bool dispatchGenericMotionEvent(JNIEnv* env, AInputEvent* motionEvent);

		// send key event to Java
		static bool javaDispatchKeyEvent(JNIEnv* env, long long downTime, long long eventTime, int action, int code,
			int repeat, int metaState, int deviceId, int scancode, int flags, int source);

		// send motion event to Java
		static bool javaDispatchGenericMotionEvent(JNIEnv* env,
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
				int* pointerPropertiesId,
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
				float* axisValues);

		// receive motion event from Java
		static void dispatchGenericMotionEventNative(JNIEnv* env, jobject thiz,
			jint playerNum,
			jint axis,
			jfloat val);

		// receive key event from Java
		static void dispatchKeyEventNative(JNIEnv* env, jobject thiz,
			jint playerNum,
			jint keyCode,
			jint action);

		// get axis value
		static float getAxis(int playerNum, int axis);

		// check if a button is pressed
		static bool isPressed(int playerNum, int keyCode);

		// check if a button was down
		static bool isPressedDown(int playerNum, int keyCode);

		// check if a button was up
		static bool isPressedUp(int playerNum, int keyCode);

		// clear the button state for detecting up and down
		static void clearButtonStates();

		// clear the axis values
		static void clearAxes();

		// clear the button values
		static void clearButtons();

	private:
		static jclass _jcOuyaInputView;

		static JNINativeMethod _nativeMethodTable[];
		static int _nativeMethodTableSize;

		static JNINativeMethod _nativeMethodTable2[];
		static int _nativeMethodTableSize2;

		//axis states
		static std::vector< std::map<int, float> > _axis;

		//button states
		static std::vector< std::map<int, bool> > _button;
		static std::vector< std::map<int, bool> > _buttonDown;
		static std::vector< std::map<int, bool> > _buttonUp;
		static std::vector< std::map<int, bool> > _lastButtonDown;
		static std::vector< std::map<int, bool> > _lastButtonUp;
	};
}

#endif
