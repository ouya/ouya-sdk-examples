#ifndef android_content_res_AssetManager_AssetManager__
#define android_content_res_AssetManager_AssetManager__

#include <string>
#include <vector>

#include "InputStream.h"

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
		static int ACCESS_BUFFER();
		std::vector<std::string> list(const std::string& path);
		java_io_InputStream::InputStream open(const std::string& fileName);
		java_io_InputStream::InputStream open(const std::string& fileName, int accessMode);
	private:
		static JNIEnv* _env;
		static jclass _jcAssetManager;
		static jfieldID _jfAccessBuffer;
		static jmethodID _mList;
		static jmethodID _mOpen;
		static jmethodID _mOpen2;
		jobject _instance;
	};
}

#endif