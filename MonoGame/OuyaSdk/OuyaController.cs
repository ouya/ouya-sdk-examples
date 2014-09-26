using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using System;

namespace OuyaSdk
{
    public class OuyaController
    {
        public class ButtonData
        {
            private static IntPtr _jcButtonData = IntPtr.Zero;
            private static IntPtr _jfButtonDrawable = IntPtr.Zero;
            private static IntPtr _jfButtonName = IntPtr.Zero;
            private IntPtr _instance = IntPtr.Zero;

            static ButtonData()
            {
                try
                {
                    {
                        string strName = "tv/ouya/console/api/OuyaController$ButtonData";
                        _jcButtonData = JNIEnv.FindClass(strName);
                        if (_jcButtonData != IntPtr.Zero)
                        {
                            Log.Info(LOG_TAG, string.Format("Found {0} class", strName));
                        }
                        else
                        {
                            Log.Error(LOG_TAG, string.Format("Failed to find {0} class", strName));
                            return;
                        }
                    }

                    {
                        string strField = "buttonDrawable";
                        _jfButtonDrawable = JNIEnv.GetFieldID(_jcButtonData, strField, "Landroid/graphics/drawable/Drawable;");
                        if (_jfButtonDrawable != IntPtr.Zero)
                        {
                            Log.Info(LOG_TAG, string.Format("Found {0} field", strField));
                        }
                        else
                        {
                            Log.Error(LOG_TAG, string.Format("Failed to find {0} field", strField));
                            return;
                        }
                    }

                    {
                        string strField = "buttonName";
                        _jfButtonName = JNIEnv.GetFieldID(_jcButtonData, strField, "Ljava/lang/String;");
                        if (_jfButtonName != IntPtr.Zero)
                        {
                            Log.Info(LOG_TAG, string.Format("Found {0} field", strField));
                        }
                        else
                        {
                            Log.Error(LOG_TAG, string.Format("Failed to find {0} field", strField));
                            return;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error(LOG_TAG, string.Format("Exception loading JNI - {0}", ex));
                }
            }

            public IntPtr Instance
            {
                get { return _instance; }
                set { _instance = value; }
            }

            public Drawable buttonDrawable
            {
                get
                {
                    if (_instance == IntPtr.Zero)
                    {
                        Log.Error(LOG_TAG, "_instance is not initialized");
                        return null;
                    }
                    if (_jfButtonDrawable == IntPtr.Zero)
                    {
                        Log.Error(LOG_TAG, "_jfButtonDrawable is not initialized");
                        return null;
                    }
                    IntPtr result = JNIEnv.GetObjectField(_instance, _jfButtonDrawable);
                    if (result == IntPtr.Zero)
                    {
                        Log.Error(LOG_TAG, "Failed to get drawable");
                        return null;
                    }
                    Drawable retVal = Drawable.GetObject<Drawable>(result, JniHandleOwnership.DoNotTransfer);
                    return retVal;
                }
            }

            public string buttonName
            {
                get
                {
                    if (_instance == IntPtr.Zero)
                    {
                        Log.Error(LOG_TAG, "_instance is not initialized");
                        return null;
                    }
                    if (_jfButtonName == IntPtr.Zero)
                    {
                        Log.Error(LOG_TAG, "_jfButtonName is not initialized");
                        return null;
                    }
                    IntPtr result = JNIEnv.GetObjectField(_instance, _jfButtonName);
                    if (result == IntPtr.Zero)
                    {
                        Log.Error(LOG_TAG, "Failed to get button name");
                        return null;
                    }
                    String retVal = JNIEnv.GetString(result, JniHandleOwnership.DoNotTransfer);
                    JNIEnv.DeleteLocalRef(result);
                    return retVal;
                }
            }
        }

        private const string LOG_TAG = "OuyaController";
        private static IntPtr _jcOuyaController = IntPtr.Zero;
        private static IntPtr _jmInit = IntPtr.Zero;
        private static IntPtr _jmGetButtonData = IntPtr.Zero;
        private static IntPtr _jmGetControllerByDeviceId = IntPtr.Zero;
        private static IntPtr _jmGetDeviceName = IntPtr.Zero;
        private static IntPtr _jmGetPlayerNumByDeviceId = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        public static int AXIS_LS_X = 0;
        public static int AXIS_LS_Y = 1;
        public static int AXIS_RS_X = 11;
        public static int AXIS_RS_Y = 14;
        public static int AXIS_L2 = 17;
        public static int AXIS_R2 = 18;

        public static int BUTTON_O = 96;
        public static int BUTTON_U = 99;
        public static int BUTTON_Y = 100;
        public static int BUTTON_A = 97;
        public static int BUTTON_L1 = 102;
        public static int BUTTON_R1 = 103;
        public static int BUTTON_L3 = 106;
        public static int BUTTON_R3 = 107;
        public static int BUTTON_DPAD_UP = 19;
        public static int BUTTON_DPAD_DOWN = 20;
        public static int BUTTON_DPAD_RIGHT = 22;
        public static int BUTTON_DPAD_LEFT = 21;
        public static int BUTTON_MENU = 82;

        public static int MAX_CONTROLLERS = 4;

        static OuyaController()
        {
            try
            {
                {
                    string strName = "tv/ouya/console/api/OuyaController";
                    _jcOuyaController = JNIEnv.FindClass(strName);
                    if (_jcOuyaController != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} class", strName));
                        _jcOuyaController = JNIEnv.NewGlobalRef(_jcOuyaController);
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} class", strName));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LOG_TAG, string.Format("Exception loading JNI - {0}", ex));
            }
        }

        public IntPtr Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        private static void InitJNI()
        {
            try
            {
                {
                    string strMethod = "init";
                    _jmInit = JNIEnv.GetStaticMethodID(_jcOuyaController, strMethod, "(Landroid/content/Context;)V");
                    if (_jmInit != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getControllerByDeviceId";
                    _jmGetControllerByDeviceId = JNIEnv.GetStaticMethodID(_jcOuyaController, strMethod, "(I)Ltv/ouya/console/api/OuyaController;");
                    if (_jmGetControllerByDeviceId != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getDeviceName";
                    _jmGetDeviceName = JNIEnv.GetMethodID(_jcOuyaController, strMethod, "()Ljava/lang/String;");
                    if (_jmGetDeviceName != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getPlayerNumByDeviceId";
                    _jmGetPlayerNumByDeviceId = JNIEnv.GetStaticMethodID(_jcOuyaController, strMethod, "(I)I");
                    if (_jmGetPlayerNumByDeviceId != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "getButtonData";
                    _jmGetButtonData = JNIEnv.GetStaticMethodID(_jcOuyaController, strMethod, "(I)Ltv/ouya/console/api/OuyaController$ButtonData;");
                    if (_jmGetButtonData != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} method", strMethod));
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LOG_TAG, string.Format("Exception loading JNI - {0}", ex));
            }
        }

        public static void init(IntPtr context)
        {
            InitJNI();

            if (_jmInit == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmInit is not initialized");
                return;
            }

            JNIEnv.CallStaticVoidMethod(_jcOuyaController, _jmInit, new JValue(context));
        }

        public static int getPlayerNumByDeviceId(int deviceId)
        {
            InitJNI();

            if (_jcOuyaController == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaController is not initialized");
                return 0;
            }
            if (_jmGetPlayerNumByDeviceId == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmGetPlayerNumByDeviceId is not initialized");
                return 0;
            }
            return JNIEnv.CallStaticIntMethod(_jcOuyaController, _jmGetPlayerNumByDeviceId, new JValue(deviceId));
        }

        public static OuyaController getControllerByDeviceId(int deviceId)
        {
            InitJNI();

            if (_jcOuyaController == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaController is not initialized");
                return null;
            }
            if (_jmGetControllerByDeviceId == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmGetControllerByDeviceId is not initialized");
                return null;
            }
            IntPtr result = JNIEnv.CallStaticObjectMethod(_jcOuyaController, _jmGetControllerByDeviceId, new JValue(deviceId));
            if (result == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "Failed to get OuyaController");
                return null;
            }
            OuyaController retVal = new OuyaController();
            retVal.Instance = result;
            return retVal;
        }

        public String getDeviceName()
        {
            InitJNI();

            if (_instance == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_instance is not initialized");
                return null;
            }
            if (_jmGetDeviceName == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmGetDeviceName is not initialized");
                return null;
            }
            IntPtr result = JNIEnv.CallObjectMethod(_instance, _jmGetDeviceName);
            if (result == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "Failed to get device name");
                return null;
            }
            String retVal = JNIEnv.GetString(result, JniHandleOwnership.DoNotTransfer);
            JNIEnv.DeleteLocalRef(result);
            return retVal;
        }

        public static ButtonData getButtonData(int button)
        {
            InitJNI();

            if (_jcOuyaController == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaController is not initialized");
                return null;
            }
            if (_jmGetButtonData == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmGetButtonData is not initialized");
                return null;
            }
            IntPtr result = JNIEnv.CallStaticObjectMethod(_jcOuyaController, _jmGetButtonData, new JValue(button));
            if (result == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "Failed to get ButtonData");
                return null;
            }
            ButtonData retVal = new ButtonData();
            retVal.Instance = result;
            return retVal;
        }
    }
}