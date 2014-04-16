#ifndef __ANDROID_CONTENT_RES_ASSETFILEDESCRIPTOR_ASSETFILEDESCRIPTOR_H__
#define __ANDROID_CONTENT_RES_ASSETFILEDESCRIPTOR_ASSETFILEDESCRIPTOR_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_res_AssetFileDescriptor
{
	class AssetFileDescriptor
	{
	public:
		AssetFileDescriptor(jobject assetFileDescriptor);
		AssetFileDescriptor(const AssetFileDescriptor& rhs);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		long getLength();
	private:
		static JNIEnv* _env;
		static jclass _jcAssetFileDescriptor;
		static jmethodID _mGetLength;
		jobject _instance;
	};
}

#endif