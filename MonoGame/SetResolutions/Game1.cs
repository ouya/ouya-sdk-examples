using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SetResolutions
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private string m_resolution = string.Empty;

        private FocusManager m_focusManager = new FocusManager();

        private List<ButtonSprite> m_buttons = new List<ButtonSprite>();

        private ButtonSprite Btn480 = null;
        private ButtonSprite Btn720 = null;
        private ButtonSprite Btn1080 = null;
        private ButtonSprite BtnPause = null;

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

            BtnPause = new ButtonSprite();
            Btn480 = new ButtonSprite();
            Btn720 = new ButtonSprite();
            Btn1080 = new ButtonSprite();

            m_buttons.Add(BtnPause);
            m_buttons.Add(Btn480);
            m_buttons.Add(Btn720);
            m_buttons.Add(Btn1080);

            BtnPause.Position = new Vector2(100, 225);
            BtnPause.TextureScale = new Vector2(1f, 0.5f);
            BtnPause.Text = "Pause";
            BtnPause.TextOffset = new Vector2(30, 20);

            Btn480.Position = new Vector2(250, 225);
            Btn480.TextureScale = new Vector2(0.5f, 0.5f);
            Btn480.Text = "480";
            Btn480.TextOffset = new Vector2(15, 20);

            Btn720.Position = new Vector2(325, 225);
            Btn720.TextureScale = new Vector2(0.5f, 0.5f);
            Btn720.Text = "720";
            Btn720.TextOffset = new Vector2(15, 20);

            Btn1080.Position = new Vector2(400, 225);
            Btn1080.TextureScale = new Vector2(0.6f, 0.5f);
            Btn1080.Text = "1080";
            Btn1080.TextOffset = new Vector2(15, 20);

            m_focusManager.Mappings[BtnPause] = new FocusManager.ButtonMapping()
            {
                Right = Btn480,
            };

            m_focusManager.Mappings[Btn480] = new FocusManager.ButtonMapping()
            {
                Right = Btn720,
            };

            m_focusManager.Mappings[Btn720] = new FocusManager.ButtonMapping()
            {
                Left = Btn480,
                Right = Btn1080,
            };

            m_focusManager.Mappings[Btn1080] = new FocusManager.ButtonMapping()
            {
                Left = Btn720,
            };

            m_focusManager.SelectedButton = Btn1080;
            m_resolution = "1920x1080";

            base.Initialize();
        }

        private void EventOnKeyUp(object sender, GamePad.KeyEventArgs keyEventArgs)
        {
            //m_debugText = string.Format("Detected Key: {0}", keyEventArgs.KeyCode);
            if (keyEventArgs.KeyCode == 82)
            {
                m_focusManager.SelectedButton = BtnPause;
                OnClick(null, new FocusManager.ClickEventArgs() { Button = BtnPause });
            }
        }

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs ||
                null == clickEventArgs.Button)
            {
                return;
            }

            m_focusManager.SelectedButton = clickEventArgs.Button;

            if (clickEventArgs.Button == BtnPause)
            {
            }

            if (clickEventArgs.Button == Btn480)
            {
                m_resolution = "640x480";
            }

            if (clickEventArgs.Button == Btn720)
            {
                m_resolution = "1280x720";
            }

            if (clickEventArgs.Button == Btn1080)
            {
                m_resolution = "1920x1080";
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

            BtnPause.LoadContent(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));

            Btn480.LoadContent(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));

            Btn720.LoadContent(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));

            Btn1080.LoadContent(font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
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

            spriteBatch.DrawString(font, "Hello From MonoGame!", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, "DPAD: Left+Right - Changes Focus", new Vector2(100, 125), Color.White);
            spriteBatch.DrawString(font, "BUTTON: O - Switch to Selected Resolution", new Vector2(100, 150), Color.White);
            spriteBatch.DrawString(font, string.Format("RESOLUTION: {0}", m_resolution), new Vector2(100, 175), Color.White);

            foreach (ButtonSprite button in m_buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
