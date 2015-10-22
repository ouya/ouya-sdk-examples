using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDown
{
    public class Projectile : Entity
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
        public float _mLifeTime = 1000f;

        /// <summary>
        /// Size of the projectile
        /// </summary>
        private Rectangle _mRectangle;

        /// <summary>
        /// Texture of the projectile
        /// </summary>
        public Texture2D _mTexture = null;

        /// <summary>
        /// Light of the projectile
        /// </summary>
        private Light _mLight = null;

        /// <summary>
        /// Color of the projectile
        /// </summary>
        private Color _mColor;

        /// <summary>
        /// Speed of the projectile
        /// </summary>
        private const float PROJECTILE_SPEED = 2f;

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
            _mLight = new Light(this, width*8, height*8);
        }

        public Projectile(Projectile projectile, Vector2 position, Vector2 direction)
        {
            _mColor = projectile._mColor;
            _mRectangle = projectile._mRectangle;
            _mTexture = projectile._mTexture;
            _mPosition = position;
            _mDirection = direction;

            _mLight = new Light(projectile._mLight, this);
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

            _mLight.LoadContent(graphicsDevice);
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

            _mPosition += _mDirection * deltaTime * PROJECTILE_SPEED;

            _mLifeTime -= deltaTime;
        }

        /// <summary>
        /// Draw the projectile light
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLight(SpriteBatch spriteBatch)
        {
            _mLight.Draw(spriteBatch);
        }
    }
}
