//#define VERBOSE_LOGGING
#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Java.IO.OutputStream
{
    public class OutputStream : IDisposable
    {
        private static IntPtr _jcOutputStream = IntPtr.Zero;
        private static IntPtr _jmClose = IntPtr.Zero;
        private static IntPtr _jmWrite = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static OutputStream()
        {
            try
            {
                {
                    string strName = "java/io/OutputStream";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcOutputStream = AndroidJNI.NewGlobalRef(localRef);
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
            {
                string strMethod = "close";
                _jmClose = AndroidJNI.GetMethodID(_jcOutputStream, strMethod, "()V");
                if (_jmClose != IntPtr.Zero)
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
                string strMethod = "write";
                _jmWrite = AndroidJNI.GetMethodID(_jcOutputStream, strMethod, "([B)V");
                if (_jmWrite != IntPtr.Zero)
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

        public OutputStream(IntPtr instance)
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

        public void write(byte[] bytes)
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }
            if (_jmWrite == IntPtr.Zero)
            {
                Debug.LogError("_jmWrite is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.ToByteArray(bytes);
            AndroidJNI.CallVoidMethod(_instance, _jmWrite, new jvalue[] { new jvalue() {l = arg1} });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public void close()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }
            if (_jmClose == IntPtr.Zero)
            {
                Debug.LogError("_jmClose is not initialized");
                return;
            }
            AndroidJNI.CallVoidMethod(_instance, _jmClose, new jvalue[0] { });
        }
    }
}

#endif