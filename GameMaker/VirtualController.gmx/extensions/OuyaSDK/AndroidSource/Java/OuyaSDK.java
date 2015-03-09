package ${YYAndroidPackageName};

import android.app.Activity;
import android.content.Intent;
import android.util.Log;
import android.widget.FrameLayout;
import com.yoyogames.runner.RunnerJNILib;
import java.lang.String;
import tv.ouya.sdk.OuyaInputView;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.app.Dialog;
import android.view.MotionEvent;
import tv.ouya.console.api.OuyaController;

import ${YYAndroidPackageName}.R;
import ${YYAndroidPackageName}.RunnerActivity;

public class OuyaSDK extends RunnerSocial {
	private static final String TAG = OuyaSDK.class.getSimpleName();
	
	private static final boolean mEnableLogging = true;
	
	private static final String sFalse = "false";
	private static final String sTrue = "true";
	
	private static OuyaInputView sInputView = null;
	
	@Override
	public void onResume() {
		Log.i(TAG, "onResume called in MyExtensionClass extension");
	}

	@Override
	public void Init() {
		Log.i(TAG, "Overridden init called for MyExtensionClass");
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		Log.i(TAG, "onActivityResult called in MyExtensionClass extension");
	}
	
	public String init() {
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "Current activity is null");
			return sFalse;
		} else {
			Log.d(TAG, "Current activity is valid");
		}
		
		final FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
        if (null == content) {
			Log.d(TAG, "Content is null");
			return sFalse;
		} else {
			Runnable runnable = new Runnable() {
				public void run() {
					Log.d(TAG, "Disable screensaver");
					content.setKeepScreenOn(true);
					
					Log.d(TAG, "Add inputView");
					sInputView = new OuyaInputView(activity);
				}
			};
			activity.runOnUiThread(runnable);
        }
		
		return sTrue;
	}
	
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
    	if (null != sInputView) {
			return sInputView.dispatchKeyEvent(keyEvent);
		}
		return true;
	}
	
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	if (null != sInputView) {
			return sInputView.dispatchGenericMotionEvent(motionEvent);
		}
		return true;
	}
	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		if (null != sInputView) {
			return sInputView.onGenericMotionEvent(motionEvent);
		}
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		if (null != sInputView) {
			return sInputView.onKeyDown(keyCode, keyEvent);
		}
		return true;
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		if (null != sInputView) {
			return sInputView.onKeyUp(keyCode, keyEvent);
		}
		return true;
	}

	//double, string, string
	public double getAxis(String strPlayerNum, String strAxis) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int axis = Integer.parseInt(strAxis);
		double result = OuyaInputView.getAxis(playerNum, axis);
		/*
		if (mEnableLogging) {
			Log.i(TAG, "getAxis playerNum="+strPlayerNum+" axis="+strAxis+" result="+result);
		}
		*/
		return result;
	}
	
	//boolean, string, string
	public String getButton(String strPlayerNum, String strButton) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int button = Integer.parseInt(strButton);
		boolean result = OuyaInputView.getButton(playerNum, button);
		/*
		if (mEnableLogging) {
			Log.i(TAG, "getButton playerNum="+strPlayerNum+" button="+strButton+" result="+result);
		}
		*/
		return Boolean.toString(result);
	}
	
	//boolean, string, string
	public String getButtonDown(String strPlayerNum, String strButton) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int button = Integer.parseInt(strButton);
		boolean result = OuyaInputView.getButtonDown(playerNum, button);
		/*
		if (mEnableLogging) {
			Log.i(TAG, "getButtonDown playerNum="+strPlayerNum+" button="+strButton+" result="+result);
		}
		*/
		return Boolean.toString(result);
	}
	
	public String getButtonUp(String strPlayerNum, String strButton) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int button = Integer.parseInt(strButton);
		boolean result = OuyaInputView.getButtonUp(playerNum, button);
		/*
		if (mEnableLogging) {
			Log.i(TAG, "getButtonUp playerNum="+strPlayerNum+" button="+strButton+" result="+result);
		}
		*/
		return Boolean.toString(result);
	}
	
	public String isConnected(String strPlayerNum) {
		int playerNum = Integer.parseInt(strPlayerNum);
		if (OuyaController.getControllerByPlayer(playerNum) == null) {
			return "false";
		} else {
			return "true";
		}
	}
	
	public double clearButtonStatesPressedReleased() {
		OuyaInputView.clearButtonStatesPressedReleased();
		return 0;
	}
}
