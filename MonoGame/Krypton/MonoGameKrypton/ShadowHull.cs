using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Krypton.Common;

namespace Krypton
{
    /// <summary>
    /// A hull used for casting shadows from a light source
    /// </summary>
    public class ShadowHull
    {
        #region Orientation

        /// <summary>
        /// The position of the shadow hull
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The angle of the shadow hull
        /// </summary>
        public float Angle;

        #endregion

        #region Shape

        /// <summary>
        /// The maximum radius in which all of the shadow hull's vertices are contained, originating from the hull's position
        /// </summary>
        public float MaxRadius;

        /// <summary>
        /// The vertices comprising the shadow hull
        /// </summary>
        public ShadowHullPoint[] Points;

        /// <summary>
        /// The number of vertices comprising the shadow hull
        /// </summary>
        public int NumPoints;

        /// <summary>
        /// The indicies used to render the shadow hull
        /// </summary>
        public Int32[] Indicies;

        /// <summary>
        /// The number of indicies used to render the shadow hull
        /// </summary>
        public int NumIndicies;

        /// <summary>
        /// A value indicating if the hull should cast a shadow
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// A value indicating how much to stretch the hull
        /// </summary>
        public Vector2 Scale = Vector2.One;

        #endregion

        private ShadowHull(){}

        #region Factory Methods

        /// <summary>
        /// Creates a rectangular shadow hull
        /// </summary>
        /// <param name="size">The dimensions of the rectangle</param>
        /// <returns>A rectangular shadow hull</returns>
        public static ShadowHull CreateRectangle(Vector2 size)
        {
            ShadowHull hull = new ShadowHull();

            size *= 0.5f;

            hull.MaxRadius = (float)Math.Sqrt(size.X * size.X + size.Y * size.Y);

            hull.NumPoints = 4 * 2;
            var numTris = hull.NumPoints - 2;
            hull.NumIndicies = numTris * 3;

            hull.Points = new ShadowHullPoint[hull.NumPoints];
            hull.Indicies = new Int32[hull.NumIndicies];

            // Vertex position
            var posTR = new Vector2(+size.X, +size.Y);
            var posBR = new Vector2(+size.X, -size.Y);
            var posBL = new Vector2(-size.X, -size.Y);
            var posTL = new Vector2(-size.X, +size.Y);

            // Right
            hull.Points[0] = new ShadowHullPoint(posTR, Vector2.UnitX);
            hull.Points[1] = new ShadowHullPoint(posBR, Vector2.UnitX);
                                                              
            // Bottom                                         
            hull.Points[2] = new ShadowHullPoint(posBR, -Vector2.UnitY);
            hull.Points[3] = new ShadowHullPoint(posBL, -Vector2.UnitY);
                                                              
            // Left                                           
            hull.Points[4] = new ShadowHullPoint(posBL, -Vector2.UnitX);
            hull.Points[5] = new ShadowHullPoint(posTL, -Vector2.UnitX);

            // Top
            hull.Points[6] = new ShadowHullPoint(posTL, Vector2.UnitY);
            hull.Points[7] = new ShadowHullPoint(posTR, Vector2.UnitY);

            // Create tris
            for (int i = 0; i < numTris; i++)
            {
                hull.Indicies[i * 3 + 0] = 0;
                hull.Indicies[i * 3 + 1] = i + 1;
                hull.Indicies[i * 3 + 2] = i + 2;
            }

            return hull;
        }

        /// <summary>
        /// Creates a circular shadow hull
        /// </summary>
        /// <param name="radius">radius of the circle</param>
        /// <param name="sides">number of sides the circle will be comprised of</param>
        /// <returns>A circular shadow hull</returns>
        public static ShadowHull CreateCircle(float radius, int sides)
        {
            // Validate input
            if (sides < 3) { throw new ArgumentException("Shadow hull must have at least 3 sides."); }

            ShadowHull hull = new ShadowHull();

            hull.MaxRadius = radius;

            // Calculate number of sides
            hull.NumPoints = sides * 2;
            var numTris = hull.NumPoints - 2;
            hull.NumIndicies = numTris * 3;
            
            hull.Points = new ShadowHullPoint[hull.NumPoints];
            hull.Indicies = new Int32[hull.NumIndicies];

            var angle = (float)(-Math.PI * 2) / sides; // XNA Renders Clockwise
            var angleOffset = angle / 2;

            for (int i = 0; i < sides; i++)
            {
                // Create vertices
                var v1 = new ShadowHullPoint();
                var v2 = new ShadowHullPoint();

                // Vertex Position
                v1.Position.X = (float)Math.Cos(angle * i) * radius;
                v1.Position.Y = (float)Math.Sin(angle * i) * radius;

                v2.Position.X = (float)Math.Cos(angle * (i + 1)) * radius;
                v2.Position.Y = (float)Math.Sin(angle * (i + 1)) * radius;

                // Vertex Normal
                v1.Normal.X = (float)Math.Cos(angle * i + angleOffset);
                v1.Normal.Y = (float)Math.Sin(angle * i + angleOffset);

                v2.Normal.X = (float)Math.Cos(angle * i + angleOffset);
                v2.Normal.Y = (float)Math.Sin(angle * i + angleOffset);

                // Copy vertices
                hull.Points[i * 2 + 0] = v1;
                hull.Points[i * 2 + 1] = v2;
            }

            for (int i = 0; i < numTris; i++)
            {
                hull.Indicies[i * 3 + 0] = 0;
                hull.Indicies[i * 3 + 1] = (Int32)(i + 1);
                hull.Indicies[i * 3 + 2] = (Int32)(i + 2);
            }

            return hull;
        }

        /// <summary>
        /// Creates a custom shadow hull based on a series of vertices
        /// </summary>
        /// <param name="points">The points which the shadow hull will be comprised of</param>
        /// <returns>A custom shadow hulll</returns>
        public static ShadowHull CreateConvex(ref Vector2[] points)
        {
            // Validate input
            if (points == null) { throw new ArgumentNullException("Points cannot be null."); }
            if (points.Length < 3) { throw new ArgumentException("Need at least 3 points to create shadow hull."); }

            var numPoints = points.Length;

            ShadowHull hull = new ShadowHull();

            hull.NumPoints = numPoints * 2;
            var numTris = hull.NumPoints - 2;
            hull.NumIndicies = numTris * 3;

            hull.Points = new ShadowHullPoint[hull.NumPoints];
            hull.Indicies = new Int32[hull.NumIndicies];

            Vector2 pointMin = points[0];
            Vector2 pointMax = points[0];

            for (int i = 0; i < numPoints; i++)
            {
                var p1 = points[(i + 0) % numPoints];
                var p2 = points[(i + 1) % numPoints];

                hull.MaxRadius = Math.Max(hull.MaxRadius, p1.Length());

                var line = p2 - p1;

                var normal = new Vector2(-line.Y, +line.X);

                normal.Normalize();

                hull.Points[i * 2 + 0] = new ShadowHullPoint(p1, normal);
                hull.Points[i * 2 + 1] = new ShadowHullPoint(p2, normal);
            }

            for (Int32 i = 0; i < numTris; i++)
            {
                hull.Indicies[i * 3 + 0] = 0;
                hull.Indicies[i * 3 + 1] = (Int32)(i + 1);
                hull.Indicies[i * 3 + 2] = (Int32)(i + 2);
            }

            return hull;
        }

        #endregion
    }
}
