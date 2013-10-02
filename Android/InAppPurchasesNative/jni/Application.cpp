#include "Application.h"

struct android_app* Application::m_app = NULL;
PluginOuya Application::m_pluginOuya = PluginOuya();
UI Application::m_ui = UI();