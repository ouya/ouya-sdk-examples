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

import com.ideaworks3d.marmalade.LoaderAPI;
import com.ideaworks3d.marmalade.LoaderActivity;
import android.util.Log;
import android.app.Activity;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.os.Bundle;
import tv.ouya.console.api.OuyaController;

public class ODK extends LoaderActivity
{
	private final String LOG_TAG = "ODK";

	public static ODK 				m_Activity = null;
	public static OuyaController	m_SelectedController = null;
    public static boolean			m_bInitialized = false;

    @Override
    public void onCreate(Bundle savedInstanceState)
	{
        Log.v(LOG_TAG, "ODK onCreate OK!" );
        super.onCreate(savedInstanceState);
        m_Activity = this;
		OuyaController.init(this);
    }


	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event)
	{
		boolean handled = OuyaController.onKeyDown(keyCode, event);
		return handled || super.onKeyDown(keyCode, event);
	}


	@Override
	public boolean onKeyUp(int keyCode, KeyEvent event)
	{
		boolean handled = OuyaController.onKeyUp(keyCode, event);
		return handled || super.onKeyUp(keyCode, event);
	}

	@Override
	public boolean onGenericMotionEvent(MotionEvent event)
	{
		boolean handled = OuyaController.onGenericMotionEvent(event);
		return handled || super.onGenericMotionEvent(event);
	}


    public void OuyaController_startOfFrame()
    {
        OuyaController.startOfFrame();
    }


    public boolean OuyaController_selectControllerByPlayer(int playerNum)
    {
		m_SelectedController = OuyaController.getControllerByPlayer(playerNum);
        return (m_SelectedController!=null);
    }


    public boolean OuyaController_selectControllerByDeviceID(int deviceID)
    {
        m_SelectedController = OuyaController.getControllerByDeviceId(deviceID);
        return (m_SelectedController!=null);
    }


    public int OuyaController_getAxisValue(int axis)
    {
		if (m_SelectedController!=null)
			return (int)(m_SelectedController.getAxisValue(axis)*256.0f);
		else
			return 0;
    }


    public boolean OuyaController_getButton(int button)
    {
        if (m_SelectedController!=null)
			return m_SelectedController.getButton(button);
		else
			return false;
    }


    public boolean OuyaController_buttonPressedThisFrame(int button)
    {
       if (m_SelectedController!=null)
			return m_SelectedController.buttonPressedThisFrame(button);
		else
			return false;
    }


    public boolean OuyaController_buttonReleasedThisFrame(int button)
    {
        if (m_SelectedController!=null)
			return m_SelectedController.buttonReleasedThisFrame(button);
		else
			return false;
    }


    public boolean OuyaController_buttonChangedThisFrame(int button)
    {
        if (m_SelectedController!=null)
			return m_SelectedController.buttonChangedThisFrame(button);
		else
			return false;
    }


    public int OuyaController_getPlayerNum()
    {
         if (m_SelectedController!=null)
			return m_SelectedController.getPlayerNum();
		else
			return -1;
    }
}
