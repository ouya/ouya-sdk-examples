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

public class OuyaNativeFunctionLogError implements FREFunction {
	
	private static final String TAG = OuyaNativeFunctionLogError.class.getSimpleName();
	
	@Override
	public FREObject call(FREContext context, FREObject[] args) {
		
		try {
			String message;
			if (args.length > 0) {
				message = args[0].getAsString();
			} else {
				Log.e(TAG, "Missing message:String argument");
				return FREObject.newObject((double)0);
			}
			Log.e(TAG, message);
		} catch (Exception e) {
			e.printStackTrace();
			Log.e(TAG, "Unexpected exception");
		}
		
		return null;
	}
}
