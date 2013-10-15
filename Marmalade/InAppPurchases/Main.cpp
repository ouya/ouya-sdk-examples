#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "s3e.h"
#include "IwDebug.h"

#include "Application.h"

#include "ODK.h"
//#include "PluginOuya.h"


static int32 ButtonEventHandler(void* _systemData, void* userData);
static int32 KeyEventHandler(void* _systemData, void* userData);
static int32 MotionEventHandler(void* _systemData, void* userData);
static int32 TouchEventHandler(void* _systemData, void* userData);
static int32 TouchMotionEventHandler(void* _systemData, void* userData);

const char* g_version = "Version: 011";


void registerInput()
{
	// Register pointer event handler
    s3ePointerRegister(S3E_POINTER_BUTTON_EVENT, &ButtonEventHandler, NULL);

    // Register keyboard event handler
    s3eKeyboardRegister(S3E_KEYBOARD_KEY_EVENT, &KeyEventHandler, NULL);

	// Register motion event handler
	s3ePointerRegister(S3E_POINTER_MOTION_EVENT, &MotionEventHandler, NULL);

	// Register touch event handler
	s3ePointerRegister(S3E_POINTER_TOUCH_EVENT, &TouchEventHandler, NULL);

	// Register touch motion event handler
	s3ePointerRegister(S3E_POINTER_TOUCH_MOTION_EVENT, &TouchMotionEventHandler, NULL);


}

void render()
{
	IwGxClear();

	IwGxPrintString(200, 200, g_version);

	Application::m_ui.InitUI();

	Application::m_ui.Render();

	IwGxFlush();
	IwGxSwapBuffers();
}

int main()
{
	if (!ODKAvailable())
	{
		IwTrace(DEFAULT, ("Not running on OUYA, exit!"));
		return 0;
	}

	//Application::m_app = app;

	//Application::m_pluginOuya.AsyncSetDeveloperId("310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");

	IwGxInit();
	IwGxSetColClear(0, 0, 0xff, 0xff);
	IwResManagerInit();
	Iw2DInit();
	registerInput();

    const int textWidth = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_WIDTH);
    const int textHeight = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_HEIGHT);
    const int width = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
    const int height = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

	for (int index = 0; index < 10; ++index)
	{
		IwTrace(DEFAULT, ("Entering Main loop..."));
	}

	while (!s3eDeviceCheckQuitRequest())
	{
		//IwTrace(DEFAULT, ("Main loop while (!s3eDeviceCheckQuitRequest()"));

		Application::m_ui.HandleInput();
		render();

		// Yield until unyield is called or a quit request is recieved
		s3eDeviceYield(0);
	}

	Application::m_ui.Destroy();

	Iw2DTerminate();
	IwResManagerTerminate();
	IwGxTerminate();
	return 0;
}

/*
 * Key event handler:
 * systemData contains a structure of type s3eKeyboardEvent which contains
 * information about the pressed key.
 * Note the generic makeup of the callback function, which always supplies
 * the parameters:
 * systemData - callback-specific system supplied data.
 * userData - data supplied by user at callback registration (in this case
 * NULL) and returns int32 as success staus.
 */
static int32 ButtonEventHandler(void* systemData, void* userData)
{
	//sprintf(g_StatusString, "ButtonEventHandler");

    // Wake up and take control from the OS
    s3eDeviceUnYield();

    s3ePointerUpdate();

    // Read in the type of key that has been pressed
    s3eKeyboardEvent* pkeyPressed = (s3eKeyboardEvent*)systemData;
    s3eKey keyPressed = (*pkeyPressed).m_Key;

	char buffer[256];
	sprintf(buffer, "ButtonEventHandler: Key event detected. key=%d", keyPressed);
	IwTrace(ODK, (buffer));

	Application::m_ui.HandleInput();
	render();

    return 0;
}

static int32 KeyEventHandler(void* systemData, void* userData)
{
	//sprintf(g_StatusString, "KeyEventHandler");

    // Wake up and take control from the OS
    s3eDeviceUnYield();

    s3ePointerUpdate();

    // Read in the type of key that has been pressed
    s3eKeyboardEvent* pkeyPressed = (s3eKeyboardEvent*)systemData;
    s3eKey keyPressed = (*pkeyPressed).m_Key;

	/*
	char buffer[256];
	sprintf(buffer, "KeyEventHandler: Key event detected. key=%d pressed=%s", keyPressed, (*pkeyPressed).m_Pressed ? "true" : "false");	
	IwTrace(DEFAULT, (buffer));
	*/

	Application::m_ui.HandleInput();

	Application::m_ui.HandleInput();
	render();

    return 0;
}

static int32 MotionEventHandler(void* systemData, void* userData)
{
	//sprintf(g_StatusString, "MotionEventHandler");

	// Wake up and take control from the OS
    s3eDeviceUnYield();

    s3ePointerUpdate();

    // Read in the type of key that has been pressed
    s3eKeyboardEvent* pkeyPressed = (s3eKeyboardEvent*)systemData;
    s3eKey keyPressed = (*pkeyPressed).m_Key;

	char buffer[256];
	sprintf(buffer, "MotionEventHandler: Key event detected. key=%d", keyPressed);
	IwTrace(DEFAULT, (buffer));

	Application::m_ui.HandleInput();
	render();

    return 0;
}

static int32 TouchEventHandler(void* systemData, void* userData)
{
	//sprintf(g_StatusString, "TouchEventHandler");

	// Wake up and take control from the OS
    s3eDeviceUnYield();

    s3ePointerUpdate();

    // Read in the type of key that has been pressed
    s3eKeyboardEvent* pkeyPressed = (s3eKeyboardEvent*)systemData;
    s3eKey keyPressed = (*pkeyPressed).m_Key;

	char buffer[256];
	sprintf(buffer, "TouchEventHandler: Key event detected. key=%d", keyPressed);
	IwTrace(DEFAULT, (buffer));

	Application::m_ui.HandleInput();
	render();

    return 0;
}

static int32 TouchMotionEventHandler(void* systemData, void* userData)
{
	//sprintf(g_StatusString, "TouchMotionEventHandler");

	// Wake up and take control from the OS
    s3eDeviceUnYield();

    s3ePointerUpdate();

    // Read in the type of key that has been pressed
    s3eKeyboardEvent* pkeyPressed = (s3eKeyboardEvent*)systemData;
    s3eKey keyPressed = (*pkeyPressed).m_Key;

	char buffer[256];
	sprintf(buffer, "TouchMotionEventHandler: Key event detected. key=%d", keyPressed);
	IwTrace(DEFAULT, (buffer));

	Application::m_ui.HandleInput();
	render();

    return 0;
}