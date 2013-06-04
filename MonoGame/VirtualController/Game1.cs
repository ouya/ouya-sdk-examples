using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace VirtualController
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private const int MAX_CONTROLLERS = 4;

        private List<VirtualControllerSprite> m_controllers = new List<VirtualControllerSprite>(); 

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
            for (int index = 0; index < MAX_CONTROLLERS; ++index)
            {
                VirtualControllerSprite controller = new VirtualControllerSprite();
                Vector2 position = new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X + 175 + index * 350,
                    GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                controller.Index = (PlayerIndex)index;
                controller.Position = position;
                m_controllers.Add(controller);
            }
            
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

            foreach (VirtualControllerSprite controller in m_controllers)
            {
                controller.Initialize(
                    Content.Load<Texture2D>("Graphics\\a"),
                    Content.Load<Texture2D>("Graphics\\cutter"),
                    Content.Load<Texture2D>("Graphics\\dpad_down"),
                    Content.Load<Texture2D>("Graphics\\dpad_left"),
                    Content.Load<Texture2D>("Graphics\\dpad_right"),
                    Content.Load<Texture2D>("Graphics\\dpad_up"),
                    Content.Load<Texture2D>("Graphics\\lb"),
                    Content.Load<Texture2D>("Graphics\\lt"),
                    Content.Load<Texture2D>("Graphics\\l_stick"),
                    Content.Load<Texture2D>("Graphics\\o"),
                    Content.Load<Texture2D>("Graphics\\rb"),
                    Content.Load<Texture2D>("Graphics\\rt"),
                    Content.Load<Texture2D>("Graphics\\r_stick"),
                    Content.Load<Texture2D>("Graphics\\thumbl"),
                    Content.Load<Texture2D>("Graphics\\thumbr"),
                    Content.Load<Texture2D>("Graphics\\u"),
                    Content.Load<Texture2D>("Graphics\\y"));
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
            spriteBatch.DrawString(font, "Hello from MonoGame!", new Vector2(100, 100), Color.White);
            foreach (VirtualControllerSprite controller in m_controllers)
            {
                controller.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
