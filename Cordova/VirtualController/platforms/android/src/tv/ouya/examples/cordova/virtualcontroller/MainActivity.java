/*
       Licensed to the Apache Software Foundation (ASF) under one
       or more contributor license agreements.  See the NOTICE file
       distributed with this work for additional information
       regarding copyright ownership.  The ASF licenses this file
       to you under the Apache License, Version 2.0 (the
       "License"); you may not use this file except in compliance
       with the License.  You may obtain a copy of the License at

         http://www.apache.org/licenses/LICENSE-2.0

       Unless required by applicable law or agreed to in writing,
       software distributed under the License is distributed on an
       "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
       KIND, either express or implied.  See the License for the
       specific language governing permissions and limitations
       under the License.
 */

package tv.ouya.examples.cordova.virtualcontroller;

import android.os.Bundle;
import android.util.Log;
import android.util.SparseArray;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import java.util.HashMap;
import org.apache.cordova.*;

import tv.ouya.console.api.OuyaController;
import tv.ouya.sdk.OuyaInputView;

public class MainActivity extends CordovaActivity
{
    private static final String TAG = MainActivity.class.getSimpleName();

    private static final boolean sEnableLogging = false;

    OuyaInputView mInputView = null;

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);

        mInputView = new OuyaInputView(this);

        // Set by <content src="index.html" /> in config.xml
        loadUrl(launchUrl);
    }

    @Override
    public void onDestroy ()
    {
        super.onDestroy();
        if (null != mInputView) {
            mInputView.shutdown();
        }
    }

    @Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
        if (sEnableLogging) {
            Log.i(TAG, "dispatchGenericMotionEvent");
        }
        if (null != mInputView) {
            mInputView.dispatchGenericMotionEvent(motionEvent);
        }
        return false;
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
                        //raiseVolume();
                        //return true; //the volume was handled
                        return false; //show the xiaomi volume overlay
                    case 25:
                        if (sEnableLogging) {
                            Log.i(TAG, "Volume Down detected.");
                        }
                        //lowerVolume();
                        //return true; //the volume was handled
                        return false; //show the xiaomi volume overlay
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
                    case 4:
                        if (sEnableLogging) {
                            Log.i(TAG, "Remote back button detected.");
                        }
                        if (null != mInputView) {
                            if (keyEvent.getAction() == KeyEvent.ACTION_DOWN) {
                                mInputView.onKeyDown(OuyaController.BUTTON_A, keyEvent);
                            } else if (keyEvent.getAction() == KeyEvent.ACTION_UP) {
                                mInputView.onKeyUp(OuyaController.BUTTON_A, keyEvent);
                            }
                        }
                        return true;
                }
            }
        }
        if (null != mInputView) {
            mInputView.dispatchKeyEvent(keyEvent);
        }
        return true;
    }
}
