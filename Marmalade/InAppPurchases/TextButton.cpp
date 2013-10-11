#include "TextButton.h"

#include "IwGx.h"

TextButton::TextButton()
{
	ActiveText = "";
	InactiveText = "";
	m_text = "";

	Down = 0;
	Left = 0;
	Right = 0;
	Up = 0;
}

TextButton::TextButton(std::string activeText, std::string inactiveText, TextButton* down, TextButton* left, TextButton* right, TextButton* up)
{
	ActiveText = activeText;
	InactiveText = inactiveText;

	Down = down;
	Left = left;
	Right = right;
	Up = up;

	m_text = "";
}

void TextButton::Setup(int font, int size)
{
	SetActive(false);
}

void TextButton::Setup(int font, int size, std::string activeText, std::string inactiveText)
{
	ActiveText = activeText;
	InactiveText = inactiveText;
	SetActive(false);
}

void TextButton::SetPosition(int x, int y)
{
	m_x = x;
	m_y = y;
}

void TextButton::SetActive(bool flag)
{
	if (flag)
	{
		m_text = ActiveText;
	}
	else
	{
		m_text = InactiveText;
	}
}

void TextButton::Render()
{
	IwGxPrintString(m_x, m_y, m_text.c_str());
}

void TextButton::Destroy()
{
}