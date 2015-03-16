package ${YYAndroidPackageName};

import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.widget.FrameLayout;
import com.yoyogames.runner.RunnerJNILib;
import java.io.IOException;
import java.io.InputStream;
import java.lang.String;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import tv.ouya.console.api.*;
import tv.ouya.sdk.OuyaInputView;

import ${YYAndroidPackageName}.R;
import ${YYAndroidPackageName}.RunnerActivity;

public class OuyaSDK extends RunnerSocial {
	private static final String TAG = OuyaSDK.class.getSimpleName();
	
	private static final boolean sEnableLogging = false;
	
	private static final String sFalse = "false";
	private static final String sTrue = "true";
	private static final double sNoop = 0;
	
	private static boolean sInitialized = false;
	
	private static OuyaInputView sInputView = null;
	
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
	
	// GameMakers async id
	private static ArrayList<String> sAsyncResults = new ArrayList<String>();
	
	@Override
	public void onResume() {
		Log.i(TAG, "onResume called in MyExtensionClass extension");
	}

	@Override
	public void Init() {
		Log.i(TAG, "Overridden init called for MyExtensionClass");
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		
		/*
		if (sEnableLogging) {
			Log.i(TAG, "onActivityResult called in MyExtensionClass extension");
		}
		*/

		if (null == sOuyaFacade) {
        	return;
        }
		
        // Forward this result to the facade, in case it is waiting for any activity results
        if (sOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
            return;
        }
	}
	
