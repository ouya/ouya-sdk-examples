#include "VirtualControllerSprite.h"

#include "ODK.h"

#include <math.h>

#define LOG_TAG "VirtualControllerSprite.cpp"

#define AXIS_SCALER 4.0f

#define DEADZONE 0.25f

VirtualControllerSprite::VirtualControllerSprite()
{
	DeviceName = "Unknown";

	PlayerIndex = -1;

    ButtonA = NULL;
    ButtonO = NULL;
    ButtonU = NULL;
    ButtonY = NULL;
    Controller = NULL;
    DpadDown = NULL;
    DpadLeft = NULL;
    DpadRight = NULL;
    DpadUp = NULL;
    LeftBumper = NULL;
    LeftTrigger = NULL;
    LeftStickActive = NULL;
    LeftStickInactive = NULL;
    RightBumper = NULL;
    RightTrigger = NULL;
    RightStickActive = NULL;
    RightStickInactive = NULL;
	Menu = NULL;
}

void VirtualControllerSprite::Initialize(
	int playerIndex,
	CIw2DImage* buttonA,
	CIw2DImage* controller,
	CIw2DImage* dpadDown,
	CIw2DImage* dpadLeft,
	CIw2DImage* dpadRight,
	CIw2DImage* dpadUp,
	CIw2DImage* leftBumper,
	CIw2DImage* leftTrigger,
	CIw2DImage* leftStickInactive,
	CIw2DImage* buttonO,
	CIw2DImage* rightBumper,
	CIw2DImage* rightTrigger,
	CIw2DImage* rightStickInactive,
	CIw2DImage* leftStickActive,
	CIw2DImage* rightStickActive,
	CIw2DImage* buttonU,
	CIw2DImage* buttonY,
	CIw2DImage* buttonMenu)
{
	DeviceName = "Unknown";

	PlayerIndex = playerIndex;

    ButtonA = buttonA;
    ButtonO = buttonO;
    ButtonU = buttonU;
    ButtonY = buttonY;
    Controller = controller;
    DpadDown = dpadDown;
    DpadLeft = dpadLeft;
    DpadRight = dpadRight;
    DpadUp = dpadUp;
    LeftBumper = leftBumper;
    LeftTrigger = leftTrigger;
    LeftStickActive = leftStickActive;
    LeftStickInactive = leftStickInactive;
    RightBumper = rightBumper;
    RightTrigger = rightTrigger;
    RightStickActive = rightStickActive;
    RightStickInactive = rightStickInactive;
	Menu = buttonMenu;
}

float VirtualControllerSprite::GetAxis(int axis)
{
	int val = OuyaPlugin_getAxis(PlayerIndex, axis);
	float result = *(reinterpret_cast<float*>(&val));
	//IwTrace(LOG_TAG, ("axis=%d val=%f", axis, result));
	return result;
}

bool VirtualControllerSprite::GetButton(int button)
{
	bool val = OuyaPlugin_isPressed(PlayerIndex, button);
	//IwTrace(LOG_TAG, ("button=%d val=%s", button, val ? "true" : "false"));
	return val;
}

bool VirtualControllerSprite::GetButtonDown(int button)
{
	bool val = OuyaPlugin_isPressedDown(PlayerIndex, button);
	//IwTrace(LOG_TAG, ("button=%d val=%s", button, val ? "true" : "false"));
	return val;
}

bool VirtualControllerSprite::GetButtonUp(int button)
{
	bool val = OuyaPlugin_isPressedUp(PlayerIndex, button);
	//IwTrace(LOG_TAG, ("button=%d val=%s", button, val ? "true" : "false"));
	return val;
}

void VirtualControllerSprite::HandleInput()
{
	if (GetButtonUp(OuyaController_BUTTON_MENU))
	{
		_timerMenuDetected = std::clock();
	}

	/*
	if (PlayerIndex == 0)
	{
		IwTrace(LOG_TAG, ("duration=%d", (std::clock() - _timerMenuDetected)));
	}
	*/
}

void VirtualControllerSprite::Render()
{
	Render(Controller);

	if (GetButton(OuyaController_BUTTON_A))
	{
		Render(ButtonA);
	}

	if (GetButton(OuyaController_BUTTON_O))
	{
		Render(ButtonO);
	}

    if (GetButton(OuyaController_BUTTON_U))
	{
		Render(ButtonU);
	}

	if (GetButton(OuyaController_BUTTON_Y))
	{
		Render(ButtonY);
	}

	if (GetButton(OuyaController_BUTTON_DPAD_DOWN))
	{
		Render(DpadDown);
	}

    if (GetButton(OuyaController_BUTTON_DPAD_LEFT))
	{
		Render(DpadLeft);
	}

	if (GetButton(OuyaController_BUTTON_DPAD_RIGHT))
	{
		Render(DpadRight);
	}

    if (GetButton(OuyaController_BUTTON_DPAD_UP))
	{
		Render(DpadUp);
	}	
	
	if (GetButton(OuyaController_BUTTON_L1))
	{
		Render(LeftBumper);
	}

	if (fabs(GetAxis(OuyaController_AXIS_L2)) > DEADZONE)
	{
		Render(LeftTrigger);
	}

	//rotate input by N degrees to match image
    float degrees = 135;
    float radians = degrees / 180.0f * 3.14f;
    float valCos = cosf(radians);
    float valSin = sinf(radians);

	float x = GetAxis(OuyaController_AXIS_LS_X);
	float y = GetAxis(OuyaController_AXIS_LS_Y);

	CIwFVec2 pos;
	pos.x = AXIS_SCALER * (x * valCos - y * valSin);
	pos.y = AXIS_SCALER * (x * valSin + y * valCos);

    if (GetButton(OuyaController_BUTTON_L3))
	{
		Render(LeftStickActive, pos);
	}
	else
	{
		Render(LeftStickInactive, pos);
	}

	if (GetButton(OuyaController_BUTTON_R1))
	{
		Render(RightBumper);
	}

	if (abs(GetAxis(OuyaController_AXIS_R2)) > DEADZONE)
	{
		Render(RightTrigger);
	}

	x = GetAxis(OuyaController_AXIS_RS_X);
	y = GetAxis(OuyaController_AXIS_RS_Y);

	pos.x = AXIS_SCALER * (x * valCos - y * valSin);
	pos.y = AXIS_SCALER * (x * valSin + y * valCos);

	if (GetButton(OuyaController_BUTTON_R3))
	{
		Render(RightStickActive, pos);
	}
	else
	{
		Render(RightStickInactive, pos);
	}

	if ((std::clock() - _timerMenuDetected) < CLOCKS_PER_SEC)
	{
		Render(Menu, pos);
	}

	if ((std::clock() - _timerGetName) > (10*CLOCKS_PER_SEC))
	{
		DeviceName = OuyaPlugin_getDeviceName(PlayerIndex);
	}

	if (DeviceName.c_str())
	{
		IwGxPrintString(Position.x + 150, Position.y + 315, DeviceName.c_str());
	}
}

void VirtualControllerSprite::Render(CIw2DImage* image)
{
	if (image)
	{
		Iw2DDrawImage(image, Position, CIwFVec2(image->GetWidth(), image->GetHeight()));
	}
}

void VirtualControllerSprite::Render(CIw2DImage* image, const CIwFVec2& offset)
{
	if (image)
	{
		Iw2DDrawImage(image, Position + offset, CIwFVec2(image->GetWidth(), image->GetHeight()));
	}
}