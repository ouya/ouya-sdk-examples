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
        /// Ref: http://stackoverflow.com/questions/5641579/xna-draw-a-filled-circle
        /// </summary>
        /// <param name="importedGraphicsDevice"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Texture2D CreateCircle(GraphicsDevice graphicsDevice, int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(graphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            //width
            for (int i = 0; i < outerRadius; i++)
            {
                int yStart = -1;
                int yEnd = -1;

                //loop through height to find start and end to fill
                for (int j = 0; j < outerRadius; j++)
                {

                    if (yStart == -1)
                    {
                        if (j == outerRadius - 1)
                        {
                            //last row so there is no row below to compare to
                            break;
                        }

                        //start is indicated by Color followed by Transparent
                        if (data[i + (j * outerRadius)] == Color.White && data[i + ((j + 1) * outerRadius)] == Color.Transparent)
                        {
                            yStart = j + 1;
                            continue;
                        }
                    }
                    else if (data[i + (j * outerRadius)] == Color.White)
                    {
                        yEnd = j;
                        break;
                    }
                }

                //if we found a valid start and end position
                if (yStart != -1 && yEnd != -1)
                {
                    //height
                    for (int j = yStart; j < yEnd; j++)
                    {
                        data[i + (j * outerRadius)] = Color.White;
                    }
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
            spriteBatch.Draw(_mTexture, _mParent.GetPosition() - new Vector2(_mRectangle.Width * 0.25f, _mRectangle.Height * 0.25f), Color.White);
        }
    }
}
