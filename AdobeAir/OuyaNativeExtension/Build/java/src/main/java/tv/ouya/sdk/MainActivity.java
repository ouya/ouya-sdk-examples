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
import android.content.Intent;
import android.content.res.AssetManager;
import android.graphics.Color;
import android.graphics.Point;
import android.os.Bundle;
import android.util.Log;
import android.view.Display;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.view.WindowManager;
import android.view.*;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.FrameLayout;
import java.io.IOException;
import java.io.InputStream;
import tv.ouya.console.api.*;
import tv.ouya.console.api.*;
import tv.ouya.sdk.*;

public class MainActivity extends Activity implements OnClickListener {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final boolean sEnableLogging = false;
	
	private static MainActivity sInstance = null;
	
    private OuyaInputView mInputView = null;
	
	private static String sDeveloperId;
	
	private static OuyaFacade sOuyaFacade = null;
	
	private static boolean sInitialized = false;
    
    @Override
    public void onCreate(Bundle savedInstanceState) {
		Log.d(TAG, "**** onCreate");
        super.onCreate(savedInstanceState);
		
		sInstance = this;
        
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
		
		OuyaController.init(this);
		
		if (null == sOuyaFacade) {
			byte[] applicationKey = null;
			
			// load the application key from assets
			try {
				AssetManager assetManager = getAssets();
				InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
				applicationKey = new byte[inputStream.available()];
				inputStream.read(applicationKey);
				inputStream.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
			
			if (null == applicationKey) {
				Log.e(TAG, "Failed to load signing key");
				return;
			}
			
			sOuyaFacade = OuyaFacade.getInstance();
			
			Bundle developerInfo = new Bundle();
			developerInfo.putString(OuyaFacade.OUYA_DEVELOPER_ID, sDeveloperId);
			developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, applicationKey);
			sOuyaFacade.init(this, developerInfo);
			
			Log.d(TAG, "Initialized OuyaFacade.");
			sInitialized = true;
		}
    }
    
	@Override
    public void onDestroy()
    {
    	super.onDestroy();
		if (null != mInputView) {
			mInputView.shutdown();
		}
		sInstance = null;
    }
	
	@Override
	protected void onStart() {
		super.onStart();
		Log.d(TAG, "**** onStart");
	}
	
	@Override
	protected void onPause() {
		super.onPause();
		Log.d(TAG, "**** onPause");
	}
	
	@Override
	protected void onResume() {
		super.onResume();
		Log.d(TAG, "**** onResume");
	}

	@Override
	protected void onStop() {
		super.onStop();
		Log.d(TAG, "**** onStop isFinishing="+isFinishing());
		finish();
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
	
	private static int getDisplayWidth() {
		Activity activity = sInstance;
		if (null != activity) {
			WindowManager windowManager = activity.getWindowManager();
			Display display = windowManager.getDefaultDisplay();
			Point size = new Point();
			display.getSize(size);
			return size.x;
		} else {
			return 0;
		}
	}

	private static int getDisplayHeight() {
		Activity activity = sInstance;
		if (null != activity) {
			WindowManager windowManager = activity.getWindowManager();
			Display display = windowManager.getDefaultDisplay();
			Point size = new Point();
			display.getSize(size);
			return size.y;
		} else {
			return 0;
		}
	}
	
	private static void updateSafeArea(float progress) {
		Activity activity = sInstance;
		if (null != activity) {
			//Log.d(TAG, "updateSafeArea: progress="+progress);
			// bring in by %
			float percent = 0.1f;
			float ratio = 1 - (1 - progress) * percent;
			float halfRatio = 1 - (1 - progress) * percent * 0.5f;
			float maxWidth = getDisplayWidth();
			float maxHeight = getDisplayHeight();
			FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
			LayoutParams layout = content.getLayoutParams();
			layout.width = (int)(maxWidth * ratio);
			layout.height = (int)(maxHeight * ratio);
			content.setLayoutParams(layout);
			content.setX(maxWidth - maxWidth * halfRatio);
			content.setY(maxHeight - maxHeight * halfRatio);
		}
	}
	
	public static void setResolution(final int width, final int height)
	{
		try
		{
			//Log.d(TAG, "setResolution: width="+width+" height="+height);
			Activity activity = sInstance;
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						
					}
				};
				activity.runOnUiThread(runnable);
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "setResolution: exception=" + e.toString());
		}
	}

	public static void setSafeArea(final float percentage)
	{
		try
		{
			//Log.d(TAG, "setSafeArea: percentage="+percentage);
			Activity activity = sInstance;
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						updateSafeArea(percentage);
					}
				};
				activity.runOnUiThread(runnable);
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "setSafeArea: exception=" + e.toString());
		}
	}
	
	public static void setDeveloperId(String developerId) {
		sDeveloperId = developerId;
	}
	
	public static boolean isInitialized() {
		return sInitialized;
	}
	
	// Process onActivityResult events that only happen for Activity
    @Override
    protected void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
        Log.i(TAG, "Processing activity result");
        
        if (null == sOuyaFacade) {
        	return;
        }

        // Forward this result to the facade, in case it is waiting for any activity results
        if (sOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
            return;
        }
    }
	
}
