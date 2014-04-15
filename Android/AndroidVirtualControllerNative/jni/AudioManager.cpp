#include <android/log.h>
#include <jni.h>

#include "AudioManager.h"

#define LOG_TAG "android_media_AudioManager"

namespace android_media_AudioManager
{
	JNIEnv* AudioManager::_env = 0;
	jclass AudioManager::_jcAudioManager = 0;
	jfieldID AudioManager::_fStreamMusic = 0;

	int AudioManager::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/media/AudioManager";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcAudioManager = env->FindClass(strClass);
			if (_jcAudioManager)
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
			const char* strStreamMusic = "STREAM_MUSIC";
			_fStreamMusic = env->GetStaticFieldID(_jcAudioManager, strStreamMusic, "I");
			if (_fStreamMusic)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strStreamMusic);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strStreamMusic);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	int AudioManager::STREAM_MUSIC()
	{
		jint result = _env->GetStaticIntField(_jcAudioManager, _fStreamMusic);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get static field");
			return 0;
		}
		else
		{
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success on get static field");
			return result;
		}
	}
}