#include <android/log.h>
#include <jni.h>

#include "JSONArray.h"
#include "JSONObject.h"
#include "MappingParser.h"

#define LOG_TAG "MappingParser"

using namespace org_json_JSONArray;
using namespace org_json_JSONObject;

namespace OuyaEverywhere
{
	void MappingParser::parse(std::string jsonData)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "jsonData: %s", jsonData.c_str());

		JSONObject jObject = JSONObject(jsonData);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded input json");

		JSONArray map = jObject.getJSONArray("map");
		for (int deviceId = 0; deviceId < map.length(); ++deviceId)
		{
			Device mappingDevice = Device();

			JSONObject objDevice = map.getJSONObject(deviceId);
			//Log.i(TAG, "device="+objDevice.toString());
			JSONObject device = objDevice.getJSONObject("device");
			JSONArray deviceAlias = device.getJSONArray("alias");
			//Log.i(TAG, "alias="+alias.toString());
			for (int aliasId = 0; aliasId < deviceAlias.length(); ++aliasId)
			{
				std::string strAlias = deviceAlias.getString(aliasId);
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "device alias=%s", strAlias.c_str());
				mappingDevice.mAlias.push_back(strAlias);
				mDevice[strAlias] = mappingDevice;
			}

			/*
			JSONArray controller = device.getJSONArray("controller");
			//Log.i(TAG, "controller="+controller.toString());
			for (int controllerId = 0; controllerId < controller.length(); ++controllerId)
			{
				Controller mappingController = Controller();

				JSONObject objController = controller.getJSONObject(controllerId);
				//Log.i(TAG, "objController="+objController.toString());
				JSONArray controllerAlias = objController.getJSONArray("alias");
				//Log.i(TAG, "controllerAlias="+controllerAlias.toString());
				for (int aliasId = 0; aliasId < controllerAlias.length(); ++aliasId) {
					std::string strController = controllerAlias.getString(aliasId);
					__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "controller alias=%s", strController.c_str());
					mappingController.mAlias.push_back(strController);
					mappingDevice.mController[strController] = mappingController;
				}
				if (objController.has("axis_remap")) {
					JSONArray axis = objController.getJSONArray("axis_remap");
					//Log.i(TAG, "axisRemap="+axis.toString());
					for (int axisId = 0; axisId < axis.length(); ++axisId) {

						AxisRemap mappingAxis = AxisRemap();

						JSONObject objAxis = axis.getJSONObject(axisId);
						//Log.i(TAG, "objAxis="+objAxis);
						mappingAxis.mSourceAxis = objAxis.getInt("source_axis");
						//Log.i(TAG, "SourceAxis="+mappingAxis.mSourceAxis);
						mappingAxis.mDestinationAxis = objAxis.getInt("destination_axis");
						//Log.i(TAG, "destinationAxis="+mappingAxis.mDestinationAxis);

						mappingController.mAxisRemap.push_back(mappingAxis);
					}
				}
			}
			*/
		}
	}
}