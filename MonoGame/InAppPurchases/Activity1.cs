using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Ouya.Console.Api;

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

        public static OuyaFacade PurchaseFacade = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Game1.Activity = this;
            var g = new Game1();
            SetContentView(g.Window);
            g.Run();

            PurchaseFacade = OuyaFacade.Instance;
            PurchaseFacade.Init(this, DEVELOPER_ID);
        }
    }
}

