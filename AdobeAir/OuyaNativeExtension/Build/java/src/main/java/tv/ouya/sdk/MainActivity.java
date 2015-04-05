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
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.*;
import tv.ouya.sdk.OuyaInputView;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
    OuyaInputView mInputView = null;
    
    @Override
    public void onCreate(Bundle savedInstanceState) {
		Log.i(TAG, "**** onCreate");
        super.onCreate(savedInstanceState);
        //int id = getResources().getIdentifier("main", "layout", getPackageName());
        //Log.i(TAG, "********** Layout id===="+id);
        //setContentView(id);
        
        //mInputView = new OuyaInputView(this);
        
        //Log.d(TAG, "disable screensaver");
        //mInputView.setKeepScreenOn(true);
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
		Log.i(TAG, "dispatchGenericMotionEvent");
		return false;
	}
	
	@Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		Log.i(TAG, "dispatchKeyEvent");
		return false;
	}
	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		Log.i(TAG, "onGenericMotionEvent");
		return false;
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		Log.i(TAG, "onKeyUp");
		return false;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		Log.i(TAG, "onKeyDown");
		return false;
	}
}
