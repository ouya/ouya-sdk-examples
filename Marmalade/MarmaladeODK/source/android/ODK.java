/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
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

/*
java implementation of the OuyaODK extension.

Add android-specific functionality here.

These functions are called via JNI from native code.
*/
/*
 * NOTE: This file was originally written by the extension builder, but will not
 * be overwritten (unless --force is specified) and is intended to be modified.
 */

package tv.ouya.sdk.marmalade;


import android.app.Activity;
import android.content.Intent;
import android.content.Context;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import com.ideaworks3d.marmalade.LoaderAPI;
import com.ideaworks3d.marmalade.LoaderActivity;
import java.io.IOException;
import java.io.InputStream;
import tv.ouya.console.api.*;
import tv.ouya.sdk.marmalade.*;

public class ODK extends LoaderActivity
{
	private static final String TAG = ODK.class.getSimpleName();

	private static final boolean sEnableLogging = false;

	// Only use native methods after the native plugin loads
	private static boolean mNativePluginLoaded = false;

    @Override
    public void onCreate(Bundle savedInstanceState)
	{
        Log.i(TAG, "onCreate" );
        super.onCreate(savedInstanceState);

		IMarmaladeOuyaActivity.SetActivity(this);

		OuyaController.init(this);

		OuyaInputMapper.init(this);
    }

	@Override
	protected void onStart() {
		super.onStart();

		View content = (View)findViewById(android.R.id.content);
		if (null != content) {
			Log.d(TAG, "Disable screensaver" );
			content.setKeepScreenOn(true);
		} else {
			Log.e(TAG, "Content view is missing");
		}
	}

    @Override
		protected void onDestroy() {

			Log.i(TAG, "onDestroy" );
			super.onDestroy();

			OuyaInputMapper.shutdown(this);
	}

	@Override
	public void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
		Log.i(TAG, "onActivityResult");
		super.onActivityResult(requestCode, resultCode, data);
		MarmaladeOuyaFacade marmaladeOuyaFacade = IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade();
		if (null != marmaladeOuyaFacade)
		{
			// Forward this result to the facade, in case it is waiting for any activity results
			if (marmaladeOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
				return;
			}
		} else {
			Log.e(TAG, "MarmaladeOuyaFacade is null");
		}
	}

	public static void nativeLoaded() {
		mNativePluginLoaded = true;
	}

	public native void dispatchGenericMotionEventNative(int deviceId, int axis, float value);
	public native void dispatchKeyEventNative(int deviceId, int keyCode, int action);

	@Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	//Log.i(TAG, "dispatchGenericMotionEvent");
    	//DebugInput.debugMotionEvent(motionEvent);
		if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
		    return OuyaInputMapper.dispatchGenericMotionEvent(this, motionEvent);
		}
    	return super.dispatchGenericMotionEvent(motionEvent);
    }
	
	@Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "dispatchKeyEvent keyCode="+keyEvent.getKeyCode());
		}
		InputDevice device = keyEvent.getDevice();
		if (null != device) {
			String name = device.getName();
			if (null != name &&
				name.equals("aml_keypad")) {
				switch (keyEvent.getKeyCode()) {
				case 24:
					if (sEnableLogging) {
						Log.i(TAG, "Volume Up detected.");
					}
					return false;
				case 25:
					if (sEnableLogging) {
						Log.i(TAG, "Volume Down detected.");
					}
					return false;
				case 66:
					if (sEnableLogging) {
						Log.i(TAG, "Remote button detected.");
					}
					if (keyEvent.getAction() == KeyEvent.ACTION_DOWN) {
						onKeyDown(OuyaController.BUTTON_O, keyEvent);
					} else if (keyEvent.getAction() == KeyEvent.ACTION_UP) {
						onKeyUp(OuyaController.BUTTON_O, keyEvent);
					}
					return false;
				}
			}
		}
	    if (OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
	    	return OuyaInputMapper.dispatchKeyEvent(this, keyEvent);
	    }
	    return super.dispatchKeyEvent(keyEvent);
    }

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//DebugInput.debugMotionEvent(motionEvent);

		if (!mNativePluginLoaded) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	//Log.w(TAG, "Failed to find playerId for Controller="+motionEvent.getDevice().getName());
	    	playerNum = 0;
	    }

		try {
			dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
			dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
			dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
			dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
			dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
			dispatchGenericMotionEventNative(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
		} catch (Exception e) {
			// axis might be used before native plugin loads
		}
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyUp keyCode=" + keyCode);
		//Log.i(TAG, "It came from=" + this.getClass().getSimpleName());
		//Log.i(TAG, "onKeyUp keyCode=" + DebugInput.debugGetButtonName(keyCode));

		if (!mNativePluginLoaded) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	Log.w(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	playerNum = 0;
	    }

		int action = keyEvent.getAction();
		try {
			dispatchKeyEventNative(playerNum, keyCode, action);
		} catch (Exception e) {
			// button might be used before native plugin loads
		}
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyDown keyCode=" + DebugInput.debugGetButtonName(keyCode));

		if (!mNativePluginLoaded) {
			return false;
		}

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	Log.w(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	playerNum = 0;
	    }

		int action = keyEvent.getAction();
		try {
			dispatchKeyEventNative(playerNum, keyCode, action);
		} catch (Exception e) {
			// button might be used before native plugin loads
		}
		return true;
	}
}
