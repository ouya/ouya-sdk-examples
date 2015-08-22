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
import tv.ouya.sdk.marmalade.*;

public class ODK extends LoaderActivity
{
	private static final String TAG = ODK.class.getSimpleName();

	private static final boolean sEnableLogging = false;

	private static OuyaInputView sInputView = null;

    @Override
    public void onCreate(Bundle savedInstanceState)
	{
		if (sEnableLogging) {
			Log.i(TAG, "onCreate" );
		}
        super.onCreate(savedInstanceState);

		if (sEnableLogging) {
			Log.d(TAG, "Add inputView");
		}
		sInputView = new OuyaInputView(this);

		IMarmaladeOuyaActivity.SetActivity(this);
    }

	@Override
	protected void onStart() {
		super.onStart();

		View content = (View)findViewById(android.R.id.content);
		if (null != content) {
			if (sEnableLogging) {
				Log.d(TAG, "Disable screensaver" );
			}
			content.setKeepScreenOn(true);
		} else {
			Log.e(TAG, "Content view is missing");
		}
	}

	@Override
    public void onDestroy()
    {
    	super.onDestroy();
    	if (null != sInputView) {
    		sInputView.shutdown();
    	}
    }

	@Override
	public void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
		if (sEnableLogging) {
			Log.i(TAG, "onActivityResult");
		}
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

	@Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	//Log.i(TAG, "dispatchGenericMotionEvent");
    	//DebugInput.debugMotionEvent(motionEvent);
    	//DebugInput.debugOuyaMotionEvent(motionEvent);
		if (null == sInputView) {
			if (sEnableLogging) {
				Log.e(TAG, "dispatchGenericMotionEvent InputView is not initialized!");
			}
			return false;
		}
		return sInputView.dispatchGenericMotionEvent(motionEvent);
    }
	
	@Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		if (sEnableLogging) {
			Log.i(TAG, "dispatchKeyEvent keyCode=" + keyEvent.getKeyCode()+" name="+DebugInput.debugGetKeyEvent(keyEvent));
		}
		if (null == sInputView) {
			if (sEnableLogging) {
				Log.e(TAG, "dispatchKeyEvent InputView is not initialized!");
			}
			return false;
		}
		return sInputView.dispatchKeyEvent(keyEvent);
	}
}
