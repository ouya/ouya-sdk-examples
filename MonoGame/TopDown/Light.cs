using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TopDown
{
    public class Light
    {
        /// <summary>
        /// Parent of the light
        /// </summary>
        private Entity _mParent = null;

        /// <summary>
        /// Size of the light
        /// </summary>
        private Rectangle _mRectangle;

        /// <summary>
        /// Texture of the light
        /// </summary>
        Texture2D _mTexture = null;

        /// <summary>
        /// Construct the light
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Light(Entity parent, int width, int height)
        {
            _mParent = parent;
            _mRectangle = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Copy the texture from an existing light
        /// </summary>
        /// <param name="light"></param>
        /// <param name="parent"></param>
        public Light(Light light, Entity parent)
        {
            _mParent = parent;
            _mRectangle = light._mRectangle;
            _mTexture = light._mTexture;
        }

        /// <summary>
        /// Create a circle texture
        /// </summary>
        /// <param name="importedGraphicsDevice"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Texture2D CreateCircle(GraphicsDevice graphicsDevice, int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(graphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            int i = 0;
            Vector2 middle = new Vector2(radius, radius);
            for (int y = 0; y < outerRadius; ++y)
            {
                for (int x = 0; x < outerRadius; ++x)
                {
                    Vector2 position = new Vector2(x, y);
                    float t = Vector2.Distance(middle, position) / (float)radius;
                    data[i] = Color.Lerp(Color.Black, Color.Transparent, t);
                    ++i;
                }
            }

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// Create the texture for the player
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            _mTexture = CreateCircle(graphicsDevice, (int)(_mRectangle.Width*0.75f));
        }

        /// <summary>
        /// Draw the player
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = _mParent.GetPosition() - new Vector2(
                _mParent.GetRectangle().Width * 0.5f + _mRectangle.Width * 0.5f,
                _mParent.GetRectangle().Height * 0.5f + _mRectangle.Height * 0.5f);
            spriteBatch.Draw(_mTexture, position, Color.White);
        }
    }
}
