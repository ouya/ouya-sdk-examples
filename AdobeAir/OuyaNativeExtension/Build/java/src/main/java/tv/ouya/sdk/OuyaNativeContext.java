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
import android.content.res.Configuration;
import android.util.Log;
import com.adobe.air.ActivityResultCallback;
import com.adobe.air.AndroidActivityWrapper;
import com.adobe.air.AndroidActivityWrapper.ActivityState;
import com.adobe.air.StateChangeCallback;
import com.adobe.fre.FREContext;
import com.adobe.fre.FREFunction;
import java.util.HashMap;
import java.util.Map;

public class OuyaNativeContext extends FREContext implements ActivityResultCallback, StateChangeCallback {
	
	private static final String TAG = OuyaNativeContext.class.getSimpleName();
	
	private AndroidActivityWrapper mAndroidActivityWrapper;
	
	private boolean mDetectedStop = false;
	
	public OuyaNativeContext() {  
        mAndroidActivityWrapper = AndroidActivityWrapper.GetAndroidActivityWrapper();  
        mAndroidActivityWrapper.addActivityResultListener(this);  
        mAndroidActivityWrapper.addActivityStateChangeListner(this);  
    }
	
	@Override  
    public void onActivityResult(int requestCode, int resultCode, Intent intent ) {  
    }
	
	@Override   
    public void onActivityStateChanged(ActivityState state) {
		Log.d(TAG, "***** onActivityStateChanged state="+state);
        switch ( state ) {
            case STOPPED:
				mDetectedStop = true;
				break;
			case RESUMED:
				if (mDetectedStop &&
					null != mAndroidActivityWrapper) {
					mDetectedStop = false;
					Activity activity = mAndroidActivityWrapper.getActivity();
					if (null != activity) {
						Log.d(TAG, "***** onActivityStateChanged: Relaunch MainActivity!");
						Intent intent = new Intent(activity, MainActivity.class);
						activity.startActivity(intent);
					} else {
						Log.e(TAG, "***** onActivityStateChanged: Activity is null!");
					}
				}
				break;
			case RESTARTED:
            case STARTED:      
            case PAUSED:  
            case DESTROYED:  
        }  
    }
	
	@Override  
    public void onConfigurationChanged(Configuration paramConfiguration) {
		if (null != mAndroidActivityWrapper) {  
            mAndroidActivityWrapper.removeActivityResultListener(this);  
            mAndroidActivityWrapper.removeActivityStateChangeListner(this);  
            mAndroidActivityWrapper = null;  
        }  
    }
	
	@Override
	public void dispose() {
	}
	
	@Override
	public Map<String, FREFunction> getFunctions() {
		Map<String, FREFunction> map = new HashMap<String, FREFunction>();
		
		map.put("ouyaInit", new OuyaNativeFunctionInit());
		map.put("ouyaIsAnyConnected", new OuyaNativeFunctionIsAnyConnected());
		map.put("ouyaIsConnected", new OuyaNativeFunctionIsConnected());
		map.put("ouyaGetAxis", new OuyaNativeFunctionGetAxis());
		map.put("ouyaGetAnyButton", new OuyaNativeFunctionGetAnyButton());
		map.put("ouyaGetAnyButtonDown", new OuyaNativeFunctionGetAnyButtonDown());
		map.put("ouyaGetAnyButtonUp", new OuyaNativeFunctionGetAnyButtonUp());
		map.put("ouyaGetButton", new OuyaNativeFunctionGetButton());
		map.put("ouyaGetButtonDown", new OuyaNativeFunctionGetButtonDown());
		map.put("ouyaGetButtonUp", new OuyaNativeFunctionGetButtonUp());
		map.put("ouyaClearButtonStatesPressedReleased", new OuyaNativeFunctionClearButtonStatesPressedReleased());
		map.put("ouyaGetTrackpadX", new OuyaNativeFunctionGetTrackpadX());
		map.put("ouyaGetTrackpadY", new OuyaNativeFunctionGetTrackpadY());
		map.put("ouyaGetTrackpadDown", new OuyaNativeFunctionGetTrackpadDown());
		
		map.put("ouyaLogInfo", new OuyaNativeFunctionLogInfo());
		map.put("ouyaLogError", new OuyaNativeFunctionLogError());
		
		map.put("ouyaToggleInputLogging", new OuyaNativeFunctionToggleInputLogging());
		
		map.put("ouyaSetResolution", new OuyaNativeFunctionSetResolution());
		map.put("ouyaSetSafeArea", new OuyaNativeFunctionSetSafeArea());
		
		map.put("ouyaGetDeviceHardwareName", new OuyaNativeFunctionGetDeviceHardwareName());
		map.put("ouyaGetButtonName", new OuyaNativeFunctionGetButtonName());
		
		map.put("ouyaIsInitialized", new OuyaNativeFunctionIsInitialized());
		
		map.put("ouyaRequestProducts", new OuyaNativeFunctionRequestProducts());
		map.put("ouyaRequestPurchase", new OuyaNativeFunctionRequestPurchase());
		map.put("ouyaRequestReceipts", new OuyaNativeFunctionRequestReceipts());
		map.put("ouyaRequestGamerInfo", new OuyaNativeFunctionRequestGamerInfo());
		map.put("ouyaShutdown", new OuyaNativeFunctionShutdown());
		
		return map;
	}
}
