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

namespace InAppPurchases
{
    internal class FocusManager
    {
        public ButtonSprite SelectedButton = null;

        private DateTime m_timerSelection = DateTime.MinValue;

        private void SetSelection(ButtonSprite selection)
        {
            if (null != selection &&
                m_timerSelection < DateTime.Now)
            {
                m_timerSelection = DateTime.Now + TimeSpan.FromMilliseconds(250);
                SelectedButton = selection;
            }
        }

        public class ButtonMapping
        {
            public ButtonSprite Up = null;
            public ButtonSprite Left = null;
            public ButtonSprite Right = null;
            public ButtonSprite Down = null;
        }

        public Dictionary<ButtonSprite, ButtonMapping> Mappings = new Dictionary<ButtonSprite, ButtonMapping>();

        private GamePadState GetState(PlayerIndex index)
        {
            return GamePad.GetState(index);
        }
        
        private bool GetDpadDown(PlayerIndex index)
        {
            return (GetState(index).DPad.Down == ButtonState.Pressed);
        }
        private bool GetDpadLeft(PlayerIndex index)
        {
            return (GetState(index).DPad.Left == ButtonState.Pressed);
        }
        private bool GetDpadRight(PlayerIndex index)
        {
            return (GetState(index).DPad.Right == ButtonState.Pressed);
        }
        private bool GetDpadUp(PlayerIndex index)
        {
            return (GetState(index).DPad.Up == ButtonState.Pressed);
        }
        private bool GetButtonO(PlayerIndex index)
        {
            return (GetState(index).Buttons.A == ButtonState.Pressed);
        }

        public class ClickEventArgs : EventArgs
        {
            public ButtonSprite Button = null;
        }

        public EventHandler<ClickEventArgs> OnClick = null;

        public void UpdateFocus()
        {
            if (null == SelectedButton)
            {
                return;
            }

            if (Mappings.ContainsKey(SelectedButton))
            {
                for (int index = 0; index < 4; ++index)
                {
                    #region DPADS

                    if (GetDpadDown((PlayerIndex) index))
                    {
                        SetSelection(Mappings[SelectedButton].Down);
                    }
                    else if (GetDpadLeft((PlayerIndex)index))
                    {
                        SetSelection(Mappings[SelectedButton].Left);
                    }
                    else if (GetDpadRight((PlayerIndex)index))
                    {
                        SetSelection(Mappings[SelectedButton].Right);
                    }
                    else if (GetDpadUp((PlayerIndex)index))
                    {
                        SetSelection(Mappings[SelectedButton].Up);
                    }
                    else if (GetButtonO((PlayerIndex)index))
                    {
                        if (null != OnClick)
                        {
                            OnClick.Invoke(SelectedButton, new ClickEventArgs() { Button = SelectedButton});
                        }
                    }

                    #endregion
                }
            }
        }
    }
}