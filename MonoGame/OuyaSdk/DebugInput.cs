using Android.Views;
using System;
using Android.Util;

namespace OuyaSdk
{
    public class DebugInput
    {
        private const String TAG = "DebugInput";

        public static String DebugGetAxisName(int axis)
        {
            return ((Axis) axis).ToString();
        }

        public static void DebugOuyaMotionEvent(MotionEvent motionEvent)
        {
            Log.Info(TAG, "OuyaController.AXIS_LS_X value=" + motionEvent.GetAxisValue((Axis)OuyaController.AXIS_LS_X));
            Log.Info(TAG, "OuyaController.AXIS_LS_Y value=" + motionEvent.GetAxisValue((Axis)OuyaController.AXIS_LS_Y));
            Log.Info(TAG, "OuyaController.AXIS_RS_X value=" + motionEvent.GetAxisValue((Axis)OuyaController.AXIS_RS_X));
            Log.Info(TAG, "OuyaController.AXIS_RS_Y value=" + motionEvent.GetAxisValue((Axis)OuyaController.AXIS_RS_Y));
            Log.Info(TAG, "OuyaController.AXIS_L2 value=" + motionEvent.GetAxisValue((Axis)OuyaController.AXIS_L2));
            Log.Info(TAG, "OuyaController.AXIS_R2 value=" + motionEvent.GetAxisValue((Axis)OuyaController.AXIS_R2));
        }

        public static void DebugMotionEvent(MotionEvent motionEvent)
        {
            foreach (Axis axis in Enum.GetValues(typeof (Axis)))
            {
                Log.Info(TAG, string.Format("({0}) {1} = {2}", (int)axis, axis, motionEvent.GetAxisValue(axis)));
            }
        }
    }
}