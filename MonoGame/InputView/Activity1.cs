#define MONOGAME_4_4

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
#if MONOGAME_4_4
using Android.Views;
#endif
using System;
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
        private OuyaInputView _ouyaInputView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

#if MONOGAME_4_4
            _game = new Game1();
            SetContentView((View)_game.Services.GetService(typeof(View)));

#else
            Game1.Activity = this;
			_game = new Game1();
			SetContentView(_game.Window);
#endif

            _ouyaInputView = new OuyaInputView(this);

			_game.Run();
        }

#if MONOGAME_4_4
        public override bool DispatchGenericMotionEvent(MotionEvent motionEvent)
        {
            if (null != _ouyaInputView)
            {
                return _ouyaInputView.DispatchGenericMotionEvent(motionEvent);

            }
            return base.DispatchGenericMotionEvent(motionEvent);
        }

        public override bool DispatchKeyEvent(KeyEvent keyEvent)
        {
            if (null != _ouyaInputView)
            {
                return _ouyaInputView.DispatchKeyEvent(keyEvent);
            }
            return base.DispatchKeyEvent(keyEvent);
        }
    }
#endif
}
