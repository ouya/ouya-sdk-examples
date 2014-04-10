#include <android/log.h>
#include <jni.h>

#include "JSONArray.h"
#include "JSONObject.h"
#include "MappingParser.h"
#include "OuyaController.h"

#define LOG_TAG "MappingParser"

using namespace org_json_JSONArray;
using namespace org_json_JSONObject;
using namespace tv_ouya_console_api_OuyaController;

namespace OuyaEverywhere
{
	MappingParser::Device* MappingParser::getDevice(const std::string& deviceName)
	{
		std::map<std::string, Device*>::const_iterator search = mDevice.find(deviceName);
		if (search == mDevice.end())
		{
			return NULL;
		}
		
		return search->second;
	}
	MappingParser::Controller* MappingParser::getController(MappingParser::Device* device, const std::string& controllerName)
	{
		if (NULL == device)
		{
			return NULL;
		}

		std::map<std::string, Controller*>::const_iterator search = device->mController.find(controllerName);
		if (search == device->mController.end())
		{
			return NULL;
		}

		return search->second;
	}
	MappingParser::Button* MappingParser::getButton(const std::string& deviceName, const std::string& controllerName, int keyCode)
	{
		MappingParser::Device* device = getDevice(deviceName);
		if (NULL == device) {
			return NULL;
		}
		MappingParser::Controller* controller = getController(device, controllerName);
		if (NULL == controller)
		{
			return NULL;
		}

		std::map<int, Button*>::const_iterator search = controller->mButton.find(keyCode);
		if (search == controller->mButton.end())
		{
			return NULL;
		}

		return search->second;
	}
	std::vector<MappingParser::AxisRemap*>* MappingParser::getAxisRemap(const std::string& deviceName, const std::string& controllerName)
	{
		MappingParser::Device* device = getDevice(deviceName);
		if (NULL == device)
		{
			return NULL;
		}
		MappingParser::Controller* controller = getController(device, controllerName);
		if (NULL == controller)
		{
			return NULL;
		}
		return &controller->mAxisRemap;
	}
	std::vector<MappingParser::ButtonIsAxis*>* MappingParser::getButtonIsAxis(const std::string& deviceName, const std::string& controllerName)
	{
		Device* device = getDevice(deviceName);
		if (NULL == device)
		{
			return NULL;
		}
		Controller* controller = getController(device, controllerName);
		if (NULL == controller)
		{
			return NULL;
		}
		return &controller->mButtonIsAxis;
	}

	int MappingParser::getKeyCode(const std::string& name)
	{
		if (name.compare("BUTTON_O")) {
			return OuyaController::BUTTON_O();
		}
		else if (name.compare("BUTTON_U")) {
			return OuyaController::BUTTON_U();
		}
		else if (name.compare("BUTTON_Y")) {
			return OuyaController::BUTTON_Y();
		}
		else if (name.compare("BUTTON_A")) {
			return OuyaController::BUTTON_A();
		}
		else if (name.compare("BUTTON_L1")) {
			return OuyaController::BUTTON_L1();
		}
		else if (name.compare("BUTTON_R1")) {
			return OuyaController::BUTTON_R1();
		}
		else if (name.compare("BUTTON_L3")) {
			return OuyaController::BUTTON_L3();
		}
		else if (name.compare("BUTTON_R3")) {
			return OuyaController::BUTTON_R3();
		}
		else if (name.compare("BUTTON_DPAD_UP")) {
			return OuyaController::BUTTON_DPAD_UP();
		}
		else if (name.compare("BUTTON_DPAD_DOWN")) {
			return OuyaController::BUTTON_DPAD_DOWN();
		}
		else if (name.compare("BUTTON_DPAD_RIGHT")) {
			return OuyaController::BUTTON_DPAD_RIGHT();
		}
		else if (name.compare("BUTTON_DPAD_LEFT")) {
			return OuyaController::BUTTON_DPAD_LEFT();
		}
		else {
			return 0;
		}
	}

