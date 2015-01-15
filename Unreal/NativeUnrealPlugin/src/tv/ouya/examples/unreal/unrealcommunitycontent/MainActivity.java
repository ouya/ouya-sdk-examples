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

package tv.ouya.examples.unreal.unrealcommunitycontent;

import org.json.JSONArray;
import org.json.JSONObject;

import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaMod;
import tv.ouya.sdk.OuyaInputView;
import tv.ouya.sdk.unreal.AsyncCppInitOuyaPlugin;
import tv.ouya.sdk.unreal.AsyncCppOuyaContentInit;
import tv.ouya.sdk.unreal.IUnrealOuyaActivity;
import tv.ouya.sdk.unreal.UnrealOuyaFacade;
import tv.ouya.sdk.unreal.UnrealOuyaPlugin;
import android.os.Bundle;
import android.os.Handler;
import android.app.Activity;
import android.util.Log;
import android.view.Menu;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	OuyaInputView mInputView = null;

	static {
		System.loadLibrary("native-activity");
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		mInputView = new OuyaInputView(this);
		
		String jsonData = null;
		
		try {
			JSONArray jsonArray = new JSONArray();
			JSONObject jsonObject = new JSONObject();
			jsonObject.put("key", "tv.ouya.developer_id");
			jsonObject.put("value", "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
			jsonArray.put(0, jsonObject);
			jsonData = jsonArray.toString();
			
			IUnrealOuyaActivity.SetActivity(this);
			
			AsyncCppInitOuyaPlugin.invoke(jsonData);
			
			AsyncCppOuyaContentInit.invoke();
			
			testSave();
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	@Override
	public void onDestroy() {
		super.onDestroy();
		mInputView.shutdown();
	}
	
	void testSave() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized()) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testSave();
				}
			}, 1000);
		} else {
			
			Runnable runnable = new Runnable()
			{
				public void run()
				{
			
					try {
						OuyaContent ouyaContent = IUnrealOuyaActivity.GetOuyaContent();
						if (null == ouyaContent) {
							Log.e(TAG, "OUYA Content is null");
							return;
						}
						Log.i(TAG, "OuyaContent is initialize="+ouyaContent.isInitialized());
						Log.i(TAG, "OuyaContent is available="+ouyaContent.isAvailable());
						OuyaMod ouyaMod = ouyaContent.create();
						if (null == ouyaMod) {
							Log.e(TAG, "OUYA Mod is null");
							return;
						}
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "Unreal OUYA Facade is null");
							return;
						}
						OuyaMod.Editor editor = ouyaMod.edit();
						if (null == editor) {
							Log.e(TAG, "OUYA Mod Editor is null");
							return;
						}
						unrealOuyaFacade.Save(editor);
					} catch (Exception e) {
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
}