	public String init(final String jsonData) {
		
		if (sEnableLogging) {
			Log.i(TAG, "init: jsonData="+jsonData);
		}
		
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "Current activity is null");
			return sFalse;
		} else {
			Log.d(TAG, "Current activity is valid");
		}
		
		final FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
        if (null == content) {
			Log.d(TAG, "Content is null");
			return sFalse;
		} else {
			Runnable runnable = new Runnable() {
				public void run() {
					Log.d(TAG, "Disable screensaver");
					content.setKeepScreenOn(true);
					
					Log.d(TAG, "Add inputView");
					sInputView = new OuyaInputView(activity);
					
					byte[] applicationKey = null;
					
					// load the application key from assets
					try {
						AssetManager assetManager = activity.getAssets();
						InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
						applicationKey = new byte[inputStream.available()];
						inputStream.read(applicationKey);
						inputStream.close();
					} catch (IOException e) {
						e.printStackTrace();
					}
					
					if (null == applicationKey) {
						Log.e(TAG, "Failed to load signing key");
						activity.finish();
					}
					
					if (sEnableLogging) {
						Log.i(TAG, "Signing key loaded!");
					}
					
					Bundle developerInfo = new Bundle();		
					
					developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, applicationKey);
					
					try {
						JSONObject jObject = new JSONObject(jsonData);
						Iterator<?> keys = jObject.keys();
						while ( keys.hasNext() ) {
							String name = (String)keys.next();
							if (null == name) {
								continue;
							}
							String value = (String)jObject.get(name);
							if (null == value) {
								continue;
							}
							if (name.equals("tv.ouya.product_id_list")) {
								String[] productIds = value.split(",");
								if (null == productIds) {
									continue;
								}
								developerInfo.putStringArray("tv.ouya.product_id_list", productIds);
							} else {
								if (sEnableLogging) {
									Log.i(TAG, "************* name="+name+" value="+value);
								}
								developerInfo.putString(name, value);
							}
						}
					} catch (JSONException e) {
						e.printStackTrace();
						Log.e(TAG, "Failed to load init plugin values");
						activity.finish();
					}
					
					sOuyaFacade = OuyaFacade.getInstance();
					sOuyaFacade.init(activity, developerInfo);
					
					sRequestGamerInfoListener = new CancelIgnoringOuyaResponseListener<GamerInfo>() {
						@Override
						public void onSuccess(GamerInfo info) {
							if (null == info) {
								Log.e(TAG, "GamerInfo is null!");
								return;
							}
							if (sEnableLogging) {
								Log.i(TAG, "sRequestGamerInfoListener: onSuccess uuid="+info.getUuid()+" username="+info.getUsername());
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onSuccessRequestGamerInfo");
								JSONObject data = new JSONObject();
								data.put("uuid", info.getUuid());
								data.put("username", info.getUsername());
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}

						@Override
						public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
							if (sEnableLogging) {
								Log.e(TAG, "sRequestGamerInfoListener: onFailure errorCode="+errorCode+" errorMessage="+errorMessage);
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onFailureRequestGamerInfo");
								JSONObject data = new JSONObject();
								data.put("errorCode", Integer.toString(errorCode));
								data.put("errorMessage", errorMessage);
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}
					};
					
					sRequestProductsListener = new CancelIgnoringOuyaResponseListener<List<Product>>() {
						@Override
						public void onSuccess(final List<Product> products) {
							if (null == products) {
								Log.e(TAG, "Products are null!");
								return;
							}
							if (sEnableLogging) {
								Log.i(TAG, "sRequestProductsListener: onSuccess");
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onSuccessRequestProducts");
								JSONArray data = new JSONArray();
								int index = 0;
								for (Product product : products) {
									JSONObject item = new JSONObject();
									try {
										item.put("currencyCode", product.getCurrencyCode());
										item.put("description", product.getDescription());
										item.put("identifier", product.getIdentifier());
										item.put("localPrice", product.getLocalPrice());
										item.put("name", product.getName());
										item.put("originalPrice", product.getOriginalPrice());
										item.put("percentOff", product.getPercentOff());
										item.put("developerName", product.getDeveloperName());
										data.put(index, item);
										++index;
									} catch (JSONException e2) {
									}
								}
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}

						@Override
						public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
							if (sEnableLogging) {
								Log.e(TAG, "sRequestProductsListener: onFailure errorCode="+errorCode+" errorMessage="+errorMessage);
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onFailureRequestProducts");
								JSONObject data = new JSONObject();
								data.put("errorCode", Integer.toString(errorCode));
								data.put("errorMessage", errorMessage);
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}
					};

					sRequestPurchaseListener = new OuyaResponseListener<PurchaseResult>() {

						@Override
						public void onSuccess(PurchaseResult result) {
							if (null == result) {
								Log.e(TAG, "PurchaseResult is null!");
								return;
							}
							if (sEnableLogging) {
								Log.i(TAG, "sRequestPurchaseListener: onSuccess");
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onSuccessRequestPurchase");
								JSONObject data = new JSONObject();
								data.put("identifier", result.getProductIdentifier());
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}

						@Override
						public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
							if (sEnableLogging) {
								Log.e(TAG, "sRequestPurchaseListener: onFailure errorCode="+errorCode+" errorMessage="+errorMessage);
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onFailureRequestPurchase");
								JSONObject data = new JSONObject();
								data.put("errorCode", Integer.toString(errorCode));
								data.put("errorMessage", errorMessage);
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}

						@Override
						public void onCancel() {
							if (sEnableLogging) {
								Log.i(TAG, "sRequestPurchaseListener: onCancel");
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onCancelRequestPurchase");
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}
					};
					
					sRequestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {

						@Override
						public void onSuccess(Collection<Receipt> receipts) {
							if (null == receipts) {
								Log.e(TAG, "Receipts are null!");
								return;
							}
							if (sEnableLogging) {
								Log.i(TAG, "requestReceipts onSuccess: received "+receipts.size() + " receipts");
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onSuccessRequestReceipts");
								JSONArray data = new JSONArray();
								int index = 0;
								for (Receipt receipt : receipts)
								{
									JSONObject item = new JSONObject();
									try {
										item.put("identifier", receipt.getIdentifier());
										item.put("purchaseDate", receipt.getPurchaseDate());
										item.put("gamer", receipt.getGamer());
										item.put("uuid", receipt.getUuid());
										item.put("localPrice", receipt.getLocalPrice());
										item.put("currency", receipt.getCurrency());
										item.put("generatedDate", receipt.getGeneratedDate());
										data.put(index, item);
										++index;
									} catch (JSONException e2) {
									}
								}
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}

						@Override
						public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
							if (sEnableLogging) {
								Log.e(TAG, "requestReceipts onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onFailureRequestReceipts");
								JSONObject data = new JSONObject();
								data.put("errorCode", Integer.toString(errorCode));
								data.put("errorMessage", errorMessage);
								json.put("data", data);
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}

						@Override
						public void onCancel() {
							if (sEnableLogging) {
								Log.i(TAG, "requestReceipts onCancel");
							}
							JSONObject json = new JSONObject();
							try {
								json.put("method", "onCancelRequestReceipts");
							} catch (JSONException e1) {
							}
							String jsonData = json.toString();
							sAsyncResults.add(jsonData);
						}
					};
					
					sInitialized = true;
				}
			};
			activity.runOnUiThread(runnable);
        }
		
		return sTrue;
	}
	
	public String getAsyncResult() {
		if (sAsyncResults.size() > 0) {
			return sAsyncResults.get(0);
		} else {
			return "";
		}
	}
	
	public String popAsyncResult() {
		if (sAsyncResults.size() > 0) {
			sAsyncResults.remove(0);
		}
		return "";
	}
	
	public String getAsyncMethod() {
		if (sAsyncResults.size() > 0) {
			String jsonData = sAsyncResults.get(0);
			Log.i(TAG, "getAsyncMethod jsonData="+jsonData);
			if (null == jsonData) {
				return "";
			}
			try {
				JSONObject json = new JSONObject(jsonData);
				if (json.has("method")) {
					String result = json.getString("method");
					if (null == result) {
						return "";
					} else {
						return result;
					}
				}
			} catch (JSONException e) {
			}
		}
		return "";
	}
	
	public String getAsyncDataString(String field) {
		if (sAsyncResults.size() > 0) {
			String jsonData = sAsyncResults.get(0);
			if (null == jsonData) {
				return "";
			}
			try {
				JSONObject json = new JSONObject(jsonData);
				if (json.has("data")) {
					JSONObject data = json.getJSONObject("data");
					if (null == data) {
						return "";
					} else {
						if (data.has(field)) {
							String result = data.getString(field);
							if (null == result) {
								return "";
							} else {
								return result;
							}
						}
					}
				}
			} catch (JSONException e) {
			}
		}
		return "";
	}
	
	public double getAsyncDataArrayCount() {
		if (sAsyncResults.size() > 0) {
			String jsonData = sAsyncResults.get(0);
			if (null == jsonData) {
				return 0;
			}
			try {
				JSONObject json = new JSONObject(jsonData);
				if (json.has("data")) {
					JSONArray data = json.getJSONArray("data");
					if (null == data) {
						return 0;
					} else {
						return data.length();
					}
				}
			} catch (JSONException e) {
			}
		}
		return 0;
	}
	
	public String getAsyncDataArrayString(String strIndex, String field) {
		if (sAsyncResults.size() > 0) {
			String jsonData = sAsyncResults.get(0);
			if (null == jsonData) {
				return "";
			}
			try {
				JSONObject json = new JSONObject(jsonData);
				if (json.has("data")) {
					JSONArray data = json.getJSONArray("data");
					if (null == data) {
						return "";
					}

					int index = Integer.parseInt(strIndex);
					if (data.length() <= index) {
						return "";
					}
					JSONObject item = data.getJSONObject(index);
					if (null == item) {
						return "";
					}
					if (item.has(field)) {
						String result = item.getString(field);
						if (null == result) {
							return "";
						} else {
							return result;
						}
					}
				}
			} catch (JSONException e) {
			}
		}
		return "";
	}
	
	public double getAsyncDataArrayDouble(String strIndex, String field) {
		if (sAsyncResults.size() > 0) {
			String jsonData = sAsyncResults.get(0);
			if (null == jsonData) {
				return 0;
			}
			try {
				JSONObject json = new JSONObject(jsonData);
				if (json.has("data")) {
					JSONArray data = json.getJSONArray("data");
					if (null == data) {
						return 0;
					}

					int index = Integer.parseInt(strIndex);
					if (data.length() <= index) {
						return 0;
					}
					JSONObject item = data.getJSONObject(index);
					if (null == item) {
						return 0;
					}
					if (item.has(field)) {
						return item.getDouble(field);
					}
				}
			} catch (JSONException e) {
			}
		}
		return 0;
	}
	
	public String isInitialized() {
		return Boolean.toString(sInitialized);
	}
	
	@Override
	public boolean dispatchKeyEvent(KeyEvent keyEvent) {
    	if (null != sInputView) {
			return sInputView.dispatchKeyEvent(keyEvent);
		}
		return true;
	}
	
	@Override
	public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	if (null != sInputView) {
			return sInputView.dispatchGenericMotionEvent(motionEvent);
		}
		return true;
	}
	
	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
		if (null != sInputView) {
			return sInputView.onGenericMotionEvent(motionEvent);
		}
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
		if (null != sInputView) {
			return sInputView.onKeyDown(keyCode, keyEvent);
		}
		return true;
	}
	
	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
		if (null != sInputView) {
			return sInputView.onKeyUp(keyCode, keyEvent);
		}
		return true;
	}

	//double, string, string
	public double getAxis(String strPlayerNum, String strAxis) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int axis = Integer.parseInt(strAxis);
		double result = OuyaInputView.getAxis(playerNum, axis);
		/*
		if (sEnableLogging) {
			Log.i(TAG, "getAxis playerNum="+strPlayerNum+" axis="+strAxis+" result="+result);
		}
		*/
		return result;
	}
	
	//boolean, string, string
	public String getButton(String strPlayerNum, String strButton) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int button = Integer.parseInt(strButton);
		boolean result = OuyaInputView.getButton(playerNum, button);
		/*
		if (sEnableLogging) {
			Log.i(TAG, "getButton playerNum="+strPlayerNum+" button="+strButton+" result="+result);
		}
		*/
		return Boolean.toString(result);
	}
	
	//boolean, string
	public String getAnyButton(String strButton) {
		int button = Integer.parseInt(strButton);
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			boolean result = OuyaInputView.getButton(playerNum, button);
			if (result) {
				/*
				if (sEnableLogging) {
					Log.i(TAG, "getAnyButton playerNum="+strPlayerNum+" button="+strButton+" result="+result);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
	}
	
	//boolean, string, string
	public String getButtonDown(String strPlayerNum, String strButton) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int button = Integer.parseInt(strButton);
		boolean result = OuyaInputView.getButtonDown(playerNum, button);
		/*
		if (sEnableLogging) {
			Log.i(TAG, "getButtonDown playerNum="+strPlayerNum+" button="+strButton+" result="+result);
		}
		*/
		return Boolean.toString(result);
	}
	
	//boolean, string
	public String getAnyButtonDown(String strButton) {
		int button = Integer.parseInt(strButton);
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			boolean result = OuyaInputView.getButtonDown(playerNum, button);
			if (result) {
				/*
				if (sEnableLogging) {
					Log.i(TAG, "getAnyButtonDown playerNum="+strPlayerNum+" button="+strButton+" result="+result);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
	}
	
	public String getButtonUp(String strPlayerNum, String strButton) {
		int playerNum = Integer.parseInt(strPlayerNum);
		int button = Integer.parseInt(strButton);
		boolean result = OuyaInputView.getButtonUp(playerNum, button);
		/*
		if (sEnableLogging) {
			Log.i(TAG, "getButtonUp playerNum="+strPlayerNum+" button="+strButton+" result="+result);
		}
		*/
		return Boolean.toString(result);
	}
	
	//boolean, string
	public String getAnyButtonUp(String strButton) {
		int button = Integer.parseInt(strButton);
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			boolean result = OuyaInputView.getButtonUp(playerNum, button);
			if (result) {
				/*
				if (sEnableLogging) {
					Log.i(TAG, "getAnyButtonUp playerNum="+strPlayerNum+" button="+strButton+" result="+result);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
	}
	
	public String isConnected(String strPlayerNum) {
		int playerNum = Integer.parseInt(strPlayerNum);
		if (OuyaController.getControllerByPlayer(playerNum) == null) {
			return sFalse;
		} else {
			return sTrue;
		}
	}
	
	public String isAnyConnected() {
		for (int playerNum = 0; playerNum < OuyaController.MAX_CONTROLLERS; ++playerNum) {			
			if (OuyaController.getControllerByPlayer(playerNum) != null) {
				/*
				if (sEnableLogging) {
					Log.i(TAG, "isAnyConnected playerNum="+strPlayerNum);
				}
				*/
				return sTrue;
			}
		}
		return sFalse;
	}
	
	public double clearButtonStatesPressedReleased() {
		OuyaInputView.clearButtonStatesPressedReleased();
		return sNoop;
	}
	
	public double requestGamerInfo() {
		if (null == sOuyaFacade) {
			Log.e(TAG, "OuyaFacade is null!");
			return sNoop;
		}
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "requestGamerInfo: Activity is null");
			return sNoop;
		}
		if (null == sRequestGamerInfoListener) {
			Log.e(TAG, "requestGamerInfo: sRequestGamerInfoListener is null");
			return sNoop;
		}
		Runnable runnable = new Runnable() {
				public void run() {
					sOuyaFacade.requestGamerInfo(activity, sRequestGamerInfoListener);
				}
		};
		activity.runOnUiThread(runnable);
		return sNoop;
	}
	
	public double requestProducts(final String identifiers) {
		if (null == sOuyaFacade) {
			Log.e(TAG, "OuyaFacade is null!");
			return sNoop;
		}
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "Activity is null");
			return sNoop;
		}
		Runnable runnable = new Runnable() {
				public void run() {
					ArrayList<Purchasable> products = new ArrayList<Purchasable>();
					String[] ids = identifiers.split(",");
					for (String identifier : ids) {
						Purchasable purchasable = new Purchasable(identifier);
						products.add(purchasable);
					}
					sOuyaFacade.requestProductList(activity, products, sRequestProductsListener);
				}
		};
		activity.runOnUiThread(runnable);
		return sNoop;
	}
	
	public double requestPurchase(final String identifier) {
		if (null == sOuyaFacade) {
			Log.e(TAG, "OuyaFacade is null!");
			return sNoop;
		}
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "Activity is null");
			return sNoop;
		}
		if (null == sRequestPurchaseListener) {
			Log.e(TAG, "requestPurchase: sRequestPurchaseListener is null");
			return sNoop;
		}
		Runnable runnable = new Runnable() {
				public void run() {
					Purchasable purchasable = new Purchasable(identifier);
					sOuyaFacade.requestPurchase(activity, purchasable, sRequestPurchaseListener);
				}
		};
		activity.runOnUiThread(runnable);
		return sNoop;
	}
	
	public double requestReceipts() {
		if (null == sOuyaFacade) {
			Log.e(TAG, "OuyaFacade is null!");
			return sNoop;
		}
		final Activity activity = RunnerActivity.CurrentActivity;
		if (null == activity) {
			Log.d(TAG, "Activity is null");
			return sNoop;
		}
		if (null == sRequestReceiptsListener) {
			Log.e(TAG, "requestReceipts: sRequestReceiptsListener is null");
			return sNoop;
		}
		Runnable runnable = new Runnable() {
				public void run() {
					sOuyaFacade.requestReceipts(activity, sRequestReceiptsListener);
				}
		};
		activity.runOnUiThread(runnable);
		return sNoop;
	}
}
