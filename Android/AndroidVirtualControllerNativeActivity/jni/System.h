#ifndef __JAVA_LANG_SYSTEM_SYSTEM_H__
#define __JAVA_LANG_SYSTEM_SYSTEM_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace java_lang_System
{
	class System
	{
	public:
		static int InitJNI(JNIEnv* env);
		static void loadLibrary(const std::string& libName);
	private:
		static JNIEnv* _env;
		static jclass _jcSystem;
		static jmethodID _mLoadLibrary;
	};
}

#endif