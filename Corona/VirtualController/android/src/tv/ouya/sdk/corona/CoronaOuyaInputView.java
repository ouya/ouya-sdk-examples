package tv.ouya.sdk.corona;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.util.SparseArray;
import android.util.SparseBooleanArray;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import com.ansca.corona.CoronaEnvironment;
import com.ansca.corona.input.CoronaKeyEvent;
import java.util.ArrayList;
import java.util.List;
import java.util.HashMap;
import tv.ouya.console.api.*;
import tv.ouya.sdk.DebugInput;

public class CoronaOuyaInputView extends View {
	
	private static final String TAG = CoronaOuyaInputView.class.getSimpleName();

	private static final boolean sEnableLogging = false;

	private Activity mActivity = null;

	// State of axis and buttons to be used by engine	
	private static List<SparseArray<Float>> sStateAxis = new ArrayList<SparseArray<Float>>();
	private static List<SparseBooleanArray> sStateButton = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sStateButtonDown = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sStateButtonUp = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sLastStateButtonDown = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sLastStateButtonUp = new ArrayList<SparseBooleanArray>();

	private static SparseArray<HashMap<Integer, Float>> sAxisValues = new SparseArray<HashMap<Integer, Float>>();
	private static SparseArray<HashMap<Integer, Boolean>> sButtonValues = new SparseArray<HashMap<Integer, Boolean>>();

	private static final float DEAD_ZONE = 0.25f;

