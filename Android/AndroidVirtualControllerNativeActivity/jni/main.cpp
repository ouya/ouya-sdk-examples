/*
 * Copyright (C) 2010 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

//BEGIN_INCLUDE(all)
#include <algorithm>
#include <jni.h>
#include <errno.h>
#include <map>
#include <math.h>
#include <stdlib.h>

#include <EGL/egl.h>
#include <GLES/gl.h>

#include <android/bitmap.h>
#include <android/log.h>
#include <android/sensor.h>
#include "android_native_app_glue.h"

#include "Activity.h"
#include "ApplicationInfo.h"
#include "AudioFormat.h"
#include "AudioManager.h"
#include "AudioTrack.h"
#include "AssetFileDescriptor.h"
#include "AssetManager.h"
#include "Bitmap.h"
#include "BitmapFactory.h"
#include "Build.h"
#include "ClassLoader.h"
#include "DexFile.h"
#include "Context.h"
#include "GLUtils.h"
#include "InputDevice.h"
#include "InputStream.h"
#include "JSONArray.h"
#include "JSONObject.h"
#include "MappingParser.h"
#include "OuyaController.h"
#include "SoundPool.h"
#include "String.h"
#include "System.h"

//sound assets
#include "Audio_Explode.h"
#include "Audio_Hit.h"

#define LOG_TAG "VirtualControllerNativeActivity"

#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO, LOG_TAG, __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN, LOG_TAG, __VA_ARGS__))
#define LOGE(...) ((void)__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, __VA_ARGS__))

using namespace android_app_Activity;
using namespace android_content_Context;
using namespace android_content_pm_ApplicationInfo;
using namespace android_content_res_AssetFileDescriptor;
using namespace android_content_res_AssetManager;
using namespace android_graphics_Bitmap;
using namespace android_graphics_BitmapFactory;
using namespace android_opengl_GLUtils;
using namespace android_os_Build;
using namespace audio_media_AudioFormat;
using namespace android_media_AudioManager;
using namespace android_media_AudioTrack;
using namespace android_media_SoundPool;
using namespace android_view_InputDevice;
using namespace dalvik_system_DexFile;
using namespace java_io_InputStream;
using namespace java_lang_ClassLoader;
using namespace java_lang_String;
using namespace java_lang_System;
using namespace org_json_JSONArray;
using namespace org_json_JSONObject;
using namespace tv_ouya_console_api_OuyaController;

using namespace OuyaEverywhere;

//controller remapping
MappingParser g_parser;

// android.os.Build.MODEL
static std::string g_model;

// InputDevice names
static std::map<int, std::string> g_device;

//axis states
static std::map<int, float> g_axis;

//button states
static std::map<int, bool> g_button;

//sound ids
int g_soundBoom = 0;
int g_soundOuch = 0;

//gles textures
const int TEXTURE_COUNT = 17;
static GLuint textures[TEXTURE_COUNT];

// texture Ids
static int g_controller = 0;
static int g_buttonA = 0;
static int g_buttonO = 0;
static int g_buttonU = 0;
static int g_buttonY = 0;
static int g_dpadDown = 0;
static int g_dpadLeft = 0;
static int g_dpadRight = 0;
static int g_dpadUp = 0;
static int g_leftBumper = 0;
static int g_leftTrigger = 0;
static int g_leftStickInactive = 0;
static int g_leftStickActive = 0;
static int g_rightBumper = 0;
static int g_rightTrigger = 0;
static int g_rightStickInactive = 0;
static int g_rightStickActive = 0;

// vbo buffer
static unsigned int g_vbo[3];

// position verteces
const float POS_LEFT = -0.1f;
const float POS_RIGHT = 0.1f;
const float POS_TOP = 0.15f;
const float POS_BOTTOM = -0.15f;
static float g_positions[12] =
{
	POS_RIGHT, POS_BOTTOM, 0.0,
	POS_RIGHT, POS_TOP, 0.0,
	POS_LEFT, POS_BOTTOM, 0.0,
	POS_LEFT, POS_TOP, 0.0
};
static float g_positions2[12] =
{
	POS_RIGHT, POS_BOTTOM, 0.0,
	POS_RIGHT, POS_TOP, 0.0,
	POS_LEFT, POS_BOTTOM, 0.0,
	POS_LEFT, POS_TOP, 0.0
};

// texture coordinates
const float TEX_LEFT = 0.0f;
const float TEX_RIGHT = 1.0f;
const float TEX_TOP = 0.0f;
const float TEX_BOTTOM = 1.0f;
static float g_textureCoords[8] =
{
	TEX_RIGHT, TEX_BOTTOM,
	TEX_RIGHT, TEX_TOP,
	TEX_LEFT, TEX_BOTTOM,
	TEX_LEFT, TEX_TOP
};

// face indices
static short g_indices[4] = { 0, 1, 2, 3 };

// defer initialization of the sound pool
static SoundPool g_soundPool = SoundPool(0);

// defer initialization of the audio track
static AudioTrack g_audioTrack = AudioTrack(0);

// get device name
static std::string getDeviceName(int id)
{
	std::map<int, std::string>::const_iterator search = g_device.find(id);
	if (search != g_device.end())
	{
		return search->second;
	}

	InputDevice device = InputDevice::getDevice(id);
	std::string name = device.getName();
	g_device[id] = name;

	return name;
}

// get axis value
static float getAxis(int axis)
{
	std::map<int, float>::const_iterator search = g_axis.find(axis);
	if (search != g_axis.end())
	{
		return search->second;
	}
	return 0.0f;
}

// check if a button is pressed
static bool isPressed(int keyCode)
{
	std::map<int, bool>::const_iterator search = g_button.find(keyCode);
	if (search != g_button.end())
	{
		return search->second;
	}
	return false;
}

/**
 * Our saved state data.
 */
