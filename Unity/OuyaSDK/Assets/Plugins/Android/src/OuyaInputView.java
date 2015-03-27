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
import android.view.InputEvent;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.FrameLayout;

public class OuyaInputView extends View {

	private static final String TAG = OuyaInputView.class.getSimpleName();

	private static final boolean sEnableLogging = false;

	public static boolean sNativeInitialized = false;

	static {
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

		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
		dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
		return true;
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

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	    	playerNum = 0;
	    }

		int action = keyEvent.getAction();
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

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0 || playerNum >= OuyaController.MAX_CONTROLLERS) {
	    	playerNum = 0;
	    }

		int action = keyEvent.getAction();
		dispatchKeyEventNative(playerNum, keyCode, action);
		return true;
	}
}
