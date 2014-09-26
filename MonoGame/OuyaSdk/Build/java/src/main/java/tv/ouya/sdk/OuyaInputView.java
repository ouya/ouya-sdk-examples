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

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.FrameLayout;
import tv.ouya.console.api.*;

public class OuyaInputView extends View {

	private static final String TAG = OuyaInputView.class.getSimpleName();

	private Activity mActivity = null;

	static {
    	Log.i(TAG, "Loading lib-ouya-ndk...");
		try {
			System.loadLibrary("-ouya-ndk");
		} catch (Exception ex) {
			ex.printStackTrace();
		}
    }

	public OuyaInputView(Activity activity, Context context) {
		super(context);
		Log.i(TAG, "OuyaInputView");
		mActivity = activity;
		if (null != mActivity) {
			OuyaInputMapper.init(mActivity);

			FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
			content.addView(this);
			Log.i(TAG, "Added OuyaInputView to content");

			setFocusable(true);
			requestFocus();
			Log.i(TAG, "Give the OuyaInputView focus");
		} else {
			Log.e(TAG, "Activity is null");
		}
	}

	public void shutdown() {
		if (null != mActivity) {
			OuyaInputMapper.shutdown(mActivity);
		} else {
			Log.e(TAG, "Activity is null");
		}
	}

	public native void dispatchGenericMotionEventNative(int deviceId, int axis, float value);
    public native void dispatchKeyEventNative(int deviceId, int keyCode, int action);

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
		//Log.i(TAG, "dispatchKeyEvent keyCode="+keyEvent.getKeyCode());
		if (null == mActivity) {
			return false;
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
		//Log.i(TAG, "dispatchGenericMotionEvent");
		if (null == mActivity) {
			return false;
		}
        if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
		    return OuyaInputMapper.dispatchGenericMotionEvent(mActivity, motionEvent);
        } else {
            return super.dispatchGenericMotionEvent(motionEvent);
        }
    }

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//Log.i(TAG, "onGenericMotionEvent");
		//DebugInput.debugMotionEvent(motionEvent);

		if (null == mActivity) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());
	    if (playerNum < 0) {
	    	Log.e(TAG, "Failed to find playerId for Controller="+motionEvent.getDevice().getName());
	    	return true;
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
		//Log.i(TAG, "onKeyUp keyCode=" + DebugInput.debugGetButtonName(keyCode));

		if (null == mActivity) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
	    if (playerNum < 0) {
	    	Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	return true;
	    }

		int action = keyEvent.getAction();
		dispatchKeyEventNative(playerNum, keyCode, action);
		return true;
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyDown keyCode=" + DebugInput.debugGetButtonName(keyCode));

		if (null == mActivity) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
	    if (playerNum < 0) {
	    	Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	return true;
	    }

		int action = keyEvent.getAction();
		dispatchKeyEventNative(playerNum, keyCode, action);
		return true;
	}
}
