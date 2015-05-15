#include "OuyaInputView.h"

#include <android/log.h>
#include <android_native_app_glue.h>
#include <jni.h>

#define LOG_TAG "tv_ouya_sdk_OuyaInputView"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING false

#define MAX_CONTROLLERS 4

namespace tv_ouya_sdk_OuyaInputView
{
	jclass OuyaInputView::_jcOuyaInputView = 0;

	JNINativeMethod OuyaInputView::_nativeMethodTable[] = {
		{ "dispatchGenericMotionEventNative", "(IIF)V", (void *)OuyaInputView::dispatchGenericMotionEventNative }
	};

	int OuyaInputView::_nativeMethodTableSize = sizeof(_nativeMethodTable) / sizeof(_nativeMethodTable[0]);

	JNINativeMethod OuyaInputView::_nativeMethodTable2[] = {
		{ "dispatchKeyEventNative", "(III)V", (void *)OuyaInputView::dispatchKeyEventNative }
	};

	int OuyaInputView::_nativeMethodTableSize2 = sizeof(_nativeMethodTable2) / sizeof(_nativeMethodTable2[0]);

	std::vector< std::map<int, float> > OuyaInputView::_axis;
	std::vector< std::map<int, bool> > OuyaInputView::_button;
	std::vector< std::map<int, bool> > OuyaInputView::_buttonDown;
	std::vector< std::map<int, bool> > OuyaInputView::_buttonUp;
	std::vector< std::map<int, bool> > OuyaInputView::_lastButtonDown;
	std::vector< std::map<int, bool> > OuyaInputView::_lastButtonUp;

	int OuyaInputView::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "tv/ouya/sdk/OuyaInputView";
			#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			#endif
			jclass localRef = env->FindClass(strClass);
			if (localRef)
			{
#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
#endif
				_jcOuyaInputView = (jclass)env->NewGlobalRef((jobject)localRef);
				env->DeleteLocalRef((jobject)localRef);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		// setup native receiving remapped input

		{
			jint ret = env->RegisterNatives(_jcOuyaInputView, _nativeMethodTable, _nativeMethodTableSize);
			if (ret < 0)
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to register native methods");
				return JNI_ERR;
			}
		}

		{
			jint ret = env->RegisterNatives(_jcOuyaInputView, _nativeMethodTable2, _nativeMethodTableSize2);
			if (ret < 0)
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to register native methods 2");
				return JNI_ERR;
			}
		}

		for (int index = 0; index < MAX_CONTROLLERS; ++index)
		{
			_axis.push_back(std::map<int, float>());
			_button.push_back(std::map<int, bool>());
			_buttonDown.push_back(std::map<int, bool>());
			_buttonUp.push_back(std::map<int, bool>());
			_lastButtonDown.push_back(std::map<int, bool>());
			_lastButtonUp.push_back(std::map<int, bool>());
		}

		jfieldID fieldNativeInitialized = env->GetStaticFieldID(_jcOuyaInputView, "sNativeInitialized", "Z");
		if (fieldNativeInitialized)
		{
			env->SetStaticBooleanField(_jcOuyaInputView, fieldNativeInitialized, true);
#if VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Native plugin has loaded.");
#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find sNativeInitialized field");
			return JNI_ERR;
		}

		return JNI_OK;
	}

	bool OuyaInputView::dispatchKeyEvent(JNIEnv* env, AInputEvent* keyEvent)
	{
		int64_t downTime = AKeyEvent_getDownTime(keyEvent);
		int64_t eventTime = AKeyEvent_getEventTime(keyEvent);
		int32_t action = AKeyEvent_getAction(keyEvent);
		int32_t code = AKeyEvent_getKeyCode(keyEvent);
		int32_t repeat = AKeyEvent_getRepeatCount(keyEvent);
		int32_t metaState = AKeyEvent_getMetaState(keyEvent);
		int32_t deviceId = AInputEvent_getDeviceId(keyEvent);
		int32_t scancode = AKeyEvent_getScanCode(keyEvent);
		int32_t flags = AKeyEvent_getFlags(keyEvent);
		int32_t source = AInputEvent_getSource(keyEvent);

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchKeyEvent: downTime=%lld eventTime=%lld action=%d code=%d repeat=%d metaState=%d deviceId=%d scancode=%d flags=%d source=%d",
			downTime, eventTime, action, code,
			repeat, metaState, deviceId, scancode, flags, source);