struct saved_state {
    float angle;
    int32_t x;
    int32_t y;
};

/**
 * Shared state for our app.
 */
struct engine {
    struct android_app* app;

    ASensorManager* sensorManager;
    const ASensor* accelerometerSensor;
    ASensorEventQueue* sensorEventQueue;

    EGLDisplay display;
    EGLSurface surface;
    EGLContext context;
    int32_t width;
    int32_t height;
    struct saved_state state;
};

jint JNI_OnLoad(JavaVM* vm, void* reserved)
{
	JNIEnv* env;
	if (vm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK) {
		return -1;
	}

	LOGI("***************");
	LOGI("***************");
	LOGI("*****JNI*******");
	LOGI("******ONLOAD***");
	LOGI("***************");
	LOGI("***************");

	// Get jclass with env->FindClass.
	// Register methods with env->RegisterNatives.

	if (JNI_ERR == OuyaController::InitJNI(env))
	{
		return JNI_ERR;
	}

	return JNI_VERSION_1_6;
}

jint RegisterClasses(ANativeActivity* activity)
{
	JNIEnv* env = activity->env;

	JavaVM* vm = activity->vm;
	vm->AttachCurrentThread(&env, NULL);

	jclass activityClass = env->GetObjectClass(activity->clazz);
	if (!activityClass)
	{
		LOGE("Failed to find activity class");
		return JNI_ERR;
	}
	else
	{
		LOGI("Found activity class");
	}

	// load the system first
	if (JNI_ERR == System::InitJNI(env))
	{
		return JNI_ERR;
	}

	// load the class loader second
	if (JNI_ERR == ClassLoader::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == Activity::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == ApplicationInfo::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == AudioManager::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == AudioTrack::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == AssetFileDescriptor::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == AssetManager::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == Bitmap::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == BitmapFactory::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == BitmapFactory::Options::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == Build::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == Context::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == DexFile::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == GLUtils::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == InputDevice::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == InputStream::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == JSONArray::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == JSONObject::InitJNI(env))
	{
		return JNI_ERR;
	}

	/*
	if (JNI_ERR == OuyaController::InitJNI(env))
	{
		return JNI_ERR;
	}
	*/

	if (JNI_ERR == SoundPool::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == String::InitJNI(env))
	{
		return JNI_ERR;
	}

	return JNI_VERSION_1_6;
}

int LoadTexture(JNIEnv* env, AssetManager& assetManager, const BitmapFactory::Options& options, const std::string& texturePath, int textureId)
{
	InputStream stream = assetManager.open(texturePath);
	Bitmap bitmap = BitmapFactory::decodeStream(stream, 0, options);
	stream.close();
	int width = bitmap.getWidth();
	int height = bitmap.getHeight();
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded %s bitmap width=%d height=%d", texturePath.c_str(), width, height);

	AndroidBitmapInfo info = AndroidBitmapInfo();
	AndroidBitmap_getInfo(env, bitmap.GetInstance(), &info);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.width=%d", info.width);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.height=%d", info.height);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.stride=%d", info.stride);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.format=%d", info.format);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.flags=%d", info.flags);

	if (info.format != ANDROID_BITMAP_FORMAT_RGBA_8888) {
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Bitmap format is not RGBA_8888! %s", texturePath.c_str());
		return -1;
	}

	glBindTexture(GL_TEXTURE_2D, textures[textureId]);
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "glBindTexture=%d", textures[textureId]);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

	android_opengl_GLUtils::GLUtils::texImage2D(GL_TEXTURE_2D, 0, bitmap.GetInstance(), 0);

	bitmap.recycle();

	return textureId;
}

