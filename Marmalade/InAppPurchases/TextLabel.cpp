#include "TextLabel.h"

#include "IwGx.h"

TextLabel::TextLabel()
{
	m_text = "";
}

void TextLabel::Setup(int x, int y, const std::string& text)
{
	m_x = x;
	m_y = y;
	m_text = text;
}

void TextLabel::SetText(const std::string& text)
{
	m_text = text;
}

void TextLabel::SetPosition(int x, int y)
{
	m_x = x;
	m_y = y;
}

void TextLabel::Render()
{
	IwGxPrintString(m_x, m_y, m_text.c_str());
}

void TextLabel::Destroy()
{
	m_text = "";
}