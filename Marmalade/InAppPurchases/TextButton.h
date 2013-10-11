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