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
import tv.ouya.examples.android.virtualcontroller.MappingParser.ButtonIsAxis;

public class OuyaActivity extends Activity {
	
	private static final String TAG = OuyaActivity.class.getSimpleName();	
	
	private HashMap<Integer, Boolean> mLastValue = new HashMap<Integer, Boolean>();
	
	private MappingParser mParser = new MappingParser();

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
		
		mParser.parse(json);
	
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
	    
	    Vector<ButtonIsAxis> buttons =
				mParser.getButtonIsAxis(android.os.Build.MODEL, motionEvent.getDevice().getName());
		
		for (int i = 0; i < buttons.size(); ++i) {
			ButtonIsAxis button = buttons.get(i);	    			
			float axis = motionEvent.getAxisValue(button.mSourceAxis);
			if (axis == button.mActionDown) {
				if (!mLastValue.containsKey(button.mDestinationKeyCode) ||
					!mLastValue.get(button.mDestinationKeyCode)) {					
					mLastValue.put(button.mDestinationKeyCode, true);
					//Log.i(TAG, "Injected ACTION_DOWN for " + button.mDestinationKeyCode);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_DOWN, button.mDestinationKeyCode));
				}
			} else {
				if (mLastValue.containsKey(button.mDestinationKeyCode) &&
					mLastValue.get(button.mDestinationKeyCode)) {					
					mLastValue.put(button.mDestinationKeyCode, false);
					//Log.i(TAG, "Injected ACTION_UP for " + button.mDestinationKeyCode);
					super.dispatchKeyEvent(new KeyEvent(KeyEvent.ACTION_UP, button.mDestinationKeyCode));
				}
			}
		}
	    
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
		int source = keyEvent.getSource();
		Log.i(TAG, "dispatchKeyEvent="+keyCode);
		
		MappingParser.Button button =
				mParser.getButton(android.os.Build.MODEL, keyEvent.getDevice().getName(), keyCode);
		if (null == button) {
			return true;
		}

		if (button.mExcludeSource.contains(source)) {
			return true;
		}
		//Log.i(TAG, "destinationKeyCode="+button.mDestinationKeyCode);
		super.dispatchKeyEvent(new KeyEvent(keyEvent.getAction(), button.mDestinationKeyCode));
        return true;
	}	
	
}
