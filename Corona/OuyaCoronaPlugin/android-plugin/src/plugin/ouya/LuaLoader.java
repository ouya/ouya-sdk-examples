package plugin.ouya;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.res.AssetManager;
import android.util.Log;
import com.ansca.corona.*;
import java.io.IOException;
import java.io.InputStream;
import tv.ouya.console.api.OuyaController;
import tv.ouya.sdk.corona.*;

public class LuaLoader implements com.naef.jnlua.JavaFunction {
	
	private final String LOG_TAG = LuaLoader.class.getSimpleName();
		
	com.naef.jnlua.NamedJavaFunction[] luaFunctions = null;
	
	private static LuaLoader.CoronaRuntimeEventHandler runtimeEventHandler = null;

	public LuaLoader() {
	
		if (null == runtimeEventHandler) {
			// OUYA activity has not been initialized
			
			Log.i(LOG_TAG, "Switching to OUYA SDK activity");
			final Activity activity = CoronaEnvironment.getCoronaActivity();
			Intent intent = new Intent(activity, CoronaOuyaActivity.class);
			activity.startActivity(intent);
			activity.finish();
			return;
		}
		
		// Set up a Corona runtime listener used to add custom APIs to Lua.
		if (null == runtimeEventHandler) { //only construct one
			runtimeEventHandler = new LuaLoader.CoronaRuntimeEventHandler();
		}
		com.ansca.corona.CoronaEnvironment.addRuntimeListener(runtimeEventHandler);		
	}

	/**
	 * Called when this plugin has been loaded by Lua's require() function.
	 * <p>
	 * Warning! This method is not called on the main UI thread.
	 */
	@Override
	public int invoke(com.naef.jnlua.LuaState luaState) {
		
		//Log.i(LOG_TAG, "invoke");

		// This is where you would register Lua functions into this object's Lua library.
		
		// the normal way to register named java functions
		//Log.i(LOG_TAG, "Register named functions for lua");
		
		// Add a module named "myTests" to Lua having the following functions.
		luaFunctions = new com.naef.jnlua.NamedJavaFunction[] {
			new AsyncLuaOuyaGetControllerName(),
			new AsyncLuaOuyaInitInput(),
			new AsyncLuaInitOuyaPlugin(),
			new AsyncLuaOuyaRequestGamerInfo(),
			new AsyncLuaOuyaRequestProducts(),
			new AsyncLuaOuyaRequestPurchase(),
			new AsyncLuaOuyaRequestReceipts(),
			new LuaOuyaGetDeviceHardwareName(),
			new LuaOuyaGetStringResource(),
		};
		luaState.register("ouyaSDK", luaFunctions);
		luaState.pop(1);
		
		initializeOUYA();
		
		//Log.i(LOG_TAG, "Named functions for lua registered.");
		//Log.i(LOG_TAG, "****\n*****\n****\n****\n****\n");

		return 1;
	}
	
	// instead register named java functions so we have access to the events
	
	/** Receives and handles Corona runtime events. */
	private class CoronaRuntimeEventHandler implements com.ansca.corona.CoronaRuntimeListener {

		private CoronaActivity.OnActivityResultHandler mOnActivityResultHandler = null;

		/**
		 * Called after the Corona runtime has been created and just before executing the "main.lua" file.
		 * This is the application's opportunity to register custom APIs into Lua.
		 * <p>
		 * Warning! This method is not called on the main thread.
		 * @param runtime Reference to the CoronaRuntime object that has just been loaded/initialized.
		 *                Provides a LuaState object that allows the application to extend the Lua API.
		 */
		@Override
		public void onLoaded(com.ansca.corona.CoronaRuntime runtime) {
			Log.i(LOG_TAG, "CoronaRuntimeEventHandler.onLoaded");
		}
		
		/**
		 * Called just after the Corona runtime has executed the "main.lua" file.
		 * <p>
		 * Warning! This method is not called on the main thread.
		 * @param runtime Reference to the CoronaRuntime object that has just been started.
		 */
		@Override
		public void onStarted(com.ansca.corona.CoronaRuntime runtime) {
			Log.i(LOG_TAG, "CoronaRuntimeEventHandler.onStarted");

			mOnActivityResultHandler = new CoronaActivity.OnActivityResultHandler() {
				@Override
				public void onHandleActivityResult(CoronaActivity activity, int requestCode, int resultCode, android.content.Intent data) {
					Log.i(LOG_TAG, "onHandleActivityResult");
					CoronaOuyaFacade coronaOuyaFacade = IOuyaActivity.GetCoronaOuyaFacade();
			    	if (null != coronaOuyaFacade)
					{
				    	// Forward this result to the facade, in case it is waiting for any activity results
				        if(coronaOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
				            return;
				        }
					}
				}
			};

			Log.i(LOG_TAG, "Get activity from Corona Environment...");
			CoronaActivity coronaActivity = CoronaEnvironment.getCoronaActivity();

			Log.i(LOG_TAG, "Registered Activity Result Handler");
			int result = coronaActivity.registerActivityResultHandler(mOnActivityResultHandler);

			Log.i(LOG_TAG, "Registered Activity Result Handler returned="+result);
		}
		
		/**
		 * Called just after the Corona runtime has been suspended which pauses all rendering, audio, timers,
		 * and other Corona related operations. This can happen when another Android activity (ie: window) has
		 * been displayed, when the screen has been powered off, or when the screen lock is shown.
		 * <p>
		 * Warning! This method is not called on the main thread.
		 * @param runtime Reference to the CoronaRuntime object that has just been suspended.
		 */
		@Override
		public void onSuspended(com.ansca.corona.CoronaRuntime runtime) {
			Log.i(LOG_TAG, "CoronaRuntimeEventHandler.onSuspended");
		}
		
		/**
		 * Called just after the Corona runtime has been resumed after a suspend.
		 * <p>
		 * Warning! This method is not called on the main thread.
		 * @param runtime Reference to the CoronaRuntime object that has just been resumed.
		 */
		@Override
		public void onResumed(com.ansca.corona.CoronaRuntime runtime) {
			Log.i(LOG_TAG, "CoronaRuntimeEventHandler.onResumed");
		}
		
		/**
		 * Called just before the Corona runtime terminates.
		 * <p>
		 * This happens when the Corona activity is being destroyed which happens when the user presses the Back button
		 * on the activity, when the native.requestExit() method is called in Lua, or when the activity's finish()
		 * method is called. This does not mean that the application is exiting.
		 * <p>
		 * Warning! This method is not called on the main thread.
		 * @param runtime Reference to the CoronaRuntime object that is being terminated.
		 */
		@Override
		public void onExiting(com.ansca.corona.CoronaRuntime runtime) {
			Log.i(LOG_TAG, "CoronaRuntimeEventHandler.onExiting");
		}
	}

	private void initializeOUYA() {
		
		Log.i(LOG_TAG, "Initializing OUYA...");
		
		Log.i(LOG_TAG, "Get application context...");
		Context context = com.ansca.corona.CoronaEnvironment.getApplicationContext();
		
		Log.i(LOG_TAG, "Load signing key...");
		// load the application key from assets
		try {
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);
			
			Log.i(LOG_TAG, "***Loaded signing key*********");
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		Log.i(LOG_TAG, "Initialize controller...");
		
		// Init the controller
		OuyaController.init(context);
			
		Log.i(LOG_TAG, "Get activity from Corona Environment...");
		final Activity activity = CoronaEnvironment.getCoronaActivity();
		
		//make activity accessible to Unity
		IOuyaActivity.SetActivity(activity);
	}
}