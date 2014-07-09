using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoundPoolExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private SoundPool _soundPool;
        private int _soundBoom;
        private int _soundOuch;

        private FocusManager _focusManager = new FocusManager();

        private List<ButtonSprite> _buttons = new List<ButtonSprite>();

        private ButtonSprite _btnBoom = null;
        private ButtonSprite _btnOuch = null;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

            _focusManager.OnClick += OnClick;
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

            _soundPool = new SoundPool(5, Stream.Music, 0);

            AssetFileDescriptor assetFileDescriptor;

            assetFileDescriptor = Activity.ApplicationContext.Assets.OpenFd("Content/boom.wav");
            _soundBoom = _soundPool.Load(assetFileDescriptor, 1);

            assetFileDescriptor = Activity.ApplicationContext.Assets.OpenFd("Content/ouch.wav");
            _soundOuch = _soundPool.Load(assetFileDescriptor, 1);

            _btnBoom = new ButtonSprite();
            _btnBoom.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnBoom.Position = new Vector2(150, 200);
            _btnBoom.TextureScale = new Vector2(2f, 0.5f);
            _btnBoom.Text = "Boom";
            _btnBoom.TextOffset = new Vector2(40, 20);
            _buttons.Add(_btnBoom);

            _btnOuch = new ButtonSprite();
            _btnOuch.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnOuch.Position = new Vector2(600, 200);
            _btnOuch.TextureScale = new Vector2(2f, 0.5f);
            _btnOuch.Text = "Ouch";
            _btnOuch.TextOffset = new Vector2(40, 20);
            _buttons.Add(_btnOuch);

            _focusManager.SelectedButton = _btnBoom;
            _focusManager.Mappings[_btnBoom] = new FocusManager.ButtonMapping()
            {
                Right = _btnOuch
            };
            _focusManager.Mappings[_btnOuch] = new FocusManager.ButtonMapping()
            {
                Left = _btnBoom,
            };
        }

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs ||
                null == clickEventArgs.Button)
            {
                return;
            }

            if (clickEventArgs.Button == _btnBoom)
            {
                _soundPool.Play(_soundBoom, 0.99f, 0.99f, 0, 0, 1);   
            }

            if (clickEventArgs.Button == _btnOuch)
            {
                _soundPool.Play(_soundOuch, 0.99f, 0.99f, 0, 0, 1);
            }
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
            _spriteBatch.DrawString(_font, "MonoGame SoundPool example!", 
                new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), Color.White);
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
