#include "VirtualControllerSprite.h"

void VirtualControllerSprite::Render()
{
	Render(Controller);
}

void VirtualControllerSprite::Render(CIw2DImage* image)
{
	if (image)
	{
		Iw2DDrawImage(image, Position, CIwFVec2(image->GetWidth(), image->GetHeight()));
	}
}