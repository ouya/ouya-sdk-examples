using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Krypton
{
    /// <summary>
    /// A two dimensional point with normal
    /// </summary>
    public struct ShadowHullPoint
    {
        /// <summary>
        /// Creates a new Shadow Hull Point
        /// </summary>
        /// <param name="position">The position of the point</param>
        /// <param name="normal">The normal of the side which the point helps represent</param>
        public ShadowHullPoint(Vector2 position, Vector2 normal)
        {
            this.Position = position;
            this.Normal = normal;
        }

        /// <summary>
        /// The position of the point
        /// </summary>
        public Vector2 Position;
        
        /// <summary>
        /// The normal of the side of the shadow hull of which the point helps represent
        /// </summary>
        public Vector2 Normal;
    }
}