void LoadBitmaps(JavaVM* vm, JNIEnv* env, jobject objActivity)
{
	vm->AttachCurrentThread(&env, NULL);

	BitmapFactory::Options options = BitmapFactory::Options();
	options.set_inScaled(false); // No pre-scaling
	Activity activity = Activity(objActivity);
	Context context = activity.getApplicationContext();
	AssetManager assetManager = context.getAssets();

	std::vector<std::string> files = assetManager.list("");
	for (int index = 0; index < files.size(); ++index)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, files[index].c_str());
	}

	glGenTextures(TEXTURE_COUNT, &textures[0]);

	int textureId = 0;
	g_controller = LoadTexture(env, assetManager, options, "controller.png", textureId);
	g_buttonA = LoadTexture(env, assetManager, options, "a.png", ++textureId);
	g_dpadDown = LoadTexture(env, assetManager, options, "dpad_down.png", ++textureId);
	g_dpadLeft = LoadTexture(env, assetManager, options, "dpad_left.png", ++textureId);
	g_dpadRight = LoadTexture(env, assetManager, options, "dpad_right.png", ++textureId);
	g_dpadUp = LoadTexture(env, assetManager, options, "dpad_up.png", ++textureId);
	g_leftBumper = LoadTexture(env, assetManager, options, "lb.png", ++textureId);
	g_leftTrigger = LoadTexture(env, assetManager, options, "lt.png", ++textureId);
	g_leftStickInactive = LoadTexture(env, assetManager, options, "l_stick.png", ++textureId);
	g_buttonO = LoadTexture(env, assetManager, options, "o.png", ++textureId);
	g_rightBumper = LoadTexture(env, assetManager, options, "rb.png", ++textureId);
	g_rightTrigger = LoadTexture(env, assetManager, options, "rt.png", ++textureId);
	g_rightStickInactive = LoadTexture(env, assetManager, options, "r_stick.png", ++textureId);
	g_leftStickActive = LoadTexture(env, assetManager, options, "thumbl.png", ++textureId);
	g_rightStickActive = LoadTexture(env, assetManager, options, "thumbr.png", ++textureId);
	g_buttonU = LoadTexture(env, assetManager, options, "u.png", ++textureId);
	g_buttonY = LoadTexture(env, assetManager, options, "y.png", ++textureId);

	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded %d textures", textureId + 1);
}

/**
 * Initialize an EGL context for the current display.
 */
static int engine_init_display(struct engine* engine) {
    // initialize OpenGL ES and EGL
	if (JNI_ERR == RegisterClasses(engine->app->activity))
	{
		return JNI_ERR;
	}

	g_model = Build::MODEL();

	{
		Activity activity = Activity(engine->app->activity->clazz);
		Context context = activity.getApplicationContext();
		AssetManager assetManager = context.getAssets();

		InputStream inputStream = assetManager.open("input.json", AssetManager::ACCESS_BUFFER());
		int length = inputStream.available();
		jbyte* configurationBytes = new jbyte[length];
		inputStream.read(configurationBytes, length);
		String json = String(configurationBytes, length);
		std::string strJson = json.ToString();
		inputStream.close();
		delete configurationBytes;

		g_parser.parse(strJson);

		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "****SOUND*******");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "*****POOL*******");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "****************");
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "****************");

		g_soundPool = SoundPool(5, AudioManager::STREAM_MUSIC(), 0);

		// load the resources
		AssetFileDescriptor afd1 = assetManager.openFd("boom.wav");
		g_soundBoom = g_soundPool.load(afd1, 1);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "boom.wav file=%d length=%ld", g_soundBoom, afd1.getLength());

		AssetFileDescriptor afd2 = assetManager.openFd("ouch.wav");
		g_soundOuch = g_soundPool.load(afd2, 1);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "ouch.wav file=%d length=%ld", g_soundOuch, afd2.getLength());

		g_audioTrack = AudioTrack(AudioManager::STREAM_MUSIC(), 11025, AudioFormat::CHANNEL_OUT_MONO(), AudioFormat::ENCODING_PCM_8BIT(), 11025, AudioTrack::MODE_STREAM());

		g_audioTrack.write((jbyte*)Assets::AUDIO_EXPLODE, 0, sizeof(Assets::AUDIO_EXPLODE));
	}

    /*
     * Here specify the attributes of the desired configuration.
     * Below, we select an EGLConfig with at least 8 bits per color
     * component compatible with on-screen windows
     */
    const EGLint attribs[] = {
            EGL_SURFACE_TYPE, EGL_WINDOW_BIT,
            EGL_BLUE_SIZE, 8,
            EGL_GREEN_SIZE, 8,
            EGL_RED_SIZE, 8,
            EGL_NONE
    };
    EGLint w, h, dummy, format;
    EGLint numConfigs;
    EGLConfig config;
    EGLSurface surface;
    EGLContext context;

    EGLDisplay display = eglGetDisplay(EGL_DEFAULT_DISPLAY);

    eglInitialize(display, 0, 0);

    /* Here, the application chooses the configuration it desires. In this
     * sample, we have a very simplified selection process, where we pick
     * the first EGLConfig that matches our criteria */
    eglChooseConfig(display, attribs, &config, 1, &numConfigs);

    /* EGL_NATIVE_VISUAL_ID is an attribute of the EGLConfig that is
     * guaranteed to be accepted by ANativeWindow_setBuffersGeometry().
     * As soon as we picked a EGLConfig, we can safely reconfigure the
     * ANativeWindow buffers to match, using EGL_NATIVE_VISUAL_ID. */
    eglGetConfigAttrib(display, config, EGL_NATIVE_VISUAL_ID, &format);

    ANativeWindow_setBuffersGeometry(engine->app->window, 0, 0, format);

    surface = eglCreateWindowSurface(display, config, engine->app->window, NULL);
    context = eglCreateContext(display, config, NULL, NULL);

    if (eglMakeCurrent(display, surface, surface, context) == EGL_FALSE) {
        LOGW("Unable to eglMakeCurrent");
        return -1;
    }

    eglQuerySurface(display, surface, EGL_WIDTH, &w);
    eglQuerySurface(display, surface, EGL_HEIGHT, &h);

    engine->display = display;
    engine->context = context;
    engine->surface = surface;
    engine->width = w;
    engine->height = h;
    engine->state.angle = 0;

    // Initialize GL state.
    glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_FASTEST);
    glEnable(GL_CULL_FACE);
	glEnable(GL_TEXTURE_2D);
    glShadeModel(GL_SMOOTH);
    glDisable(GL_DEPTH_TEST);

	LoadBitmaps(engine->app->activity->vm, engine->app->activity->env, engine->app->activity->clazz);

	glGenBuffers(3, g_vbo);
	glBindBuffer(GL_ARRAY_BUFFER, g_vbo[0]);
	glBufferData(GL_ARRAY_BUFFER, 4 * 12, g_positions, GL_STATIC_DRAW);

	glBindBuffer(GL_ARRAY_BUFFER, g_vbo[1]);
	glBufferData(GL_ARRAY_BUFFER, 4 * 8, g_textureCoords, GL_STATIC_DRAW);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, g_vbo[2]);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, 2 * 4, g_indices, GL_STATIC_DRAW);

    return 0;
}

