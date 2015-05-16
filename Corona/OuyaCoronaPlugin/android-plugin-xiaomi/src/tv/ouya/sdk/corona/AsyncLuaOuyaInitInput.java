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

import com.ansca.corona.input.ViewInputHandler;

import java.lang.reflect.Field;
import java.lang.reflect.Method;


import android.app.Activity;
import android.util.Log;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.LinearLayout;


/**
 * Implements the asyncCallLuaFunction() function in Lua.
 * <p>
 * Demonstrates how to fetch a "Lua function argument" and then call that Lua function from another thread.
 * <p>
 * You must never manipulate the Lua state from another thread or else race conditions and exceptions that
 * can crash the application can occur. This example demonstrates how to do this in a thread safe manner.
 */
public class AsyncLuaOuyaInitInput implements com.naef.jnlua.NamedJavaFunction {
	
	public static final String TAG = "AsyncLuaOuyaInitInput";
	
	// Custom input view detects input to send axis and button events to Lua
	private CoronaOuyaInputView mInputView = null;
	
	/**
	 * Gets the name of the Lua function as it would appear in the Lua script.
	 * @return Returns the name of the custom Lua function.
	 */
	@Override
	public String getName() {
		return "asyncLuaOuyaInitInput";
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
		
		CallbacksOuyaInput callbacks = new CallbacksOuyaInput(luaState);
		
		// store for access
		IOuyaActivity.SetCallbacksOuyaInput(callbacks);
		
		// disable built-in input
		disableBuiltInCoronaInput();
		
		// add Corona OUYA Input View to layout
		addCoronaOuyaInputView();
		
		// Return 0 since this Lua function does not return any values.
		return 0;
	}
	
	private void disableBuiltInCoronaInput() {
		
		Activity activity = com.ansca.corona.CoronaEnvironment.getCoronaActivity();
		
		//disable standard Corona input
		Field[] fs = com.ansca.corona.CoronaActivity.class.getDeclaredFields();
		for (Field field : fs) {
			if (null != field) {
				//Log.i(TAG, "Field="+field.getClass().getName());
				field.setAccessible(true);
				try {
					//Log.i(TAG, "Field name="+field.getName()+ " type="+field.getType().getName());
					
					if (field.getType().getName().equals("com.ansca.corona.input.ViewInputHandler")) {
						//Log.i(TAG, "Found ViewInputHandler");
						field.setAccessible(true);
						
						//Log.i(TAG, "Get ViewInputHandler");
						ViewInputHandler view = (ViewInputHandler)field.get(activity);
						if (null != view) {
							//Log.i(TAG, "Got the view handler");
							unsubscribeViewInputHandler(view);
						} else {
							Log.w(TAG, "Didn't get the view handler");
						}
					}
				} catch (java.lang.Exception ex) {
					ex.printStackTrace();
				}
			}
		}
	}
	
	private void unsubscribeEventHandler(Object eventHandler) {
		//Log.i(TAG, "unsubscribeEventHandler");
		if (null == eventHandler) {
			return;
		}
	
		try {
			Method unsubscribe = eventHandler.getClass().getMethod("unsubscribe");
			if (null != unsubscribe) {
				//Log.i(TAG, "Found unsubscribe");
				unsubscribe.invoke(eventHandler, new Object[0]);
			} else {
				Log.w(TAG, "Failed to find unsubscribe method");
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}
	}
	
	private void unsubscribeViewInputHandler(ViewInputHandler inputHandler) {
		//Log.i(TAG, "unsubscribeViewInputHandler");
		if (null == inputHandler) {
			return;
		}
	
		Field[] fs = ViewInputHandler.class.getDeclaredFields();
		for (Field field : fs) {
			if (null != field) {
				//Log.i(TAG, "Field="+field.getClass().getName());
				field.setAccessible(true);
				try {
					//Log.i(TAG, "Field name="+field.getName()+ " type="+field.getType().getName());
					if (field.getType().getName().equals("com.ansca.corona.input.ViewInputHandler$EventHandler")) {
						Object handler = field.get(inputHandler);
						if (null != handler) {
							//Log.i(TAG, "Found event handler");
							unsubscribeEventHandler(handler);
						} else {
							Log.w(TAG, "Failed to get event handler");
						}
					}
				} catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		}
	}
	
	// find the main frame layout from the layout content
	private FrameLayout getFrameLayout(final Activity activity) {
		if (null != activity.getWindow() &&
        	null != activity.getWindow().getDecorView()) {
	        View view = activity.getWindow().getDecorView().findViewById(android.R.id.content);
	        if (null != view) {
	        	if (view instanceof android.widget.FrameLayout) {
	        		return (FrameLayout)view;
	        	}
	        }
        }
		return null;
	}
	
	private void addCoronaOuyaInputView() {
		//Log.i(TAG, "addCoronaOuyaInputView");
		
		final Activity activity = com.ansca.corona.CoronaEnvironment.getCoronaActivity();
		
		activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				// *** We are now running in the main UI thread. ***
				try {
					//Log.i(TAG, "Construct CoronaOuyaInputView");
					if (null == mInputView) {
						mInputView = new CoronaOuyaInputView(activity.getApplicationContext());
						
						mInputView.setLayoutParams(new LinearLayout.LayoutParams(
                            LinearLayout.LayoutParams.MATCH_PARENT,
                            LinearLayout.LayoutParams.MATCH_PARENT));
					
    					//Log.i(TAG, "Find frame layout");
    					FrameLayout fm = getFrameLayout(activity);
    					if (null != fm) {
    						//Log.i(TAG, "Add OuyaInputView to frame layout");
    						fm.addView(mInputView);

    						//Log.i(TAG, "Disable screensaver");
    						mInputView.setKeepScreenOn(true);
    						
    						//Log.i(TAG, "Request focus for OuyaInputView");
    						mInputView.requestFocus();
    					} else {
    						Log.w(TAG, "Filed to find frame layout");
    					}
					}
				}
				catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		});
	}
}
