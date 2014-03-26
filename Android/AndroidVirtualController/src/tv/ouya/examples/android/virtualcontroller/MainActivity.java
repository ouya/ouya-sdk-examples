package tv.ouya.examples.android.virtualcontroller;

import java.util.HashMap;

import android.os.Bundle;
import android.app.Activity;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import tv.ouya.console.api.OuyaController;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final Float AXIS_SCALER = 4f;
	
	private TextView txtController = null;
	//private ImageView imgController = null;
	private ImageView imgButtonA = null;
	private ImageView imgDpadDown = null;
	private ImageView imgDpadLeft = null;
	private ImageView imgDpadRight = null;
	private ImageView imgDpadUp = null;
	private ImageView imgLeftStick = null;
	private ImageView imgLeftBumper = null;
	private ImageView imgLeftTrigger = null;
	private ImageView imgButtonO = null;
	private ImageView imgRightStick = null;
	private ImageView imgRightBumper = null;
	private ImageView imgRightTrigger = null;
	private ImageView imgLeftThumb = null;
	private ImageView imgRightThumb = null;
	private ImageView imgButtonU = null;
	private ImageView imgButtonY = null;
	
	private HashMap<Integer, Boolean> mLastValue = new HashMap<Integer, Boolean>();

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		txtController = (TextView)findViewById(R.id.txtController);
		//imgController = (ImageView)findViewById(R.id.imgController);
		imgButtonA = (ImageView)findViewById(R.id.imgButtonA);
		imgDpadDown = (ImageView)findViewById(R.id.imgDpadDown);
		imgDpadLeft = (ImageView)findViewById(R.id.imgDpadLeft);
		imgDpadRight = (ImageView)findViewById(R.id.imgDpadRight);
		imgDpadUp = (ImageView)findViewById(R.id.imgDpadUp);
		imgLeftStick = (ImageView)findViewById(R.id.imgLeftStick);
		imgLeftBumper = (ImageView)findViewById(R.id.imgLeftBumper);
		imgLeftTrigger = (ImageView)findViewById(R.id.imgLeftTrigger);
		imgButtonO = (ImageView)findViewById(R.id.imgButtonO);
		imgRightStick = (ImageView)findViewById(R.id.imgRightStick);
		imgRightBumper = (ImageView)findViewById(R.id.imgRightBumper);
		imgRightTrigger = (ImageView)findViewById(R.id.imgRightTrigger);
		imgLeftThumb = (ImageView)findViewById(R.id.imgLeftThumb);
		imgRightThumb = (ImageView)findViewById(R.id.imgRightThumb);
		imgButtonU = (ImageView)findViewById(R.id.imgButtonU);
		imgButtonY = (ImageView)findViewById(R.id.imgButtonY);
		
		takeKeyEvents(true);
		
		OuyaController.init(this);
        OuyaController.showCursor(false); //hide mouse
	}

	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		
		float lsX = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
	    float lsY = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
	    float rsX = motionEvent.getAxisValue(OuyaController.AXIS_RS_X);
	    float rsY = motionEvent.getAxisValue(OuyaController.AXIS_RS_Y);
	    float l2 = motionEvent.getAxisValue(OuyaController.AXIS_L2);
	    float r2 = motionEvent.getAxisValue(OuyaController.AXIS_R2);
		
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
			
		} else {
			lsX = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
		    lsY = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
		}
		
		/*
		Log.i(TAG, "dispatchGenericMotionEvent lsX="+lsX);
		Log.i(TAG, "dispatchGenericMotionEvent lsY="+lsY);
		Log.i(TAG, "dispatchGenericMotionEvent rsX="+rsX);
		Log.i(TAG, "dispatchGenericMotionEvent rsY="+rsY);
		Log.i(TAG, "dispatchGenericMotionEvent l2="+l2);
		Log.i(TAG, "dispatchGenericMotionEvent r2="+r2);
		*/
		
		// if remapping is needed
		//motionEvent.obtain(downTime, eventTime, action, x, y, pressure, size, metaState, xPrecision, yPrecision, deviceId, edgeFlags)
		
		return super.dispatchGenericMotionEvent(motionEvent);
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		
		//Log.i(TAG, "dispatchKeyEvent="+keyEvent.getKeyCode());
		
		int keyCode = keyEvent.getKeyCode();
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
		
        return true;
	}

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		txtController.setText(motionEvent.getDevice().getName());
		
		float lsX = motionEvent.getAxisValue(OuyaController.AXIS_LS_X);
	    float lsY = motionEvent.getAxisValue(OuyaController.AXIS_LS_Y);
	    float rsX = motionEvent.getAxisValue(OuyaController.AXIS_RS_X);
	    float rsY = motionEvent.getAxisValue(OuyaController.AXIS_RS_Y);
	    float l2 = motionEvent.getAxisValue(OuyaController.AXIS_L2);
	    float r2 = motionEvent.getAxisValue(OuyaController.AXIS_R2);
	    
	    //rotate input by N degrees to match image
        float degrees = 135;
        float radians = degrees / 180f * 3.14f;
        float cos = (float)Math.cos(radians);
        float sin = (float)Math.sin(radians);
	    
	    imgLeftStick.setX(AXIS_SCALER * (lsX * cos - lsY * sin));
	    imgLeftThumb.setY(AXIS_SCALER * (lsX * cos - lsY * sin));
	    
	    imgLeftStick.setY(AXIS_SCALER * (lsX * sin + lsY * cos));
	    imgLeftThumb.setY(AXIS_SCALER * (lsX * sin + lsY * cos));
	    
	    imgRightStick.setX(AXIS_SCALER * (rsX * cos - rsY * sin));
	    imgRightThumb.setX(AXIS_SCALER * (rsX * cos - rsY * sin));
	    
	    imgRightStick.setY(AXIS_SCALER * (rsX * sin + rsY * cos));
	    imgRightThumb.setY(AXIS_SCALER * (rsX * sin + rsY * cos));
		
		//Log.i(TAG, "Unrecognized GenericMotionEvent="+motionEvent.getAction());
	    
	    if (l2 > 0.25f) {
	    	imgLeftTrigger.setVisibility(View.VISIBLE);
	    } else {
	    	imgLeftTrigger.setVisibility(View.INVISIBLE);
	    }
	    
	    if (r2 > 0.25f) {
	    	imgRightTrigger.setVisibility(View.VISIBLE);
	    } else {
	    	imgRightTrigger.setVisibility(View.INVISIBLE);
	    }
	    
		return true;
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		txtController.setText(keyEvent.getDevice().getName());
		
		//Log.i(TAG, "KeyDown="+keyCode);
		
		switch (keyCode)
		{
		case OuyaController.BUTTON_L1:
			imgLeftBumper.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_L3:
			imgLeftStick.setVisibility(View.INVISIBLE);
			imgLeftThumb.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_R1:
			imgRightBumper.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_R3:
			imgRightStick.setVisibility(View.INVISIBLE);
			imgRightThumb.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_O:
			imgButtonO.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_U:
			imgButtonU.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_Y:
			imgButtonY.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_A:
			imgButtonA.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_DOWN:
			imgDpadDown.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_LEFT:
			imgDpadLeft.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_RIGHT:
			imgDpadRight.setVisibility(View.VISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_UP:
			imgDpadUp.setVisibility(View.VISIBLE);
			return true;
		}
		
		Log.i(TAG, "Unrecognized KeyDown="+keyCode);
		
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		
		txtController.setText(keyEvent.getDevice().getName());
		
		//Log.i(TAG, "KeyUp="+keyCode);
		
		switch (keyCode)
		{
		case OuyaController.BUTTON_L1:
			imgLeftBumper.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_L3:
			imgLeftStick.setVisibility(View.VISIBLE);
			imgLeftThumb.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_R1:
			imgRightBumper.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_R3:
			imgRightStick.setVisibility(View.VISIBLE);
			imgRightThumb.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_O:
			imgButtonO.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_U:
			imgButtonU.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_Y:
			imgButtonY.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_A:
			imgButtonA.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_DOWN:
			imgDpadDown.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_LEFT:
			imgDpadLeft.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_RIGHT:
			imgDpadRight.setVisibility(View.INVISIBLE);
			return true;
		case OuyaController.BUTTON_DPAD_UP:
			imgDpadUp.setVisibility(View.INVISIBLE);
			return true;
		}
		
		// TODO Auto-generated method stub
		//return super.onKeyUp(keyCode, keyEvent);
		
		Log.i(TAG, "Unrecognized KeyUp="+keyCode);
		
		return true;
	}

	
	
}
