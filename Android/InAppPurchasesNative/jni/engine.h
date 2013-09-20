#ifndef __ENGINE_H
#define __ENGINE_H

#include "TextButton.h"
#include "PluginOuya.h"

#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>

#define APP_NAME "inapppurchasesnative"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

class Engine
{
public:
	Engine(NvEGLUtil& egl, struct android_app* app, PluginOuya* pluginOuya);
	~Engine();

	bool isGameplayMode() { return mGameplayMode; }

	void updateFrame(bool interactible, long deltaTime);

protected:

	PluginOuya* m_pluginOuya;

	void setGameplayMode(bool paused);

	void advanceTime(long time) { mTimeVal += time * 0.001; }

	bool isForcedRenderPending() { return mForceRender > 0; }
	void requestForceRender() { mForceRender = 4; }

	bool checkWindowResized();

	bool renderFrame(bool allocateIfNeeded);

	static void handleCmdThunk(struct android_app* app, int32_t cmd);
	static int32_t handleInputThunk(struct android_app* app, AInputEvent* event);

	bool initUI();

	bool resizeIfNeeded();

	int handleInput(AInputEvent* event);
	void handleCommand(int cmd);

    struct android_app* mApp;

	NvEGLUtil& mEgl;

	bool mResizePending;

	bool mGameplayMode;

	int mForceRender;

    double mTimeVal;

	bool m_uiInitialized;

	TextButton m_uiRequestGamerUUID;
	TextButton m_uiRequestProducts;
	TextButton m_uiRequestPurchase;
	TextButton m_uiRequestReceipts;
	TextButton m_uiPause;

	TextButton* m_selectedButton;
};


#endif // __ENGINE_H
