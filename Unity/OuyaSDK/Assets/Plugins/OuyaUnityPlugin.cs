//#define VERBOSE_LOGGING

using System.Reflection;
#if UNITY_ANDROID && !UNITY_EDITOR
using Android.Graphics;
using System;
using System.Collections.Generic;
using tv.ouya.console.api.content;
using UnityEngine;

namespace tv.ouya.sdk
{
    public class OuyaUnityPlugin
    {
        private const string LOG_TAG = "OuyaUnityPlugin";
        private static IntPtr _jcOuyaUnityPlugin = IntPtr.Zero;
        private static IntPtr _jmConstructor = IntPtr.Zero;
        private static IntPtr _jmInitOuyaPlugin = IntPtr.Zero;
        private static IntPtr _jmGetGameData = IntPtr.Zero;
        private static IntPtr _jmPutGameData = IntPtr.Zero;
        private static IntPtr _jmRequestGamerInfo = IntPtr.Zero;
        private static IntPtr _jmRequestProducts = IntPtr.Zero;
        private static IntPtr _jmRequestPurchase = IntPtr.Zero;
        private static IntPtr _jmRequestReceipts = IntPtr.Zero;
        private static IntPtr _jmIsRunningOnOUYASupportedHardware = IntPtr.Zero;
        private static IntPtr _jmSetSafeArea = IntPtr.Zero;
        private static IntPtr _jmClearFocus = IntPtr.Zero;
        private static IntPtr _jmGetOuyaContent = IntPtr.Zero;
        private static IntPtr _jmSaveOuyaMod = IntPtr.Zero;
        private static IntPtr _jmGetOuyaContentInstalled = IntPtr.Zero;
        private static IntPtr _jmGetOuyaContentInstalledResults = IntPtr.Zero;
        private static IntPtr _jmGetOuyaContentPublished = IntPtr.Zero;
        private static IntPtr _jmGetOuyaContentPublishedResults = IntPtr.Zero;
        private static IntPtr _jmContentDelete = IntPtr.Zero;
        private static IntPtr _jmContentDownload = IntPtr.Zero;
        private static IntPtr _jmContentPublish = IntPtr.Zero;
        private static IntPtr _jmContentUnpublish = IntPtr.Zero;
        private static IntPtr _jmGetFloat = IntPtr.Zero;
        private static IntPtr _jmGetBitmapArray = IntPtr.Zero;
        private static IntPtr _jmGetOuyaModScreenshotArray = IntPtr.Zero;
        private static IntPtr _jmGetStringArray = IntPtr.Zero;
        private static IntPtr _jmGetStringResource = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        /// <summary>
        /// Make one request at a time
        /// </summary>
        public static bool m_pendingRequestGamerInfo = false;

        /// <summary>
        /// Make one request at a time
        /// </summary>
        public static bool m_pendingRequestProducts = false;

        /// <summary>
        /// Make one request at a time
        /// </summary>
        public static bool m_pendingRequestPurchase = false;

        /// <summary>
        /// Make one request at a time
        /// </summary>
        public static bool m_pendingRequestReceipts = false;

