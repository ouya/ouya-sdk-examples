using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

namespace TopDown
{
    public class Player : Entity
    {
        /// <summary>
        /// The controller index
        /// </summary>
        private int _mIndex = 0;

        /// <summary>
        /// Color of the player
        /// </summary>
        private Color _mColor;

        /// <summary>
        /// The axis deadzone
        /// </summary>
        private const float DEAD_ZONE = 0.25f;

        /// <summary>
        /// A small direction variance
        /// </summary>
        private const float PROJECTILE_VARIANCE = 0.01f;

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
        /// Light of the player
        /// </summary>
        Light _mLight = null;

        /// <summary>
        /// The projectile prefab fired by the player
        /// </summary>
        Projectile _mProjectilePrefab = null;

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
            _mPosition.X = Game1._sRandom.Next() % Game1._sGraphics.PreferredBackBufferWidth;
            _mPosition.Y = Game1._sRandom.Next() % Game1._sGraphics.PreferredBackBufferHeight;
            PositionPlayerInBounds();

            _mLight = new Light(this, width*2, height*2);

            _mProjectilePrefab = new Projectile(color, new Vector2(-100, -100), 10, 10, Vector2.Zero);
        }

        /// <summary>
        /// Implement Entity
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return _mPosition;
        }

        /// <summary>
        /// Implement Entity
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            return _mRectangle;
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

            _mLight.LoadContent(graphicsDevice, 0.5f, 0.6f, 0.75f);

            _mProjectilePrefab.LoadContent(graphicsDevice);
        }

        /// <summary>
        /// Draw the player
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameTime gameTime)
        {
            spriteBatch.Draw(_mTexture, _mPosition - new Vector2(_mRectangle.Width*0.5f, _mRectangle.Height*0.5f), Color.White);
            spriteBatch.DrawString(font, string.Format("{0}", _mIndex + 1),
                _mPosition + new Vector2(-5, -10),
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

            float rsX = OuyaInput.GetAxis(_mIndex, OuyaController.AXIS_RS_X);
            float rsY = OuyaInput.GetAxis(_mIndex, OuyaController.AXIS_RS_Y);
            if (Math.Abs(rsX) > DEAD_ZONE ||
                Math.Abs(rsY) > DEAD_ZONE)
            {
                Vector2 direction = new Vector2(rsX, rsY);
                Vector2 variance = new Vector2((Game1._sRandom.Next() % 10 * PROJECTILE_VARIANCE), (Game1._sRandom.Next() % 10 * PROJECTILE_VARIANCE));
                direction += variance;
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    Game1._sProjectiles.Add(new Projectile(_mProjectilePrefab, _mPosition, direction));
                }
            }
        }

        /// <summary>
        /// Draw the player light
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLight(SpriteBatch spriteBatch)
        {
            _mLight.Draw(spriteBatch);
        }

        /// <summary>
        /// Position the player in bounds
        /// </summary>
        private void PositionPlayerInBounds()
        {
            _mPosition.X = Math.Max(_mRectangle.Width / 2, _mPosition.X);
            _mPosition.X = Math.Min(Game1._sGraphics.PreferredBackBufferWidth - _mRectangle.Width / 2, _mPosition.X);
            _mPosition.Y = Math.Max(_mRectangle.Height / 2, _mPosition.Y);
            _mPosition.Y = Math.Min(Game1._sGraphics.PreferredBackBufferHeight - _mRectangle.Height / 2, _mPosition.Y);
        }
    }
}