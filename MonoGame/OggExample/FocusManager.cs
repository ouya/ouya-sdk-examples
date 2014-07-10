using System;
using System.Collections.Generic;

namespace OggExample
{
    public class FocusManager
    {
        private const int DELAY_MS = 150;

        public ButtonSprite SelectedButton = null;

        private DateTime m_timerSelection = DateTime.MinValue;

        private void SetSelection(ButtonSprite selection)
        {
            if (null != selection &&
                m_timerSelection < DateTime.Now)
            {
                m_timerSelection = DateTime.Now + TimeSpan.FromMilliseconds(DELAY_MS);
                SelectedButton = selection;
            }
        }

        public void InvokeClick()
        {
            if (null != OnClick &&
                null != SelectedButton &&
                m_timerSelection < DateTime.Now)
            {
                m_timerSelection = DateTime.Now + TimeSpan.FromMilliseconds(DELAY_MS);
                OnClick.Invoke(SelectedButton, new ClickEventArgs() { Button = SelectedButton });
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

        public void FocusDown()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Down);
            }
        }
        public void FocusLeft()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Left);
            }
        }
        public void FocusRight()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Right);
            }
        }
        public void FocusUp()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Up);
            }
        }

        public class ClickEventArgs : EventArgs
        {
            public ButtonSprite Button = null;
        }

        public EventHandler<ClickEventArgs> OnClick = null;
    }
}