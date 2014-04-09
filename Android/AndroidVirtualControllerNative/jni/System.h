#ifndef __java_lang_System_System__
#define __java_lang_System_System__

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