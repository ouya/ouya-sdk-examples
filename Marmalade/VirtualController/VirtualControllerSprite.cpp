#include "VirtualControllerSprite.h"

#include "ODK.h"

#include <math.h>

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
	CIw2DImage* buttonY)
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
}

bool VirtualControllerSprite::HandleAxis(int axis)
{
	float val = OuyaController_getAxisValue(axis) / 256.0f;
	m_axisValues[axis] = val;
	return val;
}

float VirtualControllerSprite::GetAxis(int axis)
{
	std::map<int, float>::iterator iter = m_axisValues.find(axis);
	if (iter == m_axisValues.end())
	{
		return 0;
	}
	else
	{
		return m_axisValues[axis];
	}
}

bool VirtualControllerSprite::HandleButtonPressed(int button)
{
	if (OuyaController_getButton(button))
	{
		// not found
		if (std::find(m_pressed.begin(), m_pressed.end(), button) == m_pressed.end())
		{
			m_pressed.push_back(button);
		}
		return true;
	}
	else
	{
		std::vector<int>::iterator lookup = std::find(m_pressed.begin(), m_pressed.end(), button);
		// found
		if (lookup != m_pressed.end())
		{
			m_pressed.erase(lookup);
		}
		
		return false;
	}
}

bool VirtualControllerSprite::ButtonPressed(int button)
{
	// not found
	if (std::find(m_pressed.begin(), m_pressed.end(), button) == m_pressed.end())
	{
		return false;
	}
	else
	{
		return true;
	}
}

void VirtualControllerSprite::HandleInput()
{
	HandleAxis(OuyaController_AXIS_LS_X);
	HandleAxis(OuyaController_AXIS_LS_Y);
	HandleAxis(OuyaController_AXIS_RS_X);
	HandleAxis(OuyaController_AXIS_RS_Y);
	HandleAxis(OuyaController_AXIS_L2);
	HandleAxis(OuyaController_AXIS_R2);
	
	HandleButtonPressed(OuyaController_BUTTON_DPAD_UP);
	HandleButtonPressed(OuyaController_BUTTON_DPAD_DOWN);
	HandleButtonPressed(OuyaController_BUTTON_DPAD_LEFT);
	HandleButtonPressed(OuyaController_BUTTON_DPAD_RIGHT);
	
	//HandleButtonPressed(OuyaController_BUTTON_MENU);

	HandleButtonPressed(OuyaController_BUTTON_O);
	HandleButtonPressed(OuyaController_BUTTON_A);
	HandleButtonPressed(OuyaController_BUTTON_U);
	HandleButtonPressed(OuyaController_BUTTON_Y);
	
	HandleButtonPressed(OuyaController_BUTTON_L1);
	HandleButtonPressed(OuyaController_BUTTON_R1);
	HandleButtonPressed(OuyaController_BUTTON_L2);
	HandleButtonPressed(OuyaController_BUTTON_R2);
	HandleButtonPressed(OuyaController_BUTTON_R3);
	HandleButtonPressed(OuyaController_BUTTON_L3);
}

void VirtualControllerSprite::Render()
{
	Render(Controller);

	if (ButtonPressed(OuyaController_BUTTON_A))
	{
		Render(ButtonA);
	}

	if (ButtonPressed(OuyaController_BUTTON_O))
	{
		Render(ButtonO);
	}

    if (ButtonPressed(OuyaController_BUTTON_U))
	{
		Render(ButtonU);
	}

	if (ButtonPressed(OuyaController_BUTTON_Y))
	{
		Render(ButtonY);
	}

	if (ButtonPressed(OuyaController_BUTTON_DPAD_DOWN))
	{
		Render(DpadDown);
	}

    if (ButtonPressed(OuyaController_BUTTON_DPAD_LEFT))
	{
		Render(DpadLeft);
	}

	if (ButtonPressed(OuyaController_BUTTON_DPAD_RIGHT))
	{
		Render(DpadRight);
	}

    if (ButtonPressed(OuyaController_BUTTON_DPAD_UP))
	{
		Render(DpadUp);
	}	
	
	if (ButtonPressed(OuyaController_BUTTON_L1))
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
	pos.x = AXIS_SCALER * x * valCos - y * valSin;
	pos.y = AXIS_SCALER * x * valSin + y * valCos;

    if (ButtonPressed(OuyaController_BUTTON_L3))
	{
		Render(LeftStickActive, pos);
	}
	else
	{
		Render(LeftStickInactive, pos);
	}

	if (ButtonPressed(OuyaController_BUTTON_R1))
	{
		Render(RightBumper);
	}

	if (abs(GetAxis(OuyaController_AXIS_R2)) > DEADZONE)
	{
		Render(RightTrigger);
	}

	x = GetAxis(OuyaController_AXIS_RS_X);
	y = GetAxis(OuyaController_AXIS_RS_Y);

	pos.x = AXIS_SCALER * x * valCos - y * valSin;
	pos.y = AXIS_SCALER * x * valSin + y * valCos;

	if (ButtonPressed(OuyaController_BUTTON_R3))
	{
		Render(RightStickActive, pos);
	}
	else
	{
		Render(RightStickInactive, pos);
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