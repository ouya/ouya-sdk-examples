using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using KryptonTestbed;

namespace MonoGameKryptonTestbed
{
	[Activity(Label = "MonoGameKryptonTestbed"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.SensorLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
	[IntentFilter(new[] { Intent.ActionMain }
		, Categories = new[] { Intent.CategoryLauncher, CATEGORY_GAME })]
	public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
	{
		private const String CATEGORY_GAME = "tv.ouya.intent.category.GAME";

		protected override void OnCreate (Bundle bundle)
		{
			Krypton.KryptonEngine.Activity = this;
			base.OnCreate (bundle);

			var g = new KryptonDemoGame();
			SetContentView((View)g.Services.GetService(typeof(View)));

			View content = FindViewById (Android.Resource.Id.Content);
			if (null != content) {
				content.KeepScreenOn = true;
			}

			g.Run ();
		}
	}
}


