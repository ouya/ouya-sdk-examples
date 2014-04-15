#ifndef __ANDROID_CONTENT_PM_APPLICATIONINFO_APPLICATIONINFO_H__
#define __ANDROID_CONTENT_PM_APPLICATIONINFO_APPLICATIONINFO_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_pm_ApplicationInfo
{
	class ApplicationInfo
	{
	public:
		ApplicationInfo(jobject applicationInfo);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		std::string sourceDir();
	private:
		static JNIEnv* _env;
		static jclass _jcApplicationInfo;
		static jfieldID _fSourceDir;
		jobject _instance;
	};
}

#endif