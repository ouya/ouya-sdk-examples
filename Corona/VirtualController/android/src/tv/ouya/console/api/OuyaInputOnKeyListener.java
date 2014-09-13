package tv.ouya.console.api;

import android.util.Log;
import android.view.KeyEvent;
import android.view.View;

public class OuyaInputOnKeyListener implements View.OnKeyListener {	
	private static final String TAG = OuyaInputOnKeyListener.class.getSimpleName();

	@Override
	public boolean onKey(View v, int keyCode, KeyEvent keyEvent) {
		if (null == keyEvent) {
			Log.i(TAG, "onKey null keyEvent");
			return false;
		}
		Log.i(TAG, "onKey keyCode=" + keyEvent.getKeyCode() + " " + DebugInput.debugGetButtonName(keyEvent.getKeyCode()) + " action="+keyEvent.getAction()+" source="+keyEvent.getSource());
		return true;
	}
}
