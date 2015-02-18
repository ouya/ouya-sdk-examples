using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;

namespace AsyncContentLoading
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private const int _numberOfTextures = 1000;
        private const int _numberOfWorkers = 8;

        private static Game1 _instance = null;
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();

        private DateTime _startTime = DateTime.MinValue;
        private DateTime _endTime = DateTime.MinValue;

        class AyncLoader
        {
            public int _minRange = 0;
            public int _maxRange = 0;
        }

        public void LoadWorker(object obj)
        {
            AyncLoader loader = obj as AyncLoader;
            int index = loader._minRange;
            while (index < loader._maxRange)
            {
                try
                {
                    Texture2D texture = _instance.Content.Load<Texture2D>(index.ToString());
                    _instance._textures[index] = texture;
                }
                catch (System.Exception)
                {

                }
                ++index;
                Thread.Sleep(1);
            }
        }

        public Game1()
        {
            _instance = this;
            _startTime = DateTime.Now;
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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

            base.Initialize();
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

            int index = 0;
            int increment = (int)Math.Ceiling(_numberOfTextures / (float)_numberOfWorkers);
            while (index <= _numberOfTextures)
            {
                AyncLoader worker = new AyncLoader();
                worker._minRange = index;
                worker._maxRange = Math.Min(_numberOfTextures, worker._minRange + increment);
                ParameterizedThreadStart ts = new ParameterizedThreadStart(LoadWorker);
                Thread thread = new Thread(ts);
                thread.Start(worker);
                index += increment;
            }
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
            
            string message = string.Format("Number of Workers: {0}", _numberOfWorkers);
            spriteBatch.DrawString(font, message, new Vector2(200, 200), Color.White);

            message = string.Format("Number of Textures: {0}", _numberOfTextures);
            spriteBatch.DrawString(font, message, new Vector2(200, 240), Color.White);

            message = string.Format("Loaded {0} textures", _textures.Count);
            spriteBatch.DrawString(font, message, new Vector2(200, 280), Color.White);

            if (_textures.Count == _numberOfTextures &&
                _endTime == DateTime.MinValue)
            {
                _endTime = DateTime.Now;
            }

            if (_endTime == DateTime.MinValue)
            {
                if (_textures.Count == _numberOfTextures)
                {
                    _endTime = DateTime.Now;
                }
                message = string.Format("Elapsed: {0} seconds", (DateTime.Now - _startTime).TotalSeconds);
            }
            else
            {
                message = string.Format("Elapsed: {0} seconds", (_endTime - _startTime).TotalSeconds);
            }
            spriteBatch.DrawString(font, message, new Vector2(200, 320), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
