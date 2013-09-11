#include "VirtualControllerSprite.h"

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