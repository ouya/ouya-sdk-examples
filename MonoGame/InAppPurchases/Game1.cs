using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        private string m_debugText = string.Empty;

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

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs.Button)
            {
                return;
            }

            m_debugText = clickEventArgs.Button.Text;
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

            ButtonSprite btnGetProducts = new ButtonSprite();
            btnGetProducts.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            btnGetProducts.Position = new Vector2(150, 200);
            btnGetProducts.TextureScale = new Vector2(2f, 0.5f);
            btnGetProducts.Text = "Get Product List";
            btnGetProducts.TextOffset = new Vector2(40, 20);
            m_buttons.Add(btnGetProducts);

            ButtonSprite btnPurchase = new ButtonSprite();
            btnPurchase.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            btnPurchase.Position = new Vector2(600, 200);
            btnPurchase.TextureScale = new Vector2(2f, 0.5f);
            btnPurchase.Text = "Request Purchase";
            btnPurchase.TextOffset = new Vector2(40, 20);
            m_buttons.Add(btnPurchase);

            ButtonSprite btnGetReceipts = new ButtonSprite();
            btnGetReceipts.Initialize(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            btnGetReceipts.Position = new Vector2(1100, 200);
            btnGetReceipts.TextureScale = new Vector2(1.5f, 0.5f);
            btnGetReceipts.Text = "Get Receipts";
            btnGetReceipts.TextOffset = new Vector2(30, 20);
            m_buttons.Add(btnGetReceipts);

            m_focusManager.SelectedButton = btnGetProducts;
            m_focusManager.Mappings[btnGetProducts] = new FocusManager.ButtonMapping()
                                                          {
                                                              Right = btnPurchase
                                                          };
            m_focusManager.Mappings[btnPurchase] = new FocusManager.ButtonMapping()
                                                       {
                                                           Left = btnGetProducts,
                                                           Right = btnGetReceipts
                                                       };
            m_focusManager.Mappings[btnGetReceipts] = new FocusManager.ButtonMapping()
            {
                Left = btnPurchase,
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
