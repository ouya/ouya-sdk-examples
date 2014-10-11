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

package tv.ouya.sdk.marmalade;


import android.util.Log;


public class CallbacksRequestPurchase {

	private final String LOG_TAG  = "CallbacksRequestPurchase";

	public String m_purchasable = "";

	public native void CallbacksRequestPurchaseOnSuccess(String jsonData);
	public native void CallbacksRequestPurchaseOnFailure(int errorCode, String errorMessage);
	public native void CallbacksRequestPurchaseOnCancel();

	public void onSuccess(final String jsonData) {
		Log.i("CallbacksRequestPurchase", "onSuccess jsonData=" + jsonData);
		CallbacksRequestPurchaseOnSuccess(jsonData);
	}

	public void onFailure(final int errorCode, final String errorMessage) {
		Log.i("CallbacksRequestPurchase", "onFailure: errorCode=" + errorCode + " errorMessagee=" + errorMessage);
		CallbacksRequestPurchaseOnFailure(errorCode, errorMessage);
	}

	public void onCancel() {
		Log.i("CallbacksRequestPurchase", "onCancel");
		CallbacksRequestPurchaseOnCancel();
	}
}