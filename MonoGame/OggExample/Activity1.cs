using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using OuyaSdk;

namespace OggExample
{
    [Activity(Label = "OggExample"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    [IntentFilter(new[] { Intent.ActionMain }
        , Categories = new[] { Intent.CategoryLauncher, OuyaIntent.CategoryGame })]
    public class Activity1 : OuyaActivity
    {
        private Game1 _game = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Game1.Activity = this;
            _game = new Game1();
            SetContentView(_game.Window);
            _game.Run();
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent keyEvent)
        {
            //Log.Info(TAG, "OnKeyUp=" + (int)keyCode);
            int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.DeviceId);
            if (playerNum >= 0 && playerNum < 4)
            {
                if ((int)keyCode == OuyaController.BUTTON_DPAD_DOWN)
                {
                    _game.GetFocusManager().FocusDown();
                }
                if ((int)keyCode == OuyaController.BUTTON_DPAD_LEFT)
                {
                    _game.GetFocusManager().FocusLeft();
                }
                if ((int)keyCode == OuyaController.BUTTON_DPAD_RIGHT)
                {
                    _game.GetFocusManager().FocusRight();
                }
                if ((int)keyCode == OuyaController.BUTTON_DPAD_UP)
                {
                    _game.GetFocusManager().FocusUp();
                }
                if ((int)keyCode == OuyaController.BUTTON_O)
                {
                    _game.GetFocusManager().InvokeClick();
                }
                if ((int)keyCode == OuyaController.BUTTON_A)
                {
                    _game.Exit();
                }
            }
            return true;
        }
    }
}

