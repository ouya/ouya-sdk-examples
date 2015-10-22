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
        Texture2D _mLight = null;

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

            _mProjectilePrefab = new Projectile(color, new Vector2(-100, -100), 10, 10, Vector2.Zero);
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
            Color[] data = new Color[_mRectangle.Width * _mRectangle.Height];
            _mTexture = new Texture2D(graphicsDevice, _mRectangle.Width, _mRectangle.Height);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = _mColor;
            }
            _mTexture.SetData(data);

            _mLight = CreateCircle(graphicsDevice, (int)(_mRectangle.Width*0.75f));

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
            spriteBatch.Draw(_mLight, _mPosition - new Vector2(_mRectangle.Width * 0.25f, _mRectangle.Height * 0.25f), Color.White);
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
                    Game1._sProjectiles.Add(new Projectile(_mProjectilePrefab,
                        _mPosition + new Vector2(_mRectangle.Width / 2, _mRectangle.Height / 2), direction));
                }
            }
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