static void DrawTexture(int textureId)
{
	glBindBuffer(GL_ARRAY_BUFFER, g_vbo[0]);
	glBufferData(GL_ARRAY_BUFFER, 4 * 12, g_positions, GL_STATIC_DRAW);

	glBindTexture(GL_TEXTURE_2D, textures[textureId]);
	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, 0);
}

static void DrawTexture(int textureId, float offsetX, float offsetY)
{
	g_positions2[0] = g_positions[0] + offsetX;
	g_positions2[3] = g_positions[3] + offsetX;
	g_positions2[6] = g_positions[6] + offsetX;
	g_positions2[9] = g_positions[9] + offsetX;

	g_positions2[1] = g_positions[1] + offsetY;
	g_positions2[4] = g_positions[4] + offsetY;
	g_positions2[7] = g_positions[7] + offsetY;
	g_positions2[10] = g_positions[10] + offsetY;

	glBindBuffer(GL_ARRAY_BUFFER, g_vbo[0]);
	glBufferData(GL_ARRAY_BUFFER, 4 * 12, g_positions2, GL_STATIC_DRAW);

	glBindTexture(GL_TEXTURE_2D, textures[textureId]);
	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, 0);
}

/**
 * Just the current frame in the display.
 */
static void engine_draw_frame(struct engine* engine) {
    if (engine->display == NULL) {
        // No display.
        return;
    }

	glClearColor(0, 0, 0, 1);

	glClear(GL_COLOR_BUFFER_BIT);

	// model view matrix
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	
	//glRotatef(90, 0, 0, 1);
	glScalef(4, 4, 4);

	glEnableClientState(GL_VERTEX_ARRAY);
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);

	// draw the model
	glBindBuffer(GL_ARRAY_BUFFER, g_vbo[0]);
	glVertexPointer(3, GL_FLOAT, 0, 0);
	glBindBuffer(GL_ARRAY_BUFFER, g_vbo[1]);
	glTexCoordPointer(2, GL_FLOAT, 0, 0);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	DrawTexture(g_controller);
	if (isPressed(OuyaController::BUTTON_A()))
	{
		DrawTexture(g_buttonA);
	}
	
	if (isPressed(OuyaController::BUTTON_DPAD_DOWN()))
	{
		DrawTexture(g_dpadDown);
	}

	if (isPressed(OuyaController::BUTTON_DPAD_LEFT()))
	{
		DrawTexture(g_dpadLeft);
	}

	if (isPressed(OuyaController::BUTTON_DPAD_RIGHT()))
	{
		DrawTexture(g_dpadRight);
	}

	if (isPressed(OuyaController::BUTTON_DPAD_UP()))
	{
		DrawTexture(g_dpadUp);
	}

	if (isPressed(OuyaController::BUTTON_L1()))
	{
		DrawTexture(g_leftBumper);
	}

	if (isPressed(OuyaController::BUTTON_O()))
	{
		DrawTexture(g_buttonO);
	}

	if (isPressed(OuyaController::BUTTON_R1()))
	{
		DrawTexture(g_rightBumper);
	}

	if (isPressed(OuyaController::BUTTON_U()))
	{
		DrawTexture(g_buttonU);
	}

	if (isPressed(OuyaController::BUTTON_Y()))
	{
		DrawTexture(g_buttonY);
	}

	if (getAxis(OuyaController::AXIS_L2()) > 0.25f)
	{
		DrawTexture(g_leftTrigger);
	}

	if (getAxis(OuyaController::AXIS_R2()) > 0.25f)
	{
		DrawTexture(g_rightTrigger);
	}

	const float AXIS_SENSITIVITY = 0.005f;

	// rotate input by N degrees to match image
	const float degrees = 135;
	const float radians = degrees / 180.0f * 3.14f;
	const float cos = (float)cosf(radians);
	const float sin = (float)sinf(radians);

	float lx = getAxis(OuyaController::AXIS_LS_X());
	float ly = getAxis(OuyaController::AXIS_LS_Y());
	if (isPressed(OuyaController::BUTTON_L3()))
	{
		DrawTexture(g_leftStickActive, AXIS_SENSITIVITY * (lx * cos - ly * sin), -AXIS_SENSITIVITY * (lx * sin + ly * cos));
	}
	else
	{
		DrawTexture(g_leftStickInactive, AXIS_SENSITIVITY * (lx * cos - ly * sin), -AXIS_SENSITIVITY * (lx * sin + ly * cos));
	}

	float rx = getAxis(OuyaController::AXIS_RS_X());
	float ry = getAxis(OuyaController::AXIS_RS_Y());
	if (isPressed(OuyaController::BUTTON_R3()))
	{
		DrawTexture(g_rightStickActive, AXIS_SENSITIVITY * (rx * cos - ry * sin), -AXIS_SENSITIVITY * (rx * sin + ry * cos));
	}
	else
	{
		DrawTexture(g_rightStickInactive, AXIS_SENSITIVITY * (rx * cos - ry * sin), -AXIS_SENSITIVITY * (rx * sin + ry * cos));
	}

	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);

    eglSwapBuffers(engine->display, engine->surface);
}

