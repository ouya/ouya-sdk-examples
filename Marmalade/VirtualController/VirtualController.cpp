#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "s3e.h"
#include "VirtualControllerSprite.h"

//the controller instances
VirtualControllerSprite m_controllers[4];

//player status information
static char g_debugButtonEvent[256];
static char g_debugKeyEvent[256];
static char g_debugMotionEvent[256];
static char g_debugTouchEvent[256];
static char g_debugTouchMotionEvent[256];

static int32 ButtonEventHandler(void* _systemData, void* userData);
static int32 KeyEventHandler(void* _systemData, void* userData);
static int32 MotionEventHandler(void* _systemData, void* userData);
static int32 TouchEventHandler(void* _systemData, void* userData);
static int32 TouchMotionEventHandler(void* _systemData, void* userData);

void setupTextures()
{
	IwGetResManager()->LoadGroup("tiles.group");

	CIw2DImage* a = NULL;
	CIw2DImage* cutter = NULL;
	CIw2DImage* dpad_down = NULL;
	CIw2DImage* dpad_left = NULL;
	CIw2DImage* dpad_right = NULL;
	CIw2DImage* dpad_up = NULL;
	CIw2DImage* lb = NULL;
	CIw2DImage* lt = NULL;
	CIw2DImage* l_stick = NULL;
	CIw2DImage* o = NULL;
	CIw2DImage* rb = NULL;
	CIw2DImage* rt = NULL;
	CIw2DImage* r_stick = NULL;
	CIw2DImage* thumbl = NULL;
	CIw2DImage* thumbr = NULL;
	CIw2DImage* u = NULL;
	CIw2DImage* y = NULL;
	
	a = Iw2DCreateImageResource("a");
	cutter = Iw2DCreateImageResource("cutter");
	dpad_down = Iw2DCreateImageResource("dpad_down");
	dpad_left = Iw2DCreateImageResource("dpad_left");
	dpad_right = Iw2DCreateImageResource("dpad_right");
	dpad_up = Iw2DCreateImageResource("dpad_up");
	lb = Iw2DCreateImageResource("lb");
	lt = Iw2DCreateImageResource("lt");
	l_stick = Iw2DCreateImageResource("l_stick");
	o = Iw2DCreateImageResource("o");
	rb = Iw2DCreateImageResource("rb");
	rt = Iw2DCreateImageResource("rt");
	r_stick = Iw2DCreateImageResource("r_stick");
	thumbl = Iw2DCreateImageResource("thumbl");
	thumbr = Iw2DCreateImageResource("thumbr");
	u = Iw2DCreateImageResource("u");
	y = Iw2DCreateImageResource("y");

	m_controllers[0].Position = CIwFVec2(400, 150);
	m_controllers[1].Position = CIwFVec2(1000, 150);
	m_controllers[2].Position = CIwFVec2(400, 550);
	m_controllers[3].Position = CIwFVec2(1000, 550);

	for (int index = 0; index < 4; ++index)
	{
		m_controllers[index].Initialize(
			a,
			cutter,
			dpad_down,
			dpad_left,
			dpad_right,
			dpad_up,
			lb,
			lt,
			l_stick,
			o,
			rb,
			rt,
			r_stick,
			thumbl,
			thumbr,
			u,
			y);
	}
}

void destroyTextures()
{
	delete m_controllers[0].ButtonA;
	delete m_controllers[0].ButtonO;
	delete m_controllers[0].ButtonU;
	delete m_controllers[0].ButtonY;
	delete m_controllers[0].Controller;
	delete m_controllers[0].DpadDown;
	delete m_controllers[0].DpadLeft;
	delete m_controllers[0].DpadRight;
	delete m_controllers[0].DpadUp;
	delete m_controllers[0].LeftBumper;
	delete m_controllers[0].LeftTrigger;
	delete m_controllers[0].LeftStickActive;
	delete m_controllers[0].LeftStickInactive;
	delete m_controllers[0].RightBumper;
	delete m_controllers[0].RightTrigger;
	delete m_controllers[0].RightStickActive;
	delete m_controllers[0].RightStickInactive;
}

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
	for (int index = 0; index < 4; ++index)
	{
		m_controllers[index].Render();
	}

	IwGxPrintString(300, 50, g_debugButtonEvent);
	IwGxPrintString(300, 75, g_debugKeyEvent);
	IwGxPrintString(300, 100, g_debugMotionEvent);
	IwGxPrintString(300, 125, g_debugTouchEvent);
	IwGxPrintString(300, 150, g_debugTouchMotionEvent);

	IwGxFlush();
	IwGxSwapBuffers();
}

int main()
{
	IwGxInit();
	IwGxSetColClear(0, 0, 0xff, 0xff);
	IwResManagerInit();
	Iw2DInit();
	setupTextures();
	registerInput();

    const int textWidth = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_WIDTH);
    const int textHeight = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_HEIGHT);
    const int width = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
    const int height = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

	sprintf(g_debugButtonEvent, "ButtonEvent:");
	sprintf(g_debugKeyEvent, "KeyEvent:");
	sprintf(g_debugMotionEvent, "MotionEvent:");
	sprintf(g_debugTouchEvent, "TouchEvent:");
	sprintf(g_debugTouchMotionEvent, "TouchMotionEvent:");

	while (!s3eDeviceCheckQuitRequest())
	{
		render();

		// Yield until unyield is called or a quit request is recieved
        s3eDeviceYield(S3E_DEVICE_YIELD_FOREVER);
	}
	destroyTextures();
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

	sprintf(g_debugButtonEvent, "ButtonEventHandler: Key event detected. key=%d", keyPressed);

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

	sprintf(g_debugKeyEvent, "KeyEventHandler: Key event detected. key=%d pressed=%s", keyPressed, (*pkeyPressed).m_Pressed ? "true" : "false");

	if (keyPressed == 210)
	{
		m_controllers[0].PressedButtonA = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 208)
	{
		m_controllers[0].PressedButtonO = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 203)
	{
		m_controllers[0].PressedButtonU = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 202)
	{
		m_controllers[0].PressedButtonY = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 205)
	{
		m_controllers[0].PressedDpadDown = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 206)
	{
		m_controllers[0].PressedDpadLeft = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 207)
	{
		m_controllers[0].PressedDpadRight = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 204)
	{
		m_controllers[0].PressedDpadUp = (*pkeyPressed).m_Pressed;
	}

	else if (keyPressed == 74)
	{
		m_controllers[0].PressedLeftBumper = (*pkeyPressed).m_Pressed;
	}

	//m_controllers[0].PressedLeftTrigger;
	//m_controllers[0].PressedLeftStick;

	else if (keyPressed == 75)
	{
		m_controllers[0].PressedRightBumper = (*pkeyPressed).m_Pressed;
	}

	//m_controllers[0].PressedRightTrigger;
	//m_controllers[0].PressedRightStick;

	else if (keyPressed == 127)
	{
		// system buton
	}

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

	sprintf(g_debugMotionEvent, "MotionEventHandler: Key event detected. key=%d", keyPressed);

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

	sprintf(g_debugTouchEvent, "TouchEventHandler: Key event detected. key=%d", keyPressed);

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

	sprintf(g_debugTouchMotionEvent, "TouchMotionEventHandler: Key event detected. key=%d", keyPressed);

	render();

    return 0;
}