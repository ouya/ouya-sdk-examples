using System;
using System.Runtime.InteropServices;

namespace TV.Ouya.Sdk{
	public class OuyaInput
	{
		private class NativeWrapper
		{
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API float getAxis(int playerNum, int axis)
			public static extern float getAxis(int playerNum, int axis);
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API bool isPressed(int playerNum, int keyCode)
			public static extern bool isPressed(int playerNum, int keyCode);
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API bool isPressedDown(int playerNum, int keyCode)
			public static extern bool isPressedDown(int playerNum, int keyCode);
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API bool isPressedUp(int playerNum, int keyCode)
			public static extern bool isPressedUp(int playerNum, int keyCode);
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API void clearButtonStates()
			public static extern void clearButtonStates();
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API void clearAxes()
			public static extern void clearAxes();
			[DllImport("lib-ouya-ndk")]
			// EXPORT_API void clearButtons()
			public static extern void clearButtons();
		}

		public static float GetAxis(int playerNum, int axis)
		{
			return NativeWrapper.getAxis(playerNum, axis);
		}

		public static bool GetButton(int playerNum, int button)
		{
			return NativeWrapper.isPressed(playerNum, button);
		}

		public static bool GetButton(int button)
		{
			return (GetButton (0, button) ||
				GetButton (1, button) ||
				GetButton (2, button) ||
				GetButton (3, button));
		}

		public static bool GetButtonDown(int playerNum, int button)
		{
			return NativeWrapper.isPressedDown(playerNum, button);
		}

		public static bool GetButtonDown(int button)
		{
			return (GetButtonDown (0, button) ||
				GetButtonDown (1, button) ||
				GetButtonDown (2, button) ||
				GetButtonDown (3, button));
		}

		public static bool GetButtonUp(int playerNum, int button)
		{
			return NativeWrapper.isPressedUp(playerNum, button);
		}

		public static bool GetButtonUp(int button)
		{
			return (GetButtonUp (0, button) ||
				GetButtonUp (1, button) ||
				GetButtonUp (2, button) ||
				GetButtonUp (3, button));
		}

		public static void ClearAxes()
		{
			NativeWrapper.clearAxes();
		}

		public static void ClearButtons()
		{
			NativeWrapper.clearButtons();
		}

		public static void ClearButtonStates()
		{
			NativeWrapper.clearButtonStates();
		}
	}
}

