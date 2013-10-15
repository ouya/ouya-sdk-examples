/*
 * windows-specific implementation of the ODK extension.
 * Add any platform-specific functionality here.
 */
/*
 * NOTE: This file was originally written by the extension builder, but will not
 * be overwritten (unless --force is specified) and is intended to be modified.
 */
#include "ODK_internal.h"

s3eResult ODKInit_platform()
{
    // Add any platform-specific initialisation code here
    return S3E_RESULT_SUCCESS;
}

void ODKTerminate_platform()
{
    // Add any platform-specific termination code here
}

void OuyaController_startOfFrame_platform()
{
}

bool OuyaController_selectControllerByPlayer_platform(int playerNum)
{
    return false;
}

bool OuyaController_selectControllerByDeviceID_platform(int deviceID)
{
    return false;
}

int OuyaController_getAxisValue_platform(int axis)
{
    return 0;
}

bool OuyaController_getButton_platform(int button)
{
    return false;
}

bool OuyaController_buttonPressedThisFrame_platform(int button)
{
    return false;
}

bool OuyaController_buttonReleasedThisFrame_platform(int button)
{
    return false;
}

bool OuyaController_buttonChangedThisFrame_platform(int button)
{
    return false;
}

int OuyaController_getPlayerNum_platform()
{
    return 0;
}
