using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Media;
using Java.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OggExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private FocusManager _focusManager = new FocusManager();

        private List<ButtonSprite> _buttons = new List<ButtonSprite>();

        private ButtonSprite _btnFadeIn1 = null;
        private ButtonSprite _btnFadeIn2 = null;
        private ButtonSprite _btnFadeOut1 = null;
        private ButtonSprite _btnFadeOut2 = null;
        private ButtonSprite _btnCrossFade1 = null;
        private ButtonSprite _btnCrossFade2 = null;

        private CustomMediaPlayer _mpSound1 = null;
        private CustomMediaPlayer _mpSound2 = null;

        private const string SOUND_1 = "Content/sound1.ogg";
        private const string SOUND_2 = "Content/sound2.ogg";

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

            _mpSound1 = new CustomMediaPlayer();
            _mpSound2 = new CustomMediaPlayer();

            int center = _graphics.PreferredBackBufferWidth/2;
            int y = 200;
            int offset = 75;

            _btnFadeIn1 = new ButtonSprite();
            _btnFadeIn1.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnFadeIn1.Position = new Vector2(center, y += offset);
            _btnFadeIn1.TextureScale = new Vector2(2f, 0.5f);
            _btnFadeIn1.Text = "Fade In 1";
            _btnFadeIn1.TextOffset = new Vector2(60, 20);
            _buttons.Add(_btnFadeIn1);

            _btnFadeIn2 = new ButtonSprite();
            _btnFadeIn2.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnFadeIn2.Position = new Vector2(center, y += offset);
            _btnFadeIn2.TextureScale = new Vector2(2f, 0.5f);
            _btnFadeIn2.Text = "Fade In 2";
            _btnFadeIn2.TextOffset = new Vector2(60, 20);
            _buttons.Add(_btnFadeIn2);

            _btnFadeOut1 = new ButtonSprite();
            _btnFadeOut1.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnFadeOut1.Position = new Vector2(center, y += offset);
            _btnFadeOut1.TextureScale = new Vector2(2f, 0.5f);
            _btnFadeOut1.Text = "Fade Out 1";
            _btnFadeOut1.TextOffset = new Vector2(60, 20);
            _buttons.Add(_btnFadeOut1);

            _btnFadeOut2 = new ButtonSprite();
            _btnFadeOut2.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnFadeOut2.Position = new Vector2(center, y += offset);
            _btnFadeOut2.TextureScale = new Vector2(2f, 0.5f);
            _btnFadeOut2.Text = "Fade Out 2";
            _btnFadeOut2.TextOffset = new Vector2(60, 20);
            _buttons.Add(_btnFadeOut2);

            _btnCrossFade1 = new ButtonSprite();
            _btnCrossFade1.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnCrossFade1.Position = new Vector2(center, y += offset);
            _btnCrossFade1.TextureScale = new Vector2(2f, 0.5f);
            _btnCrossFade1.Text = "Cross Fade 1";
            _btnCrossFade1.TextOffset = new Vector2(60, 20);
            _buttons.Add(_btnCrossFade1);

            _btnCrossFade2 = new ButtonSprite();
            _btnCrossFade2.Initialize(_font,
                Content.Load<Texture2D>("Graphics\\ButtonActive"),
                Content.Load<Texture2D>("Graphics\\ButtonInactive"));
            _btnCrossFade2.Position = new Vector2(center, y += offset);
            _btnCrossFade2.TextureScale = new Vector2(2f, 0.5f);
            _btnCrossFade2.Text = "Cross Fade 2";
            _btnCrossFade2.TextOffset = new Vector2(60, 20);
            _buttons.Add(_btnCrossFade2);

            _focusManager.SelectedButton = _btnFadeIn1;
            _focusManager.Mappings[_btnFadeIn1] = new FocusManager.ButtonMapping()
            {
                Down = _btnFadeIn2
            };
            _focusManager.Mappings[_btnFadeIn2] = new FocusManager.ButtonMapping()
            {
                Up = _btnFadeIn1,
                Down = _btnFadeOut1,
            };
            _focusManager.Mappings[_btnFadeOut1] = new FocusManager.ButtonMapping()
            {
                Up = _btnFadeIn2,
                Down = _btnFadeOut2,
            };
            _focusManager.Mappings[_btnFadeOut2] = new FocusManager.ButtonMapping()
            {
                Up = _btnFadeOut1,
                Down = _btnCrossFade1,
            };
            _focusManager.Mappings[_btnCrossFade1] = new FocusManager.ButtonMapping()
            {
                Up = _btnFadeOut2,
                Down = _btnCrossFade2,
            };
            _focusManager.Mappings[_btnCrossFade2] = new FocusManager.ButtonMapping()
            {
                Up = _btnCrossFade1,
            };
        }

        private void OnClick(object sender, FocusManager.ClickEventArgs clickEventArgs)
        {
            if (null == clickEventArgs ||
                null == clickEventArgs.Button)
            {
                return;
            }

            if (clickEventArgs.Button == _btnFadeIn1)
            {
                CustomMediaPlayer.FadeIn(_mpSound1, SOUND_1);
            }

            if (clickEventArgs.Button == _btnFadeIn2)
            {
                CustomMediaPlayer.FadeIn(_mpSound2, SOUND_2);
            }

            if (clickEventArgs.Button == _btnFadeOut1)
            {
                CustomMediaPlayer.FadeOut(_mpSound1);
            }

            if (clickEventArgs.Button == _btnFadeOut2)
            {
                CustomMediaPlayer.FadeOut(_mpSound2);
            }

            if (clickEventArgs.Button == _btnCrossFade1)
            {
                CustomMediaPlayer.CrossFade(_mpSound2, _mpSound1, SOUND_1);
            }

            if (clickEventArgs.Button == _btnCrossFade2)
            {
                CustomMediaPlayer.CrossFade(_mpSound1, _mpSound2, SOUND_2);
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

            if (null != _mpSound1)
            {
                _mpSound1.Update();
            }
            if (null != _mpSound2)
            {
                _mpSound2.Update();
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
            _spriteBatch.DrawString(_font, "MonoGame Ogg example!", 
                new Vector2(_graphics.PreferredBackBufferWidth / 2, 200), Color.White);
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