        static OuyaUnityPlugin()
        {
            try
            {
                {
                    string strName = "tv/ouya/sdk/OuyaUnityPlugin";
                    IntPtr localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcOuyaUnityPlugin = AndroidJNI.NewGlobalRef(localRef);
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
                    _jmConstructor = AndroidJNI.GetMethodID(_jcOuyaUnityPlugin, strMethod, "(Landroid/app/Activity;)V");
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
                    string strMethod = "initOuyaPlugin";
                    _jmInitOuyaPlugin = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)V");
                    if (_jmInitOuyaPlugin != IntPtr.Zero)
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
                    string strMethod = "getGameData";
                    _jmGetGameData = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
                    if (_jmGetGameData != IntPtr.Zero)
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
                    string strMethod = "putGameData";
                    _jmPutGameData = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;Ljava/lang/String;)V");
                    if (_jmPutGameData != IntPtr.Zero)
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
                    string strMethod = "requestGamerInfo";
                    _jmRequestGamerInfo = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmRequestGamerInfo != IntPtr.Zero)
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
                    string strMethod = "requestProducts";
                    _jmRequestProducts = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)V");
                    if (_jmRequestProducts != IntPtr.Zero)
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
                    string strMethod = "requestPurchase";
                    _jmRequestPurchase = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)V");
                    if (_jmRequestPurchase != IntPtr.Zero)
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
                    string strMethod = "requestReceipts";
                    _jmRequestReceipts = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmRequestReceipts != IntPtr.Zero)
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
                    string strMethod = "isRunningOnOUYASupportedHardware";
                    _jmIsRunningOnOUYASupportedHardware = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()Z");
                    if (_jmIsRunningOnOUYASupportedHardware != IntPtr.Zero)
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
                    string strMethod = "setSafeArea";
                    _jmSetSafeArea = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(F)V");
                    if (_jmSetSafeArea != IntPtr.Zero)
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
                    string strMethod = "clearFocus";
                    _jmClearFocus = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmClearFocus != IntPtr.Zero)
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
                    string strMethod = "getOuyaContent";
                    _jmGetOuyaContent = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()Ltv/ouya/console/api/content/OuyaContent;");
                    if (_jmGetOuyaContent != IntPtr.Zero)
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
                    string strMethod = "saveOuyaMod";
                    _jmSaveOuyaMod = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;Ltv/ouya/console/api/content/OuyaMod$Editor;)V");
                    if (_jmSaveOuyaMod != IntPtr.Zero)
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
                    string strMethod = "getOuyaContentInstalled";
                    _jmGetOuyaContentInstalled = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()V");
                    if (_jmGetOuyaContentInstalled != IntPtr.Zero)
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
                    string strMethod = "getOuyaContentInstalledResults";
                    _jmGetOuyaContentInstalledResults = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()[Ltv/ouya/console/api/content/OuyaMod;");
                    if (_jmGetOuyaContentInstalledResults != IntPtr.Zero)
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
                    string strMethod = "getOuyaContentPublished";
                    _jmGetOuyaContentPublished = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)V");
                    if (_jmGetOuyaContentPublished != IntPtr.Zero)
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
                    string strMethod = "getOuyaContentPublishedResults";
                    _jmGetOuyaContentPublishedResults = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "()[Ltv/ouya/console/api/content/OuyaMod;");
                    if (_jmGetOuyaContentPublishedResults != IntPtr.Zero)
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
                    string strMethod = "contentDelete";
                    _jmContentDelete = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
                    if (_jmContentDelete != IntPtr.Zero)
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
                    string strMethod = "contentDownload";
                    _jmContentDownload = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
                    if (_jmContentDownload != IntPtr.Zero)
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
                    string strMethod = "contentPublish";
                    _jmContentPublish = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
                    if (_jmContentPublish != IntPtr.Zero)
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
                    string strMethod = "contentUnpublish";
                    _jmContentUnpublish = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ltv/ouya/console/api/content/OuyaMod;)V");
                    if (_jmContentUnpublish != IntPtr.Zero)
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
                    string strMethod = "getFloat";
                    _jmGetFloat = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/Float;)F");
                    if (_jmGetFloat != IntPtr.Zero)
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
                    string strMethod = "getBitmapArray";
                    _jmGetBitmapArray = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/util/List;)[Landroid/graphics/Bitmap;");
                    if (_jmGetBitmapArray != IntPtr.Zero)
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
                    string strMethod = "getOuyaModScreenshotArray";
                    _jmGetOuyaModScreenshotArray = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/util/List;)[Ltv/ouya/console/api/content/OuyaModScreenshot;");
                    if (_jmGetOuyaModScreenshotArray != IntPtr.Zero)
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
                    string strMethod = "getStringArray";
                    _jmGetStringArray = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/util/List;)[Ljava/lang/String;");
                    if (_jmGetStringArray != IntPtr.Zero)
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
                    string strMethod = "getStringResource";
                    _jmGetStringResource = AndroidJNI.GetStaticMethodID(_jcOuyaUnityPlugin, strMethod, "(Ljava/lang/String;)Ljava/lang/String;");
                    if (_jmGetStringResource != IntPtr.Zero)
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

        public OuyaUnityPlugin(IntPtr currentActivity)
        {
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmConstructor == IntPtr.Zero)
            {
                Debug.LogError("_jmConstructor is not initialized");
                return;
            }
            _instance = AndroidJNI.NewObject(_jcOuyaUnityPlugin, _jmConstructor, new jvalue[] { new jvalue() { l = currentActivity } });
        }

        public static void initOuyaPlugin(string jsonData)
        {
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmInitOuyaPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jmInitOuyaPlugin is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(jsonData);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmInitOuyaPlugin, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static string getGameData(string key)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetGameData == IntPtr.Zero)
            {
                Debug.LogError("_jmGetGameData is not initialized");
                return null;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(key);
            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetGameData, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);

            if (result == IntPtr.Zero)
            {
                Debug.LogError("Failed to getGameData");
                return null;
            }

            String retVal = AndroidJNI.GetStringUTFChars(result);
            return retVal;
        }

        public static void putGameData(string key, string val)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmPutGameData == IntPtr.Zero)
            {
                Debug.LogError("_jmPutGameData is not initialized");
                return;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(key);
            IntPtr arg2 = AndroidJNI.NewStringUTF(val);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmPutGameData, new jvalue[] { new jvalue() { l = arg1 }, new jvalue() { l = arg2 } });
            AndroidJNI.DeleteLocalRef(arg1);
            AndroidJNI.DeleteLocalRef(arg2);
        }

        public static void requestGamerInfo()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmRequestGamerInfo == IntPtr.Zero)
            {
                Debug.LogError("_jmRequestGamerInfo is not initialized");
                return;
            }

            // Make one request at a time
            if (m_pendingRequestGamerInfo)
            {
                return;
            }
            m_pendingRequestGamerInfo = true;

            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmRequestGamerInfo, new jvalue[0]);
        }

        public static void requestProducts(string jsonData)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmRequestProducts == IntPtr.Zero)
            {
                Debug.LogError("_jmGetProductsAsync is not initialized");
                return;
            }

            // Make one request at a time
            if (m_pendingRequestProducts)
            {
                return;
            }
            m_pendingRequestProducts = true;

            IntPtr arg1 = AndroidJNI.NewStringUTF(jsonData);
            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmRequestProducts, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static void requestPurchase(string identifier)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmRequestPurchase == IntPtr.Zero)
            {
                Debug.LogError("_jmRequestPurchase is not initialized");
                return;
            }
            
            // Make one request at a time
            if (m_pendingRequestPurchase)
            {
                return;
            }
            m_pendingRequestPurchase = true;

            IntPtr arg1 = AndroidJNI.NewStringUTF(identifier);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmRequestPurchase, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static void requestReceipts()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmRequestReceipts == IntPtr.Zero)
            {
                Debug.LogError("_jmRequestReceipts is not initialized");
                return;
            }

            // Make one request at a time
            if (m_pendingRequestReceipts)
            {
                return;
            }
            m_pendingRequestReceipts = true;

            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmRequestReceipts, new jvalue[0]);
        }

        public static bool isRunningOnOUYASupportedHardware()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return false;
            }
            if (_jmIsRunningOnOUYASupportedHardware == IntPtr.Zero)
            {
                Debug.LogError("_jmIsRunningOnOUYASupportedHardware is not initialized");
                return false;
            }
            return AndroidJNI.CallStaticBooleanMethod(_jcOuyaUnityPlugin, _jmIsRunningOnOUYASupportedHardware, new jvalue[0]);
        }

        public static void setSafeArea(float percentage)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmSetSafeArea == IntPtr.Zero)
            {
                Debug.LogError("_jmSetSafeArea is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmSetSafeArea, new jvalue[1] {new jvalue() {f = percentage}});
        }

        public static void clearFocus()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmClearFocus == IntPtr.Zero)
            {
                Debug.LogError("_jmClearFocus is not initialized");
                return;
            }
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmClearFocus, new jvalue[0] { });
        }

        public static OuyaContent getOuyaContent()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetOuyaContent == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContent is not initialized");
                return null;
            }
            IntPtr retVal = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetOuyaContent, new jvalue[0] { });
            if (retVal == IntPtr.Zero)
            {
                Debug.LogError("GetOuyaContent returned null");
                return null;
            }
            IntPtr globalPtr = AndroidJNI.NewGlobalRef(retVal);
            AndroidJNI.DeleteLocalRef(retVal);
            return new OuyaContent(globalPtr);
        }

        public static void saveOuyaMod(OuyaMod ouyaMod, OuyaMod.Editor editor)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmSaveOuyaMod == IntPtr.Zero)
            {
                Debug.LogError("_jmSaveOuyaMod is not initialized");
                return;
            }

            IntPtr arg1 = ouyaMod.GetInstance();
            IntPtr arg2 = editor.GetInstance();
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmSaveOuyaMod, new jvalue[] { new jvalue() { l = arg1 }, new jvalue() { l = arg2 } });
        }

        public static void getOuyaContentInstalled()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmGetOuyaContentInstalled == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContentInstalled is not initialized");
                return;
            }

            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmGetOuyaContentInstalled, new jvalue[0] { });
        }

        public static List<OuyaMod> getOuyaContentInstalledResults()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetOuyaContentInstalledResults == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContentInstalledResults is not initialized");
                return null;
            }

            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetOuyaContentInstalledResults, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContentInstalledResults returned null");
                return null;
            }

            List<OuyaMod> ouyaMods = new List<OuyaMod>();
            IntPtr[] resultArray = AndroidJNI.FromObjectArray(result);
            foreach (IntPtr ptr in resultArray)
            {
                IntPtr globalRef = AndroidJNI.NewGlobalRef(ptr);
                AndroidJNI.DeleteLocalRef(ptr);
                OuyaMod ouyaMod = new OuyaMod(globalRef);
                ouyaMods.Add(ouyaMod);
            }
            AndroidJNI.DeleteLocalRef(result);
            return ouyaMods;
        }

        public static void getOuyaContentPublished(OuyaContent.SortMethod sortMethod)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmGetOuyaContentPublished == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContentPublished is not initialized");
                return;
            }

            String strSortMethod = sortMethod.ToString();
            //Debug.Log(string.Format("SortMethod={0}", strSortMethod));
            IntPtr arg1 = AndroidJNI.NewStringUTF(strSortMethod);
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmGetOuyaContentPublished, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
        }

        public static List<OuyaMod> getOuyaContentPublishedResults()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetOuyaContentPublishedResults == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContentPublishedResults is not initialized");
                return null;
            }

            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetOuyaContentPublishedResults, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaContentPublishedResults returned null");
                return null;
            }

            List<OuyaMod> ouyaMods = new List<OuyaMod>();
            IntPtr[] resultArray = AndroidJNI.FromObjectArray(result);
            foreach (IntPtr ptr in resultArray)
            {
                IntPtr globalRef = AndroidJNI.NewGlobalRef(ptr);
                AndroidJNI.DeleteLocalRef(ptr);
                OuyaMod ouyaMod = new OuyaMod(globalRef);
                ouyaMods.Add(ouyaMod);
            }
            AndroidJNI.DeleteLocalRef(result);
            return ouyaMods;
        }

        public static void contentDelete(OuyaMod ouyaMod)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmContentDelete == IntPtr.Zero)
            {
                Debug.LogError("_jmContentDelete is not initialized");
                return;
            }

            IntPtr arg1 = ouyaMod.GetInstance();
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmContentDelete, new jvalue[] { new jvalue() { l = arg1 } });
        }

        public static void contentPublish(OuyaMod ouyaMod)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmContentPublish == IntPtr.Zero)
            {
                Debug.LogError("_jmContentPublish is not initialized");
                return;
            }

            IntPtr arg1 = ouyaMod.GetInstance();
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmContentPublish, new jvalue[] { new jvalue() { l = arg1 } });
        }

        public static void contentUnpublish(OuyaMod ouyaMod)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmContentUnpublish == IntPtr.Zero)
            {
                Debug.LogError("_jmContentUnpublish is not initialized");
                return;
            }

            IntPtr arg1 = ouyaMod.GetInstance();
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmContentUnpublish, new jvalue[] { new jvalue() { l = arg1 } });
        }

        public static void contentDownload(OuyaMod ouyaMod)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return;
            }
            if (_jmContentDownload == IntPtr.Zero)
            {
                Debug.LogError("_jmContentDownload is not initialized");
                return;
            }

            IntPtr arg1 = ouyaMod.GetInstance();
            AndroidJNI.CallStaticVoidMethod(_jcOuyaUnityPlugin, _jmContentDownload, new jvalue[] { new jvalue() { l = arg1 } });
        }

        public static float getFloat(IntPtr f)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return 0;
            }
            if (_jmGetFloat == IntPtr.Zero)
            {
                Debug.LogError("_jmGetFloat is not initialized");
                return 0;
            }

            IntPtr arg1 = f;
            return AndroidJNI.CallStaticFloatMethod(_jcOuyaUnityPlugin, _jmGetFloat, new jvalue[] { new jvalue() { l = arg1 } });
        }

        public static List<Bitmap> getBitmapList(IntPtr list)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetBitmapArray == IntPtr.Zero)
            {
                Debug.LogError("_jmGetBitmapArray is not initialized");
                return null;
            }

            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetBitmapArray, new jvalue[] { new jvalue() { l = list } });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetBitmapArray returned null");
                return null;
            }

            List<Bitmap> items = new List<Bitmap>();

