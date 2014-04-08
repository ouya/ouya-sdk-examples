#ifndef __android_app_Activity_Activity__
#define __android_app_Activity_Activity__

#include "Context.h"

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_app_Activity
{
	class Activity : public android_content_Context::Context
	{
	public:
		Activity(jobject activity);
		static int InitJNI(JNIEnv* env);
		android_content_Context::Context getApplicationContext();
	private:
		static JNIEnv* _env;
		static jclass _jcActivity;
		static jmethodID _mGetApplicationContext;
	};
}

#endif