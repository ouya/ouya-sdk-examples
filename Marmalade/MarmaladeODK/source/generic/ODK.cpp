/*
Generic implementation of the ODK extension.
This file should perform any platform-indepedentent functionality
(e.g. error checking) before calling platform-dependent implementations.
*/

/*
 * NOTE: This file was originally written by the extension builder, but will not
 * be overwritten (unless --force is specified) and is intended to be modified.
 */


#include "ODK_internal.h"
s3eResult ODKInit()
{
    //Add any generic initialisation code here
    return ODKInit_platform();
}

void ODKTerminate()
{
    //Add any generic termination code here
    ODKTerminate_platform();
}

void OuyaController_startOfFrame()
{
	OuyaController_startOfFrame_platform();
}

bool OuyaController_selectControllerByPlayer(int playerNum)
{
	return OuyaController_selectControllerByPlayer_platform(playerNum);
}

bool OuyaController_selectControllerByDeviceID(int deviceID)
{
	return OuyaController_selectControllerByDeviceID_platform(deviceID);
}

int OuyaController_getAxisValue(int axis)
{
	return OuyaController_getAxisValue_platform(axis);
}

bool OuyaController_getButton(int button)
{
	return OuyaController_getButton_platform(button);
}

bool OuyaController_buttonPressedThisFrame(int button)
{
	return OuyaController_buttonPressedThisFrame_platform(button);
}

bool OuyaController_buttonReleasedThisFrame(int button)
{
	return OuyaController_buttonReleasedThisFrame_platform(button);
}

bool OuyaController_buttonChangedThisFrame(int button)
{
	return OuyaController_buttonChangedThisFrame_platform(button);
}

int OuyaController_getPlayerNum()
{
	return OuyaController_getPlayerNum_platform();
}