#endif

		return javaDispatchKeyEvent(env, downTime, eventTime, action, code,
			repeat, metaState, deviceId, scancode, flags, source);
	}

	bool OuyaInputView::dispatchGenericMotionEvent(JNIEnv* env, AInputEvent* motionEvent)
	{
		int64_t downTime = AMotionEvent_getDownTime(motionEvent);
		int64_t eventTime = AMotionEvent_getEventTime(motionEvent);
		int32_t action = AMotionEvent_getAction(motionEvent);
		int32_t metaState = AMotionEvent_getMetaState(motionEvent);
		int32_t pointerCount = AMotionEvent_getPointerCount(motionEvent);
		int32_t buttonState = AMotionEvent_getButtonState(motionEvent);
		float xPrecision = AMotionEvent_getXPrecision(motionEvent);
		float yPrecision = AMotionEvent_getYPrecision(motionEvent);
		int32_t deviceId = AInputEvent_getDeviceId(motionEvent);
		int32_t edgeFlags = AMotionEvent_getEdgeFlags(motionEvent);
		int32_t flags = AMotionEvent_getFlags(motionEvent);
		int32_t source = AInputEvent_getSource(motionEvent);

		int* pointerPropertiesId = new int[pointerCount];
		int* pointerPropertiesToolType = new int[pointerCount];
		float* pointerCoordsOrientation = new float[pointerCount];
		float* pointerCoordsPressure = new float[pointerCount];
		float* pointerCoordsSize = new float[pointerCount];
		float* pointerCoordsToolMajor = new float[pointerCount];
		float* pointerCoordsToolMinor = new float[pointerCount];
		float* pointerCoordsTouchMajor = new float[pointerCount];
		float* pointerCoordsTouchMinor = new float[pointerCount];
		float* pointerCoordsX = new float[pointerCount];
		float* pointerCoordsY = new float[pointerCount];

		std::vector<int> listAxisIndices;
		std::vector<float> listAxisValues;

		if (pointerCount > 0)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchGenericMotionEvent: pointerCount=%d deviceId=%d source=%d",
				pointerCount, deviceId, source);
#endif

			// MotionEvent.PointerProperties
			long long pointerId = AMotionEvent_getPointerId(motionEvent, 0);
			int32_t toolType = AMotionEvent_getToolType(motionEvent, 0);

#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchGenericMotionEvent: PointerProperties pointerId=%lld toolType-%d",
				pointerId, toolType);
#endif

			pointerPropertiesId[0] = pointerId;
			pointerPropertiesToolType[0] = toolType;

			// MotionEvent.PointerCoords
			float orientation = AMotionEvent_getOrientation(motionEvent, pointerId);
			float pressure = AMotionEvent_getPressure(motionEvent, pointerId);
			float size = AMotionEvent_getSize(motionEvent, pointerId);
			float toolMajor = AMotionEvent_getTouchMajor(motionEvent, pointerId);
			float toolMinor = AMotionEvent_getToolMinor(motionEvent, pointerId);
			float touchMajor = AMotionEvent_getTouchMajor(motionEvent, pointerId);
			float touchMinor = AMotionEvent_getTouchMinor(motionEvent, pointerId);
			float x = AMotionEvent_getX(motionEvent, pointerId);
			float y = AMotionEvent_getY(motionEvent, pointerId);