	static {
		for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index) {
			HashMap<Integer, Float> axisMap = new HashMap<Integer, Float>();
			axisMap.put(MotionEvent.AXIS_HAT_X, 0f);
			axisMap.put(MotionEvent.AXIS_HAT_Y, 0f);
			sAxisValues.put(index, axisMap);
			HashMap<Integer, Boolean> buttonMap = new HashMap<Integer, Boolean>();
			sButtonValues.put(index, buttonMap);
		}
    }

	public CoronaOuyaInputView(Context context) {
		super(context);
		//Log.i(TAG, "CoronaOuyaInputView(Context)");
		
		mActivity = CoronaEnvironment.getCoronaActivity();
		OuyaInputMapper.init(mActivity);

		// prepare OUYA-Everywhere Input states
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {
			sStateAxis.add(new SparseArray<Float>());
			sStateButton.add(new SparseBooleanArray());
			sStateButtonDown.add(new SparseBooleanArray());
			sStateButtonUp.add(new SparseBooleanArray());
			sLastStateButtonDown.add(new SparseBooleanArray());
			sLastStateButtonUp.add(new SparseBooleanArray());
		}
		
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
		/*
		if (sEnableLogging) {
			DebugInput.debugMotionEvent(motionEvent);
		}
		*/

		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());	    
	    if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	    	playerNum = 0;
	    }

		SparseArray<Float> stateAxises = sStateAxis.get(playerNum);
		if (null == stateAxises) {
			Log.e(TAG, "onGenericMotionEvent stateAxises is null");
			return false;
		}
		
		float dpadX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
		float dpadY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
		sAxisValues.get(playerNum).put(MotionEvent.AXIS_HAT_X, dpadX);
		sAxisValues.get(playerNum).put(MotionEvent.AXIS_HAT_Y, dpadY);
		
		if (null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_LEFT) &&
			null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_RIGHT)) {
			if (dpadX < -DEAD_ZONE) {
				processKeyDown(playerNum, OuyaController.BUTTON_DPAD_LEFT);
			} else {
				processKeyUp(playerNum, OuyaController.BUTTON_DPAD_LEFT);
			}
			if (dpadX > DEAD_ZONE) {
				processKeyDown(playerNum, OuyaController.BUTTON_DPAD_RIGHT);
			} else {
				processKeyUp(playerNum, OuyaController.BUTTON_DPAD_RIGHT);
			}
		}
		
		if (null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_DOWN) &&
			null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_UP)) {
			if (dpadY > DEAD_ZONE) {
				processKeyDown(playerNum, OuyaController.BUTTON_DPAD_DOWN);
			} else {
				processKeyUp(playerNum, OuyaController.BUTTON_DPAD_DOWN);
			}
			if (dpadY < -DEAD_ZONE) {
				processKeyDown(playerNum, OuyaController.BUTTON_DPAD_UP);
			} else {
				processKeyUp(playerNum, OuyaController.BUTTON_DPAD_UP);
			}
		}
		
		stateAxises.put(MotionEvent.AXIS_HAT_X, dpadX);
    	stateAxises.put(MotionEvent.AXIS_HAT_Y, dpadY);
    	stateAxises.put(OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
    	stateAxises.put(OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
    	stateAxises.put(OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
    	stateAxises.put(OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
    	stateAxises.put(OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
    	stateAxises.put(OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
	    
	    CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {
			input.onGenericMotionEvent(playerNum, MotionEvent.AXIS_HAT_X, dpadX);
			input.onGenericMotionEvent(playerNum, MotionEvent.AXIS_HAT_Y, dpadY);
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
	    }
		return false;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		
		if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK) {
			return false;
		}
		
		/*
		if (sEnableLogging) {
			Log.i(TAG, "onKeyUp keyCode=" + keyCode + " ("+DebugInput.debugGetButtonName(keyCode)+") source="+keyEvent.getSource());
		}
		*/
		
    	int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
    	if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
    		playerNum = 0;
    	}
		
		CallbacksOuyaInput input;
		switch (keyCode) {
			case OuyaController.BUTTON_DPAD_DOWN:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) > DEAD_ZONE) {
						processKeyUp(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_DPAD_LEFT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) < -DEAD_ZONE) {
						processKeyUp(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_DPAD_RIGHT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) > DEAD_ZONE) {
						processKeyUp(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_DPAD_UP:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) < -DEAD_ZONE) {
						processKeyUp(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_L2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_L2, 0f);
				input = IOuyaActivity.GetCallbacksOuyaInput();
				if (null != input) {
					input.onGenericMotionEvent(playerNum, OuyaController.AXIS_L2, 0f);
				}
				break;
			case OuyaController.BUTTON_R2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_R2, 0f);
				input = IOuyaActivity.GetCallbacksOuyaInput();
				if (null != input) {
					input.onGenericMotionEvent(playerNum, OuyaController.AXIS_R2, 0f);
				}
				break;
		}
		
		return processKeyUp(playerNum, keyCode);
	}

	private boolean processKeyUp(int playerNum, int keyCode) {
		
		SparseBooleanArray stateButton = sStateButton.get(playerNum);
		if (null == stateButton) {
			Log.e(TAG, "processKeyUp stateButton playerNum="+playerNum+" keyCode="+keyCode+" is null");
			return false;
		}
		if (stateButton.get(keyCode) != false) {
			stateButton.put(keyCode, false);
			/*
			if (sEnableLogging) {
				Log.i(TAG, "processKeyUp stateButton playerNum="+playerNum+" keyCode="+keyCode+" ("+DebugInput.debugGetButtonName(keyCode)+") is="+stateButton.get(keyCode));
			}
			*/
			
			SparseBooleanArray stateButtonUp = sStateButtonUp.get(playerNum);
			if (null == stateButtonUp) {
				Log.e(TAG, "processKeyUp stateButtonUp playerNum="+playerNum+" keyCode="+keyCode+" is null");
			} else {
				stateButtonUp.put(keyCode, true);
				///*
				if (sEnableLogging) {
					Log.i(TAG, "processKeyUp stateButtonUp playerNum="+playerNum+" keyCode="+keyCode+" ("+DebugInput.debugGetButtonName(keyCode)+") is="+stateButtonUp.get(keyCode));
				}
				//*/
				CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
			    if (null != input) {
			    	input.onKeyUp(playerNum, keyCode);
			    }
			}
		}
		
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
	
		if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK) {
			return false;
		}
		
		/*
		if (sEnableLogging) {
			Log.i(TAG, "onKeyDown keyCode=" + keyCode + " ("+DebugInput.debugGetButtonName(keyCode)+") source="+keyEvent.getSource());
		}
		*/
		
    	int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
    	if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
    		playerNum = 0;
    	}
		
		CallbacksOuyaInput input;
		switch (keyCode) {
			case OuyaController.BUTTON_DPAD_DOWN:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) > DEAD_ZONE) {
						processKeyDown(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_DPAD_LEFT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) < -DEAD_ZONE) {
						processKeyDown(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_DPAD_RIGHT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) > DEAD_ZONE) {
						processKeyDown(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_DPAD_UP:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) < -DEAD_ZONE) {
						processKeyDown(playerNum, keyCode);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_L2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_L2, 1f);
				input = IOuyaActivity.GetCallbacksOuyaInput();
				if (null != input) {
					input.onGenericMotionEvent(playerNum, OuyaController.AXIS_L2, 1f);
				}
				break;
			case OuyaController.BUTTON_R2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_R2, 1f);
				input = IOuyaActivity.GetCallbacksOuyaInput();
				if (null != input) {
					input.onGenericMotionEvent(playerNum, OuyaController.AXIS_R2, 1f);
				}
				break;
		}
		
		return processKeyDown(playerNum, keyCode);
	}

	private boolean processKeyDown(int playerNum, int keyCode) {

		SparseBooleanArray stateButton = sStateButton.get(playerNum);
		if (null == stateButton) {
			Log.e(TAG, "processKeyDown stateButton playerNum="+playerNum+" keyCode="+keyCode+" is null");
			return false;
		}
		if (stateButton.get(keyCode) != true) {
			stateButton.put(keyCode, true);
			/*
			if (sEnableLogging) {
				Log.i(TAG, "processKeyDown stateButton playerNum="+playerNum+" keyCode="+keyCode+" ("+DebugInput.debugGetButtonName(keyCode)+") is="+stateButton.get(keyCode));
			}
			*/
			
			SparseBooleanArray stateButtonDown = sStateButtonDown.get(playerNum);
			if (null == stateButtonDown) {
				Log.e(TAG, "processKeyDown stateButtonDown playerNum="+playerNum+" keyCode="+keyCode+" is null");
			} else {
				stateButtonDown.put(keyCode, true);
				///*
				if (sEnableLogging) {
					Log.i(TAG, "processKeyDown stateButtonDown playerNum="+playerNum+" keyCode="+keyCode+" ("+DebugInput.debugGetButtonName(keyCode)+") is="+stateButtonDown.get(keyCode));
				}
				//*/
				CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
			    if (null != input) {
			    	input.onKeyDown(playerNum, keyCode);
			    }
			}
		}

		return true;
	}
}
