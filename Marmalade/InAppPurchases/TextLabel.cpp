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