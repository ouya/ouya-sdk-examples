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

package tv.ouya.examples.corona.virtualcontroller;

import tv.ouya.sdk.corona.*;

import android.accounts.AccountManager;
import android.app.Activity;
import android.app.NativeActivity;
import android.content.*;
import android.content.res.AssetManager;
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
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.ArrayList;

import tv.ouya.console.api.*;

/**
 * Extends the Application class to receive Corona runtime events and to extend the Lua API.
 * <p>
 * Only one instance of this class will be created by the Android OS. It will be created before this application's
 * activity is displayed and will persist after the activity is destroyed. The name of this class must be set in the
 * AndroidManifest.xml file's "application" tag or else an instance of this class will not be created on startup.
 */
public class MainActivity extends CoronaOuyaActivity {
//public class MainActivity extends com.ansca.corona.CoronaActivity {
	
	private final String TAG = "MainActivity";
	
	private boolean m_waitToExit = true;
	
	/** Called when your application has started. */
	protected void onCreate(Bundle savedInstanceState) {
		
		//make activity accessible to Corona
		IOuyaActivity.SetActivity(this);

		//make bundle accessible to Corona
		IOuyaActivity.SetSavedInstanceState(savedInstanceState);
		
		super.onCreate(savedInstanceState);
		
		Log.i(TAG, "***Starting Activity*********");

		Context context = getBaseContext();

		// load the application key from Corona assets
		try {
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);
			
			Log.i(TAG, "***Loaded signing key*********");
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		/*
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
		*/

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
		super.onDestroy();
		m_waitToExit = false;
	}
	
	/*
	
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		//Log.i(TAG, "dispatchKeyEvent " + keyEvent.getClass().getName());
		if (keyEvent.getClass().getName() == "com.ansca.corona.input.CoronaKeyEvent") {
			//eat the extra event
			return true;
		}
		Log.i(TAG, "dispatchKeyEvent keyCode=" + keyEvent.getKeyCode() + " " + DebugInput.debugGetButtonName(keyEvent.getKeyCode()) + " action="+keyEvent.getAction()+" source="+keyEvent.getSource());
        return super.dispatchKeyEvent(keyEvent);
	}
	
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		return super.dispatchGenericMotionEvent(motionEvent);
	}
	*/
	
	///*
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//DebugInput.debugMotionEvent(motionEvent);

		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	Log.e(TAG, "Failed to find playerId for Controller="+motionEvent.getDevice().getName());
	    	return true;
	    }
	    
	    CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {
	    	
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
	    	input.onGenericMotionEvent(playerNum, OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));
	    }
	    
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyUp keyCode=" + DebugInput.debugGetButtonName(keyCode));

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	return true;
	    }

		int action = keyEvent.getAction();
		//dispatchKeyEventNative(playerNum, keyCode, action);
		
		CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {	
	    	input.onKeyUp(playerNum, keyCode);
	    }
		
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyDown keyCode=" + DebugInput.debugGetButtonName(keyCode));

		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());	    
	    if (playerNum < 0) {
	    	Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
	    	return true;
	    }

		int action = keyEvent.getAction();
		//dispatchKeyEventNative(playerNum, keyCode, action);
		
		CallbacksOuyaInput input = IOuyaActivity.GetCallbacksOuyaInput();
	    if (null != input) {	
	    	input.onKeyDown(playerNum, keyCode);
	    }
		
		return true;
	}
	//*/

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
    }

    /**
     * Unregister the account change listener when the application is stopped.
     */
    @Override
    public void onStop() {
		unregisterReceiver(mAuthChangeReceiver);
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
						coronaOuyaFacade.requestGamerInfo();
						break;
					case CoronaOuyaFacade.PURCHASE_AUTHENTICATION_ACTIVITY_ID:
						coronaOuyaFacade.restartInterruptedPurchase();
						break;
				}
            }
        }
    }
	
}
