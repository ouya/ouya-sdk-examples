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

#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "s3e.h"
#include "VirtualControllerSprite.h"

#include "ODK.h"

const char* g_version = "Mamalade Version: 009";

//the controller instances
VirtualControllerSprite m_controllers[OuyaController_MAX_CONTROLLERS];


void setupTextures()
{
	IwGetResManager()->LoadGroup("tiles.group");
	CIw2DImage* a = NULL;
	CIw2DImage* controller = NULL;
	CIw2DImage* dpad_down = NULL;
	CIw2DImage* dpad_left = NULL;
	CIw2DImage* dpad_right = NULL;
	CIw2DImage* dpad_up = NULL;
	CIw2DImage* lb = NULL;
	CIw2DImage* lt = NULL;
	CIw2DImage* l_stick = NULL;
	CIw2DImage* o = NULL;
	CIw2DImage* rb = NULL;
	CIw2DImage* rt = NULL;
	CIw2DImage* r_stick = NULL;
	CIw2DImage* thumbl = NULL;
	CIw2DImage* thumbr = NULL;
	CIw2DImage* u = NULL;
	CIw2DImage* y = NULL;
	CIw2DImage* menu = NULL;
	
	a = Iw2DCreateImageResource("a");
	controller = Iw2DCreateImageResource("controller");
	dpad_down = Iw2DCreateImageResource("dpad_down");
	dpad_left = Iw2DCreateImageResource("dpad_left");
	dpad_right = Iw2DCreateImageResource("dpad_right");
	dpad_up = Iw2DCreateImageResource("dpad_up");
	lb = Iw2DCreateImageResource("lb");
	lt = Iw2DCreateImageResource("lt");
	l_stick = Iw2DCreateImageResource("l_stick");
	o = Iw2DCreateImageResource("o");
	rb = Iw2DCreateImageResource("rb");
	rt = Iw2DCreateImageResource("rt");
	r_stick = Iw2DCreateImageResource("r_stick");
	thumbl = Iw2DCreateImageResource("thumbl");
	thumbr = Iw2DCreateImageResource("thumbr");
	u = Iw2DCreateImageResource("u");
	y = Iw2DCreateImageResource("y");
	menu = Iw2DCreateImageResource("menu");

	m_controllers[0].Position = CIwFVec2(400, 150);
	m_controllers[1].Position = CIwFVec2(1000, 150);
	m_controllers[2].Position = CIwFVec2(400, 550);
	m_controllers[3].Position = CIwFVec2(1000, 550);

	for (int index = 0; index < OuyaController_MAX_CONTROLLERS; ++index)
	{
		m_controllers[index].Initialize(
			index,
			a,
			controller,
			dpad_down,
			dpad_left,
			dpad_right,
			dpad_up,
			lb,
			lt,
			l_stick,
			o,
			rb,
			rt,
			r_stick,
			thumbl,
			thumbr,
			u,
			y,
			menu);
	}
}

void destroyTextures()
{
	for (int index = 0; index < OuyaController_MAX_CONTROLLERS; ++index)
	{
		delete m_controllers[index].ButtonA;
		delete m_controllers[index].ButtonO;
		delete m_controllers[index].ButtonU;
		delete m_controllers[index].ButtonY;
		delete m_controllers[index].Controller;
		delete m_controllers[index].DpadDown;
		delete m_controllers[index].DpadLeft;
		delete m_controllers[index].DpadRight;
		delete m_controllers[index].DpadUp;
		delete m_controllers[index].LeftBumper;
		delete m_controllers[index].LeftTrigger;
		delete m_controllers[index].LeftStickActive;
		delete m_controllers[index].LeftStickInactive;
		delete m_controllers[index].RightBumper;
		delete m_controllers[index].RightTrigger;
		delete m_controllers[index].RightStickActive;
		delete m_controllers[index].RightStickInactive;
		delete m_controllers[index].Menu;
	}
}

void handleInput()
{
	for (int index = 0; index < OuyaController_MAX_CONTROLLERS; ++index)
	{
		m_controllers[index].HandleInput();
	}

	OuyaPlugin_clearButtonStates();
}

void render()
{
	IwGxClear();

	IwGxPrintString(200, 200, g_version);

	for (int index = 0; index < 4; ++index)
	{
		m_controllers[index].Render();
	}

	IwGxFlush();
	IwGxSwapBuffers();
}

int main()
{
	if (!ODKAvailable())
	{
		IwTrace(DEFAULT, ("Not running on OUYA, exit!"));
		return 0;
	}

	IwGxInit();
	IwGxSetColClear(0, 0, 0xff, 0xff);
	IwResManagerInit();
	Iw2DInit();
	setupTextures();

    const int textWidth = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_WIDTH);
    const int textHeight = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_HEIGHT);
    const int width = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
    const int height = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

	while (!s3eDeviceCheckQuitRequest())
	{
		render();

		handleInput();

		// keep polling for input, don't kill the CPU
		s3eDeviceYield(0);
	}
	destroyTextures();
	Iw2DTerminate();
	IwResManagerTerminate();
	IwGxTerminate();
	return 0;
}