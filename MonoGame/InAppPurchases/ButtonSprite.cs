using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InAppPurchases
{
    class ButtonSprite
    {
        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// Scale the texture
        /// </summary>
        public Vector2 TextureScale = Vector2.Zero;

        /// <summary>
        /// Offset the sprite text
        /// </summary>
        public Vector2 TextOffset = Vector2.Zero;

		/// <summary>
		/// True if the button is active
		/// </summary>
        public bool m_isActive = false;

		/// <summary>
		/// Text to show on the button
		/// </summary>
        public string Text = string.Empty;

        private void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            Draw(spriteBatch, texture, Position);
        }

        private void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
        {
            if (null != texture)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D buttonActive, Texture2D buttonInactive)
        {
			if (m_isActive) {
				Draw (spriteBatch, buttonActive);
			} else {
				Draw (spriteBatch, buttonInactive);
			}
			Activity1.DrawString(spriteBatch, font, Text, Position + TextOffset, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
