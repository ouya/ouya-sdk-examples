//#define VERBOSE_LOGGING

#if UNITY_ANDROID && !UNITY_EDITOR
using Android.Graphics;
using Java.IO.InputStream;
using Java.IO.OutputStream;
using System;
using System.Collections.Generic;
using System.Reflection;
using tv.ouya.sdk;
using UnityEngine;

namespace tv.ouya.console.api.content
{
    public class OuyaMod : IDisposable
    {
        public class Editor : IDisposable
        {
            private static IntPtr _jcEditor = IntPtr.Zero;
            private static IntPtr _jmAddScreenshot = IntPtr.Zero;
            private static IntPtr _jmRemoveScreenshot = IntPtr.Zero;
            private static IntPtr _jmAddTag = IntPtr.Zero;
            private static IntPtr _jmDeleteFile = IntPtr.Zero;
            private static IntPtr _jmNewFile = IntPtr.Zero;
            private static IntPtr _jmRemoveTag = IntPtr.Zero;
            private static IntPtr _jmSetCategory = IntPtr.Zero;
            private static IntPtr _jmSetDescription = IntPtr.Zero;
            private static IntPtr _jmSetMetadata = IntPtr.Zero;
            private static IntPtr _jmSetTitle = IntPtr.Zero;
            private IntPtr _instance = IntPtr.Zero;