#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchGenericMotionEvent: PointerCoords orientation=%f pressure=%f size=%f toolMajor=%f toolMinor=%f touchMajor=%f touchMinor=%f x=%f y=%f",
				orientation, pressure, size,
				toolMajor, toolMinor, touchMajor, touchMinor,
				x, y);
#endif

			pointerCoordsOrientation[0] = orientation;
			pointerCoordsPressure[0] = pressure;
			pointerCoordsSize[0] = size;
			pointerCoordsToolMajor[0] = toolMajor;
			pointerCoordsToolMinor[0] = toolMinor;
			pointerCoordsTouchMajor[0] = touchMajor;
			pointerCoordsTouchMinor[0] = touchMinor;
			pointerCoordsX[0] = x;
			pointerCoordsY[0] = y;

			for (int32_t axis = 0; axis < 50; ++axis) // 50 is based on the AXIS_value range I saw in the documentation
			{
				float val = AMotionEvent_getAxisValue(motionEvent, axis, pointerId);
				if (val != 0.0f)
				{
#if ENABLE_VERBOSE_LOGGING
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchGenericMotionEvent: axis=%d val=%f", axis, val);
#endif
					listAxisIndices.push_back(axis);
					listAxisValues.push_back(val);
				}
			}
		}

		int axisCount = listAxisIndices.size();
		int* axisIndexes = new int[axisCount];
		float* axisValues = new float[axisCount];
		for (int index = 0; index < axisCount; ++index)
		{
			axisIndexes[index] = listAxisIndices[index];
			axisValues[index] = listAxisValues[index];
		}
		listAxisIndices.clear();
		listAxisValues.clear();

		bool handled = javaDispatchGenericMotionEvent(env,
			downTime,
			eventTime,
			action,
			pointerCount,
			metaState,
			buttonState,
			xPrecision,
			yPrecision,
			deviceId,
			edgeFlags,
			source,
			flags,
			pointerPropertiesId,
			pointerPropertiesToolType,
			pointerCoordsOrientation,
			pointerCoordsPressure,
			pointerCoordsSize,
			pointerCoordsToolMajor,
			pointerCoordsToolMinor,
			pointerCoordsTouchMajor,
			pointerCoordsTouchMinor,
			pointerCoordsX,
			pointerCoordsY,
			axisCount,
			axisIndexes,
			axisValues);

		delete pointerPropertiesId;
		delete pointerPropertiesToolType;
		delete pointerCoordsOrientation;
		delete pointerCoordsPressure;
		delete pointerCoordsSize;
		delete pointerCoordsToolMajor;
		delete pointerCoordsToolMinor;
		delete pointerCoordsTouchMajor;
		delete pointerCoordsTouchMinor;
		delete pointerCoordsX;
		delete pointerCoordsY;
		delete axisIndexes;
		delete axisValues;
		return handled;
	}

	bool OuyaInputView::javaDispatchKeyEvent(JNIEnv* env, long long downTime, long long eventTime, int action, int code,
		int repeat, int metaState, int deviceId, int scancode, int flags, int source)
	{
		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return false;
		}
		else
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "javaDispatchKeyEvent: env is valid");
#endif
		}

		const char* strMethod = "javaDispatchKeyEvent";
		#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for method %s", strMethod);
		#endif
		jmethodID method = env->GetStaticMethodID(_jcOuyaInputView, strMethod, "(JJIIIIIIII)Z");
		if (method)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
			return false;
		}

		bool result = env->CallStaticBooleanMethod(_jcOuyaInputView, method,
			downTime, eventTime, action, code,
			repeat, metaState, deviceId, scancode, flags, source);

#if ENABLE_VERBOSE_LOGGING
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "invoked _jmJavaDispatchKeyEvent result=true");
		}
		else
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "invoked _jmJavaDispatchKeyEvent result=false");
		}
