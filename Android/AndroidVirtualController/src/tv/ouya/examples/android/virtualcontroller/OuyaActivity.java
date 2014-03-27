package tv.ouya.examples.android.virtualcontroller;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;

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

public class OuyaActivity extends Activity {
	
	private static final String TAG = OuyaActivity.class.getSimpleName();
	
	private HashMap<Integer, Boolean> mLastValue = new HashMap<Integer, Boolean>();

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		Context context = getApplicationContext();
		String json = "";
		
		try {
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("input.json", AssetManager.ACCESS_BUFFER);
			byte[] configurationBytes = new byte[inputStream.available()];
			inputStream.read(configurationBytes);
			inputStream.close();
			
			json = new String(configurationBytes); 
			
			Log.i(TAG, "Loaded input configuration");
		} catch (IOException e) {
			Log.e(TAG, "Failed to load input configuration");
		}
		
		//Log.i(TAG, "JSON: " + json);
		
		try {
			JSONObject jObject = new JSONObject(json);
			Log.i(TAG, "Loaded input json");
			
			JSONArray map = jObject.getJSONArray("map");
			for (int deviceId=0; deviceId < map.length(); ++deviceId) {
				JSONObject objDevice = map.getJSONObject(deviceId);
				//Log.i(TAG, "device="+objDevice.toString());
				JSONObject device = objDevice.getJSONObject("device");
				JSONArray deviceAlias = device.getJSONArray("alias");
				//Log.i(TAG, "alias="+alias.toString());
				for (int aliasId=0; aliasId < deviceAlias.length(); ++aliasId) {
					Log.i(TAG, "device alias="+deviceAlias.getString(aliasId));					
				}
				JSONArray controller = device.getJSONArray("controller");
				//Log.i(TAG, "controller="+controller.toString());
				for (int controllerId=0; controllerId < controller.length(); ++controllerId) {
					JSONObject objController = controller.getJSONObject(controllerId);
					//Log.i(TAG, "objController="+objController.toString());
					JSONArray controllerAlias = objController.getJSONArray("alias");
					//Log.i(TAG, "controllerAlias="+controllerAlias.toString());
					for (int aliasId=0; aliasId < controllerAlias.length(); ++aliasId) {
						Log.i(TAG, "controller alias="+controllerAlias.getString(aliasId));					
					}
					if (objController.has("button")) {
						JSONArray button = objController.getJSONArray("button");
						//Log.i(TAG, "buttons="+button.toString());
						for (int buttonId=0; buttonId < button.length(); ++buttonId) {
							JSONObject objButton = button.getJSONObject(buttonId);
							//Log.i(TAG, "objButton="+objButton);
							String sourceKeycode = objButton.getString("source_keycode"); 
							//Log.i(TAG, "sourceKeycode="+sourceKeycode);
							String destination_keycode = objButton.getString("destination_keycode"); 
							//Log.i(TAG, "destination_keycode="+destination_keycode);							
							Log.i(TAG, "sourceKeycode="+sourceKeycode+" destination_keycode="+destination_keycode);
						}
					}
				}
				/*
				if (objController.has("button")) {
					JSONObject objButton = objController.getJSONObject("button");
					Log.i(TAG, "objButton="+objButton.toString());
				}
				*/
				
				Log.i(TAG, "**********");
			}
		} catch (JSONException e) {
			Log.e(TAG, "Failed to load input json");
			e.printStackTrace();
		}
	
