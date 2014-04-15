#ifndef __DALVIK_SYSTEM_DEXFILE_DEXFILE_H__
#define __DALVIK_SYSTEM_DEXFILE_DEXFILE_H__

#include <string>
#include <vector>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace dalvik_system_DexFile
{
	class DexFile
	{
	public:
		DexFile(jobject dexFile);
		DexFile(const std::string& val);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		std::vector<std::string> entries();
	private:
		static JNIEnv* _env;
		static jclass _jcDexFile;
		static jmethodID _mConstructor;
		static jmethodID _mEntries;
		jobject _instance;
	};
}

#endif