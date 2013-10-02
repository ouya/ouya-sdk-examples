#ifndef __APPLICATION_H__
#define __APPLICATION_H__

#include "PluginOuya.h"
#include "UI.h"

class Application
{
public:
	static struct android_app* m_app;
	static OuyaSDK::PluginOuya m_pluginOuya;
	static UI m_ui;
};

#endif