#ifndef __org_json_JSONObject_JSONObject__
#define __org_json_JSONObject_JSONObject__

#include <string>

#include "JsonArray.h"

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace org_json_JSONObject
{
	class JSONObject
	{
	public:
		JSONObject(jobject jsonObject);
		JSONObject(const std::string& json);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		org_json_JSONArray::JSONArray getJSONArray(const std::string& name);
	private:
		static JNIEnv* _env;
		static jclass _jcJsonObject;
		static jmethodID _mConstruct;
		static jmethodID _mGetJsonArray;
		jobject _instance;
	};
}

#endif