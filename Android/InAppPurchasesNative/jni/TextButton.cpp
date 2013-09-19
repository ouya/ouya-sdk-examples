#include "TextButton.h"

TextButton::TextButton()
{
	ActiveText = 0;
	InactiveText = 0;

	Down = 0;
	Left = 0;
	Right = 0;
	Up = 0;

	UIText = 0;
}

TextButton::TextButton(char* activeText, char* inactiveText, TextButton* down, TextButton* left, TextButton* right, TextButton* up)
{
	ActiveText = activeText;
	InactiveText = inactiveText;

	Down = down;
	Left = left;
	Right = right;
	Up = up;

	UIText = 0;
}

void TextButton::Setup(int font, int size, char* activeText, char* inactiveText)
{
	UIText = NVBFTextAlloc();
	NVBFTextSetFont(UIText, font);
	NVBFTextSetSize(UIText, size);
    NVBFTextSetColor(UIText, NV_PC_PREDEF_WHITE);
	
	ActiveText = activeText;
	InactiveText = inactiveText;
	SetActive(false);
}

void TextButton::SetAlignment(NvU8 horizontal, NvU8 vertical)
{
	if (UIText)
	{
		NVBFTextCursorAlign(UIText, horizontal, vertical);
	}
}

void TextButton::SetPosition(int x, int y)
{
	if (UIText)
	{
		NVBFTextCursorPos(UIText, x, y);
	}
}

void TextButton::SetActive(bool flag)
{
	if (UIText)
	{
		if (flag)
		{
			NVBFTextSetString(UIText, ActiveText);
		}
		else
		{
			NVBFTextSetString(UIText, InactiveText);
		}
	}
}

void TextButton::Render()
{
	if (UIText)
	{
		NVBFTextRender(UIText);
	}
}

void TextButton::Destroy()
{
	if (UIText)
	{
		NVBFTextFree(UIText);
		UIText = 0;
	}
}