using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace InputView
{
    public class VirtualControllerSprite
    {
        /// <summary>
        /// The controller index
        /// </summary>
        public int Index = 0;

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

        private DateTime m_timerMenuDetected = DateTime.MinValue;

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

		private Dictionary<int, float> m_axisStates = new Dictionary<int, float>();
		private Dictionary<int, bool> m_buttonStates = new Dictionary<int, bool>();
		private Dictionary<int, bool> m_buttonDownStates = new Dictionary<int, bool>();
		private Dictionary<int, bool> m_buttonUpStates = new Dictionary<int, bool>();

		public void UpdateInput()
		{
			#region Track Axis States

			lock (m_axisStates)
			{
				m_axisStates[OuyaController.AXIS_LS_X] = OuyaInput.getAxis(Index, OuyaController.AXIS_LS_X);
				m_axisStates[OuyaController.AXIS_LS_Y] = OuyaInput.getAxis(Index, OuyaController.AXIS_LS_Y);
				m_axisStates[OuyaController.AXIS_RS_X] = OuyaInput.getAxis(Index, OuyaController.AXIS_RS_X);
				m_axisStates[OuyaController.AXIS_RS_Y] = OuyaInput.getAxis(Index, OuyaController.AXIS_RS_Y);
				m_axisStates[OuyaController.AXIS_L2] = OuyaInput.getAxis(Index, OuyaController.AXIS_L2);
				m_axisStates[OuyaController.AXIS_R2] = OuyaInput.getAxis(Index, OuyaController.AXIS_R2);
			}

			#endregion

			#region Track Button Up / Down States

			lock (m_buttonStates)
			{
				m_buttonStates[OuyaController.BUTTON_O] = OuyaInput.isPressed(Index, OuyaController.BUTTON_O);
				m_buttonStates[OuyaController.BUTTON_U] = OuyaInput.isPressed(Index, OuyaController.BUTTON_U);
				m_buttonStates[OuyaController.BUTTON_Y] = OuyaInput.isPressed(Index, OuyaController.BUTTON_Y);
				m_buttonStates[OuyaController.BUTTON_A] = OuyaInput.isPressed(Index, OuyaController.BUTTON_A);
				m_buttonStates[OuyaController.BUTTON_L1] = OuyaInput.isPressed(Index, OuyaController.BUTTON_L1);
				m_buttonStates[OuyaController.BUTTON_R1] = OuyaInput.isPressed(Index, OuyaController.BUTTON_R1);
				m_buttonStates[OuyaController.BUTTON_L3] = OuyaInput.isPressed(Index, OuyaController.BUTTON_L3);
				m_buttonStates[OuyaController.BUTTON_R3] = OuyaInput.isPressed(Index, OuyaController.BUTTON_R3);
				m_buttonStates[OuyaController.BUTTON_DPAD_UP] = OuyaInput.isPressed(Index, OuyaController.BUTTON_DPAD_UP);
				m_buttonStates[OuyaController.BUTTON_DPAD_DOWN] = OuyaInput.isPressed(Index, OuyaController.BUTTON_DPAD_DOWN);
				m_buttonStates[OuyaController.BUTTON_DPAD_RIGHT] = OuyaInput.isPressed(Index, OuyaController.BUTTON_DPAD_RIGHT);
				m_buttonStates[OuyaController.BUTTON_DPAD_LEFT] = OuyaInput.isPressed(Index, OuyaController.BUTTON_DPAD_LEFT);
				m_buttonStates[OuyaController.BUTTON_MENU] = OuyaInput.isPressed(Index, OuyaController.BUTTON_MENU);
			}

			lock (m_buttonDownStates)
			{
				m_buttonDownStates[OuyaController.BUTTON_O] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_O);
				m_buttonDownStates[OuyaController.BUTTON_U] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_U);
				m_buttonDownStates[OuyaController.BUTTON_Y] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_Y);
				m_buttonDownStates[OuyaController.BUTTON_A] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_A);
				m_buttonDownStates[OuyaController.BUTTON_L1] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_L1);
				m_buttonDownStates[OuyaController.BUTTON_R1] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_R1);
				m_buttonDownStates[OuyaController.BUTTON_L3] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_L3);
				m_buttonDownStates[OuyaController.BUTTON_R3] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_R3);
				m_buttonDownStates[OuyaController.BUTTON_DPAD_UP] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_DPAD_UP);
				m_buttonDownStates[OuyaController.BUTTON_DPAD_DOWN] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_DPAD_DOWN);
				m_buttonDownStates[OuyaController.BUTTON_DPAD_RIGHT] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_DPAD_RIGHT);
				m_buttonDownStates[OuyaController.BUTTON_DPAD_LEFT] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_DPAD_LEFT);
				m_buttonDownStates[OuyaController.BUTTON_MENU] = OuyaInput.isPressedDown(Index, OuyaController.BUTTON_MENU);
			}

			lock (m_buttonUpStates)
			{
				m_buttonUpStates[OuyaController.BUTTON_O] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_O);
				m_buttonUpStates[OuyaController.BUTTON_U] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_U);
				m_buttonUpStates[OuyaController.BUTTON_Y] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_Y);
				m_buttonUpStates[OuyaController.BUTTON_A] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_A);
				m_buttonUpStates[OuyaController.BUTTON_L1] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_L1);
				m_buttonUpStates[OuyaController.BUTTON_R1] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_R1);
				m_buttonUpStates[OuyaController.BUTTON_L3] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_L3);
				m_buttonUpStates[OuyaController.BUTTON_R3] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_R3);
				m_buttonUpStates[OuyaController.BUTTON_DPAD_UP] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_DPAD_UP);
				m_buttonUpStates[OuyaController.BUTTON_DPAD_DOWN] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_DPAD_DOWN);
				m_buttonUpStates[OuyaController.BUTTON_DPAD_RIGHT] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_DPAD_RIGHT);
				m_buttonUpStates[OuyaController.BUTTON_DPAD_LEFT] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_DPAD_LEFT);
				m_buttonUpStates[OuyaController.BUTTON_MENU] = OuyaInput.isPressedUp(Index, OuyaController.BUTTON_MENU);
			}

			#endregion

			//debugOuyaController(deviceId);
		}

		private float GetAxis(int axis)
		{
			lock (m_axisStates) {
				if (m_axisStates.ContainsKey (axis)) {
					return m_axisStates [axis];
				} else {
					return 0f;
				}
			}
		}

		private bool GetButton(int button)
		{
			lock (m_buttonStates) {
				if (m_buttonStates.ContainsKey (button)) {
					return m_buttonStates [button];
				} else {
					return false;
				}
			}
		}

		private bool GetButtonDown(int button)
		{
			lock (m_buttonDownStates) {
				if (m_buttonDownStates.ContainsKey (button)) {
					return m_buttonDownStates [button];
				} else {
					return false;
				}
			}
		}

		private bool GetButtonUp(int button)
		{
			lock (m_buttonUpStates) {
				if (m_buttonUpStates.ContainsKey (button)) {
					return m_buttonUpStates [button];
				} else {
					return false;
				}
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

			if (GetButtonDown(OuyaController.BUTTON_MENU))
            {
                m_timerMenuDetected = DateTime.Now + TimeSpan.FromSeconds(1);
                Draw(spriteBatch, ButtonMenu);
            }
            else if (m_timerMenuDetected > DateTime.Now)
            {
                Draw(spriteBatch, ButtonMenu);
            }

            #endregion

            #region DPADS

			if (GetButton(OuyaController.BUTTON_DPAD_DOWN))
            {
                Draw(spriteBatch, DpadDown);
            }

			if (GetButton(OuyaController.BUTTON_DPAD_LEFT))
            {
                Draw(spriteBatch, DpadLeft);
            }

			if (GetButton(OuyaController.BUTTON_DPAD_RIGHT))
            {
                Draw(spriteBatch, DpadRight);
            }

			if (GetButton(OuyaController.BUTTON_DPAD_UP))
            {
                Draw(spriteBatch, DpadUp);
            }

            #endregion

            #region Buttons

			if (GetButton(OuyaController.BUTTON_A))
            {
                Draw(spriteBatch, ButtonA);
            }

			if (GetButton(OuyaController.BUTTON_O))
            {
                Draw(spriteBatch, ButtonO);
            }

			if (GetButton(OuyaController.BUTTON_U))
            {
                Draw(spriteBatch, ButtonU);
            }

			if (GetButton(OuyaController.BUTTON_Y))
            {
                Draw(spriteBatch, ButtonY);
            }

			if (GetButton(OuyaController.BUTTON_L1))
            {
                Draw(spriteBatch, LeftBumper);
            }

			if (GetButton(OuyaController.BUTTON_R1))
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

			if (GetButton(OuyaController.BUTTON_L3))
            {
                Draw(spriteBatch, LeftStickActive, Position + AXIS_SCALER * new Vector2(input.X * cos - input.Y * sin, input.X * sin + input.Y * cos));
            }
            else
            {
                Draw(spriteBatch, LeftStickInactive, Position + AXIS_SCALER * new Vector2(input.X * cos - input.Y * sin, input.X * sin + input.Y * cos));
            }

            //rotate by same degrees
			input = new Vector2(GetAxis(OuyaController.AXIS_RS_X), GetAxis(OuyaController.AXIS_RS_Y));

			if (GetButton(OuyaController.BUTTON_R3))
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