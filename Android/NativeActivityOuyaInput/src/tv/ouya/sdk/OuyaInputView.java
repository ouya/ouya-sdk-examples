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

package tv.ouya.sdk;

import tv.ouya.console.api.DebugInput;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaInputMapper;
import android.app.Activity;
import android.content.Context;
import android.util.AttributeSet;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.MotionEvent.PointerCoords;
import android.view.MotionEvent.PointerProperties;
import android.view.View;
import android.widget.FrameLayout;

public class OuyaInputView extends View {

    private static final String TAG = OuyaInputView.class.getSimpleName();

    private static final boolean sEnableLogging = true;
    
    private static OuyaInputView sInstance = null;

	public static boolean sNativeInitialized = false;
	
    private static class ViewRemappedEventDispatcher implements OuyaInputMapper.RemappedEventDispatcher {
        public static OuyaInputView mView;
        public ViewRemappedEventDispatcher(OuyaInputView view) {
            mView = view;
        }
        @Override
        public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
            if (sEnableLogging) {
                Log.i(TAG, "ViewRemappedEventDispatcher: dispatchGenericMotionEvent");
            }
            return OuyaInputView.remappedDispatchGenericMotionEvent(motionEvent);
        }
        @Override
        public boolean dispatchKeyEvent(KeyEvent keyEvent) {
            if (sEnableLogging) {
                Log.i(TAG, "ViewRemappedEventDispatcher: dispatchKeyEvent");
            }
            return OuyaInputView.remappedDispatchKeyEvent(keyEvent);
        }
    }
    
    private static ViewRemappedEventDispatcher sViewRemappedEventDispatcher = null;

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

    public static boolean javaDispatchKeyEvent(long downTime, long eventTime, int action, int code,
                                        int repeat, int metaState, int deviceId, int scancode, int flags, int source) {
    	try {
    		if (sNativeInitialized) {
    			if (sEnableLogging) {
    				Log.i(TAG, "javaDispatchKeyEvent: Native plugin has initialized!");
    			}
    		} else {
    			Log.e(TAG, "javaDispatchKeyEvent: Native plugin has not been initialized!");
    			return false;
    		}
    		
    		if (sEnableLogging) {
    			Log.i(TAG, "javaDispatchKeyEvent downTime="+downTime+" eventTime="+eventTime+" action="+action+" code="+code+" repeat="+repeat+
    				" metaState="+metaState+" deviceId="+deviceId+" scancode="+scancode+" flags="+flags+" source="+source);
    		}
	    	
	        KeyEvent keyEvent = new KeyEvent(downTime, eventTime, action, code,
	        	repeat, metaState, deviceId, scancode, flags, source);
	
	        if (sEnableLogging) {
	            Log.i(TAG, "javaDispatchKeyEvent keyCode=" + keyEvent.getKeyCode()+" name="+DebugInput.debugGetKeyEvent(keyEvent));
	        }
	        
	        // support Xiaomi volume controls
	        InputDevice device = keyEvent.getDevice();
	        if (null == device) {
	        	Log.e(TAG, "javaDispatchKeyEvent: Device is null!");
	        } else {
	            String name = device.getName();
	            if (sEnableLogging) {
	                Log.i(TAG, "InputDevice name="+name+" keyCode="+code);
	            }
	            if (null != name &&
	                name.equals("aml_keypad")) {
	                int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
	                if (playerNum < 0) {
	                    playerNum = 0;
	                }
	                switch (code) {
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
	        
	        if (null == sInstance) {
	        	Log.e(TAG, "Instance was not found.");
	        	return false;
	        }
	
	        Activity activity = ((Activity)sInstance.getContext());       
	        if (null != activity) {
	            if (OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
	                return OuyaInputMapper.dispatchKeyEvent(keyEvent, sInstance, sViewRemappedEventDispatcher);
	            } else {
	            	Log.i(TAG, "javaDispatchKeyEvent: shouldHandleInputEvent returned false.");            	
	            }
	        } else {
	            Log.e(TAG, "Activity was not found.");
	        }
	        return false;
    	} catch (Exception e) {
    		e.printStackTrace();
    		return false;
    	}
    }

    public static boolean javaDispatchGenericMotionEvent(
            long downTime,
            long eventTime,
            int action,
            int pointerCount,
            int metaState,
            int buttonState,
            float xPrecision,
            float yPrecision,
            int deviceId,
            int edgeFlags,
            int source,
            int flags,
            int[] pointerPropertiesId,
            int[] pointerPropertiesToolType,
            float[] pointerCoordsOrientation,
            float[] pointerCoordsPressure,
            float[] pointerCoordsSize,
            float[] pointerCoordsToolMajor,
            float[] pointerCoordsToolMinor,
            float[] pointerCoordsTouchMajor,
            float[] pointerCoordsTouchMinor,
            float[] pointerCoordsX,
            float[] pointerCoordsY,
            int axisCount,
            int[] axisIndexes,
            float[] axisValues) {

        if (sEnableLogging) {
            Log.i(TAG, "javaDispatchGenericMotionEvent");
        }

        PointerProperties[] pointerProperties = new PointerProperties[pointerCount];
        PointerCoords[] pointerCoords = new PointerCoords[pointerCount];

        if (pointerCount > 0)
        {
            long pointerIndex = pointerPropertiesId[0];

            PointerProperties properties = new PointerProperties();
            properties.id = (int)pointerIndex;
            properties.toolType = pointerPropertiesToolType[0];
            pointerProperties[0] = properties;

            PointerCoords coords = new PointerCoords();
            coords.orientation = pointerCoordsOrientation[0];
            coords.pressure = pointerCoordsPressure[0];
            coords.size = pointerCoordsSize[0];
            coords.toolMajor = pointerCoordsToolMajor[0];
            coords.toolMinor = pointerCoordsToolMinor[0];
            coords.touchMajor = pointerCoordsTouchMajor[0];
            coords.touchMinor = pointerCoordsTouchMinor[0];
            coords.x = pointerCoordsX[0];
            coords.y = pointerCoordsY[0];
            for (int index = 0; index < axisCount; ++index)
            {
                int axis = axisIndexes[index];
                float value = axisValues[index];
                coords.setAxisValue(axis, value);
            }
            pointerCoords[0] = coords;
        }

        MotionEvent motionEvent = MotionEvent.obtain(downTime, eventTime, action,
                pointerCount, pointerProperties, pointerCoords,
                metaState, buttonState, xPrecision, yPrecision, deviceId, edgeFlags, source, flags);

        //DebugInput.debugMotionEvent(motionEvent);
        
        if (null == sInstance) {
        	Log.e(TAG, "Instance was not found.");
        	return false;
        }

        Activity activity = ((Activity)sInstance.getContext());       
        if (null != activity) {
            if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
                boolean handled = OuyaInputMapper.dispatchGenericMotionEvent(motionEvent, sInstance, sViewRemappedEventDispatcher);
                motionEvent.recycle();
                return handled;
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
        if (sEnableLogging) {
            Log.i(TAG, "remappedDispatchGenericMotionEvent");
            DebugInput.debugOuyaMotionEvent(motionEvent);
        }

        int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());
        if (playerNum < 0) {
            playerNum = 0;
        }
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
        dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
        return true;
    }

    public static boolean remappedDispatchKeyEvent(KeyEvent keyEvent) {
		if (!sNativeInitialized) {
			Log.e(TAG, "Native plugin has not been initialized!");
			return false;
		}
        if (sEnableLogging) {
            Log.i(TAG, "remappedDispatchKeyEvent keyCode=" + keyEvent.getKeyCode()+" name="+DebugInput.debugGetKeyEvent(keyEvent));
        }

        int keyCode = keyEvent.getKeyCode();
        int action = keyEvent.getAction();
        int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
        if (playerNum < 0) {
            playerNum = 0;
        }
        dispatchKeyEventNative(playerNum, keyCode, action);
        return true;
    }

    public static native void dispatchGenericMotionEventNative(int deviceId, int axis, float value);
    public static native void dispatchKeyEventNative(int deviceId, int keyCode, int action);
}