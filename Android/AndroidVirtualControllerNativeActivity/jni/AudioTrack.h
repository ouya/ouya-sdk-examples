#ifndef __ANDROID_MEDIA_AUDIOTRACK_AUDIOTRACK_H__
#define __ANDROID_MEDIA_AUDIOTRACK_AUDIOTRACK_H__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_media_AudioTrack
{
	class AudioTrack
	{
	public:
		AudioTrack(jobject audioTrack);
		AudioTrack(int streamType, int sampleRateInHz, int channelConfig, int audioFormat, int bufferSizeInBytes, int mode);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		int write(jbyte* audioData, int offsetInBytes, int sizeInBytes);
		void play();
		static int MODE_STREAM();

	private:
		static JNIEnv* _env;
		static jclass _jcAudioTrack;
		static jmethodID _mConstructor;
		static jmethodID _mWrite;
		static jmethodID _mPlay;
		jobject _instance;
	};
}

#endif