package tv.ouya.examples.android.razervirtualcontroller;

import android.app.Activity;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.util.SparseArray;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import java.lang.reflect.Method;
import java.util.HashMap;
import tv.ouya.sdk.*;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final Float AXIS_SCALER = 4f;
	
	private static final int BUTTON_MENU = 82;
	
	private TextView txtSystem = null;
	private TextView txtController = null;
	private TextView txtKeyCode = null;
	private TextView txtKeyCode2 = null;
	private ImageView imgDpadDown = null;
	private ImageView imgDpadLeft = null;
	private ImageView imgDpadRight = null;
	private ImageView imgDpadUp = null;
	private ImageView imgLeftStick = null;
	private ImageView imgLeftBumper = null;
	private ImageView imgLeftTrigger = null;
	private ImageView imgRightStick = null;
	private ImageView imgRightBumper = null;
	private ImageView imgRightTrigger = null;
	private ImageView imgLeftThumb = null;
	private ImageView imgRightThumb = null;
	private ImageView imgButtonA = null;
	private ImageView imgButtonB = null;
	private ImageView imgButtonX = null;
	private ImageView imgButtonY = null;
	private ImageView imgButtonBack = null;
	private ImageView imgButtonNext = null;
	private ImageView imgButtonHome = null;
	private ImageView imgButtonPrevious = null;
	private ImageView imgButtonPower = null;
	
	// keep track when menu button was seen
	private Boolean mWaitToExit = true;
	private float mHomeDetected = 0f;
	private float mPowerDetected = 0f;

	private static SparseArray<HashMap<Integer, Float>> sAxisValues = new SparseArray<HashMap<Integer, Float>>();
	private static SparseArray<HashMap<Integer, Boolean>> sButtonValues = new SparseArray<HashMap<Integer, Boolean>>();
	
	static {
		for (int index = 0; index < RazerController.MAX_CONTROLLERS; ++index) {
			HashMap<Integer, Float> axisMap = new HashMap<Integer, Float>();
			axisMap.put(MotionEvent.AXIS_HAT_X, 0f);
			axisMap.put(MotionEvent.AXIS_HAT_Y, 0f);
			sAxisValues.put(index, axisMap);
			HashMap<Integer, Boolean> buttonMap = new HashMap<Integer, Boolean>();
			sButtonValues.put(index, buttonMap);
		}
    }
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		setContentView(R.layout.activity_main);
		super.onCreate(savedInstanceState);
		
		ViewGroup mainLayout = (ViewGroup)this.findViewById(android.R.id.content);		
		mainLayout.setOnClickListener(mClickListener);
		
		mainLayout.setKeepScreenOn(true);
		
		txtSystem = (TextView)findViewById(R.id.txtSystem);
		txtController = (TextView)findViewById(R.id.txtController);
		txtKeyCode = (TextView)findViewById(R.id.txtKeyCode);
		txtKeyCode2 = (TextView)findViewById(R.id.txtKeyCode2);
		imgButtonA = (ImageView)findViewById(R.id.imgButtonA);
		imgDpadDown = (ImageView)findViewById(R.id.imgDpadDown);
		imgDpadLeft = (ImageView)findViewById(R.id.imgDpadLeft);
		imgDpadRight = (ImageView)findViewById(R.id.imgDpadRight);
		imgDpadUp = (ImageView)findViewById(R.id.imgDpadUp);
		imgLeftStick = (ImageView)findViewById(R.id.imgLeftStick);
		imgLeftBumper = (ImageView)findViewById(R.id.imgLeftBumper);
		imgLeftTrigger = (ImageView)findViewById(R.id.imgLeftTrigger);
		imgRightStick = (ImageView)findViewById(R.id.imgRightStick);
		imgRightBumper = (ImageView)findViewById(R.id.imgRightBumper);
		imgRightTrigger = (ImageView)findViewById(R.id.imgRightTrigger);
		imgLeftThumb = (ImageView)findViewById(R.id.imgLeftThumb);
		imgRightThumb = (ImageView)findViewById(R.id.imgRightThumb);
		imgButtonA = (ImageView)findViewById(R.id.imgButtonA);
		imgButtonB = (ImageView)findViewById(R.id.imgButtonB);
		imgButtonX = (ImageView)findViewById(R.id.imgButtonX);
		imgButtonY = (ImageView)findViewById(R.id.imgButtonY);
		imgButtonBack = (ImageView)findViewById(R.id.imgButtonBack);
		imgButtonNext = (ImageView)findViewById(R.id.imgButtonNext);
		imgButtonHome = (ImageView)findViewById(R.id.imgButtonHome);
		imgButtonPrevious = (ImageView)findViewById(R.id.imgButtonPrevious);
		imgButtonPower = (ImageView)findViewById(R.id.imgButtonPower);
		
    	// spawn thread to toggle menu button
        Thread timer = new Thread()
        {
	        public void run()
	        {
	        	while (mWaitToExit)
	        	{
	        		if (mHomeDetected != 0 &&
	        				mHomeDetected < System.nanoTime())
		        		{
	        			mHomeDetected = 0;
		        			Runnable runnable = new Runnable()
		        			{
			        			public void run()
			        			{
			        				imgButtonHome.setVisibility(View.INVISIBLE);
			        			}
		        			};
		        			runOnUiThread(runnable);
		        			
		        		}
	        		if (mPowerDetected != 0 &&
	        			mPowerDetected < System.nanoTime())
	        		{
	        			mPowerDetected = 0;
	        			Runnable runnable = new Runnable()
	        			{
		        			public void run()
		        			{
		        				imgButtonPower.setVisibility(View.INVISIBLE);
		        			}
	        			};
	        			runOnUiThread(runnable);
	        			
	        		}
	        		try
	        		{
	        			Thread.sleep(50);
	        		}
	        		catch (InterruptedException e)
	        		{
	        		}
		        }
			}
        };
		timer.start();
	}

	@Override
	protected void onStart() {
		super.onStart();
		txtSystem.setText("Brand=" + android.os.Build.BRAND + " Model=" + android.os.Build.MODEL + " Device=" + Build.DEVICE +
				" Version=" + android.os.Build.VERSION.SDK_INT);
	}
	
	@Override
	protected void onDestroy() {
		super.onDestroy();
		mWaitToExit = false; //let timer exit
	}
	
	private OnClickListener mClickListener = new OnClickListener() {
	    public void onClick(View v) {
	    	txtKeyCode.setText("Click detected");
	    	txtController.setText("");
	    }
	};

	void updateDPad(int playerNum) {
		if (null == sButtonValues.get(playerNum).get(RazerController.BUTTON_DPAD_LEFT) &&
			null == sButtonValues.get(playerNum).get(RazerController.BUTTON_DPAD_RIGHT)) {
			float dpadX = sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_X);
			if (dpadX > 0.25f) {
		    	imgDpadRight.setVisibility(View.VISIBLE);
		    } else {
		    	imgDpadRight.setVisibility(View.INVISIBLE);
		    }		    
		    if (dpadX < -0.25f) {
		    	imgDpadLeft.setVisibility(View.VISIBLE);
		    } else {
		    	imgDpadLeft.setVisibility(View.INVISIBLE);
		    }
		}
		
		if (null == sButtonValues.get(playerNum).get(RazerController.BUTTON_DPAD_DOWN) &&
			null == sButtonValues.get(playerNum).get(RazerController.BUTTON_DPAD_UP)) {	    
		    float dpadY = sAxisValues.get(playerNum).get(MotionEvent.AXIS_HAT_Y);	    
		    if (dpadY > 0.25f) {
		    	imgDpadDown.setVisibility(View.VISIBLE);
		    } else {
		    	imgDpadDown.setVisibility(View.INVISIBLE);
		    }
		    
		    if (dpadY < -0.25f) {
		    	imgDpadUp.setVisibility(View.VISIBLE);
		    } else {
		    	imgDpadUp.setVisibility(View.INVISIBLE);
		    }
		}
	}

	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//Log.i(TAG, "onGenericMotionEvent");
		DebugInput.debugMotionEvent(motionEvent);
		//DebugInput.debugOuyaMotionEvent(motionEvent);

		int playerNum = 0;
		
		float dpadX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
		float dpadY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);
		sAxisValues.get(playerNum).put(MotionEvent.AXIS_HAT_X, dpadX);
		sAxisValues.get(playerNum).put(MotionEvent.AXIS_HAT_Y, dpadY);
		updateDPad(playerNum);

		float lsX = motionEvent.getAxisValue(RazerController.AXIS_LS_X);
	    float lsY = motionEvent.getAxisValue(RazerController.AXIS_LS_Y);
	    float rsX = motionEvent.getAxisValue(RazerController.AXIS_RS_X);
	    float rsY = motionEvent.getAxisValue(RazerController.AXIS_RS_Y);
	    float l2 = motionEvent.getAxisValue(RazerController.AXIS_L2);
	    float r2 = motionEvent.getAxisValue(RazerController.AXIS_R2);
	    
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
	    
		return false;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyDown keyCode="+keyEvent.getKeyCode());
		
		if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK) {
			return false;
		}
		
		InputDevice device = keyEvent.getDevice();
		if (null != device) {
			String text = "KeyCode=(" + keyCode + ") "
					+ DebugInput.debugGetButtonName(keyCode)+" source="+keyEvent.getSource()
					+ " fallback="+(keyEvent.getFlags() == KeyEvent.FLAG_FALLBACK);
			if (android.os.Build.VERSION.SDK_INT >= 19) {
				try {
					Method m = device.getClass().getDeclaredMethod("getVendorId");
					text += " vendor="+Integer.toHexString((Integer)m.invoke(device, (Object[])null));
				} catch (Exception e) {
				}
				
				try {
					Method m = device.getClass().getDeclaredMethod("getProductId");
					text += " product="+Integer.toHexString((Integer)m.invoke(device, (Object[])null));
				} catch (Exception e) {
				}
			}
			txtKeyCode2.setText(text);
		}
		
		int playerNum = 0;	    

		switch (keyCode)
		{
		case RazerController.BUTTON_DPAD_DOWN:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, true);
				imgDpadDown.setVisibility(View.VISIBLE);
			}
			break;
		case RazerController.BUTTON_DPAD_LEFT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, true);
				imgDpadLeft.setVisibility(View.VISIBLE);
			}
			break;
		case RazerController.BUTTON_DPAD_RIGHT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, true);
				imgDpadRight.setVisibility(View.VISIBLE);
			}
			break;
		case RazerController.BUTTON_DPAD_UP:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, true);
				imgDpadUp.setVisibility(View.VISIBLE);
			}
			break;
		case RazerController.BUTTON_L1:
			imgLeftBumper.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_L2:
			imgLeftTrigger.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_L3:
			imgLeftStick.setVisibility(View.INVISIBLE);
			imgLeftThumb.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_R1:
			imgRightBumper.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_R2:
			imgRightTrigger.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_R3:
			imgRightStick.setVisibility(View.INVISIBLE);
			imgRightThumb.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_A:
			imgButtonA.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_B:
			imgButtonB.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_X:
			imgButtonX.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_Y:
			imgButtonY.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_BACK:
			imgButtonBack.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_NEXT:
			imgButtonNext.setVisibility(View.VISIBLE);
			break;
		case BUTTON_MENU:
		case RazerController.BUTTON_HOME:
			imgButtonHome.setVisibility(View.VISIBLE);
			mHomeDetected = System.nanoTime() + 1000000000;
			break;
		case RazerController.BUTTON_PREVIOUS:
			imgButtonPrevious.setVisibility(View.VISIBLE);
			break;
		case RazerController.BUTTON_POWER:
			imgButtonPower.setVisibility(View.VISIBLE);
			mPowerDetected = System.nanoTime() + 1000000000;
			break;
		default:
			Log.i(TAG, "Unrecognized KeyDown="+keyCode);
			break;
		}
		
		return true;
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyUp keyCode="+keyEvent.getKeyCode());
		
		if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK) {
			return false;
		}
		
		int playerNum = 0;

		switch (keyCode)
		{
		case RazerController.BUTTON_DPAD_DOWN:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, false);
				imgDpadDown.setVisibility(View.INVISIBLE);
			}
			break;
		case RazerController.BUTTON_DPAD_LEFT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, false);
				imgDpadLeft.setVisibility(View.INVISIBLE);
			}
			break;
		case RazerController.BUTTON_DPAD_RIGHT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, false);
				imgDpadRight.setVisibility(View.INVISIBLE);
			}
			break;
		case RazerController.BUTTON_DPAD_UP:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad(playerNum);
			} else {
				sButtonValues.get(playerNum).put(keyCode, false);
				imgDpadUp.setVisibility(View.INVISIBLE);
			}
			break;
		case RazerController.BUTTON_L1:
			imgLeftBumper.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_L2:
			imgLeftTrigger.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_L3:
			imgLeftStick.setVisibility(View.VISIBLE);
			imgLeftThumb.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_R1:
			imgRightBumper.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_R2:
			imgRightTrigger.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_R3:
			imgRightStick.setVisibility(View.VISIBLE);
			imgRightThumb.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_A:
			imgButtonA.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_B:
			imgButtonB.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_X:
			imgButtonX.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_Y:
			imgButtonY.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_BACK:
			imgButtonBack.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_NEXT:
			imgButtonNext.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_HOME:
			//wait 1 sec
			break;
		case RazerController.BUTTON_PREVIOUS:
			imgButtonPrevious.setVisibility(View.INVISIBLE);
			break;
		case RazerController.BUTTON_POWER:
			//wait 1 sec
			break;
		}
		
		return true;
	}	
}