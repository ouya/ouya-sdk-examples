/*
 * Copyright (C) 2012-2015 OUYA, Inc.
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

package tv.ouya.sdk.unreal;

import tv.ouya.console.api.content.OuyaMod;
import android.util.Log;


public class CallbacksContentDelete {

	private final String TAG  = CallbacksContentDelete.class.getSimpleName();

	public native void CallbacksContentDeleteOnDeleteFailed(OuyaMod ouyaMod, int code, String reason);
	public native void CallbacksContentDeleteOnDeleted(OuyaMod ouyaMod);

	public void onDeleteFailed(OuyaMod ouyaMod, final int code, final String reason) {
		Log.e(TAG, "onDeleteFailed code=" + code + " reason="+reason);
		
		CallbacksContentDeleteOnDeleteFailed(ouyaMod, code, reason);
	}

	public void onDeleted(OuyaMod ouyaMod) {
		Log.i(TAG, "onDeleted");

		CallbacksContentDeleteOnDeleted(ouyaMod);
	}
}
