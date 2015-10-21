using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

namespace TopDown
{
    public class Player
    {
        /// <summary>
        /// The controller index
        /// </summary>
        private int _mIndex = 0;

        private Color _mColor;

        /// <summary>
        /// The axis deadzone
        /// </summary>
        private const float DEAD_ZONE = 0.25f;

        /// <summary>
        /// Position of the player
        /// </summary>
        private Vector2 _mPosition = Vector2.Zero;

        /// <summary>
        /// Size of the player
        /// </summary>
        private Rectangle _mRectangle;

        /// <summary>
        /// Texture of the player
        /// </summary>
        Texture2D _mTexture = null;

        /// <summary>
        /// Construct the player
        /// </summary>
        /// <param name="index"></param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Player(int index, Color color, int width, int height)
        {
            _mIndex = index;
            _mColor = color;
            _mRectangle = new Rectangle(0, 0, width, height);
            Random random = new Random();
            _mPosition.X = random.Next() % Game1._sGraphics.PreferredBackBufferWidth;
            _mPosition.Y = random.Next() % Game1._sGraphics.PreferredBackBufferHeight;
            PositionPlayerInBounds();
        }

        /// <summary>
        /// Create the texture for the player
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            Color[] data = new Color[_mRectangle.Width * _mRectangle.Height];
            _mTexture = new Texture2D(graphicsDevice, _mRectangle.Width, _mRectangle.Height);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = _mColor;
            }
            _mTexture.SetData(data);
        }

        /// <summary>
        /// Draw the player
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameTime gameTime)
        {
            spriteBatch.Draw(_mTexture, _mPosition, Color.White);
            spriteBatch.DrawString(font, string.Format("{0}", _mIndex + 1),
                _mPosition + new Vector2(_mRectangle.Width / 2 - 5, _mRectangle.Height / 2 - 10),
                Color.Black);

            float lsX = OuyaInput.GetAxis(_mIndex, OuyaController.AXIS_LS_X);
            if (Math.Abs(lsX) > DEAD_ZONE)
            {
                _mPosition.X += lsX * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            float lsY = OuyaInput.GetAxis(_mIndex, OuyaController.AXIS_LS_Y);
            if (Math.Abs(lsY) > DEAD_ZONE)
            {
                _mPosition.Y += lsY * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            PositionPlayerInBounds();
        }

        /// <summary>
        /// Position the player in bounds
        /// </summary>
        private void PositionPlayerInBounds()
        {
            _mPosition.X = Math.Max(0f, _mPosition.X);
            _mPosition.X = Math.Min(Game1._sGraphics.PreferredBackBufferWidth - _mRectangle.Width, _mPosition.X);
            _mPosition.Y = Math.Max(0f, _mPosition.Y);
            _mPosition.Y = Math.Min(Game1._sGraphics.PreferredBackBufferHeight - _mRectangle.Height, _mPosition.Y);
        }
    }
}