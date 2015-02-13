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

		public void Update()
		{
		}
	}
}

