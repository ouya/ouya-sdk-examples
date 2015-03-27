#include <jni.h>
#include <android/log.h>

#include <map>
#include <string>
#include <vector>

#define trace(fmt, ...) __android_log_print(ANDROID_LOG_DEBUG, "JNI", "trace: %s (%i) " fmt, __FUNCTION__, __LINE__, __VA_ARGS__)

#define LOG_TAG "lib-ouya-ndk.cpp"

#define VERBOSE_LOGGING false

#define MAX_CONTROLLERS 4

//axis states
static std::vector< std::map<int, float> > g_axis;

//button states
static std::vector< std::map<int, bool> > g_button;
static std::vector< std::map<int, bool> > g_buttonDown;
static std::vector< std::map<int, bool> > g_buttonUp;
static std::vector< std::map<int, bool> > g_lastButtonDown;
static std::vector< std::map<int, bool> > g_lastButtonUp;

void dispatchGenericMotionEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint axis,
	jfloat val)
{
#if VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Device=%d axis=%d val=%f", deviceId, axis, val);
#endif
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}
	g_axis[deviceId][axis] = val;
}

void dispatchKeyEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint keyCode,
	jint action)
{
#if VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Device=%d KeyCode=%d Action=%d", deviceId, keyCode, action);
#endif
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		deviceId = 0;
	}

	bool buttonDown = action == 0;

	if (g_button[deviceId][keyCode] != buttonDown)
	{
		g_button[deviceId][keyCode] = buttonDown;
		if (buttonDown)
		{
			g_buttonDown[deviceId][keyCode] = true;
		}
		else
		{
			g_buttonUp[deviceId][keyCode] = true;
		}
	}	
}

static JNINativeMethod method_table[] = {
	{ "dispatchGenericMotionEventNative", "(IIF)V", (void *)dispatchGenericMotionEventNative }
};

static int method_table_size = sizeof(method_table) / sizeof(method_table[0]);

static JNINativeMethod method_table2[] = {
	{ "dispatchKeyEventNative", "(III)V", (void *)dispatchKeyEventNative }
};

static int method_table_size2 = sizeof(method_table2) / sizeof(method_table2[0]);

jint JNI_OnLoad(JavaVM* vm, void* reserved)
{
#if VERBOSE_LOGGING
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JNI_OnLoad");
#endif

	for (int index = 0; index < MAX_CONTROLLERS; ++index)
	{
		g_axis.push_back(std::map<int, float>());
		g_button.push_back(std::map<int, bool>());
		g_buttonDown.push_back(std::map<int, bool>());
		g_buttonUp.push_back(std::map<int, bool>());
		g_lastButtonDown.push_back(std::map<int, bool>());
		g_lastButtonUp.push_back(std::map<int, bool>());
	}

	JNIEnv* env;
	if (vm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK)
	{
		return JNI_ERR;
	}

	jclass clazz = env->FindClass("tv/ouya/sdk/OuyaInputView");
	if (clazz)
	{
		jint ret = env->RegisterNatives(clazz, method_table, method_table_size);
		ret = env->RegisterNatives(clazz, method_table2, method_table_size2);
		jfieldID fieldNativeInitialized = env->GetStaticFieldID(clazz, "sNativeInitialized", "Z");
		if (fieldNativeInitialized)
		{
			env->SetStaticBooleanField(clazz, fieldNativeInitialized, true);
			env->DeleteLocalRef(clazz);
#if VERBOSE_LOGGING
			__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Native plugin has loaded.");
#endif
		}
		else
		{
			__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find sNativeInitialized field");
			env->DeleteLocalRef(clazz);
			return JNI_ERR;
		}
	}
	else
	{
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Failed to find OuyaInputView class");
		return JNI_ERR;
	}

	return JNI_VERSION_1_6;
}

extern "C"
{
	// Hello world interface
	char* AndroidGetHelloWorld(long* size)
	{
		const char* cString = "Hello World!\0";
		*size = strlen(cString);
		char* result = new char[*cString];
		strcpy(result, cString);
		return result;
	}

	// Release unmanaged memory
	void AndroidReleaseMemory(char* buffer)
	{
		if (NULL == buffer)
		{
			return;
		}

		delete buffer;
	}

	// Example interface
	void AndroidExampleFunction1(unsigned char* a, int b, int* c)
	{
		(*c) = 3;
	}

	// get axis value
	float getAxis(int deviceId, int axis)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return 0.0f;
		}

		std::map<int, float>::const_iterator search = g_axis[deviceId].find(axis);
		if (search != g_axis[deviceId].end())
		{
			return search->second;
		}
		return 0.0f;
	}

	// check if a button is pressed
	bool isPressed(int deviceId, int keyCode)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return false;
		}

		std::map<int, bool>::const_iterator search = g_button[deviceId].find(keyCode);
		if (search != g_button[deviceId].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was down
	bool isPressedDown(int deviceId, int keyCode)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return false;
		}

		std::map<int, bool>::const_iterator search = g_lastButtonDown[deviceId].find(keyCode);
		if (search != g_lastButtonDown[deviceId].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was up
	bool isPressedUp(int deviceId, int keyCode)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return false;
		}

		std::map<int, bool>::const_iterator search = g_lastButtonUp[deviceId].find(keyCode);
		if (search != g_lastButtonUp[deviceId].end())
		{
			return search->second;
		}
		return false;
	}

	// clear the button state for detecting up and down
	void clearButtonStates()
	{
		if (g_buttonDown.size() == 0) {
			return;
		}
		if (g_buttonUp.size() == 0) {
			return;
		}
		for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId)
		{
			g_lastButtonDown[deviceId].clear();
			g_lastButtonUp[deviceId].clear();
			for (std::map<int, bool>::iterator it = g_buttonDown[deviceId].begin(); it != g_buttonDown[deviceId].end(); ++it)
			{
				int keyCode = it->first;
				g_lastButtonDown[deviceId][keyCode] = g_buttonDown[deviceId][keyCode];
			}
			for (std::map<int, bool>::iterator it = g_buttonUp[deviceId].begin(); it != g_buttonUp[deviceId].end(); ++it)
			{
				int keyCode = it->first;
				g_lastButtonUp[deviceId][keyCode] = g_buttonUp[deviceId][keyCode];
			}
			g_buttonDown[deviceId].clear();
			g_buttonUp[deviceId].clear();
		}
	}

	// clear the axis values
	void clearAxes()
	{
		if (g_axis.size() == 0) {
			return;
		}
		for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId) {
			g_axis[deviceId].clear();
		}
	}

	// clear the button values
	void clearButtons()
	{
		if (g_button.size() == 0) {
			return;
		}
		if (g_buttonDown.size() == 0) {
			return;
		}
		if (g_buttonUp.size() == 0) {
			return;
		}
		for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId) {
			g_button[deviceId].clear();
			g_buttonDown[deviceId].clear();
			g_buttonUp[deviceId].clear();
		}
	}
}
