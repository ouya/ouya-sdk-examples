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
#include "IwDebug.h"

#include "Application.h"

#include "ODK.h"


const char* g_version = "Mamalade Version: 074";


void render()
{
	IwGxClear();

	IwGxPrintString(200, 200, g_version);

	Application::m_ui.InitUI();

	Application::m_ui.Render();

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

    const int textWidth = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_WIDTH);
    const int textHeight = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_HEIGHT);
    const int width = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
    const int height = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

	Application::InitOuyaPlugin();

	while (!s3eDeviceCheckQuitRequest())
	{
		//IwTrace(DEFAULT, ("Main loop while (!s3eDeviceCheckQuitRequest()"));

		render();

		Application::m_ui.HandleInput();

		// keep polling for input, don't kill the CPU
		s3eDeviceYield(0);
	}

	Application::m_ui.Destroy();

	Iw2DTerminate();
	IwResManagerTerminate();
	IwGxTerminate();
	return 0;
}