package tv.ouya.examples.android.virtualcontroller;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.List;
import java.util.Vector;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;
import android.util.SparseArray;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.MotionEvent.PointerCoords;
import android.view.MotionEvent.PointerProperties;
import tv.ouya.console.api.OuyaController;

public class MappingParser {
	
	private static final String TAG = MappingParser.class.getSimpleName();
	
	private class Button
	{
		public int mSourceKeyCode = 0;
		public int mDestinationKeyCode = 0;
	}
	private class Controller
	{
		public Vector<String> mAlias = new Vector<String>();
		public HashMap<Integer, Button> mButton = new HashMap<Integer, Button>();					
	}
	private class Device
	{		
		public Vector<String> mAlias = new Vector<String>();
		public HashMap<String, Controller> mController = new HashMap<String, Controller>();
	}
	
	private HashMap<String, Device> mDevice = new HashMap<String, Device>();	
	private Device getDevice(String deviceName) {
		if (mDevice.containsKey(deviceName)) {
			return mDevice.get(deviceName);
		} else {
			return null;
		}
	}
	private Controller getController(Device device, String controllerName) {
		if (null == device) {
			return null;
		}
		if (!device.mController.containsKey(controllerName)) {
			return null;
		}
		return device.mController.get(controllerName);
	}
	public int getButton(String deviceName, String controllerName, int keyCode) {
		Device device = getDevice(deviceName);
		if (null == device) {
			return 0;
		}
		Controller controller = getController(device, controllerName);
		if (null == controller) {
			return 0;
		}
		if (!controller.mButton.containsKey(keyCode)) {
			return 0;
		}
		Button button = controller.mButton.get(keyCode);
		if (null == button) {
			return 0;
		}
		return button.mDestinationKeyCode;
	}
	
	public void parse(String jsonData) {
		
		//Log.i(TAG, "jsonData: " + jsonData);
		
		try {
			JSONObject jObject = new JSONObject(jsonData);
			Log.i(TAG, "Loaded input json");
			
			JSONArray map = jObject.getJSONArray("map");
			for (int deviceId=0; deviceId < map.length(); ++deviceId) {
				
				Device mappingDevice = new Device();
				
				JSONObject objDevice = map.getJSONObject(deviceId);
				//Log.i(TAG, "device="+objDevice.toString());
				JSONObject device = objDevice.getJSONObject("device");
				JSONArray deviceAlias = device.getJSONArray("alias");
				//Log.i(TAG, "alias="+alias.toString());
				for (int aliasId=0; aliasId < deviceAlias.length(); ++aliasId) {
					String strAlias = deviceAlias.getString(aliasId);
					Log.i(TAG, "device alias="+strAlias);
					mappingDevice.mAlias.add(strAlias);
					mDevice.put(strAlias, mappingDevice);
				}
				JSONArray controller = device.getJSONArray("controller");
				//Log.i(TAG, "controller="+controller.toString());
				for (int controllerId=0; controllerId < controller.length(); ++controllerId) {
					
					Controller mappingController = new Controller();
					
					JSONObject objController = controller.getJSONObject(controllerId);
					//Log.i(TAG, "objController="+objController.toString());
					JSONArray controllerAlias = objController.getJSONArray("alias");
					//Log.i(TAG, "controllerAlias="+controllerAlias.toString());
					for (int aliasId=0; aliasId < controllerAlias.length(); ++aliasId) {
						String strController = controllerAlias.getString(aliasId);
						Log.i(TAG, "controller alias="+strController);
						mappingController.mAlias.add(strController);
						mappingDevice.mController.put(strController, mappingController);
					}
					if (objController.has("button")) {
						JSONArray button = objController.getJSONArray("button");
						//Log.i(TAG, "buttons="+button.toString());
						for (int buttonId=0; buttonId < button.length(); ++buttonId) {
							
							Button mappingButton = new Button();
							
							JSONObject objButton = button.getJSONObject(buttonId);
							//Log.i(TAG, "objButton="+objButton);
							int sourceKeycode = objButton.getInt("source_keycode"); 
							//Log.i(TAG, "sourceKeycode="+sourceKeycode);
							String destination_keycode = objButton.getString("destination_keycode"); 
							//Log.i(TAG, "destination_keycode="+destination_keycode);							
							Log.i(TAG, "sourceKeycode="+sourceKeycode+" destination_keycode="+destination_keycode);
							
							mappingButton.mSourceKeyCode = sourceKeycode;
							//mappingButton.mDestinationKeyCode = destination_keycode;
							if (destination_keycode.equals("BUTTON_O")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_O;
							} else if (destination_keycode.equals("BUTTON_U")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_U;
							} else if (destination_keycode.equals("BUTTON_Y")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_Y;
							} else if (destination_keycode.equals("BUTTON_A")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_A;
							} else if (destination_keycode.equals("BUTTON_L1")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_L1;
							} else if (destination_keycode.equals("BUTTON_R1")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_R1;
							} else if (destination_keycode.equals("BUTTON_L3")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_L3;
							} else if (destination_keycode.equals("BUTTON_R3")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_R3;
							} else if (destination_keycode.equals("BUTTON_DPAD_UP")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_DPAD_UP;
							} else if (destination_keycode.equals("BUTTON_DPAD_DOWN")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_DPAD_DOWN;
							} else if (destination_keycode.equals("BUTTON_DPAD_RIGHT")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_DPAD_RIGHT;
							} else if (destination_keycode.equals("BUTTON_DPAD_LEFT")) {
								mappingButton.mDestinationKeyCode = OuyaController.BUTTON_DPAD_LEFT;
							}
							mappingController.mButton.put(sourceKeycode, mappingButton);
						}
					}
				}

				Log.i(TAG, "**********");
			}
		} catch (JSONException e) {
			Log.e(TAG, "Failed to load input json");
			e.printStackTrace();
		}
	}	
}