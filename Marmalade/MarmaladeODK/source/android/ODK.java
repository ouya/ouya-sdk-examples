/*
java implementation of the OuyaODK extension.

Add android-specific functionality here.

These functions are called via JNI from native code.
*/
/*
 * NOTE: This file was originally written by the extension builder, but will not
 * be overwritten (unless --force is specified) and is intended to be modified.
 */

package com.ODK;


import android.app.Activity;
import android.app.NativeActivity;
import android.content.Context;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import com.ideaworks3d.marmalade.LoaderAPI;
import com.ideaworks3d.marmalade.LoaderActivity;
import java.io.IOException;
import java.io.InputStream;
import tv.ouya.console.api.OuyaController;

public class ODK extends LoaderActivity
{
	private static final String LOG_TAG = "ODK";

	// stop the thread on exit
	private static boolean m_waitToExit = true;

	public static ODK 				m_Activity = null;
	public static OuyaController	m_SelectedController = null;
    public static boolean			m_bInitialized = false;

    @Override
    public void onCreate(Bundle savedInstanceState)
	{
        Log.i(LOG_TAG, "onCreate" );
        super.onCreate(savedInstanceState);
        m_Activity = this;
		OuyaController.init(this);

		initializeOUYA();
    }

    @Override
		protected void onDestroy() {
			super.onDestroy();

			// notify threads it's time to exit
			m_waitToExit = false;
	}

	private void initializeOUYA() {

		Log.i(LOG_TAG, "Initializing OUYA...");

		Log.i(LOG_TAG, "Get application context...");
		Context context = getApplicationContext();

		Log.i(LOG_TAG, "Load signing key...");
		// load the application key from assets
		try {
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);

			Log.i(LOG_TAG, "***Loaded signing key*********");
		} catch (IOException e) {
			e.printStackTrace();
		}

		Log.i(LOG_TAG, "Initialize MarmaladeOuyaPlugin...");

		// Initialize the Marmalade OUYA Plugin
		MarmaladeOuyaPlugin marmaladeOuyaPlugin = new MarmaladeOuyaPlugin();
		IOuyaActivity.SetMarmaladeOuyaPlugin(marmaladeOuyaPlugin);

		//make activity accessible to statically
		IOuyaActivity.SetActivity(this);

		Log.i(LOG_TAG, "Spawn wait thread...");

		// spawn thread to wait to initialize ouya facade
		Thread timer = new Thread() {
			public void run() {
				// wait for developer id to be set
				while (m_waitToExit) {

					final MarmaladeOuyaPlugin marmaladeOuyaPlugin = IOuyaActivity.GetMarmaladeOuyaPlugin();
					if (null != marmaladeOuyaPlugin) {
						if (marmaladeOuyaPlugin.getDeveloperId() != "") {
							Log.i(LOG_TAG, "Detected developer id initializing...");

							Runnable runnable = new Runnable()
							{
								public void run()
								{
									Log.i(LOG_TAG, "MarmaladeOuyaPlugin initializing...");
									marmaladeOuyaPlugin.Initialize();
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
	public boolean onKeyDown(int keyCode, KeyEvent event)
	{
		//Log.i(LOG_TAG, "onKeyDown keyCode="+keyCode);
		boolean handled = OuyaController.onKeyDown(keyCode, event);
		return handled || super.onKeyDown(keyCode, event);
	}


	@Override
	public boolean onKeyUp(int keyCode, KeyEvent event)
	{
		//Log.i(LOG_TAG, "onKeyUp keyCode="+keyCode);
		boolean handled = OuyaController.onKeyUp(keyCode, event);
		return handled || super.onKeyUp(keyCode, event);
	}

	@Override
	public boolean onGenericMotionEvent(MotionEvent event)
	{
		//Log.i(LOG_TAG, "onGenericMotionEvent");
		boolean handled = OuyaController.onGenericMotionEvent(event);
		return handled || super.onGenericMotionEvent(event);
	}


    public void OuyaController_startOfFrame()
    {
		//Log.i(LOG_TAG, "OuyaController_startOfFrame");
        OuyaController.startOfFrame();
    }


    public boolean OuyaController_selectControllerByPlayer(int playerNum)
    {
		//Log.i(LOG_TAG, "OuyaController_selectControllerByPlayer playerNum="+playerNum);
		m_SelectedController = OuyaController.getControllerByPlayer(playerNum);
        return (m_SelectedController!=null);
    }


    public boolean OuyaController_selectControllerByDeviceID(int deviceID)
    {
		//Log.i(LOG_TAG, "OuyaController_selectControllerByDeviceID deviceID="+deviceID);
        m_SelectedController = OuyaController.getControllerByDeviceId(deviceID);
        return (m_SelectedController!=null);
    }


    public int OuyaController_getAxisValue(int axis)
    {
		//Log.i(LOG_TAG, "OuyaController_getAxisValue axis="+axis);
		if (m_SelectedController!=null)
			return (int)(m_SelectedController.getAxisValue(axis)*256.0f);
		else
			return 0;
    }


    public boolean OuyaController_getButton(int button)
    {
		//Log.i(LOG_TAG, "OuyaController_getButton button="+button);
        if (m_SelectedController!=null)
			return m_SelectedController.getButton(button);
		else
			return false;
    }


    public boolean OuyaController_buttonPressedThisFrame(int button)
    {
		//Log.i(LOG_TAG, "OuyaController_buttonPressedThisFrame button="+button);
		if (m_SelectedController!=null)
			return m_SelectedController.buttonPressedThisFrame(button);
		else
			return false;
    }


    public boolean OuyaController_buttonReleasedThisFrame(int button)
    {
		//Log.i(LOG_TAG, "OuyaController_buttonReleasedThisFrame button="+button);
        if (m_SelectedController!=null)
			return m_SelectedController.buttonReleasedThisFrame(button);
		else
			return false;
    }


    public boolean OuyaController_buttonChangedThisFrame(int button)
    {
		//Log.i(LOG_TAG, "OuyaController_buttonChangedThisFrame button="+button);
        if (m_SelectedController!=null)
			return m_SelectedController.buttonChangedThisFrame(button);
		else
			return false;
    }


    public int OuyaController_getPlayerNum()
    {
		//Log.i(LOG_TAG, "OuyaController_getPlayerNum");
		if (m_SelectedController!=null)
			return m_SelectedController.getPlayerNum();
		else
			return -1;
    }

    public void OuyaPlugin_asyncSetDeveloperId(String developerId)
    {
		Log.i(LOG_TAG, "OuyaPlugin_asyncSetDeveloperId developerId="+developerId);
    }
}