/**
 * Tear down the EGL context currently associated with the display.
 */
static void engine_term_display(struct engine* engine) {
    if (engine->display != EGL_NO_DISPLAY) {
        eglMakeCurrent(engine->display, EGL_NO_SURFACE, EGL_NO_SURFACE, EGL_NO_CONTEXT);
        if (engine->context != EGL_NO_CONTEXT) {
            eglDestroyContext(engine->display, engine->context);
        }
        if (engine->surface != EGL_NO_SURFACE) {
            eglDestroySurface(engine->display, engine->surface);
        }
        eglTerminate(engine->display);
    }
    engine->display = EGL_NO_DISPLAY;
    engine->context = EGL_NO_CONTEXT;
    engine->surface = EGL_NO_SURFACE;
}

static void debugButton(int button)
{
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "%d, %s value=%d", button, MappingParser::debugGetButtonName(button).c_str(), isPressed(button));
}

static void debugOuyaKeyEvent()
{
	debugButton(OuyaController::BUTTON_O());
	debugButton(OuyaController::BUTTON_U());
	debugButton(OuyaController::BUTTON_Y());
	debugButton(OuyaController::BUTTON_A());;
	debugButton(OuyaController::BUTTON_L1());
	debugButton(OuyaController::BUTTON_L3());
	debugButton(OuyaController::BUTTON_R1());
	debugButton(OuyaController::BUTTON_R3());
	debugButton(OuyaController::BUTTON_MENU());
	debugButton(OuyaController::BUTTON_DPAD_UP());
	debugButton(OuyaController::BUTTON_DPAD_RIGHT());
	debugButton(OuyaController::BUTTON_DPAD_DOWN());
	debugButton(OuyaController::BUTTON_DPAD_LEFT());
}

static void debugAxis(int axis)
{
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "%d, %s value=%f", axis, MappingParser::debugGetAxisName(axis).c_str(), getAxis(axis));
}

static void debugOuyaMotionEvent()
{
	debugAxis(OuyaController::AXIS_LS_X());
	debugAxis(OuyaController::AXIS_LS_Y());
	debugAxis(OuyaController::AXIS_RS_X());
	debugAxis(OuyaController::AXIS_RS_Y());
	debugAxis(OuyaController::AXIS_L2());
	debugAxis(OuyaController::AXIS_R2());
}

