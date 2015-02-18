using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace AsyncContentLoading
{
    [Activity(Label = "AsyncContentLoading"
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
        private const string CategoryGame = "tv.ouya.intent.category.GAME";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Game1.Activity = this;
            var g = new Game1();
            SetContentView(g.Window);
            g.Run();
        }
    }
}

