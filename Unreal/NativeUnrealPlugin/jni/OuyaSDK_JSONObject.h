/*
* Copyright (C) 2012-2015 OUYA, Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

#ifndef __ORG_JSON_JSONOBJECT_JSONOBJECT_H__
#define __ORG_JSON_JSONOBJECT_JSONOBJECT_H__

#if PLATFORM_ANDROID

#include <jni.h>
#include <string>

namespace org_json_JSONArray
{
	class JSONArray;
}

namespace org_json_JSONObject
{
	class JSONObject
	{
	public:
		static int InitJNI(JavaVM* jvm);
		static int FindJNI();
		JSONObject();
		JSONObject(jobject jsonObject);
		JSONObject(const std::string& json);
		jobject GetInstance() const;
		void Dispose() const;
		double getDouble(const std::string& name) const;
		org_json_JSONArray::JSONArray getJSONArray(const std::string& name) const;
		JSONObject getJSONObject(const std::string& name) const;
		int getInt(const std::string& name) const;
		std::string getString(const std::string& name) const;
		bool has(const std::string& name) const;
		JSONObject put(const std::string& name, const std::string& value) const;
		std::string toString() const;
	private:
		static JavaVM* _jvm;
		static jclass _jcJsonObject;
		static jmethodID _mConstruct;
		static jmethodID _mConstruct2;
		static jmethodID _mGetDouble;
		static jmethodID _mGetInt;
		static jmethodID _mGetJsonArray;
		static jmethodID _mGetJsonObject;
		static jmethodID _mGetString;
		static jmethodID _mHas;
		static jmethodID _mPut;
		static jmethodID _mToString;
		jobject _instance;
	};
}

#endif

#endif
