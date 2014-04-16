#include <android/log.h>
#include <jni.h>

#include "AssetFileDescriptor.h"
#include "SoundPool.h"

#define LOG_TAG "android_media_SoundPool"
#define ENABLE_VERBOSE_LOGGING true

using namespace android_content_res_AssetFileDescriptor;

namespace android_media_SoundPool
{
	JNIEnv* SoundPool::_env = 0;
	jclass SoundPool::_jcSoundPool = 0;
	jmethodID SoundPool::_mConstructor = 0;
	jmethodID SoundPool::_mLoad = 0;
	jmethodID SoundPool::_mLoad2 = 0;
	jmethodID SoundPool::_mPlay = 0;

	int SoundPool::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "android/media/SoundPool";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcSoundPool = env->FindClass(strClass);
			if (_jcSoundPool)
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
			_mConstructor = env->GetMethodID(_jcSoundPool, strConstructor, "(III)V");
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
			const char* strLoad = "load";
			_mLoad = env->GetMethodID(_jcSoundPool, strLoad, "(Landroid/content/Context;II)I");
			if (_mLoad)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strLoad);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strLoad);
				return JNI_ERR;
			}
		}

		{
			const char* strLoad = "load";
			_mLoad2 = env->GetMethodID(_jcSoundPool, strLoad, "(Landroid/content/res/AssetFileDescriptor;I)I");
			if (_mLoad2)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strLoad);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strLoad);
				return JNI_ERR;
			}
		}

		{
			const char* strPlay = "play";
			_mPlay = env->GetMethodID(_jcSoundPool, strPlay, "(IFFIIF)I");
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

	SoundPool::SoundPool(jobject soundPool)
	{
		_instance = soundPool;
	}

	SoundPool::SoundPool(const SoundPool& rhs)
	{
		_instance = rhs._instance;
	}

	SoundPool::SoundPool(int maxStreams, int streamType, int srcQuality)
	{
		_instance = _env->AllocObject(_jcSoundPool);

		_env->CallVoidMethod(_instance, _mConstructor, maxStreams, streamType, srcQuality);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct the SoundPool");
		}
	}

	jobject SoundPool::GetInstance() const
	{
		return _instance;
	}

	int SoundPool::load(const AssetFileDescriptor& afd, int priority)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return 0;
		}

		jobject arg1 = afd.GetInstance();
		jint arg2 = priority;
		jint result = _env->CallIntMethod(_instance, _mLoad2, arg1, arg2);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to load sound");
			return 0;
		}
		else
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success load sound=%d", result);
#endif
			return (int)result;
		}
	}

	int SoundPool::play(int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		if (!_instance)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Instance is not valid!");
			return 0;
		}

		jint result = _env->CallIntMethod(_instance, _mPlay, soundId, leftVolume, rightVolume, priority, loop, rate);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed on play sound soundId=%d leftVolume=%f rightVolume=%f priority=%d loop=%d rate=%f",
				soundId, leftVolume, rightVolume, priority, loop, rate);
			return 0;
		}
		else
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on play sound soundId=%d leftVolume=%f rightVolume=%f priority=%d loop=%d rate=%f",
				soundId, leftVolume, rightVolume, priority, loop, rate);
#endif
			return result;
		}
	}
}