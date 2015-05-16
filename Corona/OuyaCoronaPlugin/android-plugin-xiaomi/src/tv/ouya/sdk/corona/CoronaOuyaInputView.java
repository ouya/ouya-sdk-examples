package tv.ouya.sdk.corona;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;

import com.ansca.corona.CoronaEnvironment;
import com.ansca.corona.input.CoronaKeyEvent;

import tv.ouya.console.api.*;

public class CoronaOuyaInputView extends View {
	
	private static final String TAG = CoronaOuyaInputView.class.getSimpleName();
	private Activity mActivity = null;

	public CoronaOuyaInputView(Context context) {
		super(context);
		//Log.i(TAG, "CoronaOuyaInputView(Context)");
		
		mActivity = CoronaEnvironment.getCoronaActivity();
		OuyaInputMapper.init(mActivity);
		
		this.setFocusable(true);
	}
	
	/**
	 * Dispatch KeyEvents to the OuyaInputMapper. The remapped input will
	 * return to the activity with onGenericMotionEvent, onKeyDown, and onKeyUp
	 * events.
	 *
	 * Be sure to use the remapped input from the above events instead of
	 * the KeyEvent passed to this method that has not been remapped yet.
	 *
	 * @param keyEvent passed passed to the OuyaInputMapper
	 */
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		InputDevice device = keyEvent.getDevice();
		if (null != device) {
			String name = device.getName();
			if (null != name &&
				name.equals("aml_keypad")) {
				switch (keyEvent.getKeyCode()) {
				case 24:
					//VOLUME UP
					return false; //let the system handle the event
				case 25:
					//VOLUME DOWN
					return false; //let the system handle the event
				case 66:
					//REMOTE BUTTON
					if (keyEvent.getAction() == KeyEvent.ACTION_DOWN) {
						onKeyDown(OuyaController.BUTTON_O, keyEvent);
					}
					else if (keyEvent.getAction() == KeyEvent.ACTION_UP) {
						onKeyUp(OuyaController.BUTTON_O, keyEvent);
					}
					return false; //let the system handle the event
				case 4:
					//REMOTE BACK BUTTON
					if (keyEvent.getAction() == KeyEvent.ACTION_DOWN) {
						onKeyDown(OuyaController.BUTTON_A, keyEvent);
					}
					else if (keyEvent.getAction() == KeyEvent.ACTION_UP) {
						onKeyUp(OuyaController.BUTTON_A, keyEvent);
					}
					return false; //let the system handle the event
				}
			}
		}		
		if (keyEvent instanceof CoronaKeyEvent) {
			//eat the extra event
			return true;
		}
        if (OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
		    return OuyaInputMapper.dispatchKeyEvent(mActivity, keyEvent);
        } else {
            return super.dispatchKeyEvent(keyEvent);
        }
	}

	/**
	 * Dispatch MotionEvents to the OuyaInputMapper. The remapped input will
	 * return to the activity with onGenericMotionEvent, onKeyDown, and onKeyUp
	 * events.
	 *
	 * Be sure to use the remapped input from the above events instead of
	 * the MotionEvent passed to this method that has not been remapped yet.
	 *
	 * @param motionEvent passed passed to the OuyaInputMapper
	 */
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
        if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
		    return OuyaInputMapper.dispatchGenericMotionEvent(mActivity, motionEvent);
        } else {
            return super.dispatchGenericMotionEvent(motionEvent);
        }
    }

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//DebugInput.debugMotionEvent(motionEvent);

		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	//Log.e(TAG, "Failed to find playerId for Controller="+motionEvent.getDevice().getName());
	    	//return true;
	    	playerNum = 0; //treat the controller as the first controller
	    }
	    
	    CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {
	    	
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
	    }
	    
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyUp keyCode=" + DebugInput.debugGetButtonName(keyCode));

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	//Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	//return true;
	    	
	    	//hack for non-supported controllers
	    	playerNum = 0; //treat the controller as the first controller
	    	switch (keyCode) {
	    	case KeyEvent.KEYCODE_BUTTON_A:
	    	case KeyEvent.KEYCODE_ENTER:
	    		keyCode = OuyaController.BUTTON_O;
	    		break;
	    	case KeyEvent.KEYCODE_BUTTON_X:
	    		keyCode = OuyaController.BUTTON_U;
	    		break;
	    	case KeyEvent.KEYCODE_BUTTON_Y:
	    		keyCode = OuyaController.BUTTON_Y;
	    		break;
	    	case KeyEvent.KEYCODE_BUTTON_B:
	    		keyCode = OuyaController.BUTTON_A;
	    		break;
	    	default:
	    		if (keyEvent.getScanCode() == 102) {
	    			keyCode = OuyaController.BUTTON_MENU;
	    		}
	    		break;
	    	}
	    }

		int action = keyEvent.getAction();
		
		CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {	
	    	input.onKeyUp(playerNum, keyCode);
	    }
		
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyDown keyCode=" + DebugInput.debugGetButtonName(keyCode));

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	//Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	//return true;
	    	
	    	//hack for non-supported controllers
	    	playerNum = 0; //treat the controller as the first controller
	    	switch (keyCode) {
	    	case KeyEvent.KEYCODE_BUTTON_A:
	    	case KeyEvent.KEYCODE_ENTER:
	    		keyCode = OuyaController.BUTTON_O;
	    		break;
	    	case KeyEvent.KEYCODE_BUTTON_X:
	    		keyCode = OuyaController.BUTTON_U;
	    		break;
	    	case KeyEvent.KEYCODE_BUTTON_Y:
	    		keyCode = OuyaController.BUTTON_Y;
	    		break;
	    	case KeyEvent.KEYCODE_BUTTON_B:
	    		keyCode = OuyaController.BUTTON_A;
	    		break;
	    	default:
	    		if (keyEvent.getScanCode() == 102) {
	    			keyCode = OuyaController.BUTTON_MENU;
	    		}
	    		break;
	    	}
	    }

		int action = keyEvent.getAction();
		
		CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {	
	    	input.onKeyDown(playerNum, keyCode);
	    }
		
		return true;
	}
}
