using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
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
    public class Activity1 : OuyaActivity
    {
        private const string TAG = "Activity1";

        private Game1 m_game = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game1.Activity = this;
            m_game = new Game1();
            SetContentView(m_game.Window);
            m_game.Run();
        }

        public override bool OnGenericMotionEvent(MotionEvent motionEvent)
        {
            int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.DeviceId);
            if (playerNum >= 0 && playerNum < 4 &&
                null != m_game &&
                null != m_game.Controllers &&
                playerNum < m_game.Controllers.Count)
            {
                m_game.Controllers[playerNum].Axis[OuyaController.AXIS_LS_X] =
                    motionEvent.GetAxisValue((Axis) OuyaController.AXIS_LS_X);
                m_game.Controllers[playerNum].Axis[OuyaController.AXIS_LS_Y] =
                    motionEvent.GetAxisValue((Axis) OuyaController.AXIS_LS_Y);
                m_game.Controllers[playerNum].Axis[OuyaController.AXIS_RS_X] =
                    motionEvent.GetAxisValue((Axis) OuyaController.AXIS_RS_X);
                m_game.Controllers[playerNum].Axis[OuyaController.AXIS_RS_Y] =
                    motionEvent.GetAxisValue((Axis) OuyaController.AXIS_RS_Y);
                m_game.Controllers[playerNum].Axis[OuyaController.AXIS_L2] =
                    motionEvent.GetAxisValue((Axis) OuyaController.AXIS_L2);
                m_game.Controllers[playerNum].Axis[OuyaController.AXIS_R2] =
                    motionEvent.GetAxisValue((Axis) OuyaController.AXIS_R2);
                base.OnGenericMotionEvent(motionEvent);
            }
            return true;
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent keyEvent)
        {
            //Log.Info(TAG, "OnKeyDown=" + (int)keyCode);
            int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.DeviceId);
            if (playerNum >= 0 && playerNum < 4 &&
                null != m_game &&
                null != m_game.Controllers &&
                playerNum < m_game.Controllers.Count)
            {
                m_game.Controllers[playerNum].Button[(int) keyCode] = true;

                if ((int) keyCode == OuyaController.BUTTON_MENU)
                {
                    Log.Info(TAG, "Menu Detected=" + (int) keyCode);
                    m_game.Controllers[playerNum].TimerMenuDetected = DateTime.Now + TimeSpan.FromSeconds(1);
                }
            }
            return true;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent keyEvent)
        {
            //Log.Info(TAG, "OnKeyUp=" + (int)keyCode);
            int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.DeviceId);
            if (playerNum >= 0 && playerNum < 4 &&
                null != m_game &&
                null != m_game.Controllers &&
                playerNum < m_game.Controllers.Count)
            {
                m_game.Controllers[playerNum].Button[(int)keyCode] = false;
            }
            return true;
        }
    }
}

