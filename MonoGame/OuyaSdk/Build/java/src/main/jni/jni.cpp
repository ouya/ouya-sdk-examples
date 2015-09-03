#include <jni.h>
#include <android/log.h>

#include <map>
#include <string>
#include <vector>

#define trace(fmt, ...) __android_log_print(ANDROID_LOG_DEBUG, "JNI", "trace: %s (%i) " fmt, __FUNCTION__, __LINE__, __VA_ARGS__)

#define LOG_TAG "jni.cpp"

#define LOG_VERBOSE false

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
	jint playerNum,
	jint axis,
	jfloat val)
{
#if LOG_VERBOSE
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "playerNum=%d axis=%d val=%f", playerNum, axis, val);
#endif
	if (playerNum < 0 ||
		playerNum >= MAX_CONTROLLERS)
	{
		playerNum = 0;
	}
	g_axis[playerNum][axis] = val;
}

void dispatchKeyEventNative(JNIEnv* env, jobject thiz,
	jint playerNum,
	jint keyCode,
	jint action)
{
#if LOG_VERBOSE
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "playerNum=%d KeyCode=%d Action=%d", playerNum, keyCode, action);
#endif
	if (playerNum < 0 ||
		playerNum >= MAX_CONTROLLERS)
	{
		playerNum = 0;
	}

	bool buttonDown = action == 0;

	if (g_button[playerNum][keyCode] != buttonDown)
	{
		g_button[playerNum][keyCode] = buttonDown;
		if (buttonDown)
		{
			g_buttonDown[playerNum][keyCode] = true;
		}
		else
		{
			g_buttonUp[playerNum][keyCode] = true;
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
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JNI_OnLoad");

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
		//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Native Plugin Initialized");
		jfieldID fieldNativeInitialized = env->GetStaticFieldID(clazz, "sNativeInitialized", "Z");	
		env->SetStaticBooleanField(clazz, fieldNativeInitialized, true);
		env->DeleteLocalRef(clazz);
	}
	else
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Failed to find OuyaInputView");
		return JNI_ERR;
	}
	
	return JNI_VERSION_1_6;
}

extern "C"
{
	// get axis value
	float getAxis(int playerNum, int axis)
	{
		if (g_axis.size() == 0) {
			return 0;
		}
		
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, float>::const_iterator search = g_axis[playerNum].find(axis);
		if (search != g_axis[playerNum].end())
		{
			return search->second;
		}
		return 0.0f;
	}

	// check if a button is pressed
	bool isPressed(int playerNum, int keyCode)
	{
		if (g_button.size() == 0) {
			return false;
		}
		
		if (g_buttonDown.size() == 0) {
			return false;
		}
		
		if (g_buttonUp.size() == 0) {
			return false;
		}
		
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, bool>::const_iterator search = g_button[playerNum].find(keyCode);
		if (search != g_button[playerNum].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was down
	bool isPressedDown(int playerNum, int keyCode)
	{
		if (g_buttonDown.size() == 0) {
			return false;
		}
		
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, bool>::const_iterator search = g_lastButtonDown[playerNum].find(keyCode);
		if (search != g_lastButtonDown[playerNum].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was up
	bool isPressedUp(int playerNum, int keyCode)
	{
		if (g_buttonUp.size() == 0) {
			return false;
		}
		
		if (playerNum < 0 ||
			playerNum >= MAX_CONTROLLERS)
		{
			playerNum = 0;
		}

		std::map<int, bool>::const_iterator search = g_lastButtonUp[playerNum].find(keyCode);
		if (search != g_lastButtonUp[playerNum].end())
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
		for (int playerNum = 0; playerNum < MAX_CONTROLLERS; ++playerNum)
		{
			g_lastButtonDown[playerNum].clear();
			g_lastButtonUp[playerNum].clear();
			for (std::map<int, bool>::iterator it = g_buttonDown[playerNum].begin(); it != g_buttonDown[playerNum].end(); ++it)
			{
				int keyCode = it->first;
				g_lastButtonDown[playerNum][keyCode] = g_buttonDown[playerNum][keyCode];
			}
			for (std::map<int, bool>::iterator it = g_buttonUp[playerNum].begin(); it != g_buttonUp[playerNum].end(); ++it)
			{
				int keyCode = it->first;
				g_lastButtonUp[playerNum][keyCode] = g_buttonUp[playerNum][keyCode];
			}
			g_buttonDown[playerNum].clear();
			g_buttonUp[playerNum].clear();
		}
	}

	// clear the axis values
	void clearAxes()
	{
		if (g_axis.size() == 0) {
			return;
		}
		for (int playerNum = 0; playerNum < MAX_CONTROLLERS; ++playerNum) {
			g_axis[playerNum].clear();
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
		for (int playerNum = 0; playerNum < MAX_CONTROLLERS; ++playerNum) {
			g_button[playerNum].clear();
			g_buttonDown[playerNum].clear();
			g_buttonUp[playerNum].clear();
			g_lastButtonDown[playerNum].clear();
			g_lastButtonUp[playerNum].clear();
		}
	}
}
