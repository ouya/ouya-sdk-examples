#ifndef __STRING_H__
#define __STRING_H__

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
		String(const String& copy);
		String(const char* val);
		~String();
		static int InitJNI(JNIEnv* env);
		jstring GetInstance() const;
	private:
		static JNIEnv* _env;
		static jclass _jcString;
		jstring _instance;
	};
}

#endif