//#define VERBOSE_LOGGING
#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;

namespace org.json
{
    public class JSONArray : IDisposable
    {
        private const string LOG_TAG = "JSONObject";

        private static IntPtr _jcJsonArray = IntPtr.Zero;
        private static IntPtr _jmConstructor = IntPtr.Zero;
        private static IntPtr _jmConstructor2 = IntPtr.Zero;
        private static IntPtr _jmGetInt;
        private static IntPtr _jmGetJsonObject;
        private static IntPtr _jmGetString;
        private static IntPtr _jmLength;
        private static IntPtr _jmPut;
        private static IntPtr _jmToString;
        private IntPtr _instance = IntPtr.Zero;

        static JSONArray()
        {
            try
            {
                {
                    string strName = "org/json/JSONArray";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcJsonArray = AndroidJNI.NewGlobalRef(localRef);
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
                    _jmConstructor = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "()V");
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
                    string strMethod = "<init>";
                    _jmConstructor2 = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(Ljava/lang/String;)V");
                    if (_jmConstructor2 != IntPtr.Zero)
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
                    string strMethod = "getJSONObject";
                    _jmGetJsonObject = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(I)Lorg/json/JSONObject;");
                    if (_jmGetJsonObject != IntPtr.Zero)
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
                    string strMethod = "getInt";
                    _jmGetInt = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(I)I");
                    if (_jmGetInt != IntPtr.Zero)
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
                    string strMethod = "getString";
                    _jmGetString = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(I)Ljava/lang/String;");
                    if (_jmGetString != IntPtr.Zero)
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
                    string strMethod = "length";
                    _jmLength = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "()I");
                    if (_jmLength != IntPtr.Zero)
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
                    string strMethod = "put";
                    _jmPut = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "(ILjava/lang/Object;)Lorg/json/JSONArray;");
                    if (_jmPut != IntPtr.Zero)
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
                    string strMethod = "toString";
                    _jmToString = AndroidJNI.GetMethodID(_jcJsonArray, strMethod, "()Ljava/lang/String;");
                    if (_jmLength != IntPtr.Zero)
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

        public JSONArray(IntPtr instance)
        {
            _instance = instance;
        }

        public IntPtr GetInstance()
        {
            return _instance;
        }

        public JSONArray()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonArray is not initialized");
                return;
            }
            if (_jmConstructor == IntPtr.Zero)
            {
                Debug.LogError("_jmConstructor is not initialized");
                return;
            }

            _instance = AndroidJNI.AllocObject(_jcJsonArray);
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("Failed to allocate JSONArray");
                return;
            }

            AndroidJNI.CallVoidMethod(_instance, _jmConstructor, new jvalue[0]);
        }

        public JSONArray(string buffer)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonArray is not initialized");
                return;
            }
            if (_jmConstructor2 == IntPtr.Zero)
            {
                Debug.LogError("_jmConstructor2 is not initialized");
                return;
            }

            _instance = AndroidJNI.AllocObject(_jcJsonArray);
            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("Failed to allocate JSONArray");
                return;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(buffer);
            AndroidJNI.CallVoidMethod(_instance, _jmConstructor2, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public void Dispose()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            if (_instance != IntPtr.Zero)
            {
                AndroidJNI.DeleteLocalRef(_instance);
                _instance = IntPtr.Zero;
            }
        }

        public int length()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return 0;
            }
            if (_jmLength == IntPtr.Zero)
            {
                Debug.LogError("_jmLength is not initialized");
                return 0;
            }

            int result = AndroidJNI.CallIntMethod(_instance, _jmLength, new jvalue[0]);
            return result;
        }

        public org.json.JSONObject getJSONObject(int index)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetJsonObject == IntPtr.Zero)
            {
                Debug.LogError("_jmGetJsonObject is not initialized");
                return null;
            }

            int arg1 = index;
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetJsonObject, new jvalue[] { new jvalue() { i = arg1 } });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get JSONObject");
                return null;
            }

            org.json.JSONObject retVal = new JSONObject(result);
            return retVal;
        }

        public int getInt(int index)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return 0;
            }
            if (_jmGetInt == IntPtr.Zero)
            {
                Debug.LogError("_jmGetInt is not initialized");
                return 0;
            }

            int arg1 = index;
            int result = AndroidJNI.CallIntMethod(_instance, _jmGetInt, new jvalue[] { new jvalue() { i = arg1 } });
            return result;
        }

        public string getString(int index)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmGetString == IntPtr.Zero)
            {
                Debug.LogError("_jmGetString is not initialized");
                return null;
            }

            int arg1 = index;
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetString, new jvalue[] { new jvalue() { i = arg1 } });

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to get String");
                return null;
            }

            return AndroidJNI.GetStringUTFChars(result);
        }

        public org.json.JSONArray put(int index, String value)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmPut == IntPtr.Zero)
            {
                Debug.LogError("_jmPut is not initialized");
                return null;
            }

            int arg1 = index;
            IntPtr arg2 = AndroidJNI.NewStringUTF(value);
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmPut, new jvalue[] { new jvalue() { i = arg1 }, new jvalue() { l = arg2 } });
            AndroidJNI.DeleteLocalRef(arg2);

            return this;
        }

        public org.json.JSONArray put(int index, JSONObject jsonObject)
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmPut == IntPtr.Zero)
            {
                Debug.LogError("_jmPut is not initialized");
                return null;
            }

            if (null == jsonObject)
            {
                Debug.LogError("jsonObject is null");
                return null;
            }

            if (jsonObject.GetInstance() == IntPtr.Zero)
            {
                Debug.LogError("jsonObject instance is not initialized");
                return null;
            }

            int arg1 = index;
            IntPtr arg2 = jsonObject.GetInstance();
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmPut, new jvalue[] { new jvalue() { i = arg1 }, new jvalue() { l = arg2 } });

            return this;
        }

        public string toString()
        {
#if VERBOSE_LOGGING
            Debug.Log(MethodBase.GetCurrentMethod().Name);
#endif
            JNIFind();
            if (_jcJsonArray == IntPtr.Zero)
            {
                Debug.LogError("_jcJsonObject is not initialized");
                return null;
            }
            if (_jmToString == IntPtr.Zero)
            {
                Debug.LogError("_jmToString is not initialized");
                return null;
            }

            IntPtr retVal = AndroidJNI.CallObjectMethod(_instance, _jmToString, new jvalue[0]);

            if (retVal == IntPtr.Zero)
            {
                Debug.LogError("Failed to get String");
                return null;
            }

            String result = AndroidJNI.GetStringUTFChars(retVal);

            AndroidJNI.DeleteLocalRef(retVal);

            return result;
        }
    }
}

#endif