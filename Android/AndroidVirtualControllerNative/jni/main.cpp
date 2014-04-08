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
#include <jni.h>
#include <errno.h>

#include <EGL/egl.h>
#include <GLES/gl.h>

#include <android/bitmap.h>
#include <android/log.h>
#include <android/sensor.h>
#include "../native_app_glue/android_native_app_glue.h"

#include "Activity.h"
#include "AssetManager.h"
#include "Bitmap.h"
#include "BitmapFactory.h"
#include "Context.h"
#include "GLUtils.h"
#include "InputStream.h"
#include "String.h"

#define LOG_TAG "VirtualControllerNative"

#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO, LOG_TAG, __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN, LOG_TAG, __VA_ARGS__))
#define LOGE(...) ((void)__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, __VA_ARGS__))

using namespace android_app_Activity;
using namespace android_content_Context;
using namespace android_content_res_AssetManager;
using namespace android_opengl_GLUtils;
using namespace android_graphics_Bitmap;
using namespace android_graphics_BitmapFactory;
using namespace java_io_InputStream;
using namespace java_lang_String;

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

    int animating;
    EGLDisplay display;
    EGLSurface surface;
    EGLContext context;
    int32_t width;
    int32_t height;
    struct saved_state state;
};

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

	if (JNI_ERR == Activity::InitJNI(env))
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

	if (JNI_ERR == Context::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == GLUtils::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == InputStream::InitJNI(env))
	{
		return JNI_ERR;
	}

	if (JNI_ERR == String::InitJNI(env))
	{
		return JNI_ERR;
	}

	return JNI_VERSION_1_6;
}

const int textureCount = 17;
static GLuint textures[textureCount];

void LoadTexture(JNIEnv* env, AssetManager& assetManager, const BitmapFactory::Options& options, const char* texturePath, int textureId)
{
	String strController = String(texturePath);
	InputStream stream = assetManager.open(strController);
	Bitmap bitmap = BitmapFactory::decodeStream(stream, 0, options);
	stream.close();
	int width = bitmap.getWidth();
	int height = bitmap.getHeight();
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded %s bitmap width=%d height=%d", texturePath, width, height);

	AndroidBitmapInfo info = AndroidBitmapInfo();
	AndroidBitmap_getInfo(env, bitmap.GetInstance(), &info);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.width=%d", info.width);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.height=%d", info.height);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.stride=%d", info.stride);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.format=%d", info.format);
	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "info.flags=%d", info.flags);

	if (info.format != ANDROID_BITMAP_FORMAT_RGBA_8888) {
		__android_log_print(ANDROID_LOG_ERROR, LOG_TAG, "Bitmap format is not RGBA_8888! %s", texturePath);
		return;
	}

	glBindTexture(GL_TEXTURE_2D, textures[textureId]);
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "glBindTexture=%d", textures[textureId]);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

	android_opengl_GLUtils::GLUtils::texImage2D(GL_TEXTURE_2D, 0, bitmap.GetInstance(), 0);

	//bitmap.recycle();
}

void Test(JavaVM* vm, JNIEnv* env, jobject objActivity)
{
	vm->AttachCurrentThread(&env, NULL);

	BitmapFactory::Options options = BitmapFactory::Options();
	options.set_inScaled(false); // No pre-scaling
	bool result = options.get_inScaled();
	Activity activity = Activity(objActivity);
	Context context = activity.getApplicationContext();
	AssetManager assetManager = context.getAssets();

	String path = String("");
	std::vector<std::string> files = assetManager.list(path);
	for (int index = 0; index < files.size(); ++index)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, files[index].c_str());
	}

	glGenTextures(textureCount, &textures[0]);

	int textureId = 0;
	LoadTexture(env, assetManager, options, "controller.png", textureId);
	LoadTexture(env, assetManager, options, "a.png", ++textureId);
	LoadTexture(env, assetManager, options, "dpad_down.png", ++textureId);
	LoadTexture(env, assetManager, options, "dpad_left.png", ++textureId);
	LoadTexture(env, assetManager, options, "dpad_right.png", ++textureId);
	LoadTexture(env, assetManager, options, "dpad_up.png", ++textureId);
	LoadTexture(env, assetManager, options, "lb.png", ++textureId);
	LoadTexture(env, assetManager, options, "lt.png", ++textureId);
	LoadTexture(env, assetManager, options, "l_stick.png", ++textureId);
	LoadTexture(env, assetManager, options, "o.png", ++textureId);
	LoadTexture(env, assetManager, options, "rb.png", ++textureId);
	LoadTexture(env, assetManager, options, "rt.png", ++textureId);
	LoadTexture(env, assetManager, options, "r_stick.png", ++textureId);
	LoadTexture(env, assetManager, options, "thumbl.png", ++textureId);
	LoadTexture(env, assetManager, options, "thumbr.png", ++textureId);
	LoadTexture(env, assetManager, options, "u.png", ++textureId);
	LoadTexture(env, assetManager, options, "y.png", ++textureId);

	__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded %d textures", textureId + 1);
}

