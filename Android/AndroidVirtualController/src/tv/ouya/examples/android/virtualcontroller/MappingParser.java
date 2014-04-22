package tv.ouya.examples.android.virtualcontroller;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map.Entry;
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
import tv.ouya.examples.android.virtualcontroller.MappingParser.Alias;

public class MappingParser {
	
	private static final String TAG = MappingParser.class.getSimpleName();
	
	protected class Alias
	{
		public String mName = "";
		public String mFriendlyName = "";
		public Boolean mFallback = false;
	}
	protected class Button
	{
		public int mSourceKeyCode = 0;
		public int mDestinationKeyCode = 0;
		public Vector<Integer> mExcludeSource = new Vector<Integer>();
	}
	protected class AxisRemap
	{
		public int mSourceAxis = 0;
		public int mDestinationAxis = 0;
	}
	protected class ButtonIsAxis
	{
		public int mSourceAxis = 0;
		public float mActionDownMax = 0;
		public float mActionDownMin = 0;
		public int mDestinationKeyCode = 0;
	}
	private class Controller
	{
		public HashMap<String, Alias> mAlias = new HashMap<String, Alias>();
		public Vector<AxisRemap> mAxisRemap = new Vector<AxisRemap>();
		public HashMap<Integer, Button> mButton = new HashMap<Integer, Button>();
		public Vector<ButtonIsAxis> mButtonIsAxis = new Vector<ButtonIsAxis>();
	}
	private class Device
	{		
		public HashMap<String, Alias> mAlias = new HashMap<String, Alias>();
		public HashMap<String, Controller> mController = new HashMap<String, Controller>();
		public Controller mControllerFallback = null;
	}
	
	private HashMap<String, Device> mDevice = new HashMap<String, Device>();
	private Device mDeviceFallback = null;
	private Device getDevice(String deviceName) {
		if (mDevice.containsKey(deviceName)) {
			return mDevice.get(deviceName);
		} else {
			if (null != mDeviceFallback) {
				Iterator<Entry<String, Alias>> it = mDeviceFallback.mAlias.entrySet().iterator();
				while (it.hasNext()) {
					Log.i(TAG, "Using device fallback="+it.next().getKey());
				}
			}
			return mDeviceFallback;
		}
	}
	private Controller getController(Device device, String controllerName) {
		if (null == device) {
			return null;
		}
		if (!device.mController.containsKey(controllerName)) {
			if (null != device.mControllerFallback) {
				Iterator<Entry<String, Alias>> it = device.mControllerFallback.mAlias.entrySet().iterator();
				while (it.hasNext()) {
					Log.i(TAG, "Using controller fallback="+it.next().getKey());
				}
			}
			return device.mControllerFallback;
		}
		return device.mController.get(controllerName);
	}
	public Button getButton(String deviceName, String controllerName, int keyCode) {
		Device device = getDevice(deviceName);
		if (null == device) {
			return null;
		}
		Controller controller = getController(device, controllerName);
		if (null == controller) {
			return null;
		}
		if (!controller.mButton.containsKey(keyCode)) {
			return null;
		}
		return controller.mButton.get(keyCode);
	}
	public Vector<AxisRemap> getAxisRemap(String deviceName, String controllerName) {
		Device device = getDevice(deviceName);
		if (null == device) {
			return null;
		}
		Controller controller = getController(device, controllerName);
		if (null == controller) {
			return null;
		}
		return controller.mAxisRemap;
	}
	public Vector<ButtonIsAxis> getButtonIsAxis(String deviceName, String controllerName) {
		Device device = getDevice(deviceName);
		if (null == device) {
			return null;
		}
		Controller controller = getController(device, controllerName);
		if (null == controller) {
			return null;
		}
		return controller.mButtonIsAxis;
	}
	
