//#define VERBOSE_LOGGING
#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Java.IO.InputStream
{
    public class InputStream : IDisposable
    {
        private static IntPtr _jcInputStream = IntPtr.Zero;
        private static IntPtr _jmClose = IntPtr.Zero;
        private static IntPtr _jmRead = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static InputStream()
        {
            try
            {
                {
                    string strName = "java/io/InputStream";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcInputStream = AndroidJNI.NewGlobalRef(localRef);
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
                _jmClose = AndroidJNI.GetMethodID(_jcInputStream, strMethod, "()V");
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
                string strMethod = "read";
                _jmRead = AndroidJNI.GetMethodID(_jcInputStream, strMethod, "([B)I");
                if (_jmRead != IntPtr.Zero)
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

        public InputStream(IntPtr instance)
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

        public int read(ref byte[] bytes)
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return - 1;
            }
            if (_jmRead == IntPtr.Zero)
            {
                Debug.LogError("_jmRead is not initialized");
                return -1;
            }
            IntPtr arg1 = AndroidJNI.ToByteArray(bytes);
            int result = AndroidJNI.CallIntMethod(_instance, _jmRead, new jvalue[] { new jvalue() { l = arg1 } });
            byte[] copy = AndroidJNI.FromByteArray(arg1);
            Array.Copy(copy, bytes, copy.Length);
            AndroidJNI.DeleteLocalRef(arg1);

            return result;
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