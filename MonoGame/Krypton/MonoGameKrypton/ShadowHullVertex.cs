using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Krypton
{
    public struct ShadowHullVertex : IVertexType
    {
        /// <summary>
        /// The position of the vertex
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The normal of the vertex
        /// </summary>
        public Vector2 Normal;

        /// <summary>
        /// The color of vertex
        /// </summary>
        public Color Color;

        private static readonly VertexDeclaration mVertexDeclaration;

        /// <summary>
        /// 
        /// </summary>
        public VertexDeclaration VertexDeclaration { get { return ShadowHullVertex.mVertexDeclaration; } }

        static ShadowHullVertex()
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.Normal,0),
                new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color,0),
            };

            mVertexDeclaration = new VertexDeclaration(elements);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="opacity"></param>
        public ShadowHullVertex(Vector2 position, Vector2 normal, Color color)
        {
            this.Position = position;
            this.Normal = normal;
            this.Color = color;
        }
    }
}