#endif

		return result;
	}

	bool OuyaInputView::javaDispatchGenericMotionEvent(JNIEnv* env,
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
		float* axisValues)
	{
		if (!env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return false;
		}

		if (!_jcOuyaInputView)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "_jcOuyaInputView is not valid!");
			return false;
		}

		const char* strMethod = "javaDispatchGenericMotionEvent";
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for method %s", strMethod);
#endif
		jmethodID method = env->GetStaticMethodID(_jcOuyaInputView, strMethod, "(JJIIIIFFIIII[I[I[F[F[F[F[F[F[F[F[FI[I[F)Z");
		if (method)
		{
			#if ENABLE_VERBOSE_LOGGING
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strMethod);
			#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strMethod);
		}

		jintArray argPointerPropertiesId = env->NewIntArray(pointerCount);
		jintArray argpointerPropertiesToolType = env->NewIntArray(pointerCount);
		jfloatArray argpointerCoordsOrientation = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsPressure = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsSize = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsToolMajor = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsToolMinor = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsTouchMajor = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsTouchMinor = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsX = env->NewFloatArray(pointerCount);
		jfloatArray argpointerCoordsY = env->NewFloatArray(pointerCount);
		jintArray argAxisIndexes = env->NewIntArray(axisCount);
		jfloatArray argAxisValues = env->NewFloatArray(axisCount);

		env->SetIntArrayRegion(argPointerPropertiesId, 0, pointerCount, pointerPropertiesId);
		env->SetIntArrayRegion(argpointerPropertiesToolType, 0, pointerCount, pointerPropertiesToolType);
		env->SetFloatArrayRegion(argpointerCoordsOrientation, 0, pointerCount, pointerCoordsOrientation);
		env->SetFloatArrayRegion(argpointerCoordsPressure, 0, pointerCount, pointerCoordsPressure);
		env->SetFloatArrayRegion(argpointerCoordsSize, 0, pointerCount, pointerCoordsSize);
		env->SetFloatArrayRegion(argpointerCoordsToolMajor, 0, pointerCount, pointerCoordsToolMajor);
		env->SetFloatArrayRegion(argpointerCoordsToolMinor, 0, pointerCount, pointerCoordsToolMinor);
		env->SetFloatArrayRegion(argpointerCoordsTouchMajor, 0, pointerCount, pointerCoordsTouchMajor);
		env->SetFloatArrayRegion(argpointerCoordsTouchMinor, 0, pointerCount, pointerCoordsTouchMinor);
		env->SetFloatArrayRegion(argpointerCoordsX, 0, pointerCount, pointerCoordsX);
		env->SetFloatArrayRegion(argpointerCoordsY, 0, pointerCount, pointerCoordsY);
		env->SetIntArrayRegion(argAxisIndexes, 0, axisCount, axisIndexes);
		env->SetFloatArrayRegion(argAxisValues, 0, axisCount, axisValues);

		bool handled = env->CallStaticBooleanMethod(_jcOuyaInputView, method,
				downTime,
				eventTime,
				action,
				pointerCount,
				metaState,
				buttonState,
				xPrecision,
				yPrecision,
				deviceId,
				edgeFlags,
				source,
				flags,
				argPointerPropertiesId,
				argpointerPropertiesToolType,
				argpointerCoordsOrientation,
				argpointerCoordsPressure,
				argpointerCoordsSize,
				argpointerCoordsToolMajor,
				argpointerCoordsToolMinor,
				argpointerCoordsTouchMajor,
				argpointerCoordsTouchMinor,
				argpointerCoordsX,
				argpointerCoordsY,
				axisCount,
				argAxisIndexes,
				argAxisValues);

		env->DeleteLocalRef(argPointerPropertiesId);
		env->DeleteLocalRef(argpointerPropertiesToolType);
		env->DeleteLocalRef(argpointerCoordsOrientation);
		env->DeleteLocalRef(argpointerCoordsPressure);
		env->DeleteLocalRef(argpointerCoordsSize);
		env->DeleteLocalRef(argpointerCoordsToolMajor);
		env->DeleteLocalRef(argpointerCoordsToolMinor);
		env->DeleteLocalRef(argpointerCoordsTouchMajor);
		env->DeleteLocalRef(argpointerCoordsTouchMinor);
		env->DeleteLocalRef(argpointerCoordsX);
		env->DeleteLocalRef(argpointerCoordsY);
		env->DeleteLocalRef(argAxisIndexes);
		env->DeleteLocalRef(argAxisValues);

		return handled;
	}

	void OuyaInputView::dispatchGenericMotionEventNative(JNIEnv* env, jobject thiz,
		jint playerNum,
		jint axis,
		jfloat val)
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchGenericMotionEventNative: Native remapped playerNum=%d axis=%d val=%f", playerNum, axis, val);
#endif
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}
		_axis[playerNum][axis] = val;
	}

	void OuyaInputView::dispatchKeyEventNative(JNIEnv* env, jobject thiz,
		jint playerNum,
		jint keyCode,
		jint action)
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "dispatchKeyEventNative: Native remapped playerNum=%d KeyCode=%d Action=%d", playerNum, keyCode, action);
#endif
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		bool buttonDown = action == 0;

		if (_button[playerNum][keyCode] != buttonDown)
		{
			_button[playerNum][keyCode] = buttonDown;
			if (buttonDown)
			{
				_buttonDown[playerNum][keyCode] = true;
			}
			else
			{
				_buttonUp[playerNum][keyCode] = true;
			}
		}
	}

	// get axis value
	float OuyaInputView::getAxis(int playerNum, int axis)
	{
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, float>::const_iterator search = _axis[playerNum].find(axis);
		if (search != _axis[playerNum].end())
		{
			return search->second;
		}
		return 0.0f;
	}

	// check if a button is pressed
	bool OuyaInputView::isPressed(int playerNum, int keyCode)
	{
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, bool>::const_iterator search = _button[playerNum].find(keyCode);
		if (search != _button[playerNum].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was down
	bool OuyaInputView::isPressedDown(int playerNum, int keyCode)
	{
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, bool>::const_iterator search = _lastButtonDown[playerNum].find(keyCode);
		if (search != _lastButtonDown[playerNum].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was up
	bool OuyaInputView::isPressedUp(int playerNum, int keyCode)
	{
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, bool>::const_iterator search = _lastButtonUp[playerNum].find(keyCode);
		if (search != _lastButtonUp[playerNum].end())
		{
			return search->second;
		}
		return false;
	}

	// clear the button state for detecting up and down
	void OuyaInputView::clearButtonStates()
	{
		for (int playerNum = 0; playerNum < MAX_CONTROLLERS; ++playerNum)
		{
			_lastButtonDown[playerNum].clear();
			_lastButtonUp[playerNum].clear();
			for (std::map<int, bool>::iterator it = _buttonDown[playerNum].begin(); it != _buttonDown[playerNum].end(); ++it)
			{
				int keyCode = it->first;
				_lastButtonDown[playerNum][keyCode] = _buttonDown[playerNum][keyCode];
			}
			for (std::map<int, bool>::iterator it = _buttonUp[playerNum].begin(); it != _buttonUp[playerNum].end(); ++it)
			{
				int keyCode = it->first;
				_lastButtonUp[playerNum][keyCode] = _buttonUp[playerNum][keyCode];
			}
			_buttonDown[playerNum].clear();
			_buttonUp[playerNum].clear();
		}
	}

	// clear the axis values
	void OuyaInputView::clearAxes()
	{
		for (int playerNum = 0; playerNum < MAX_CONTROLLERS; ++playerNum) {
			_axis[playerNum].clear();
		}
	}

	// clear the button values
	void OuyaInputView::clearButtons()
	{
		for (int playerNum = 0; playerNum < MAX_CONTROLLERS; ++playerNum) {
			_button[playerNum].clear();
			_buttonDown[playerNum].clear();
			_buttonUp[playerNum].clear();
			_lastButtonDown[playerNum].clear();
			_lastButtonUp[playerNum].clear();
		}
	}

}
