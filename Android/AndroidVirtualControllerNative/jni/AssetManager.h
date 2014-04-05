#ifndef __ASSET_MANAGER_H__
#define __ASSET_MANAGER_H__

#include "String.h"

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_res_AssetManager
{
	class AssetManager
	{
	public:
		AssetManager(jobject assetManager);
		AssetManager(const AssetManager& assetManager);
		~AssetManager();
		static int InitJNI(JNIEnv* env);
		java_lang_String::String* list(const java_lang_String::String& path) const;
	private:
		static JNIEnv* _env;
		static jclass _jcAssetManager;
		static jmethodID _mList;
		jobject _instance;
	};
}

#endif