#ifndef __JSON_UTIL_H__
#define __JSON_UTIL_H__

#include "JSON.h"

class JsonUtil
{
public:
	static std::string ParseString(JSONObject item, std::wstring fieldName);
	static int ParseInt(JSONObject item, std::wstring fieldName);
	static float ParseFloat(JSONObject item, std::wstring fieldName);
};

#endif