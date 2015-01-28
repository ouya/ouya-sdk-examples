#include "AndroidJNI.h"

// OUYA handles remapping native input
#include "OuyaSDK_OutputStream.h"
#include "OuyaSDK_OuyaInputView.h"
#include "OuyaSDK_PluginOuya.h"
#include "OuyaSDK_String.h"

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "AndroidJNI"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING false

#define JNI_CURRENT_VERSION JNI_VERSION_1_6

JavaVM* GJavaVM;

// OUYA handles remapping native input
// Include the OUYA namespace

using namespace java_io_OutputStream;
using namespace OuyaSDK;
using namespace tv_ouya_sdk_OuyaInputView;

JNIEXPORT jint JNI_OnLoad(JavaVM* InJavaVM, void* InReserved)
{
	JNIEnv* env = NULL;
	InJavaVM->GetEnv((void **)&env, JNI_CURRENT_VERSION);

	// if you have problems with stuff being missing esspecially in distribution builds then it could be because proguard is stripping things from java
	// check proguard-project.txt and see if your stuff is included in the exceptions
	GJavaVM = InJavaVM;

	/*
		START OF OUYA-SDK
	*/

	// setup sending native input to Java

#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Initialize the OuyaInputView classes");
#endif

	if (OuyaInputView::InitJNI(GJavaVM) == JNI_ERR)
	{
		return JNI_ERR;
	}

#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Initialize the PluginOuya classes");
#endif

	if (PluginOuya::InitJNI(GJavaVM) == JNI_ERR)
	{
		return JNI_ERR;
	}

	/*
		END OF OUYA-SDK
	*/


	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "+++NATIVE++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "+++++UNIT++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++TESTS++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");

	jstring testString = env->NewStringUTF("Hello World Test String");
	java_lang_String::String javaString = java_lang_String::String(testString);
	int length;
	int* bytes = javaString.getBytes(length);
	if (!bytes)
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to get bytes");
	}
	else
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Bytes are valid length=%d", length);

		//OutputStream os = OutputStream();
		//os.write(bytes, length);
		//os.flush();
		//os.close();

		delete bytes;
	}
	env->DeleteLocalRef(testString);


	return JNI_CURRENT_VERSION;
}