static void debugMotionEvent(AInputEvent* motionEvent)
{
	std::map<int, const char*> names;
	/*
	names[AMOTION_EVENT_AXIS_X] = "AXIS_X";
	names[AMOTION_EVENT_AXIS_Y] = "AXIS_Y";
	names[AMOTION_EVENT_AXIS_PRESSURE] = "AXIS_PRESSURE";
	names[AMOTION_EVENT_AXIS_SIZE] = "AXIS_SIZE";
	names[AMOTION_EVENT_AXIS_TOUCH_MAJOR] = "AXIS_TOUCH_MAJOR";
	names[AMOTION_EVENT_AXIS_TOUCH_MINOR] = "AXIS_TOUCH_MINOR";
	names[AMOTION_EVENT_AXIS_TOOL_MAJOR] = "AXIS_TOOL_MAJOR";
	names[AMOTION_EVENT_AXIS_TOOL_MINOR] = "AXIS_TOOL_MINOR";
	names[AMOTION_EVENT_AXIS_ORIENTATION] = "AXIS_ORIENTATION";
	names[AMOTION_EVENT_AXIS_VSCROLL] = "AXIS_VSCROLL";
	names[AMOTION_EVENT_AXIS_HSCROLL] = "AXIS_HSCROLL";
	names[AMOTION_EVENT_AXIS_Z] = "AXIS_Z";
	names[AMOTION_EVENT_AXIS_RX] = "AXIS_RX";
	names[AMOTION_EVENT_AXIS_RY] = "AXIS_RY";
	names[AMOTION_EVENT_AXIS_RZ] = "AXIS_RZ";
	names[AMOTION_EVENT_AXIS_HAT_X] = "AXIS_HAT_X";
	names[AMOTION_EVENT_AXIS_HAT_Y] = "AXIS_HAT_Y";
	names[AMOTION_EVENT_AXIS_LTRIGGER] = "AXIS_LTRIGGER";
	names[AMOTION_EVENT_AXIS_RTRIGGER] = "AXIS_RTRIGGER";
	names[AMOTION_EVENT_AXIS_THROTTLE] = "AXIS_THROTTLE";
	names[AMOTION_EVENT_AXIS_RUDDER] = "AXIS_RUDDER";
	names[AMOTION_EVENT_AXIS_WHEEL] = "AXIS_WHEEL";
	names[AMOTION_EVENT_AXIS_GAS] = "AXIS_GAS";
	names[AMOTION_EVENT_AXIS_BRAKE] = "AXIS_BRAKE";
	names[AMOTION_EVENT_AXIS_DISTANCE] = "AXIS_DISTANCE";
	names[AMOTION_EVENT_AXIS_TILT] = "AXIS_TILT";
	names[AMOTION_EVENT_AXIS_GENERIC_1] = "AXIS_GENERIC_1";
	names[AMOTION_EVENT_AXIS_GENERIC_2] = "AXIS_GENERIC_2";
	names[AMOTION_EVENT_AXIS_GENERIC_3] = "AXIS_GENERIC_3";
	names[AMOTION_EVENT_AXIS_GENERIC_4] = "AXIS_GENERIC_4";
	names[AMOTION_EVENT_AXIS_GENERIC_5] = "AXIS_GENERIC_5";
	names[AMOTION_EVENT_AXIS_GENERIC_6] = "AXIS_GENERIC_6";
	names[AMOTION_EVENT_AXIS_GENERIC_7] = "AXIS_GENERIC_7";
	names[AMOTION_EVENT_AXIS_GENERIC_8] = "AXIS_GENERIC_8";
	names[AMOTION_EVENT_AXIS_GENERIC_9] = "AXIS_GENERIC_9";
	names[AMOTION_EVENT_AXIS_GENERIC_10] = "AXIS_GENERIC_10";
	names[AMOTION_EVENT_AXIS_GENERIC_11] = "AXIS_GENERIC_11";
	names[AMOTION_EVENT_AXIS_GENERIC_12] = "AXIS_GENERIC_12";
	names[AMOTION_EVENT_AXIS_GENERIC_13] = "AXIS_GENERIC_13";
	names[AMOTION_EVENT_AXIS_GENERIC_14] = "AXIS_GENERIC_14";
	names[AMOTION_EVENT_AXIS_GENERIC_15] = "AXIS_GENERIC_15";
	names[AMOTION_EVENT_AXIS_GENERIC_16] = "AXIS_GENERIC_16";
	*/

	std::map<int, const char*>::iterator it = names.begin();
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_X, "AXIS_X"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_Y, "AXIS_Y"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_PRESSURE, "AXIS_PRESSURE"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_SIZE, "AXIS_SIZE"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_TOUCH_MAJOR, "AXIS_TOUCH_MAJOR"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_TOUCH_MINOR, "AXIS_TOUCH_MINOR"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_TOOL_MAJOR, "AXIS_TOOL_MAJOR"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_TOOL_MINOR, "AXIS_TOOL_MINOR"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_ORIENTATION, "AXIS_ORIENTATION"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_VSCROLL, "AXIS_VSCROLL"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_HSCROLL, "AXIS_HSCROLL"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_Z, "AXIS_Z"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_RX, "AXIS_RX"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_RY, "AXIS_RY"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_RZ, "AXIS_RZ"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_HAT_X, "AXIS_HAT_X"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_HAT_Y, "AXIS_HAT_Y"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_LTRIGGER, "AXIS_LTRIGGER"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_RTRIGGER, "AXIS_RTRIGGER"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_THROTTLE, "AXIS_THROTTLE"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_RUDDER, "AXIS_RUDDER"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_WHEEL, "AXIS_WHEEL"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GAS, "AXIS_GAS"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_BRAKE, "AXIS_BRAKE"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_DISTANCE, "AXIS_DISTANCE"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_TILT, "AXIS_TILT"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_1, "AXIS_GENERIC_1"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_2, "AXIS_GENERIC_2"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_3, "AXIS_GENERIC_3"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_4, "AXIS_GENERIC_4"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_5, "AXIS_GENERIC_5"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_6, "AXIS_GENERIC_6"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_7, "AXIS_GENERIC_7"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_8, "AXIS_GENERIC_8"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_9, "AXIS_GENERIC_9"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_10, "AXIS_GENERIC_10"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_11, "AXIS_GENERIC_11"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_12, "AXIS_GENERIC_12"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_13, "AXIS_GENERIC_13"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_14, "AXIS_GENERIC_14"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_15, "AXIS_GENERIC_15"));
	names.insert(it, std::pair<int, const char*>(AMOTION_EVENT_AXIS_GENERIC_16, "AXIS_GENERIC_16"));

	for (std::map<int, const char*>::iterator iter = names.begin(); iter != names.end(); ++iter)
	{
		float val = AMotionEvent_getAxisValue(motionEvent, iter->first, 0);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "%d %s=%f", iter->first, iter->second, val);
	}
}

