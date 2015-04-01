using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Krypton
{
    public static class LightTextureBuilder
    {
        /// <summary>
        /// Create a Point Light based on a size.
        /// </summary>
        /// <param name="device">Your game's GraphicsDevice</param>
        /// <param name="size">Maximum Size</param>
        /// <returns>Light Texture</returns>
        public static Texture2D CreatePointLight(GraphicsDevice device, int size)
        {
            return CreateConicLight(device, size, MathHelper.TwoPi, 0);
        }

        /// <summary>
        /// Create a Conic Light based on the size, and field of view.
        /// </summary>
        /// <param name="device">Your game's GraphicsDevice</param>
        /// <param name="size">Maximum Size</param>
        /// <param name="FOV">Maximum Field of View</param>
        /// <returns>Light Texture</returns>
        public static Texture2D CreateConicLight(GraphicsDevice device, int size, float FOV)
        {
            return CreateConicLight(device, size, FOV, 0);
        }

        /// <summary>
        /// Create a Conic Light based on the size, field of view, and near plane distance.
        /// </summary>
        /// <param name="device">Your game's GraphicsDevice</param>
        /// <param name="size">Maximum size</param>
        /// <param name="FOV">Maximum Field of View</param>
        /// <param name="nearPlaneDistance">Prevents texture from being drawn at this plane distance, originating from the center of light</param>
        /// <returns>Light Texture</returns>
        public static Texture2D CreateConicLight(GraphicsDevice device, int size, float FOV, float nearPlaneDistance)
        {
            /*if (!IsPowerOfTwo(size))
                throw new Exception("The size must be a power of 2");*/
            float[,] Data = new float[size, size];

            float center = size / 2;

            FOV = FOV / 2;

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    float Distance = Vector2.Distance(new Vector2(x, y), new Vector2(center));

                    Vector2 Difference = new Vector2(x, y) - new Vector2(center);
                    float Angle = (float)Math.Atan2(Difference.Y, Difference.X);

                    if (Distance <= center && Distance >= nearPlaneDistance && Math.Abs(Angle) <= FOV)
                        Data[x, y] = (center - Distance) / center;
                    else
                        Data[x, y] = 0;
                }

            Texture2D tex = new Texture2D(device, size, size);

            Color[] Data1D = new Color[size * size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    Data1D[x + y * size] = new Color(new Vector3(Data[x, y]));

            tex.SetData<Color>(Data1D);

            return tex;
        }

        /// <summary>
        /// Math helper to determine if integer is a power of two
        /// </summary>
        /// <param name="x">Integer value</param>
        private static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }
    }
}
