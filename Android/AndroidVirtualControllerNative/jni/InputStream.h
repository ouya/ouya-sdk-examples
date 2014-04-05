#ifndef __INPUT_STREAM_H__
#define __INPUT_STREAM_H__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace java_io_InputStream
{
	class InputStream
	{
	public:
		InputStream(jobject inputStream);
		InputStream(const InputStream& copy);
		~InputStream();
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
		void close();
	private:
		static JNIEnv* _env;
		static jclass _jcInputStream;
		static jmethodID _mClose;
		jobject _instance;
	};
}

#endif