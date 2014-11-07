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

package tv.ouya.sdk.marmalade;

import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;
import java.io.IOException;
import java.io.InputStream;


public class AsyncCppInitOuyaPlugin {

	static final String LOG_TAG = AsyncCppInitOuyaPlugin.class.getSimpleName();

	public static void invoke(String jsonData) {

		//Log.i(LOG_TAG, "invoke jsonData="+jsonData);		

		CallbacksInitOuyaPlugin callbacks = new CallbacksInitOuyaPlugin();

		IMarmaladeOuyaActivity.SetCallbacksInitOuyaPlugin(callbacks);

		// load the signing key from assets
		try {
			Context context = IMarmaladeOuyaActivity.GetActivity().getApplicationContext();
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IMarmaladeOuyaActivity.SetApplicationKey(applicationKey);
		} catch (IOException e) {
			e.printStackTrace();
			callbacks.onFailure(0, "Failed to read signing key");
			return;
		}

		if (null == IMarmaladeOuyaActivity.GetApplicationKey()) {
			callbacks.onFailure(0, "Signing key is missing");
			return;
		}
		
		try {
			MarmaladeOuyaPlugin.InitializePlugin(jsonData);
			callbacks.onSuccess();
		} catch (Exception e) {
			e.printStackTrace();
			callbacks.onFailure(0, "Failed to initialize plugin");
		}
    }
}