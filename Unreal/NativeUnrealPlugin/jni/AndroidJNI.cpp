#include "AndroidJNI.h"

// OUYA handles remapping native input
#include "OuyaSDK_Bitmap.h"
#include "OuyaSDK_CallbacksContentSave.h"
#include "OuyaSDK_InputStream.h"
#include "OuyaSDK_OutputStream.h"
#include "OuyaSDK_OuyaContent.h"
#include "OuyaSDK_OuyaInputView.h"
#include "OuyaSDK_OuyaMod.h"
#include "OuyaSDK_OuyaModEditor.h"
#include "OuyaSDK_OuyaModScreenshot.h"
#include "OuyaSDK_PluginOuya.h"
#include "OuyaSDK_String.h"

#ifdef LOG_TAG
#undef LOG_TAG
#endif
#define LOG_TAG "AndroidJNI"

#ifdef ENABLE_VERBOSE_LOGGING
#undef ENABLE_VERBOSE_LOGGING
#endif
#define ENABLE_VERBOSE_LOGGING true

#define JNI_CURRENT_VERSION JNI_VERSION_1_6

JavaVM* GJavaVM;

// OUYA handles remapping native input
// Include the OUYA namespace

using namespace android_graphics_Bitmap;
using namespace java_io_InputStream;
using namespace java_io_OutputStream;
using namespace OuyaSDK;
using namespace std;
using namespace tv_ouya_console_api_content_OuyaContent;
using namespace tv_ouya_console_api_content_OuyaMod;
using namespace tv_ouya_console_api_content_OuyaModEditor;
using namespace tv_ouya_console_api_content_OuyaModScreenshot;
using namespace tv_ouya_sdk_OuyaInputView;

void unitTestReadFiles(const OuyaMod& ouyaMod)
{
	vector<string> filenames = ouyaMod.getFilenames();
	for (int index = 0; index < filenames.size(); ++index)
	{
		string filename = filenames[index];

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Opening filename=%s", filename.c_str());

		InputStream inputStream = ouyaMod.openFile(filename);

		const int length = 100000;
		signed char buffer[length];
		int readAmount = inputStream.read(buffer, length);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Read %d bytes", readAmount);

		char* contents = new char[readAmount+1];
		for (int index = 0; index < readAmount; ++index)
		{
			contents[index] = buffer[index];
		}
		contents[readAmount] = 0;
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Contents=%s", contents);
		string strContents = contents;
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Contents=%s", strContents.c_str());
		delete []contents;

		inputStream.close();
		inputStream.Dispose();
	}
}

class UnitTestCallbacksContentSave : public CallbacksContentSave
{
public:

	void OnError(OuyaMod ouyaMod, int code, const std::string& reason)
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "UnitTestCallbacksContentSave::OnError: code=%d reason=%s", code, reason.c_str());
	}

	void OnSuccess(OuyaMod ouyaMod)
	{
#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "UnitTestCallbacksContentSave::OnSuccess");
#endif

		unitTestReadFiles(ouyaMod);
	}
};

void unitTestAddTextFile(const OuyaMod& ouyaMod)
{
	OuyaModEditor ouyaModEditor = ouyaMod.edit();

	OutputStream outputStream = ouyaModEditor.newFile("AnotherFile.AnotherExtension");
	if (outputStream.GetInstance())
	{
		const char* contents = "Hello UE4 World!";
		int length = strlen(contents);
	#if ENABLE_VERBOSE_LOGGING
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Bytes are valid length=%d", length);
	#endif
		signed char* buffer = new signed char[length];
		for (int index = 0; index < length; ++index)
		{
			buffer[index] = contents[index];
		}

		outputStream.write(buffer, length);
		outputStream.flush();
		outputStream.close();

		delete[] buffer;

		outputStream.Dispose();
	}

	UnitTestCallbacksContentSave* callbacks = new UnitTestCallbacksContentSave();
	PluginOuya::saveOuyaMod(ouyaModEditor, ouyaMod, callbacks);

	ouyaModEditor.Dispose();
}

