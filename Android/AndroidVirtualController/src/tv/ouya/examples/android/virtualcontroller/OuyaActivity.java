package tv.ouya.examples.android.virtualcontroller;

import java.util.HashMap;

import android.os.Bundle;
import android.app.Activity;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import tv.ouya.console.api.OuyaController;

public class OuyaActivity extends Activity {
	
	private static final String TAG = OuyaActivity.class.getSimpleName();
	
	private HashMap<Integer, Boolean> mLastValue = new HashMap<Integer, Boolean>();

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
	
		takeKeyEvents(true);
	}

	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		
		float lsX = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
	    float lsY = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
	    float rsX = motionEvent.getAxisValue(OuyaController.AXIS_RS_X);
	    float rsY = motionEvent.getAxisValue(OuyaController.AXIS_RS_Y);
	    float l2 = motionEvent.getAxisValue(OuyaController.AXIS_L2);
	    float r2 = motionEvent.getAxisValue(OuyaController.AXIS_R2);
		
		if (motionEvent.getDevice().getName().equals("GameStick Controller")) {			
			lsX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
			lsY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
			//Log.i(TAG, "dispatchGenericMotionEvent x="+lsX);
			//Log.i(TAG, "dispatchGenericMotionEvent y="+lsY);			
			if (lsX == -1f) {
				if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) ||
					!mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {					
					mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, true);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_LEFT));
				}				
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
				}
			} else if (lsX == 1f) {
				if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) ||
					!mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {					
					mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, true);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_RIGHT));
				}				
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
				}
			} else if (lsX == 0f)  {
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
				}
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
				}				
			}
			if (lsY == -1f) {
				if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) ||
					!mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {					
					mLastValue.put(OuyaController.BUTTON_DPAD_UP, true);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_UP));
				}				
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
				}
			} else if (lsY == 1f) {
				if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) ||
					!mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {					
					mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, true);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_DOWN));
				}				
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
				}
			} else if (lsY == 0f)  {
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
				}
				if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
					mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
					mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
				}				
			}
			
		} else {
			lsX = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
		    lsY = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
		}
		
		/*
		Log.i(TAG, "dispatchGenericMotionEvent lsX="+lsX);
		Log.i(TAG, "dispatchGenericMotionEvent lsY="+lsY);
		Log.i(TAG, "dispatchGenericMotionEvent rsX="+rsX);
		Log.i(TAG, "dispatchGenericMotionEvent rsY="+rsY);
		Log.i(TAG, "dispatchGenericMotionEvent l2="+l2);
		Log.i(TAG, "dispatchGenericMotionEvent r2="+r2);
		*/
		
		// if remapping is needed
		//motionEvent.obtain(downTime, eventTime, action, x, y, pressure, size, metaState, xPrecision, yPrecision, deviceId, edgeFlags)
		
		return super.dispatchGenericMotionEvent(motionEvent);
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		
		//Log.i(TAG, "dispatchKeyEvent="+keyEvent.getKeyCode());
		
		int keyCode = keyEvent.getKeyCode();
        if (keyCode == OuyaController.BUTTON_L1 ||
            keyCode == OuyaController.BUTTON_R1||
            keyCode == OuyaController.BUTTON_O ||
            keyCode == OuyaController.BUTTON_U ||
            keyCode == OuyaController.BUTTON_Y ||
            keyCode == OuyaController.BUTTON_A ||
            keyCode == OuyaController.BUTTON_L3 ||
            keyCode == OuyaController.BUTTON_R3) {
            super.dispatchKeyEvent(keyEvent);
            return true;
        }

        int source = keyEvent.getSource();
        if (keyCode == OuyaController.BUTTON_DPAD_UP ||
        	keyCode == OuyaController.BUTTON_DPAD_DOWN ||
            keyCode == OuyaController.BUTTON_DPAD_LEFT ||
            keyCode == OuyaController.BUTTON_DPAD_RIGHT) {
            if (source != InputDevice.SOURCE_JOYSTICK) {
            	super.dispatchKeyEvent(keyEvent);
                return true;
            }
        }
		
        return true;
	}	
	
}
