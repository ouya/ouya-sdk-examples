package com.mycompany.simpleluaextension;
//package tv.ouya.examples.corona.inapppurchases;


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
