package tv.ouya.examples.android.xarcadetank;

import android.os.Bundle;
import android.util.Log;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import tv.ouya.console.api.DebugInput;
import tv.ouya.console.api.OuyaActivity;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaController.ButtonData;
import tv.ouya.console.api.OuyaInputMapper;
import tv.ouya.examples.android.xarcadetank.R;

public class MainActivity extends OuyaActivity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final Float AXIS_SCALER = 16f;
	
	private TextView txtSystem = null;
	private TextView txtController = null;
	private TextView txtKeyCode = null;
	private ImageView imgButtonMenu1 = null;
	private ImageView imgButtonMenu2 = null;
	private ImageView imgLeftStick = null;
	private ImageView imgButtonL1 = null;
	private ImageView imgButtonL2 = null;
	private ImageView imgButtonL3 = null;
	private ImageView imgButtonL4 = null;
	private ImageView imgButtonL5 = null;
	private ImageView imgButtonL6 = null;
	private ImageView imgButtonL7 = null;
	private ImageView imgButtonL8 = null;
	private ImageView imgButtonL9 = null;
	private ImageView imgButtonL10 = null;
	private ImageView imgTrackball = null;
	private ImageView imgRightStick = null;
	private ImageView imgButtonR1 = null;
	private ImageView imgButtonR2 = null;
	private ImageView imgButtonR3 = null;
	private ImageView imgButtonR4 = null;
	private ImageView imgButtonR5 = null;
	private ImageView imgButtonR6 = null;
	private ImageView imgButtonR7 = null;
	private ImageView imgButtonR8 = null;
	private ImageView imgButtonR9 = null;
	private ImageView imgButtonR10 = null;
	
	// keep track when menu button was seen
	private Boolean mWaitToExit = true;
	private long mMenuDetected = 0;
	
	private long mTrackballDetected = 0;
	private float mTrackballX = 0f;
	private float mTrackballY = 0f;
	
	private long mSecondTicks = 1000000000;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		setContentView(R.layout.activity_main);
		super.onCreate(savedInstanceState);
		
		txtSystem = (TextView)findViewById(R.id.txtSystem);
		txtController = (TextView)findViewById(R.id.txtController);
		txtKeyCode = (TextView)findViewById(R.id.txtKeyCode);
		
		imgButtonMenu1 = (ImageView)findViewById(R.id.imgButtonMenu1);
		imgButtonMenu2 = (ImageView)findViewById(R.id.imgButtonMenu2);
		imgLeftStick = (ImageView)findViewById(R.id.imgLeftStick);
		imgButtonL1 = (ImageView)findViewById(R.id.imgButtonL1);
		imgButtonL2 = (ImageView)findViewById(R.id.imgButtonL2);
		imgButtonL3 = (ImageView)findViewById(R.id.imgButtonL3);
		imgButtonL4 = (ImageView)findViewById(R.id.imgButtonL4);
		imgButtonL5 = (ImageView)findViewById(R.id.imgButtonL5);
		imgButtonL6 = (ImageView)findViewById(R.id.imgButtonL6);
		imgButtonL7 = (ImageView)findViewById(R.id.imgButtonL7);
		imgButtonL8 = (ImageView)findViewById(R.id.imgButtonL8);
		imgButtonL9 = (ImageView)findViewById(R.id.imgButtonL9);
		imgButtonL10 = (ImageView)findViewById(R.id.imgButtonL10);
		imgTrackball = (ImageView)findViewById(R.id.imgTrackball);
		imgRightStick = (ImageView)findViewById(R.id.imgRightStick);
		imgButtonR1 = (ImageView)findViewById(R.id.imgButtonR1);
		imgButtonR2 = (ImageView)findViewById(R.id.imgButtonR2);
		imgButtonR3 = (ImageView)findViewById(R.id.imgButtonR3);
		imgButtonR4 = (ImageView)findViewById(R.id.imgButtonR4);
		imgButtonR5 = (ImageView)findViewById(R.id.imgButtonR5);
		imgButtonR6 = (ImageView)findViewById(R.id.imgButtonR6);
		imgButtonR7 = (ImageView)findViewById(R.id.imgButtonR7);
		imgButtonR8 = (ImageView)findViewById(R.id.imgButtonR8);
		imgButtonR9 = (ImageView)findViewById(R.id.imgButtonR9);
		imgButtonR10 = (ImageView)findViewById(R.id.imgButtonR10);
		
        OuyaInputMapper.setEnableControllerDispatch(true);        
        OuyaController.showCursor(false);
        
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
		        				imgButtonMenu1.setVisibility(View.INVISIBLE);
		        				imgButtonMenu2.setVisibility(View.INVISIBLE);
		        			}
	        			};
	        			runOnUiThread(runnable);
	        			
	        		}
	        		if (mTrackballDetected != 0 &&
	        			mTrackballDetected < System.nanoTime()) {
	        			mTrackballDetected = 0;
	        			Runnable runnable = new Runnable()
	        			{
		        			public void run()
		        			{
		        				imgTrackball.setVisibility(View.INVISIBLE);
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
		txtSystem.setText("Brand=" + android.os.Build.BRAND + " Model=" + android.os.Build.MODEL + " Version=" + android.os.Build.VERSION.SDK_INT);
	}
	
	@Override
	protected void onDestroy() {
		super.onDestroy();
		mWaitToExit = false; //let timer exit
	}
	
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		
		Log.i(TAG, "dispatchKeyEvent keyCode="+keyEvent.getKeyCode());
		
		//Log.i(TAG, "dispatchKeyEvent");
		if (null != txtKeyCode) {
			InputDevice device = keyEvent.getDevice();
			if (null != device) {
				txtKeyCode.setText("Original KeyEvent device=" + device.getName() + " KeyCode=(" + keyEvent.getKeyCode() + ") scan="+keyEvent.getScanCode()+" "
						+ DebugInput.debugGetButtonName(keyEvent.getKeyCode()));
			}
		}
		return super.dispatchKeyEvent(keyEvent);
	}
	
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		DebugInput.debugMotionEvent(motionEvent);
		return super.dispatchGenericMotionEvent(motionEvent);
	}

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		//Log.i(TAG, "onGenericMotionEvent");
		DebugInput.debugOuyaMotionEvent(motionEvent);
		
		if (motionEvent.getAxisValue(2) > 0.25) {
			imgButtonL9.setVisibility(View.VISIBLE);
		} else {
			imgButtonL9.setVisibility(View.INVISIBLE);
		}
		
		if (motionEvent.getAxisValue(2) > 0.25) {
			imgButtonR9.setVisibility(View.VISIBLE);
		} else {
			imgButtonR9.setVisibility(View.INVISIBLE);
		}
		
		if (Math.abs(mTrackballX - motionEvent.getAxisValue(0)) > 0.1f) {
			mTrackballX = motionEvent.getAxisValue(0);
			mTrackballDetected = System.nanoTime() + mSecondTicks;
			imgTrackball.setVisibility(View.VISIBLE);
		}		
		
		if (Math.abs(mTrackballY - motionEvent.getAxisValue(1)) > 0.1f) {
			mTrackballY = motionEvent.getAxisValue(1);
			mTrackballDetected = System.nanoTime() + (long)(mSecondTicks * 0.2);
			imgTrackball.setVisibility(View.VISIBLE);
		}		
	    
		return true;
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		Log.i(TAG, "onKeyDown="+keyCode+" scanCode="+keyEvent.getScanCode());
		
		OuyaController controller = OuyaController.getControllerByDeviceId(keyEvent.getDeviceId());
		if (null != controller) {
			txtController.setText("Remapped onKeyDown device=" + controller.getDeviceName() + " KeyCode=(" + keyCode + ") "
					+ DebugInput.debugGetButtonName(keyCode));
		}
		
		int scanCode = keyEvent.getScanCode();
		
		switch (scanCode) {
		case 72:
			imgLeftStick.setY(-AXIS_SCALER);
			imgLeftStick.setVisibility(View.VISIBLE);
			return true;
		case 75:
			imgLeftStick.setX(-AXIS_SCALER);
			imgLeftStick.setVisibility(View.VISIBLE);
			return true;
		case 77:
			imgLeftStick.setX(AXIS_SCALER);
			imgLeftStick.setVisibility(View.VISIBLE);
			return true;
		case 80:
			imgLeftStick.setY(AXIS_SCALER);
			imgLeftStick.setVisibility(View.VISIBLE);
			return true;
		case 29:
			imgButtonL1.setVisibility(View.VISIBLE);
			return true;
		case 56:
			imgButtonL2.setVisibility(View.VISIBLE);
			return true;
		case 57:
			imgButtonL3.setVisibility(View.VISIBLE);
			return true;
		case 42:
			imgButtonL4.setVisibility(View.VISIBLE);
			return true;
		case 44:
			imgButtonL5.setVisibility(View.VISIBLE);
			return true;
		case 45:
			imgButtonL6.setVisibility(View.VISIBLE);
			return true;
		case 46:
			imgButtonL7.setVisibility(View.VISIBLE);
			return true;
		case 6:
			imgButtonL8.setVisibility(View.VISIBLE);
			return true;
		case 4:
			imgButtonL10.setVisibility(View.VISIBLE);
			return true;
		case 19:
			imgRightStick.setY(-AXIS_SCALER);
			imgRightStick.setVisibility(View.VISIBLE);
			return true;
		case 32:
			imgRightStick.setX(-AXIS_SCALER);
			imgRightStick.setVisibility(View.VISIBLE);
			return true;
		case 33:
			imgRightStick.setY(AXIS_SCALER);
			imgRightStick.setVisibility(View.VISIBLE);
			return true;
		case 34:
			imgRightStick.setX(AXIS_SCALER);
			imgRightStick.setVisibility(View.VISIBLE);
			return true;
		case 30:
			imgButtonR1.setVisibility(View.VISIBLE);
			return true;
		case 31:
			imgButtonR2.setVisibility(View.VISIBLE);
			return true;
		case 16:
			imgButtonR3.setVisibility(View.VISIBLE);
			return true;
		case 17:
			imgButtonR4.setVisibility(View.VISIBLE);
			return true;
		case 18:
			imgButtonR5.setVisibility(View.VISIBLE);
			return true;
		case 26:
			imgButtonR6.setVisibility(View.VISIBLE);
			return true;
		case 27:
			imgButtonR7.setVisibility(View.VISIBLE);
			return true;
		case 7:
			imgButtonR8.setVisibility(View.VISIBLE);
			return true;
		case 5:
			imgButtonR10.setVisibility(View.VISIBLE);
			return true;
		}
		
		switch (keyCode)
		{
		case OuyaController.BUTTON_MENU:
			imgButtonMenu1.setVisibility(View.VISIBLE);
			imgButtonMenu2.setVisibility(View.VISIBLE);
			mMenuDetected = System.nanoTime() + 1000000000;
			return true;
		}
		
		Log.i(TAG, "Unrecognized KeyDown="+keyCode);
		
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		Log.i(TAG, "onKeyUp="+keyCode+" scanCode="+keyEvent.getScanCode());

		OuyaController controller = OuyaController.getControllerByDeviceId(keyEvent.getDeviceId());
		if (null != controller) {
			txtController.setText("Remapped onKeyUp device=" + controller.getDeviceName() + " KeyCode=(" + keyCode + ") "					
					+ DebugInput.debugGetButtonName(keyCode));
		}
		
		int scanCode = keyEvent.getScanCode();
		
		switch (scanCode) {
		case 72:
		case 75:
		case 77:
		case 80:
			imgLeftStick.setVisibility(View.INVISIBLE);
			return true;
		case 29:
			imgButtonL1.setVisibility(View.INVISIBLE);
			return true;
		case 56:
			imgButtonL2.setVisibility(View.INVISIBLE);
			return true;
		case 57:
			imgButtonL3.setVisibility(View.INVISIBLE);
			return true;
		case 42:
			imgButtonL4.setVisibility(View.INVISIBLE);
			return true;
		case 44:
			imgButtonL5.setVisibility(View.INVISIBLE);
			return true;
		case 45:
			imgButtonL6.setVisibility(View.INVISIBLE);
			return true;
		case 46:
			imgButtonL7.setVisibility(View.INVISIBLE);
			return true;
		case 6:
			imgButtonL8.setVisibility(View.INVISIBLE);
			return true;
		case 4:
			imgButtonL10.setVisibility(View.INVISIBLE);
			return true;
		case 19:
		case 32:
		case 33:
		case 34:
			imgRightStick.setVisibility(View.INVISIBLE);
			return true;
		case 30:
			imgButtonR1.setVisibility(View.INVISIBLE);
			return true;
		case 31:
			imgButtonR2.setVisibility(View.INVISIBLE);
			return true;
		case 16:
			imgButtonR3.setVisibility(View.INVISIBLE);
			return true;
		case 17:
			imgButtonR4.setVisibility(View.INVISIBLE);
			return true;
		case 18:
			imgButtonR5.setVisibility(View.INVISIBLE);
			return true;
		case 26:
			imgButtonR6.setVisibility(View.INVISIBLE);
			return true;
		case 27:
			imgButtonR7.setVisibility(View.INVISIBLE);
			return true;
		case 7:
			imgButtonR8.setVisibility(View.INVISIBLE);
			return true;
		case 5:
			imgButtonR10.setVisibility(View.INVISIBLE);
			return true;
		}
		
		switch (keyCode)
		{
		case OuyaController.BUTTON_MENU:
			//wait 1 second
			return true;
		}
		
		return true;
	}	
}