#include <android/log.h>
#include <jni.h>

#include "ClassLoader.h"

#define LOG_TAG "java_lang_ClassLoader"

namespace java_lang_ClassLoader
{
	JNIEnv* ClassLoader::_env = 0;
	jclass ClassLoader::_jcClassLoader = 0;
	jmethodID ClassLoader::_mGetSystemClassLoader = 0;
	jmethodID ClassLoader::_mLoadClass = 0;

	int ClassLoader::InitJNI(JNIEnv* env)
	{
		{
			const char* strClassLoaderClass = "java/lang/ClassLoader";
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Searching for %s", strClassLoaderClass);
			_jcClassLoader = env->FindClass(strClassLoaderClass);
			if (_jcClassLoader)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClassLoaderClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClassLoaderClass);
				return JNI_ERR;
			}
		}

		{
			const char* strClassLoaderGetSystemClassLoader = "getSystemClassLoader";
			_mGetSystemClassLoader = env->GetStaticMethodID(_jcClassLoader, strClassLoaderGetSystemClassLoader, "()Ljava/lang/ClassLoader;");
			if (_mGetSystemClassLoader)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClassLoaderGetSystemClassLoader);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClassLoaderGetSystemClassLoader);
				return JNI_ERR;
			}
		}

		{
			const char* strClassLoaderLoadClass = "loadClass";
			_mLoadClass = env->GetMethodID(_jcClassLoader, strClassLoaderLoadClass, "(Ljava/lang/String;Z)Ljava/lang/Class;");
			if (_mLoadClass)
			{
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Found %s", strClassLoaderLoadClass);
			}
			else
			{
				__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find %s", strClassLoaderLoadClass);
				return JNI_ERR;
			}
		}

		_env = env;

		return JNI_OK;
	}

	ClassLoader::ClassLoader(jobject classLoader)
	{
		_instance = classLoader;
	}

	jobject ClassLoader::GetInstance() const
	{
		return _instance;
	}

	ClassLoader ClassLoader::getSystemClassLoader()
	{
		ClassLoader retVal = ClassLoader(0);

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jobject result = (jobject)_env->CallStaticObjectMethod(_jcClassLoader, _mGetSystemClassLoader);
		if (result)
		{
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Success on get system class loader");
			retVal = ClassLoader(result);
			return retVal;
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get system class loader");
			return retVal;
		}

		return retVal;
	}
	
	std::string ClassLoader::loadClass(const std::string& className)
	{
		std::string retVal;

		if (!_env)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "JNI must be initialized with a valid environment!");
			return retVal;
		}

		jstring arg1 = _env->NewStringUTF(className.c_str());
		jstring result = (jstring)_env->CallObjectMethod(_instance, _mLoadClass, arg1);
		if (_env->ExceptionCheck())
		{
			_env->ExceptionDescribe();
			_env->ExceptionClear();
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to load class");
		}
		else
		{
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Success on load class");
		}
		_env->DeleteLocalRef(arg1);

		return retVal;
	}
}