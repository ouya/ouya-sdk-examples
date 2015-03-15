package ${YYAndroidPackageName};

import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.widget.FrameLayout;
import com.yoyogames.runner.RunnerJNILib;
import java.io.IOException;
import java.io.InputStream;
import java.lang.String;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import tv.ouya.console.api.*;
import tv.ouya.sdk.OuyaInputView;

import ${YYAndroidPackageName}.R;
import ${YYAndroidPackageName}.RunnerActivity;

public class OuyaSDK extends RunnerSocial {
	private static final String TAG = OuyaSDK.class.getSimpleName();
	
	private static final boolean mEnableLogging = true;
	
	private static final String sFalse = "false";
	private static final String sTrue = "true";
	private static final double sNoop = 0;
	
	private static OuyaInputView sInputView = null;
	
	// Your game talks to the OuyaFacade, which hides all the mechanics of doing an in-app purchase.
	private static OuyaFacade sOuyaFacade = null;
	
	// listener for fetching gamer info
	private static CancelIgnoringOuyaResponseListener<GamerInfo> sRequestGamerInfoListener = null;

	// listener for getting products
	private static CancelIgnoringOuyaResponseListener<List<Product>> sRequestProductsListener = null;
	
	// listener for requesting purchase
	private OuyaResponseListener<PurchaseResult> sRequestPurchaseListener = null;
	
	// listener for getting receipts
	private static OuyaResponseListener<Collection<Receipt>> sRequestReceiptsListener = null;
	
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
		
		/*
		if (mEnableLogging) {
			Log.i(TAG, "onActivityResult called in MyExtensionClass extension");
		}
		*/

		if (null == sOuyaFacade) {
        	return;
        }
		
        // Forward this result to the facade, in case it is waiting for any activity results
        if (sOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
            return;
        }
	}
	
	public String init(String json) {
		
		if (mEnableLogging) {
			Log.i(TAG, "init: json="+json);
		}
		
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "Current activity is null");
			return sFalse;
		} else {
			Log.d(TAG, "Current activity is valid");
		}
		
		byte[] applicationKey = null;
		
		// load the application key from assets
		try {
			AssetManager assetManager = activity.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		if (null == applicationKey) {
			Log.e(TAG, "Failed to load signing key");
			activity.finish();
		}
		
		if (mEnableLogging) {
			Log.i(TAG, "************");
			Log.i(TAG, "************");
			Log.i(TAG, "************");
			Log.i(TAG, "************");
			Log.i(TAG, "************");
			Log.i(TAG, "Signing key loaded!");
		}
		
		Bundle developerInfo = new Bundle();
		
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
	
	//boolean, string
	public String getAnyButton(String strButton) {
		int button = Integer.parseInt(strButton);
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			boolean result = OuyaInputView.getButton(playerNum, button);
			if (result) {
				/*
				if (mEnableLogging) {
					Log.i(TAG, "getAnyButton playerNum="+strPlayerNum+" button="+strButton+" result="+result);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
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
	
	//boolean, string
	public String getAnyButtonDown(String strButton) {
		int button = Integer.parseInt(strButton);
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			boolean result = OuyaInputView.getButtonDown(playerNum, button);
			if (result) {
				/*
				if (mEnableLogging) {
					Log.i(TAG, "getAnyButtonDown playerNum="+strPlayerNum+" button="+strButton+" result="+result);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
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
	
	//boolean, string
	public String getAnyButtonUp(String strButton) {
		int button = Integer.parseInt(strButton);
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			boolean result = OuyaInputView.getButtonUp(playerNum, button);
			if (result) {
				/*
				if (mEnableLogging) {
					Log.i(TAG, "getAnyButtonUp playerNum="+strPlayerNum+" button="+strButton+" result="+result);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
	}
	
	public String isConnected(String strPlayerNum) {
		int playerNum = Integer.parseInt(strPlayerNum);
		if (OuyaController.getControllerByPlayer(playerNum) == null) {
			return sFalse;
		} else {
			return sTrue;
		}
	}
	
	public String isAnyConnected() {
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			if (OuyaController.getControllerByPlayer(playerNum) != null) {
				/*
				if (mEnableLogging) {
					Log.i(TAG, "isAnyConnected playerNum="+strPlayerNum);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
	}
	
	public double clearButtonStatesPressedReleased() {
		OuyaInputView.clearButtonStatesPressedReleased();
		return sNoop;
	}
	
	public double requestGamerInfo() {
		return sNoop;
	}
	
	public double requestProducts(String json) {
		return sNoop;
	}
	
	public double requestPurchase(String identifier) {
		return sNoop;
	}
	
	public double requestReceipts() {
		return sNoop;
	}
}
