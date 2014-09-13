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


/**
 * Implements the asyncCallLuaFunction() function in Lua.
 * <p>
 * Demonstrates how to fetch a "Lua function argument" and then call that Lua function from another thread.
 * <p>
 * You must never manipulate the Lua state from another thread or else race conditions and exceptions that
 * can crash the application can occur. This example demonstrates how to do this in a thread safe manner.
 */
public class AsyncLuaOuyaRequestPurchase implements com.naef.jnlua.NamedJavaFunction {
	/**
	 * Gets the name of the Lua function as it would appear in the Lua script.
	 * @return Returns the name of the custom Lua function.
	 */
	@Override
	public String getName() {
		return "asyncLuaOuyaRequestPurchase";
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
	public int invoke(com.naef.jnlua.LuaState luaState) {
		
		CallbacksRequestPurchase callbacks = new CallbacksRequestPurchase(luaState);
		
		// store for access
		IOuyaActivity.SetCallbacksRequestPurchase(callbacks);
		
		// invoke service
		CoronaOuyaPlugin.requestPurchaseAsync(callbacks.m_purchasable);
		
		// Return 0 since this Lua function does not return any values.
		return 0;
	}
}
