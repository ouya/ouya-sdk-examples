#ifndef __ANDROID_MEDIA_AUDIOFORMAT_AUDIOFORMAT_H__
#define __ANDROID_MEDIA_AUDIOFORMAT_AUDIOFORMAT_H__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace audio_media_AudioFormat
{
	class AudioFormat
	{
	public:
		static int CHANNEL_OUT_MONO();
		static int CHANNEL_OUT_STEREO();
		static int ENCODING_PCM_16BIT();
		static int ENCODING_PCM_8BIT();
	};
}

#endif