		takeKeyEvents(true);
	}

	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		
		float lsX = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
	    float lsY = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
	    float rsX = motionEvent.getAxisValue(OuyaController.AXIS_RS_X);
	    float rsY = motionEvent.getAxisValue(OuyaController.AXIS_RS_Y);
	    float l2 = motionEvent.getAxisValue(OuyaController.AXIS_L2);
	    float r2 = motionEvent.getAxisValue(OuyaController.AXIS_R2);
	    
	    if (android.os.Build.MODEL.equals("Nexus 10")) {
	    	if (motionEvent.getDevice().getName().equals("Broadcom Bluetooth HID")) { //OUYA Controller	    	
		    	int pointerCount = motionEvent.getPointerCount();
		    	PointerProperties[] pointerProperties = new PointerProperties[1];
		    	pointerProperties[0] = new PointerProperties();
		    	PointerCoords[] pointerCoords = new PointerCoords[1];
		    	pointerCoords[0] = new PointerCoords();
		    	if (pointerCount > 0) {
		    		motionEvent.getPointerProperties(0, pointerProperties[0]);
		    		motionEvent.getPointerCoords(0, pointerCoords[0]);
		    		pointerCoords[0].setAxisValue(OuyaController.AXIS_L2, rsX);
		    		pointerCoords[0].setAxisValue(OuyaController.AXIS_R2, rsY);
		    		pointerCoords[0].setAxisValue(OuyaController.AXIS_RS_X, motionEvent.getAxisValue(MotionEvent.AXIS_RX));
		    		pointerCoords[0].setAxisValue(OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(MotionEvent.AXIS_RY));
	
			    	long downTime = motionEvent.getDownTime();
			    	long eventTime = motionEvent.getEventTime();
			    	int action = motionEvent.getAction();
			    	int metaState = motionEvent.getMetaState();
			    	int buttonState = motionEvent.getButtonState();
			    	float xPrecision = 1;
			    	float yPrecision = 1;
			    	int deviceId = motionEvent.getDeviceId();
			    	int edgeFlags = motionEvent.getEdgeFlags();
			    	int source = motionEvent.getSource();
			    	int flags = motionEvent.getFlags();
			    	
			    	motionEvent = MotionEvent.obtain(downTime, eventTime, action,
			    			pointerCount, pointerProperties, pointerCoords,
			    			metaState, buttonState, xPrecision, yPrecision, deviceId, edgeFlags,
			    			source, flags);
			    	super.dispatchGenericMotionEvent(motionEvent);
			    	motionEvent.recycle();
			    	return true;
		    	}
	    	} else if (motionEvent.getDevice().getName().equals("Microsoft X-Box 360 pad")) {
				lsX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
				lsY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
				//Log.i(TAG, "dispatchGenericMotionEvent x="+lsX);
				//Log.i(TAG, "dispatchGenericMotionEvent y="+lsY);			
				if (lsX == -1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_LEFT));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
					}
				} else if (lsX == 1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_RIGHT));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
					}
				} else if (lsX == 0f)  {
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
					}
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
					}				
				}
				if (lsY == -1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_UP));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
					}
				} else if (lsY == 1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_DOWN));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
					}
				} else if (lsY == 0f)  {
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
					}
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
					}				
				}
	    	}
	    } else if (android.os.Build.MODEL.equals("OUYA Console")) {
			if (motionEvent.getDevice().getName().equals("GameStick Controller")) {			
				lsX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
				lsY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
				//Log.i(TAG, "dispatchGenericMotionEvent x="+lsX);
				//Log.i(TAG, "dispatchGenericMotionEvent y="+lsY);			
				if (lsX == -1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_LEFT));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
					}
				} else if (lsX == 1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_RIGHT));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
					}
				} else if (lsX == 0f)  {
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
					}
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
					}				
				}
				if (lsY == -1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_UP));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
					}
				} else if (lsY == 1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_DOWN));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
					}
				} else if (lsY == 0f)  {
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
					}
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
					}				
				}
				
	    	} else if (motionEvent.getDevice().getName().equals("Microsoft X-Box 360 pad")) {
				lsX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
				lsY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
				//Log.i(TAG, "dispatchGenericMotionEvent x="+lsX);
				//Log.i(TAG, "dispatchGenericMotionEvent y="+lsY);			
				if (lsX == -1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_LEFT));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
					}
				} else if (lsX == 1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_RIGHT));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
					}
				} else if (lsX == 0f)  {
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_LEFT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_LEFT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_LEFT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_LEFT));
					}
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_RIGHT) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_RIGHT)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_RIGHT, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_RIGHT));
					}				
				}
				if (lsY == -1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_UP));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
					}
				} else if (lsY == 1f) {
					if (!mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) ||
						!mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {					
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, true);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, OuyaController.BUTTON_DPAD_DOWN));
					}				
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
					}
				} else if (lsY == 0f)  {
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_UP) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_UP)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_UP, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_UP));
					}
					if (mLastValue.containsKey(OuyaController.BUTTON_DPAD_DOWN) &&
						mLastValue.get(OuyaController.BUTTON_DPAD_DOWN)) {
						mLastValue.put(OuyaController.BUTTON_DPAD_DOWN, false);
						super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, OuyaController.BUTTON_DPAD_DOWN));
					}				
				}
	    	}
	    }
		
		///*
		Log.i(TAG, "dispatchGenericMotionEvent lsX="+lsX);
		Log.i(TAG, "dispatchGenericMotionEvent lsY="+lsY);
		Log.i(TAG, "dispatchGenericMotionEvent rsX="+rsX);
		Log.i(TAG, "dispatchGenericMotionEvent rsY="+rsY);
		Log.i(TAG, "dispatchGenericMotionEvent l2="+l2);
		Log.i(TAG, "dispatchGenericMotionEvent r2="+r2);
		
		SparseArray<String> names = new SparseArray<String>();
        names.append(MotionEvent.AXIS_X, "AXIS_X");
        names.append(MotionEvent.AXIS_Y, "AXIS_Y");
        names.append(MotionEvent.AXIS_PRESSURE, "AXIS_PRESSURE");
        names.append(MotionEvent.AXIS_SIZE, "AXIS_SIZE");
        names.append(MotionEvent.AXIS_TOUCH_MAJOR, "AXIS_TOUCH_MAJOR");
        names.append(MotionEvent.AXIS_TOUCH_MINOR, "AXIS_TOUCH_MINOR");
        names.append(MotionEvent.AXIS_TOOL_MAJOR, "AXIS_TOOL_MAJOR");
        names.append(MotionEvent.AXIS_TOOL_MINOR, "AXIS_TOOL_MINOR");
        names.append(MotionEvent.AXIS_ORIENTATION, "AXIS_ORIENTATION");
        names.append(MotionEvent.AXIS_VSCROLL, "AXIS_VSCROLL");
        names.append(MotionEvent.AXIS_HSCROLL, "AXIS_HSCROLL");
        names.append(MotionEvent.AXIS_Z, "AXIS_Z");
        names.append(MotionEvent.AXIS_RX, "AXIS_RX");
        names.append(MotionEvent.AXIS_RY, "AXIS_RY");
        names.append(MotionEvent.AXIS_RZ, "AXIS_RZ");
        names.append(MotionEvent.AXIS_HAT_X, "AXIS_HAT_X");
        names.append(MotionEvent.AXIS_HAT_Y, "AXIS_HAT_Y");
        names.append(MotionEvent.AXIS_LTRIGGER, "AXIS_LTRIGGER");
        names.append(MotionEvent.AXIS_RTRIGGER, "AXIS_RTRIGGER");
        names.append(MotionEvent.AXIS_THROTTLE, "AXIS_THROTTLE");
        names.append(MotionEvent.AXIS_RUDDER, "AXIS_RUDDER");
        names.append(MotionEvent.AXIS_WHEEL, "AXIS_WHEEL");
        names.append(MotionEvent.AXIS_GAS, "AXIS_GAS");
        names.append(MotionEvent.AXIS_BRAKE, "AXIS_BRAKE");
        names.append(MotionEvent.AXIS_DISTANCE, "AXIS_DISTANCE");
        names.append(MotionEvent.AXIS_TILT, "AXIS_TILT");
        names.append(MotionEvent.AXIS_GENERIC_1, "AXIS_GENERIC_1");
        names.append(MotionEvent.AXIS_GENERIC_2, "AXIS_GENERIC_2");
        names.append(MotionEvent.AXIS_GENERIC_3, "AXIS_GENERIC_3");
        names.append(MotionEvent.AXIS_GENERIC_4, "AXIS_GENERIC_4");
        names.append(MotionEvent.AXIS_GENERIC_5, "AXIS_GENERIC_5");
        names.append(MotionEvent.AXIS_GENERIC_6, "AXIS_GENERIC_6");
        names.append(MotionEvent.AXIS_GENERIC_7, "AXIS_GENERIC_7");
        names.append(MotionEvent.AXIS_GENERIC_8, "AXIS_GENERIC_8");
        names.append(MotionEvent.AXIS_GENERIC_9, "AXIS_GENERIC_9");
        names.append(MotionEvent.AXIS_GENERIC_10, "AXIS_GENERIC_10");
        names.append(MotionEvent.AXIS_GENERIC_11, "AXIS_GENERIC_11");
        names.append(MotionEvent.AXIS_GENERIC_12, "AXIS_GENERIC_12");
        names.append(MotionEvent.AXIS_GENERIC_13, "AXIS_GENERIC_13");
        names.append(MotionEvent.AXIS_GENERIC_14, "AXIS_GENERIC_14");
        names.append(MotionEvent.AXIS_GENERIC_15, "AXIS_GENERIC_15");
        names.append(MotionEvent.AXIS_GENERIC_16, "AXIS_GENERIC_16");
        
        final int count = names.size();
        for (int i = 0; i < count; i++) {
        	float val = motionEvent.getAxisValue(names.keyAt(i));
            Log.i(TAG, names.valueAt(i) + "=" + val);
        }
		
		return super.dispatchGenericMotionEvent(motionEvent);
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		int keyCode = keyEvent.getKeyCode();
		Log.i(TAG, "dispatchKeyEvent="+keyCode);
		
		if (android.os.Build.MODEL.equals("Nexus 10")) {
			if (keyEvent.getDevice().getName().equals("GameStick Controller")) {
				switch (keyCode) {
				case 96:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_O));
					return true;
				case 97:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_U));
					return true;
				case 98:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_Y));
					return true;
				case 99:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_A));
					return true;
				case 100:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_L1));
					return true;
				case 101:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_R1));
					return true;
				case 102:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_L3));
					return true;
				case 103:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_R3));
					return true;
				case 104:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_DPAD_UP));
					return true;
				case 105:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_DPAD_DOWN));
					return true;
				case 108:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_DPAD_RIGHT));
					return true;
				case 109:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_DPAD_LEFT));
					return true;
				}
			} else if (keyEvent.getDevice().getName().equals("Microsoft X-Box 360 pad")) {
				switch (keyCode) {
				case 96:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_O));
					return true;
				case 97:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_A));
					return true;
				case 99:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_U));
					return true;
				case 100:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_Y));
					return true;
				case 102:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_L1));
					return true;
				case 103:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_R1));
					return true;
				case 106:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_L3));
					return true;
				case 107:
					super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), OuyaController.BUTTON_R3));
					return true;
				}
	    	}
	    } else if (android.os.Build.MODEL.equals("OUYA Console")) {
	        if (keyCode == OuyaController.BUTTON_L1 ||
	            keyCode == OuyaController.BUTTON_R1||
	            keyCode == OuyaController.BUTTON_O ||
	            keyCode == OuyaController.BUTTON_U ||
	            keyCode == OuyaController.BUTTON_Y ||
	            keyCode == OuyaController.BUTTON_A ||
	            keyCode == OuyaController.BUTTON_L3 ||
	            keyCode == OuyaController.BUTTON_R3) {
	            super.dispatchKeyEvent(keyEvent);
	            return true;
	        }
	
	        int source = keyEvent.getSource();
	        if (keyCode == OuyaController.BUTTON_DPAD_UP ||
	        	keyCode == OuyaController.BUTTON_DPAD_DOWN ||
	            keyCode == OuyaController.BUTTON_DPAD_LEFT ||
	            keyCode == OuyaController.BUTTON_DPAD_RIGHT) {
	            if (source != InputDevice.SOURCE_JOYSTICK) {
	            	super.dispatchKeyEvent(keyEvent);
	                return true;
	            }
	        }
	    }
		
        return true;
	}	
	
}
