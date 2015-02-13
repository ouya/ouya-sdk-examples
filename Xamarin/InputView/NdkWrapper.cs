using System;
using System.Runtime.InteropServices;

namespace InputView
{
	public class NdkWrapper
	{
		[DllImport("lib-ouya-ndk")]
		// EXPORT_API float getAxis(int deviceId, int axis)
		public static extern float getAxis(int deviceId, int axis);
		[DllImport("lib-ouya-ndk")]
		// EXPORT_API bool isPressed(int deviceId, int keyCode)
		public static extern bool isPressed(int deviceId, int keyCode);
		[DllImport("lib-ouya-ndk")]
		// EXPORT_API bool isPressedDown(int deviceId, int keyCode)
		public static extern bool isPressedDown(int deviceId, int keyCode);
		[DllImport("lib-ouya-ndk")]
		// EXPORT_API bool isPressedUp(int deviceId, int keyCode)
		public static extern bool isPressedUp(int deviceId, int keyCode);
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
}

