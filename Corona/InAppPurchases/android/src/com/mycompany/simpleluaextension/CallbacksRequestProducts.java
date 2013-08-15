package com.mycompany.simpleluaextension;

import android.util.Log;


public class CallbacksRequestProducts {
	
	private int m_luaStackIndexOnSuccess = 1;
	private int m_luaReferenceKeyOnSuccess = 0;
	
	private int m_luaStackIndexOnFailure = 2;
	private int m_luaReferenceKeyOnFailure = 0;
	
	private int m_luaStackIndexOnCancel = 3;
	private int m_luaReferenceKeyOnCancel = 0;
	
	private int m_luaStackIndexProducts = 4;
	
	private com.ansca.corona.CoronaRuntimeTaskDispatcher m_dispatcher = null;
	
	public CallbacksRequestProducts(com.naef.jnlua.LuaState luaState) {
		
		setupCallbackOnSuccess(luaState);
		setupCallbackOnFailure(luaState);
		setupCallbackOnCancel(luaState);
		
		setupProducts(luaState);
		
		// Set up a dispatcher which allows us to send a task to the Corona runtime thread from another thread.
		m_dispatcher = new com.ansca.corona.CoronaRuntimeTaskDispatcher(luaState);
	}
	
	private void setupCallbackOnSuccess(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexOnSuccess, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexOnSuccess);
		m_luaReferenceKeyOnSuccess = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	private void setupCallbackOnFailure(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexOnFailure, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexOnFailure);
		m_luaReferenceKeyOnFailure = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	private void setupCallbackOnCancel(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexOnCancel, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexOnCancel);
		m_luaReferenceKeyOnCancel = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	private void setupProducts(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexProducts, com.naef.jnlua.LuaType.TABLE);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		//clear the list of products
		CoronaOuyaPlugin.clearGetProductList();
		
