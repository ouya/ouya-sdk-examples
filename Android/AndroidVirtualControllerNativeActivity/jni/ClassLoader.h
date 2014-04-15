#ifndef __JAVA_LANG_CLASSLOADER_CLASSLOADER_H__
#define __JAVA_LANG_CLASSLOADER_CLASSLOADER_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace java_lang_ClassLoader
{
	class ClassLoader
	{
	public:
		ClassLoader(jobject classLoader);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		static ClassLoader getSystemClassLoader();
		std::string loadClass(const std::string& className);
	private:
		static JNIEnv* _env;
		static jclass _jcClassLoader;
		static jmethodID _mGetSystemClassLoader;
		static jmethodID _mLoadClass;
		jobject _instance;
	};
}

#endif