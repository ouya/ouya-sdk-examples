#ifndef __ANDROID_CONTENT_CONTEXT_CONTEXT_H__
#define __ANDROID_CONTENT_CONTEXT_CONTEXT_H__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_pm_ApplicationInfo
{
	class ApplicationInfo;
}

namespace android_content_res_AssetManager
{
	class AssetManager;
}

namespace android_content_Context
{
	class Context
	{
	public:
		Context(jobject context);
		static int InitJNI(JNIEnv* env);
		Context getApplicationContext();
		android_content_pm_ApplicationInfo::ApplicationInfo getApplicationInfo();
		android_content_res_AssetManager::AssetManager getAssets();
	private:
		static JNIEnv* _env;
		static jclass _jcContext;
		static jmethodID _mGetApplicationContext;
		static jmethodID _mGetApplicationInfo;
		static jmethodID _mGetAssets;
	protected:
		jobject _instance;
	};
}

#endif