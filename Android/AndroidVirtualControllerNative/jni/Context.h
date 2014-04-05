#ifndef __CONTEXT_H__
#define __CONTEXT_H__

#include "AssetManager.h"

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_Context
{
	class Context
	{
	public:
		Context(jobject context);
		Context(const Context& context);
		~Context();
		static int InitJNI(JNIEnv* env);
		Context getApplicationContext();
		android_content_res_AssetManager::AssetManager getAssets();
	private:
		static JNIEnv* _env;
		static jclass _jcContext;
		static jmethodID _mGetApplicationContext;
		static jmethodID _mGetAssets;
	protected:
		jobject _instance;
	};
}

#endif