#ifndef __VIRTUAL_CONTROLLER_SPRITE__
#define __VIRTUAL_CONTROLLER_SPRITE__

#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"

#include <ctime>
#include <map>
#include <string>
#include <vector>

// Scale axis graphics but this amount
#define AXIS_SCALER 4.0f;

// The input deadzone
#define DEAD_ZONE 0.25f;

class VirtualControllerSprite
{
public:	

    // The controller index
    int PlayerIndex;

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
	CIw2DImage* Menu;

	// axis values
	std::map<int, float> m_axisValues;

	// pressed state
	std::vector<int> m_pressed;

	// Name of the device
	std::string DeviceName;

	VirtualControllerSprite();

    // Set the texture references        
    void Initialize(
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
		CIw2DImage* buttonMenu);
	
	// Check for input
	void HandleInput();

	// Render the controller
	void Render();

private:

	std::clock_t _timerMenuDetected;

	std::clock_t _timerGetName;

	float GetAxis(int axis);
	bool GetButton(int button);
	bool GetButtonDown(int button);
	bool GetButtonUp(int button);

	// Render an image with error checking
	void Render(CIw2DImage* image);

	// Render with an offset
	void Render(CIw2DImage* image, const CIwFVec2& offset);

};

#endif