void unitTestReadScreenshots(const OuyaMod& ouyaMod)
{
	vector<OuyaModScreenshot> ouyaModScreenshots = ouyaMod.getScreenshots();

#if ENABLE_VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "unitTestReadScreenshots found %d screenshots", ouyaModScreenshots.size());
#endif

	for (int index = 0; index < ouyaModScreenshots.size(); ++index)
	{
		const OuyaModScreenshot& ouyaModScreenshot = ouyaModScreenshots[index];
		if (!ouyaModScreenshot.GetInstance())
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "unitTestReadScreenshots ouyaModScreenshot reference is null");
			continue;
		}
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "unitTestReadScreenshots ouyaModScreenshot is valid");

		Bitmap image = ouyaModScreenshot.getImage();
		if (!image.GetInstance())
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "unitTestReadScreenshots ouyaModScreenshot image reference is null");
			continue;
		}
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "unitTestReadScreenshots ouyaModScreenshot image is valid");
		int imageWidth = image.getWidth();
		int imageHeight = image.getHeight();
		int* imagePixels = new int[imageWidth*imageHeight];
		image.getPixels(imagePixels, 0, imageWidth, 0, 0, imageWidth, imageHeight);
		delete[] imagePixels;
		image.Dispose();

		Bitmap thumbnail = ouyaModScreenshot.getThumbnail();
		if (!thumbnail.GetInstance())
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "unitTestReadScreenshots ouyaModScreenshot thumbnail reference is null");
			continue;
		}
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "unitTestReadScreenshots ouyaModScreenshot thumbnail is valid");
		int thumbnailWidth = thumbnail.getWidth();
		int thumbnailHeight = thumbnail.getHeight();
		int* thumbnailPixels = new int[thumbnailWidth*thumbnailHeight];
		thumbnail.getPixels(thumbnailPixels, 0, thumbnailWidth, 0, 0, thumbnailWidth, thumbnailHeight);
		delete[] thumbnailPixels;
		thumbnail.Dispose();

		ouyaModScreenshot.Dispose();
	}
}

void unitTestNative(JNIEnv* env, jobject thiz, jobject localOuyaMod)
{
	jobject globalRef = (jobject)env->NewGlobalRef(localOuyaMod);
	env->DeleteLocalRef(localOuyaMod);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Invoked unitTestNative");
	OuyaMod ouyaMod = OuyaMod(globalRef);

	unitTestReadFiles(ouyaMod);

	unitTestReadScreenshots(ouyaMod);

	unitTestAddTextFile(ouyaMod);

	ouyaMod.Dispose();
}

JNINativeMethod nativeTableUnitTest[] = {
	{ "unitTestNative", "(Ltv/ouya/console/api/content/OuyaMod;)V", (void *)unitTestNative }
};

int nativeTableSizeUnitTest = sizeof(nativeTableUnitTest) / sizeof(nativeTableUnitTest[0]);

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

	{
		const char* strClass = "tv/ouya/examples/unreal/unrealcommunitycontent/MainActivity";
		jclass localRef = env->FindClass(strClass);
		jint ret = env->RegisterNatives(localRef, nativeTableUnitTest, nativeTableSizeUnitTest);
		if (ret < 0)
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to register native methods");
			return JNI_ERR;
		}
	}


	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "+++NATIVE++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "+++++UNIT++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++TESTS++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "++++++++++++++");

	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Run unit tests...");

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

void UnitTestUpdate()
{
	OuyaContent ouyaContent = OuyaContent::getInstance();

	if (ouyaContent.GetInstance()) {
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Got OuyaContent");

		if (ouyaContent.isInitialized()) {
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "OuyaContent is initialized");
		} else {
			__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Waiting for OuyaContent to initialize...");
		}
	} else {
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "OuyaContent not available yet..");
	}

	ouyaContent.Dispose();
}
