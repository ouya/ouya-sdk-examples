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


public class CallbacksRequestGamerInfo {
	
	private static final String TAG = CallbacksRequestGamerInfo.class.getSimpleName();
	
	private int m_luaStackIndexOnSuccess = 1;
	private int m_luaReferenceKeyOnSuccess = 0;
	
	private int m_luaStackIndexOnFailure = 2;
	private int m_luaReferenceKeyOnFailure = 0;
	
	private int m_luaStackIndexOnCancel = 3;
	private int m_luaReferenceKeyOnCancel = 0;
	
	private com.ansca.corona.CoronaRuntimeTaskDispatcher m_dispatcher = null;
	
	public CallbacksRequestGamerInfo(com.naef.jnlua.LuaState luaState) {
		
		setupCallbackOnSuccess(luaState);
		setupCallbackOnFailure(luaState);
		setupCallbackOnCancel(luaState);
		
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
	
	public void onSuccess(final String jsonData) {
		
		Log.i("CallbacksRequestGamerInfo", "onSuccess=" + jsonData);
		
		// Post a Runnable object on the UI thread that will call the given Lua function.
		//Activity activity = com.ansca.corona.CoronaEnvironment.getCoronaActivity();
		Activity activity = IOuyaActivity.GetActivity();
		if (null == activity) {
			Log.i(TAG, "Activity is null");
			return;
		}
		activity.runOnUiThread(new Runnable() {
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
		
		Log.i("CallbacksRequestGamerInfo", "onFailure: errorCode=" + errorCode + " errorMessagee=" + errorMessage);
		
		// Post a Runnable object on the UI thread that will call the given Lua function.
		//Activity activity = com.ansca.corona.CoronaEnvironment.getCoronaActivity();
		Activity activity = IOuyaActivity.GetActivity();
		if (null == activity) {
			Log.i(TAG, "Activity is null");
			return;
		}
		activity.runOnUiThread(new Runnable() {
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
		
		Log.i("CallbacksRequestGamerInfo", "onCancel");
		
		// Post a Runnable object on the UI thread that will call the given Lua function.
		//Activity activity = com.ansca.corona.CoronaEnvironment.getCoronaActivity();
		Activity activity = IOuyaActivity.GetActivity();
		if (null == activity) {
			Log.i(TAG, "Activity is null");
			return;
		}
		activity.runOnUiThread(new Runnable() {
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
