using System;
using System.Collections;
using System.Threading.Tasks;
using Android.OS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tv.Ouya.Sdk.Android;

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
        public static FocusManager m_focusManager = new FocusManager();
        public static string m_debugText = string.Empty;
        private ButtonSprite BtnGetProducts = null;
        private ButtonSprite BtnPurchase = null;
        private ButtonSprite BtnGetReceipts = null;
        private ButtonSprite BtnGetUUID = null;
        private ButtonSprite BtnPause = null;

        private CallbacksFetchGamerUUID m_callbacksFetchGamerUUID = new CallbacksFetchGamerUUID();
        private CallbacksRequestProducts m_callbacksRequestProducts = new CallbacksRequestProducts();
        private CallbacksRequestPurchase m_callbacksRequestPurchase = new CallbacksRequestPurchase();
        private CallbacksRequestReceipts m_callbacksRequestReceipts = new CallbacksRequestReceipts();

        public static List<Product> m_products = new List<Product>();
        public static List<Receipt> m_receipts = new List<Receipt>();

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

            GamePad.EventOnKeyUp += EventOnKeyUp;

            base.Initialize();
        }

        private void EventOnKeyUp(object sender, GamePad.KeyEventArgs keyEventArgs)
        {
            //m_debugText = string.Format("Detected Key: {0}", keyEventArgs.KeyCode);
            if (keyEventArgs.KeyCode == 82)
            {
                m_focusManager.SelectedButton = BtnPause;
                OnClick(null, new FocusManager.ClickEventArgs() {Button = BtnPause});
            }
        }

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs ||
                null == clickEventArgs.Button)
            {
                return;
            }

            m_debugText = clickEventArgs.Button.Text;

            if (clickEventArgs.Button == BtnGetProducts)
            {
                m_debugText = "Fetching product list...";
                List<string> purchasables = new List<string>()
                {
                    "long_sword",
                    "sharp_axe",
                    "cool_level",
                    "awesome_sauce",
                    "__DECLINED__THIS_PURCHASE",
                };

                m_focusManager.SelectedProductIndex = 0;

                m_debugText = "Requesting Product List...";
                m_products.Clear();
                PluginOuya.AsyncOuyaRequestProducts(m_callbacksRequestProducts, purchasables);
            }

            else if (clickEventArgs.Button == BtnPurchase)
            {
                if (m_products.Count == 0)
                {
                    m_debugText = "Request Products First...";
                }
                else if (m_focusManager.SelectedProductIndex < m_products.Count)
                {
                    m_debugText = "Requesting Purchase...";
                    Product product = m_products[m_focusManager.SelectedProductIndex];
                    PluginOuya.AsyncOuyaRequestPurchase(m_callbacksRequestPurchase, product.Identifier);
                }
            }

            else if (clickEventArgs.Button == BtnGetReceipts)
            {
                m_focusManager.SelectedReceiptIndex = 0;
                m_debugText = "Requesting Receipts...";
                m_receipts.Clear();
                PluginOuya.AsyncOuyaRequestReceipts(m_callbacksRequestReceipts);
            }

            else if (clickEventArgs.Button == BtnGetUUID)
            {
                m_debugText = "Requesting Gamer UUID...";
                PluginOuya.AsyncOuyaFetchGamerUUID(m_callbacksFetchGamerUUID);
            }

            else if (clickEventArgs.Button == BtnPause)
            {
                m_debugText = "Pause button detected.";
                m_focusManager.SelectedButton = BtnPause;
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

            BtnGetUUID = new ButtonSprite();
            BtnGetUUID.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            BtnGetUUID.Position = new Vector2(1400, 200);
            BtnGetUUID.TextureScale = new Vector2(1.25f, 0.5f);
            BtnGetUUID.Text = "Get UUID";
            BtnGetUUID.TextOffset = new Vector2(30, 20);
            m_buttons.Add(BtnGetUUID);

            BtnPause = new ButtonSprite();
            BtnPause.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            BtnPause.Position = new Vector2(1650, 200);
            BtnPause.TextureScale = new Vector2(1f, 0.5f);
            BtnPause.Text = "Pause";
            BtnPause.TextOffset = new Vector2(30, 20);
            m_buttons.Add(BtnPause);

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
                Right = BtnGetUUID,
            };

            m_focusManager.Mappings[BtnGetUUID] = new FocusManager.ButtonMapping()
            {
                Left = BtnGetReceipts,
            };

            m_focusManager.Mappings[BtnPause] = new FocusManager.ButtonMapping()
            {
                Left = BtnGetUUID,
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

            m_focusManager.UpdatePauseFocus(BtnPause);

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
            spriteBatch.DrawString(font, "Use DPAD to switch between buttons | Press O to click the button", new Vector2(500, 170), Color.Orange);
            foreach (ButtonSprite button in m_buttons)
            {
                button.Draw(spriteBatch);
            }

            #region Products

            if (null != m_products)
            {
                Vector2 position = new Vector2(140, 300);
                for (int index = 0; index < m_products.Count; ++index)
                {
                    Product product = m_products[index];
                    spriteBatch.DrawString(font, string.Format("Product: {0}", product.Identifier), position, index == m_focusManager.SelectedProductIndex ? Color.Orange : Color.White);
                    position += new Vector2(0, 20);
                }
            }

            #endregion

            #region Receipts

            if (null != m_receipts)
            {
                Vector2 position = new Vector2(1120, 300);
                for (int index = 0; index < m_receipts.Count; ++index)
                {
                    Receipt receipt = m_receipts[index];
                    spriteBatch.DrawString(font, string.Format("Receipt: {0}", receipt.Identifier), position, index == m_focusManager.SelectedReceiptIndex ? Color.Orange : Color.White);
                    position += new Vector2(0, 20);
                }
            }

            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
