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
	public class OuyaController
	{
		private const string LOG_TAG = "OuyaController";
		private static IntPtr jc_class = IntPtr.Zero;
		private static IntPtr method_init = IntPtr.Zero;

		static OuyaController()
		{
			try
			{
				jc_class = JNIEnv.FindClass("tv/ouya/console/api/OuyaController");
				if (jc_class != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found class");
				} else {
					Log.Info (LOG_TAG, "Failed to find class");
					return;
				}

				Log.Info (LOG_TAG, "Finding init method...");
				method_init = JNIEnv.GetStaticMethodID (jc_class, "init", "(Landroid/content/Context;)V");
				if (method_init != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found init method");
				} else {
					Log.Info (LOG_TAG, "Failed to find init method");
				}
			}
			catch (System.Exception) {
				Log.Info (LOG_TAG, "Exception finding class");
			}
		}

		public static void init(Context context) {
			if (jc_class != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found class");
			} else {
				Log.Info (LOG_TAG, "Failed to find class");
				return;
			}
			if (method_init != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found init method");
			} else {
				Log.Info (LOG_TAG, "Failed to find init method");
				return;
			}

			JNIEnv.CallStaticVoidMethod (jc_class, method_init, new JValue(context));
		}
	}
}