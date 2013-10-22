/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