		// Print the Lua function's argument to the Android logging system.
		try {
			// Check if the Lua function's first argument is a Lua table.
			// Will throw an exception if it is not a table or if no argument was given.
			int luaTableStackIndex = m_luaStackIndexProducts;
			luaState.checkType(luaTableStackIndex, com.naef.jnlua.LuaType.TABLE);
			
			// Print all of the key/value paris in the Lua table.
			System.out.println("printTable()");
			System.out.println("{");
			for (luaState.pushNil(); luaState.next(luaTableStackIndex); luaState.pop(1)) {
				// Fetch the table entry's string key.
				// An index of -2 accesses the key that was pushed into the Lua stack by luaState.next() up above.
				String keyName = null;
				com.naef.jnlua.LuaType luaType = luaState.type(-2);
				switch (luaType) {
					case STRING:
						// Fetch the table entry's string key.
						keyName = luaState.toString(-2);
						break;
					case NUMBER:
						// The key will be a number if the given Lua table is really an array.
						// In this case, the key is an array index. Do not call luaState.toString() on the
						// numeric key or else Lua will convert the key to a string from within the Lua table.
						keyName = Integer.toString(luaState.toInteger(-2));
						break;
				}
				if (keyName == null) {
					// A valid key was not found. Skip this table entry.
					continue;
				}
				
				// Fetch the table entry's value in string form.
				// An index of -1 accesses the entry's value that was pushed into the Lua stack by luaState.next() above.
				String valueString = null;
				luaType = luaState.type(-1);
				switch (luaType) {
					case STRING:
						valueString = luaState.toString(-1);
						break;
					case BOOLEAN:
						valueString = Boolean.toString(luaState.toBoolean(-1));
						break;
					case NUMBER:
						valueString = Double.toString(luaState.toNumber(-1));
						break;
					default:
						valueString = luaType.displayText();
						break;
				}
				if (valueString == null) {
					valueString = "";
				}
				
				// Print the table entry to the Android logging system.
				System.out.println("   [" + keyName + "] = " + valueString);
				
				//add to the product list
				CoronaOuyaPlugin.addGetProduct(valueString);
			}
			System.out.println("}");
			
			//show added products
			CoronaOuyaPlugin.debugGetProductList();
		}
		catch (Exception ex) {
			// An exception will occur if given an invalid argument or no argument. Print the error.
			ex.printStackTrace();
		}
		
		
		
	}
	
	public void onSuccess(final String jsonData) {
		
		Log.i("CallbacksRequestProducts", "onSuccess jsonData=" + jsonData);
		
		// Post a Runnable object on the UI thread that will call the given Lua function.
		com.ansca.corona.CoronaEnvironment.getCoronaActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				// *** We are now running in the main UI thread. ***
				
				// Create a task that will call the given Lua function.
				// This task's execute() method will be called on the Corona runtime thread, just before rendering a frame.
				com.ansca.corona.CoronaRuntimeTask task = new com.ansca.corona.CoronaRuntimeTask() {
					@Override
					public void executeUsing(com.ansca.corona.CoronaRuntime runtime) {
						// *** We are now running on the Corona runtime thread. ***
						try {
							// Fetch the Corona runtime's Lua state.
							com.naef.jnlua.LuaState luaState = runtime.getLuaState();
							
							// Fetch the Lua function stored in the registry and push it to the top of the stack.
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnSuccess);
							
							// Remove the Lua function from the registry.
							luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnSuccess);
							
							// pass as argument
							luaState.pushString(jsonData);
							
							// call the method and pass the jsonData
							luaState.call(1, 0);
						}
						catch (Exception ex) {
							ex.printStackTrace();
						}
					}
				};
				
				// Send the above task to the Corona runtime asynchronously.
				m_dispatcher.send(task);
			}
		});
	}
	
	public void onFailure(final int errorCode, final String errorMessage) {
		
		Log.i("CallbacksRequestProducts", "onFailure: errorCode=" + errorCode + " errorMessagee=" + errorMessage);
		
		// Post a Runnable object on the UI thread that will call the given Lua function.
		com.ansca.corona.CoronaEnvironment.getCoronaActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				// *** We are now running in the main UI thread. ***
				
				// Create a task that will call the given Lua function.
				// This task's execute() method will be called on the Corona runtime thread, just before rendering a frame.
				com.ansca.corona.CoronaRuntimeTask task = new com.ansca.corona.CoronaRuntimeTask() {
					@Override
					public void executeUsing(com.ansca.corona.CoronaRuntime runtime) {
						// *** We are now running on the Corona runtime thread. ***
						try {
							// Fetch the Corona runtime's Lua state.
							com.naef.jnlua.LuaState luaState = runtime.getLuaState();
							
							// Fetch the Lua function stored in the registry and push it to the top of the stack.
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnFailure);
							
							// Remove the Lua function from the registry.
							luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnFailure);
							
							// pass as argument
							luaState.pushInteger(errorCode);
							
							// pass as argument
							luaState.pushString(errorMessage);
							
							luaState.call(2, 0);
						}
						catch (Exception ex) {
							ex.printStackTrace();
						}
					}
				};
				
				// Send the above task to the Corona runtime asynchronously.
				m_dispatcher.send(task);
			}
		});
	}
	
	public void onCancel() {
		
		Log.i("CallbacksRequestProducts", "onCancel");
		
		// Post a Runnable object on the UI thread that will call the given Lua function.
		com.ansca.corona.CoronaEnvironment.getCoronaActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				// *** We are now running in the main UI thread. ***
				
				// Create a task that will call the given Lua function.
				// This task's execute() method will be called on the Corona runtime thread, just before rendering a frame.
				com.ansca.corona.CoronaRuntimeTask task = new com.ansca.corona.CoronaRuntimeTask() {
					@Override
					public void executeUsing(com.ansca.corona.CoronaRuntime runtime) {
						// *** We are now running on the Corona runtime thread. ***
						try {
							// Fetch the Corona runtime's Lua state.
							com.naef.jnlua.LuaState luaState = runtime.getLuaState();
							
							// Fetch the Lua function stored in the registry and push it to the top of the stack.
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnCancel);
							
							// Remove the Lua function from the registry.
							luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnCancel);
							
							luaState.call(0, 0);
						}
						catch (Exception ex) {
							ex.printStackTrace();
						}
					}
				};
				
				// Send the above task to the Corona runtime asynchronously.
				m_dispatcher.send(task);
			}
		});
	}
	
}
