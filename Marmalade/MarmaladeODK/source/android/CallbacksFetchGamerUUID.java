/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
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

package com.ODK;


import android.util.Log;


public class CallbacksFetchGamerUUID {

	private final String LOG_TAG  = "CallbacksFetchGamerUUID";

	public native void CallbacksFetchGamerUUIDOnSuccess(String gamerUUID);
	public native void CallbacksFetchGamerUUIDOnFailure(int errorCode, String errorMessage);
	public native void CallbacksFetchGamerUUIDOnCancel();

	public void onSuccess(final String gamerUUID) {
		Log.i(LOG_TAG, "onSuccess=" + gamerUUID);
		CallbacksFetchGamerUUIDOnSuccess(gamerUUID);
	}

	public void onFailure(final int errorCode, final String errorMessage) {
		Log.i(LOG_TAG, "onFailure: errorCode=" + errorCode + " errorMessagee=" + errorMessage);
		CallbacksFetchGamerUUIDOnFailure(errorCode, errorMessage);
	}

	public void onCancel() {
		Log.i(LOG_TAG, "onCancel");
		CallbacksFetchGamerUUIDOnCancel();
	}
}