using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System;

namespace InputView
{
	public class VirtualControllerView
	{
		const float DEAD_ZONE = 0.25f;

		const float AXIS_SCALER = 4f;


		int _playerNum;
		View _controller = null;
		View _buttonA = null;
		View _buttonDpadDown = null;
		View _buttonDpadLeft = null;
		View _buttonDpadRight = null;
		View _buttonDpadUp = null;
		View _buttonL1 = null;
		View _buttonMenu = null;
		View _axisL2 = null;
		View _axisL3Active = null;
		View _axisL3Inactive = null;
		View _buttonO = null;
		View _buttonR1 = null;
		View _axisR2 = null;
		View _axisR3Active = null;
		View _axisR3Inactive = null;
		View _buttonU = null;
		View _buttonY = null;

		DateTime _menuDetected = DateTime.MinValue;

		public VirtualControllerView (
			int playerNum,
			View controller,
			View buttonA,
			View buttonDpadDown,
			View buttonDpadLeft,
			View buttonDpadRight,
			View buttonDpadUp,
			View buttonL1,
			View axisL2,
			View axisL3Inactive,
			View buttonMenu,
			View buttonO,
			View buttonR1,
			View axisR2,
			View axisR3Inactive,
			View axisL3Active,
			View axisR3Active,
			View buttonU,
			View buttonY)
		{
			_playerNum = playerNum;
			_controller = controller;
			_buttonA = buttonA;
			_buttonDpadDown = buttonDpadDown;
			_buttonDpadLeft = buttonDpadLeft;
			_buttonDpadRight = buttonDpadRight;
			_buttonDpadUp = buttonDpadUp;
			_buttonL1 = buttonL1;
			_buttonMenu = buttonMenu;
			_axisL2 = axisL2;
			_axisL3Active = axisL3Active;
			_axisL3Inactive = axisL3Inactive;
			_buttonO = buttonO;
			_buttonR1 = buttonR1;
			_axisR2 = axisR2;
			_axisR3Active = axisR3Active;
			_axisR3Inactive = axisR3Inactive;
			_buttonU = buttonU;
			_buttonY = buttonY;

			_controller.Visibility = ViewStates.Visible;
			_buttonA.Visibility = ViewStates.Invisible;
			_buttonDpadDown.Visibility = ViewStates.Invisible;
			_buttonDpadLeft.Visibility = ViewStates.Invisible;
			_buttonDpadRight.Visibility = ViewStates.Invisible;
			_buttonDpadUp.Visibility = ViewStates.Invisible;
			_buttonL1.Visibility = ViewStates.Invisible;
			_buttonMenu.Visibility = ViewStates.Invisible;
			_axisL2.Visibility = ViewStates.Invisible;
			_axisL3Active.Visibility = ViewStates.Invisible;
			_axisL3Inactive.Visibility = ViewStates.Visible;
			_buttonO.Visibility = ViewStates.Invisible;
			_buttonR1.Visibility = ViewStates.Invisible;
			_axisR2.Visibility = ViewStates.Invisible;
			_axisR3Active.Visibility = ViewStates.Invisible;
			_axisR3Inactive.Visibility = ViewStates.Visible;
			_buttonU.Visibility = ViewStates.Invisible;
			_buttonY.Visibility = ViewStates.Invisible;
		}

		private void SetVisibility(View view, bool visible)
		{
			if (visible) {
				view.Visibility = ViewStates.Visible;
			} else {
				view.Visibility = ViewStates.Invisible;
			}
		}

		public void Update()
		{
			SetVisibility (_buttonO, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_O));
			SetVisibility (_buttonU, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_U));
			SetVisibility (_buttonY, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_Y));
			SetVisibility (_buttonA, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_A));
			SetVisibility (_buttonDpadDown, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_DPAD_DOWN));
			SetVisibility (_buttonDpadLeft, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_DPAD_LEFT));
			SetVisibility (_buttonDpadRight, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_DPAD_RIGHT));
			SetVisibility (_buttonDpadUp, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_DPAD_UP));
			SetVisibility (_buttonL1, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_L1));
			SetVisibility (_buttonR1, NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_R1));

			if (NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_L3)) {
				SetVisibility (_axisL3Active, true);
				SetVisibility (_axisL3Inactive, false);
			} else {
				SetVisibility (_axisL3Active, false);
				SetVisibility (_axisL3Inactive, true);
			}

			if (NdkWrapper.isPressed (_playerNum, OuyaController.BUTTON_R3)) {
				SetVisibility (_axisR3Active, true);
				SetVisibility (_axisR3Inactive, false);
			} else {
				SetVisibility (_axisR3Active, false);
				SetVisibility (_axisR3Inactive, true);
			}

			if (NdkWrapper.isPressedUp (_playerNum, OuyaController.BUTTON_MENU)) {
				_menuDetected = DateTime.Now + TimeSpan.FromSeconds (1);
			}

			if (Math.Abs (NdkWrapper.getAxis (_playerNum, OuyaController.AXIS_L2)) > DEAD_ZONE) {
				SetVisibility (_axisL2, true);
			} else {
				SetVisibility (_axisL2, false);
			}

			if (Math.Abs (NdkWrapper.getAxis (_playerNum, OuyaController.AXIS_R2)) > DEAD_ZONE) {
				SetVisibility (_axisR2, true);
			} else {
				SetVisibility (_axisR2, false);
			}

			SetVisibility (_buttonMenu, DateTime.Now < _menuDetected);

			//rotate input by N degrees to match image
			float degrees = 135;
			float radians = degrees / 180f * 3.14f;
			float cos = (float)Math.Cos(radians);
			float sin = (float)Math.Sin(radians);

			float lx = NdkWrapper.getAxis (_playerNum, OuyaController.AXIS_LS_X);
			float ly = NdkWrapper.getAxis (_playerNum, OuyaController.AXIS_LS_Y);

			_axisL3Active.SetX (_controller.GetX() + AXIS_SCALER * (lx * cos - ly * sin));
			_axisL3Active.SetY (_controller.GetY() + AXIS_SCALER * (lx * sin + ly * cos));

			_axisL3Inactive.SetX (_controller.GetX() + AXIS_SCALER * (lx * cos - ly * sin));
			_axisL3Inactive.SetY (_controller.GetY() + AXIS_SCALER * (lx * sin + ly * cos));

			float rx = NdkWrapper.getAxis (_playerNum, OuyaController.AXIS_RS_X);
			float ry = NdkWrapper.getAxis (_playerNum, OuyaController.AXIS_RS_Y);

			_axisR3Active.SetX (_controller.GetX() + AXIS_SCALER * (rx * cos - ry * sin));
			_axisR3Active.SetY (_controller.GetY() + AXIS_SCALER * (rx * sin + ry * cos));

			_axisR3Inactive.SetX (_controller.GetX() + AXIS_SCALER * (rx * cos - ry * sin));
			_axisR3Inactive.SetY (_controller.GetY() + AXIS_SCALER * (rx * sin + ry * cos));

		}
	}
}

