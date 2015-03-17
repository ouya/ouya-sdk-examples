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

        public SpriteFont Font = null;
        public Texture2D ButtonTexture = null;
        public Texture2D ButtonActive = null;
        public Texture2D ButtonInactive = null;
        public string Text = string.Empty;

        /// <summary>
        /// Set the texture references
        /// </summary>
        public void Initialize(
            SpriteFont font,
            Texture2D buttonActive,
            Texture2D buttonInactive)
        {
            Font = font;
            ButtonActive = buttonActive;
            ButtonInactive = buttonInactive;
            ButtonTexture = ButtonInactive;
        }

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
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, ButtonTexture);
            spriteBatch.DrawString(Font, Text, Position + TextOffset, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }


    }
}