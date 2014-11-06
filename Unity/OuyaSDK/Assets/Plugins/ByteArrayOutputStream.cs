//#define VERBOSE_LOGGING

#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using UnityEngine;

namespace java.io
{
    public class ByteArrayOutputStream : IDisposable
    {
        static IntPtr _jcByteArrayOutputStream = IntPtr.Zero;
        static IntPtr _jmClose = IntPtr.Zero;
        static IntPtr _jmConstructor = IntPtr.Zero;
        static IntPtr _jmSize = IntPtr.Zero;
        static IntPtr _jmToByteArray = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static ByteArrayOutputStream()
        {
            try
            {
                {
                    string strName = "java/io/ByteArrayOutputStream";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcByteArrayOutputStream = AndroidJNI.NewGlobalRef(localRef);
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
                    string strMethod = "<init>";
                    _jmConstructor = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()V");
                    if (_jmConstructor != IntPtr.Zero)
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
                    string strMethod = "close";
                    _jmClose = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()V");
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
                    string strMethod = "size";
                    _jmSize = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()I");
                    if (_jmSize != IntPtr.Zero)
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
                    string strMethod = "toByteArray";
                    _jmToByteArray = AndroidJNI.GetMethodID(_jcByteArrayOutputStream, strMethod, "()[B");
                    if (_jmToByteArray != IntPtr.Zero)
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

        public ByteArrayOutputStream()
        {
            JNIFind();

            _instance = AndroidJNI.AllocObject(_jcByteArrayOutputStream);
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("Failed to allocate ByteArrayOutputStream");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmConstructor, new jvalue[0]);

#if VERBOSE_LOGGING
            Debug.Log("Allocated ByteArrayOutputStream");
#endif
        }

        public IntPtr GetInstance()
        {
            return _instance;
        }

        public void Dispose()
        {
            if (_instance != IntPtr.Zero)
            {
                AndroidJNI.DeleteLocalRef(_instance);
                _instance = IntPtr.Zero;
            }
        }

        public void close()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmClose, new jvalue[0]);
        }

        public int size()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return 0;
            }

            int result = AndroidJNI.CallIntMethod(_instance, _jmSize, new jvalue[0]);
#if VERBOSE_LOGGING
            Debug.Log(string.Format("ByteArrayOutputStream.size() == {0}", result));
#endif
            return result;
        }

        public byte[] toByteArray()
        {
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmToByteArray == IntPtr.Zero)
            {
                Debug.LogError("_jmToByteArray is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmToByteArray, new jvalue[0]);
            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get byte array");
                return null;
            }
            int count = AndroidJNI.GetArrayLength(result);
            byte[] retVal = new byte[count];
            for (int index = 0; index < count; ++index)
            {
                retVal[index] = AndroidJNI.GetByteArrayElement(result, index);
            }
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }
    }
}

#endif