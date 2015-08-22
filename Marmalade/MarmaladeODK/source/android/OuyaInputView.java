/*
 * Copyright (C) 2012-2015 OUYA, Inc.
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

package tv.ouya.sdk.marmalade;

import android.app.Activity;
import android.content.Context;
import android.util.AttributeSet;
import android.util.Log;
import android.util.SparseArray;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.MotionEvent.PointerCoords;
import android.view.MotionEvent.PointerProperties;
import android.view.View;
import android.widget.FrameLayout;
import java.util.HashMap;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaInputMapper;
import tv.ouya.sdk.marmalade.*;

public class OuyaInputView extends View {

    private static final String TAG = OuyaInputView.class.getSimpleName();

    private static final boolean sEnableLogging = false;
    
    private static OuyaInputView sInstance = null;

	public static boolean sNativeInitialized = false;
	
	// Interim axis and button values before applied to states
	private static SparseArray<HashMap<Integer, Float>> sAxisValues = new SparseArray<HashMap<Integer, Float>>();
	private static SparseArray<HashMap<Integer, Boolean>> sButtonValues = new SparseArray<HashMap<Integer, Boolean>>();
	
	private static final float DEAD_ZONE = 0.25f;
	
    private static class ViewRemappedEventDispatcher implements OuyaInputMapper.RemappedEventDispatcher {
        public static OuyaInputView mView;
        public ViewRemappedEventDispatcher(OuyaInputView view) {
            mView = view;
        }
        @Override
        public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
        	/*
            if (sEnableLogging) {
                Log.i(TAG, "ViewRemappedEventDispatcher: dispatchGenericMotionEvent");
            }
            */
            return OuyaInputView.remappedDispatchGenericMotionEvent(motionEvent);
        }
        @Override
        public boolean dispatchKeyEvent(KeyEvent keyEvent) {
        	/*
            if (sEnableLogging) {
                Log.i(TAG, "ViewRemappedEventDispatcher: dispatchKeyEvent");
            }
            */
            return OuyaInputView.remappedDispatchKeyEvent(keyEvent);
        }
    }
    
    private static ViewRemappedEventDispatcher sViewRemappedEventDispatcher = null;
    
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
        
        sInstance = this;

        // disable screensaver
        setKeepScreenOn(true);

        // get remapped events
        sViewRemappedEventDispatcher = new ViewRemappedEventDispatcher(this);

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
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		if (sEnableLogging) {
			OuyaController ouyaController = OuyaController.getControllerByDeviceId(keyEvent.getDeviceId());
			if (null == ouyaController) {
				Log.i(TAG, "dispatchKeyEvent keyCode=" + keyEvent.getKeyCode()+" name="+DebugInput.debugGetKeyEvent(keyEvent));
			} else {
				Log.i(TAG, "dispatchKeyEvent name=\""+ouyaController.getDeviceName()+"\" keyCode=" + keyEvent.getKeyCode()+" name="+DebugInput.debugGetKeyEvent(keyEvent));
			}
		}
	    // support Xiaomi volume controls
	    InputDevice device = keyEvent.getDevice();
	    if (null == device) {
	        Log.e(TAG, "dispatchKeyEvent: Device is null!");
	    } else {
	        String name = device.getName();
	        /*
	        if (sEnableLogging) {
	            Log.i(TAG, "dispatchKeyEvent device name="+name+" keyCode="+code+" ("+DebugInput.debugGetButtonName(code)+")");
	        }
	        */
	        if (null != name &&
	            name.equals("aml_keypad")) {
	            int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
	            if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	                playerNum = 0;
	            }
				int action = keyEvent.getAction();
				int keyCode = keyEvent.getKeyCode();
	            switch (keyCode) {
	            case 24:
	                if (sEnableLogging) {
	                    Log.i(TAG, "Volume Up detected.");
	                }
	                return false; // let system handle event
	            case 25:
	                if (sEnableLogging) {
	                    Log.i(TAG, "Volume Down detected.");
	                }
	                return false; // let system handle event
	            case 66:
	                if (sEnableLogging) {
	                    Log.i(TAG, "Remote button detected.");
	                }
	                dispatchKeyEventNative(playerNum, OuyaController.BUTTON_O, action);
	                return true;
	            case 4:
	                if (sEnableLogging) {
	                    Log.i(TAG, "Remote back button detected.");
	                }
	                dispatchKeyEventNative(playerNum, OuyaController.BUTTON_A, action);
	                return true;
	            }
	        }
	    }
		Activity activity = ((Activity)getContext());		
		if (null != activity) {
	    	if (OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
	    		return OuyaInputMapper.dispatchKeyEvent(keyEvent, this, sViewRemappedEventDispatcher);
	    	}
	    } else {
	    	Log.e(TAG, "Activity was not found.");
	    }
		return false;
	}
		
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		/*
    	if (sEnableLogging) {
			DebugInput.debugMotionEvent(motionEvent);
		}
		*/
    	
    	Activity activity = ((Activity)getContext());		
		if (null != activity) {
		    if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
		    	return OuyaInputMapper.dispatchGenericMotionEvent(motionEvent, this, sViewRemappedEventDispatcher);
		    }
	    } else {
	    	Log.e(TAG, "Activity was not found.");
	    }
		return false;
    }

    public static boolean remappedDispatchGenericMotionEvent(MotionEvent motionEvent) {
		if (!sNativeInitialized) {
			Log.e(TAG, "Native plugin has not been initialized!");
			return false;
		}
		/*
        if (sEnableLogging) {
            Log.i(TAG, "remappedDispatchGenericMotionEvent");
            DebugInput.debugOuyaMotionEvent(motionEvent);
        }
        */

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
        
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
        
        return false;
    }

    public static boolean remappedDispatchKeyEvent(KeyEvent keyEvent) {
		if (!sNativeInitialized) {
			Log.e(TAG, "Native plugin has not been initialized!");
			return false;
		}
        if (sEnableLogging) {
            Log.i(TAG, "remappedDispatchKeyEvent keyCode=" + keyEvent.getKeyCode()+" ("+DebugInput.debugGetButtonName(keyEvent.getKeyCode())+") name="+DebugInput.debugGetKeyEvent(keyEvent));
        }

        int keyCode = keyEvent.getKeyCode();
        int action = keyEvent.getAction();
        int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
        if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
            playerNum = 0;
        }
		switch (keyCode) {
			case OuyaController.BUTTON_DPAD_DOWN:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) > DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, action == KeyEvent.ACTION_DOWN);
				}
				break;
			case OuyaController.BUTTON_DPAD_LEFT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) < -DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, action == KeyEvent.ACTION_DOWN);
				}
				break;
			case OuyaController.BUTTON_DPAD_RIGHT:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X) > DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, action == KeyEvent.ACTION_DOWN);
				}
				break;
			case OuyaController.BUTTON_DPAD_UP:
				if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
					if (sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y) < -DEAD_ZONE) {
						dispatchKeyEventNative(playerNum, keyCode, action);
					}
					return true;
				} else {
					sButtonValues.get(playerNum).put(keyCode, action == KeyEvent.ACTION_DOWN);
				}
				break;
			case OuyaController.BUTTON_L2:
				if (action == KeyEvent.ACTION_DOWN) {
					sAxisValues.get(playerNum).put(OuyaController.AXIS_L2, 1f);
				} else {
					sAxisValues.get(playerNum).put(OuyaController.AXIS_L2, 0f);
				}
				break;
			case OuyaController.BUTTON_R2:
				if (action == KeyEvent.ACTION_DOWN) {
					sAxisValues.get(playerNum).put(OuyaController.AXIS_R2, 1f);
				} else {
					sAxisValues.get(playerNum).put(OuyaController.AXIS_R2, 0f);
				}
				break;
		}
        dispatchKeyEventNative(playerNum, keyCode, action);
        return true;
    }

    public static native void dispatchGenericMotionEventNative(int deviceId, int axis, float value);
    public static native void dispatchKeyEventNative(int deviceId, int keyCode, int action);
}