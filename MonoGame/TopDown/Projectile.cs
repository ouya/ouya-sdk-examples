using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

namespace TopDown
{
    public class Projectile
    {
        /// <summary>
        /// The position of the projectile
        /// </summary>
        private Vector2 _mPosition = Vector2.Zero;

        /// <summary>
        /// The direction the projectile is travelling
        /// </summary>
        private Vector2 _mDirection = Vector2.Zero;

        /// <summary>
        /// The lifetime of the projectile
        /// </summary>
        public float _mLifeTime = 2000f;

        /// <summary>
        /// Size of the projectile
        /// </summary>
        private Rectangle _mRectangle;

        /// <summary>
        /// Texture of the projectile
        /// </summary>
        public Texture2D _mTexture = null;

        /// <summary>
        /// Color of the projectile
        /// </summary>
        private Color _mColor;

        /// <summary>
        /// Construct the player
        /// </summary>
        /// <param name="index"></param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Projectile(Color color, Vector2 position, int width, int height, Vector2 direction)
        {
            _mColor = color;
            _mRectangle = new Rectangle(0, 0, width, height);
            _mPosition = position;
            _mDirection = direction;
        }

        public Projectile(Projectile projectile, Vector2 position, Vector2 direction)
        {
            _mColor = projectile._mColor;
            _mRectangle = projectile._mRectangle;
            _mTexture = projectile._mTexture;
            _mPosition = position;
            _mDirection = direction;
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
        /// Draw the projectile
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GameTime gameTime)
        {
            if (_mDirection == Vector2.Zero)
            {
                return;
            }

            spriteBatch.Draw(_mTexture, _mPosition, Color.White);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _mPosition += _mDirection * deltaTime;

            _mLifeTime -= deltaTime;
        }
    }
}