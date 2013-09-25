#include "ui.h"

#include <EGL/egl.h>
#include <EGL/eglplatform.h>
#include <GLES2/gl2.h>

#include <android/log.h>
#include <nv_and_util/nv_native_app_glue.h>
#include <nv_egl_util/nv_egl_util.h>
#include <nv_bitfont/nv_bitfont.h>
#include <nv_shader/nv_shader.h>

#define APP_NAME "inapppurchasenative-ui"

#define LOGD(...) ((void)__android_log_print(ANDROID_LOG_DEBUG,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO,  \
											 APP_NAME, \
											 __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN,  \
											 APP_NAME, \
											 __VA_ARGS__))

UI::UI()
{
	m_uiInitialized = false;

	m_productIds = new char*[5];

	m_productIds[0] = "long_sword";
	m_productIds[1] = "sharp_axe";
	m_productIds[2] = "cool_level";
	m_productIds[3] = "awesome_sauce";
	m_productIds[4] = "__DECLINED__THIS_PURCHASE";
}

void UI::Initialize(PluginOuya* pluginOuya)
{
	m_pluginOuya = pluginOuya;
}

bool UI::InitUI()
{
	if (m_uiInitialized)
		return true;

	#define NUM_FONTS	2
	static NvBool fontsSplit[NUM_FONTS] = {1,1}; /* all are split */
	static const char *fontFiles[NUM_FONTS] = {
	    "courier+lucida_256.dds",
	    "utahcond+bold_1024.dds"
	};
	if (NVBFInitialize(NUM_FONTS, (const char**)fontFiles, fontsSplit, 0))
	{
		LOGW("Could not initialize NvBitFont");
		return false;
	}

	m_uiLabelFetch = NVBFTextAlloc();
	NVBFTextSetFont(m_uiLabelFetch, 2);
	NVBFTextSetSize(m_uiLabelFetch, 32);
    NVBFTextSetColor(m_uiLabelFetch, NV_PC_PREDEF_WHITE);

	m_uiRequestGamerUUID.Setup(2, 32, NVBF_COLORSTR_GREEN "[Get GamerUUID]", NVBF_COLORSTR_WHITE "Get GamerUUID");
	m_uiRequestProducts.Setup(2, 32, NVBF_COLORSTR_GREEN "[Get Products]", NVBF_COLORSTR_WHITE "Get Products");
	m_uiRequestPurchase.Setup(2, 32, NVBF_COLORSTR_GREEN "[Purchase]", NVBF_COLORSTR_WHITE "Purchase");
	m_uiRequestReceipts.Setup(2, 32, NVBF_COLORSTR_GREEN "[Get Receipts]", NVBF_COLORSTR_WHITE "Get Receipts");
	m_uiPause.Setup(2, 32, NVBF_COLORSTR_GREEN "[Pause]", NVBF_COLORSTR_WHITE "Pause");

	m_uiRequestGamerUUID.Down = &m_uiRequestProducts;

	m_uiRequestProducts.Right = &m_uiRequestPurchase;
	m_uiRequestProducts.Up = &m_uiRequestGamerUUID;

	m_uiRequestPurchase.Left = &m_uiRequestProducts;
	m_uiRequestPurchase.Right = &m_uiRequestReceipts;

	m_uiRequestReceipts.Left = &m_uiRequestPurchase;

	m_uiPause.Left = &m_uiRequestReceipts;

	m_selectedButton = &m_uiPause;
	m_uiPause.SetActive(true);

	m_uiInitialized = true;

	return true;
}

void UI::Destroy()
{
	NVBFTextFree(m_uiLabelFetch);
	m_uiLabelFetch = 0;

	m_uiRequestGamerUUID.Destroy();
	m_uiRequestProducts.Destroy();
	m_uiRequestPurchase.Destroy();
	m_uiRequestReceipts.Destroy();
	m_uiPause.Destroy();
}

void UI::Resize(int w, int h)
{
	NVBFTextCursorAlign(m_uiLabelFetch, NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	NVBFTextCursorPos(m_uiLabelFetch, w*4/5, h/6);

	m_uiRequestGamerUUID.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestGamerUUID.SetPosition(w/5, h/6);

	m_uiRequestProducts.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestProducts.SetPosition(w/5, h/4);

	m_uiRequestPurchase.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestPurchase.SetPosition(w*2/5, h/4);

	m_uiRequestReceipts.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiRequestReceipts.SetPosition(w*3/5, h/4);

	m_uiPause.SetAlignment(NVBF_ALIGN_CENTER, NVBF_ALIGN_CENTER);
	m_uiPause.SetPosition(w*4/5, h/4);
}

void UI::Render()
{
	NVBFTextRender(m_uiLabelFetch);

	m_uiRequestGamerUUID.Render();
	m_uiRequestProducts.Render();
	m_uiRequestPurchase.Render();
	m_uiRequestReceipts.Render();
	m_uiPause.Render();
}

void UI::HandleInput(int keyCode, int action)
{
	if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == 82) //system button
	{
		if (m_selectedButton)
		{
			m_selectedButton->SetActive(false);
		}

		m_selectedButton = &m_uiPause;
		m_uiPause.SetActive(true);
	}

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == 21) //dpad left
	{
		if (m_selectedButton &&
			m_selectedButton->Left)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Left;
			m_selectedButton->SetActive(true);
		}
	}

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == 22) //dpad right
	{
		if (m_selectedButton &&
			m_selectedButton->Right)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Right;
			m_selectedButton->SetActive(true);
		}
	}

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == 19) //dpad up
	{
		if (m_selectedButton &&
			m_selectedButton->Up)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Up;
			m_selectedButton->SetActive(true);
		}
	}

	else if (action == AMOTION_EVENT_ACTION_UP &&
		keyCode == 20) //dpad down
	{
		if (m_selectedButton &&
			m_selectedButton->Down)
		{
			m_selectedButton->SetActive(false);
			m_selectedButton = m_selectedButton->Down;
			m_selectedButton->SetActive(true);
		}
	}

	else if(action == AMOTION_EVENT_ACTION_UP &&
		keyCode == 23) //button O
	{
		if (m_selectedButton)
		{
			LOGI("Executing action");
			if (m_selectedButton == &m_uiRequestGamerUUID)
			{
				m_pluginOuya->AsyncOuyaFetchGamerUUID(m_callbacksFetchGamerUUID);
			}
			if (m_selectedButton == &m_uiRequestProducts)
			{
				m_pluginOuya->AsyncOuyaRequestProducts(m_callbacksRequestProducts, m_productIds);
			}
			if (m_selectedButton == &m_uiRequestPurchase)
			{
				m_pluginOuya->AsyncOuyaRequestPurchase(m_callbacksRequestPurchase, "hello");
			}
			if (m_selectedButton == &m_uiRequestReceipts)
			{
				m_pluginOuya->AsyncOuyaRequestReceipts(m_callbacksRequestReceipts);
			}
		}
	}
}

void UI::SetGamerUUID(const char* gamerUUID)
{
	if (m_uiLabelFetch)
	{
		NVBFTextSetString(m_uiLabelFetch, gamerUUID);
	}
}