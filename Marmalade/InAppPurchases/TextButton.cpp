/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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