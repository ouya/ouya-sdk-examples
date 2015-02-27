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

import tv.ouya.console.api.OuyaController;
import tv.ouya.sdk.*;

import android.accounts.AccountManager;
import android.app.Activity;
import android.content.*;
import android.content.res.AssetManager;
import android.content.res.Configuration;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.InputDevice;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;

import com.unity3d.player.UnityPlayer;

import tv.ouya.console.api.*;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;

public class MainActivity extends OuyaUnityActivity
{
	private static final String TAG = MainActivity.class.getSimpleName();

	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);

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

		// Create the View
		RelativeLayout mainLayout = new RelativeLayout(this);
		LayoutParams lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		mainLayout.setLayoutParams(lp);

		// disable the screensaver
		mainLayout.setKeepScreenOn(true);

		LinearLayout linearLayout = new LinearLayout(this);
		lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		linearLayout.setOrientation(LinearLayout.VERTICAL);
		mainLayout.addView(linearLayout, 0, lp);
		
		FrameLayout unityLayout = new FrameLayout(this);
		lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		linearLayout.addView(unityLayout, 0, lp);
		
		// Create the UnityPlayer
        IOuyaActivity.SetUnityPlayer(new UnityPlayer(this));
        int glesMode = IOuyaActivity.GetUnityPlayer().getSettings().getInt("gles_mode", 1);
        boolean trueColor8888 = false;
        IOuyaActivity.GetUnityPlayer().init(glesMode, trueColor8888);
        setContentView(mainLayout);

        // Add the Unity view
        lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
        unityLayout.addView(IOuyaActivity.GetUnityPlayer().getView(), 0, lp);

		// Set the focus
		mainLayout.setFocusableInTouchMode(true);
	}

    /**
     * Broadcast listener to handle re-requesting the receipts when a user has re-authenticated
     */

    private BroadcastReceiver mAuthChangeReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
			if (null != unityOuyaFacade)
			{
				unityOuyaFacade.requestReceipts();
			}
        }
    };

    /**
     * Broadcast listener to handle menu appearing
     */

    private BroadcastReceiver mMenuAppearingReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			Log.i(TAG, "BroadcastReceiver intent=" + intent.getAction());
			if(intent.getAction().equals(OuyaIntent.ACTION_MENUAPPEARING)) {
				//pause music, free up resources, etc.

				//invoke a unity callback
				Log.i(TAG, "BroadcastReceiver tell Unity we see the menu appearing");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "onMenuAppearing", "");
				Log.i(TAG, "BroadcastReceiver notified Unity onMenuAppearing");
			}
        }
    };

    /**
     * Request an up to date list of receipts and start listening for any account changes
     * whilst the application is running.
     */
    @Override
    public void onStart() {
        super.onStart();

        // Request an up to date list of receipts for the user.
        //requestReceipts();

        // Register to receive notifications about account changes. This will re-query
        // the receipt list in order to ensure it is always up to date for whomever
        // is logged in.
        IntentFilter accountsChangedFilter = new IntentFilter();
        accountsChangedFilter.addAction(AccountManager.LOGIN_ACCOUNTS_CHANGED_ACTION);
        registerReceiver(mAuthChangeReceiver, accountsChangedFilter);

		IntentFilter menuAppearingFilter = new IntentFilter();
		menuAppearingFilter.addAction(OuyaIntent.ACTION_MENUAPPEARING);
		registerReceiver(mMenuAppearingReceiver, menuAppearingFilter);
    }

    /**
     * Unregister the account change listener when the application is stopped.
     */
    @Override
    public void onStop() {
		unregisterReceiver(mAuthChangeReceiver);
		unregisterReceiver(mMenuAppearingReceiver);
        super.onStop();
		finish();
    }

    @Override
	public void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
		Log.i(TAG, "onActivityResult");
		super.onActivityResult(requestCode, resultCode, data);
		UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
		if (null != unityOuyaFacade)
		{
			// Forward this result to the facade, in case it is waiting for any activity results
			if (unityOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
				return;
			}
		} else {
			Log.e(TAG, "MarmaladeOuyaFacade is null");
		}
	}

	@Override
    protected void onDestroy()
	{
		UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
		if (null != unityOuyaFacade)
		{
			unityOuyaFacade.onDestroy();
		}

		if (null != IOuyaActivity.GetUnityPlayer())
		{
			IOuyaActivity.GetUnityPlayer().quit();
		}

        super.onDestroy();
    }

    @Override
    public void onResume()
	{
		super.onResume();

		UnityPlayer.UnitySendMessage("OuyaGameObject", "onResume", "");
		if (null != IOuyaActivity.GetUnityPlayer()) {
			IOuyaActivity.GetUnityPlayer().resume();
		}
    }

    @Override
    public void onPause()
	{
		super.onPause();

		if (!isFinishing()) {
			UnityPlayer.UnitySendMessage("OuyaGameObject", "onPause", "");
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					if (null != IOuyaActivity.GetUnityPlayer()) {
						IOuyaActivity.GetUnityPlayer().pause();
					}
				}
			}, 250);
		}
    }

	public void onConfigurationChanged(Configuration newConfig)
	{
		super.onConfigurationChanged(newConfig);
		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i(TAG, "IOuyaActivity.GetUnityPlayer() is null");
			return;
		}
		IOuyaActivity.GetUnityPlayer().configurationChanged(newConfig);
	}
	public void onWindowFocusChanged(boolean hasFocus)
	{
		super.onWindowFocusChanged(hasFocus);
		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i(TAG, "IOuyaActivity.GetUnityPlayer() is null");
			return;
		}
		IOuyaActivity.GetUnityPlayer().windowFocusChanged(hasFocus);
	}
}
