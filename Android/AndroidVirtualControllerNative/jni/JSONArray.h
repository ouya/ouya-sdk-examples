#ifndef __org_json_JSONArray_JSONArray__
#define __org_json_JSONArray_JSONArray__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace org_json_JSONArray
{
	class JSONArray
	{
	public:
		JSONArray(jobject jsonArray);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		int length();
	private:
		static JNIEnv* _env;
		static jclass _jcJsonArray;
		static jmethodID _mLength;
		jobject _instance;
	};
}

#endif