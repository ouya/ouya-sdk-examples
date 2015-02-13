using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System;

namespace InputView
{
	[Activity (Label = "Xamarin InputView", MainLauncher = true)]
	[IntentFilter(new[] { Intent.ActionMain }
		, Categories = new[] { Intent.CategoryLauncher, CategoryGame })]
	public class MainActivity : Activity
	{
		public const String CategoryGame = "tv.ouya.intent.category.GAME";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			using (var ignore = new TV.Ouya.Sdk.OuyaInputView(this))
			{
				// do nothing
			}
		}
	}
}