static unsigned int vbo[3];
static float positions[12] = { 0.1, -0.1, 0.0, 0.1, 0.1, 0.0, -0.1, -0.1, 0.0, -0.1, 0.1, 0.0 };
static float textureCoords[8] = { 0.0, 0.0, 1.0, 0.0, 0.0, 1.0, 1.0, 1.0 };
static short indices[4] = { 0, 1, 2, 3 };

/**
 * Initialize an EGL context for the current display.
 */
static int engine_init_display(struct engine* engine) {
    // initialize OpenGL ES and EGL
	if (JNI_ERR == RegisterClasses(engine->app->activity))
	{
		return JNI_ERR;
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

	Test(engine->app->activity->vm, engine->app->activity->env, engine->app->activity->clazz);

	glGenBuffers(3, vbo);
	glBindBuffer(GL_ARRAY_BUFFER, vbo[0]);
	glBufferData(GL_ARRAY_BUFFER, 4 * 12, positions, GL_STATIC_DRAW);

	glBindBuffer(GL_ARRAY_BUFFER, vbo[1]);
	glBufferData(GL_ARRAY_BUFFER, 4 * 8, textureCoords, GL_STATIC_DRAW);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, vbo[2]);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, 2 * 4, indices, GL_STATIC_DRAW);

    return 0;
}

static void DrawTexture(int textureId)
{
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
	
	glRotatef(90, 0, 0, 1);
	glScalef(4, 4, 4);

	glEnableClientState(GL_VERTEX_ARRAY);
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);

	// draw the model
	glBindBuffer(GL_ARRAY_BUFFER, vbo[0]);
	glVertexPointer(3, GL_FLOAT, 0, 0);
	glBindBuffer(GL_ARRAY_BUFFER, vbo[1]);
	glTexCoordPointer(2, GL_FLOAT, 0, 0);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	DrawTexture(0);

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
    engine->animating = 0;
    engine->display = EGL_NO_DISPLAY;
    engine->context = EGL_NO_CONTEXT;
    engine->surface = EGL_NO_SURFACE;
}

/**
 * Process the next input event.
 */
static int32_t engine_handle_input(struct android_app* app, AInputEvent* event) {
    struct engine* engine = (struct engine*)app->userData;
    if (AInputEvent_getType(event) == AINPUT_EVENT_TYPE_MOTION) {
        engine->animating = 1;
        engine->state.x = AMotionEvent_getX(event, 0);
        engine->state.y = AMotionEvent_getY(event, 0);
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
            // Also stop animating.
            engine->animating = 0;
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

        // If not animating, we will block forever waiting for events.
        // If animating, we loop until all events are read, then continue
        // to draw the next frame of animation.
        while ((ident=ALooper_pollAll(engine.animating ? 0 : -1, NULL, &events,
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

        if (engine.animating) {
            // Done with events; draw next animation frame.
            engine.state.angle += .01f;
            if (engine.state.angle > 1) {
                engine.state.angle = 0;
            }

            // Drawing is throttled to the screen update rate, so there
            // is no need to do timing here.
            engine_draw_frame(&engine);
        }
    }
}
//END_INCLUDE(all)
