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

package tv.ouya.sdk.monogame;


import android.R;
import android.app.Activity;
import android.app.NativeActivity;
import android.content.Context;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;

import tv.ouya.console.api.OuyaController;


public class OuyaMonoGameActivity extends NativeActivity
{
	private static final String LOG_TAG = "OuyaNativeActivity";

	// stop the thread on exit
	private static boolean m_waitToExit = true;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		
		//make activity accessible to statically
		IOuyaActivity.SetActivity(this);

		initializeOUYA();
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();

		// notify threads it's time to exit
		OuyaMonoGameActivity.m_waitToExit = false;
	}

	static {
        //System.loadLibrary("inapppurchasesnative");
    }

	public static void initializeOUYA() {
		
		final Activity activity = IOuyaActivity.GetActivity();
		
		if (null == activity) {
			Log.i(LOG_TAG, "Activity is null...");
			return;
		}

		Log.i(LOG_TAG, "Initializing OUYA...");
		

		Log.i(LOG_TAG, "Get application context...");
		Context context = activity.getApplicationContext();

		/*
		Log.i(LOG_TAG, "Load signing key...");
		// load the application key from assets
		try {
			int resId = context.getResources().getIdentifier("key", "raw", context.getPackageName());
			InputStream inputStream = activity.getResources().openRawResource(resId);
            byte[] applicationKey = new byte[inputStream.available()];
            inputStream.read(applicationKey);
            inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);

			Log.i(LOG_TAG, "***Loaded signing key*********");
		} catch (IOException e) {
			Log.i(LOG_TAG, "***Failed to load signing key*********");
			e.printStackTrace();
		}
		*/
		
		Log.i(LOG_TAG, "Initialize controller...");

		// Init the controller
		OuyaController.init(context);


		Log.i(LOG_TAG, "Initialize MonoGameOuyaPlugin...");

		// Initialize the MonoGame OUYA Plugin
		MonoGameOuyaPlugin monoGameOuyaPlugin = new MonoGameOuyaPlugin();
		IOuyaActivity.SetMonoGameOuyaPlugin(monoGameOuyaPlugin);

		Log.i(LOG_TAG, "Spawn wait thread...");

		// spawn thread to wait to initialize ouya facade
		Thread timer = new Thread() {
			public void run() {
				// wait for developer id to be set
				while (OuyaMonoGameActivity.m_waitToExit) {

					final MonoGameOuyaPlugin monoGameOuyaPlugin = IOuyaActivity.GetMonoGameOuyaPlugin();
					if (null != monoGameOuyaPlugin) {
						if (monoGameOuyaPlugin.getDeveloperId() != "") {
							Log.i(LOG_TAG, "Detected developer id initializing...");

							Runnable runnable = new Runnable()
							{
								public void run()
								{
									Log.i(LOG_TAG, "MonoGameOuyaPlugin initializing...");
									monoGameOuyaPlugin.Initialize();
								}
							};

							activity.runOnUiThread(runnable);

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
}