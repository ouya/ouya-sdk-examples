#ifndef __tv_ouya_console_api_OuyaController_OuyaController__
#define __tv_ouya_console_api_OuyaController_OuyaController__

struct _JNIEnv;
typedef _JNIEnv JNIEnv;

class _jclass;
typedef _jclass* jclass;

namespace tv_ouya_console_api_OuyaController
{
	class OuyaController
	{
	public:
		static int InitJNI(JNIEnv* env);
		static const int AXIS_LS_X();
		static const int AXIS_LS_Y();
		static const int AXIS_RS_X();
		static const int AXIS_RS_Y();
		static const int AXIS_L2();
		static const int AXIS_R2();
	private:
		static JNIEnv* _env;
		static jclass _jcOuyaController;
		static jfieldID _jfAxisLsX;
		static jfieldID _jfAxisLsY;
		static jfieldID _jfAxisRsX;
		static jfieldID _jfAxisRsY;
		static jfieldID _jfAxisL2;
		static jfieldID _jfAxisR2;
	};
}

#endif