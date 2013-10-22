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

#ifndef __TEXT_LABEL_H__
#define __TEXT_LABEL_H__

#include <string>

class TextLabel
{
public:
	TextLabel();

	void Setup(int x, int y, const std::string& text);

	void SetPosition(int x, int y);

	void SetText(const std::string& text);

	void Render();

	void Destroy();

private:
	std::string m_text;

	int m_x;
	int m_y;
};


#endif