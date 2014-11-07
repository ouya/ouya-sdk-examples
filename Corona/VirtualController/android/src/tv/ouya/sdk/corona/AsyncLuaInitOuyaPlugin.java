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

package tv.ouya.sdk.corona;

import android.app.Activity;


/**
 * Implements the printString() function in Lua.
 * <p>
 * Demonstrates how to fetch a string argument from a Lua function.
 */
public class AsyncLuaInitOuyaPlugin implements com.naef.jnlua.NamedJavaFunction {
	/**
	 * Gets the name of the Lua function as it would appear in the Lua script.
	 * @return Returns the name of the custom Lua function.
	 */
	@Override
	public String getName() {
		return "initOuyaPlugin";
	}
	
	/**
	 * This method is called when the Lua function is called.
	 * <p>
	 * Warning! This method is not called on the main UI thread.
	 * @param luaState Reference to the Lua state.
	 *                 Needed to retrieve the Lua function's parameters and to return values back to Lua.
	 * @return Returns the number of values to be returned by the Lua function.
	 */
	@Override
	public int invoke(final com.naef.jnlua.LuaState luaState) {

		final CallbacksInitOuyaPlugin callbacks = new CallbacksInitOuyaPlugin(luaState);
					
		// store for access
		IOuyaActivity.SetCallbacksInitOuyaPlugin(callbacks);

		Activity activity = IOuyaActivity.GetActivity();
		if (null != activity) {
			Runnable runnable = new Runnable()
			{
				public void run()
				{

					// Print the Lua function's argument to the Android logging system.
					try {
						CoronaOuyaPlugin.initOuyaPlugin(callbacks.getJSONData());
						callbacks.onSuccess();
					}
					catch (Exception ex) {
						// An exception will occur if given an invalid argument or no argument. Print the error.
						ex.printStackTrace();

						callbacks.onFailure(0, "Failed to initialize CoronaOuyaPlugin");
					}
				}
			};
			activity.runOnUiThread(runnable);
		};
		
		// Return 0 since this Lua function does not return any values.
		return 0;
	}
}
