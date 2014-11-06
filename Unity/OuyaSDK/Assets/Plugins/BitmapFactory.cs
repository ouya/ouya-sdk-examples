//#define VERBOSE_LOGGING
#if UNITY_ANDROID && !UNITY_EDITOR

using java.io;
using System;
using System.IO;
using UnityEngine;

namespace Android.Graphics
{
    public class BitmapFactory
    {
        static IntPtr _jcBitmapFactory = IntPtr.Zero;
        static IntPtr _jmDecodeByteArray = IntPtr.Zero;

        static BitmapFactory()
        {
            try
            {
                {
                    string strName = "android/graphics/BitmapFactory";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcBitmapFactory = AndroidJNI.NewGlobalRef(localRef);
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
            try
            {
                {
                    string strMethod = "decodeByteArray";
                    _jmDecodeByteArray = AndroidJNI.GetStaticMethodID(_jcBitmapFactory, strMethod, "([BII)Landroid/graphics/Bitmap;");
                    if (_jmDecodeByteArray != IntPtr.Zero)
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

        public static Bitmap decodeByteArray(byte[] buffer, int offset, int length)
        {
            JNIFind();

            if (_jcBitmapFactory == IntPtr.Zero)
            {
                Debug.LogError("_jcBitmapFactory is not initialized");
                return null;
            }
            if (_jmDecodeByteArray == IntPtr.Zero)
            {
                Debug.LogError("_jmDecodeByteArray is not initialized");
                return null;
            }
            IntPtr arg1 = AndroidJNI.ToByteArray(buffer);
            IntPtr retVal = AndroidJNI.CallStaticObjectMethod(_jcBitmapFactory, _jmDecodeByteArray, new jvalue[] { new jvalue() { l = arg1 }, new jvalue() { i = offset }, new jvalue() { i = length } });
            AndroidJNI.DeleteLocalRef(arg1);

            if (retVal == IntPtr.Zero)
            {
                Debug.LogError("decodeByteArray returned null bitmap");
                return null;
            }

            IntPtr globalRef = AndroidJNI.NewGlobalRef(retVal);
            AndroidJNI.DeleteLocalRef(retVal);

            Bitmap result = new Bitmap(globalRef);
            return result;
        }
    }
}

#endif