// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using tv.ouya.console.api;

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Controller)]
	[Tooltip("Check if OuyaController button is down.")]
	public class OuyaControllerGetButtonDown : FsmStateAction
	{
        public enum OuyaPlayerNumber
        {
            CONTROLLER_0 = 0,
            CONTROLLER_1 = 1,
            CONTROLLER_2 = 2,
            CONTROLLER_3 = 3,
        }

		[RequiredField]
        [Tooltip("Player Number (Controllers are Zero-Based)")]
        public OuyaPlayerNumber playerNumber;

        public enum OuyaButton
        {
            BUTTON_O = OuyaController.BUTTON_O,
            BUTTON_U = OuyaController.BUTTON_U,
            BUTTON_Y = OuyaController.BUTTON_Y,
            BUTTON_A = OuyaController.BUTTON_A,
            BUTTON_DPAD_DOWN = OuyaController.BUTTON_DPAD_DOWN,
            BUTTON_DPAD_LEFT = OuyaController.BUTTON_DPAD_LEFT,
            BUTTON_DPAD_RIGHT = OuyaController.BUTTON_DPAD_RIGHT,
            BUTTON_DPAD_UP = OuyaController.BUTTON_DPAD_UP,
            BUTTON_L1 = OuyaController.BUTTON_L1,
            BUTTON_L3 = OuyaController.BUTTON_L3,
            BUTTON_R1 = OuyaController.BUTTON_R1,
            BUTTON_R3 = OuyaController.BUTTON_R3,
        }

        [RequiredField]
        [Tooltip("Button (KeyCode)")]
        public OuyaButton button;

		public override void Reset()
		{
            playerNumber = 0;
		    button = 0;
		}

		public override void OnEnter()
		{
		}

		public override void OnUpdate()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
            if (OuyaSDK.OuyaInput.GetButtonDown((int)playerNumber, (int)button))
            {
                Finish();
            }
#endif
        }
	}
}