/*
 * Copyright (C) 2012-2014 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package tv.ouya.sdk;

import tv.ouya.console.api.DebugInput;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaInputMapper;
import android.app.Activity;
import android.content.Context;
import android.util.AttributeSet;
import android.util.Log;
import android.util.SparseArray;
import android.view.InputDevice;
import android.view.InputEvent;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.FrameLayout;
import java.util.HashMap;

public class OuyaInputView extends View {

	private static final String TAG = OuyaInputView.class.getSimpleName();

	private static final boolean sEnableLogging = false;

	public static boolean sNativeInitialized = false;

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
    	Log.i(TAG, "Loading lib-ouya-ndk...");
        System.loadLibrary("-ouya-ndk");
    }

    public OuyaInputView(Context context, AttributeSet attrs) {
    	super(context, attrs);
        init();
    }

    public OuyaInputView(Context context, AttributeSet attrs, int defStyle) {
    	super(context, attrs, defStyle);
        init();
    }

    public OuyaInputView(Context context) {
        super(context);
        init();
    }

    private void init() {
    	if (sEnableLogging) {
    		Log.i(TAG, "Construct OuyaInputView");
    	}
		Activity activity = ((Activity)getContext());		
		if (null != activity) {			
			
			FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
			if (null != content) {
				content.addView(this);
			} else {
				Log.e(TAG, "Content view is missing");
			}
			
			OuyaInputMapper.init(activity);			
			activity.takeKeyEvents(true);
			setFocusable(true);
			requestFocus();
		} else {
			Log.e(TAG, "Activity is null");
		}
	}
	
	public void shutdown() {
		Activity activity = ((Activity)getContext());		
		if (null != activity) {
    		OuyaInputMapper.shutdown(activity);
    	} else {
    		Log.e(TAG, "Activity was not found.");
    	}
    }
	
	@Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	//Log.i(TAG, "dispatchGenericMotionEvent");
    	//DebugInput.debugMotionEvent(motionEvent);
    	Activity activity = ((Activity)getContext());		
		if (null != activity) {
		    if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
		    	return OuyaInputMapper.dispatchGenericMotionEvent(activity, motionEvent);
		    }
	    } else {
	    	Log.e(TAG, "Activity was not found.");
	    }
    	return super.dispatchGenericMotionEvent(motionEvent);
    }
	
	@Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		//Log.i(TAG, "dispatchKeyEvent");
		Activity activity = ((Activity)getContext());		
		if (null != activity) {
	    	if (OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
	    		return OuyaInputMapper.dispatchKeyEvent(activity, keyEvent);
	    	}
	    } else {
	    	Log.e(TAG, "Activity was not found.");
	    }
	    return super.dispatchKeyEvent(keyEvent);
    }

	public native void dispatchGenericMotionEventNative(int deviceId, int axis, float value);
    public native void dispatchKeyEventNative(int deviceId, int keyCode, int action);
    
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		if (sEnableLogging) {
			DebugInput.debugOuyaMotionEvent(motionEvent);
			//DebugInput.debugMotionEvent(motionEvent);
		}

		if (!sNativeInitialized) {
			Log.e(TAG, "Native Plugin has not yet initialized...");
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());	    
	    if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	    	playerNum = 0;
	    }

		float dpadX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
		float dpadY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
		sAxisValues.get(playerNum).put(MotionEvent.AXIS_HAT_X, dpadX);
		sAxisValues.get(playerNum).put(MotionEvent.AXIS_HAT_Y, dpadY);

		if (null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_LEFT) &&
			null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_RIGHT)) {
			if (dpadX < -DEAD_ZONE) {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_LEFT, KeyEvent.ACTION_DOWN);
			} else {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_LEFT, KeyEvent.ACTION_UP);
			}

			if (dpadX > DEAD_ZONE) {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_RIGHT, KeyEvent.ACTION_DOWN);
			} else {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_RIGHT, KeyEvent.ACTION_UP);
			}
		}

		if (null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_DOWN) &&
			null == sButtonValues.get(playerNum).get(OuyaController.BUTTON_DPAD_UP)) {
			if (dpadY > DEAD_ZONE) {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_DOWN, KeyEvent.ACTION_DOWN);
			} else {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_DOWN, KeyEvent.ACTION_UP);
			}
			if (dpadY < -DEAD_ZONE) {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_UP, KeyEvent.ACTION_DOWN);
			} else {
				dispatchKeyEventNative(playerNum, OuyaController.BUTTON_DPAD_UP, KeyEvent.ACTION_UP);
			}
		}

		dispatchGenericMotionEventNative(playerNum, MotionEvent.AXIS_HAT_X, dpadX);
		dispatchGenericMotionEventNative(playerNum, MotionEvent.AXIS_HAT_Y, dpadY);
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
		return false;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		if (sEnableLogging) {
			Log.i(TAG, "onKeyUp keyCode=" + DebugInput.debugGetButtonName(keyCode));
		}

		if (!sNativeInitialized) {
			Log.e(TAG, "Native Plugin has not yet initialized...");
			return false;
		}

		if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	    	playerNum = 0;
	    }

		int action = keyEvent.getAction();
		switch (keyCode) {
			case OuyaController.BUTTON_DPAD_DOWN:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) > DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_DPAD_LEFT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) < -DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_DPAD_RIGHT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) > DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_DPAD_UP:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) < -DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, true);
				}
				break;
			case OuyaController.BUTTON_L2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_L2, 0f);
				break;
			case OuyaController.BUTTON_R2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_R2, 0f);
				break;
		}
		dispatchKeyEventNative(playerNum, keyCode, action);
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		if (sEnableLogging) {
			Log.i(TAG, "onKeyDown keyCode=" + DebugInput.debugGetButtonName(keyCode));
		}

		if (!sNativeInitialized) {
			Log.e(TAG, "Native Plugin has not yet initialized...");
			return false;
		}

		if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	    	playerNum = 0;
	    }

		int action = keyEvent.getAction();
		switch (keyCode) {
			case OuyaController.BUTTON_DPAD_DOWN:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) > DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_DPAD_LEFT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) < -DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_DPAD_RIGHT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) > DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_DPAD_UP:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) < -DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, false);
				}
				break;
			case OuyaController.BUTTON_L2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_L2, 1f);
				break;
			case OuyaController.BUTTON_R2:
				sAxisValues.get(playerNum).put(OuyaController.AXIS_R2, 1f);
				break;
		}
		dispatchKeyEventNative(playerNum, keyCode, action);
		return true;
	}
}
