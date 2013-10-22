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

#ifndef __TEXT_BUTTON_H__
#define __TEXT_BUTTON_H__

#include <string>

class TextButton
{
public:
	std::string ActiveText;
	std::string InactiveText;

	TextButton* Down;
	TextButton* Left;
	TextButton* Right;
	TextButton* Up;

	void* DataContext;

	TextButton();
	TextButton(std::string activeText, std::string inactiveText, TextButton* down, TextButton* left, TextButton* right, TextButton* up);

	void Setup(int font, int size);

	void Setup(int font, int size, std::string activeText, std::string inactiveText);

	void SetPosition(int x, int y);

	void SetActive(bool flag);
	
	void Render();

	void Destroy();

private:
	std::string m_text;

	int m_x;
	int m_y;
};


#endif