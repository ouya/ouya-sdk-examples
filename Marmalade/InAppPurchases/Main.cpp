#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "s3e.h"
#include "IwDebug.h"

#include "Application.h"

#include "ODK.h"


static int32 ButtonEventHandler(void* _systemData, void* userData);
static int32 KeyEventHandler(void* _systemData, void* userData);
static int32 MotionEventHandler(void* _systemData, void* userData);
static int32 TouchEventHandler(void* _systemData, void* userData);
static int32 TouchMotionEventHandler(void* _systemData, void* userData);

const char* g_version = "Mamalade Version: 070";


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

	OuyaPlugin_asyncSetDeveloperId("310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");

	IwGxInit();
	IwGxSetColClear(0, 0, 0xff, 0xff);
	IwResManagerInit();
	Iw2DInit();

    const int textWidth = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_WIDTH);
    const int textHeight = s3eDebugGetInt(S3E_DEBUG_FONT_SIZE_HEIGHT);
    const int width = s3eSurfaceGetInt(S3E_SURFACE_WIDTH);
    const int height = s3eSurfaceGetInt(S3E_SURFACE_HEIGHT);

	for (int index = 0; index < 10; ++index)
	{
		IwTrace(DEFAULT, ("Entering Main loop..."));
	}

	while (!s3eDeviceCheckQuitRequest())
	{
		//IwTrace(DEFAULT, ("Main loop while (!s3eDeviceCheckQuitRequest()"));

		Application::m_ui.HandleInput();
		render();

		// Yield until unyield is called or a quit request is recieved
		s3eDeviceYield(0);
	}

	Application::m_ui.Destroy();

	Iw2DTerminate();
	IwResManagerTerminate();
	IwGxTerminate();
	return 0;
}