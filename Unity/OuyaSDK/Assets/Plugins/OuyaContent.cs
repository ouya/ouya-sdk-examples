//#define VERBOSE_LOGGING
#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace tv.ouya.console.api.content
{
    public class OuyaContent : IDisposable
    {
        private static IntPtr _jcOuyaContent = IntPtr.Zero;
        private static IntPtr _jmIsInitialized = IntPtr.Zero;
        private static IntPtr _jmCreate = IntPtr.Zero;
        private static IntPtr _jmIsAvailable = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        public enum SortMethod
        {
            Rating,
            CreatedAt,
            UpdatedAt,
        }

        static OuyaContent()
        {
            try
            {
                {
                    string strName = "tv/ouya/console/api/content/OuyaContent";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcOuyaContent = AndroidJNI.NewGlobalRef(localRef);
                        AndroidJNI.DeleteLocalRef(localRef);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Failed to find {0} class", strName));
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
            {
                string strMethod = "create";
                _jmCreate = AndroidJNI.GetMethodID(_jcOuyaContent, strMethod, "()Ltv/ouya/console/api/content/OuyaMod;");
                if (_jmCreate != IntPtr.Zero)
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
                string strMethod = "isAvailable";
                _jmIsAvailable = AndroidJNI.GetMethodID(_jcOuyaContent, strMethod, "()Z");
                if (_jmIsAvailable != IntPtr.Zero)
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
                string strMethod = "isInitialized";
                _jmIsInitialized = AndroidJNI.GetMethodID(_jcOuyaContent, strMethod, "()Z");
                if (_jmIsInitialized != IntPtr.Zero)
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

        public OuyaContent(IntPtr instance)
        {
            _instance = instance;
        }

        public IntPtr GetInstance()
        {
            return _instance;
        }

        public void Dispose()
        {
            if (_instance != IntPtr.Zero)
            {
                AndroidJNI.DeleteGlobalRef(_instance);
                _instance = IntPtr.Zero;
            }
        }

        public OuyaMod create()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmCreate == IntPtr.Zero)
            {
                Debug.LogError("_jmCreate is not initialized");
                return null;
            }
            IntPtr retVal = AndroidJNI.CallObjectMethod(_instance, _jmCreate, new jvalue[0] { });
            if (retVal == IntPtr.Zero)
            {
                Debug.LogError("Create returned null");
                return null;
            }
            IntPtr globalRef = AndroidJNI.NewGlobalRef(retVal);
            AndroidJNI.DeleteLocalRef(retVal);
            return new OuyaMod(globalRef);
        }

        public bool isAvailable()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return false;
            }
            if (_jmIsAvailable == IntPtr.Zero)
            {
                Debug.LogError("_jmIsAvailable is not initialized");
                return false;
            }
            return AndroidJNI.CallBooleanMethod(_instance, _jmIsAvailable, new jvalue[0] { });
        }

        public bool isInitialized()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return false;
            }
            if (_jmIsInitialized == IntPtr.Zero)
            {
                Debug.LogError("_jmIsInitialized is not initialized");
                return false;
            }
            return AndroidJNI.CallBooleanMethod(_instance, _jmIsInitialized, new jvalue[0] { });
        }
    }
}

#endif