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

package tv.ouya.sdk.corona;

import android.accounts.AccountManager;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.*;
import android.os.Bundle;
import android.os.Parcelable;
import android.util.Base64;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ListView;
//import android.widget.Toast;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.security.*;
import java.security.spec.X509EncodedKeySpec;
import java.text.ParseException;
import java.util.*;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import tv.ouya.console.api.*;

public class CoronaOuyaFacade
{
    /**
     * The tag for log messages
     */

    private static final String LOG_TAG = "CoronaOuyaFacade";

    /**
     * Your game talks to the OuyaFacade, which hides all the mechanics of doing an in-app purchase.
     */
    private OuyaFacade ouyaFacade;

    /**
     * The cryptographic key for this application
     */

    private PublicKey mPublicKey;

	//android context
	private Context context;

	// listener for requesting gamer info
	CancelIgnoringOuyaResponseListener<GamerInfo> m_requestGamerInfoListener = null;

	// listener for getting products
	OuyaResponseListener<List<Product>> m_requestProductsListener = null;

	// listener for requesting purchases
	OuyaResponseListener<PurchaseResult> m_requestPurchaseListener = null;

	// listener for getting receipts
	OuyaResponseListener<Collection<Receipt>> m_requestReceiptsListener = null;

	public CoronaOuyaFacade(Context context, Bundle savedInstanceState, Bundle developerInfo)
	{
		try
		{
			this.context = context;

			ouyaFacade = OuyaFacade.getInstance();

			Init(developerInfo);

			// Create a PublicKey object from the key data downloaded from the developer portal.
			try {
				X509EncodedKeySpec keySpec = new X509EncodedKeySpec(IOuyaActivity.GetApplicationKey());
				KeyFactory keyFactory = KeyFactory.getInstance("RSA");
				mPublicKey = keyFactory.generatePublic(keySpec);
			} catch (Exception e) {
				Log.e(LOG_TAG, "Unable to create encryption key", e);
			}
        }
		catch (Exception ex)
		{
            Log.e(LOG_TAG, "Constructor exception", ex);
        }
	}

	public class ErrorResponse
	{
		public int errorCode = 0;
		public String errorMessage = "";
	}

	private void Init(Bundle developerInfo)
	{		
        ouyaFacade.init(context, developerInfo);

        m_requestGamerInfoListener = new CancelIgnoringOuyaResponseListener<GamerInfo>() {
            @Override
            public void onSuccess(GamerInfo info) {
				
            	JSONObject json = new JSONObject();
				try {
					json.put("uuid", info.getUuid());
					json.put("username", info.getUsername());
				} catch (JSONException e1) {
				}
				String jsonData = json.toString();
            	
				Log.i(LOG_TAG, "m_requestGamerInfoListener requestGamerUUIDSuccessListener=" + jsonData);
				
				IOuyaActivity.GetCallbacksRequestGamerInfo().onSuccess(jsonData);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.e(LOG_TAG, "Unable to request gamer info (error " + errorCode + ": " + errorMessage + ")");
				IOuyaActivity.GetCallbacksRequestGamerInfo().onFailure(errorCode, errorMessage);
            }
        };

		m_requestProductsListener = new OuyaResponseListener<List<Product>>()
		{
			@Override
			public void onSuccess(final List<Product> products) {

				//send each item in the list
				if (null != products) {
					
					JSONArray jarray = new JSONArray();
					int index = 0;
					for (Product product : products)
					{
						JSONObject json = new JSONObject();
						try {
							json.put("currencyCode", product.getCurrencyCode());
							json.put("description", product.getDescription());
							json.put("identifier", product.getIdentifier());
							json.put("localPrice", product.getLocalPrice());
							json.put("name", product.getName());
							json.put("originalPrice", product.getOriginalPrice());
							json.put("percentOff", product.getPercentOff());
							json.put("developerName", product.getDeveloperName());
							jarray.put(index, json);
						} catch (JSONException e1) {
						}
						++index;
					}
					
					String jsonData = jarray.toString();
					
					//Log.i(LOG_TAG, "m_productListListener ProductListListener jsonData=" + jsonData);
					IOuyaActivity.GetCallbacksRequestProducts().onSuccess(jsonData);
				} else {
					//send the complete message
					Log.i(LOG_TAG, "RequestProducts, no data to send");
					IOuyaActivity.GetCallbacksRequestProducts().onSuccess("");
				}

			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.i(LOG_TAG, "Unable to request products (error " + errorCode + ": " + errorMessage + ")");
				IOuyaActivity.GetCallbacksRequestProducts().onFailure(errorCode, errorMessage);
			}
			
			@Override
			public void onCancel() {
				IOuyaActivity.GetCallbacksRequestProducts().onCancel();
			}
		};

		m_requestPurchaseListener = new OuyaResponseListener<PurchaseResult>() {
	        /**
	         * Handle a successful purchase.
	         *
	         * @param result The response from the server.
	         */
	        @Override
	        public void onSuccess(PurchaseResult result)
			{
				if (null != result)
				{
					JSONObject json = new JSONObject();
					try {
						json.put("identifier", result.getProductIdentifier());
					} catch (JSONException e1) {
					}
					String jsonData = json.toString();

					Log.i(LOG_TAG, "PurchaseListener PurchaseSuccessListener jsonData=" + jsonData);
					IOuyaActivity.GetCallbacksRequestPurchase().onSuccess(jsonData);
				} else {
					Log.w(LOG_TAG, "PurchaseListener result is null");
				}
	        }

	        /**
	         * Handle an error. If the OUYA framework supplies an intent this means that the user needs to
	         * either authenticate or re-authenticate themselves, so we start the supplied intent.
	         *
	         * @param errorCode An HTTP error code between 0 and 999, if there was one. Otherwise, an internal error code from the
	         *                  Ouya server, documented in the {@link OuyaErrorCodes} class.
	         *
	         * @param errorMessage Empty for HTTP error codes. Otherwise, a brief, non-localized, explanation of the error.
	         *
	         * @param optionalData A Map of optional key/value pairs which provide additional information.
	         */

	        @Override
	        public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				IOuyaActivity.GetCallbacksRequestPurchase().onFailure(errorCode, errorMessage);    
	        }

	        /**
	         * Handle the cancel event.
	         *
	         */
	        @Override
	        public void onCancel()
			{
				//showError("Purchase was cancelled");

				Log.i(LOG_TAG, "PurchaseListener Invoke PurchaseCancelListener");
				IOuyaActivity.GetCallbacksRequestPurchase().onCancel();
			}
	    };

