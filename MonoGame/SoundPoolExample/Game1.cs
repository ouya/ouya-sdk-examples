using System;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        private SoundPool m_soundPool;
        private int m_soundBoom;
        private int m_soundOuch;
        private DateTime m_timerSound = DateTime.MinValue;
        private Random m_random = new Random(123);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
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

            m_soundPool = new SoundPool(5, Stream.Music, 0);

            AssetFileDescriptor assetFileDescriptor;

            assetFileDescriptor = Activity.ApplicationContext.Assets.OpenFd("Content/ouch.wav");
            m_soundBoom = m_soundPool.Load(assetFileDescriptor, 1);

            assetFileDescriptor = Activity.ApplicationContext.Assets.OpenFd("Content/boom.wav");
            m_soundOuch = m_soundPool.Load(assetFileDescriptor, 1);
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

            if (m_timerSound < DateTime.Now)
            {
                m_timerSound = DateTime.Now + TimeSpan.FromSeconds(1);

                switch (m_random.Next(0, 2))
                {
                    case 0:
                        m_soundPool.Play(m_soundBoom, 0.99f, 0.99f, 0, 0, 1);
                        break;
                    case 1:
                        m_soundPool.Play(m_soundOuch, 0.99f, 0.99f, 0, 0, 1);
                        break;
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
            spriteBatch.DrawString(font, "MonoGame SoundPool example!", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
