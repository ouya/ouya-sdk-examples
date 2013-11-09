using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace tv.ouya.sdk.monogame
{
	public class AsyncCppOuyaSetActivity
	{
		private const string LOG_TAG = "AsyncCppOuyaSetActivity";
		private static IntPtr jc_class = IntPtr.Zero;
		private static IntPtr method_invoke = IntPtr.Zero;

		static AsyncCppOuyaSetActivity()
		{
			try
			{
				jc_class = JNIEnv.FindClass("tv/ouya/sdk/monogame/AsyncCppOuyaSetActivity");
				if (jc_class != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found class");
				} else {
					Log.Info (LOG_TAG, "Failed to find class");
					return;
				}

				Log.Info (LOG_TAG, "Finding invoke method...");
				method_invoke = JNIEnv.GetStaticMethodID (jc_class, "invoke", "(Landroid/app/Activity;)V");
				if (method_invoke != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found invoke method");
				} else {
					Log.Info (LOG_TAG, "Failed to find invoke method");
				}
			}
			catch (System.Exception) {
				Log.Info (LOG_TAG, "Exception finding AsyncCppOuyaSetDeveloperId class");
			}
		}

		public static void invoke(Activity activity) {
			if (jc_class != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found class");
			} else {
				Log.Info (LOG_TAG, "Failed to find class");
				return;
			}
			if (method_invoke != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found invoke method");

				JNIEnv.CallStaticVoidMethod (jc_class, method_invoke, new JValue(activity));
			} else {
				Log.Info (LOG_TAG, "Failed to find invoke method");
				return;
			}
		}
	}
}