	void MappingParser::parse(const std::string& jsonData)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "jsonData: %s", jsonData.c_str());

		JSONObject jObject = JSONObject(jsonData);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded input json");

		JSONArray map = jObject.getJSONArray("map");
		for (int deviceId = 0; deviceId < map.length(); ++deviceId)
		{
			Device* mappingDevice = new Device();

			JSONObject objDevice = map.getJSONObject(deviceId);
			//Log.i(TAG, "device="+objDevice.toString());
			JSONObject device = objDevice.getJSONObject("device");
			JSONArray deviceAlias = device.getJSONArray("alias");
			//Log.i(TAG, "alias="+alias.toString());
			for (int aliasId = 0; aliasId < deviceAlias.length(); ++aliasId)
			{
				std::string strAlias = deviceAlias.getString(aliasId);
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "device alias=%s", strAlias.c_str());
				mappingDevice->mAlias.push_back(strAlias);
				mDevice[strAlias] = mappingDevice;
			}

			JSONArray controller = device.getJSONArray("controller");
			//Log.i(TAG, "controller="+controller.toString());
			for (int controllerId = 0; controllerId < controller.length(); ++controllerId)
			{
				Controller* mappingController = new Controller();

				JSONObject objController = controller.getJSONObject(controllerId);
				//Log.i(TAG, "objController="+objController.toString());
				JSONArray controllerAlias = objController.getJSONArray("alias");
				//Log.i(TAG, "controllerAlias="+controllerAlias.toString());
				for (int aliasId = 0; aliasId < controllerAlias.length(); ++aliasId) {
					std::string strController = controllerAlias.getString(aliasId);
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "controller alias=%s", strController.c_str());
					mappingController->mAlias.push_back(strController);
					mappingDevice->mController[strController] = mappingController;
				}
				if (objController.has("axis_remap")) {
					JSONArray axis = objController.getJSONArray("axis_remap");
					//Log.i(TAG, "axisRemap="+axis.toString());
					for (int axisId = 0; axisId < axis.length(); ++axisId) {

						AxisRemap* mappingAxis = new AxisRemap();

						JSONObject objAxis = axis.getJSONObject(axisId);
						//Log.i(TAG, "objAxis="+objAxis);
						mappingAxis->mSourceAxis = objAxis.getInt("source_axis");
						//Log.i(TAG, "SourceAxis="+mappingAxis.mSourceAxis);
						mappingAxis->mDestinationAxis = objAxis.getInt("destination_axis");
						//Log.i(TAG, "destinationAxis="+mappingAxis.mDestinationAxis);

						mappingController->mAxisRemap.push_back(mappingAxis);
					}
				}
				if (objController.has("button_is_axis")) {
					JSONArray button = objController.getJSONArray("button_is_axis");
					//Log.i(TAG, "buttonIsAxis="+button.toString());
					for (int buttonId = 0; buttonId < button.length(); ++buttonId) {

						ButtonIsAxis* mappingButton = new ButtonIsAxis();

						JSONObject objButton = button.getJSONObject(buttonId);
						//Log.i(TAG, "objButton="+objButton);
						mappingButton->mSourceAxis = objButton.getInt("source_axis");
						//Log.i(TAG, "sourceAxis="+sourceAxis);
						mappingButton->mActionDown = (float)objButton.getDouble("action_down");
						//Log.i(TAG, "actionDown="+actionDown);
						std::string destinationKeyCode = objButton.getString("destination_keycode");
						//Log.i(TAG, "destinationKeyCode="+destinationKeyCode);
						mappingButton->mDestinationKeyCode = getKeyCode(destinationKeyCode);

						mappingController->mButtonIsAxis.push_back(mappingButton);
					}
				}
				if (objController.has("button")) {
					JSONArray button = objController.getJSONArray("button");
					//Log.i(TAG, "buttons="+button.toString());
					for (int buttonId = 0; buttonId < button.length(); ++buttonId) {

						Button* mappingButton = new Button();

						JSONObject objButton = button.getJSONObject(buttonId);
						//Log.i(TAG, "objButton="+objButton);
						mappingButton->mSourceKeyCode = objButton.getInt("source_keycode");
						//Log.i(TAG, "sourceKeycode="+mappingButton.mSourceKeyCode);
						std::string destination_keycode = objButton.getString("destination_keycode");
						//Log.i(TAG, "destination_keycode="+destination_keycode);							
						//Log.i(TAG, "sourceKeycode="+mappingButton.mSourceKeyCode+" destination_keycode="+destination_keycode);

						if (objButton.has("exclude_source")) {
							JSONArray excludeSource = objButton.getJSONArray("exclude_source");
							for (int excludeId = 0; excludeId < excludeSource.length(); ++excludeId) {
								int source = excludeSource.getInt(excludeId);
								mappingButton->mExcludeSource.push_back(source);
								//Log.i(TAG, "exclude_source="+source);									
							}
						}

						mappingButton->mDestinationKeyCode = getKeyCode(destination_keycode);

						mappingController->mButton[mappingButton->mSourceKeyCode] = mappingButton;
					}
				}
			}
		}
	}
}