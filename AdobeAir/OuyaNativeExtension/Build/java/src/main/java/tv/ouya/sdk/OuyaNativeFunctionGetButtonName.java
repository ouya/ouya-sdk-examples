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
import tv.ouya.console.api.OuyaController.ButtonData;

public class OuyaNativeFunctionGetButtonName implements FREFunction {
	
	private static final String TAG = OuyaNativeFunctionGetButtonName.class.getSimpleName();
	
	@Override
	public FREObject call(FREContext context, FREObject[] args) {
		
		try {
			int button;
			if (args.length > 0) {
				button = args[0].getAsInt();
			} else {
				Log.e(TAG, "Missing button:int argument");
				return FREObject.newObject("UNKNOWN");
			}
			
			OuyaController.ButtonData buttonData = OuyaController.getButtonData(button);
			if (null != buttonData) {
				String buttonName = buttonData.buttonName;
				if (null != buttonName) {
					Log.d(TAG, "Button:"+button+" ButtonName: "+buttonName);
					return FREObject.newObject(buttonName);
				} else {
					Log.e(TAG, "ButtonName is null!");
				}
			} else {
				Log.e(TAG, "ButtonData is null!");
			}
			
			return FREObject.newObject("UNKNOWN");			
		} catch (Exception e) {
			e.printStackTrace();
			Log.e(TAG, "Unexpected exception");
		}
		
		return null;
	}
}
