#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"

CIw2DImage* m_cutter = NULL;

void setupTextures()
{
	IwGetResManager()->LoadGroup("tiles.group");
	m_cutter = Iw2DCreateImageResource("cutter");
}

void destroyTextures()
{
	delete m_cutter;
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
		Iw2DDrawImage(m_cutter, CIwFVec2(0, 0), CIwFVec2(128, 128));
		IwGxFlush();
		IwGxSwapBuffers();
	}
	destroyTextures();
	Iw2DTerminate();
	IwResManagerTerminate();
	IwGxTerminate();
	return 0;
}