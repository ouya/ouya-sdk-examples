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
import android.app.NativeActivity;
import android.content.*;
import android.content.res.AssetManager;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;

import com.unity3d.player.UnityPlayer;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;

import tv.ouya.console.api.*;
import tv.ouya.sdk.*;

public class MainActivity extends Activity
{
	private static final String TAG = "MainActivity";

	private static final boolean sEnableLogging = false;

	protected UnityPlayer mUnityPlayer;		// don't change the name of this variable; referenced from native code

	OuyaInputView mInputView = null;

	// Setup activity layout
	@Override protected void onCreate (Bundle savedInstanceState)
	{
		//make activity accessible to Unity
		IOuyaActivity.SetActivity(this);

		//make bundle accessible to Unity
		if (null != savedInstanceState)
		{
			IOuyaActivity.SetSavedInstanceState(savedInstanceState);
		}

		// load the signing key from assets
		try {
			Context context = getApplicationContext();
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);
		} catch (IOException e) {
			e.printStackTrace();
		}

		requestWindowFeature(Window.FEATURE_NO_TITLE);
		super.onCreate(savedInstanceState);

		getWindow().takeSurface(null);
		setTheme(android.R.style.Theme_NoTitleBar_Fullscreen);
		getWindow().setFormat(PixelFormat.RGB_565);

		mUnityPlayer = new UnityPlayer(this);
		if (mUnityPlayer.getSettings ().getBoolean ("hide_status_bar", true))
			getWindow ().setFlags (WindowManager.LayoutParams.FLAG_FULLSCREEN,
			                       WindowManager.LayoutParams.FLAG_FULLSCREEN);

		setContentView(mUnityPlayer);
		//mUnityPlayer.requestFocus();

        mInputView = new OuyaInputView(this);
        
		if (sEnableLogging) {
			Log.d(TAG, "disable screensaver");
		}
        mInputView.setKeepScreenOn(true);

		keepFocus();
	}

	private void keepFocus() {
        final Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
				if (null != mInputView) {
					mInputView.requestFocus();
				}
				if (!isFinishing()) {
					keepFocus();
				}
            }
        }, 1000);
    }

	// Quit Unity
	@Override protected void onDestroy ()
	{
		mUnityPlayer.quit();
		super.onDestroy();
		mInputView.shutdown();
	}

	/**
     * Broadcast listener to handle menu appearing
     */

    private BroadcastReceiver mMenuAppearingReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			if (sEnableLogging) {
				Log.i(TAG, "BroadcastReceiver intent=" + intent.getAction());
			}
			if(intent.getAction().equals(OuyaIntent.ACTION_MENUAPPEARING)) {
				if (sEnableLogging) {
					Log.i(TAG, "OuyaGameObject->onMenuAppearing");
				}
				UnityPlayer.UnitySendMessage("OuyaGameObject", "onMenuAppearing", "");
			}
        }
    };

    @Override
    public void onStart() {
        super.onStart();

		IntentFilter menuAppearingFilter = new IntentFilter();
		menuAppearingFilter.addAction(OuyaIntent.ACTION_MENUAPPEARING);
		registerReceiver(mMenuAppearingReceiver, menuAppearingFilter);
    }

    @Override
    public void onStop() {
		unregisterReceiver(mMenuAppearingReceiver);
        super.onStop();
    }

	// Pause Unity
	@Override protected void onPause()
	{
		super.onPause();
		if (sEnableLogging) {
			Log.d(TAG, "OuyaGameObject->onPause");
		}
		UnityPlayer.UnitySendMessage("OuyaGameObject", "onPause", "");
		mUnityPlayer.pause();
		if (null != mInputView) {
			mInputView.requestFocus();
		}
	}

	// Resume Unity
	@Override protected void onResume()
	{
		super.onResume();
		mUnityPlayer.resume();
		if (sEnableLogging) {
			Log.d(TAG, "OuyaGameObject->onResume");
		}
		UnityPlayer.UnitySendMessage("OuyaGameObject", "onResume", "");
		if (null != mInputView) {
			mInputView.requestFocus();
		}
	}

	// This ensures the layout will be correct.
	@Override public void onConfigurationChanged(Configuration newConfig)
	{
		super.onConfigurationChanged(newConfig);
		mUnityPlayer.configurationChanged(newConfig);
	}

	// Notify Unity of the focus change.
	@Override public void onWindowFocusChanged(boolean hasFocus)
	{
		super.onWindowFocusChanged(hasFocus);
		mUnityPlayer.windowFocusChanged(hasFocus);
		UnityPlayer.UnitySendMessage("OuyaGameObject", "onResume", "");
		if (null != mInputView) {
			mInputView.requestFocus();
		}
	}

    @Override
	public void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
		if (sEnableLogging) {
			Log.i(TAG, "onActivityResult");
		}
		super.onActivityResult(requestCode, resultCode, data);
		UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
		if (null != unityOuyaFacade)
		{
			// Forward this result to the facade, in case it is waiting for any activity results
			if (unityOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
				return;
			}
		} else {
			Log.e(TAG, "UnityOuyaFacade is null");
		}
	}
}
