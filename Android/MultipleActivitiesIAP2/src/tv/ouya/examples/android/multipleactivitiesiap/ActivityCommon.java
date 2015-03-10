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

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import tv.ouya.console.api.*;

// Common code used by both activities
// Isolate the purchase IAP code here
public class ActivityCommon extends Activity
{
	// The tag for log messages
	private static final String TAG = ActivityCommon.class.getSimpleName();

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
	
	// the active callback listener
	public static ActivityCommon sCallbacks = null;
	
	private void init() {
		
		sCallbacks = this;
		
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
        developerInfo.putString(OuyaFacade.XIAOMI_APPLICATION_ID, "0000000000000");
        developerInfo.putString(OuyaFacade.XIAOMI_APPLICATION_KEY, "000000000000000000");
        
        String[] products = new String[] {
            "sharp_axe",
        };

        developerInfo.putStringArray(OuyaFacade.OUYA_PRODUCT_ID_LIST, products);
        
		sOuyaFacade = OuyaFacade.getInstance();
		sOuyaFacade.init(context, developerInfo);
		
		sRequestGamerInfoListener = new CancelIgnoringOuyaResponseListener<GamerInfo>() {
            @Override
            public void onSuccess(GamerInfo info) {
            	Log.i(TAG, "sRequestGamerInfoListener: onSuccess");
            	sCallbacks.onSuccessRequestGamerInfo(info);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
            	Log.i(TAG, "sRequestGamerInfoListener: onFailure errorCode="+errorCode+" errorMessage="+errorMessage);
            	sCallbacks.onFailureRequestGamerInfo(errorCode, errorMessage, optionalData);
            }
        };
        
		sRequestProductsListener = new CancelIgnoringOuyaResponseListener<List<Product>>() {
			@Override
			public void onSuccess(final List<Product> products) {
				Log.i(TAG, "sRequestProductsListener: onSuccess");
				sCallbacks.onSuccessRequestProducts(products);
			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.i(TAG, "sRequestProductsListener: onFailure errorCode="+errorCode+" errorMessage="+errorMessage);
				sCallbacks.onFailureRequestProducts(errorCode, errorMessage, optionalData);
			}
		};

		sRequestPurchaseListener = new OuyaResponseListener<PurchaseResult>() {

			@Override
			public void onSuccess(PurchaseResult result) {
				Log.i(TAG, "sRequestPurchaseListener: onSuccess");
				sCallbacks.onSuccessRequestPurchase(result);
			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.i(TAG, "sRequestPurchaseListener: onFailure errorCode="+errorCode+" errorMessage="+errorMessage);
				sCallbacks.onFailureRequestPurchase(errorCode, errorMessage, optionalData);
			}

			@Override
			public void onCancel() {
				Log.i(TAG, "sRequestPurchaseListener: onCancel");
				sCallbacks.onCancelRequestPurchase();
			}
		};
		
		sRequestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {

			@Override
			public void onSuccess(Collection<Receipt> receipts) {
				Log.i(TAG, "requestReceipts onSuccess: received "+receipts.size() + " receipts");
				sCallbacks.onSuccessRequestReceipts(receipts);
			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.e(TAG, "requestReceipts onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
				sCallbacks.onFailureRequestReceipts(errorCode, errorMessage, optionalData);
			}

			@Override
			public void onCancel() {
				Log.i(TAG, "requestReceipts onCancel");
				sCallbacks.onCancelRequestReceipts();
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
    
    protected void requestGamerInfo() {
		if (null == sRequestGamerInfoListener) {
			Log.e(TAG, "requestGamerInfo: sRequestGamerInfoListener is null");
			return;
		}
		sOuyaFacade.requestGamerInfo(this, sRequestGamerInfoListener);
	}
    
    protected void requestProducts() {
		if (null == sRequestProductsListener) {
			Log.e(TAG, "requestProducts: sRequestProductsListener is null");
			return;
		}
		ArrayList<Purchasable> products = new ArrayList<Purchasable>();
		Purchasable purchasable = new Purchasable("kt_game_000");
		products.add(purchasable);
		sOuyaFacade.requestProductList(this, products, sRequestProductsListener);
	}
    
	protected void requestPurchase() {
		if (null == sRequestPurchaseListener) {
			Log.e(TAG, "requestPurchase: sRequestPurchaseListener is null");
			return;
		}
		Purchasable purchasable = new Purchasable("kt_game_000");
		sOuyaFacade.requestPurchase(this, purchasable, sRequestPurchaseListener);
	}
	
	protected void requestReceipts() {
		if (null == sRequestReceiptsListener) {
			Log.e(TAG, "requestReceipts: sRequestReceiptsListener is null");
			return;
		}
		sOuyaFacade.requestReceipts(this, sRequestReceiptsListener);
	}
	
	// override this method in the activity to show in the text status field
	protected void onSuccessRequestGamerInfo(GamerInfo info) {
		Log.i(TAG, "onSuccessRequestGamerInfo");
	}

	// override this method in the activity to show in the text status field
	protected void onFailureRequestGamerInfo(int errorCode, String errorMessage, Bundle optionalData) {
		Log.i(TAG, "onFailureRequestGamerInfo");
	}
	
	// override this method in the activity to show in the text status field
	protected void onSuccessRequestProducts(final List<Product> products) {
		Log.i(TAG, "onSuccessRequestProducts");
	}

	// override this method in the activity to show in the text status field
	protected void onFailureRequestProducts(int errorCode, String errorMessage, Bundle optionalData) {
		Log.i(TAG, "onFailureRequestProducts");
	}

	// override this method in the activity to show in the text status field
	protected void onSuccessRequestPurchase(PurchaseResult result) {
		Log.i(TAG, "onSuccessRequestPurchase");
	}

	// override this method in the activity to show in the text status field
	protected void onFailureRequestPurchase(int errorCode, String errorMessage, Bundle optionalData) {
		Log.i(TAG, "onFailureRequestPurchase");
	}
	
	// override this method in the activity to show in the text status field
	protected void onCancelRequestPurchase() {
		Log.i(TAG, "onCancelRequestPurchase");
	}
	
	// override this method in the activity to show in the text status field
	protected void onSuccessRequestReceipts(Collection<Receipt> receipts) {
		Log.i(TAG, "onSuccessRequestReceipts");
	}

	// override this method in the activity to show in the text status field
	protected void onFailureRequestReceipts(int errorCode, String errorMessage, Bundle optionalData) {
		Log.i(TAG, "onFailureRequestReceipts");
	}
	
	// override this method in the activity to show in the text status field
	protected void onCancelRequestReceipts() {
		Log.i(TAG, "onCancelRequestReceipts");
	}
	
	@Override
	public void onResume() {
		super.onResume();
		init();
	}
}