static void passOnKeyDown(int deviceId, int keyCode)
{
	//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "passOnKeyDown: deviceId=%d keyCode=%s", deviceId, MappingParser::debugGetButtonName(keyCode).c_str());
	
	if (!isPressed(keyCode))
	{
		g_soundPool.play(g_soundBoom, 0.99, 0.99, 0, 0, 1);
		//g_audioTrack.play();
	}
	
	g_button[keyCode] = true;
	//debugOuyaKeyEvent();
}

static void passOnKeyUp(int deviceId, int keyCode)
{
	//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "passOnKeyUp: deviceId=%d keyCode=%s", deviceId, MappingParser::debugGetButtonName(keyCode).c_str());
	
	if (isPressed(keyCode))
	{
		g_soundPool.play(g_soundOuch, 0.99, 0.99, 0, 0, 1);
		//g_audioTrack.play();
	}
	
	g_button[keyCode] = false;
	//debugOuyaKeyEvent();
}

static void passOnGenericMotionEvent(int deviceId, int axis, float val)
{
	g_axis[axis] = val;
	//debugOuyaMotionEvent();
}

static bool dispatchGenericMotionEvent(AInputEvent* motionEvent)
{
	int deviceId = AInputEvent_getDeviceId(motionEvent);
	std::string deviceName = getDeviceName(deviceId);

	//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "%s : dispatchGenericMotionEvent deviceId=%d name=%s", g_model.c_str(), deviceId, deviceName.c_str());

	//debugMotionEvent(motionEvent);
	//debugOuyaMotionEvent();

	/*
	MappingParser::Device* device = g_parser.getDevice(g_model);
	if (device)
	{
		MappingParser::Controller* controller = g_parser.getController(device, deviceName);
		if (controller)
		{
			MappingParser::debugController(controller);
		}
	}
	*/

	std::vector<MappingParser::ButtonIsAxis*>* buttons =
		g_parser.getButtonIsAxis(g_model, deviceName);

	if (NULL != buttons)
	{
		//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Button Is Axis Count: %d", buttons->size());
		for (int i = 0; i < buttons->size(); ++i)
		{
			MappingParser::ButtonIsAxis* button = (*buttons)[i];
			if (button)
			{
				float axis = AMotionEvent_getAxisValue(motionEvent, button->mSourceAxis, 0);
				/*
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Button Is Axis source=%d,%s destination=%d,%s Expected=%f Value=%f",
					button->mSourceAxis, MappingParser::debugGetAxisName(button->mSourceAxis).c_str(),
					button->mDestinationKeyCode, MappingParser::debugGetButtonName(button->mDestinationKeyCode).c_str(),
					button->mActionDown, axis);
				*/
				if (axis == button->mActionDown)
				{
					if (!isPressed(button->mDestinationKeyCode))
					{
						//Log.i(TAG, "Injected ACTION_DOWN for " + button.mDestinationKeyCode);
						passOnKeyDown(deviceId, button->mDestinationKeyCode);
					}
				}
				else
				{
					if (isPressed(button->mDestinationKeyCode))
					{
						//Log.i(TAG, "Injected ACTION_UP for " + button.mDestinationKeyCode);
						passOnKeyUp(deviceId, button->mDestinationKeyCode);
					}
				}
			}
		}
	}

	std::vector<MappingParser::AxisRemap*>* axises =
		g_parser.getAxisRemap(g_model, deviceName);

	if (NULL != axises)
	{
		//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Axis Remap Count: %d", axises->size());
		for (int i = 0; i < axises->size(); ++i)
		{
			MappingParser::AxisRemap* axis = (*axises)[i];
			if (axis)
			{
				float val = AMotionEvent_getAxisValue(motionEvent, axis->mSourceAxis, 0);
				/*
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Remap %s to %s val=%f",
					MappingParser::debugGetAxisName(axis->mSourceAxis).c_str(),
					MappingParser::debugGetAxisName(axis->mDestinationAxis).c_str(),
					val);
				*/
				passOnGenericMotionEvent(deviceId, axis->mDestinationAxis, val);
			}
		}
	}

	/*
	debugMotionEvent(motionEvent);
	//debugOuyaMotionEvent(motionEvent);
	passDispatchGenericMotionEvent(motionEvent);
	*/

	return true;
}

static bool dispatchKeyEvent(AInputEvent* keyEvent)
{
	int deviceId = AInputEvent_getDeviceId(keyEvent);
	std::string deviceName = getDeviceName(deviceId);

	int32_t keyCode = AKeyEvent_getKeyCode(keyEvent);

	MappingParser::Button* button =
		g_parser.getButton(g_model, deviceName, keyCode);
	if (NULL == button) {
		return true;
	}

	int source = AInputEvent_getSource(keyEvent);
	if (std::find(button->mExcludeSource.begin(), button->mExcludeSource.end(), source) != button->mExcludeSource.end())
	{
		return true;
	}

	int action = AMotionEvent_getAction(keyEvent);
	if (action == AMOTION_EVENT_ACTION_UP)
	{
		//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "onKeyUp keyCode=%d", keyCode);
		passOnKeyUp(deviceId, button->mDestinationKeyCode);
	}
	else if (action == AMOTION_EVENT_ACTION_DOWN)
	{
		//__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "onKeyDown keyCode=%d", keyCode);
		passOnKeyDown(deviceId, button->mDestinationKeyCode);
	}
}

/**
 * Process the next input event.
 */
