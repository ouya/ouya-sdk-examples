#ifndef __ANDROID_MEDIA_AUDIOMANAGER_AUDIOMANAGER_H__
#define __ANDROID_MEDIA_AUDIOMANAGER_AUDIOMANAGER_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_media_AudioManager
{
	class AudioManager
	{
	public:
		static int InitJNI(JNIEnv* env);
		static int STREAM_MUSIC();

	private:
		static JNIEnv* _env;
		static jclass _jcAudioManager;
		static jfieldID _fStreamMusic;
	};
}

#endif