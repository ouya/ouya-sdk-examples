#include "IwGx.h"
#include "IwResManager.h"
#include "Iw2D.h"
#include "VirtualControllerSprite.h"

VirtualControllerSprite m_controllers[4];

void setupTextures()
{
	IwGetResManager()->LoadGroup("tiles.group");

	CIw2DImage* a = NULL;
	CIw2DImage* cutter = NULL;
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
	
	a = Iw2DCreateImageResource("a");
	cutter = Iw2DCreateImageResource("cutter");
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

	m_controllers[0].Position = CIwFVec2(400, 150);
	m_controllers[1].Position = CIwFVec2(1000, 150);
	m_controllers[2].Position = CIwFVec2(400, 550);
	m_controllers[3].Position = CIwFVec2(1000, 550);

	for (int index = 0; index < 4; ++index)
	{
		m_controllers[index].Initialize(
			a,
			cutter,
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
			y);
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