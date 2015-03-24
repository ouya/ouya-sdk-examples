using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using System.Collections.Generic;
using TV.Ouya.Sdk;

namespace InputView
{
    [Activity(Label = "MonoGame InputView"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    [IntentFilter(new[] { Intent.ActionMain }
        , Categories = new[] { Intent.CategoryLauncher, CategoryGame })]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        public const String CategoryGame = "tv.ouya.intent.category.GAME";
		private Game1 _game = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game1.Activity = this;
			_game = new Game1();
			SetContentView(_game.Window);

            using (var ignore = new TV.Ouya.Sdk.OuyaInputView(this))
            {
                // do nothing
            }

			_game.Run();
        }
    }
}
