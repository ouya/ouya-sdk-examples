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

import android.content.*;
import android.os.Bundle;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.io.UnsupportedEncodingException;
import java.security.*;
import java.security.spec.X509EncodedKeySpec;
import java.util.*;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import tv.ouya.console.api.*;
import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaMod;
import tv.ouya.console.api.content.OuyaModException;
import tv.ouya.console.api.OuyaFacade.DeviceHardware;

public class UnityOuyaFacade {
    /**
     * The tag for log messages
     */

    private static final String TAG = UnityOuyaFacade.class.getSimpleName();

	private static final boolean sEnableLogging = false;

    /**
     * Your game talks to the OuyaFacade, which hides all the mechanics of doing an in-app purchase.
     */
    private OuyaFacade mOuyaFacade = null;

    /**
     * The cryptographic key for this application
     */

    private PublicKey mPublicKey;

	//android context
	private Context context;

    // InitCompleteListener has initialized
    private static boolean sInitialized = false;

    // listener for init complete
    private CancelIgnoringOuyaResponseListener<Bundle> mInitCompleteListener = null;

	// listener for fetching gamer info
	private CancelIgnoringOuyaResponseListener<GamerInfo> mRequestGamerInfoListener = null;

	// listener for getting products
	private CancelIgnoringOuyaResponseListener<List<Product>> mRequestProductsListener = null;

	// listener for requesting purchase
	private OuyaResponseListener<PurchaseResult> mRequestPurchaseListener = null;

	// listener for getting receipts
	private OuyaResponseListener<Collection<Receipt>> mRequestReceiptsListener = null;

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

	public UnityOuyaFacade(Context context, Bundle savedInstanceState, Bundle developerInfo) {
		try {
			this.context = context;

			if (null == mOuyaFacade) {
				mOuyaFacade = OuyaFacade.getInstance();
			}

			Init(developerInfo);

			// Create a PublicKey object from the key data downloaded from the developer portal.
			try {
				X509EncodedKeySpec keySpec = new X509EncodedKeySpec(IOuyaActivity.GetApplicationKey());
				KeyFactory keyFactory = KeyFactory.getInstance("RSA");
				mPublicKey = keyFactory.generatePublic(keySpec);
			} catch (Exception e) {
				Log.e(TAG, "Unable to create encryption key", e);
			}
        }
		catch (Exception ex) {
            Log.e(TAG, "UnityOuyaFacade constructor exception", ex);
        }
	}

	private void Init(Bundle developerInfo) {
        
        mInitCompleteListener = new CancelIgnoringOuyaResponseListener<Bundle>() {
            @Override
            public void onSuccess(Bundle bundle) {
                if (sEnableLogging) {
                    Log.i(TAG, "InitCompleteListener onSuccess");
                }
                sInitialized = true;
                Log.i(TAG, "initOuyaPlugin: OuyaGameObject send OnSuccessInitializePlugin");
                UnityPlayer.UnitySendMessage("OuyaGameObject", "OnSuccessInitializePlugin", "");
            }

            @Override
            public void onFailure(int i, String s, Bundle bundle) {
                if (sEnableLogging) {
                    Log.i(TAG, "InitCompleteListener onFailure");
                }
                UnityPlayer.UnitySendMessage("OuyaGameObject", "OnFailureInitializePlugin", "InitCompleteListener onFailure");
            }
        };

        mOuyaFacade.registerInitCompletedListener(mInitCompleteListener);

		mRequestGamerInfoListener = new CancelIgnoringOuyaResponseListener<GamerInfo>() {
            @Override
            public void onSuccess(GamerInfo info) {
				if (sEnableLogging) {
					Log.i(TAG, "RequestGamerInfoListener onFailure");
				}

            	JSONObject json = new JSONObject();
				try {
					json.put("uuid", info.getUuid());
					json.put("username", info.getUsername());
				} catch (JSONException e1) {
				}
				String jsonData = json.toString();

				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestGamerInfoSuccessListener", jsonData);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				if (sEnableLogging) {
					Log.i(TAG, "RequestGamerInfoListener onFailure");
				}

				JSONObject json = new JSONObject();
				try {
					json.put("errorCode", errorCode);
					json.put("errorMessage", errorMessage);
				} catch (JSONException e1) {
				}
				String jsonData = json.toString();

				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestGamerInfoFailureListener", jsonData);
            }
        };

