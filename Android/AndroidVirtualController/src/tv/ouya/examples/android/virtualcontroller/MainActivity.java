package tv.ouya.examples.android.virtualcontroller;

import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import tv.ouya.console.api.*;
import tv.ouya.console.api.OuyaController.ButtonData;
import tv.ouya.sdk.*;

public class MainActivity extends OuyaActivity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final Float AXIS_SCALER = 4f;
	
	private TextView txtSystem = null;
	private TextView txtController = null;
	private TextView txtKeyCode = null;
	private TextView txtKeyCode2 = null;
	private ImageView imgControllerO = null;
	private ImageView imgControllerU = null;
	private ImageView imgControllerY = null;
	private ImageView imgControllerA = null;
	private ImageView imgControllerL1 = null;
	private ImageView imgControllerL2 = null;
	private ImageView imgControllerL3 = null;
	private ImageView imgControllerR1 = null;
	private ImageView imgControllerR2 = null;
	private ImageView imgControllerR3 = null;
	private ImageView imgControllerDpadDown = null;
	private ImageView imgControllerDpadLeft = null;
	private ImageView imgControllerDpadRight = null;
	private ImageView imgControllerDpadUp = null;
	private ImageView imgControllerMenu = null;
	private ImageView imgButtonMenu = null;
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
	
	// track motion event data
	private float mDpadX = 0f;
	private float mDpadY = 0f;
	
	// keep track when menu button was seen
	private Boolean mWaitToExit = true;
	private float mMenuDetected = 0f;
	
	private OuyaFacade mOuyaFacade = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		setContentView(R.layout.activity_main);
		super.onCreate(savedInstanceState);
		
		ViewGroup mainLayout = (ViewGroup)this.findViewById(android.R.id.content);		
		mainLayout.setOnClickListener(mClickListener);
		
		mainLayout.setKeepScreenOn(true);
		
		mOuyaFacade = OuyaFacade.getInstance();
		mOuyaFacade.init(this, "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
		
		txtSystem = (TextView)findViewById(R.id.txtSystem);
		txtController = (TextView)findViewById(R.id.txtController);
		imgButtonMenu = (ImageView)findViewById(R.id.imgButtonMenu);
		txtKeyCode = (TextView)findViewById(R.id.txtKeyCode);
		txtKeyCode2 = (TextView)findViewById(R.id.txtKeyCode2);
		imgControllerO = (ImageView)findViewById(R.id.imgControllerO);
		imgControllerU = (ImageView)findViewById(R.id.imgControllerU);
		imgControllerY = (ImageView)findViewById(R.id.imgControllerY);
		imgControllerA = (ImageView)findViewById(R.id.imgControllerA);
		imgControllerL1 = (ImageView)findViewById(R.id.imgControllerL1);
		imgControllerL2 = (ImageView)findViewById(R.id.imgControllerL2);
		imgControllerL3 = (ImageView)findViewById(R.id.imgControllerl3);
		imgControllerR1 = (ImageView)findViewById(R.id.imgControllerR1);
		imgControllerR2 = (ImageView)findViewById(R.id.imgControllerR2);
		imgControllerR3 = (ImageView)findViewById(R.id.imgControllerR3);
		imgControllerDpadDown = (ImageView)findViewById(R.id.imgControllerDpadDown);
		imgControllerDpadLeft = (ImageView)findViewById(R.id.imgControllerDpadLeft);
		imgControllerDpadRight = (ImageView)findViewById(R.id.imgControllerDpadRight);
		imgControllerDpadUp = (ImageView)findViewById(R.id.imgControllerDpadUp);
		imgControllerMenu = (ImageView)findViewById(R.id.imgControllerMenu);
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
		
        OuyaInputMapper.setEnableControllerDispatch(true);        
        OuyaController.showCursor(true); //show cursor
        
    	// spawn thread to toggle menu button
        Thread timer = new Thread()
        {
	        public void run()
	        {
	        	while (mWaitToExit)
	        	{
	        		if (mMenuDetected != 0 &&
	        			mMenuDetected < System.nanoTime())
	        		{
	        			mMenuDetected = 0;
	        			Runnable runnable = new Runnable()
	        			{
		        			public void run()
		        			{
		        				imgButtonMenu.setVisibility(View.INVISIBLE);
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
				" Version=" + android.os.Build.VERSION.SDK_INT +
				" isRunningOnOUYASupportedHardware="+mOuyaFacade.isRunningOnOUYASupportedHardware());
		
		setDrawable(imgControllerO, OuyaController.BUTTON_O);
		setDrawable(imgControllerU, OuyaController.BUTTON_U);
		setDrawable(imgControllerY, OuyaController.BUTTON_Y);
		setDrawable(imgControllerA, OuyaController.BUTTON_A);
		setDrawable(imgControllerL1, OuyaController.BUTTON_L1);
		setDrawable(imgControllerL2, OuyaController.BUTTON_L2);
		setDrawable(imgControllerL3, OuyaController.BUTTON_L3);
		setDrawable(imgControllerR1, OuyaController.BUTTON_R1);
		setDrawable(imgControllerR2, OuyaController.BUTTON_R2);
		setDrawable(imgControllerR3, OuyaController.BUTTON_R3);
		setDrawable(imgControllerDpadDown, OuyaController.BUTTON_DPAD_DOWN);
		setDrawable(imgControllerDpadLeft, OuyaController.BUTTON_DPAD_LEFT);
		setDrawable(imgControllerDpadRight, OuyaController.BUTTON_DPAD_RIGHT);
		setDrawable(imgControllerDpadUp, OuyaController.BUTTON_DPAD_UP);
		setDrawable(imgControllerMenu, OuyaController.BUTTON_MENU);
	}
	
	@Override
	protected void onDestroy() {
		super.onDestroy();
		mOuyaFacade.shutdown();
		mWaitToExit = false; //let timer exit
	}
	
	private OnClickListener mClickListener = new OnClickListener() {
	    public void onClick(View v) {
	    	txtKeyCode.setText("Click detected");
	    	txtController.setText("");
	    }
	};
	
	private void setDrawable(ImageView imageView, int keyCode) {
		ButtonData data = OuyaController.getButtonData(keyCode);
		if (null != data) {
			imageView.setImageDrawable(data.buttonDrawable);
		}
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		//Log.i(TAG, "dispatchKeyEvent");
		if (null != txtKeyCode) {
			InputDevice device = keyEvent.getDevice();
			if (null != device) {
				txtKeyCode.setText("Original KeyEvent device=" + device.getName() + " KeyCode=(" + keyEvent.getKeyCode() + ") "
						+ DebugInput.debugGetButtonName(keyEvent.getKeyCode())+" source="+keyEvent.getSource());
			}
		}
		return super.dispatchKeyEvent(keyEvent);
	}
	
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		if (null != txtKeyCode) {
			InputDevice device = motionEvent.getDevice();
			if (null != device) {
				txtKeyCode.setText("Original MotionEvent device=" + device.getName());
			}
		}
		DebugInput.debugMotionEvent(motionEvent);		
		return super.dispatchGenericMotionEvent(motionEvent);
	}
	
	void updateDPad() {
		if (mDpadX > 0.25f) {
	    	imgDpadRight.setVisibility(View.VISIBLE);
	    } else {
	    	imgDpadRight.setVisibility(View.INVISIBLE);
	    }
	    
	    if (mDpadX < -0.25f) {
	    	imgDpadLeft.setVisibility(View.VISIBLE);
	    } else {
	    	imgDpadLeft.setVisibility(View.INVISIBLE);
	    }
	    
	    if (mDpadY > 0.25f) {
	    	imgDpadDown.setVisibility(View.VISIBLE);
	    } else {
	    	imgDpadDown.setVisibility(View.INVISIBLE);
	    }
	    
	    if (mDpadY < -0.25f) {
	    	imgDpadUp.setVisibility(View.VISIBLE);
	    } else {
	    	imgDpadUp.setVisibility(View.INVISIBLE);
	    }
	}
	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//Log.i(TAG, "onGenericMotionEvent");
		//DebugInput.debugOuyaMotionEvent(motionEvent);
		
		if (null != txtKeyCode2) {
			InputDevice device = motionEvent.getDevice();
			if (null != device) {
				txtKeyCode.setText("Original MotionEvent device=" + device.getName());
				OuyaController controller = OuyaController.getControllerByDeviceId(device.getId());
				if (null == controller) {
					txtKeyCode2.setText("Remapped MotionEvent device=unknown");
				} else {
					txtKeyCode2.setText("Remapped MotionEvent device=" + controller.getDeviceName()+" playerNum="+controller.getPlayerNum());
				}
			}
		}
		
		mDpadX = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_X);
		mDpadY = motionEvent.getAxisValue(MotionEvent.AXIS_HAT_Y);		
		updateDPad();
		
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
	    
		return false;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyDown");
		
		OuyaController controller = OuyaController.getControllerByDeviceId(keyEvent.getDeviceId());
		if (null != controller) {
			txtKeyCode2.setText("Remapped onKeyDown device=" + controller.getDeviceName() + " KeyCode=(" + keyCode + ") "
					+ DebugInput.debugGetButtonName(keyCode) + " playerNum="+controller.getPlayerNum());
		} else {
			txtKeyCode2.setText("Remapped onKeyDown device=unknown KeyCode=(" + keyCode + ") "
					+ DebugInput.debugGetButtonName(keyCode));
		}
		
		//Log.i(TAG, "KeyDown="+keyCode);
		
		switch (keyCode)
		{
		case OuyaController.BUTTON_L1:
			imgLeftBumper.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_L2:
			imgLeftTrigger.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_L3:
			imgLeftStick.setVisibility(View.INVISIBLE);
			imgLeftThumb.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_R1:
			imgRightBumper.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_R2:
			imgRightTrigger.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_R3:
			imgRightStick.setVisibility(View.INVISIBLE);
			imgRightThumb.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_O:
			imgButtonO.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_U:
			imgButtonU.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_Y:
			imgButtonY.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_A:
			imgButtonA.setVisibility(View.VISIBLE);
			break;
		case OuyaController.BUTTON_DPAD_DOWN:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadDown.setVisibility(View.VISIBLE);
			}
			break;
		case OuyaController.BUTTON_DPAD_LEFT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadLeft.setVisibility(View.VISIBLE);
			}
			break;
		case OuyaController.BUTTON_DPAD_RIGHT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadRight.setVisibility(View.VISIBLE);
			}
			break;
		case OuyaController.BUTTON_DPAD_UP:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadUp.setVisibility(View.VISIBLE);
			}
			break;
		case OuyaController.BUTTON_MENU:
			imgButtonMenu.setVisibility(View.VISIBLE);
			mMenuDetected = System.nanoTime() + 1000000000;
			break;
		default:
			Log.i(TAG, "Unrecognized KeyDown="+keyCode);
			break;
		}
		
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		//Log.i(TAG, "onKeyUp");
		
		if (keyEvent.getSource() != 1281) {
			return false;
		}
		
		OuyaController controller = OuyaController.getControllerByDeviceId(keyEvent.getDeviceId());
		if (null != controller) {
			txtKeyCode2.setText("Remapped onKeyUp device=" + controller.getDeviceName() + " KeyCode=(" + keyCode + ") "
					+ DebugInput.debugGetButtonName(keyCode) + " playerNum="+controller.getPlayerNum());
		}
		
		//Log.i(TAG, "KeyUp="+keyCode);
		
		switch (keyCode)
		{
		case OuyaController.BUTTON_L1:
			imgLeftBumper.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_L2:
			imgLeftTrigger.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_L3:
			imgLeftStick.setVisibility(View.VISIBLE);
			imgLeftThumb.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_R1:
			imgRightBumper.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_R2:
			imgRightTrigger.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_R3:
			imgRightStick.setVisibility(View.VISIBLE);
			imgRightThumb.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_O:
			imgButtonO.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_U:
			imgButtonU.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_Y:
			imgButtonY.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_A:
			imgButtonA.setVisibility(View.INVISIBLE);
			break;
		case OuyaController.BUTTON_DPAD_DOWN:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadDown.setVisibility(View.INVISIBLE);
			}
			break;
		case OuyaController.BUTTON_DPAD_LEFT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadLeft.setVisibility(View.INVISIBLE);
			}
			break;
		case OuyaController.BUTTON_DPAD_RIGHT:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadRight.setVisibility(View.INVISIBLE);
			}
			break;
		case OuyaController.BUTTON_DPAD_UP:
			if (keyEvent.getSource() == InputDevice.SOURCE_JOYSTICK ) {
				updateDPad();
			} else {
				imgDpadUp.setVisibility(View.INVISIBLE);
			}
			break;
		case OuyaController.BUTTON_MENU:
			//wait 1 second
			break;
		}
		
		return true;
	}	
}