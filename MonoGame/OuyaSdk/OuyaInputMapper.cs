using Android.Runtime;
using Android.Util;
using Android.Views;
using System;

namespace OuyaSdk
{
    public class OuyaInputMapper
    {
        private const string LOG_TAG = "OuyaInputMapper";
        private static IntPtr _jcOuyaInputMapper = IntPtr.Zero;
        private static IntPtr _jmInit = IntPtr.Zero;
        private static IntPtr _jmDispatchGenericMotionEvent = IntPtr.Zero;
        private static IntPtr _jmDispatchKeyEvent = IntPtr.Zero;
        private static IntPtr _jmShouldHandleInputEvent = IntPtr.Zero;
        private static IntPtr _jmShutdown = IntPtr.Zero;

        static OuyaInputMapper()
        {
            try
            {
                {
                    string strName = "tv/ouya/console/api/OuyaInputMapper";
                    _jcOuyaInputMapper = JNIEnv.FindClass(strName);
                    if (_jcOuyaInputMapper != IntPtr.Zero)
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
                    string strMethod = "init";
                    _jmInit = JNIEnv.GetStaticMethodID(_jcOuyaInputMapper, strMethod, "(Landroid/app/Activity;)V");
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
                    string strMethod = "dispatchGenericMotionEvent";
                    _jmDispatchGenericMotionEvent = JNIEnv.GetStaticMethodID(_jcOuyaInputMapper, strMethod, "(Landroid/app/Activity;Landroid/view/MotionEvent;)Z");
                    if (_jmDispatchGenericMotionEvent != IntPtr.Zero)
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
                    string strMethod = "dispatchKeyEvent";
                    _jmDispatchKeyEvent = JNIEnv.GetStaticMethodID(_jcOuyaInputMapper, strMethod, "(Landroid/app/Activity;Landroid/view/KeyEvent;)Z");
                    if (_jmDispatchKeyEvent != IntPtr.Zero)
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
                    string strMethod = "shutdown";
                    _jmShutdown = JNIEnv.GetStaticMethodID(_jcOuyaInputMapper, strMethod, "(Landroid/app/Activity;)V");
                    if (_jmShutdown != IntPtr.Zero)
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
                    string strMethod = "shouldHandleInputEvent";
                    _jmShouldHandleInputEvent = JNIEnv.GetStaticMethodID(_jcOuyaInputMapper, strMethod, "(Landroid/view/InputEvent;)Z");
                    if (_jmShouldHandleInputEvent != IntPtr.Zero)
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

        public static void init(IntPtr activity)
        {
            if (_jcOuyaInputMapper == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaInputMapper is not initialized");
                return;
            }

            if (_jmInit == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmInit is not initialized");
                return;
            }

            if (activity == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "activity is not initialized");
                return;
            }

            JNIEnv.CallStaticVoidMethod(_jcOuyaInputMapper, _jmInit, new JValue(activity));
        }

        public static bool DispatchGenericMotionEvent(IntPtr activity, MotionEvent motionEvent)
        {
            if (_jcOuyaInputMapper == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaInputMapper is not initialized");
                return true;
            }

            if (_jmDispatchGenericMotionEvent == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmDispatchGenericMotionEvent is not initialized");
                return true;
            }

            return JNIEnv.CallStaticBooleanMethod(_jcOuyaInputMapper, _jmDispatchGenericMotionEvent, new JValue(activity), new JValue(motionEvent));
        }

        public static bool DispatchKeyEvent(IntPtr activity, KeyEvent keyEvent)
        {
            if (_jcOuyaInputMapper == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaInputMapper is not initialized");
                return true;
            }

            if (_jmDispatchKeyEvent == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmDispatchKeyEvent is not initialized");
                return true;
            }

            return JNIEnv.CallStaticBooleanMethod(_jcOuyaInputMapper, _jmDispatchKeyEvent, new JValue(activity), new JValue(keyEvent));
        }

        public static bool shouldHandleInputEvent(IntPtr inputEvent)
        {
            if (_jcOuyaInputMapper == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaInputMapper is not initialized");
                return false;
            }

            if (_jmShouldHandleInputEvent == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmShouldHandleInputEvent is not initialized");
                return false;
            }

            return JNIEnv.CallStaticBooleanMethod(_jcOuyaInputMapper, _jmShouldHandleInputEvent, new JValue(inputEvent));
        }

        public static void shutdown(IntPtr activity)
        {
            if (_jcOuyaInputMapper == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaInputMapper is not initialized");
                return;
            }

            if (_jmShutdown == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmShutdown is not initialized");
                return;
            }

            JNIEnv.CallStaticVoidMethod(_jcOuyaInputMapper, _jmShutdown, new JValue(activity));
        }
    }
}