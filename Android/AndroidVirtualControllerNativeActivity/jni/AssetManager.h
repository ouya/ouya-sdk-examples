#ifndef ANDROID_CONTENT_RES_ASSETMANAGER_ASSETMANAGER_H__
#define ANDROID_CONTENT_RES_ASSETMANAGER_ASSETMANAGER_H__

#include <string>
#include <vector>

#include "InputStream.h"

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_res_AssetFileDescriptor
{
	class AssetFileDescriptor;
}

namespace android_content_res_AssetManager
{
	class AssetManager
	{
	public:
		AssetManager(jobject assetManager);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance();
		static int ACCESS_BUFFER();
		std::vector<std::string> list(const std::string& path);
		java_io_InputStream::InputStream open(const std::string& fileName);
		java_io_InputStream::InputStream open(const std::string& fileName, int accessMode);
		android_content_res_AssetFileDescriptor::AssetFileDescriptor openFd(const std::string& fileName);
	private:
		static JNIEnv* _env;
		static jclass _jcAssetManager;
		static jfieldID _jfAccessBuffer;
		static jmethodID _mList;
		static jmethodID _mOpen;
		static jmethodID _mOpen2;
		static jmethodID _mOpenFd;
		jobject _instance;
	};
}

#endif