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