#ifndef __BITMAP_H__
#define __BITMAP_H__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_graphics_Bitmap
{
	class Bitmap
	{
	public:
		Bitmap(jobject bitmap);
		static int InitJNI(JNIEnv* env);
		jobject GetInstance() const;
	private:
		static JNIEnv* _env;
		static jclass _jcBitmap;
		jobject _instance;
	};
}

#endif