	private int getKeyCode(String name) {
		if (name.equals("BUTTON_O")) {
			return OuyaController.BUTTON_O;
		} else if (name.equals("BUTTON_U")) {
			return OuyaController.BUTTON_U;
		} else if (name.equals("BUTTON_Y")) {
			return OuyaController.BUTTON_Y;
		} else if (name.equals("BUTTON_A")) {
			return OuyaController.BUTTON_A;
		} else if (name.equals("BUTTON_L1")) {
			return OuyaController.BUTTON_L1;
		} else if (name.equals("BUTTON_R1")) {
			return OuyaController.BUTTON_R1;
		} else if (name.equals("BUTTON_L3")) {
			return OuyaController.BUTTON_L3;
		} else if (name.equals("BUTTON_R3")) {
			return OuyaController.BUTTON_R3;
		} else if (name.equals("BUTTON_DPAD_UP")) {
			return OuyaController.BUTTON_DPAD_UP;
		} else if (name.equals("BUTTON_DPAD_DOWN")) {
			return OuyaController.BUTTON_DPAD_DOWN;
		} else if (name.equals("BUTTON_DPAD_RIGHT")) {
			return OuyaController.BUTTON_DPAD_RIGHT;
		} else if (name.equals("BUTTON_DPAD_LEFT")) {
			return OuyaController.BUTTON_DPAD_LEFT;
		} else {
			return 0;
		}
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
					JSONObject objAlias = deviceAlias.getJSONObject(aliasId);
					//Log.i(TAG, "device alias="+objAlias.toString());
					Alias alias = new Alias();
					alias.mName = objAlias.getString("name");
					Log.i(TAG, "device alias name="+alias.mName);
					alias.mFriendlyName = objAlias.getString("friendly_name");
					Log.i(TAG, "device alias friendly_name="+alias.mFriendlyName);
					if (objAlias.has("fallback")) {
						alias.mFallback = objAlias.getBoolean("fallback");
						Log.i(TAG, "device alias fallback="+alias.mFallback);
					}
					mappingDevice.mAlias.put(alias.mName, alias);
					mDevice.put(alias.mName, mappingDevice);
					if (alias.mFallback) {
						mDeviceFallback = mappingDevice;
					}
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
						JSONObject objAlias = controllerAlias.getJSONObject(aliasId);
						//Log.i(TAG, "controller alias="+objAlias.toString());
						Alias alias = new Alias();
						alias.mName = objAlias.getString("name");
						Log.i(TAG, "controller alias name="+alias.mName);
						alias.mFriendlyName = objAlias.getString("friendly_name");
						Log.i(TAG, "controller alias friendly_name="+alias.mFriendlyName);
						if (objAlias.has("fallback")) {
							alias.mFallback = objAlias.getBoolean("fallback");
							Log.i(TAG, "controller alias fallback="+alias.mFallback);
						}
						mappingController.mAlias.put(alias.mName, alias);
						mappingDevice.mController.put(alias.mName, mappingController);
						if (alias.mFallback) {
							mappingDevice.mControllerFallback = mappingController;
						}
					}
					if (objController.has("axis_remap")) {
						JSONArray axis = objController.getJSONArray("axis_remap");
						//Log.i(TAG, "axisRemap="+axis.toString());
						for (int axisId=0; axisId < axis.length(); ++axisId) {
							
							AxisRemap mappingAxis = new AxisRemap();
							
							JSONObject objAxis = axis.getJSONObject(axisId);
							//Log.i(TAG, "objAxis="+objAxis);
							mappingAxis.mSourceAxis = objAxis.getInt("source_axis");
							//Log.i(TAG, "SourceAxis="+mappingAxis.mSourceAxis);
							mappingAxis.mDestinationAxis = objAxis.getInt("destination_axis");
							//Log.i(TAG, "destinationAxis="+mappingAxis.mDestinationAxis);
							
							mappingController.mAxisRemap.add(mappingAxis);
						}
					}
					if (objController.has("button_is_axis")) {
						JSONArray button = objController.getJSONArray("button_is_axis");
						//Log.i(TAG, "buttonIsAxis="+button.toString());
						for (int buttonId=0; buttonId < button.length(); ++buttonId) {
							
							ButtonIsAxis mappingButton = new ButtonIsAxis();
							
							JSONObject objButton = button.getJSONObject(buttonId);
							//Log.i(TAG, "objButton="+objButton);
							mappingButton.mSourceAxis = objButton.getInt("source_axis");
							//Log.i(TAG, "sourceAxis="+sourceAxis);
							mappingButton.mActionDownMax = (float)objButton.getDouble("action_down_max");
							//Log.i(TAG, "actionDownMax="+actionDownMax
							mappingButton.mActionDownMin = (float)objButton.getDouble("action_down_min");
							//Log.i(TAG, "actionDownMin="+actionDownMin);
							int destinationKeyCode = objButton.getInt("destination_keycode"); 
							//Log.i(TAG, "destinationKeyCode="+destinationKeyCode);
							mappingButton.mDestinationKeyCode = destinationKeyCode;
							
							mappingController.mButtonIsAxis.add(mappingButton);
						}
					}
					if (objController.has("button")) {
						JSONArray button = objController.getJSONArray("button");
						//Log.i(TAG, "buttons="+button.toString());
						for (int buttonId=0; buttonId < button.length(); ++buttonId) {
							
							Button mappingButton = new Button();
							
							JSONObject objButton = button.getJSONObject(buttonId);
							//Log.i(TAG, "objButton="+objButton);
							mappingButton.mSourceKeyCode = objButton.getInt("source_keycode"); 
							//Log.i(TAG, "sourceKeycode="+mappingButton.mSourceKeyCode);
							int destination_keycode = objButton.getInt("destination_keycode"); 
							//Log.i(TAG, "destination_keycode="+destination_keycode);							
							//Log.i(TAG, "sourceKeycode="+mappingButton.mSourceKeyCode+" destination_keycode="+destination_keycode);
							
							if (objButton.has("exclude_source")) {
								JSONArray excludeSource = objButton.getJSONArray("exclude_source");
								for (int excludeId=0; excludeId < excludeSource.length(); ++excludeId) {
									int source = excludeSource.getInt(excludeId);
									mappingButton.mExcludeSource.add(source);
									//Log.i(TAG, "exclude_source="+source);									
								}
							}

							mappingButton.mDestinationKeyCode = destination_keycode;
							
							mappingController.mButton.put(mappingButton.mSourceKeyCode, mappingButton);
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