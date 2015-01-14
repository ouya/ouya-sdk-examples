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


public class CallbacksContentSearchPublished {

	private final String TAG  = CallbacksContentSearchPublished.class.getSimpleName();

	public native void CallbacksContentSearchPublishedOnError(int code, String reason);
	public native void CallbacksContentSearchPublishedOnResults(OuyaMod[] ouyaMods, int count);

	public void onError(int code, String reason) {
		Log.e(TAG, "onError code="+code+" reason="+reason);
		
		CallbacksContentSearchPublishedOnError(code, reason);
	}

	public void onResults(OuyaMod[] ouyaMods, int count) {
		Log.i(TAG, "onResults count="+count);

		CallbacksContentSearchPublishedOnResults(ouyaMods, count);
	}
}
