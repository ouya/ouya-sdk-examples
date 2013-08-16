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

package com.mycompany.simpleluaextension;

import tv.ouya.sdk.corona.*;

import android.accounts.AccountManager;
import android.app.Activity;
import android.app.NativeActivity;
import android.content.*;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.InputDevice;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;

import tv.ouya.console.api.*;
import tv.ouya.console.api.OuyaController;

/**
 * Extends the Application class to receive Corona runtime events and to extend the Lua API.
 * <p>
 * Only one instance of this class will be created by the Android OS. It will be created before this application's
 * activity is displayed and will persist after the activity is destroyed. The name of this class must be set in the
 * AndroidManifest.xml file's "application" tag or else an instance of this class will not be created on startup.
 */
public class CoronaOuyaActivity extends com.ansca.corona.CoronaActivity {
	
	private final String TAG = "CoronaOuyaActivity";
	
	private boolean m_waitToExit = true;
	
	/** Called when your application has started. */
	protected void onCreate(Bundle savedInstanceState) {
		
		//make activity accessible to Unity
		IOuyaActivity.SetActivity(this);

		//make bundle accessible to Unity
		IOuyaActivity.SetSavedInstanceState(savedInstanceState);
		
		super.onCreate(savedInstanceState);
		
		Log.i(TAG, "***Starting Activity*********");
		
		// load the raw resource for the application key
		try {
			InputStream inputStream = getResources().openRawResource(R.raw.key);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);
			
			Log.i(TAG, "***Loaded signing key*********");
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		Context context = getBaseContext();

		// Init the controller
		OuyaController.init(context);
		
		// Initialize the OUYA Corona Plugin
		CoronaOuyaPlugin ouyaCoronaPlugin = new CoronaOuyaPlugin();
		IOuyaActivity.SetOuyaCoronaPlugin(ouyaCoronaPlugin);
		
		Thread timer = new Thread() {
			public void run() {
				// wait for developer id to be set
				while (m_waitToExit) {
					
					final CoronaOuyaPlugin ouyaCoronaPlugin = IOuyaActivity.GetOuyaCoronaPlugin();
					if (null != ouyaCoronaPlugin) {
						if (ouyaCoronaPlugin.getDeveloperId() != "") {
							Log.i(TAG, "Detected developer id initializing...");
							
							Runnable runnable = new Runnable()
							{
								public void run()
								{
									Log.i(TAG, "OuyaCoronaPlugin initializing...");
									ouyaCoronaPlugin.InitializeTest();
								}
							};

							runOnUiThread(runnable);
							
							break;
						}
					}

					try {
						Thread.sleep(50);
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}
        };
        timer.start();
	}
	

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();

		m_waitToExit = false;
	}

    /**
     * Broadcast listener to handle re-requesting the receipts when a user has re-authenticated
     */

    private BroadcastReceiver mAuthChangeReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			CoronaOuyaFacade coronaOuyaFacade = IOuyaActivity.GetCoronaOuyaFacade();
			if (null != coronaOuyaFacade)
			{
				coronaOuyaFacade.requestReceipts();
			}
        }
    };

    /**
     * Broadcast listener to handle menu appearing
     */

    private BroadcastReceiver mMenuAppearingReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			Log.i("Unity", "BroadcastReceiver intent=" + intent.getAction());
			if(intent.getAction().equals(OuyaIntent.ACTION_MENUAPPEARING)) {
				//pause music, free up resources, etc.

				Log.i("Unity", "BroadcastReceiver tell Unity we see the menu appearing");
				//UnityPlayer.UnitySendMessage("OuyaGameObject", "onMenuAppearing", "");
				Log.i("Unity", "BroadcastReceiver notified Unity onMenuAppearing");
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
    }

    /**
     * Check for the result from a call through to the authentication intent. If the authentication was
     * successful then re-try the purchase.
     */

    @Override
    protected void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
        if(resultCode == RESULT_OK) {
        	CoronaOuyaFacade coronaOuyaFacade = IOuyaActivity.GetCoronaOuyaFacade();
			if (null != coronaOuyaFacade)
			{
				switch (requestCode) {
					case CoronaOuyaFacade.GAMER_UUID_AUTHENTICATION_ACTIVITY_ID:
						coronaOuyaFacade.fetchGamerUUID();
						break;
					case CoronaOuyaFacade.PURCHASE_AUTHENTICATION_ACTIVITY_ID:
						coronaOuyaFacade.restartInterruptedPurchase();
						break;
				}
            }
        }
    }
	
}
