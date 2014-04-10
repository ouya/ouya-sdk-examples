#ifndef __android_os_Build_Build__
#define __android_os_Build_Build__

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