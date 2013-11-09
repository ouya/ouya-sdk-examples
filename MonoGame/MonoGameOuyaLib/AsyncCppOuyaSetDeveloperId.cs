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
	public class AsyncCppOuyaSetDeveloperId
	{
		private const string LOG_TAG = "AsyncCppOuyaSetDeveloperId";
		private static IntPtr jc_class = IntPtr.Zero;
		private static IntPtr method_invoke = IntPtr.Zero;

		static AsyncCppOuyaSetDeveloperId()
		{
			try
			{
				jc_class = JNIEnv.FindClass("tv/ouya/sdk/monogame/AsyncCppOuyaSetDeveloperId");
				if (jc_class != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found AsyncCppOuyaSetDeveloperId class");
				} else {
					Log.Info (LOG_TAG, "Failed to find AsyncCppOuyaSetDeveloperId class");
					return;
				}

				Log.Info (LOG_TAG, "Finding invoke method...");
				method_invoke = JNIEnv.GetStaticMethodID (jc_class, "invoke", "(Ljava/lang/String;)V");
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

		public static void invoke(string developerId) {
			if (jc_class != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found AsyncCppOuyaSetDeveloperId class");
			} else {
				Log.Info (LOG_TAG, "Failed to find AsyncCppOuyaSetDeveloperId class");
				return;
			}
			if (method_invoke != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found invoke method");

				Java.Lang.String developerIdString = new Java.Lang.String(developerId);
				JNIEnv.CallStaticVoidMethod (jc_class, method_invoke, new JValue(developerIdString));
			} else {
				Log.Info (LOG_TAG, "Failed to find invoke method");
				return;
			}
		}
	}
}