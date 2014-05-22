using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using OuyaSdk;

namespace VirtualController
{
    public class VirtualControllerSprite : OuyaSdk.VirtualController
    {
        /// <summary>
        /// Scale axis graphics but this amount
        /// </summary>
        private const float AXIS_SCALER = 4f;

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
        public Texture2D ButtonMenu = null;
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
            Texture2D buttonMenu,
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
            ButtonMenu = buttonMenu;
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

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Controller);

            #region MENU

            if (TimerMenuDetected > DateTime.Now)
            {
                Draw(spriteBatch, ButtonMenu);
            }

            #endregion

            #region DPADS

            if (IsPressed(OuyaController.BUTTON_DPAD_DOWN))
            {
                Draw(spriteBatch, DpadDown);
            }

            if (IsPressed(OuyaController.BUTTON_DPAD_LEFT))
            {
                Draw(spriteBatch, DpadLeft);
            }

            if (IsPressed(OuyaController.BUTTON_DPAD_RIGHT))
            {
                Draw(spriteBatch, DpadRight);
            }

            if (IsPressed(OuyaController.BUTTON_DPAD_UP))
            {
                Draw(spriteBatch, DpadUp);
            }

            #endregion

            #region Buttons

            if (IsPressed(OuyaController.BUTTON_A))
            {
                Draw(spriteBatch, ButtonA);
            }

            if (IsPressed(OuyaController.BUTTON_O))
            {
                Draw(spriteBatch, ButtonO);
            }

            if (IsPressed(OuyaController.BUTTON_U))
            {
                Draw(spriteBatch, ButtonU);
            }

            if (IsPressed(OuyaController.BUTTON_Y))
            {
                Draw(spriteBatch, ButtonY);
            }

            if (IsPressed(OuyaController.BUTTON_L1))
            {
                Draw(spriteBatch, LeftBumper);
            }

            if (IsPressed(OuyaController.BUTTON_R1))
            {
                Draw(spriteBatch, RightBumper);
            }

            #endregion

            #region Triggers

            if (GetAxis(OuyaController.AXIS_L2) > DeadZone)
            {
                Draw(spriteBatch, LeftTrigger);
            }

            if (GetAxis(OuyaController.AXIS_R2) > DeadZone)
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

            Vector2 input = new Vector2(GetAxis(OuyaController.AXIS_LS_X), GetAxis(OuyaController.AXIS_LS_Y));

            if (IsPressed(OuyaController.BUTTON_L3) ||
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
            input = new Vector2(GetAxis(OuyaController.AXIS_RS_X), GetAxis(OuyaController.AXIS_RS_Y));

            if (IsPressed(OuyaController.BUTTON_R3) ||
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