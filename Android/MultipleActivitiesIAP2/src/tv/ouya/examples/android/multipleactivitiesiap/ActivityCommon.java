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
package tv.ouya.examples.android.multipleactivitiesiap;


import java.io.IOException;
import java.io.InputStream;
import java.util.Collection;

import tv.ouya.console.api.OuyaFacade;
import tv.ouya.console.api.OuyaResponseListener;
import tv.ouya.console.api.Receipt;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;

// Common code used by both activities
// Isolate the purchase IAP code here
public class ActivityCommon extends Activity
{
	// The tag for log messages
	private static final String TAG = ActivityCommon.class.getSimpleName();

	// Your game talks to the OuyaFacade, which hides all the mechanics of doing an in-app purchase.
	private static OuyaFacade sOuyaFacade = null;
	
	// listener for getting receipts
	private static OuyaResponseListener<Collection<Receipt>> sRequestReceiptsListener = null;
	
	public static ActivityCommon sReceiptCallback = null;
	
	private void init() {
		
		sReceiptCallback = this;
		
		if (sOuyaFacade != null) {
			Log.d(TAG, "OuyaFacade already initialized.");
			return;
		}
		
		Context context = getApplicationContext();
		
		byte[] applicationKey = null;
		
		// load the application key from assets
		try {
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		if (null == applicationKey) {
			Log.e(TAG, "Failed to load signing key");
			return;
		}
		
		Bundle developerInfo = new Bundle();
		developerInfo.putString(OuyaFacade.OUYA_DEVELOPER_ID, "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
		developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, applicationKey);
		
		sOuyaFacade = OuyaFacade.getInstance();
		sOuyaFacade.init(context, developerInfo);
		
		sRequestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {

			 // Handle the successful fetching of the data for the receipts from the server.
			@Override
			public void onSuccess(Collection<Receipt> receipts) {
				Log.i(TAG, "requestReceipts onSuccess: received "+receipts.size() + " receipts");
				sReceiptCallback.onSuccessRequestReceipts(receipts);
			}

			// Handle a failure. Because displaying the receipts is not critical to the application we just show an error
			// message rather than asking the user to authenticate themselves just to start the application up.
			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.e(TAG, "requestReceipts onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
				sReceiptCallback.onFailureRequestReceipts(errorCode, errorMessage, optionalData);
			}

			/**
			 * Handle the cancel event.
			 *
			 */
			@Override
			public void onCancel() {
				Log.i(TAG, "requestReceipts onCancel");
				sReceiptCallback.onCancelRequestReceipts();
			}
		};
	}
	
	// Process onActivityResult events that only happen for Activity
    @Override
    protected void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
        Log.i(TAG, "Processing activity result");
        
        if (null == sOuyaFacade) {
        	return;
        }

        // Forward this result to the facade, in case it is waiting for any activity results
        if (sOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
            return;
        }
    }
	
	protected void requestReceipts() {
		if (null == sRequestReceiptsListener) {
			Log.e(TAG, "requestReceipts: sRequestReceiptsListener is null");
			return;
		}
		sOuyaFacade.requestReceipts(this, sRequestReceiptsListener);
	}
	
	// override this method in the activity to show in the text status field
	protected void onSuccessRequestReceipts(Collection<Receipt> receipts) {
		Log.i(TAG, "onSuccessRequestReceipts");
	}

	// override this method in the activity to show in the text status field
	protected void onFailureRequestReceipts(int errorCode, String errorMessage, Bundle optionalData) {
		Log.i(TAG, "onFailureRequestReceipts");
	}
	
	protected void onCancelRequestReceipts() {
		Log.i(TAG, "onCancelRequestReceipts");
	}
	
	@Override
	public void onResume() {
		super.onResume();
		init();
	}
}
