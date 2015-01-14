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

package tv.ouya.sdk.unreal;

import android.app.Activity;
import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;
import java.io.IOException;
import java.io.InputStream;


public class AsyncCppInitOuyaPlugin {

	static final String LOG_TAG = AsyncCppInitOuyaPlugin.class.getSimpleName();

	public static void invoke(final String jsonData) {

		//Log.i(LOG_TAG, "invoke jsonData="+jsonData);

		final CallbacksInitOuyaPlugin callbacks = new CallbacksInitOuyaPlugin();

		IUnrealOuyaActivity.SetCallbacksInitOuyaPlugin(callbacks);

		final Activity activity = IUnrealOuyaActivity.GetActivity();

		if (null != activity) {
			Runnable runnable = new Runnable() {
				public void run() {

					// load the signing key from assets
					try {
						Context context = activity.getApplicationContext();
						AssetManager assetManager = context.getAssets();
						InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
						byte[] applicationKey = new byte[inputStream.available()];
						inputStream.read(applicationKey);
						inputStream.close();
						IUnrealOuyaActivity.SetApplicationKey(applicationKey);
					} catch (IOException e) {
						e.printStackTrace();
						callbacks.onFailure(0, "Failed to read signing key");
						return;
					}

					if (null == IUnrealOuyaActivity.GetApplicationKey()) {
						callbacks.onFailure(0, "Signing key is missing");
						return;
					}

					try {
						UnrealOuyaPlugin.InitializePlugin(jsonData);
						callbacks.onSuccess();
					} catch (Exception e) {
						e.printStackTrace();
						callbacks.onFailure(0, "Failed to initialize plugin");
					}
				}
			};
			activity.runOnUiThread(runnable);
		} else {
			callbacks.onFailure(0, "Activity is null");
		}
    }
}