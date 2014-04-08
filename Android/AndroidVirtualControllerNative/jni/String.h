#ifndef __java_lang_String_String__
#define __java_lang_String_String__

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
		String(const char* val);
		static int InitJNI(JNIEnv* env);
		jstring GetInstance() const;
	private:
		static JNIEnv* _env;
		static jclass _jcString;
		jstring _instance;
	};
}

#endif