#if VERBOSE_LOGGING
            Debug.Log("Invoking AndroidJNI.FromObjectArray...");
#endif
            IntPtr[] resultArray = AndroidJNI.FromObjectArray(result);
#if VERBOSE_LOGGING
            Debug.Log("Invoked AndroidJNI.FromObjectArray.");
#endif

            foreach (IntPtr ptr in resultArray)
            {
#if VERBOSE_LOGGING
                Debug.Log("Found Bitmap making Global Ref...");
#endif
                IntPtr globalRef = AndroidJNI.NewGlobalRef(ptr);
                AndroidJNI.DeleteLocalRef(ptr);
#if VERBOSE_LOGGING
                Debug.Log("Made global ref for Bitmap.");
#endif
                Bitmap item = new Bitmap(globalRef);
#if VERBOSE_LOGGING
                Debug.Log("Deleting old bitmap reference...");
#endif
                items.Add(item);
            }
#if VERBOSE_LOGGING
            Debug.Log("Deleting bitmap list reference...");
#endif
            AndroidJNI.DeleteLocalRef(result);
            return items;
        }

        public static List<OuyaModScreenshot> getOuyaModScreenshotList(IntPtr list)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetOuyaModScreenshotArray == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaModScreenshotArray is not initialized");
                return null;
            }

            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetOuyaModScreenshotArray, new jvalue[] { new jvalue() { l = list } });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetOuyaModScreenshotArray returned null");
                return null;
            }

            List<OuyaModScreenshot> items = new List<OuyaModScreenshot>();

