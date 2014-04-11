#ifndef __ANDROID_VIEW_INPUTDEVICE_INPUTDEVICE_H__
#define __ANDROID_VIEW_INPUTDEVICE_INPUTDEVICE_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_view_InputDevice
{
	class InputDevice
	{
	public:
		InputDevice(jobject inputDevice);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance();
		static InputDevice getDevice(int id);
		std::string getName();
	private:
		static JNIEnv* _env;
		static jclass _jcInputDevice;
		static jmethodID _mGetDevice;
		static jmethodID _mGetName;
		jobject _instance;
	};
}

#endif