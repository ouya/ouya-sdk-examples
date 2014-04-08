#ifndef __java_io_InputStream_InputStream__
#define __java_io_InputStream_InputStream__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace java_io_InputStream
{
	class InputStream
	{
	public:
		InputStream();
		InputStream(const jobject& inputStream);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance();
		void SetInstance(jobject inputStream);
		void close();
	private:
		static JNIEnv* _env;
		static jclass _jcInputStream;
		static jmethodID _mClose;
		jobject _instance;
	};
}

#endif