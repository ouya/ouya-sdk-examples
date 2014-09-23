/*
 * Copyright (C) 2012-2014 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*
 * 
 * 

public class tv.ouya.sdk.OuyaInputView extends android.view.View{
public tv.ouya.sdk.OuyaInputView(android.app.Activity, android.content.Context);
  Signature: (Landroid/app/Activity;Landroid/content/Context;)V
public void shutdown();
  Signature: ()V
public native void dispatchGenericMotionEventNative(int, int, float);
  Signature: (IIF)V
public native void dispatchKeyEventNative(int, int, int);
  Signature: (III)V
public boolean dispatchKeyEvent(android.view.KeyEvent);
  Signature: (Landroid/view/KeyEvent;)Z
public boolean dispatchGenericMotionEvent(android.view.MotionEvent);
  Signature: (Landroid/view/MotionEvent;)Z
public boolean onGenericMotionEvent(android.view.MotionEvent);
  Signature: (Landroid/view/MotionEvent;)Z
public boolean onKeyUp(int, android.view.KeyEvent);
  Signature: (ILandroid/view/KeyEvent;)Z
public boolean onKeyDown(int, android.view.KeyEvent);
  Signature: (ILandroid/view/KeyEvent;)Z
static {};
  Signature: ()V
}

 * 
 * 
 */


using Android.Runtime;
using Android.Util;
using System;

namespace OuyaSdk
{
    public class OuyaInputView
    {
        private const string LOG_TAG = "OuyaInputViewCS";

        private static IntPtr _jcOuyaInputView = IntPtr.Zero;
        private static IntPtr _jmConstruct = IntPtr.Zero;
        private IntPtr _instance = IntPtr.Zero;

        static OuyaInputView()
        {
            try
            {
                {
                    string strName = "tv/ouya/sdk/OuyaInputView";
                    _jcOuyaInputView = JNIEnv.FindClass(strName);
                    if (_jcOuyaInputView != IntPtr.Zero)
                    {
                        Log.Info(LOG_TAG, string.Format("Found {0} class", strName));
                        _jcOuyaInputView = JNIEnv.NewGlobalRef(_jcOuyaInputView);
                    }
                    else
                    {
                        Log.Error(LOG_TAG, string.Format("Failed to find {0} class", strName));
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LOG_TAG, string.Format("Exception loading JNI - {0}", ex));
            }
        }

        private static void InitJNI()
        {
            try
            {
                {
                    string strMethod = "<init>";
                    _jmConstruct = JNIEnv.GetMethodID(_jcOuyaInputView, strMethod, "(Landroid/app/Activity;Landroid/content/Context;)V");
                    if (_jmConstruct != IntPtr.Zero)
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

        public OuyaInputView(IntPtr activity, IntPtr context)
        {
            InitJNI();

            if (_jcOuyaInputView == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jcOuyaInputView is not initialized");
                return;
            }
            else
            {
                Log.Info(LOG_TAG, "_jcOuyaInputView is valid");
            }

            if (_jmConstruct == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "_jmConstruct is not initialized");
                return;
            }
            else
            {
                Log.Info(LOG_TAG, "_jmConstruct is valid");
            }

            if (activity == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "activity is not initialized");
                return;
            }
            else
            {
                Log.Info(LOG_TAG, "activity is valid");
            }

            if (context == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "context is not initialized");
                return;
            }
            else
            {
                Log.Info(LOG_TAG, "context is valid");
            }

            _instance = JNIEnv.AllocObject(_jcOuyaInputView);

            if (_instance == IntPtr.Zero)
            {
                Log.Error(LOG_TAG, "Failed to allocate OuyaInputView");
                return;
            }

            JNIEnv.CallVoidMethod(_instance, _jmConstruct, new JValue(activity), new JValue(context));

            _instance = JNIEnv.NewGlobalRef(_instance);
        }

        public IntPtr Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
    }
}