            static Editor()
            {
                try
                {
                    {
                        string strName = "tv/ouya/console/api/content/OuyaMod$Editor";
                        IntPtr localRef = AndroidJNI.FindClass(strName);
                        if (localRef != IntPtr.Zero)
                        {
#if VERBOSE_LOGGING
                            Debug.Log(string.Format("Found {0} class", strName));
#endif
                            _jcEditor = AndroidJNI.NewGlobalRef(localRef);
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
                    string strMethod = "addScreenshot";
                    _jmAddScreenshot = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Landroid/graphics/Bitmap;)Ltv/ouya/console/api/content/OuyaModScreenshot;");
                    if (_jmAddScreenshot != IntPtr.Zero)
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
                    string strMethod = "removeScreenshot";
                    _jmRemoveScreenshot = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ltv/ouya/console/api/content/OuyaModScreenshot;)V");
                    if (_jmRemoveScreenshot != IntPtr.Zero)
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
                    string strMethod = "addTag";
                    _jmAddTag = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmAddTag != IntPtr.Zero)
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
                    string strMethod = "deleteFile";
                    _jmDeleteFile = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmDeleteFile != IntPtr.Zero)
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
                    string strMethod = "newFile";
                    _jmNewFile = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)Ljava/io/OutputStream;");
                    if (_jmNewFile != IntPtr.Zero)
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
                    string strMethod = "removeTag";
                    _jmRemoveTag = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmRemoveTag != IntPtr.Zero)
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
                    string strMethod = "setCategory";
                    _jmSetCategory = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmSetCategory != IntPtr.Zero)
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
                    string strMethod = "setDescription";
                    _jmSetDescription = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmSetDescription != IntPtr.Zero)
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
                    string strMethod = "setMetadata";
                    _jmSetMetadata = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmSetMetadata != IntPtr.Zero)
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
                    string strMethod = "setTitle";
                    _jmSetTitle = AndroidJNI.GetMethodID(_jcEditor, strMethod, "(Ljava/lang/String;)V");
                    if (_jmSetTitle != IntPtr.Zero)
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

            public Editor(IntPtr instance)
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

            public void addScreenshot(Bitmap bitmap)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmAddScreenshot == IntPtr.Zero)
                {
                    Debug.LogError("_jmAddScreenshot is not initialized");
                    return;
                }
                if (null == bitmap)
                {
                    Debug.LogError("bitmap is not initialized");
                    return;
                }
                if (bitmap.GetInstance() == IntPtr.Zero)
                {
                    Debug.LogError("bitmap.Instance is not initialized");
                    return;
                }

                IntPtr arg1 = bitmap.GetInstance();
                IntPtr retVal = AndroidJNI.CallObjectMethod(_instance, _jmAddScreenshot, new jvalue[] { new jvalue() { l = arg1 } });
            }

            public void removeScreenshot(OuyaModScreenshot screenshot)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmRemoveScreenshot == IntPtr.Zero)
                {
                    Debug.LogError("_jmRemoveScreenshot is not initialized");
                    return;
                }
                if (null == screenshot)
                {
                    Debug.LogError("bitmap is not initialized");
                    return;
                }
                if (screenshot.GetInstance() == IntPtr.Zero)
                {
                    Debug.LogError("screenshot.Instance is not initialized");
                    return;
                }

                IntPtr arg1 = screenshot.GetInstance();
                AndroidJNI.CallVoidMethod(_instance, _jmRemoveScreenshot, new jvalue[] { new jvalue() { l = arg1 } });
            }

            public void addTag(string tag)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmAddTag == IntPtr.Zero)
                {
                    Debug.LogError("_jmAddTag is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(tag);
                AndroidJNI.CallVoidMethod(_instance, _jmAddTag, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }

            public void deleteFile(string filename)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmDeleteFile == IntPtr.Zero)
                {
                    Debug.LogError("_jmDeleteFile is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(filename);
                AndroidJNI.CallVoidMethod(_instance, _jmDeleteFile, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }

            public OutputStream newFile(string filename)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return null;
                }
                if (_jmNewFile == IntPtr.Zero)
                {
                    Debug.LogError("_jmNewFile is not initialized");
                    return null;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(filename);
                IntPtr retVal = AndroidJNI.CallObjectMethod(_instance, _jmNewFile, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);

                if (retVal == IntPtr.Zero)
                {
                    Debug.LogError("NewFile returned null");
                    return null;
                }

                IntPtr globalRef = AndroidJNI.NewGlobalRef(retVal);
                AndroidJNI.DeleteLocalRef(retVal);

                return new OutputStream(globalRef);
            }

            public void removeTag(string tag)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmRemoveTag == IntPtr.Zero)
                {
                    Debug.LogError("_jmRemoveTag is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(tag);
                AndroidJNI.CallVoidMethod(_instance, _jmRemoveTag, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }

            public void setCategory(string category)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmSetCategory == IntPtr.Zero)
                {
                    Debug.LogError("_jmSetCategory is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(category);
                AndroidJNI.CallVoidMethod(_instance, _jmSetCategory, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }

            public void setDescription(string description)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmSetDescription == IntPtr.Zero)
                {
                    Debug.LogError("_jmSetDescription is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(description);
                AndroidJNI.CallVoidMethod(_instance, _jmSetDescription, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }

            public void setTitle(string title)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmSetTitle == IntPtr.Zero)
                {
                    Debug.LogError("_jmSetTitle is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(title);
                AndroidJNI.CallVoidMethod(_instance, _jmSetTitle, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }

            public void setMetadata(string metadata)
            {
#if VERBOSE_LOGGING
                Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
                JNIFind();

                if (_instance == IntPtr.Zero)
                {
                    Debug.LogError("_instance is not initialized");
                    return;
                }
                if (_jmSetMetadata == IntPtr.Zero)
                {
                    Debug.LogError("_jmSetMetadata is not initialized");
                    return;
                }

                IntPtr arg1 = AndroidJNI.NewStringUTF(metadata);
                AndroidJNI.CallVoidMethod(_instance, _jmSetMetadata, new jvalue[] { new jvalue() { l = arg1 } });
                AndroidJNI.DeleteLocalRef(arg1);
            }
        }

        private static IntPtr _jcOuyaMod = IntPtr.Zero;
        private static IntPtr _jmGetCategory = IntPtr.Zero;
        private static IntPtr _jmGetDescription = IntPtr.Zero;
        private static IntPtr _jmGetFilenames = IntPtr.Zero;
        private static IntPtr _jmGetMetaData = IntPtr.Zero;
        private static IntPtr _jmGetScreenshots = IntPtr.Zero;
        private static IntPtr _jmGetTags = IntPtr.Zero;
        private static IntPtr _jmGetTitle = IntPtr.Zero;
        private static IntPtr _jmEdit = IntPtr.Zero;
        private static IntPtr _jmFlag = IntPtr.Zero;
        private static IntPtr _jmIsPublished = IntPtr.Zero;
        private static IntPtr _jmIsInstalled = IntPtr.Zero;
        private static IntPtr _jmIsDownloading = IntPtr.Zero;
        private static IntPtr _jmOpenFile = IntPtr.Zero;
        private static IntPtr _jmRate = IntPtr.Zero;
        private static IntPtr _jmGetRatingCount = IntPtr.Zero;
        private static IntPtr _jmGetRatingAverage = IntPtr.Zero;
        private static IntPtr _jmGetUserRating = IntPtr.Zero;
        private static IntPtr _jmIsFlagged = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static OuyaMod()
        {
            try
            {
                IntPtr localRef;
                {
                    string strName = "tv/ouya/console/api/content/OuyaMod";
                    localRef = AndroidJNI.FindClass(strName);
                    if (localRef != IntPtr.Zero)
                    {
#if VERBOSE_LOGGING
                        Debug.Log(string.Format("Found {0} class", strName));
#endif
                        _jcOuyaMod = AndroidJNI.NewGlobalRef(localRef);
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
                string strMethod = "getCategory";
                _jmGetCategory = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/lang/String;");
                if (_jmGetCategory != IntPtr.Zero)
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
                string strMethod = "getDescription";
                _jmGetDescription = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/lang/String;");
                if (_jmGetDescription != IntPtr.Zero)
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
                string strMethod = "getFilenames";
                _jmGetFilenames = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/util/List;");
                if (_jmGetFilenames != IntPtr.Zero)
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
                string strMethod = "getMetaData";
                _jmGetMetaData = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/lang/String;");
                if (_jmGetMetaData != IntPtr.Zero)
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
                string strMethod = "getScreenshots";
                _jmGetScreenshots = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/util/List;");
                if (_jmGetScreenshots != IntPtr.Zero)
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
                string strMethod = "getTags";
                _jmGetTags = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/util/List;");
                if (_jmGetTags != IntPtr.Zero)
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
                string strMethod = "getTitle";
                _jmGetTitle = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/lang/String;");
                if (_jmGetTitle != IntPtr.Zero)
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
                string strMethod = "edit";
                _jmEdit = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ltv/ouya/console/api/content/OuyaMod$Editor;");
                if (_jmEdit != IntPtr.Zero)
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
                string strMethod = "isPublished";
                _jmIsPublished = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Z");
                if (_jmIsPublished != IntPtr.Zero)
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
                string strMethod = "isInstalled";
                _jmIsInstalled = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Z");
                if (_jmIsInstalled != IntPtr.Zero)
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
                string strMethod = "isDownloading";
                _jmIsDownloading = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Z");
                if (_jmIsDownloading != IntPtr.Zero)
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
                string strMethod = "flag";
                _jmFlag = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()V");
                if (_jmFlag != IntPtr.Zero)
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
                string strMethod = "openFile";
                _jmOpenFile = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "(Ljava/lang/String;)Ljava/io/InputStream;");
                if (_jmOpenFile != IntPtr.Zero)
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
                string strMethod = "rate";
                _jmRate = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()V");
                if (_jmRate != IntPtr.Zero)
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
                string strMethod = "getRatingCount";
                _jmGetRatingCount = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()I");
                if (_jmGetRatingCount != IntPtr.Zero)
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
                string strMethod = "getRatingAverage";
                _jmGetRatingAverage = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()F");
                if (_jmGetRatingAverage != IntPtr.Zero)
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
                string strMethod = "getUserRating";
                _jmGetUserRating = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Ljava/lang/Float;");
                if (_jmGetUserRating != IntPtr.Zero)
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
                string strMethod = "isFlagged";
                _jmIsFlagged = AndroidJNI.GetMethodID(_jcOuyaMod, strMethod, "()Z");
                if (_jmIsFlagged != IntPtr.Zero)
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

        public OuyaMod(IntPtr instance)
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

        public string getCategory()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmGetCategory == IntPtr.Zero)
            {
                Debug.LogError("_jmGetCategory is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetCategory, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("GetCategory returned null");
                return null;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            String retVal = AndroidJNI.GetStringUTFChars(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public string getDescription()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmGetDescription == IntPtr.Zero)
            {
                Debug.LogError("_jmGetDescription is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetDescription, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("GetDescription returned null");
                return null;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            String retVal = AndroidJNI.GetStringUTFChars(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public List<string> getFilenames()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            List<string> retVal = new List<string>();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return retVal;
            }
            if (_jmGetFilenames == IntPtr.Zero)
            {
                Debug.LogError("_jmGetFilenames is not initialized");
                return retVal;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetFilenames, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetFilenames returned null");
                return retVal;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            retVal = OuyaUnityPlugin.getStringList(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public string getMetaData()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmGetMetaData == IntPtr.Zero)
            {
                Debug.LogError("_jmGetMetaData is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetMetaData, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                //optional field could be null
                return null;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            String retVal = AndroidJNI.GetStringUTFChars(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public List<OuyaModScreenshot> getScreenshots()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            List<OuyaModScreenshot> retVal = new List<OuyaModScreenshot>();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return retVal;
            }
            if (_jmGetScreenshots == IntPtr.Zero)
            {
                Debug.LogError("_jmGetScreenshots is not initialized");
                return retVal;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetScreenshots, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("_jmGetScreenshots returned null");
                return retVal;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            retVal = OuyaUnityPlugin.getOuyaModScreenshotList(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public List<string> getTags()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            List<string> retVal = new List<string>();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return retVal;
            }
            if (_jmGetTags == IntPtr.Zero)
            {
                Debug.LogError("_jmGetTags is not initialized");
                return retVal;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetTags, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("GetTags returned null");
                return retVal;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            retVal = OuyaUnityPlugin.getStringList(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public string getTitle()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmGetTitle == IntPtr.Zero)
            {
                Debug.LogError("_jmGetTitle is not initialized");
                return null;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetTitle, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                Debug.LogError("GetTitle returned null");
                return null;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            String retVal = AndroidJNI.GetStringUTFChars(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public Editor edit()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmEdit == IntPtr.Zero)
            {
                Debug.LogError("_jmEdit is not initialized");
                return null;
            }
            IntPtr retVal = AndroidJNI.CallObjectMethod(_instance, _jmEdit, new jvalue[0] { });
            if (retVal == IntPtr.Zero)
            {
                Debug.LogError("Edit returned null");
                return null;
            }
#if VERBOSE_LOGGING
            Debug.Log(string.Format("{0} Success", MethodBase.GetCurrentMethod().Name));
#endif
            IntPtr globalRef = AndroidJNI.NewGlobalRef(retVal);
            AndroidJNI.DeleteLocalRef(retVal);
            return new OuyaMod.Editor(globalRef);
        }

        public bool isPublished()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return false;
            }
            if (_jmGetTitle == IntPtr.Zero)
            {
                Debug.LogError("_jmGetTitle is not initialized");
                return false;
            }
            return AndroidJNI.CallBooleanMethod(_instance, _jmIsPublished, new jvalue[0] { });
        }

        public bool isInstalled()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return false;
            }
            if (_jmIsInstalled == IntPtr.Zero)
            {
                Debug.LogError("_jmIsInstalled is not initialized");
                return false;
            }
            return AndroidJNI.CallBooleanMethod(_instance, _jmIsInstalled, new jvalue[0] { });
        }

        public bool isDownloading()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return false;
            }
            if (_jmIsDownloading == IntPtr.Zero)
            {
                Debug.LogError("_jmIsDownloading is not initialized");
                return false;
            }
            return AndroidJNI.CallBooleanMethod(_instance, _jmIsDownloading, new jvalue[0] { });
        }

        public void flag()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }
            if (_jmFlag == IntPtr.Zero)
            {
                Debug.LogError("_jmFlag is not initialized");
                return;
            }
            AndroidJNI.CallVoidMethod(_instance, _jmFlag, new jvalue[0] { });
        }

        public void rate()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return;
            }
            if (_jmRate == IntPtr.Zero)
            {
                Debug.LogError("_jmRate is not initialized");
                return;
            }
            AndroidJNI.CallVoidMethod(_instance, _jmRate, new jvalue[0] { });
        }

        public int getRatingCount()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return 0;
            }
            if (_jmGetRatingCount == IntPtr.Zero)
            {
                Debug.LogError("_jmGetRatingCount is not initialized");
                return 0;
            }
            return AndroidJNI.CallIntMethod(_instance, _jmGetRatingCount, new jvalue[0] { });
        }

        public float getRatingAverage()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return 0;
            }
            if (_jmGetRatingAverage == IntPtr.Zero)
            {
                Debug.LogError("_jmGetRatingAverage is not initialized");
                return 0;
            }
            return AndroidJNI.CallFloatMethod(_instance, _jmGetRatingAverage, new jvalue[0] { });
        }

        public float? getUserRating()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return 0;
            }
            if (_jmGetUserRating == IntPtr.Zero)
            {
                Debug.LogError("_jmGetUserRating is not initialized");
                return 0;
            }
            IntPtr result = AndroidJNI.CallObjectMethod(_instance, _jmGetUserRating, new jvalue[0] { });
            if (result == IntPtr.Zero)
            {
                return null;
            }
            float retVal = OuyaUnityPlugin.getFloat(result);
            AndroidJNI.DeleteLocalRef(result);
            return retVal;
        }

        public bool isFlagged()
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return false;
            }
            if (_jmIsFlagged == IntPtr.Zero)
            {
                Debug.LogError("_jmIsFlagged is not initialized");
                return false;
            }
            return AndroidJNI.CallBooleanMethod(_instance, _jmIsFlagged, new jvalue[0] { });
        }

        public InputStream openFile(string filename)
        {
#if VERBOSE_LOGGING
            Debug.Log(string.Format("Invoking {0}...", MethodBase.GetCurrentMethod().Name));
#endif
            JNIFind();

            if (_instance == IntPtr.Zero)
            {
                Debug.LogError("_instance is not initialized");
                return null;
            }
            if (_jmOpenFile == IntPtr.Zero)
            {
                Debug.LogError("_jmOpenFile is not initialized");
                return null;
            }
            IntPtr arg1 = AndroidJNI.NewStringUTF(filename);
            IntPtr localRef = AndroidJNI.CallObjectMethod(_instance, _jmOpenFile, new jvalue[] { new jvalue() { l = arg1 } });
            AndroidJNI.DeleteLocalRef(arg1);
            if (localRef == IntPtr.Zero)
            {
                Debug.LogError("_jmOpenFile returned null");
                return null;
            }
            IntPtr globalRef = AndroidJNI.NewGlobalRef(localRef);
            AndroidJNI.DeleteLocalRef(localRef);
            return new InputStream(globalRef);
        }
    }
}

#endif