package tv.ouya.examples.corona.inapppurchases;

import plugin.ouya.LuaLoader;


/**
 * Extends the Application class to receive Corona runtime events and to extend the Lua API.
 * <p>
 * Only one instance of this class will be created by the Android OS. It will be created before this application's
 * activity is displayed and will persist after the activity is destroyed. The name of this class must be set in the
 * AndroidManifest.xml file's "application" tag or else an instance of this class will not be created on startup.
 */
public class CoronaApplication extends android.app.Application {
	
	//private LuaLoader luaLoader = null;
	
	/** Called when your application has started. */
	@Override
	public void onCreate() {
		// Set up a Corona runtime listener used to add custom APIs to Lua.
		super.onCreate();
		
		//luaLoader = new LuaLoader();	
	}
}