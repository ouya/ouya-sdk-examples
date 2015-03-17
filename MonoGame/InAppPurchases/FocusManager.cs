using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV.Ouya.Console.Api;
using TV.Ouya.Sdk;

namespace InAppPurchases
{
    internal class FocusManager
    {
		private const String TAG = "FocusManager";

        public ButtonSprite SelectedButton = null;

        private void SetSelection(ButtonSprite selection)
        {
			SelectedButton = selection;
        }

        public class ButtonMapping
        {
            public ButtonSprite Left = null;
            public ButtonSprite Right = null;
        }

        public Dictionary<ButtonSprite, ButtonMapping> Mappings = new Dictionary<ButtonSprite, ButtonMapping>();

        public void UpdateFocus()
        {
            if (null == SelectedButton)
            {
				Log.Error (TAG, "SelectedButton is null!");
                return;
            }

            if (Mappings.ContainsKey(SelectedButton))
            {
                #region DPADS

				if (OuyaInput.GetButtonDown(OuyaController.BUTTON_DPAD_LEFT))
                {
					if (null != Mappings[SelectedButton].Left) {
                    	SetSelection(Mappings[SelectedButton].Left);
					}
                }
				else if (OuyaInput.GetButtonDown(OuyaController.BUTTON_DPAD_RIGHT))
                {
					if (null != Mappings[SelectedButton].Right) {
                    	SetSelection(Mappings[SelectedButton].Right);
					}
                }

                #endregion
            }
        }
    }
}