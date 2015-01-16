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
import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaMod;
import tv.ouya.console.api.content.OuyaModException;
import tv.ouya.console.api.content.OuyaModScreenshot;

public class UnrealOuyaFacade
{
    /**
     * The tag for log messages
     */

    private static final String TAG = UnrealOuyaFacade.class.getSimpleName();

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
	OuyaResponseListener<List<Product>> m_productListListener = null;

	// listener for requesting purchases
	OuyaResponseListener<PurchaseResult> m_purchaseListener = null;

	// Content interface for community content
	private OuyaContent mContent;

	// listener for initializing community content
	private OuyaContent.InitializedListener mContentInitListener = null;

	// listener for looking for installed content
	private OuyaContent.SearchListener mInstalledSearchListener = null;

	// listener for looking for published content
	private OuyaContent.SearchListener mPublishedSearchListener = null;
	
	// listener for saving content
	private OuyaContent.SaveListener mSaveListener;

	// listener for publishing content
    private OuyaContent.PublishListener mPublishListener;

	// listener for unpublishing content
    private OuyaContent.UnpublishListener mUnpublishListener;

	// listener for downloading content
    private OuyaContent.DownloadListener mDownloadListener;

	// listener for deleting content
    private OuyaContent.DeleteListener mDeleteListener;

	public UnrealOuyaFacade(Context context, Bundle savedInstanceState, Bundle developerInfo)
	{
		try
		{
			this.context = context;

			//Log.i(TAG, "Init(" + developerId + ");");

			ouyaFacade = OuyaFacade.getInstance();

			Init(developerInfo);

			// Create a PublicKey object from the key data downloaded from the developer portal.
			try {
				X509EncodedKeySpec keySpec = new X509EncodedKeySpec(IUnrealOuyaActivity.GetApplicationKey());
				KeyFactory keyFactory = KeyFactory.getInstance("RSA");
				mPublicKey = keyFactory.generatePublic(keySpec);
			} catch (Exception e) {
				Log.e(TAG, "Unable to create encryption key", e);
			}
        }
		catch (Exception ex)
		{
            Log.e(TAG, "Constructor exception", ex);
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

				Log.i(TAG, "m_requestGamerInfoListener requestGamerUUIDSuccessListener=" + jsonData);

				IUnrealOuyaActivity.GetCallbacksRequestGamerInfo().onSuccess(jsonData);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				Log.i(TAG, "Unable to request gamer info (error " + errorCode + ": " + errorMessage + ")");
				IUnrealOuyaActivity.GetCallbacksRequestGamerInfo().onFailure(errorCode, errorMessage);
            }
        };

		m_productListListener = new OuyaResponseListener<List<Product>>()
		{
			@Override
			public void onSuccess(final List<Product> products) {

				// clear the old list
				Log.i(TAG, "m_productListListener ProductListListener");

				//send each item in the list
				if (null != products) {

					JSONArray jarray = new JSONArray();
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
							jarray.put(products.indexOf(product), json);
						} catch (JSONException e1) {
						}
					}

					String jsonData = jarray.toString();

					Log.i(TAG, "m_productListListener ProductListListener jsonData=" + jsonData);
					IUnrealOuyaActivity.GetCallbacksRequestProducts().onSuccess(jsonData);
				} else {
					//send the complete message
					Log.i(TAG, "RequestProducts, no data to send");
					IUnrealOuyaActivity.GetCallbacksRequestProducts().onSuccess("");
				}

			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {

				Log.i(TAG, "Unable to request products (error " + errorCode + ": " + errorMessage + ")");
				IUnrealOuyaActivity.GetCallbacksRequestProducts().onFailure(errorCode, errorMessage);
			}

