package tv.ouya.examples.android.virtualcontroller;

import android.hardware.input.InputManager;
import android.os.Build;
import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.MotionEvent.PointerCoords;
import android.view.MotionEvent.PointerProperties;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Vector;

import tv.ouya.console.api.OuyaController;
import tv.ouya.examples.android.virtualcontroller.MappingParser.AxisRemap;
import tv.ouya.examples.android.virtualcontroller.MappingParser.ButtonIsAxis;

public class OuyaActivity extends Activity implements InputManager.InputDeviceListener
{
	private static final String TAG = OuyaActivity.class.getSimpleName();
	
	private InputManager mInputManager;
	
	private List<HashMap<Integer, Boolean>> mLastValue = new ArrayList<HashMap<Integer, Boolean>>();
	
	private MappingParser mParser = new MappingParser();

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		OuyaController.init(this);
		
		mInputManager = (InputManager)getSystemService(Context.INPUT_SERVICE);
		if (null != mInputManager) {		
			mInputManager.registerInputDeviceListener(this, null);
		}
		
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
		
		for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index) {
			mLastValue.add(new HashMap<Integer, Boolean>());
		}
		
		Log.i(TAG, "****************");
		Log.i(TAG, "****************");
		Log.i(TAG, "DETECTED CONTROLLERS");
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
	protected void onDestroy() {
		if (null != mInputManager) {
			mInputManager.unregisterInputDeviceListener(this);
		}
		super.onDestroy();
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
	
	private int getPlayerNumByDeviceId(Integer deviceId) {
		//int playerId = OuyaController.getPlayerNumByDeviceId(deviceId);
		//return playerId;
		return 0;
	}

	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		
		//Log.i(TAG, android.os.Build.MODEL + " : dispatchGenericMotionEvent");
		DebugLogging.debugMotionEvent(motionEvent);
		
		int playerId = getPlayerNumByDeviceId(motionEvent.getDeviceId());
	    
	    if (playerId < 0) {
	    	Log.i(TAG, "Failed to find PlayerId for Controller="+motionEvent.getDevice().getName());
	    	return true;
	    }
		
	    Vector<ButtonIsAxis> buttons =
				mParser.getButtonIsAxis(mParser.getIdByString(Build.MODEL), mParser.getIdByString(motionEvent.getDevice().getName()));
	    
	    if (null != buttons) {		
			for (int i = 0; i < buttons.size(); ++i) {
				ButtonIsAxis button = buttons.get(i);	    			
				float axis = motionEvent.getAxisValue(button.mSourceAxis);
				if (axis >= button.mActionDownMin &&
					axis <= button.mActionDownMax) {
					if (!mLastValue.get(playerId).containsKey(button.mDestinationKeyCode) ||
						!mLastValue.get(playerId).get(button.mDestinationKeyCode)) {					
						mLastValue.get(playerId).put(button.mDestinationKeyCode, true);
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
					if (mLastValue.get(playerId).containsKey(button.mDestinationKeyCode) &&
						mLastValue.get(playerId).get(button.mDestinationKeyCode)) {					
						mLastValue.get(playerId).put(button.mDestinationKeyCode, false);
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
	    	mParser.getAxisRemap(mParser.getIdByString(android.os.Build.MODEL), mParser.getIdByString(motionEvent.getDevice().getName()));
	    
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
		
		int playerId = getPlayerNumByDeviceId(keyEvent.getDeviceId());
	    
	    if (playerId < 0) {
	    	Log.i(TAG, "Failed to find PlayerId for Controller="+keyEvent.getDevice().getName());
	    	return true;
	    }
	    
		int keyCode = keyEvent.getKeyCode();
		int source = keyEvent.getSource();
		Log.i(TAG, "model=" + android.os.Build.MODEL);
		Log.i(TAG, "device=" + keyEvent.getDevice().getName());
		Log.i(TAG, "dispatchKeyEvent="+keyCode + " action="+keyEvent.getAction());
		
		MappingParser.Button button =
				mParser.getButton(mParser.getIdByString(android.os.Build.MODEL), mParser.getIdByString(keyEvent.getDevice().getName()), keyCode);
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

	@Override
	public void onInputDeviceAdded(int deviceId) {
		Log.i(TAG, "onInputDeviceAdded="+deviceId);
	}

	@Override
	public void onInputDeviceChanged(int deviceId) {
		Log.i(TAG, "onInputDeviceChanged="+deviceId);
	}

	@Override	
	public void onInputDeviceRemoved(int deviceId) {
		Log.i(TAG, "onInputDeviceRemoved="+deviceId);
	}	
	
}
