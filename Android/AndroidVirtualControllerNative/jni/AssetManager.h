#ifndef __ASSET_MANAGER_H__
#define __ASSET_MANAGER_H__

#include <string>
#include <vector>

#include "InputStream.h"
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
		static int InitJNI(JNIEnv* env);
		jobject GetInstance();
		std::vector<std::string> list(const java_lang_String::String& path);
		java_io_InputStream::InputStream open(const java_lang_String::String& path);
	private:
		static JNIEnv* _env;
		static jclass _jcAssetManager;
		static jmethodID _mList;
		static jmethodID _mOpen;
		jobject _instance;
	};
}

#endif