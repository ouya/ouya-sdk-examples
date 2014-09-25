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
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.FrameLayout;

public class OuyaInputView extends View {

	private static final String TAG = OuyaInputView.class.getSimpleName();

	private Activity mActivity = null;

	public OuyaInputView(Activity activity, Context context) {
		super(context);
		Log.i(TAG, "OuyaInputView");
		mActivity = activity;
		if (null != mActivity) {

			FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
			content.addView(this);
			Log.i(TAG, "Added OuyaInputView to content");
			
			setFocusable(true);
			requestFocus();
			Log.i(TAG, "Give the custom view focus");
		} else {
			Log.e(TAG, "Activity is null");
		}
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
		Log.i(TAG, "dispatchKeyEvent keyCode="+keyEvent.getKeyCode());
		return super.dispatchKeyEvent(keyEvent);
	}

	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
		Log.i(TAG, "dispatchGenericMotionEvent");
		return super.dispatchGenericMotionEvent(motionEvent);
    }

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		Log.i(TAG, "onGenericMotionEvent");
		return super.onGenericMotionEvent(motionEvent);
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		Log.i(TAG, "onKeyUp keyCode=" + keyCode);
		return super.onKeyUp(keyCode, keyEvent);
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		Log.i(TAG, "onKeyDown keyCode=" + keyCode);
		return super.onKeyDown(keyCode, keyEvent);
	}
}
