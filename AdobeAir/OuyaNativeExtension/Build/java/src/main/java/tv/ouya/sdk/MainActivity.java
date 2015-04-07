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

import android.app.Activity;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.*;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.FrameLayout;
import tv.ouya.console.api.*;
import tv.ouya.sdk.*;

public class MainActivity extends Activity implements OnClickListener {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final boolean sEnableLogging = false;

    private OuyaInputView mInputView = null;
    
    @Override
    public void onCreate(Bundle savedInstanceState) {
		Log.i(TAG, "**** onCreate");
        super.onCreate(savedInstanceState);
        
        mInputView = new OuyaInputView(this);
        
        Log.d(TAG, "Disable screensaver");
        mInputView.setKeepScreenOn(true);
		
        FrameLayout content = (FrameLayout)findViewById(android.R.id.content);
		if (null != content) {
			Button button = new Button(this);
	        button.setOnClickListener(this);
	        button.setVisibility(View.VISIBLE);
	        button.setText("");
	        button.setBackgroundColor(Color.TRANSPARENT);
			content.addView(button);
		} else {
			Log.e(TAG, "Content view is missing");
		}
    }
    
	@Override
    public void onDestroy()
    {
    	super.onDestroy();
		if (null != mInputView) {
			mInputView.shutdown();
		}
    }
	
	@Override
	protected void onStart() {
		// TODO Auto-generated method stub
		super.onStart();
	}
	

	@Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "dispatchGenericMotionEvent");
		}
    	if (null != mInputView) {
			mInputView.dispatchGenericMotionEvent(motionEvent);
		}
		return true;
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
					if (null != mInputView) {
						if (keyEvent.getAction() == KeyEvent.ACTION_DOWN) {
							mInputView.onKeyDown(OuyaController.BUTTON_O, keyEvent);
						} else if (keyEvent.getAction() == KeyEvent.ACTION_UP) {
							mInputView.onKeyUp(OuyaController.BUTTON_O, keyEvent);
						}
					}
					return false;
				}
			}
		}
    	if (null != mInputView) {
			mInputView.dispatchKeyEvent(keyEvent);
		}
		return true;
    }

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "onGenericMotionEvent");
		}
    	if (null != mInputView) {
			mInputView.requestFocus();
		}
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "onKeyUp");
		}
    	if (null != mInputView) {
			mInputView.requestFocus();
		}
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "onKeyDown");
		}
    	if (null != mInputView) {
			mInputView.requestFocus();
		}
		return true;
	}
	
	@Override
	public boolean dispatchTouchEvent(MotionEvent motionEvent) {
		if (sEnableLogging) {
			Log.i(TAG, "dispatchTouchEvent source="+motionEvent.getSource()+" x="+motionEvent.getX()+" y="+motionEvent.getY());
		}
		int source = motionEvent.getSource();
		if (source == 8194) {
			OuyaInputView.setTrackpadX(motionEvent.getX());
			OuyaInputView.setTrackpadY(motionEvent.getY());
		}
		return super.dispatchTouchEvent(motionEvent);
	}
	
	@Override
	public void onClick(View view) {
		if (sEnableLogging) {
			Log.i(TAG, "Click happened");
		}
		OuyaInputView.setTrackpadDown();
	}
}
