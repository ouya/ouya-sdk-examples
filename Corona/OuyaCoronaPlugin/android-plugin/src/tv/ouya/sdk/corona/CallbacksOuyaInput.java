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


public class CallbacksOuyaInput {
	
	private static final String TAG = "CallbacksOuyaInput";
	
	private int m_luaStackIndexOnGenericMotionEvent = 1;
	private int m_luaReferenceKeyOnGenericMotionEvent = 0;
	
	private int m_luaStackIndexOnKeyDown = 2;
	private int m_luaReferenceKeyOnKeyDown = 0;
	
	private int m_luaStackIndexOnKeyUp = 3;
	private int m_luaReferenceKeyOnKeyUp = 0;
	
	private com.ansca.corona.CoronaRuntimeTaskDispatcher m_dispatcher = null;
	
	public CallbacksOuyaInput(com.naef.jnlua.LuaState luaState) {
		
		setupCallbackOnGenericMotionEvent(luaState);
		setupCallbackOnKeyDown(luaState);
		setupCallbackOnKeyUp(luaState);
		
		// Set up a dispatcher which allows us to send a task to the Corona runtime thread from another thread.
		m_dispatcher = new com.ansca.corona.CoronaRuntimeTaskDispatcher(luaState);
	}
	
	private void setupCallbackOnGenericMotionEvent(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexOnGenericMotionEvent, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexOnGenericMotionEvent);
		m_luaReferenceKeyOnGenericMotionEvent = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	private void setupCallbackOnKeyDown(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexOnKeyDown, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexOnKeyDown);
		m_luaReferenceKeyOnKeyDown = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	private void setupCallbackOnKeyUp(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexOnKeyUp, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexOnKeyUp);
		m_luaReferenceKeyOnKeyUp = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	public void onGenericMotionEvent(final int playerNum, final int axis, final float val) {
		
		//Log.i(TAG, "onGenericMotionEvent playerNum=" + playerNum + " axis="+axis + " val="+val);
		
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
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnGenericMotionEvent);
							
							// Remove the Lua function from the registry.
							//luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnGenericMotionEvent);
							
							// pass as argument
							luaState.pushInteger(playerNum);
							
							// pass as argument
							luaState.pushInteger(axis);
							
							// pass as argument
							luaState.pushNumber(val);
							
							luaState.call(3, 0);
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
	
	public void onKeyUp(final int playerNum, final int button) {
		
		//Log.i(TAG, "onKeyUp playerNum=" + playerNum + " button="+button);
		
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
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnKeyUp);
							
							// Remove the Lua function from the registry.
							//luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnKeyUp);
							
							// pass as argument
							luaState.pushInteger(playerNum);
							
							// pass as argument
							luaState.pushInteger(button);
							
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

	public void onKeyDown(final int playerNum, final int button) {
		
		//Log.i(TAG, "onKeyDown playerNum=" + playerNum + " button="+button);
		
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
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnKeyDown);
							
							// Remove the Lua function from the registry.
							//luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyOnKeyDown);
							
							// pass as argument
							luaState.pushInteger(playerNum);
							
							// pass as argument
							luaState.pushInteger(button);
							
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
}
