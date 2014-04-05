#ifndef __BITMAP_FACTORY_H__
#define __BITMAP_FACTORY_H__

#include "Bitmap.h"
#include "BitmapFactory.h"
#include "InputStream.h"

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace android_graphics_BitmapFactory
{
	class BitmapFactory
	{
	public:
		static int InitJNI(JNIEnv* env);
		class Options
		{
		public:
			Options();
			Options(const Options& options);
			~Options();
			static int InitJNI(JNIEnv* env);
			bool get_inScaled();
			void set_inScaled(bool inScaled);
			jobject GetInstance();
		private:
			static JNIEnv* _env;
			static jclass _jcOptions;
			static jmethodID _mOptionsConstruct;
			static jfieldID _jbInScaled;
			jobject _instance;
		};
		static android_graphics_Bitmap::Bitmap decodeStream(java_io_InputStream::InputStream stream, int rect, Options options);
	private:
		static JNIEnv* _env;
		static jclass _jcBitmapFactory;
		static jmethodID _mDecodeStream;
	};
}

#endif