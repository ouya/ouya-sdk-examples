#include "IwGx.h"

int main()
{
	IwGxInit();
	IwGxSetColClear(0, 0, 0xff, 0xff);
	while (!s3eDeviceCheckQuitRequest())
	{
		IwGxClear();
		IwGxPrintString(120, 150, "Hello, World!");
		IwGxFlush();
		IwGxSwapBuffers();
	}
	IwGxTerminate();
	return 0;
}