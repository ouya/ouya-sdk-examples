#define MONOGAME_3_5

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
#if MONOGAME_3_4 || MONOGAME_3_5
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
		, Categories = new[] { Intent.CategoryLauncher, CATEGORY_GAME, CATEGORY_LEANBACK })]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
		public const String CATEGORY_GAME = "tv.ouya.intent.category.GAME";

        private const String CATEGORY_LEANBACK = "android.intent.category.LEANBACK_LAUNCHER";

        private Game1 _game = null;

        private OuyaInputView _ouyaInputView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

#if MONOGAME_3_4 || MONOGAME_3_5
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

#if MONOGAME_3_4 || MONOGAME_3_5
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
