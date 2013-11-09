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
	public class AsyncCppOuyaFetchGamerUUID
	{
		private const string LOG_TAG = "AsyncCppOuyaFetchGamerUUID";
		private static IntPtr jc_class = IntPtr.Zero;
		private static IntPtr method_invoke = IntPtr.Zero;

		static AsyncCppOuyaFetchGamerUUID()
		{
			try
			{
				jc_class = JNIEnv.FindClass("tv/ouya/sdk/monogame/AsyncCppOuyaFetchGamerUUID");
				if (jc_class != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found AsyncCppOuyaFetchGamerUUID class");
				} else {
					Log.Info (LOG_TAG, "Failed to find AsyncCppOuyaFetchGamerUUID class");
					return;
				}

				Log.Info (LOG_TAG, "Finding invoke method...");
				method_invoke = JNIEnv.GetStaticMethodID (jc_class, "invoke", "()V");
				if (method_invoke != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found invoke method");
				} else {
					Log.Info (LOG_TAG, "Failed to find invoke method");
				}
			}
			catch (System.Exception) {
				Log.Info (LOG_TAG, "Exception finding AsyncCppOuyaFetchGamerUUID class");
			}
		}

		public static void invoke() {
			if (jc_class != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found AsyncCppOuyaFetchGamerUUID class");
			} else {
				Log.Info (LOG_TAG, "Failed to find AsyncCppOuyaFetchGamerUUID class");
				return;
			}
			if (method_invoke != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found invoke method");
				JNIEnv.CallStaticVoidMethod (jc_class, method_invoke);
			} else {
				Log.Info (LOG_TAG, "Failed to find invoke method");
				return;
			}
		}
	}
}