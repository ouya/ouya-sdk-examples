#ifndef __ACTIVITY_H__
#define __ACTIVITY_H__

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
		Activity(const Activity& activity);
		~Activity();
		static int InitJNI(JNIEnv* env);
	private:
		static JNIEnv* _env;
		static jclass _jcActivity;
	};
}

#endif