package tv.ouya.sdk;

import android.view.KeyEvent;
import android.view.MotionEvent;

public class RazerController {
	private static final String TAG = RazerController.class.getSimpleName();
	
	public static final int AXIS_DPAD_X = MotionEvent.AXIS_HAT_X;
	public static final int AXIS_DPAD_Y = MotionEvent.AXIS_HAT_Y;
	public static final int AXIS_LS_X = MotionEvent.AXIS_X;
	public static final int AXIS_LS_Y = MotionEvent.AXIS_Y;
	public static final int AXIS_RS_X = MotionEvent.AXIS_Z;
	public static final int AXIS_RS_Y = MotionEvent.AXIS_RZ;
	public static final int AXIS_L2 = MotionEvent.AXIS_LTRIGGER;
	public static final int AXIS_R2 = MotionEvent.AXIS_RTRIGGER;
	
	public static final int BUTTON_DPAD_DOWN = KeyEvent.KEYCODE_DPAD_DOWN;
	public static final int BUTTON_DPAD_LEFT = KeyEvent.KEYCODE_DPAD_LEFT;
	public static final int BUTTON_DPAD_RIGHT = KeyEvent.KEYCODE_DPAD_RIGHT;
	public static final int BUTTON_DPAD_UP = KeyEvent.KEYCODE_DPAD_UP;
	public static final int BUTTON_A = KeyEvent.KEYCODE_BUTTON_A;
	public static final int BUTTON_B = KeyEvent.KEYCODE_BUTTON_B;
	public static final int BUTTON_X = KeyEvent.KEYCODE_BUTTON_X;
	public static final int BUTTON_Y = KeyEvent.KEYCODE_BUTTON_Y;
	public static final int BUTTON_L1 = KeyEvent.KEYCODE_BUTTON_L1;
	public static final int BUTTON_R1 = KeyEvent.KEYCODE_BUTTON_R1;
	public static final int BUTTON_L2 = KeyEvent.KEYCODE_BUTTON_L2;
	public static final int BUTTON_R2 = KeyEvent.KEYCODE_BUTTON_R2;
	public static final int BUTTON_L3 = KeyEvent.KEYCODE_BUTTON_THUMBL;
	public static final int BUTTON_R3 = KeyEvent.KEYCODE_BUTTON_THUMBR;
	public static final int BUTTON_BACK = KeyEvent.KEYCODE_BACK;;
	public static final int BUTTON_HOME = KeyEvent.KEYCODE_HOME;
	public static final int BUTTON_NEXT = KeyEvent.KEYCODE_BUTTON_START;
	public static final int BUTTON_PREVIOUS = KeyEvent.KEYCODE_BUTTON_SELECT;
	public static final int BUTTON_POWER = KeyEvent.KEYCODE_BUTTON_MODE;
	
	public static final int MAX_CONTROLLERS = 4;
}
