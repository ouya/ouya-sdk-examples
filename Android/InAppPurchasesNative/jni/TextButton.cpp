#include "TextButton.h"

TextButton::TextButton()
{
	ActiveText = "";
	InactiveText = "";

	Down = 0;
	Left = 0;
	Right = 0;
	Up = 0;

	UIText = 0;
}

TextButton::TextButton(std::string activeText, std::string inactiveText, TextButton* down, TextButton* left, TextButton* right, TextButton* up)
{
	ActiveText = activeText;
	InactiveText = inactiveText;

	Down = down;
	Left = left;
	Right = right;
	Up = up;

	UIText = 0;
}

void TextButton::Setup(int font, int size)
{
	UIText = NVBFTextAlloc();
	NVBFTextSetFont(UIText, font);
	NVBFTextSetSize(UIText, size);
    NVBFTextSetColor(UIText, NV_PC_PREDEF_WHITE);
	
	SetActive(false);
}

void TextButton::Setup(int font, int size, const char* activeText, const char* inactiveText)
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
			NVBFTextSetString(UIText, ActiveText.c_str());
		}
		else
		{
			NVBFTextSetString(UIText, InactiveText.c_str());
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