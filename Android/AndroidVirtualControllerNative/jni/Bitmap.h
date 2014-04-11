#ifndef __ANDROID_GRAPHICS_BITMAP_BITMAP_H__
#define __ANDROID_GRAPHICS_BITMAP_BITMAP_H__

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
		int getHeight();
		int getWidth();
		void recycle();
	private:
		static JNIEnv* _env;
		static jclass _jcBitmap;
		static jmethodID _mGetHeight;
		static jmethodID _mGetWidth;
		static jmethodID _mRecycle;
		jobject _instance;
	};
}

#endif