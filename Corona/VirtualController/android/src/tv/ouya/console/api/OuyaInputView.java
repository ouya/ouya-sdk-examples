package tv.ouya.console.api;

import android.content.Context;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;

public class OuyaInputView extends View implements View.OnKeyListener {
	
	private static final String TAG = OuyaInputView.class.getSimpleName();

	public OuyaInputView(Context context) {
		super(context);
		Log.i(TAG, "OuyaInputView(Context)");
	}

	@Override
	public boolean onKeyPreIme(int keyCode, KeyEvent keyEvent) {
		if (null == keyEvent) {
			return false;
		}
		Log.i(TAG, "onKeyPreIme keyCode=" + keyEvent.getKeyCode() + " " + DebugInput.debugGetButtonName(keyEvent.getKeyCode()) + " action="+keyEvent.getAction()+" source="+keyEvent.getSource());
		return super.onKeyPreIme(keyCode, keyEvent);
	}
	
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		if (null == keyEvent) {
			return false;
		}
		Log.i(TAG, "dispatchKeyEvent " + keyEvent.getClass().getName());
		if (keyEvent.getClass().getName() == "com.ansca.corona.input.CoronaKeyEvent") {
			//eat the extra event
			return true;
		}
		Log.i(TAG, "dispatchKeyEvent keyCode=" + keyEvent.getKeyCode() + " " + DebugInput.debugGetButtonName(keyEvent.getKeyCode()) + " action="+keyEvent.getAction()+" source="+keyEvent.getSource());
        return super.dispatchKeyEvent(keyEvent);
	}

	public boolean onKey(View v, int keyCode, KeyEvent keyEvent) {
		if (null == keyEvent) {
			return false;
		}
		Log.i(TAG, "onKey keyCode=" + keyEvent.getKeyCode() + " " + DebugInput.debugGetButtonName(keyEvent.getKeyCode()) + " action="+keyEvent.getAction()+" source="+keyEvent.getSource());
		return true;
	}

}
