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
import android.util.Log;


/**
 * Implements the printString() function in Lua.
 * <p>
 * Demonstrates how to fetch a string argument from a Lua function.
 */
public class LuaOuyaGetStringResource implements com.naef.jnlua.NamedJavaFunction {

	private static final String TAG = LuaOuyaGetStringResource.class.getSimpleName();

	/**
	 * Gets the name of the Lua function as it would appear in the Lua script.
	 * @return Returns the name of the custom Lua function.
	 */
	@Override
	public String getName() {
		return "ouyaGetStringResource";
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

		String key = null;
		try {
			key = luaState.checkString(1);
			Log.i(TAG, "key="+key);
		}
		catch (Exception ex) {
			ex.printStackTrace();

			luaState.pushString("");
			return 1;
		}

		if (null == key) {
			luaState.pushString("");
			return 1;
		}

		String result = CoronaOuyaPlugin.getStringResource(key);
		if (null == result) {
			luaState.pushString("");
			return 1;
		}

		//Log.i(TAG, "result="+result);

		luaState.pushString(result);
		return 1;
	}
}
