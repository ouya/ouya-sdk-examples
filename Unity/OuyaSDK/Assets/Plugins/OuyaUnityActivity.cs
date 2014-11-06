#if UNITY_ANDROID && !UNITY_EDITOR

//#define VERBOSE_LOGGING

using System;
#if VERBOSE_LOGGING
using System.Reflection;
#endif
using UnityEngine;

namespace tv.ouya.sdk
{
    public class OuyaUnityActivity
    {
        private const string LOG_TAG = "OuyaUnityActivity";
        private static IntPtr _jcOuyaUnityActivity = IntPtr.Zero;
        private static IntPtr _jmDebugDisplayKeyDownElapsed = IntPtr.Zero;
        private static IntPtr _jmDebugDisplayKeyUpElapsed = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static OuyaUnityActivity()
        {
            try
            {
                {
                    string strName = "tv/ouya/sdk/OuyaUnityActivity";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcOuyaUnityActivity = AndroidJNI.NewGlobalRef(localRef);
                        AndroidJNI.DeleteLocalRef(localRef);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} class", strName));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("Exception loading JNI - {0}", ex));
            }
        }

        private static void JNIFind()
        {
            try
            {
                {
                    string strMethod = "debugDisplayKeyDownElapsed";
                    _jmDebugDisplayKeyDownElapsed = AndroidJNI.GetMethodID(_jcOuyaUnityActivity, strMethod, "()V");
                    if (_jmDebugDisplayKeyDownElapsed != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }

                {
                    string strMethod = "debugDisplayKeyUpElapsed";
                    _jmDebugDisplayKeyUpElapsed = AndroidJNI.GetMethodID(_jcOuyaUnityActivity, strMethod, "()V");
                    if (_jmDebugDisplayKeyUpElapsed != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} method", strMethod));
#endif
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} method", strMethod));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("Exception loading JNI - {0}", ex));
            }
        }

        public OuyaUnityActivity(IntPtr instance)
        {
            _instance = instance;
        }

        public IntPtr GetInstance()
        {
            return _instance;
        }

        public void debugDisplayKeyDownElapsed()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcOuyaUnityActivity == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityActivity is not initialized");
                return;
            }
            if (_jmDebugDisplayKeyDownElapsed == IntPtr.Zero)
            {
                Debug.LogError("_jmDebugDisplayKeyDownElapsed is not initialized");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmDebugDisplayKeyDownElapsed, new jvalue[0]);
        }

        public void debugDisplayKeyUpElapsed()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcOuyaUnityActivity == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityActivity is not initialized");
                return;
            }
            if (_jmDebugDisplayKeyUpElapsed == IntPtr.Zero)
            {
                Debug.LogError("_jmDebugDisplayKeyUpElapsed is not initialized");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmDebugDisplayKeyUpElapsed, new jvalue[0]);
        }

    }
}

#endif