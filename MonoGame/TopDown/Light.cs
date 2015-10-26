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

        float Lerp(float from, float to, float value)
        {
            if (value < 0.0f)
                return from;
            else if (value > 1.0f)
                return to;
            return (to - from) * value + from;
        }

        float InverseLerp(float from, float to, float value)
        {
            if (from < to)
            {
                if (value < from)
                    return 0.0f;
                else if (value > to)
                    return 1.0f;
            }
            else
            {
                if (value < to)
                    return 1.0f;
                else if (value > from)
                    return 0.0f;
            }
            return (value - from) / (to - from);
        }

        /// <summary>
        /// Create a cirlce ring,
        /// inner ring should be less than outer ring
        /// 0 <= innerRing < outerRing <= 1
        /// 0 <= ringIntensitiy <= 1
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="radius"></param>
        /// <param name="ringIntensity"></param>
        /// <param name="innerRing"></param>
        /// <param name="outerRing"></param>
        /// <returns></returns>
        private Texture2D CreateCircleRing(GraphicsDevice graphicsDevice, int radius, float ringIntensity, float innerRing, float outerRing)
        {
            float midpoint = (innerRing + outerRing) * 0.5f;
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
                    if (t > innerRing &&
                        t < outerRing)
                    {
                        // put into the 0 to 1 range
                        float temp = InverseLerp(innerRing, outerRing, t);
                        // create a V starting at 0.5f going to 0 and back up to 0.75
                        if (t < midpoint)
                        {
                            t = Lerp(innerRing, ringIntensity, temp * 2);
                        }
                        else
                        {
                            t = Lerp(ringIntensity, outerRing, (temp - 0.5f) * 2);
                        }
                    }
                    data[i] = Color.Lerp(Color.Black, Color.Transparent, t);
                    ++i;
                }
            }

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// Create the light texture
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            _mTexture = CreateCircle(graphicsDevice, (int)(_mRectangle.Width*0.75f));
        }

        /// <summary>
        /// Create the light texture with a ring
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="ringIntensity"></param>
        /// <param name="innerRing"></param>
        /// <param name="outerRing"></param>
        public void LoadContent(GraphicsDevice graphicsDevice, float ringIntensity, float innerRing, float outerRing)
        {
            _mTexture = CreateCircleRing(graphicsDevice, (int)(_mRectangle.Width * 0.75f), ringIntensity, innerRing, outerRing);
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
