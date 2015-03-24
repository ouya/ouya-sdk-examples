using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Microsoft.Xna.Framework;
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

namespace SoundPoolExample
{
    [Activity(Label = "SoundPoolExample"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    [IntentFilter(new[] { Intent.ActionMain }
        , Categories = new[] { Intent.CategoryLauncher, OuyaIntent.CategoryGame })]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Game1.Activity = this;
            _game = new Game1();
            SetContentView(_game.Window);

			using (var ignore = new OuyaInputView (this)) {
				// do nothing
			}

			View content = FindViewById (Android.Resource.Id.Content);
			if (null != content) {
				content.KeepScreenOn = true;
			}

            _game.Run();
        }
    }
}

