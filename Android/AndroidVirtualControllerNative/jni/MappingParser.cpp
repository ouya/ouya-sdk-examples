#include <android/log.h>
#include <jni.h>

#include "JSONObject.h"
#include "MappingParser.h"

#define LOG_TAG "MappingParser"

using namespace org_json_JSONObject;

namespace OuyaEverywhere
{
	void MappingParser::parse(std::string jsonData)
	{
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "jsonData: %s", jsonData.c_str());

		JSONObject jObject = JSONObject(jsonData);
		__android_log_print(ANDROID_LOG_VERBOSE, LOG_TAG, "Loaded input json");
	}
}