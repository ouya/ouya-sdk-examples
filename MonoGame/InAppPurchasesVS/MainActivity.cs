using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System;
using System.IO;
using Tv.Ouya.Sdk.Android;

namespace InAppPurchases
{
	[Activity(Label = "InAppPurchases"
	          , MainLauncher = true
	          , Icon = "@drawable/icon"
	          , Theme = "@style/Theme.Splash"
	          , AlwaysRetainTaskState = true
	          , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
	          , ScreenOrientation = ScreenOrientation.SensorLandscape
	          , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
	[IntentFilter(new[] { Intent.ActionMain }
	, Categories = new[] { Intent.CategoryLauncher, OuyaIntent.CategoryGame })]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
	{
		public const string DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

		public static MonoGameOuyaFacade PurchaseFacade = null;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			Game1.Activity = this;
			var g = new Game1();
			SetContentView(g.Window);
			g.Run();

			PluginOuya.AsyncSetActivity(this);

			PluginOuya.OuyaControllerInit(ApplicationContext);

			PluginOuya.AsyncSetApplicationKey(ApplicationContext);

			PluginOuya.AsyncSetDeveloperId(DEVELOPER_ID);
		}
	}
}

