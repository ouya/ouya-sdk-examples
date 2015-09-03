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
using System;
using System.IO;
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace InAppPurchases
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
		private const string TAG = "Game1";

		public static readonly string[] PURCHASABLES =
		{
			"long_sword",
			"sharp_axe",
			"cool_level",
			"awesome_sauce",
			"__DECLINED__THIS_PURCHASE",
		};

        GraphicsDeviceManager m_graphics;
		SpriteBatch m_spriteBatch;
		SpriteFont m_font;
        List<ButtonSprite> m_buttons = new List<ButtonSprite>();
        FocusManager m_focusManager = new FocusManager();
        private static string m_debugText = string.Empty;
        private ButtonSprite BtnRequestProducts = null;
        private ButtonSprite BtnRequestPurchase = null;
        private ButtonSprite BtnRequestReceipts = null;
        private ButtonSprite BtnRequestGamerInfo = null;
		private ButtonSprite BtnExit = null;
        private ButtonSprite BtnPause = null;
		private static List<Product> s_products = new List<Product> ();
		private static List<Receipt> s_receipts = new List<Receipt> ();
		private static int s_productIndex = 0;

		private Texture2D m_inactiveButton = null;
		private Texture2D m_activeButton = null;

        public Game1()
        {
			m_graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

			m_graphics.IsFullScreen = true;
			m_graphics.PreferredBackBufferWidth = 1920;
			m_graphics.PreferredBackBufferHeight = 1080;
			m_graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        }

		void DeviceResetEvent(object sender, EventArgs args) {
			Log.Info (TAG, "Device Reset Event Occurred.");
		}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

			m_graphics.GraphicsDevice.DeviceReset += DeviceResetEvent;

			BtnRequestProducts = new ButtonSprite ();
			BtnRequestProducts.Position = new Vector2 (150, 200);
			BtnRequestProducts.TextureScale = new Vector2 (2.25f, 0.5f);
			BtnRequestProducts.Text = Activity1.GetLocalizedString("RequestProductList");
			BtnRequestProducts.TextOffset = new Vector2 (40, 20);
			m_buttons.Add (BtnRequestProducts);

			BtnRequestPurchase = new ButtonSprite ();
			BtnRequestPurchase.Position = new Vector2 (500, 200);
			BtnRequestPurchase.TextureScale = new Vector2 (2f, 0.5f);
			BtnRequestPurchase.Text = Activity1.GetLocalizedString("RequestPurchase");
			BtnRequestPurchase.TextOffset = new Vector2 (40, 20);
			m_buttons.Add (BtnRequestPurchase);

			BtnRequestReceipts = new ButtonSprite ();
			BtnRequestReceipts.Position = new Vector2 (800, 200);
			BtnRequestReceipts.TextureScale = new Vector2 (2f, 0.5f);
			BtnRequestReceipts.Text = Activity1.GetLocalizedString("RequestReceipts");
			BtnRequestReceipts.TextOffset = new Vector2 (30, 20);
			m_buttons.Add (BtnRequestReceipts);

			BtnRequestGamerInfo = new ButtonSprite ();
			BtnRequestGamerInfo.Position = new Vector2 (1100, 200);
			BtnRequestGamerInfo.TextureScale = new Vector2 (2f, 0.5f);
			BtnRequestGamerInfo.Text = Activity1.GetLocalizedString("RequestGamerInfo");
			BtnRequestGamerInfo.TextOffset = new Vector2 (30, 20);
			m_buttons.Add (BtnRequestGamerInfo);

			BtnExit = new ButtonSprite ();
			BtnExit.Position = new Vector2 (1400, 200);
			BtnExit.TextureScale = new Vector2 (1f, 0.5f);
			BtnExit.Text = Activity1.GetLocalizedString("Exit");
			BtnExit.TextOffset = new Vector2 (30, 20);
			m_buttons.Add (BtnExit);

			BtnPause = new ButtonSprite ();
			BtnPause.Position = new Vector2 (1650, 200);
			BtnPause.TextureScale = new Vector2 (1f, 0.5f);
			BtnPause.Text = Activity1.GetLocalizedString("Pause");
			BtnPause.TextOffset = new Vector2 (30, 20);
			m_buttons.Add (BtnPause);

			m_focusManager.SelectedButton = BtnRequestProducts;
			m_focusManager.Mappings [BtnRequestProducts] = new FocusManager.ButtonMapping () {
				Right = BtnRequestPurchase
			};
			m_focusManager.Mappings [BtnRequestPurchase] = new FocusManager.ButtonMapping () {
				Left = BtnRequestProducts,
				Right = BtnRequestReceipts
			};
			m_focusManager.Mappings [BtnRequestReceipts] = new FocusManager.ButtonMapping () {
				Left = BtnRequestPurchase,
				Right = BtnRequestGamerInfo,
			};

			m_focusManager.Mappings [BtnRequestGamerInfo] = new FocusManager.ButtonMapping () {
				Left = BtnRequestReceipts,
				Right = BtnExit,
			};

			m_focusManager.Mappings [BtnExit] = new FocusManager.ButtonMapping () {
				Left = BtnRequestGamerInfo,
			};

			m_focusManager.Mappings [BtnPause] = new FocusManager.ButtonMapping () {
				Left = BtnExit,
			};
        }

		Texture2D GetAssetTexture(String texture) {

			try {
				AssetManager assetManager = Activity.ApplicationContext.Assets;
				AssetFileDescriptor afd = assetManager.OpenFd(texture);
				int size = 0;
				if (null != afd) {
					size = (int)afd.Length;
					afd.Close();
					Texture2D result = null;
					using (Stream inputStream = assetManager.Open(texture, Access.Buffer))
					{
						result = Texture2D.FromStream (GraphicsDevice, inputStream);
						inputStream.Close();
					}
					return result;
				}
			} catch (Exception e) {
				Log.Error (TAG, string.Format("Failed to read application key exception={0}", e));
			}

			return null;
		}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
		{
			if (null == GraphicsDevice) {
				return;
			}
			// Create a new SpriteBatch, which can be used to draw textures.
			m_spriteBatch = new SpriteBatch (GraphicsDevice);

			// TODO: use this.Content to load your game content here
			m_font = Content.Load<SpriteFont> (Activity1.GetLocalizedString ("FontName"));

			m_inactiveButton = Content.Load<Texture2D> ("Graphics/ButtonInactive");
			m_activeButton = Content.Load<Texture2D> ("Graphics/ButtonActive");

			//m_inactiveButton = GetAssetTexture ("ButtonInactive.png");
			//m_activeButton = GetAssetTexture ("ButtonActive.png");
		}

		public class CustomRequestGamerInfoListener : RequestGamerInfoListener
		{
			private const string TAG = "CustomRequestGamerInfoListener";

			public override void OnSuccess(Java.Lang.Object jObject) {
				GamerInfo gamerInfo = jObject.JavaCast<GamerInfo>();
				if (null == gamerInfo) {
					m_debugText = string.Format ("GamerInfo is null!");
				} else {
					m_debugText = string.Format ("Request Gamer UUID={0} Username={1}", gamerInfo.Uuid, gamerInfo.Username);
					Log.Info (TAG, "OnSuccess uuid=" + gamerInfo.Uuid + " username=" + gamerInfo.Username);
				}
			}

			public override void OnFailure(int errorCode, String errorMessage, Bundle optionalData) {
				m_debugText = string.Format ("Failed to request GamerInfo errorCode=" + errorCode + " errorMessage=" + errorMessage);
				Log.Error (TAG, "OnFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
			}
		}

		public class CustomRequestProductsListener : RequestProductsListener
		{
			private const string TAG = "CustomRequestProductsListener";

			public override void OnSuccess(Java.Lang.Object jObject) {
				s_products.Clear ();
				JavaList<Product> products = jObject.JavaCast<JavaList<Product>> ();
				if (null == products) {
					m_debugText = string.Format ("Products are null!");
				} else {
					foreach (Product product in products) {
						s_products.Add (product);
					}
					m_debugText = string.Format ("Request Products: OnSuccess Count="+products.Count);
					Log.Info (TAG, "CustomRequestProductsListener: OnSuccess");
				}
			}

			public override void OnFailure(int errorCode, String errorMessage, Bundle optionalData) {
				s_products.Clear ();
				m_debugText = string.Format ("Failed to request Products errorCode=" + errorCode + " errorMessage=" + errorMessage);
				Log.Error (TAG, "CustomRequestProductsListener: OnFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
			}
		}

		public class CustomRequestPurchaseListener : RequestPurchaseListener
		{
			private const string TAG = "CustomRequestPurchaseListener";

			public override void OnSuccess(Java.Lang.Object jObject) {
				PurchaseResult purchaseResult = jObject.JavaCast<PurchaseResult>();
				if (null == purchaseResult) {
					m_debugText = string.Format ("PurchaseResult is null!");
				} else {
					m_debugText = string.Format ("Request Purchase: OnSuccess");
					Log.Info (TAG, "OnSuccess identiifer"+purchaseResult.ProductIdentifier);
				}
			}

			public override void OnFailure(int errorCode, String errorMessage, Bundle optionalData) {
				m_debugText = string.Format ("Failed to request Purchase errorCode=" + errorCode + " errorMessage=" + errorMessage);
				Log.Error (TAG, "OnFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
			}

			public override void OnCancel() {
				m_debugText = "Purchase was cancelled";
				Log.Error (TAG, "Purchase was cancelled");
			}
		}

		public class CustomRequestReceiptsListener : RequestReceiptsListener
		{
			private const string TAG = "CustomRequestReceiptsListener";

			public override void OnSuccess(Java.Lang.Object jObject) {
				s_receipts.Clear ();
				JavaCollection<Receipt> receipts = jObject.JavaCast<JavaCollection<Receipt>> ();
				if (null == receipts) {
					m_debugText = string.Format ("Receipts are null!");
				} else {
					foreach (Receipt receipt in receipts) {
						s_receipts.Add (receipt);
					}
					m_debugText = string.Format ("Request Receipts: OnSuccess Count="+s_receipts.Count);
					Log.Info (TAG, "OnSuccess");
				}
			}

			public override void OnFailure(int errorCode, String errorMessage, Bundle optionalData) {
				m_debugText = string.Format ("Failed to request Receipts errorCode=" + errorCode + " errorMessage=" + errorMessage);
				Log.Error (TAG, "OnFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
			}

			public override void OnCancel() {
				m_debugText = "Receipt request was cancelled";
				Log.Error (TAG, "Receipt request was cancelled");
			}
		}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
			//Log.Info(TAG, string.Format("BUTTON_O is {0}", OuyaInput.GetButton(OuyaController.BUTTON_O)));

			if (OuyaInput.GetButtonUp(0, OuyaController.BUTTON_A))
            {
                Exit();
            }

            // TODO: Add your update logic here
            m_focusManager.UpdateFocus();

            foreach (ButtonSprite button in m_buttons)
            {
				// button is active if the selected button is true
				button.m_isActive = (button == m_focusManager.SelectedButton);
            }

			if (OuyaInput.GetButtonUp (OuyaController.BUTTON_MENU)) {
				m_debugText = "Pause button detected.";
				m_focusManager.SelectedButton = BtnPause;
			}

			if (OuyaInput.GetButtonUp (OuyaController.BUTTON_O)) {
				if (m_focusManager.SelectedButton == BtnRequestProducts) {
					s_productIndex = 0;
					s_products.Clear ();
					m_debugText = "Requesting products...";
					Activity1.RequestProducts ();
				} else if (m_focusManager.SelectedButton == BtnRequestPurchase) {
					if (s_productIndex < s_products.Count) {
						Purchasable purchasable = new Purchasable (s_products [s_productIndex].Identifier);
						m_debugText = "Requesting purchase...";
						Activity1.RequestPurchase (purchasable);
					} else {
						m_debugText = "Be sure to request products to select a purchase...";
					}
				} else if (m_focusManager.SelectedButton == BtnRequestReceipts) {
					m_debugText = "Requesting receipts...";
					Activity1.RequestReceipts ();
				} else if (m_focusManager.SelectedButton == BtnRequestGamerInfo) {
					m_debugText = "Requesting GamerInfo...";
					Activity1.RequestGamerInfo ();
				} else if (m_focusManager.SelectedButton == BtnExit) {
					m_debugText = "Exiting...";
					Activity1.Quit ();
				} else if (m_focusManager.SelectedButton == BtnPause) {
					m_debugText = "Pause button detected...";
					m_focusManager.SelectedButton = BtnPause;
				}
			}

			if (m_focusManager.SelectedButton == BtnRequestProducts ||
				m_focusManager.SelectedButton == BtnRequestPurchase) {
				if (OuyaInput.GetButtonUp (OuyaController.BUTTON_DPAD_UP)) {
					if (s_productIndex > 0) {
						--s_productIndex;
					}
				}
				if (OuyaInput.GetButtonUp (OuyaController.BUTTON_DPAD_DOWN)) {
					if ((s_productIndex+1) < s_products.Count) {
						++s_productIndex;
					}
				}
			}

            base.Update(gameTime);

			OuyaInput.ClearButtonStates();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			m_graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			m_spriteBatch.Begin();

            if (Activity1._ouyaFacade.IsRunningOnOUYASupportedHardware)
            {
                OuyaFacade.DeviceHardware deviceHardware = Activity1._ouyaFacade.GetDeviceHardware();
                if (null == deviceHardware)
                {
                    Activity1.DrawString(m_spriteBatch, m_font, string.Format("Hello from MonoGame! Running on null device | {0}",
                        m_debugText), new Vector2(100, 100), Color.White);
                }
                else
                {
                    Activity1.DrawString(m_spriteBatch, m_font, string.Format("Hello from MonoGame! Running on '{0}' device enum '{1}' | {2}",
                        deviceHardware.DeviceName(),
                        deviceHardware.DeviceEnum(),
                        m_debugText), new Vector2(100, 100), Color.White);
                }
            }
            else
            {
                Activity1.DrawString(m_spriteBatch, m_font, string.Format("Hello from MonoGame! Not Running on OUYA Supported Hardware | {0}",
                    m_debugText), new Vector2(100, 100), Color.White);
            }

            Activity1.DrawString(m_spriteBatch, m_font, "Use DPAD to switch between buttons | Press O to click the button", new Vector2(500, 170), Color.Orange);
            foreach (ButtonSprite button in m_buttons)
            {
				button.Draw(m_spriteBatch, m_font, m_activeButton, m_inactiveButton);
            }

            #region Products

			if (m_focusManager.SelectedButton == BtnRequestProducts ||
				m_focusManager.SelectedButton == BtnRequestPurchase) {
				Vector2 position = new Vector2(140, 300);
				for (int index = 0; index < s_products.Count; ++index)
				{
					Product product = s_products[index];
					Activity1.DrawString(m_spriteBatch, m_font,
						string.Format("{0} Product: {1} LocalPrice={2}",
							index == s_productIndex ? "*" : string.Empty,
							product.Identifier,
							product.LocalPrice),
						position,
						index == s_productIndex ? Color.White : Color.Orange);
					position += new Vector2(0, 20);
				}
			}

            #endregion

            #region Receipts

			if (m_focusManager.SelectedButton == BtnRequestReceipts) {
	            Vector2 position = new Vector2(1120, 300);
	            for (int index = 0; index < s_receipts.Count; ++index)
	            {
					Receipt receipt = s_receipts[index];
					Activity1.DrawString(m_spriteBatch, m_font, string.Format("Receipt: {0} LocalPrice={1}", receipt.Identifier, receipt.LocalPrice), position, Color.White);
	                position += new Vector2(0, 20);
	            }
			}

            #endregion

			m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
