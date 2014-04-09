#ifndef __java_lang_String_String__
#define __java_lang_String_String__

#include <string>

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace java_lang_String
{
	class String
	{
	public:
		String(jstring objString);
		String(const std::string& val);
		String(jbyte* buffer, int length);
		static int InitJNI(JNIEnv* env);
		jstring GetInstance() const;
		const std::string& ToString();
	private:
		static JNIEnv* _env;
		static jclass _jcString;
		static jmethodID _mConstruct;
		jstring _instance;
		std::string _strInstance;
	};
}

#endif