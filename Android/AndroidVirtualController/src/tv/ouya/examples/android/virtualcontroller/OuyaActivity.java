package tv.ouya.examples.android.virtualcontroller;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Vector;

import android.os.Build;
import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;
import android.util.SparseArray;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.MotionEvent.PointerCoords;
import android.view.MotionEvent.PointerProperties;
import tv.ouya.console.api.OuyaController;
import tv.ouya.examples.android.virtualcontroller.MappingParser.AxisRemap;
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
		
		OuyaController.init(this);
		
		Log.i(TAG, "****************");
		Log.i(TAG, "****************");
		Log.i(TAG, "****START*******");
		Log.i(TAG, "****************");
		Log.i(TAG, "****************");
		
		for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index) {
			OuyaController controller = OuyaController.getControllerByPlayer(index);
			if (null == controller) {
				continue;
			}
			Log.i(TAG, "OUYA Controller #" + index + ": " + controller.getDevice().getName());
		}
		
		mParser.parse(json);
	
		takeKeyEvents(true);
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		return super.onKeyUp(keyCode, keyEvent);
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		return super.onKeyDown(keyCode, keyEvent);
	}
	
	private void passDispatchGenericMotionEvent(MotionEvent motionEvent) {
		super.dispatchGenericMotionEvent(motionEvent);
	}
	
	private void passDispatchKeyEvent(KeyEvent keyEvent) {
		//super.dispatchKeyEvent(keyEvent);
		int keyCode = keyEvent.getKeyCode();
		switch (keyEvent.getAction()) {
			case KeyEvent.ACTION_DOWN:
				onKeyDown(keyCode, keyEvent);
				break;
			case KeyEvent.ACTION_UP:
				onKeyUp(keyCode, keyEvent);
				break;
		}
	}

	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		
		//Log.i(TAG, android.os.Build.MODEL + " : dispatchGenericMotionEvent");
		//DebugLogging.debugMotionEvent(motionEvent);
		
		/*
		if (true) {
			return true;
		}
		*/
	    
	    Vector<ButtonIsAxis> buttons =
				mParser.getButtonIsAxis(Build.MODEL, motionEvent.getDevice().getName());
	    
	    if (null != buttons) {		
			for (int i = 0; i < buttons.size(); ++i) {
				ButtonIsAxis button = buttons.get(i);	    			
				float axis = motionEvent.getAxisValue(button.mSourceAxis);
				if (axis >= button.mActionDownMin &&
					axis <= button.mActionDownMax) {
					if (!mLastValue.containsKey(button.mDestinationKeyCode) ||
						!mLastValue.get(button.mDestinationKeyCode)) {					
						mLastValue.put(button.mDestinationKeyCode, true);
						//Log.i(TAG, "Injected ACTION_DOWN for " + button.mDestinationKeyCode);
						long downTime = 0;
						long eventTime = 0;
						int action = KeyEvent.ACTION_DOWN;
						int code = button.mDestinationKeyCode;
						int repeat = 0;
						int metaState = 0;
						int deviceId = motionEvent.getDeviceId();
						int scancode = 0;
						KeyEvent keyEvent = new KeyEvent(downTime, eventTime, action, code, repeat, metaState, deviceId, scancode);
						passDispatchKeyEvent(keyEvent);
					}
				} else {
					if (mLastValue.containsKey(button.mDestinationKeyCode) &&
						mLastValue.get(button.mDestinationKeyCode)) {					
						mLastValue.put(button.mDestinationKeyCode, false);
						//Log.i(TAG, "Injected ACTION_UP for " + button.mDestinationKeyCode);
						long downTime = 0;
						long eventTime = 0;
						int action = KeyEvent.ACTION_UP;
						int code = button.mDestinationKeyCode;
						int repeat = 0;
						int metaState = 0;
						int deviceId = motionEvent.getDeviceId();
						int scancode = 0;
						KeyEvent keyEvent = new KeyEvent(downTime, eventTime, action, code, repeat, metaState, deviceId, scancode);
						passDispatchKeyEvent(keyEvent);
					}
				}
			}
	    }
		
	    Vector<AxisRemap> axises =
	    	mParser.getAxisRemap(android.os.Build.MODEL, motionEvent.getDevice().getName());
	    
	    if (null != axises) {	    
		    int pointerCount = motionEvent.getPointerCount();
		    if (pointerCount > 0 &&
		    	axises.size() > 0) {
		    	
		    	PointerProperties[] pointerProperties = new PointerProperties[1];
		    	pointerProperties[0] = new PointerProperties();
		    	PointerCoords[] pointerCoords = new PointerCoords[1];
		    	pointerCoords[0] = new PointerCoords();
		    	
	    		motionEvent.getPointerProperties(0, pointerProperties[0]);
	    		motionEvent.getPointerCoords(0, pointerCoords[0]);
	    		
			    for (int i = 0; i < axises.size(); ++i) {
			    	AxisRemap axis = axises.get(i);
			    	float val = motionEvent.getAxisValue(axis.mSourceAxis);
			    	Log.i(TAG, "Remap "+DebugLogging.debugGetAxisName(axis.mSourceAxis) +
			    			" to "+ DebugLogging.debugGetAxisName(axis.mDestinationAxis) + "val=" + val);
		    		pointerCoords[0].setAxisValue(axis.mDestinationAxis, val);		    	
			    }
			    
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
		    	//debugMotionEvent(motionEvent);
		    	//debugOuyaMotionEvent(motionEvent);
		    	passDispatchGenericMotionEvent(motionEvent);
		    	motionEvent.recycle();
		    	return true;
		    }
	    }
		
	    //DebugLogging.debugMotionEvent(motionEvent);
	    //DebugLogging.debugOuyaMotionEvent(motionEvent);
	    passDispatchGenericMotionEvent(motionEvent);
	    return true;
	}
	
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		int keyCode = keyEvent.getKeyCode();
		int source = keyEvent.getSource();
		Log.i(TAG, "model=" + android.os.Build.MODEL);
		Log.i(TAG, "device=" + keyEvent.getDevice().getName());
		Log.i(TAG, "dispatchKeyEvent="+keyCode + " action="+keyEvent.getAction());
		
		/*
		if (true) {
			return true;
		}
		*/
		
		MappingParser.Button button =
				mParser.getButton(android.os.Build.MODEL, keyEvent.getDevice().getName(), keyCode);
		if (null == button) {
			return true;
		}

		if (button.mExcludeSource.contains(source)) {
			return true;
		}
		//Log.i(TAG, "destinationKeyCode="+button.mDestinationKeyCode);
		long downTime = keyEvent.getDownTime();
		long eventTime = keyEvent.getEventTime();
		int action = keyEvent.getAction();
		int code = button.mDestinationKeyCode;
		int repeat = keyEvent.getRepeatCount();
		int metaState = keyEvent.getMetaState();
		int deviceId = keyEvent.getDeviceId();
		int scancode = 0;
		KeyEvent newKeyEvent = new KeyEvent(downTime, eventTime, action, code, repeat, metaState, deviceId, scancode);
		passDispatchKeyEvent(newKeyEvent);
        return true;
	}	
	
}
