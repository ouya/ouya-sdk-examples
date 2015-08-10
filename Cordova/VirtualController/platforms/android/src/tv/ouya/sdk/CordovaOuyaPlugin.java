package tv.ouya.sdk;

import android.util.Log;

import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaWebView;
import org.apache.cordova.PluginResult;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

/**
 * This class echoes a string called from JavaScript.
 */
public class CordovaOuyaPlugin extends CordovaPlugin {

    private static final String TAG = CordovaOuyaPlugin.class.getSimpleName();

    private static final boolean sEnableLogging = false;

    private static CordovaOuyaPlugin sInstance = null;

    // the callback context for communicating with HTML5
    private static CallbackContext sCallbackOnGenericMotionEvent = null;
    private static CallbackContext sCallbackOnKeyUp = null;
    private static CallbackContext sCallbackOnKeyDown = null;

    @Override
    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
        super.initialize(cordova, webView);
        if (sEnableLogging) {
            Log.i(TAG, "Initialize plugin");
        }
        sInstance = this;
    }

    @Override
    public boolean execute(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
        if (sEnableLogging) {
            Log.i(TAG, "********************* execute action=" + action);
        }
        if (action.equals("setCallbackOnGenericMotionEvent")) {
            sCallbackOnGenericMotionEvent = callbackContext;
            PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
            pluginResult.setKeepCallback(true);
            sCallbackOnGenericMotionEvent.sendPluginResult(pluginResult);
            return true;
        } else if (action.equals("setCallbackOnKeyUp")) {
            sCallbackOnKeyUp = callbackContext;
            PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
            pluginResult.setKeepCallback(true);
            sCallbackOnKeyUp.sendPluginResult(pluginResult);
            return true;
        } else if (action.equals("setCallbackOnKeyDown")) {
            sCallbackOnKeyDown = callbackContext;
            PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
            pluginResult.setKeepCallback(true);
            sCallbackOnKeyDown.sendPluginResult(pluginResult);
            return true;
        }
        return false;
    }

    public static void onGenericMotionEvent(final int playerNum, final int axis, final float val) {
        if (null == sCallbackOnGenericMotionEvent) {
            Log.e(TAG, "sCallbackOnGenericMotionEvent is null!");
            return;
        }
        if (null == sInstance) {
            Log.e(TAG, "sInstance is null!");
            return;
        }
        sInstance.cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("playerNum", playerNum);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("axis", axis);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("val", val);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, jsonObject.toString());
                pluginResult.setKeepCallback(true);
                sCallbackOnGenericMotionEvent.sendPluginResult(pluginResult); // Thread-safe.
            }
        });
    }

    public static void onKeyUp(final int playerNum, final int button) {
        if (null == sCallbackOnKeyUp) {
            Log.e(TAG, "sCallbackOnKeyUp is null!");
            return;
        }
        if (null == sInstance) {
            Log.e(TAG, "sInstance is null!");
            return;
        }
        sInstance.cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("playerNum", playerNum);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("button", button);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, jsonObject.toString());
                pluginResult.setKeepCallback(true);
                sCallbackOnKeyUp.sendPluginResult(pluginResult); // Thread-safe.
            }
        });
    }

    public static void onKeyDown(final int playerNum, final int button) {
        if (null == sCallbackOnKeyDown) {
            Log.e(TAG, "sCallbackOnKeyDown is null!");
            return;
        }
        if (null == sInstance) {
            Log.e(TAG, "sInstance is null!");
            return;
        }
        sInstance.cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("playerNum", playerNum);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("button", button);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, jsonObject.toString());
                pluginResult.setKeepCallback(true);
                sCallbackOnKeyDown.sendPluginResult(pluginResult); // Thread-safe.
            }
        });
    }
}
