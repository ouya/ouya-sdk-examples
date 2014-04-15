#include <android/log.h>
#include <jni.h>

#include "DexFile.h"

#define LOG_TAG "dalvik_system_DexFile"

#define ENABLE_VERBOSE_LOGGING true

namespace dalvik_system_DexFile
{
	JNIEnv* DexFile::_env = 0;
	jclass DexFile::_jcDexFile = 0;
	jmethodID DexFile::_mConstructor = 0;
	jmethodID DexFile::_mEntries = 0;

	int DexFile::InitJNI(JNIEnv* env)
	{
		{
			const char* strClass = "dalvik/system/DexFile";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClass);
			_jcDexFile = env->FindClass(strClass);
			if (_jcDexFile)
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
			_mConstructor = env->GetMethodID(_jcDexFile, strConstructor, "(Ljava/lang/String;)V");
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
			const char* strEntries = "entries";
			_mEntries = env->GetMethodID(_jcDexFile, strEntries, "()Ljava/util/Enumeration;");
			if (_mEntries)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strEntries);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strEntries);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	DexFile::DexFile(const std::string& fileName)
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return;
		}

		_instance = _env->AllocObject(_jcDexFile);
		if (_instance)
		{
#if ENABLE_VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success allocate DexFile");
#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to allocate DexFile");
			return;
		}

		jstring arg1 = _env->NewStringUTF(fileName.c_str());
		_env->CallVoidMethod(_instance, _mConstructor, arg1);

		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to construct DexFile");
			return;
		}

#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success constructed DexFile");
#endif
		_env->DeleteLocalRef(arg1);
	}

	DexFile::DexFile(jobject dexFile)
	{
		_instance = dexFile;
	}

	jobject DexFile::GetInstance() const
	{
		return _instance;
	}

	std::vector<std::string> DexFile::entries()
	{
		std::vector<std::string> retVal;

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jobject result = _env->CallObjectMethod(_instance, _mEntries);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on entries");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed on entries");
			return retVal;
		}

		jsize const length = _env->GetArrayLength((jarray)result);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Entries returned %d items", length);

		for (jsize index(0); index < length; ++index)
		{
			jstring element = (jstring)_env->GetObjectArrayElement((jobjectArray)result, index);
			const char* nativeString = _env->GetStringUTFChars(element, 0);
			//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, nativeString);
			std::string entry = nativeString;
			retVal.push_back(entry);
			_env->ReleaseStringUTFChars(element, nativeString);
		}

		return retVal;
	}
}