#include "VirtualControllerSprite.h"

VirtualControllerSprite::VirtualControllerSprite()
{
	DeviceName = "Unknown";

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

	PressedButtonA = false;
	PressedButtonO = false;
	PressedButtonU = false;
	PressedButtonY = false;
	PressedDpadDown = false;
	PressedDpadLeft = false;
	PressedDpadRight = false;
	PressedDpadUp = false;
	PressedLeftBumper = false;
	PressedLeftTrigger = false;
	PressedLeftStick = false;
	PressedRightBumper = false;
	PressedRightTrigger = false;
	PressedRightStick = false;
}

void VirtualControllerSprite::Initialize(
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

void VirtualControllerSprite::Render()
{
	Render(Controller);

	if (PressedButtonA)
	{
		Render(ButtonA);
	}

	if (PressedButtonO)
	{
		Render(ButtonO);
	}

    if (PressedButtonU)
	{
		Render(ButtonU);
	}

	if (PressedButtonY)
	{
		Render(ButtonY);
	}

	if (PressedDpadDown)
	{
		Render(DpadDown);
	}

    if (PressedDpadLeft)
	{
		Render(DpadLeft);
	}

	if (PressedDpadRight)
	{
		Render(DpadRight);
	}

    if (PressedDpadUp)
	{
		Render(DpadUp);
	}	
	
	if (PressedLeftBumper)
	{
		Render(LeftBumper);
	}

	if (PressedLeftTrigger)
	{
		Render(LeftTrigger);
	}

    if (PressedLeftStick)
	{
		Render(LeftStickActive);
	}
	else
	{
		Render(LeftStickInactive);
	}

	if (PressedRightBumper)
	{
		Render(RightBumper);
	}

	if (PressedRightTrigger)
	{
		Render(RightTrigger);
	}

	if (PressedRightStick)
	{
		Render(RightStickActive);
	}
	else
	{
		Render(RightStickInactive);
	}    

	if (DeviceName)
	{
		IwGxPrintString(Position.x + 150, Position.y + 315, DeviceName);
	}
}

void VirtualControllerSprite::Render(CIw2DImage* image)
{
	if (image)
	{
		Iw2DDrawImage(image, Position, CIwFVec2(image->GetWidth(), image->GetHeight()));
	}
}