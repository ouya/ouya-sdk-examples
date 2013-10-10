#ifndef __VIRTUAL_CONTROLLER_SPRITE__
#define __VIRTUAL_CONTROLLER_SPRITE__

#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"

// Scale axis graphics but this amount
#define AXIS_SCALER 4.0f;

// The input deadzone
#define DEAD_ZONE 0.25f;

class VirtualControllerSprite
{
public:	

    // The controller index
    //PlayerIndex Index = PlayerIndex.One;

    // The position of the sprite
    CIwFVec2 Position;

	// Images for drawing
    CIw2DImage* ButtonA;
    CIw2DImage* ButtonO;
    CIw2DImage* ButtonU;
    CIw2DImage* ButtonY;
    CIw2DImage* Controller;
    CIw2DImage* DpadDown;
    CIw2DImage* DpadLeft;
    CIw2DImage* DpadRight;
    CIw2DImage* DpadUp;
    CIw2DImage* LeftBumper;
    CIw2DImage* LeftTrigger;
    CIw2DImage* LeftStickActive;
    CIw2DImage* LeftStickInactive;
    CIw2DImage* RightBumper;
    CIw2DImage* RightTrigger;
    CIw2DImage* RightStickActive;
    CIw2DImage* RightStickInactive;

	// pressed state
	bool PressedButtonA;
    bool PressedButtonO;
    bool PressedButtonU;
    bool PressedButtonY;
    bool PressedDpadDown;
    bool PressedDpadLeft;
    bool PressedDpadRight;
    bool PressedDpadUp;
    bool PressedLeftBumper;
    bool PressedLeftTrigger;
    bool PressedLeftStick;
    bool PressedRightBumper;
    bool PressedRightTrigger;
    bool PressedRightStick;

	// Name of the device
	char* DeviceName;

	VirtualControllerSprite();

    // Set the texture references        
    void Initialize(
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
        CIw2DImage* buttonY);

	// Render the controller
	void Render();

private:

	// Render an image with error checking
	void Render(CIw2DImage* image);

};

#endif