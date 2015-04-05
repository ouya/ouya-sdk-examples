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
import android.util.Log;
import com.adobe.fre.FREContext;
import com.adobe.fre.FREFunction;
import com.adobe.fre.FREObject;
import tv.ouya.sdk.MainActivity;
import tv.ouya.sdk.OuyaInputView;

public class OuyaNativeFunctionInit implements FREFunction {
	
	private static final String TAG = OuyaNativeFunctionInit.class.getSimpleName();
	
	@Override
	public FREObject call(FREContext context, FREObject[] args) {
		
		final Activity activity = context.getActivity();
		if (null == activity) {
			Log.e(TAG, "Activity is null!");
			return null;
		} else {
			Log.i(TAG, "Activity is valid!");
		}
		
		Intent intent = new Intent(activity, MainActivity.class);
		activity.startActivity(intent);
		//activity.finish();
		
		/*
		OuyaInputView inputView = new OuyaInputView(activity);
		if (null == inputView) {
			Log.e(TAG, "InputView is null!");
		} else {        
			Log.d(TAG, "Disable screensaver.");
			inputView.setKeepScreenOn(true);
		}
		*/
		
		return null;
	}
}
