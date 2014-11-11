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

import tv.ouya.console.api.OuyaController;
import android.app.Activity;
import android.util.Log;


public class CallbacksGetControllerName {
	
	private static final String TAG = CallbacksGetControllerName.class.getSimpleName();
	
	private int m_luaStackIndexInvoke = 1;
	private int m_luaReferenceKeyInvoke = 0;
	
	private int m_luaStackIndexPlayerNum = 2;
	private int m_luaPlayerNum = 0;
	
	private com.ansca.corona.CoronaRuntimeTaskDispatcher m_dispatcher = null;
	
	public CallbacksGetControllerName(com.naef.jnlua.LuaState luaState) {
		
		setupCallbackInvoke(luaState);
		setupCallbackPlayerNum(luaState);
		
		// Set up a dispatcher which allows us to send a task to the Corona runtime thread from another thread.
		m_dispatcher = new com.ansca.corona.CoronaRuntimeTaskDispatcher(luaState);
	}
	
	private void setupCallbackInvoke(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexInvoke, com.naef.jnlua.LuaType.FUNCTION);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		luaState.pushValue(m_luaStackIndexInvoke);
		m_luaReferenceKeyInvoke = luaState.ref(com.naef.jnlua.LuaState.REGISTRYINDEX);
	}
	
	private void setupCallbackPlayerNum(com.naef.jnlua.LuaState luaState) {
		// Check if the first argument is a function.
		// Will print a stack trace if not or if no argument was given.
		try {
			luaState.checkType(m_luaStackIndexPlayerNum, com.naef.jnlua.LuaType.NUMBER);
		}
		catch (Exception ex) {
			ex.printStackTrace();
			return;
		}
		
		m_luaPlayerNum = luaState.toInteger(m_luaStackIndexPlayerNum);
	}
	
	public void invoke() {
		
		if (m_luaPlayerNum < 0) {
	    	Log.e(TAG, "Failed with invalid playerId="+m_luaPlayerNum);
	    	return;
	    }
		
		OuyaController controller = OuyaController.getControllerByPlayer(m_luaPlayerNum);
		if (null == controller) {
			Log.e(TAG, "Failed to get controlller with playerId="+m_luaPlayerNum);
	    	return;
		}
	    
		final String name = controller.getDeviceName();
		//Log.i(TAG, "invoke playerNum=" + m_luaPlayerNum + " name="+name);
		
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
							luaState.rawGet(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyInvoke);
							
							// Remove the Lua function from the registry.
							luaState.unref(com.naef.jnlua.LuaState.REGISTRYINDEX, m_luaReferenceKeyInvoke);
							
							// pass as argument
							luaState.pushInteger(m_luaPlayerNum);
							
							// pass as argument
							luaState.pushString(name);
							
							// call the method and pass the jsonData
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
