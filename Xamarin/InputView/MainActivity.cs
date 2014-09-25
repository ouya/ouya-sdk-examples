using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OuyaSdk;
using System;

namespace InputView
{
	[Activity (Label = "InputView", MainLauncher = true)]
	[IntentFilter(new[] { Intent.ActionMain }
		, Categories = new[] { Intent.CategoryLauncher, CategoryGame })]
	public class MainActivity : Activity
	{
		public const String CategoryGame = "tv.ouya.intent.category.GAME";

		private OuyaInputView m_inputView = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			m_inputView = new OuyaInputView(Handle, ApplicationContext.Handle);
		}
	}
}


