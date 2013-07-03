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
using Microsoft.Xna.Framework.Input;

namespace VirtualController
{
    class VirtualControllerSprite
    {
        /// <summary>
        /// Scale axis graphics but this amount
        /// </summary>
        private const float AXIS_SCALER = 4f;

        /// <summary>
        /// The controller index
        /// </summary>
        public PlayerIndex Index = PlayerIndex.One;

        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        public Texture2D ButtonA = null;
        public Texture2D ButtonO = null;
        public Texture2D ButtonU = null;
        public Texture2D ButtonY = null;
        public Texture2D Controller = null;
        public Texture2D DpadDown = null;
        public Texture2D DpadLeft = null;
        public Texture2D DpadRight = null;
        public Texture2D DpadUp = null;
        public Texture2D LeftBumper = null;
        public Texture2D LeftTrigger = null;
        public Texture2D LeftStickActive = null;
        public Texture2D LeftStickInactive = null;
        public Texture2D RightBumper = null;
        public Texture2D RightTrigger = null;
        public Texture2D RightStickActive = null;
        public Texture2D RightStickInactive = null;

        private float DeadZone = 0.25f;

        /// <summary>
        /// Set the texture references
        /// </summary>
        public void Initialize(
            Texture2D buttonA,
            Texture2D controller,
            Texture2D dpadDown,
            Texture2D dpadLeft,
            Texture2D dpadRight,
            Texture2D dpadUp,
            Texture2D leftBumper,
            Texture2D leftTrigger,
            Texture2D leftStickInactive,
            Texture2D buttonO,
            Texture2D rightBumper,
            Texture2D rightTrigger,
            Texture2D rightStickInactive,
            Texture2D leftStickActive,
            Texture2D rightStickActive,
            Texture2D buttonU,
            Texture2D buttonY)
        {
            ButtonA = buttonA;
            ButtonO = buttonO;
            ButtonU = buttonU;
            ButtonY = buttonY;
            Controller = controller;
            DpadDown = dpadDown;
            DpadLeft = dpadLeft;
            DpadRight = dpadRight;
            DpadUp = dpadUp;
            LeftBumper = leftBumper;
            LeftTrigger = leftTrigger;
            LeftStickActive = leftStickActive;
            LeftStickInactive = leftStickInactive;
            RightBumper = rightBumper;
            RightTrigger = rightTrigger;
            RightStickActive = rightStickActive;
            RightStickInactive = rightStickInactive;
        }

        private void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            Draw(spriteBatch, texture, Position);
        }

        private void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
        {
            if (null != texture)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        private GamePadState GetState()
        {
            return GamePad.GetState(Index);
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Controller);

            GamePadState state = GetState();

            #region DPADS

            if (state.DPad.Down == ButtonState.Pressed)
            {
                Draw(spriteBatch, DpadDown);
            }

            if (state.DPad.Left == ButtonState.Pressed)
            {
                Draw(spriteBatch, DpadLeft);
            }

            if (state.DPad.Right == ButtonState.Pressed)
            {
                Draw(spriteBatch, DpadRight);
            }

            if (state.DPad.Up == ButtonState.Pressed)
            {
                Draw(spriteBatch, DpadUp);
            }

            #endregion

            #region Buttons

            if (state.Buttons.B == ButtonState.Pressed)
            {
                Draw(spriteBatch, ButtonA);
            }

            if (state.Buttons.A == ButtonState.Pressed)
            {
                Draw(spriteBatch, ButtonO);
            }

            if (state.Buttons.X == ButtonState.Pressed)
            {
                Draw(spriteBatch, ButtonU);
            }

            if (state.Buttons.Y == ButtonState.Pressed)
            {
                Draw(spriteBatch, ButtonY);
            }

            if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                Draw(spriteBatch, LeftBumper);
            }

            if (state.Buttons.RightShoulder == ButtonState.Pressed)
            {
                Draw(spriteBatch, RightBumper);
            }

            #endregion

            #region Triggers

            if (state.Triggers.Left > DeadZone)
            {
                Draw(spriteBatch, LeftTrigger);
            }

            if (state.Triggers.Right > DeadZone)
            {
                Draw(spriteBatch, RightTrigger);
            }

            #endregion

            #region Sticks

            //rotate input by N degrees to match image
            float degrees = 135;
            float radians = degrees / 180f * 3.14f;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            Vector2 input = state.ThumbSticks.Left;

            if (state.Buttons.LeftStick == ButtonState.Pressed ||
                Math.Abs(input.X) > DeadZone ||
                Math.Abs(input.Y) > DeadZone)
            {
                Draw(spriteBatch, LeftStickActive, Position + AXIS_SCALER * new Vector2(input.X * cos - input.Y * sin, input.X * sin + input.Y * cos));
            }
            else
            {
                Draw(spriteBatch, LeftStickInactive, Position + AXIS_SCALER * new Vector2(input.X * cos - input.Y * sin, input.X * sin + input.Y * cos));
            }

            //rotate by same degrees
            input = state.ThumbSticks.Right;

            if (state.Buttons.RightStick == ButtonState.Pressed ||
                Math.Abs(input.X) > DeadZone ||
                Math.Abs(input.Y) > DeadZone)
            {
                Draw(spriteBatch, RightStickActive, Position + AXIS_SCALER * new Vector2(input.X * cos - input.Y * sin, input.X * sin + input.Y * cos));
            }
            else
            {
                Draw(spriteBatch, RightStickInactive, Position + AXIS_SCALER * new Vector2(input.X * cos - input.Y * sin, input.X * sin + input.Y * cos));
            }

            #endregion
        }


    }
}