		mRequestProductsListener = new CancelIgnoringOuyaResponseListener<List<Product>>() {
			@Override
			public void onSuccess(final List<Product> products) {
				if (sEnableLogging) {
					Log.i(TAG, "RequestProductsListener onCancel");
				}

				if (products != null) {
					JSONArray jarray = new JSONArray();
					int index = 0;
					for (Product product : products) {
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
							++index;
						} catch (JSONException e1) {
						}
						String jsonData = jarray.toString();

						UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestProductsSuccessListener", jsonData);
					}
				}
			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				if (sEnableLogging) {
					Log.i(TAG, "RequestProductsListener onFailure");
				}

				JSONObject json = new JSONObject();
				try {
					json.put("errorCode", errorCode);
					json.put("errorMessage", errorMessage);
				} catch (JSONException e1) {
				}
				String jsonData = json.toString();

				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestProductsFailureListener", jsonData);
			}
		};

		mRequestPurchaseListener = new OuyaResponseListener<PurchaseResult>() {

			/**
			 * Handle a successful purchase.
			 *
			 * @param result The response from the server.
			 */
			@Override
			public void onSuccess(PurchaseResult result) {
				if (sEnableLogging) {
					Log.i(TAG, "RequestPurchaseListener onSuccess");
				}
				if (null != result) {
					JSONObject json = new JSONObject();
					try {
						json.put("identifier", result.getProductIdentifier());
					} catch (JSONException e) {
					}
					String jsonData = json.toString();

					UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestPurchaseSuccessListener", jsonData);
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
				if (sEnableLogging) {
					Log.i(TAG, "RequestPurchaseListener onFailure");
				}

				JSONObject json = new JSONObject();
				try {
					json.put("errorCode", errorCode);
					json.put("errorMessage", errorMessage);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();

				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestPurchaseFailureListener", jsonData);
			}

			/**
			 * Handle the cancel event.
			 *
			 */
			@Override
			public void onCancel() {
				if (sEnableLogging) {
					Log.i(TAG, "RequestPurchaseListener onCancel");
				}

				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestPurchaseCancelListener", "");
			}
		};

		mRequestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {

			/**
			 * Handle the successful fetching of the data for the receipts from the server.
			 */
			@Override
			public void onSuccess(Collection<Receipt> receipts) {
				if (sEnableLogging) {
					Log.i(TAG, "RequestReceiptsListener onSuccess");
				}

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
							++index;
						} catch (JSONException e1) {
						}
					}
					String jsonData = jarray.toString();

					//Log.i(TAG, "ReceiptListener ReceiptListListener jsonData=" + jsonData);
					UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestReceiptsSuccessListener", jsonData);
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
				if (sEnableLogging) {
					Log.i(TAG, "RequestReceiptsListener onFailure");
				}

				JSONObject json = new JSONObject();
				try {
					json.put("errorCode", errorCode);
					json.put("errorMessage", errorMessage);
				} catch (JSONException e1) {
				}
				String jsonData = json.toString();

				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestReceiptsFailureListener", jsonData);
			}

			/**
			 * Handle the cancel event.
			 *
			 */
			@Override
			public void onCancel() {
				if (sEnableLogging) {
					Log.i(TAG, "RequestReceiptsListener onCancel");
				}

				//Log.i(TAG, "PurchaseListener Invoke ReceiptListCancelListener");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "RequestReceiptsCancelListener", "");
			}
		};

        try {
            mOuyaFacade.init(context, developerInfo);
        } catch (Exception e) {
            e.printStackTrace();
        }

		mContent = OuyaContent.getInstance();
		IOuyaActivity.SetOuyaContent(mContent);

		mContentInitListener = new OuyaContent.InitializedListener() {

			@Override
			public void onDestroyed() {
				Log.i(TAG, "ContentInitListener: onDestroyed");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentInitListenerOnDestroyed", "");
			}

			@Override
			public void onInitialized() {
				Log.i(TAG, "ContentInitListener: onInitialized");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentInitListenerOnInitialized", "");
			}
			
		};

		mContent.init(context, mPublicKey);

		mContent.registerInitializedListener(mContentInitListener);

		mInstalledSearchListener = new OuyaContent.SearchListener() {
			@Override
			public void onError(int code, String reason) {
				Log.e(TAG, "InstalledSearchListener: onError code="+code+" reason="+reason);
				JSONObject json = new JSONObject();
				try {
					json.put("code", code);
					json.put("reason", reason);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentInstalledSearchListenerOnError", jsonData);
			}

			@Override
			public void onResults(List<OuyaMod> ouyaMods, int count) {
				//Log.i(TAG, "InstalledSearchListener: onResults count="+count+" list count="+ouyaMods.size());
				for (OuyaMod ouyaMod : ouyaMods) {
				}
				JSONObject json = new JSONObject();
				try {
					json.put("count", count);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentInstalledSearchListenerOnResults", jsonData);
				IOuyaActivity.SetOuyaContentInstalledResults(ouyaMods);
			}
		};
		
		mPublishedSearchListener = new OuyaContent.SearchListener() {
			@Override
			public void onError(int code, String reason) {
				Log.e(TAG, "PublishedSearchListener: onError code="+code+" reason="+reason);
				JSONObject json = new JSONObject();
				try {
					json.put("code", code);
					json.put("reason", reason);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentPublishedSearchListenerOnError", jsonData);
			}

			@Override
			public void onResults(List<OuyaMod> ouyaMods, int count) {
				//Log.i(TAG, "PublishedSearchListener: onResults count="+count+" list count="+ouyaMods.size());
				for (OuyaMod ouyaMod : ouyaMods) {
				}
				JSONObject json = new JSONObject();
				try {
					json.put("count", count);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentPublishedSearchListenerOnResults", jsonData);
				IOuyaActivity.SetOuyaContentPublishedResults(ouyaMods);
			}
		};
		
		mSaveListener = new OuyaContent.SaveListener() {

			@Override
			public void onError(OuyaMod ouyaMod, int code, String reason) {
				Log.e(TAG, "SaveListener: onError code="+code+" reason="+reason);
				JSONObject json = new JSONObject();
				try {
					json.put("code", code);
					json.put("reason", reason);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentSaveListenerOnError", jsonData);
			}

			@Override
			public void onSuccess(OuyaMod ouyaMod) {
				Log.i(TAG, "SaveListener: onSuccess");
				String jsonData = "";
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentSaveListenerOnSuccess", jsonData);
			}			
		};
		
	    mPublishListener = new OuyaContent.PublishListener() {

			@Override
			public void onError(OuyaMod ouyaMod, int code, String reason, Bundle bundle) {
				Log.e(TAG, "PublishListener: onError code="+code+" reason="+reason);
				JSONObject json = new JSONObject();
				try {
					json.put("code", code);
					json.put("reason", reason);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentPublishListenerOnError", jsonData);
			}

			@Override
			public void onSuccess(OuyaMod ouyaMod) {
				Log.i(TAG, "PublishListener: onSuccess");
				String jsonData = "";
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentPublishListenerOnSuccess", jsonData);
			}
	    	
	    };
	    
	    mUnpublishListener = new OuyaContent.UnpublishListener() {

			@Override
			public void onError(OuyaMod ouyaMod, int code, String reason) {
				Log.e(TAG, "UnpublishListener: onError code="+code+" reason="+reason);
				JSONObject json = new JSONObject();
				try {
					json.put("code", code);
					json.put("reason", reason);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentUnpublishListenerOnError", jsonData);
			}

			@Override
			public void onSuccess(OuyaMod ouyaMod) {
				Log.i(TAG, "UnpublishListener: onSuccess");
				String jsonData = "";
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentUnpublishListenerOnSuccess", jsonData);
			}
	    	
	    };
	    
	    mDeleteListener = new OuyaContent.DeleteListener() {

			@Override
			public void onDeleteFailed(OuyaMod ouyaMod, int code, String reason) {
				Log.e(TAG, "DeleteListener: onError code="+code+" reason="+reason);
				JSONObject json = new JSONObject();
				try {
					json.put("code", code);
					json.put("reason", reason);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentDeleteListenerOnDeleteFailed", jsonData);
			}

			@Override
			public void onDeleted(OuyaMod ouyaMod) {
				Log.i(TAG, "DeleteListener: onDeleted");
				String jsonData = "";
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentDeleteListenerOnDeleted", jsonData);
			}
	    	
	    };
	    
	    mDownloadListener = new OuyaContent.DownloadListener() {
			
			@Override
			public void onProgress(OuyaMod ouyaMod, int progress) {
				JSONObject json = new JSONObject();
				try {
					json.put("progress", progress);
				} catch (JSONException e) {
				}
				String jsonData = json.toString();
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentDownloadListenerOnProgress", jsonData);
			}
			
			@Override
			public void onFailed(OuyaMod ouyaMod) {
				String jsonData = "";
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentDownloadListenerOnFailed", jsonData);
			}
			
			@Override
			public void onComplete(OuyaMod ouyaMod) {
				String jsonData = "";
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ContentDownloadListenerOnComplete", jsonData);
			}
			
		};

	}

    /*
     * The IAP Facade registers a broadcast receiver with Android. You should take care to call shutdown(),
     * which unregisters the broadcast receiver, when you're done with the IAP Facade.
     */
    public void shutdown() {
		if (null != mOuyaFacade) {
			mOuyaFacade.shutdown();
		}
    }

	public boolean processActivityResult(final int requestCode, final int resultCode, final Intent data) {
		// Forward this result to the facade, in case it is waiting for any activity results
		if(mOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
			return true;
		} else {
			return false;
		}
	}

	public boolean isInitialized() {
        /*
		if (null == mOuyaFacade) {
			return false;
		} else {
			return mOuyaFacade.isInitialized();
		}
		*/

        return sInitialized;
	}

    /**
     * Get the list of products the user can purchase from the server.
     */
    public void requestProducts(ArrayList<Purchasable> products) {
		if (null != mRequestProductsListener) {
			mOuyaFacade.requestProductList(IOuyaActivity.GetActivity(), products, mRequestProductsListener);
		}
		else {
			Log.e(TAG, "mRequestProductsListener is null");
		}
    }

    public void requestGamerInfo() {

		if (null != mRequestGamerInfoListener) {
			mOuyaFacade.requestGamerInfo(IOuyaActivity.GetActivity(), mRequestGamerInfoListener);
		}
		else {
			Log.e(TAG, "UnityOuyaFacade.requestGamerInfo mRequestGamerInfoListener is null");
		}
    }

	public void putGameData(String key, String val) {
		mOuyaFacade.putGameData(key, val);
    }

	public String getGameData(String key) {
		return mOuyaFacade.getGameData(key);
    }

    /**
     * Request the receipts from the users previous purchases from the server.
     */

    public void requestReceipts() {
        if (sEnableLogging) {
            Log.i(TAG, "**************** requestReceipts");
        }
		if (null != mRequestReceiptsListener) {
			mOuyaFacade.requestReceipts(IOuyaActivity.GetActivity(), mRequestReceiptsListener);
		} else {
			Log.e(TAG, "mRequestReceiptsListener is null");
		}
    }

	public Boolean isRunningOnOUYASupportedHardware() {
		return mOuyaFacade.isRunningOnOUYASupportedHardware();
	}

    /*
     * This will be called when the user clicks on an item in the ListView.
     */
    public void requestPurchase(final Product product)
        throws GeneralSecurityException, UnsupportedEncodingException, JSONException {
        SecureRandom sr = SecureRandom.getInstance("SHA1PRNG");

		if (null != mRequestPurchaseListener) {
			//Log.i(TAG, "requestPurchase(" + product.getIdentifier() + ")");
			Purchasable purchasable = new Purchasable(product.getIdentifier());
			mOuyaFacade.requestPurchase(IOuyaActivity.GetActivity(), purchasable, mRequestPurchaseListener);
		} else {
			Log.e(TAG, "mRequestPurchaseListener is null");
		}
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
	
	public String getDeviceHardwareName() {
		if (!mOuyaFacade.isInitialized()) {
			Log.e(TAG, "OuyaFacade is not initialized!");
			return "";
		}
		DeviceHardware deviceHardware = mOuyaFacade.getDeviceHardware();
		if (null == deviceHardware) {
			return "";
		}		
		return deviceHardware.deviceName();
	}
}
