using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TV.Ouya.Sdk;

namespace TopDown
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager _sGraphics;
        SpriteBatch _mSpriteBatch;
        SpriteFont _mFont;

        public static Random _sRandom = new Random();

        List<Player> _mPlayers = new List<Player>();

        public static List<Projectile> _sProjectiles = new List<Projectile>();

        public Game1()
        {
            _sGraphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            _sGraphics.IsFullScreen = true;
            _sGraphics.PreferredBackBufferWidth = 1920;
            _sGraphics.PreferredBackBufferHeight = 1080;
            _sGraphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create players
            _mPlayers.Add(new Player(0, Color.Orange, 75, 75));
            _mPlayers.Add(new Player(1, Color.Cyan, 75, 75));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the font
            _mFont = Content.Load<SpriteFont>("spriteFont1");

            // Load the players
            foreach (Player player in _mPlayers)
            {
                player.LoadContent(GraphicsDevice);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _sGraphics.GraphicsDevice.Clear(Color.Green);

            _mSpriteBatch.Begin();
            foreach (Projectile projectile in _sProjectiles)
            {
                projectile.Draw(_mSpriteBatch, _mFont, gameTime);
            }
            foreach (Player player in _mPlayers)
            {
                player.Draw(_mSpriteBatch, _mFont, gameTime);
            }
            _mSpriteBatch.DrawString(_mFont, 
                string.Format("Hello from MonoGame: Projectiles={0}", _sProjectiles.Count),
                new Vector2(Window.ClientBounds.Width / 2 - 200, Window.ClientBounds.Height / 2 - 40), Color.White);
            _mSpriteBatch.End();

            OuyaInput.ClearButtonStates();

            base.Draw(gameTime);

            while (_sProjectiles.Count > 0 &&
                _sProjectiles[0]._mLifeTime < 0)
            {
                _sProjectiles.RemoveAt(0);
            }
        }
    }
}
