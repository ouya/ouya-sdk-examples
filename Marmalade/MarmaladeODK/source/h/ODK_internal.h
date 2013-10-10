/*
 * Internal header for the ODK extension.
 *
 * This file should be used for any common function definitions etc that need to
 * be shared between the platform-dependent and platform-indepdendent parts of
 * this extension.
 */

/*
 * NOTE: This file was originally written by the extension builder, but will not
 * be overwritten (unless --force is specified) and is intended to be modified.
 */


#ifndef ODK_INTERNAL_H
#define ODK_INTERNAL_H

#include "s3eTypes.h"
#include "ODK.h"
#include "ODK_autodefs.h"


/**
 * Initialise the extension.  This is called once then the extension is first
 * accessed by s3eregister.  If this function returns S3E_RESULT_ERROR the
 * extension will be reported as not-existing on the device.
 */
s3eResult ODKInit();

/**
 * Platform-specific initialisation, implemented on each platform
 */
s3eResult ODKInit_platform();

/**
 * Terminate the extension.  This is called once on shutdown, but only if the
 * extension was loader and Init() was successful.
 */
void ODKTerminate();

/**
 * Platform-specific termination, implemented on each platform
 */
void ODKTerminate_platform();
void OuyaController_startOfFrame_platform();

bool OuyaController_selectControllerByPlayer_platform(int playerNum);

bool OuyaController_selectControllerByDeviceID_platform(int deviceID);

int OuyaController_getAxisValue_platform(int axis);

bool OuyaController_getButton_platform(int button);

bool OuyaController_buttonPressedThisFrame_platform(int button);

bool OuyaController_buttonReleasedThisFrame_platform(int button);

bool OuyaController_buttonChangedThisFrame_platform(int button);

int OuyaController_getPlayerNum_platform();


#endif /* !ODK_INTERNAL_H */