using System.IO;
using Android.Graphics;
using Android.Graphics.Drawables;
using Java.IO;
using Java.Lang;
using Java.Nio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using StringBuilder = System.Text.StringBuilder;

namespace InputView
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;

		public List<VirtualControllerSprite> Controllers = new List<VirtualControllerSprite>();

		private List<Texture2D> m_controllerButtons = new List<Texture2D>();

		public bool m_waitForExit = true;

		public Game1()
		{
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
			Vector2 position = new Vector2();
			for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index)
			{
				VirtualControllerSprite controller = new VirtualControllerSprite();
				switch (index)
				{
				case 0:
					position = new Vector2(500, 200);
					break;
				case 1:
					position = new Vector2(1100, 200);
					break;
				case 2:
					position = new Vector2(500, 500);
					break;
				case 3:
					position = new Vector2(1100, 500);
					break;
				}                
				controller.Index = index;
				controller.Position = position;
				Controllers.Add(controller);
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

			font = Content.Load<SpriteFont>("spriteFont1");

			int[] buttons =
			{
				OuyaController.BUTTON_O,
				OuyaController.BUTTON_U,
				OuyaController.BUTTON_Y,
				OuyaController.BUTTON_A,
				OuyaController.BUTTON_L1,
				OuyaController.BUTTON_L3,
				OuyaController.BUTTON_R1,
				OuyaController.BUTTON_R3,
				OuyaController.BUTTON_DPAD_DOWN,
				OuyaController.BUTTON_DPAD_LEFT,
				OuyaController.BUTTON_DPAD_RIGHT,
				OuyaController.BUTTON_DPAD_UP,
				OuyaController.BUTTON_MENU,
			};

			foreach (VirtualControllerSprite controller in Controllers)
			{
				controller.Initialize(
					Content.Load<Texture2D>("Graphics\\a"),
					Content.Load<Texture2D>("Graphics\\controller"),
					Content.Load<Texture2D>("Graphics\\dpad_down"),
					Content.Load<Texture2D>("Graphics\\dpad_left"),
					Content.Load<Texture2D>("Graphics\\dpad_right"),
					Content.Load<Texture2D>("Graphics\\dpad_up"),
					Content.Load<Texture2D>("Graphics\\lb"),
					Content.Load<Texture2D>("Graphics\\lt"),
					Content.Load<Texture2D>("Graphics\\l_stick"),
					Content.Load<Texture2D>("Graphics\\menu"),
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
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();
			spriteBatch.DrawString (font, "Hello from MonoGame: "+DateTime.Now.ToString(), new Vector2 (Window.Width / 2 - 100, Window.Height / 2), Color.White);
			Vector2 position = new Vector2 (400, 100);
			int index = 0;
			for (; index < m_controllerButtons.Count && index < 7; ++index) { 
				Texture2D texture = m_controllerButtons [index];
				if (null != texture) {
					spriteBatch.Draw (texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					position.X += texture.Width;
				}
			}
			foreach (VirtualControllerSprite controller in Controllers) {
				controller.Draw (spriteBatch);
			}

			position = new Vector2 (400, 800);
			for (; index < m_controllerButtons.Count; ++index) {
				Texture2D texture = m_controllerButtons [index];
				if (null != texture) {
					spriteBatch.Draw (texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					position.X += texture.Width;
				}
			}
			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}