static int32_t engine_handle_input(struct android_app* app, AInputEvent* event)
{
    struct engine* engine = (struct engine*)app->userData;
    if (AInputEvent_getType(event) == AINPUT_EVENT_TYPE_MOTION)
	{
		/*
		g_axis[OuyaController::AXIS_LS_X()] = AMotionEvent_getAxisValue(event, OuyaController::AXIS_LS_X(), 0);
		g_axis[OuyaController::AXIS_LS_Y()] = AMotionEvent_getAxisValue(event, OuyaController::AXIS_LS_Y(), 0);
		g_axis[OuyaController::AXIS_RS_X()] = AMotionEvent_getAxisValue(event, OuyaController::AXIS_RS_X(), 0);
		g_axis[OuyaController::AXIS_RS_Y()] = AMotionEvent_getAxisValue(event, OuyaController::AXIS_RS_Y(), 0);
		g_axis[OuyaController::AXIS_L2()] = AMotionEvent_getAxisValue(event, OuyaController::AXIS_L2(), 0);
		g_axis[OuyaController::AXIS_R2()] = AMotionEvent_getAxisValue(event, OuyaController::AXIS_R2(), 0);
		*/

		dispatchGenericMotionEvent(event);

        return 1;
	}
	else if (AInputEvent_getType(event) == AINPUT_EVENT_TYPE_KEY)
	{
		dispatchKeyEvent(event);
		return 1;
	}
    return 0;
}

/**
 * Process the next main command.
 */
static void engine_handle_cmd(struct android_app* app, int32_t cmd) {
    struct engine* engine = (struct engine*)app->userData;
    switch (cmd) {
        case APP_CMD_SAVE_STATE:
            // The system has asked us to save our current state.  Do so.
            engine->app->savedState = malloc(sizeof(struct saved_state));
            *((struct saved_state*)engine->app->savedState) = engine->state;
            engine->app->savedStateSize = sizeof(struct saved_state);
            break;
        case APP_CMD_INIT_WINDOW:
            // The window is being shown, get it ready.
            if (engine->app->window != NULL) {
                engine_init_display(engine);
                engine_draw_frame(engine);
            }
            break;
        case APP_CMD_TERM_WINDOW:
            // The window is being hidden or closed, clean it up.
            engine_term_display(engine);
            break;
        case APP_CMD_GAINED_FOCUS:
            // When our app gains focus, we start monitoring the accelerometer.
            if (engine->accelerometerSensor != NULL) {
                ASensorEventQueue_enableSensor(engine->sensorEventQueue,
                        engine->accelerometerSensor);
                // We'd like to get 60 events per second (in us).
                ASensorEventQueue_setEventRate(engine->sensorEventQueue,
                        engine->accelerometerSensor, (1000L/60)*1000);
            }
            break;
        case APP_CMD_LOST_FOCUS:
            // When our app loses focus, we stop monitoring the accelerometer.
            // This is to avoid consuming battery while not being used.
            if (engine->accelerometerSensor != NULL) {
                ASensorEventQueue_disableSensor(engine->sensorEventQueue,
                        engine->accelerometerSensor);
            }
            engine_draw_frame(engine);
            break;
    }
}

/**
 * This is the main entry point of a native application that is using
 * android_native_app_glue.  It runs in its own thread, with its own
 * event loop for receiving input events and doing other things.
 */
void android_main(struct android_app* state) {
    struct engine engine;

    // Make sure glue isn't stripped.
    app_dummy();

    memset(&engine, 0, sizeof(engine));
    state->userData = &engine;
    state->onAppCmd = engine_handle_cmd;
    state->onInputEvent = engine_handle_input;
    engine.app = state;

    // Prepare to monitor accelerometer
    engine.sensorManager = ASensorManager_getInstance();
    engine.accelerometerSensor = ASensorManager_getDefaultSensor(engine.sensorManager,
            ASENSOR_TYPE_ACCELEROMETER);
    engine.sensorEventQueue = ASensorManager_createEventQueue(engine.sensorManager,
            state->looper, LOOPER_ID_USER, NULL, NULL);

    if (state->savedState != NULL) {
        // We are starting with a previous saved state; restore from it.
        engine.state = *(struct saved_state*)state->savedState;
    }

    // loop waiting for stuff to do.

    while (1) {
        // Read all pending events.
        int ident;
        int events;
        struct android_poll_source* source;

        while ((ident=ALooper_pollAll(0, NULL, &events,
                (void**)&source)) >= 0) {

            // Process this event.
            if (source != NULL) {
                source->process(state, source);
            }

            // If a sensor has data, process it now.
            if (ident == LOOPER_ID_USER) {
                if (engine.accelerometerSensor != NULL) {
                    ASensorEvent event;
                    while (ASensorEventQueue_getEvents(engine.sensorEventQueue,
                            &event, 1) > 0) {
                        LOGI("accelerometer: x=%f y=%f z=%f",
                                event.acceleration.x, event.acceleration.y,
                                event.acceleration.z);
                    }
                }
            }

            // Check if we are exiting.
            if (state->destroyRequested != 0) {
                engine_term_display(&engine);
                return;
            }
        }

		engine_draw_frame(&engine);
    }
}
//END_INCLUDE(all)
