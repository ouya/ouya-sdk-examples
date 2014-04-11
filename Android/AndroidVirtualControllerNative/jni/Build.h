#ifndef __ANDROID_OS_BUILD_BUILD_H__
#define __ANDROID_OS_BUILD_BUILD_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_os_Build
{
	class Build
	{
	public:
		static int InitJNI(JNIEnv* env);
		static std::string MODEL();
	private:
		static JNIEnv* _env;
		static jclass _jcBuild;
		static jfieldID _fModel;
	};
}

#endif