#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "VirtualControllerSprite.h"

VirtualControllerSprite m_controllers[4];

void setupTextures()
{
	IwGetResManager()->LoadGroup("tiles.group");
	
	CIw2DImage* cutter = Iw2DCreateImageResource("cutter");

	for (int index = 0; index < 4; ++index)
	{
		m_controllers[index].Controller = cutter;
	}
}

void destroyTextures()
{
	delete m_controllers[0].Controller;
}

int main()
{
	IwGxInit();
	IwGxSetColClear(0, 0, 0xff, 0xff);
	IwResManagerInit();
	Iw2DInit();
	setupTextures();
	while (!s3eDeviceCheckQuitRequest())
	{
		IwGxClear();
		IwGxPrintString(120, 150, "Hello, World!");
		for (int index = 0; index < 4; ++index)
		{
			m_controllers[index].Render();
		}
		IwGxFlush();
		IwGxSwapBuffers();
	}
	destroyTextures();
	Iw2DTerminate();
	IwResManagerTerminate();
	IwGxTerminate();
	return 0;
}