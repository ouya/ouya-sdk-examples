#include "Application.h"

struct android_app* Application::m_app = NULL;
OuyaSDK::PluginOuya Application::m_pluginOuya = OuyaSDK::PluginOuya();
UI Application::m_ui = UI();