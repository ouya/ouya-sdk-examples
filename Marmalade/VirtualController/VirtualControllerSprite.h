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

	void Render();

private:
	void Render(CIw2DImage* image);

};

#endif