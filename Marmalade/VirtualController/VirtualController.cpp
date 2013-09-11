#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "VirtualControllerSprite.h"

VirtualControllerSprite m_controllers[4];

void setupTextures()
{
	IwGetResManager()->LoadGroup("tiles.group");
	
	CIw2DImage* cutter = Iw2DCreateImageResource("cutter");

	m_controllers[0].Position = CIwFVec2(400, 150);
	m_controllers[1].Position = CIwFVec2(1000, 150);
	m_controllers[2].Position = CIwFVec2(400, 550);
	m_controllers[3].Position = CIwFVec2(1000, 550);

	for (int index = 0; index < 4; ++index)
	{
		m_controllers[index].Initialize(
			NULL,
			cutter,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL);
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