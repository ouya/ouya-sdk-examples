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

package tv.ouya.sdk.examples.cordova.virtualcontroller;

import android.os.Bundle;
import android.util.SparseArray;
import android.view.MotionEvent;
import java.util.HashMap;
import org.apache.cordova.*;
import tv.ouya.console.api.OuyaController;

public class MainActivity extends CordovaActivity
{
    private static final String TAG = MainActivity.class.getSimpleName();

    private static SparseArray<HashMap<Integer, Float>> sAxisValues = new SparseArray<HashMap<Integer, Float>>();
    private static SparseArray<HashMap<Integer, Boolean>> sButtonValues = new SparseArray<HashMap<Integer, Boolean>>();

    static {
        for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index) {
            HashMap<Integer, Float> axisMap = new HashMap<Integer, Float>();
            axisMap.put(MotionEvent.AXIS_HAT_X, 0f);
            axisMap.put(MotionEvent.AXIS_HAT_Y, 0f);
            sAxisValues.put(index, axisMap);
            HashMap<Integer, Boolean> buttonMap = new HashMap<Integer, Boolean>();
            sButtonValues.put(index, buttonMap);
        }
    }

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        // Set by <content src="index.html" /> in config.xml
        loadUrl(launchUrl);
    }
}
