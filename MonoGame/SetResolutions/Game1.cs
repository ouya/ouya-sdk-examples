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
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _font;

        private string _resolution = string.Empty;

        private FocusManager _focusManager = new FocusManager();

        private List<ButtonSprite> _buttons = new List<ButtonSprite>();

        private ButtonSprite _btn480 = null;
        private ButtonSprite _btn720 = null;
        private ButtonSprite _btn1080 = null;
        public ButtonSprite _btnPause = null;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

            Window.Holder.SetFixedSize(_graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight);

            _focusManager.OnClick += OnClick;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _btnPause = new ButtonSprite();
            _btn480 = new ButtonSprite();
            _btn720 = new ButtonSprite();
            _btn1080 = new ButtonSprite();

            _buttons.Add(_btnPause);
            _buttons.Add(_btn480);
            _buttons.Add(_btn720);
            _buttons.Add(_btn1080);

            _btnPause.Position = new Vector2(100, 225);
            _btnPause.TextureScale = new Vector2(1f, 0.5f);
            _btnPause.Text = "Pause";
            _btnPause.TextOffset = new Vector2(30, 20);

            _btn480.Position = new Vector2(250, 225);
            _btn480.TextureScale = new Vector2(0.5f, 0.5f);
            _btn480.Text = "480";
            _btn480.TextOffset = new Vector2(15, 20);

            _btn720.Position = new Vector2(325, 225);
            _btn720.TextureScale = new Vector2(0.5f, 0.5f);
            _btn720.Text = "720";
            _btn720.TextOffset = new Vector2(15, 20);

            _btn1080.Position = new Vector2(400, 225);
            _btn1080.TextureScale = new Vector2(0.6f, 0.5f);
            _btn1080.Text = "1080";
            _btn1080.TextOffset = new Vector2(15, 20);

            _focusManager.Mappings[_btnPause] = new FocusManager.ButtonMapping()
            {
                Right = _btn480,
            };

            _focusManager.Mappings[_btn480] = new FocusManager.ButtonMapping()
            {
                Right = _btn720,
            };

            _focusManager.Mappings[_btn720] = new FocusManager.ButtonMapping()
            {
                Left = _btn480,
                Right = _btn1080,
            };

            _focusManager.Mappings[_btn1080] = new FocusManager.ButtonMapping()
            {
                Left = _btn720,
            };

            _focusManager.SelectedButton = _btn720;
            _resolution = "1920x1080";

            base.Initialize();
        }

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs ||
                null == clickEventArgs.Button)
            {
                return;
            }

            _focusManager.SelectedButton = clickEventArgs.Button;

            if (clickEventArgs.Button == _btn480)
            {
                _resolution = "640x480";
                _graphics.PreferredBackBufferWidth = 640;
                _graphics.PreferredBackBufferHeight = 480;
                Window.Holder.SetFixedSize(_graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight);
            }

            if (clickEventArgs.Button == _btn720)
            {
                _resolution = "1280x720";
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;
                Window.Holder.SetFixedSize(_graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight);
            }

            if (clickEventArgs.Button == _btn1080)
            {
                _resolution = "1920x1080";
                _graphics.PreferredBackBufferWidth = 1920;
                _graphics.PreferredBackBufferHeight = 1080;
                Window.Holder.SetFixedSize(_graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _font = Content.Load<SpriteFont>("spriteFont1");

            _btnPause.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));

            _btn480.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));

            _btn720.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));

            _btn1080.Initialize(_font,
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
            foreach (ButtonSprite button in _buttons)
            {
                if (button == _focusManager.SelectedButton)
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
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_font, "Hello From MonoGame!", new Vector2(100, 100), Color.White);
            _spriteBatch.DrawString(_font, "DPAD: Left+Right - Changes Focus", new Vector2(100, 125), Color.White);
            _spriteBatch.DrawString(_font, "BUTTON: O - Switch to Selected Resolution", new Vector2(100, 150), Color.White);
            _spriteBatch.DrawString(_font, string.Format("RESOLUTION: {0}", _resolution), new Vector2(100, 175), Color.White);

            foreach (ButtonSprite button in _buttons)
            {
                button.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public FocusManager GetFocusManager()
        {
            return _focusManager;
        }
    }
}
