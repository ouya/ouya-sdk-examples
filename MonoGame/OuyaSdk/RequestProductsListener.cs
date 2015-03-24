using Android.Runtime;
using Android.Util;
using System;

namespace TV.Ouya.Sdk{
	public partial class RequestProductsListener
	{
		static Delegate cb_onSuccess_Ljava_util_List_;
		#pragma warning disable 0169
		static Delegate GetOnSuccess_Ljava_util_List_Handler ()
		{
			if (cb_onSuccess_Ljava_util_List_ == null)
				cb_onSuccess_Ljava_util_List_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnSuccess_Ljava_util_List_);
			return cb_onSuccess_Ljava_util_List_;
		}

		static void n_OnSuccess_Ljava_util_List_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			global::TV.Ouya.Sdk.RequestProductsListener __this = global::Java.Lang.Object.GetObject<global::TV.Ouya.Sdk.RequestProductsListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::Java.Lang.Object p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Object> (native_p0, JniHandleOwnership.DoNotTransfer);
			__this.OnSuccess (p0);
		}
		#pragma warning restore 0169

		static IntPtr id_onSuccess_Ljava_util_List_;
		// Metadata.xml XPath method reference: path="/api/package[@name='tv.ouya.sdk']/class[@name='RequestProductsListener']/method[@name='onSuccess' and count(parameter)=1 and parameter[1][@type='java.util.List']]"
		[Register ("onSuccess", "(Ljava/util/List;)V", "GetOnSuccess_Ljava_util_List_Handler")]
		public override void OnSuccess (global::Java.Lang.Object p0)
		{
			if (id_onSuccess_Ljava_util_List_ == IntPtr.Zero)
				id_onSuccess_Ljava_util_List_ = JNIEnv.GetMethodID (class_ref, "onSuccess", "(Ljava/util/List;)V");

			if (GetType () == ThresholdType)
				JNIEnv.CallVoidMethod  (Handle, id_onSuccess_Ljava_util_List_, new JValue (p0));
			else
				JNIEnv.CallNonvirtualVoidMethod  (Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onSuccess", "(Ljava/util/List;)V"), new JValue (p0));
		}
	}

}