			@Override
			public void onCancel() {
				IUnrealOuyaActivity.GetCallbacksRequestProducts().onCancel();
			}
		};

		m_purchaseListener = new OuyaResponseListener<PurchaseResult>() {

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

					Log.i(TAG, "PurchaseListener PurchaseSuccessListener jsonData=" + jsonData);
					IUnrealOuyaActivity.GetCallbacksRequestPurchase().onSuccess(jsonData);
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
				IUnrealOuyaActivity.GetCallbacksRequestPurchase().onFailure(errorCode, errorMessage);
			}

			/**
			 * Handle the cancel event.
			 *
			 */
			@Override
			public void onCancel()
			{
				//showError("Purchase was cancelled");

				Log.i(TAG, "PurchaseListener Invoke PurchaseCancelListener");
				IUnrealOuyaActivity.GetCallbacksRequestPurchase().onCancel();
			}
		};
	}

	public void initContent() {

		mContentInitListener = new OuyaContent.InitializedListener() {

			@Override
			public void onDestroyed() {
				Log.i(TAG, "ContentInitListener: onDestroyed");
				IUnrealOuyaActivity.GetCallbacksContentInit().onDestroyed();
			}

			@Override
			public void onInitialized() {
				Log.i(TAG, "ContentInitListener: onInitialized");
				IUnrealOuyaActivity.GetCallbacksContentInit().onInitialized();
			}
			
		};

		Log.i(TAG, "Init: OuyaContent.getInstance()");
		mContent = OuyaContent.getInstance();

		Log.i(TAG, "Init: Save OuyaContent reference");
		IUnrealOuyaActivity.SetOuyaContent(mContent);

		mContent.init(context, mPublicKey);
		
		mContent.registerInitializedListener(mContentInitListener);

		mInstalledSearchListener = new OuyaContent.SearchListener() {
			@Override
			public void onError(int code, String reason) {
				Log.e(TAG, "InstalledSearchListener: onError code="+code+" reason="+reason);
				IUnrealOuyaActivity.GetCallbacksContentSearchInstalled().onError(code, reason);
			}

			@Override
			public void onResults(List<OuyaMod> ouyaMods, int count) {
				Log.i(TAG, "InstalledSearchListener: onResults count="+count+" list count="+ouyaMods.size());
				IUnrealOuyaActivity.GetCallbacksContentSearchInstalled().onResults(ouyaMods.toArray(new OuyaMod[ouyaMods.size()]), count);
			}
		};
		
		mPublishedSearchListener = new OuyaContent.SearchListener() {
			@Override
			public void onError(int code, String reason) {
				Log.e(TAG, "PublishedSearchListener: onError code="+code+" reason="+reason);
				IUnrealOuyaActivity.GetCallbacksContentSearchPublished().onError(code, reason);				
			}

			@Override
			public void onResults(List<OuyaMod> ouyaMods, int count) {
				Log.i(TAG, "PublishedSearchListener: onResults count="+count+" list count="+ouyaMods.size());
				IUnrealOuyaActivity.GetCallbacksContentSearchPublished().onResults(ouyaMods.toArray(new OuyaMod[ouyaMods.size()]), count);				
			}
		};
		
		mSaveListener = new OuyaContent.SaveListener() {

			@Override
			public void onError(OuyaMod ouyaMod, int code, String reason) {
				Log.e(TAG, "SaveListener: onError code="+code+" reason="+reason);
				IUnrealOuyaActivity.GetCallbacksContentSave().onError(ouyaMod, code, reason);
			}

			@Override
			public void onSuccess(OuyaMod ouyaMod) {
				Log.i(TAG, "SaveListener: onSuccess");
				IUnrealOuyaActivity.GetCallbacksContentSave().onSuccess(ouyaMod);
			}			
		};
		
	    mPublishListener = new OuyaContent.PublishListener() {

			@Override
			public void onError(OuyaMod ouyaMod, int code, String reason, Bundle bundle) {
				Log.e(TAG, "PublishListener: onError code="+code+" reason="+reason);
				IUnrealOuyaActivity.GetCallbacksContentPublish().onError(ouyaMod, code, reason, bundle);				
			}

			@Override
			public void onSuccess(OuyaMod ouyaMod) {
				Log.i(TAG, "PublishListener: onSuccess");
				IUnrealOuyaActivity.GetCallbacksContentPublish().onSuccess(ouyaMod);
			}
	    	
	    };
	    
	    mUnpublishListener = new OuyaContent.UnpublishListener() {

			@Override
			public void onError(OuyaMod ouyaMod, int code, String reason) {
				Log.e(TAG, "UnpublishListener: onError code="+code+" reason="+reason);
				IUnrealOuyaActivity.GetCallbacksContentUnpublish().onError(ouyaMod, code, reason);
			}

			@Override
			public void onSuccess(OuyaMod ouyaMod) {
				Log.i(TAG, "UnpublishListener: onSuccess");
				IUnrealOuyaActivity.GetCallbacksContentUnpublish().onSuccess(ouyaMod);
			}
	    	
	    };
	    
	    mDeleteListener = new OuyaContent.DeleteListener() {

			@Override
			public void onDeleteFailed(OuyaMod ouyaMod, int code, String reason) {
				Log.e(TAG, "DeleteListener: onError code="+code+" reason="+reason);
				IUnrealOuyaActivity.GetCallbacksContentDelete().onDeleteFailed(ouyaMod, code, reason);
			}

			@Override
			public void onDeleted(OuyaMod ouyaMod) {
				Log.i(TAG, "DeleteListener: onDeleted");
				IUnrealOuyaActivity.GetCallbacksContentDelete().onDeleted(ouyaMod);
			}
	    	
	    };
	    
	    mDownloadListener = new OuyaContent.DownloadListener() {
			
			@Override
			public void onProgress(OuyaMod ouyaMod, int progress) {
				IUnrealOuyaActivity.GetCallbacksContentDownload().onProgress(ouyaMod, progress);				
			}
			
			@Override
			public void onFailed(OuyaMod ouyaMod) {
				IUnrealOuyaActivity.GetCallbacksContentDownload().onFailed(ouyaMod);
			}
			
			@Override
			public void onComplete(OuyaMod ouyaMod) {
				IUnrealOuyaActivity.GetCallbacksContentDownload().onComplete(ouyaMod);
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
    public void requestProducts(ArrayList<Purchasable> products) {
		//custom-iap-code
		if (null != m_productListListener)
		{
			Log.i(TAG, "requestProducts m_productListListener is valid");
			ouyaFacade.requestProductList(IUnrealOuyaActivity.GetActivity(), products, m_productListListener);
		}
		else
		{
			Log.i(TAG, "requestProducts m_productListListener is null");
		}
    }

    public void requestGamerInfo() {

		//custom-iap-code
		if (null != m_requestGamerInfoListener)
		{
			Log.i(TAG, "requestGamerInfo m_requestGamerInfoListener is valid");
			ouyaFacade.requestGamerInfo(IUnrealOuyaActivity.GetActivity(), m_requestGamerInfoListener);
		}
		else
		{
			Log.i(TAG, "requestGamerInfo m_requestGamerInfoListener is null");
		}
    }

    /**
     * Request the receipts from the users previous purchases from the server.
     */

    public void requestReceipts() {
        ouyaFacade.requestReceipts(IUnrealOuyaActivity.GetActivity(), new ReceiptListener());
    }

    /*
     * This will be called when the user clicks on an item in the ListView.
     */
    public void requestPurchase(final Product product)
        throws GeneralSecurityException, UnsupportedEncodingException, JSONException {

		Log.i(TAG, "requestPurchase(" + product.getIdentifier() + ")");
		Purchasable purchasable = new Purchasable(product.getIdentifier());
		ouyaFacade.requestPurchase(IUnrealOuyaActivity.GetActivity(), purchasable, m_purchaseListener);
    }

    /**
     * The callback for when the list of user receipts has been requested.
     */
    private class ReceiptListener implements OuyaResponseListener<Collection<Receipt>> {

        /**
         * Handle the successful fetching of the data for the receipts from the server.
         *
         * @param receiptResponse The response from the server.
         */
        @Override
        public void onSuccess(Collection<Receipt> receipts)
		{
            if (receipts != null) {

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
						++index;
					} catch (JSONException e1) {
					}
				}

				String jsonData = jarray.toString();

				Log.i(TAG, "ReceiptListener ReceiptListListener jsonData=" + jsonData);
				IUnrealOuyaActivity.GetCallbacksRequestReceipts().onSuccess(jsonData);
			} else {
				Log.i(TAG, "ReceiptListener ReceiptListListener jsonData=(empty)");
				IUnrealOuyaActivity.GetCallbacksRequestReceipts().onSuccess("");
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
            Log.w(TAG, "Request Receipts error (code " + errorCode + ": " + errorMessage + ")");
            IUnrealOuyaActivity.GetCallbacksRequestReceipts().onFailure(errorCode, errorMessage);
        }

        /**
         * Handle the cancel event.
         *
         */
        @Override
        public void onCancel()
		{
			Log.i(TAG, "ReceiptListener Invoke ReceiptListCancelListener");
			IUnrealOuyaActivity.GetCallbacksRequestReceipts().onCancel();
		}
    }
    
    public Boolean isRunningOnOUYASupportedHardware() {
		return ouyaFacade.isRunningOnOUYASupportedHardware();
	}
    
    public void saveOuyaMod(OuyaMod ouyaMod, OuyaMod.Editor editor) {
    	
    	try {
            editor.save(mSaveListener);
        } catch(OuyaModException e) {
            switch(e.getCode()) {
                case OuyaModException.CONTENT_NO_TITLE:
                	mSaveListener.onError(ouyaMod, e.getCode(), "Title required!");
                    break;
                case OuyaModException.CONTENT_NO_CATEGORY:
                	mSaveListener.onError(ouyaMod, e.getCode(), "Category required!");
                    break;
                case OuyaModException.CONTENT_NO_SCREENSHOTS:
                	mSaveListener.onError(ouyaMod, e.getCode(), "At least one screenshot required!");
                    break;
                case OuyaModException.CONTENT_NO_FILES:
                	mSaveListener.onError(ouyaMod, e.getCode(), "At least one file required!");
                    break;
                case OuyaModException.CONTENT_NOT_EDITABLE:
                	mSaveListener.onError(ouyaMod, e.getCode(), "OuyaMod is not editable!");
                    break;
                default:
                	mSaveListener.onError(ouyaMod, e.getCode(), "Save Exception!");
                	break;
            }
        }
    }
    
	public void getOuyaContentInstalled() {
		if (null == mContent) {
			Log.e(TAG, "mContent is null");
		} else if (null == mInstalledSearchListener) {
			Log.e(TAG, "mInstalledSearchListener is null");
		} else {
			mContent.getInstalled(mInstalledSearchListener);
		}
	}

	public void getOuyaContentPublished(final OuyaContent.SortMethod sortMethod) {
		if (null == mContent) {
			Log.e(TAG, "mContent is null");
		} else if (null == mPublishedSearchListener) {
			Log.e(TAG, "mPublishedSearchListener is null");
		} else {
			mContent.search(sortMethod, mPublishedSearchListener);
		}
	}

	public void contentDelete(final OuyaMod ouyaMod) {
		if (null == mDeleteListener) {
			Log.e(TAG, "mDeleteListener is null");
		} else {
			ouyaMod.delete(mDeleteListener);
		}

	}

	public void contentPublish(final OuyaMod ouyaMod) {
		if (null == mPublishListener) {
			Log.e(TAG, "mPublishListener is null");
		} else {
			ouyaMod.publish(mPublishListener);
		}
	}

	public void contentUnpublish(final OuyaMod ouyaMod) {
		if (null == mUnpublishListener) {
			Log.e(TAG, "mUnpublishListener is null");
		} else {
			ouyaMod.unpublish(mUnpublishListener);
		}
	}

	public void contentDownload(final OuyaMod ouyaMod) {
		if (null == mDownloadListener) {
			Log.e(TAG, "mDownloadListener is null");
		} else {
			ouyaMod.download(mDownloadListener);
		}
	}
}