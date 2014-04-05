#ifndef __ASSET_MANAGER_H__
#define __ASSET_MANAGER_H__

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
	private:
		static JNIEnv* _env;
		static jclass _jcAssetManager;
		jobject _instance;
	};
}

#endif