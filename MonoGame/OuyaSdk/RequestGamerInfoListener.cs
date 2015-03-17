using Android.Runtime;
using Android.Util;
using System;

namespace TV.Ouya.Sdk{
	public partial class RequestGamerInfoListener
	{
		static Delegate cb_onSuccess_Ltv_ouya_console_api_GamerInfo_;
		#pragma warning disable 0169
		static Delegate GetOnSuccess_Ltv_ouya_console_api_GamerInfo_Handler ()
		{
			if (cb_onSuccess_Ltv_ouya_console_api_GamerInfo_ == null)
				cb_onSuccess_Ltv_ouya_console_api_GamerInfo_ = JNINativeWrapper.CreateDelegate ((Action<IntPtr, IntPtr, IntPtr>) n_OnSuccess_Ltv_ouya_console_api_GamerInfo_);
			return cb_onSuccess_Ltv_ouya_console_api_GamerInfo_;
		}

		static void n_OnSuccess_Ltv_ouya_console_api_GamerInfo_ (IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
		{
			global::TV.Ouya.Sdk.RequestGamerInfoListener __this = global::Java.Lang.Object.GetObject<global::TV.Ouya.Sdk.RequestGamerInfoListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer);
			global::TV.Ouya.Console.Api.GamerInfo p0 = global::Java.Lang.Object.GetObject<global::TV.Ouya.Console.Api.GamerInfo> (native_p0, JniHandleOwnership.DoNotTransfer);
			__this.OnSuccess (p0);
		}
		#pragma warning restore 0169

		static IntPtr id_onSuccess_Ltv_ouya_console_api_GamerInfo_;
		// Metadata.xml XPath method reference: path="/api/package[@name='tv.ouya.sdk']/class[@name='RequestGamerInfoListener']/method[@name='onSuccess' and count(parameter)=1 and parameter[1][@type='tv.ouya.console.api.GamerInfo']]"
		[Register ("onSuccess", "(Ltv/ouya/console/api/GamerInfo;)V", "GetOnSuccess_Ltv_ouya_console_api_GamerInfo_Handler")]
		public override void OnSuccess (global::Java.Lang.Object p0)
		{
			if (id_onSuccess_Ltv_ouya_console_api_GamerInfo_ == IntPtr.Zero)
				id_onSuccess_Ltv_ouya_console_api_GamerInfo_ = JNIEnv.GetMethodID (class_ref, "onSuccess", "(Ltv/ouya/console/api/GamerInfo;)V");

			if (GetType () == ThresholdType)
				JNIEnv.CallVoidMethod  (Handle, id_onSuccess_Ltv_ouya_console_api_GamerInfo_, new JValue (p0));
			else
				JNIEnv.CallNonvirtualVoidMethod  (Handle, ThresholdClass, JNIEnv.GetMethodID (ThresholdClass, "onSuccess", "(Ltv/ouya/console/api/GamerInfo;)V"), new JValue (p0));
		}
	}
}