	    m_requestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {
	    	/**
	         * Handle the successful fetching of the data for the receipts from the server.
	         *
	         * @param receiptResponse The response from the server.
	         */
	        @Override
	        public void onSuccess(Collection<Receipt> receipts)
			{
				// custom-iap-code
				if(receipts != null) {
					
					JSONArray jarray = new JSONArray();
					int index = 0;
					for (Receipt receipt : receipts)
					{
						JSONObject json = new JSONObject();
						try {
							json.put("identifier", receipt.getIdentifier());
							json.put("purchaseDate", receipt.getPurchaseDate());
							json.put("gamer", receipt.getGamer());
							json.put("uuid", receipt.getUuid());
							json.put("localPrice", receipt.getLocalPrice());
							json.put("currency", receipt.getCurrency());
							json.put("generatedDate", receipt.getGeneratedDate());
							jarray.put(index, json);
						} catch (JSONException e1) {
						}
						++index;
					}
					
					String jsonData = jarray.toString();
					
					Log.i(LOG_TAG, "m_requestReceiptsListener onSuccess jsonData=" + jsonData);
					IOuyaActivity.GetCallbacksRequestReceipts().onSuccess(jsonData);
				} else {
					Log.i(LOG_TAG, "m_requestReceiptsListener onSuccess jsonData=(empty)");
					IOuyaActivity.GetCallbacksRequestReceipts().onSuccess("");
				}
	        }

	        /**
	         * Handle a failure. Because displaying the receipts is not critical to the application we just show an error
	         * message rather than asking the user to authenticate themselves just to start the application up.
	         *
	         * @param errorCode An HTTP error code between 0 and 999, if there was one. Otherwise, an internal error code from the
	         *                  Ouya server, documented in the {@link OuyaErrorCodes} class.
	         *
	         * @param errorMessage Empty for HTTP error codes. Otherwise, a brief, non-localized, explanation of the error.
	         *
	         * @param optionalData A Map of optional key/value pairs which provide additional information.
	         */

	        @Override
	        public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
	            //Log.w(LOG_TAG, "Request Receipts error (code " + errorCode + ": " + errorMessage + ")");
	            IOuyaActivity.GetCallbacksRequestReceipts().onFailure(errorCode, errorMessage);
	        }

	        /**
	         * Handle the cancel event.
	         *
	         */
	        @Override
	        public void onCancel()
			{
				//Log.i(LOG_TAG, "ReceiptListener Invoke ReceiptListCancelListener");
				IOuyaActivity.GetCallbacksRequestReceipts().onCancel();
			}
	    };
	}

    /*
     * The IAP Facade registers a broadcast receiver with Android. You should take care to call shutdown(),
     * which unregisters the broadcast receiver, when you're done with the IAP Facade.
     */
    public void onDestroy() {
        ouyaFacade.shutdown();
    }
    
    public boolean processActivityResult(final int requestCode, final int resultCode, final Intent data) {
    	// Forward this result to the facade, in case it is waiting for any activity results
    	if(ouyaFacade.processActivityResult(requestCode, resultCode, data)) {
    		return true;
    	} else {
    		return false;
    	}
    }

    /**
     * Get the list of products the user can purchase from the server.
     */
    public synchronized void requestProducts(ArrayList<Purchasable> products) {
		if (null != m_requestProductsListener)
		{
			ouyaFacade.requestProductList(IOuyaActivity.GetActivity(), products, m_requestProductsListener);
		}
		else
		{
			Log.e(LOG_TAG, "m_requestProductsListener is null");
		}
    }

    public void requestGamerInfo() {
		if (null != m_requestGamerInfoListener)
		{
			Log.i(LOG_TAG, "requestGamerInfo m_requestGamerInfoListener is valid");
			ouyaFacade.requestGamerInfo(IOuyaActivity.GetActivity(), m_requestGamerInfoListener);
		}
		else
		{
			Log.e(LOG_TAG, "m_requestGamerInfoListener is null");
		}        
    }

    /**
     * Request the receipts from the users previous purchases from the server.
     */

    public void requestReceipts() {
    	if (null != m_requestReceiptsListener) {
        	ouyaFacade.requestReceipts(IOuyaActivity.GetActivity(), m_requestReceiptsListener);
        } else {
        	Log.e(LOG_TAG, "m_requestReceiptsListener is null");
	    }
    }

    /*
     * This will be called when the user clicks on an item in the ListView.
     */
    public void requestPurchase(final Product product)
        throws GeneralSecurityException, UnsupportedEncodingException, JSONException {
        
        if (null != m_requestPurchaseListener) {
	        Purchasable purchasable = new Purchasable(product.getIdentifier());
			Log.i(LOG_TAG, "requestPurchase(" + product.getIdentifier() + ")");
			ouyaFacade.requestPurchase(IOuyaActivity.GetActivity(), purchasable, m_requestPurchaseListener);
		} else {
			Log.e(LOG_TAG, "m_requestPurchaseListener is null");
		}
    }
}
