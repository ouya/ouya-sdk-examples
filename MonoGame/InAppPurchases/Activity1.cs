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
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

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
		, Categories = new[] { Intent.CategoryLauncher, CATEGORY_GAME })]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
		private const string TAG = "Activity1";

		private const String CATEGORY_GAME = "tv.ouya.intent.category.GAME";

        private const string DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

		private const string SIGNING_KEY = "key.der";

		private static Activity1 sInstance = null;

        public static OuyaFacade _ouyaFacade = null;

		// listener for fetching gamer info
		private static RequestGamerInfoListener sRequestGamerInfoListener = null;

		// listener for getting products
		private static RequestProductsListener sRequestProductsListener = null;

		// listener for requesting purchase
		private static RequestPurchaseListener sRequestPurchaseListener = null;

		// listener for getting receipts
		private static RequestReceiptsListener sRequestReceiptsListener = null;

		private Game1 mGame = null;

        protected override void OnCreate(Bundle bundle)
        {
			sInstance = this;

            base.OnCreate(bundle);

            Game1.Activity = this;
			mGame = new Game1();
			SetContentView(mGame.Window);

			using (var ignore = new TV.Ouya.Sdk.OuyaInputView(this))
			{
				// do nothing
			}

			View content = FindViewById (Android.Resource.Id.Content);
			if (null != content) {
				content.KeepScreenOn = true;
			}
			mGame.Run();

			Bundle developerInfo = new Bundle();

			developerInfo.PutString(OuyaFacade.OUYA_DEVELOPER_ID, "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");

			byte[] applicationKey = null;

			// load the application key from assets
			try {
				AssetManager assetManager = ApplicationContext.Assets;
				AssetFileDescriptor afd = assetManager.OpenFd(SIGNING_KEY);
				int size = 0;
				if (null != afd) {
					size = (int)afd.Length;
					afd.Close();
					using (Stream inputStream = assetManager.Open(SIGNING_KEY, Access.Buffer))
					{
						applicationKey = new byte[size];
						inputStream.Read(applicationKey, 0, size);
						inputStream.Close();
					}
				}
			} catch (Exception e) {
				Log.Error (TAG, string.Format("Failed to read application key exception={0}", e));
			}

			if (null != applicationKey) {
				Log.Debug (TAG, "Read signing key");
				developerInfo.PutByteArray (OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, applicationKey);
			} else {
				Log.Error (TAG, "Failed to authorize with signing key");
				Finish ();
				return;
			}

			developerInfo.PutString(OuyaFacade.XIAOMI_APPLICATION_ID, "0000000000000");
			developerInfo.PutString(OuyaFacade.XIAOMI_APPLICATION_KEY, "000000000000000000");

			developerInfo.PutStringArray(OuyaFacade.OUYA_PRODUCT_ID_LIST, Game1.PURCHASABLES);

			_ouyaFacade = OuyaFacade.Instance;
			_ouyaFacade.Init(this, developerInfo);
        }

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (null != _ouyaFacade) {
				_ouyaFacade.ProcessActivityResult (requestCode, (int)resultCode, data);
			}
		}

		public static void RequestGamerInfo()
		{
			if (null == sInstance) {
				Log.Error (TAG, "Activity is null!");
				return;
			}

			if (null == _ouyaFacade) {
				Log.Error (TAG, "OuyaFacade is null!");
				return;
			}

			Action action = () => {
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Construct CustomRequestGamerInfoListener");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				sRequestGamerInfoListener = new Game1.CustomRequestGamerInfoListener();

				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Invoke RequestGamerInfo");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				_ouyaFacade.RequestGamerInfo(sInstance, sRequestGamerInfoListener);
			};
			sInstance.RunOnUiThread (action);
		}

		public static void RequestProducts()
		{
			if (null == sInstance) {
				Log.Error (TAG, "Activity is null!");
				return;
			}

			if (null == _ouyaFacade) {
				Log.Error (TAG, "OuyaFacade is null!");
				return;
			}

			Action action = () => {
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Construct CustomRequestProductsListener");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				sRequestProductsListener = new Game1.CustomRequestProductsListener();

				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Invoke RequestProducts");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				List<Purchasable> products = new List<Purchasable>();
				foreach (string id in Game1.PURCHASABLES)
				{
					Purchasable purchasable = new Purchasable(id);
					products.Add(purchasable);
				}
				_ouyaFacade.RequestProductList(sInstance, products, sRequestProductsListener);
			};
			sInstance.RunOnUiThread (action);
		}

		public static void RequestPurchase(Purchasable purchasable)
		{
			if (null == sInstance) {
				Log.Error (TAG, "Activity is null!");
				return;
			}

			if (null == _ouyaFacade) {
				Log.Error (TAG, "OuyaFacade is null!");
				return;
			}

			Action action = () => {
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Construct CustomRequestPurchaseListener");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				sRequestPurchaseListener = new Game1.CustomRequestPurchaseListener();

				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Invoke RequestPurchase");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				_ouyaFacade.RequestPurchase(sInstance, purchasable, sRequestPurchaseListener);
			};
			sInstance.RunOnUiThread (action);
		}

		public static void RequestReceipts()
		{
			if (null == sInstance) {
				Log.Error (TAG, "Activity is null!");
				return;
			}

			if (null == _ouyaFacade) {
				Log.Error (TAG, "OuyaFacade is null!");
				return;
			}

			Action action = () => {
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Construct CustomRequestPurchaseListener");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				sRequestReceiptsListener = new Game1.CustomRequestReceiptsListener();

				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "Invoke RequestReceipts");
				Log.Info(TAG, "****************");
				Log.Info(TAG, "****************");
				_ouyaFacade.RequestReceipts(sInstance, sRequestReceiptsListener);
			};
			sInstance.RunOnUiThread (action);
		}

		public static void Quit()
		{
			if (null != sInstance) {
				sInstance.Finish ();
			}
		}

		private String GetStringResource(String name) {
			Resources resources = Resources;
			if (null == resources) {
				return String.Empty;
			}
			int id = resources.GetIdentifier (name, "string", PackageName);
			if (id <= 0) {
				return "";
			}
			return resources.GetString(id);
		}

		private static Dictionary<string,string> sDebugStrings = new Dictionary<string, string> ();

		public static String GetLocalizedString(String name)
		{
			if (null == sInstance) {
				Log.Error(TAG, string.Format("Activity has not initialized to get resource {0}", name));
				return "";
			} else {
				String result = sInstance.GetStringResource (name);
				//Log.Info(TAG, string.Format("{0} ==> {1}", name, result));
				sDebugStrings [result] = name;
				return result;
			}
		}

		public static Vector2 MeasureString(SpriteFont font, string text) {
			try {
				return font.MeasureString(text);
			} catch (Exception) {
				if (null == text) {
					Log.Error (TAG, "Failed to measure string == text: null");
				} else if (sDebugStrings.ContainsKey(text)) {
					Log.Error (TAG, "Failed to measure string key=" + sDebugStrings[text] + " ==> " + text);
				} else {
					Log.Error (TAG, "Failed to measure string text=" + text);
				}
				return Vector2.Zero;
			}
		}

		public static void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color) {
			try {
				spriteBatch.DrawString(font, text, position, color);
			} catch (Exception) {
				if (null == text) {
					Log.Error (TAG, "Failed to draw string == text: null");
				} else if (sDebugStrings.ContainsKey(text)) {
					Log.Error (TAG, "Failed to draw string key=" + sDebugStrings[text] + " ==> " + text);
				} else {
					Log.Error (TAG, "Failed to draw string text=" + text);
				}
			}
		}

		public static void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects spriteEffects, float depth) {
			try {
				spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, spriteEffects, depth);
			} catch (Exception) {
				if (null == text) {
					Log.Error (TAG, "Failed to draw string == text: null");
				} else if (sDebugStrings.ContainsKey(text)) {
					Log.Error (TAG, "Failed to draw string key=" + sDebugStrings[text] + " ==> " + text);
				} else {
					Log.Error (TAG, "Failed to draw string text=" + text);
				}
			}
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			mGame.UninitializeContent ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			mGame.InitializeContent ();
		}
    }
}

