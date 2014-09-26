using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using System.Threading;
using OuyaSdk;

namespace VirtualController
{
    [Activity(Label = "VirtualController"
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
        private const string TAG = "Activity1";

        private Game1 m_game = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game1.Activity = this;
            m_game = new Game1();
            SetContentView(m_game.Window);

            using (var ignore = new TV.Ouya.Sdk.OuyaInputView(this, ApplicationContext))
            {
                // do nothing
            }

            m_game.Run();

            ThreadStart ts = new ThreadStart(m_game.InputWorker);
            Thread thread = new Thread(ts);
            thread.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_game.m_waitForExit = false;
        }
    }
}

