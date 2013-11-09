using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;

using System;
using System.Collections.Generic;
using System.IO;

namespace tv.ouya.sdk.monogame
{
	public class AsyncCppOuyaSetApplicationKey
	{
		private const string LOG_TAG = "AsyncCppOuyaSetApplicationKey";
		private static IntPtr jc_class = IntPtr.Zero;
		private static IntPtr method_invoke = IntPtr.Zero;

		static AsyncCppOuyaSetApplicationKey()
		{
			try
			{
				jc_class = JNIEnv.FindClass("tv/ouya/sdk/monogame/AsyncCppOuyaSetApplicationKey");
				if (jc_class != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found class");
				} else {
					Log.Info (LOG_TAG, "Failed to find class");
					return;
				}

				Log.Info (LOG_TAG, "Finding invoke method...");
				method_invoke = JNIEnv.GetStaticMethodID (jc_class, "invoke", "([B)V");
				if (method_invoke != IntPtr.Zero) {
					Log.Info (LOG_TAG, "Found invoke method");
				} else {
					Log.Info (LOG_TAG, "Failed to find invoke method");
				}
			}
			catch (System.Exception) {
				Log.Info (LOG_TAG, "Exception finding class");
			}
		}

		public static void invoke(Context context) {

			if (jc_class != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found class");
			} else {
				Log.Info (LOG_TAG, "Failed to find class");
				return;
			}
			if (method_invoke != IntPtr.Zero) {
				Log.Info (LOG_TAG, "Found invoke method");
			} else {
				Log.Info (LOG_TAG, "Failed to find invoke method");
				return;
			}

			// Load the key from the resources
			byte[] applicationKey = null;
			var resId = context.Resources.GetIdentifier("key", "raw", context.PackageName);
			using (var stream = context.Resources.OpenRawResource(resId))
			{
				using (var ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					applicationKey = ms.ToArray();
				}
			}

			JValue byteArray = new JValue (Java.Lang.Object.FromArray (applicationKey));
			JNIEnv.CallStaticVoidMethod (jc_class, method_invoke, byteArray);
		}
	}
}