#if VERBOSE_LOGGING
            Debug.Log("Invoking AndroidJNI.FromObjectArray...");
#endif

            IntPtr[] resultArray = AndroidJNI.FromObjectArray(result);
            AndroidJNI.DeleteLocalRef(result);

#if VERBOSE_LOGGING
            Debug.Log("Invoked AndroidJNI.FromObjectArray.");
#endif

            foreach (IntPtr ptr in resultArray)
            {
#if VERBOSE_LOGGING
                Debug.Log("Found Bitmap making Global Ref...");
#endif
                IntPtr globalRef = AndroidJNI.NewGlobalRef(ptr);
                AndroidJNI.DeleteLocalRef(ptr);
#if VERBOSE_LOGGING
                Debug.Log("Made global ref for Bitmap.");
#endif
                OuyaModScreenshot item = new OuyaModScreenshot(globalRef);
#if VERBOSE_LOGGING
                Debug.Log("Deleting old local ref...");
#endif
                items.Add(item);
            }
#if VERBOSE_LOGGING
            Debug.Log("Deleting OuyaModScreenshot list reference...");
#endif
            return items;
        }

        public static List<string> getStringList(IntPtr list)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetStringArray == IntPtr.Zero)
            {
                Debug.LogError("_jmGetStringArray is not initialized");
                return null;
            }

            IntPtr result = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetStringArray, new jvalue[] { new jvalue() { l = list } });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetStringArray returned null");
                return null;
            }

            List<string> items = new List<string>();
            IntPtr[] resultArray = AndroidJNI.FromObjectArray(result);
            AndroidJNI.DeleteLocalRef(result);
            foreach (IntPtr ptr in resultArray)
            {
                string item = AndroidJNI.GetStringUTFChars(ptr);
                AndroidJNI.DeleteLocalRef(ptr);
                items.Add(item);
            }
            return items;
        }

        public static string getStringResource(string key)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_jcOuyaUnityPlugin == IntPtr.Zero)
            {
                Debug.LogError("_jcOuyaUnityPlugin is not initialized");
                return null;
            }
            if (_jmGetStringResource == IntPtr.Zero)
            {
                Debug.LogError("_jmGetStringResource is not initialized");
                return null;
            }

            IntPtr arg1 = AndroidJNI.NewStringUTF(key);
            IntPtr localRef = AndroidJNI.CallStaticObjectMethod(_jcOuyaUnityPlugin, _jmGetStringResource, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
            if (localRef == IntPtr.Zero)
            {
                Debug.LogError("_jmGetStringResource returned null");
                return null;
            }

            string result = AndroidJNI.GetStringUTFChars(localRef);
            AndroidJNI.DeleteLocalRef(localRef);
            return result;
        }

    }
}

#endif