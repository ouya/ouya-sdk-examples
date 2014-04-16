#ifndef __ANDROID_MEDIA_SOUNDPOOL_SOUNDPOOL_H__
#define __ANDROID_MEDIA_SOUNDPOOL_SOUNDPOOL_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_content_Context
{
	class Context;
}

namespace android_content_res_AssetFileDescriptor
{
	class AssetFileDescriptor;
}

namespace android_media_SoundPool
{
	class SoundPool
	{
	public:
		SoundPool(const SoundPool& rhs);
		SoundPool(jobject soundPool);
		SoundPool(int maxStreams, int streamType, int srcQuality);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		int load(const std::string& path, int priority);
		int load(android_content_Context::Context context, int resId, int priority);
		int load(const android_content_res_AssetFileDescriptor::AssetFileDescriptor& afd, int priority);
		int play(int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate);
	private:
		static JNIEnv* _env;
		static jclass _jcSoundPool;
		static jmethodID _mConstructor;
		static jmethodID _mLoad;
		static jmethodID _mLoad2;
		static jmethodID _mPlay;
		jobject _instance;
	};
}

#endif