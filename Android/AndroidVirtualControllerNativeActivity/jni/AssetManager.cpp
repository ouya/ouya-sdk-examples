#include <android/log.h>
#include <jni.h>

#include "AssetFileDescriptor.h";
#include "AssetManager.h"

#define LOG_TAG "android_content_res_AssetManager"

using namespace android_content_res_AssetFileDescriptor;

namespace android_content_res_AssetManager
{
	JNIEnv* AssetManager::_env = 0;
	jclass AssetManager::_jcAssetManager = 0;
	jfieldID AssetManager::_jfAccessBuffer = 0;
	jmethodID AssetManager::_mList = 0;
	jmethodID AssetManager::_mOpen = 0;
	jmethodID AssetManager::_mOpen2 = 0;
	jmethodID AssetManager::_mOpenFd = 0;

	int AssetManager::InitJNI(JNIEnv* env)
	{
		{
			const char* strAssetManagerClass = "android/content/res/AssetManager";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strAssetManagerClass);
			_jcAssetManager = env->FindClass(strAssetManagerClass);
			if (_jcAssetManager)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerClass);
				return JNI_ERR;
			}
		}

		{
			const char* strAssetManagerAccessBuffer = "ACCESS_BUFFER";
			_jfAccessBuffer = env->GetStaticFieldID(_jcAssetManager, strAssetManagerAccessBuffer, "I");
			if (_jfAccessBuffer)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerAccessBuffer);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerAccessBuffer);
				return JNI_ERR;
			}
		}

		{
			const char* strAssetManagerList = "list";
			_mList = env->GetMethodID(_jcAssetManager, strAssetManagerList, "(Ljava/lang/String;)[Ljava/lang/String;");
			if (_mList)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerList);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerList);
				return JNI_ERR;
			}
		}

		{
			const char* strAssetManagerOpen = "open";
			_mOpen = env->GetMethodID(_jcAssetManager, strAssetManagerOpen, "(Ljava/lang/String;)Ljava/io/InputStream;");
			if (_mOpen)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerOpen);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerOpen);
				return JNI_ERR;
			}
		}

		{
			const char* strAssetManagerOpen = "open";
			_mOpen2 = env->GetMethodID(_jcAssetManager, strAssetManagerOpen, "(Ljava/lang/String;I)Ljava/io/InputStream;");
			if (_mOpen2)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerOpen);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerOpen);
				return JNI_ERR;
			}
		}

		{
			const char* strAssetManagerOpen = "openFd";
			_mOpenFd = env->GetMethodID(_jcAssetManager, strAssetManagerOpen, "(Ljava/lang/String;)Landroid/content/res/AssetFileDescriptor;");
			if (_mOpenFd)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strAssetManagerOpen);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strAssetManagerOpen);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	AssetManager::AssetManager(jobject assetManager)
	{
		_instance = assetManager;
	}

	jobject AssetManager::GetInstance()
	{
		return _instance;
	}

	int AssetManager::ACCESS_BUFFER()
	{
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return 0;
		}

		jint result = _env->GetStaticIntField(_jcAssetManager, _jfAccessBuffer);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on get access buffer: %d", result);
		return result;
	}

	std::vector<std::string> AssetManager::list(const std::string& path)
	{
		std::vector<std::string> retVal;

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jstring arg1 = _env->NewStringUTF(path.c_str());
		jobject result = _env->CallObjectMethod(_instance, _mList, arg1);
		_env->DeleteLocalRef(arg1);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on list path");
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to list path");
			return retVal;
		}

		jsize const length = _env->GetArrayLength((jarray)result);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "List returned %d items", length);

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

	java_io_InputStream::InputStream AssetManager::open(const std::string& fileName)
	{
		java_io_InputStream::InputStream retVal = java_io_InputStream::InputStream(0);
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jstring arg1 = _env->NewStringUTF(fileName.c_str());
		jobject result = _env->CallObjectMethod(_instance, _mOpen, arg1);
		_env->DeleteLocalRef(arg1);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on open path");
			retVal.SetInstance(result);
			return retVal;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to open path");
			return retVal;
		}
	}

	java_io_InputStream::InputStream AssetManager::open(const std::string& fileName, int accessMode)
	{
		java_io_InputStream::InputStream retVal = java_io_InputStream::InputStream(0);
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jstring arg1 = _env->NewStringUTF(fileName.c_str());
		jint arg2 = accessMode;
		jobject result = _env->CallObjectMethod(_instance, _mOpen2, arg1, arg2);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to open file");
			return retVal;
		}
		_env->DeleteLocalRef(arg1);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on open file");
			retVal.SetInstance(result);
			return retVal;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to open file");
			return retVal;
		}
	}

	AssetFileDescriptor AssetManager::openFd(const std::string& fileName)
	{
		AssetFileDescriptor retVal = AssetFileDescriptor(0);
		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jstring arg1 = _env->NewStringUTF(fileName.c_str());
		jobject result = _env->CallObjectMethod(_instance, _mOpenFd, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to open file descriptor");
			return retVal;
		}
		_env->DeleteLocalRef(arg1);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on open file descriptor: %s", fileName.c_str());
			retVal = AssetFileDescriptor(result);
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on open file descriptor: length=%d", retVal.getLength());
			return retVal;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to open file");
			return retVal;
		}
	}
}