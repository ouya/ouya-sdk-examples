#include <android/log.h>
#include <jni.h>

#include "AudioTrack.h"

#define LOG_TAG "android_media_AudioTrack"

namespace android_media_AudioTrack
{
	JNIEnv* AudioTrack::_env = 0;
	jclass AudioTrack::_jcAudioTrack = 0;
	jmethodID AudioTrack::_mConstructor = 0;
	jmethodID AudioTrack::_mWrite = 0;
	jmethodID AudioTrack::_mPlay = 0;

	int AudioTrack::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/media/AudioTrack";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcAudioTrack = env->FindClass(strClass);
			if (_jcAudioTrack)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClass);
				return JNI_ERR;
			}
		}

		{
			const char* strConstructor = "<init>";
			_mConstructor = env->GetMethodID(_jcAudioTrack, strConstructor, "(IIIIII)V");
			if (_mConstructor)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strConstructor);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strConstructor);
				return JNI_ERR;
			}
		}

		{
			const char* strWrite = "write";
			_mWrite = env->GetMethodID(_jcAudioTrack, strWrite, "([BII)I");
			if (_mWrite)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strWrite);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strWrite);
				return JNI_ERR;
			}
		}

		{
			const char* strPlay = "play";
			_mPlay = env->GetMethodID(_jcAudioTrack, strPlay, "()V");
			if (_mPlay)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strPlay);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strPlay);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	AudioTrack::AudioTrack(jobject audioTrack)
	{
		_instance = audioTrack;
	}	

	jobject AudioTrack::GetInstance() const
	{
		return _instance;
	}

	AudioTrack::AudioTrack(int streamType, int sampleRateInHz, int channelConfig, int audioFormat, int bufferSizeInBytes, int mode)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		jint arg1 = streamType;
		jint arg2 = sampleRateInHz;
		jint arg3 = channelConfig;
		jint arg4 = audioFormat;
		jint arg5 = bufferSizeInBytes;
		jint arg6 = mode;
		_instance = _env->AllocObject(_jcAudioTrack);
		if (_instance)
		{
			_env->CallVoidMethod(_instance, _mConstructor, arg1, arg2, arg3, arg4, arg5, arg6);
			if (_env->ExceptionCheck())
			{
				_env->ExceptionDescribe();
				_env->ExceptionClear();
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct");
			}
			else
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Constructed AudioTrack");
			}
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct AudioTrack");
		}
	}

	int AudioTrack::write(jbyte* audioData, int offsetInBytes, int sizeInBytes)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		jbyteArray arg1 = _env->NewByteArray(sizeInBytes);
		jint arg2 = offsetInBytes;
		jint arg3 = sizeInBytes;
		_env->SetByteArrayRegion(arg1, offsetInBytes, sizeInBytes, audioData);
		jint result = _env->CallIntMethod(_instance, _mWrite, arg1, arg2, arg3);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to write");
			return 0;
		}
		_env->DeleteLocalRef(arg1);

		return result;
	}

	void AudioTrack::play()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_env->CallVoidMethod(_instance, _mPlay);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to play");
		}
	}

	int AudioTrack::MODE_STREAM()
	{
		return 1;
	}
}