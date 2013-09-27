#ifndef __TEXT_BUTTON_H__
#define __TEXT_BUTTON_H__

#include <string>

#include <GLES2/gl2.h>
#include <nv_bitfont/nv_bitfont.h>

class TextButton
{
public:
	std::string ActiveText;
	std::string InactiveText;

	TextButton* Down;
	TextButton* Left;
	TextButton* Right;
	TextButton* Up;

	void* UIText;

	void* DataContext;

	TextButton();
	TextButton(char* activeText, char* inactiveText, TextButton* down, TextButton* left, TextButton* right, TextButton* up);

	void Setup(int font, int size);

	void Setup(int font, int size, const char* activeText, const char* inactiveText);

	void SetAlignment(NvU8 horizontal, NvU8 vertical);

	void SetPosition(int x, int y);

	void SetActive(bool flag);
	
	void Render();

	void Destroy();
};


#endif