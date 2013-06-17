using System;
using System.Threading.Tasks;
using Android.OS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Ouya.Console.Api;

namespace InAppPurchases
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        List<ButtonSprite> m_buttons = new List<ButtonSprite>();
        FocusManager m_focusManager = new FocusManager();
        private static string m_debugText = string.Empty;
        private ButtonSprite BtnGetProducts = null;
        private ButtonSprite BtnPurchase = null;
        private ButtonSprite BtnGetReceipts = null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            m_focusManager.OnClick += OnClick;

            base.Initialize();
        }

        public class RequestProductsListener : IOuyaResponseListener
        {
            public void Dispose()
            {
                
            }

            public IntPtr Handle
            {
                get { return Game1.Activity.Handle; }
            }

            public void OnCancel()
            {
                Game1.m_debugText = "OnCancel";
            }

            public void OnFailure(int errorCode, string errorMessage, Bundle optionalData)
            {
                Game1.m_debugText = "OnFailure";
            }

            public void OnSuccess(Java.Lang.Object result)
            {
                Game1.m_debugText = "OnSuccess";
            }
        }

        void ProductListComplete()
        {
            
        }

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs.Button)
            {
                return;
            }

            m_debugText = clickEventArgs.Button.Text;

            if (clickEventArgs.Button == BtnGetProducts)
            {
                m_debugText = "Fetching product list...";
                IList<Purchasable> purchasables = new List<Purchasable>()
                                                      {
                                                          new Purchasable("long_sword"),
                                                          new Purchasable("sharp_axe"),
                                                          new Purchasable("cool_level"),
                                                          new Purchasable("awesome_sauce"),
                                                          new Purchasable("__DECLINED__THIS_PURCHASE"),
                                                      };

                RequestProductsListener requestProductsListener = new RequestProductsListener();

                Activity1.PurchaseFacade.RequestProductList(purchasables, requestProductsListener);
            }

            else if (clickEventArgs.Button == BtnPurchase)
            {
                
            }

            else if (clickEventArgs.Button == BtnGetReceipts)
            {
                
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("spriteFont1");

            BtnGetProducts = new ButtonSprite();
            BtnGetProducts.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            BtnGetProducts.Position = new Vector2(150, 200);
            BtnGetProducts.TextureScale = new Vector2(2f, 0.5f);
            BtnGetProducts.Text = "Get Product List";
            BtnGetProducts.TextOffset = new Vector2(40, 20);
            m_buttons.Add(BtnGetProducts);

            BtnPurchase = new ButtonSprite();
            BtnPurchase.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            BtnPurchase.Position = new Vector2(600, 200);
            BtnPurchase.TextureScale = new Vector2(2f, 0.5f);
            BtnPurchase.Text = "Request Purchase";
            BtnPurchase.TextOffset = new Vector2(40, 20);
            m_buttons.Add(BtnPurchase);

            BtnGetReceipts = new ButtonSprite();
            BtnGetReceipts.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            BtnGetReceipts.Position = new Vector2(1100, 200);
            BtnGetReceipts.TextureScale = new Vector2(1.5f, 0.5f);
            BtnGetReceipts.Text = "Get Receipts";
            BtnGetReceipts.TextOffset = new Vector2(30, 20);
            m_buttons.Add(BtnGetReceipts);

            m_focusManager.SelectedButton = BtnGetProducts;
            m_focusManager.Mappings[BtnGetProducts] = new FocusManager.ButtonMapping()
                                                          {
                                                              Right = BtnPurchase
                                                          };
            m_focusManager.Mappings[BtnPurchase] = new FocusManager.ButtonMapping()
                                                       {
                                                           Left = BtnGetProducts,
                                                           Right = BtnGetReceipts
                                                       };
            m_focusManager.Mappings[BtnGetReceipts] = new FocusManager.ButtonMapping()
            {
                Left = BtnPurchase,
            };
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            // TODO: Add your update logic here
            m_focusManager.UpdateFocus();

            foreach (ButtonSprite button in m_buttons)
            {
                if (button == m_focusManager.SelectedButton)
                {
                    button.ButtonTexture = button.ButtonActive;
                }
                else
                {
                    button.ButtonTexture = button.ButtonInactive;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, string.Format("Hello from MonoGame! {0}", m_debugText), new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, "Use DPAD to switch between buttons | Press O to click the button", new Vector2(120, 120), Color.White);
            foreach (ButtonSprite button in m_buttons)
            {
                button.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
