/*
 * Copyright (C) 2012-2014 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package tv.ouya.sdk;

import android.app.Activity;
import android.content.Context;
import android.util.AttributeSet;
import android.util.Log;
import android.util.SparseArray;
import android.util.SparseBooleanArray;
import android.view.InputEvent;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.FrameLayout;
import java.util.ArrayList;
import java.util.List;
import tv.ouya.console.api.DebugInput;
import tv.ouya.console.api.OuyaController;
import tv.ouya.console.api.OuyaInputMapper;

public class OuyaInputView extends View {

	private static final String TAG = OuyaInputView.class.getSimpleName();
	
	private static boolean sEnableInputLogging = false;
	
	private static OuyaInputView sInstance = null;
	
	private static List<SparseArray<Float>> sStateAxis = new ArrayList<SparseArray<Float>>();
	private static List<SparseBooleanArray> sStateButton = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sStateButtonDown = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sStateButtonUp = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sLastStateButtonDown = new ArrayList<SparseBooleanArray>();
	private static List<SparseBooleanArray> sLastStateButtonUp = new ArrayList<SparseBooleanArray>();
	
	private static float mTrackpadX = 0f;
	private static float mTrackpadY = 0f;
	private static boolean mTrackpadDown = false;
	private static boolean mLastTrackpadDown = false;

    public OuyaInputView(Context context, AttributeSet attrs) {
    	super(context, attrs);
		if (sEnableInputLogging) {
			Log.d(TAG, "OuyaInputView(Context context, AttributeSet attrs)");
		}
        init();
    }

    public OuyaInputView(Context context, AttributeSet attrs, int defStyle) {
    	super(context, attrs, defStyle);
		if (sEnableInputLogging) {
			Log.d(TAG, "OuyaInputView(Context context, AttributeSet attrs, int defStyle)");
		}
        init();
    }

    public OuyaInputView(Context context) {
        super(context);
		if (sEnableInputLogging) {
			Log.d(TAG, "OuyaInputView(Context context)");
		}
        init();
    }

    private void init() {
		Activity activity = ((Activity)getContext());
		if (null != activity) {
				
			FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
			if (null != content) {
				content.addView(this);
				if (sEnableInputLogging) {
					Log.d(TAG, "Added view");
				}
			} else {
				Log.e(TAG, "Content view is missing");
			}

			OuyaInputMapper.init(activity);

			activity.takeKeyEvents(true);

			setFocusable(true);
			requestFocus();
			
			// prepare OUYA-Everywhere Input states
			for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {
				sStateAxis.add(new SparseArray<Float>());
				sStateButton.add(new SparseBooleanArray());
				sStateButtonDown.add(new SparseBooleanArray());
				sStateButtonUp.add(new SparseBooleanArray());
				sLastStateButtonDown.add(new SparseBooleanArray());
				sLastStateButtonUp.add(new SparseBooleanArray());
			}
			
			sInstance = this;
			
		} else {
			Log.e(TAG, "Activity is null");
		}
	}

	public void shutdown() {
		Activity activity = ((Activity)getContext());
		if (null != activity) {
    		OuyaInputMapper.shutdown(activity);
    	} else {
    		Log.e(TAG, "Activity was not found.");
    	}
    }
	
	public static void toggleInputLogging(boolean enabled) {
		sEnableInputLogging = enabled;
	}
	
	@Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		if (sEnableInputLogging) {
			Log.i(TAG, "dispatchGenericMotionEvent");
			DebugInput.debugMotionEvent(motionEvent);
		}
    	Activity activity = ((Activity)getContext());
		if (null != activity) {
		    if (OuyaInputMapper.shouldHandleInputEvent(motionEvent)) {
		    	return OuyaInputMapper.dispatchGenericMotionEvent(activity, motionEvent);
		    }
	    } else {
	    	Log.e(TAG, "Activity was not found.");
	    }
    	return super.dispatchGenericMotionEvent(motionEvent);
    }

	@Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		Activity activity = ((Activity)getContext());
		if (null != activity) {
	    	if (OuyaInputMapper.shouldHandleInputEvent(keyEvent)) {
	    		return OuyaInputMapper.dispatchKeyEvent(activity, keyEvent);
	    	}
	    } else {
	    	Log.e(TAG, "Activity was not found.");
	    }
	    return super.dispatchKeyEvent(keyEvent);
    }
	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		/*
		if (sEnableInputLogging) {
			Log.i(TAG, "onGenericMotionEvent");
			DebugInput.debugMotionEvent(motionEvent);
			DebugInput.debugOuyaMotionEvent(motionEvent);
		}
		*/
		
		int playerNum = OuyaController.getPlayerNumByDeviceId(motionEvent.getDeviceId());
		if (playerNum < 0) {
			Log.e(TAG, "Failed to find playerId for Controller="+motionEvent.getDevice().getName());
			playerNum = 0;
		}
		
		if (sStateAxis.size() <= playerNum) {
			return false;
		}
		
		//Log.i(TAG, "dispatchGenericMotionEventNative");
		SparseArray<Float> stateAxises = sStateAxis.get(playerNum);
		if (null == stateAxises) {
			Log.e(TAG, "onGenericMotionEvent stateAxises is null");
			return false;
		}
		
    	stateAxises.put(OuyaController.AXIS_LS_X, motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
    	stateAxises.put(OuyaController.AXIS_LS_Y, motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
    	stateAxises.put(OuyaController.AXIS_RS_X, motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
    	stateAxises.put(OuyaController.AXIS_RS_Y, motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
    	stateAxises.put(OuyaController.AXIS_L2, motionEvent.getAxisValue(OuyaController.AXIS_L2));
    	stateAxises.put(OuyaController.AXIS_R2, motionEvent.getAxisValue(OuyaController.AXIS_R2));

		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		/*
		if (sEnableInputLogging) {
			Log.i(TAG, "onKeyUp keyCode=" + DebugInput.debugGetButtonName(keyEvent.getKeyCode()));
		}
		*/
		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
		if (playerNum < 0) {
			Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
			playerNum = 0;
		}
		
		if (sStateButton.size() <= playerNum) {
			return false;
		}
		
		if (sStateButtonUp.size() <= playerNum) {
			return false;
		}
		
		SparseBooleanArray stateButton = sStateButton.get(playerNum);
		if (null == stateButton) {
			Log.e(TAG, "onKeyUp stateButton playerNum="+playerNum+" keyCode="+keyCode+" is null");
			return super.onKeyUp(keyCode, keyEvent);
		}
		stateButton.put(keyCode, false);
    	if (sEnableInputLogging) {
    		Log.i(TAG, "onKeyUp stateButton playerNum="+playerNum+" keyCode="+keyCode+" is="+stateButton.get(keyCode));
		}
		
		SparseBooleanArray stateButtonUp = sStateButtonUp.get(playerNum);
		if (null == stateButtonUp) {
			Log.e(TAG, "onKeyUp stateButtonUp playerNum="+playerNum+" keyCode="+keyCode+" is null");
		} else {
			stateButtonUp.put(keyCode, true);
			if (sEnableInputLogging) {
				Log.i(TAG, "onKeyUp stateButtonUp playerNum="+playerNum+" keyCode="+keyCode+" is="+stateButtonUp.get(keyCode));
			}
		}
		
		return true;
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		/*
		if (sEnableInputLogging) {
			Log.i(TAG, "onKeyDown keyCode=" + DebugInput.debugGetButtonName(keyEvent.getKeyCode()));
		}
		*/
		int playerNum = OuyaController.getPlayerNumByDeviceId(keyEvent.getDeviceId());
		if (playerNum < 0) {
			Log.e(TAG, "Failed to find playerId for Controller="+keyEvent.getDevice().getName());
			playerNum = 0;
		}
		
		if (sStateButton.size() <= playerNum) {
			return false;
		}
		
		if (sStateButtonDown.size() <= playerNum) {
			return false;
		}

		SparseBooleanArray stateButton = sStateButton.get(playerNum);
		if (null == stateButton) {
			Log.e(TAG, "onKeyDown stateButton playerNum="+playerNum+" keyCode="+keyCode+" is null");
			return super.onKeyDown(keyCode, keyEvent);
		}
		stateButton.put(keyCode, true);
    	if (sEnableInputLogging) {
    		Log.i(TAG, "onKeyDown stateButton playerNum="+playerNum+" keyCode="+keyCode+" is="+stateButton.get(keyCode));
		}
		
		SparseBooleanArray stateButtonDown = sStateButtonDown.get(playerNum);
		if (null == stateButtonDown) {
			Log.e(TAG, "onKeyDown stateButtonDown playerNum="+playerNum+" keyCode="+keyCode+" is null");
		} else {
			stateButtonDown.put(keyCode, true);
			if (sEnableInputLogging) {
				Log.i(TAG, "onKeyDown stateButtonDown playerNum="+playerNum+" keyCode="+keyCode+" is="+stateButtonDown.get(keyCode));
			}
		}
		
		return true;
	}
	
	public static float getAxis(int playerNum, int axis) {
		
		if (sStateAxis.size() <= playerNum) {
			return 0f;
		}
		
		SparseArray<Float> stateAxises = sStateAxis.get(playerNum);
		if (null == stateAxises) {
			Log.e(TAG, "getAxis stateAxises is null");
			return 0;
		}
		
		Float result = stateAxises.get(axis);
		if (null == result) {
			return 0;
		}
		
		return result.floatValue();
	}
	
	public static boolean getButton(int playerNum, int button) {
		/*
		if (sEnableInputLogging) {
			Log.i(TAG, "getButton playerNum="+playerNum+" button=" + button);
		}
		*/
		if (sStateButton.size() <= playerNum) {
			return false;
		}
		SparseBooleanArray stateButton = sStateButton.get(playerNum);
		if (null == stateButton) {
			Log.e(TAG, "getButton stateButton is null");
			return false;
		}		
		boolean result = stateButton.get(button);
		/*
		if (sEnableInputLogging) {
			Log.i(TAG, "getButton result=" + result);
		}
		*/
		return result;
	}
	
	public static boolean getButtonDown(int playerNum, int button) {
		if (sLastStateButtonDown.size() <= playerNum) {
			return false;
		}
		SparseBooleanArray stateButtonDown = sLastStateButtonDown.get(playerNum);
		if (null == stateButtonDown) {
			Log.e(TAG, "getButtonDown stateButtonDown is null");
			return false;
		}		
		return stateButtonDown.get(button);
	}
	
	public static boolean getButtonUp(int playerNum, int button) {
		if (sLastStateButtonUp.size() <= playerNum) {
			return false;
		}
		SparseBooleanArray stateButtonUp = sLastStateButtonUp.get(playerNum);
		if (null == stateButtonUp) {
			Log.e(TAG, "getButtonUp stateButtonUp is null");
			return false;
		}		
		return stateButtonUp.get(button);
	}
	
	public static void clearButtonStatesPressedReleased() {
		
		mLastTrackpadDown = mTrackpadDown;
		mTrackpadDown = false;
		
		if (sStateButtonDown.size() < OuyaController.MAX_CONTROLLERS) {
			return;
		}
		
		if (sStateButtonUp.size() < OuyaController.MAX_CONTROLLERS) {
			return;
		}
		
		if (sLastStateButtonDown.size() < OuyaController.MAX_CONTROLLERS) {
			return;
		}
		
		if (sLastStateButtonUp.size() < OuyaController.MAX_CONTROLLERS) {
			return;
		}
		
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {
			SparseBooleanArray stateButtonDown = sStateButtonDown.get(playerNum);
			if (null == stateButtonDown) {
				Log.e(TAG, "clearButtonStatesPressedReleased stateButtonDown is null");
			} else {
				sLastStateButtonDown.get(playerNum).clear();
				for (int index = 0; index < stateButtonDown.size(); ++index) {
					int keyCode = sStateButtonDown.get(playerNum).keyAt(index);
					sLastStateButtonDown.get(playerNum).put(keyCode, true);
				}
				stateButtonDown.clear();
			}
			
			SparseBooleanArray stateButtonUp = sStateButtonUp.get(playerNum);
			if (null == stateButtonUp) {
				Log.e(TAG, "clearButtonStatesPressedReleased stateButtonUp is null");
			} else {
				sLastStateButtonUp.get(playerNum).clear();
				for (int index = 0; index < stateButtonUp.size(); ++index) {
					int keyCode = sStateButtonUp.get(playerNum).keyAt(index);
					sLastStateButtonUp.get(playerNum).put(keyCode, true);
				}
				stateButtonUp.clear();
			}
		}
	}
	
	public static boolean getTrackpadDown() {
		return mLastTrackpadDown;
	}
	
	public static void setTrackpadDown() {
		mTrackpadDown = true;
	}
	
	public static float getTrackpadX() {
		return mTrackpadX;
	}
	
	public static void setTrackpadX(float val) {
		mTrackpadX = val;
	}
	
	public static float getTrackpadY() {
		return mTrackpadY;
	}
	
	public static void setTrackpadY(float val) {
		mTrackpadY = val;
	}
}
