#ifndef __ORG_JSON_JSONOBJECT_JSONOBJECT_H__
#define __ORG_JSON_JSONOBJECT_JSONOBJECT_H__

#include <string>

namespace org_json_JSONArray
{
	class JSONArray;
}

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
		double getDouble(const std::string& name);
		org_json_JSONArray::JSONArray getJSONArray(const std::string& name);
		JSONObject getJSONObject(const std::string& name);		
		int getInt(const std::string& name);
		std::string getString(const std::string& name);
		bool has(const std::string& name);
	private:
		static JNIEnv* _env;
		static jclass _jcJsonObject;
		static jmethodID _mConstruct;
		static jmethodID _mGetDouble;
		static jmethodID _mGetInt;
		static jmethodID _mGetJsonArray;
		static jmethodID _mGetJsonObject;
		static jmethodID _mGetString;
		static jmethodID _mHas;
		jobject _instance;
	};
}

#endif