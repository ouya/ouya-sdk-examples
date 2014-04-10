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
			for (int aliasId = 0; aliasId < deviceAlias.length(); ++aliasId) {
				std::string strAlias = deviceAlias.getString(aliasId);
				__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "device alias=%s", strAlias.c_str());
				mappingDevice.mAlias.push_back(strAlias);
				mDevice[strAlias] = mappingDevice;
			}			
		}
	}
}