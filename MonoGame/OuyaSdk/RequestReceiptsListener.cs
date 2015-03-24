using Android.Runtime;
using Android.Util;
using System;

namespace TV.Ouya.Sdk{
	public partial class RequestReceiptsListener
	{
		static Delegate cb_onSuccess_Ljava_util_Collection_;
		#pragma warning disable 0169
		static Delegate GetOnSuccess_Ljava_util_Collection_Handler ()
		{
			if (cb_onSuccess_Ljava_util_Collection_ == null)
				cb_onSuccess_Ljava_util_Collection_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnSuccess_Ljava_util_Collection_);
			return cb_onSuccess_Ljava_util_Collection_;
		}

		static void n_OnSuccess_Ljava_util_Collection_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			global::TV.Ouya.Sdk.RequestReceiptsListener __this = global::Java.Lang.Object.GetObject<global::TV.Ouya.Sdk.RequestReceiptsListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Java.Lang.Object p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (native_p0, JniHandleOwnership.DoNotTransfer);
			__this.OnSuccess (p0);
		}
		#pragma warning restore 0169

		static IntPtr id_onSuccess_Ljava_util_Collection_;
		// Metadata.xml XPath method reference: path="/api/package[@name='tv.ouya.sdk']/class[@name='RequestReceiptsListener']/method[@name='onSuccess' and count(parameter)=1 and parameter[1][@type='java.util.Collection']]"
		[Register ("onSuccess", "(Ljava/util/Collection;)V", "GetOnSuccess_Ljava_util_Collection_Handler")]
		public virtual void OnSuccess (global::Java.Lang.Object p0)
		{
			if (id_onSuccess_Ljava_util_Collection_ == IntPtr.Zero)
				id_onSuccess_Ljava_util_Collection_ = JNIEnv.GetMethodID (class_ref, "onSuccess", "(Ljava/util/Collection;)V");

			if (GetType () == ThresholdType)
				JNIEnv.CallVoidMethod  (Handle, id_onSuccess_Ljava_util_Collection_, new JValue (p0));
			else
				JNIEnv.CallNonvirtualVoidMethod  (Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onSuccess", "(Ljava/util/Collection;)V"), new JValue (p0));
		}
	}
}
