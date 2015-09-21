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

import android.util.Log;
import com.adobe.fre.FREContext;
import com.adobe.fre.FREFunction;
import com.adobe.fre.FREObject;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaFacade;
import tv.ouya.console.api.OuyaFacade.DeviceHardware;

public class OuyaNativeFunctionGetDeviceHardwareName implements FREFunction {
	
	private static final String TAG = OuyaNativeFunctionGetDeviceHardwareName.class.getSimpleName();
	
	@Override
	public FREObject call(FREContext context, FREObject[] args) {
		
		try {
			OuyaFacade ouyaFacade = OuyaFacade.getInstance();
			if (null != ouyaFacade) {
				DeviceHardware deviceHardware = ouyaFacade.getDeviceHardware();
				if (null != deviceHardware) {
					String deviceName = deviceHardware.deviceName();
					if (null != deviceName){
						Log.d(TAG, "DeviceName: "+deviceName);
						return FREObject.newObject(deviceName);
					} else {
						Log.e(TAG, "DeviceName is null!");
					}
				} else {
					Log.e(TAG, "DeviceHardware is null!");
				}
			} else {
				Log.e(TAG, "OuyaFacade is null!");
			}
			return FREObject.newObject("UNKNOWN");
		} catch (Exception e) {
			e.printStackTrace();
			Log.e(TAG, "Unexpected exception");
		}
		
		return null;
	}
}
