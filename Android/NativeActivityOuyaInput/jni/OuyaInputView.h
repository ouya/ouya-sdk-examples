#ifndef __TV_OUYA_SDK_OUYA_INPUT_VIEW_H__
#define __TV_OUYA_SDK_OUYA_INPUT_VIEW_H__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

struct _JavaVM;
typedef _JavaVM JavaVM;

class _jclass;
typedef _jclass* jclass;

class _jfieldID;
typedef _jfieldID* jfieldID;

class _jmethodID;
typedef _jmethodID* jmethodID;

class _jobject;
typedef _jobject* jobject;

namespace tv_ouya_sdk_OuyaInputView
{
	class OuyaInputView
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static void JNIFind();

		static OuyaInputView* getInstance();

		bool javaDispatchKeyEvent(long long downTime, long long eventTime, int action, int code,
			int repeat, int metaState, int deviceId, int scancode, int flags, int source);

		jobject GetInstance();

	private:
		static JavaVM* _jvm;
		static jclass _jcOuyaInputView;
		static jmethodID _jmGetInstance;
		static jmethodID _jmJavaDispatchKeyEvent;
		jobject _instance;
	};
}

#endif
