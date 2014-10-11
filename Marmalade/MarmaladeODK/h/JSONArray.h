#ifndef __ORG_JSON_JSONARRAY_JSONARRAY_H__
#define __ORG_JSON_JSONARRAY_JSONARRAY_H__

#if defined(ANDROID)
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include <jni.h>
#endif

#include <string>

namespace org_json_JSONObject
{
	class JSONObject;
}

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

class _jobject;
typedef _jobject* jobject;

class _jmethodID;
typedef _jmethodID* jmethodID;

namespace org_json_JSONArray
{
	class JSONArray
	{
	public:
		static int InitJNI(JNIEnv* env);
		static void FindJNI();
		JSONArray(jobject jsonArray);
		JSONArray(const std::string& json);
		jobject GetInstance() const;
		int length() const;
		org_json_JSONObject::JSONObject getJSONObject(int index) const;
		int getInt(int index) const;
		std::string getString(int index) const;
	private:
		static JNIEnv* _env;
		static jclass _jcJsonArray;
		static jmethodID _mConstruct;
		static jmethodID _mGetInt;
		static jmethodID _mGetJsonObject;
		static jmethodID _mGetString;
		static jmethodID _mLength;
		jobject _instance;
	};
}

#endif