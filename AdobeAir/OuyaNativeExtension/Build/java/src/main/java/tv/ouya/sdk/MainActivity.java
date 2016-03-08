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

package tv.ouya.sdk;

import android.app.Activity;
import android.content.Intent;
import android.content.res.AssetManager;
import android.graphics.Color;
import android.graphics.Point;
import android.os.Bundle;
import android.util.Log;
import android.view.Display;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.view.WindowManager;
import android.view.*;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.FrameLayout;
import com.adobe.fre.FREContext;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import tv.ouya.console.api.*;
import tv.ouya.console.api.*;
import tv.ouya.sdk.*;

public class MainActivity extends Activity implements OnClickListener {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final boolean sEnableLogging = false;
	
	private static MainActivity sInstance = null;
	
    private OuyaInputView mInputView = null;
	
	private static String sDeveloperId;
	
	private static OuyaFacade sOuyaFacade = null;
	
    // listener for fetching gamer info
    private static CancelIgnoringOuyaResponseListener<GamerInfo> sRequestGamerInfoListener = null;

    // listener for getting products
    private static CancelIgnoringOuyaResponseListener<List<Product>> sRequestProductsListener = null;

    // listener for requesting purchase
    private static OuyaResponseListener<PurchaseResult> sRequestPurchaseListener = null;

    // listener for getting receipts
    private static OuyaResponseListener<Collection<Receipt>> sRequestReceiptsListener = null;
	
	private static boolean sInitialized = false;
	
	public static FREContext sFREContext = null;
    
    @Override
    public void onCreate(Bundle savedInstanceState) {
		if (sEnableLogging) {
			Log.d(TAG, "**** onCreate");
		}
        super.onCreate(savedInstanceState);
		
		sInstance = this;
        
        mInputView = new OuyaInputView(this);
        
		if (sEnableLogging) {
			Log.d(TAG, "Disable screensaver");
		}
        mInputView.setKeepScreenOn(true);
		
        FrameLayout content = (FrameLayout)findViewById(android.R.id.content);
		if (null != content) {
			Button button = new Button(this);
	        button.setOnClickListener(this);
	        button.setVisibility(View.VISIBLE);
	        button.setText("");
	        button.setBackgroundColor(Color.TRANSPARENT);
			content.addView(button);
		} else {
			Log.e(TAG, "Content view is missing");
		}
		
		OuyaController.init(this);
		
		if (null == sOuyaFacade) {
			
			sRequestGamerInfoListener = new CancelIgnoringOuyaResponseListener<GamerInfo>() {
				@Override
				public void onSuccess(final GamerInfo info) {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestGamerInfoListener: onSuccess");
					}
					JSONObject result = new JSONObject();
					try {
						result.put("username", info.getUsername());
						result.put("uuid", info.getUuid());
					} catch (JSONException e) {
						sendError("RequestGamerInfoOnFailure", 0, "Failed to create results!");
						return;
					}
					sendResult("RequestGamerInfoOnSuccess", result.toString());
				}

				@Override
				public void onFailure(final int errorCode, final String errorMessage, final Bundle optionalData) {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestGamerInfoListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
					}
					sendError("RequestGamerInfoOnFailure", errorCode, errorMessage);
				};
			};

			sRequestProductsListener = new CancelIgnoringOuyaResponseListener<List<Product>>() {
				@Override
				public void onSuccess(final List<Product> products) {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestProductsListener: onSuccess");
					}
					JSONArray result = new JSONArray();
					try {
						int i = 0;
						for (Product product : products) {
							JSONObject jsonObject = new JSONObject();
							jsonObject.put("description", product.getDescription());
							jsonObject.put("identifier", product.getIdentifier());
							jsonObject.put("name", product.getName());
							jsonObject.put("localPrice", product.getLocalPrice());
							result.put(i, jsonObject);
							++i;
						}
					} catch (JSONException e) {
						sendError("RequestProductsOnFailure", 0, "Failed to create results!");
						return;
					}
					sendResult("RequestProductsOnSuccess", result.toString());
				}

				@Override
				public void onFailure(final int errorCode, final String errorMessage, final Bundle optionalData) {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestProductsListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
					}
					sendError("RequestProductsOnFailure", errorCode, errorMessage);
				}
			};

			sRequestPurchaseListener = new OuyaResponseListener<PurchaseResult>() {

				@Override
				public void onSuccess(final PurchaseResult purchaseResult) {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestPurchaseListener: onSuccess");
					}
					JSONObject result = new JSONObject();
					try {
						result.put("productIdentifier", purchaseResult.getProductIdentifier());
					} catch (JSONException e) {
						sendError("RequestPurchaseOnFailure", 0, "Failed to set productIdentifier!");
						return;
					}
					sendResult("RequestPurchaseOnSuccess", result.toString());
				}

				@Override
				public void onFailure(final int errorCode, final String errorMessage, final Bundle optionalData) {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestPurchaseListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
					}
					sendError("RequestPurchaseOnFailure", errorCode, errorMessage);
				}

				@Override
				public void onCancel() {
					if (sEnableLogging) {
						Log.d(TAG, "sRequestPurchaseListener: onCancel");
					}
					sendError("RequestPurchaseOnCancel", 0, "Purchase was cancelled!");
				}
			};

			sRequestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {

				@Override
				public void onSuccess(final Collection<Receipt> receipts) {
					if (sEnableLogging) {
						Log.d(TAG, "requestReceipts onSuccess: received " + receipts.size() + " receipts");
					}
					JSONArray result = new JSONArray();
					try {
						int i = 0;
						for (Receipt receipt : receipts) {
							JSONObject jsonObject = new JSONObject();
							jsonObject.put("currency", receipt.getCurrency());
							jsonObject.put("generatedDate", receipt.getGeneratedDate());
							jsonObject.put("identifier", receipt.getIdentifier());
							jsonObject.put("localPrice", receipt.getLocalPrice());
							result.put(i, jsonObject);
							++i;
						}
					} catch (JSONException e) {
						sendError("RequestReceiptsOnFailure", 0, "Failed to create results!");
						return;
					}
					sendResult("RequestReceiptsOnSuccess", result.toString());
				}

				@Override
				public void onFailure(final int errorCode, final String errorMessage, final Bundle optionalData) {
					if (sEnableLogging) {
						Log.d(TAG, "requestReceipts onFailure: errorCode=" + errorCode + " errorMessage=" + errorMessage);
					}
					sendError("RequestReceiptsOnFailure", errorCode, errorMessage);
				}

				@Override
				public void onCancel() {
					if (sEnableLogging) {
						Log.d(TAG, "requestReceipts onCancel");
					}
					sendError("RequestReceiptsOnCancel", 0, "Request Receipts was cancelled!");
				}
			};
			
			byte[] applicationKey = null;
			
			// load the application key from assets
			try {
				AssetManager assetManager = getAssets();
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
			
			sOuyaFacade = OuyaFacade.getInstance();
			
			Bundle developerInfo = new Bundle();
			developerInfo.putString(OuyaFacade.OUYA_DEVELOPER_ID, sDeveloperId);
			developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, applicationKey);
			
			try {
				sOuyaFacade.init(this, developerInfo);
			} catch (Exception e) {
				e.printStackTrace();
			}

			if (sEnableLogging) {
				Log.d(TAG, "Initialized OuyaFacade.");
			}
			sInitialized = true;
		}
    }
    
	@Override
    public void onDestroy()
    {
    	super.onDestroy();
		if (null != mInputView) {
			mInputView.shutdown();
		}
		sInstance = null;
    }
	
	@Override
	protected void onStart() {
		super.onStart();
		if (sEnableLogging) {
			Log.d(TAG, "**** onStart");
		}
	}
	
	@Override
	protected void onPause() {
		super.onPause();
		if (sEnableLogging) {
			Log.d(TAG, "**** onPause");
		}
	}
	
	@Override
	protected void onResume() {
		super.onResume();
		if (sEnableLogging) {
			Log.d(TAG, "**** onResume");
		}
	}

	@Override
	protected void onStop() {
		super.onStop();
		if (sEnableLogging) {
			Log.d(TAG, "**** onStop isFinishing="+isFinishing());
		}
		finish();
	}

	@Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "dispatchGenericMotionEvent");
		}
    	if (null != mInputView) {
			mInputView.dispatchGenericMotionEvent(motionEvent);
		}
		return true;
    }
	
	@Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "dispatchKeyEvent keyCode="+keyEvent.getKeyCode());
		}
		InputDevice device = keyEvent.getDevice();
		if (null != device) {
			String name = device.getName();
			if (null != name &&
				name.equals("aml_keypad")) {
				switch (keyEvent.getKeyCode()) {
				case 24:
					if (sEnableLogging) {
						Log.i(TAG, "Volume Up detected.");
					}
					return false;
				case 25:
					if (sEnableLogging) {
						Log.i(TAG, "Volume Down detected.");
					}
					return false;
				case 66:
					if (sEnableLogging) {
						Log.i(TAG, "Remote button detected.");
					}
					if (null != mInputView) {
						if (keyEvent.getAction() == KeyEvent.ACTION_DOWN) {
							mInputView.onKeyDown(OuyaController.BUTTON_O, keyEvent);
						} else if (keyEvent.getAction() == KeyEvent.ACTION_UP) {
							mInputView.onKeyUp(OuyaController.BUTTON_O, keyEvent);
						}
					}
					return false;
				}
			}
		}
    	if (null != mInputView) {
			mInputView.dispatchKeyEvent(keyEvent);
		}
		return true;
    }

	@Override
	public boolean onGenericMotionEvent(MotionEvent motionEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "onGenericMotionEvent");
		}
    	if (null != mInputView) {
			mInputView.requestFocus();
		}
		return true;
	}

	@Override
	public boolean onKeyUp(int keyCode, KeyEvent keyEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "onKeyUp");
		}
    	if (null != mInputView) {
			mInputView.requestFocus();
		}
		return true;
	}
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent keyEvent) {
    	if (sEnableLogging) {
			Log.i(TAG, "onKeyDown");
		}
    	if (null != mInputView) {
			mInputView.requestFocus();
		}
		return true;
	}
	
	@Override
	public boolean dispatchTouchEvent(MotionEvent motionEvent) {
		if (sEnableLogging) {
			Log.i(TAG, "dispatchTouchEvent source="+motionEvent.getSource()+" x="+motionEvent.getX()+" y="+motionEvent.getY());
		}
		int source = motionEvent.getSource();
		if (source == 8194) {
			OuyaInputView.setTrackpadX(motionEvent.getX());
			OuyaInputView.setTrackpadY(motionEvent.getY());
		}
		return super.dispatchTouchEvent(motionEvent);
	}
	
	@Override
	public void onClick(View view) {
		if (sEnableLogging) {
			Log.i(TAG, "Click happened");
		}
		OuyaInputView.setTrackpadDown();
	}
	
	private static int getDisplayWidth() {
		Activity activity = sInstance;
		if (null != activity) {
			WindowManager windowManager = activity.getWindowManager();
			Display display = windowManager.getDefaultDisplay();
			Point size = new Point();
			display.getSize(size);
			return size.x;
		} else {
			return 0;
		}
	}

	private static int getDisplayHeight() {
		Activity activity = sInstance;
		if (null != activity) {
			WindowManager windowManager = activity.getWindowManager();
			Display display = windowManager.getDefaultDisplay();
			Point size = new Point();
			display.getSize(size);
			return size.y;
		} else {
			return 0;
		}
	}
	
	private static void updateSafeArea(float progress) {
		Activity activity = sInstance;
		if (null != activity) {
			//Log.d(TAG, "updateSafeArea: progress="+progress);
			// bring in by %
			float percent = 0.1f;
			float ratio = 1 - (1 - progress) * percent;
			float halfRatio = 1 - (1 - progress) * percent * 0.5f;
			float maxWidth = getDisplayWidth();
			float maxHeight = getDisplayHeight();
			FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
			LayoutParams layout = content.getLayoutParams();
			layout.width = (int)(maxWidth * ratio);
			layout.height = (int)(maxHeight * ratio);
			content.setLayoutParams(layout);
			content.setX(maxWidth - maxWidth * halfRatio);
			content.setY(maxHeight - maxHeight * halfRatio);
		}
	}
	
	public static void setResolution(final int width, final int height)
	{
		try
		{
			//Log.d(TAG, "setResolution: width="+width+" height="+height);
			Activity activity = sInstance;
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						
					}
				};
				activity.runOnUiThread(runnable);
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "setResolution: exception=" + e.toString());
		}
	}

	public static void setSafeArea(final float percentage)
	{
		try
		{
			//Log.d(TAG, "setSafeArea: percentage="+percentage);
			Activity activity = sInstance;
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						updateSafeArea(percentage);
					}
				};
				activity.runOnUiThread(runnable);
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "setSafeArea: exception=" + e.toString());
		}
	}
	
	public static void setDeveloperId(String developerId) {
		sDeveloperId = developerId;
	}
	
	public static boolean isInitialized() {
		return sInitialized;
	}
	
	// Process onActivityResult events that only happen for Activity
    @Override
    protected void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
		if (sEnableLogging) {
			Log.i(TAG, "Processing activity result");
		}
        
        if (null == sOuyaFacade) {
        	return;
        }

        // Forward this result to the facade, in case it is waiting for any activity results
        if (sOuyaFacade.processActivityResult(requestCode, resultCode, data)) {
            return;
        }
    }
	
	private static void sendResult(final String tag, final String jsonData) {
		if (null == sFREContext) {
			return;
		}
		sFREContext.dispatchStatusEventAsync(tag, jsonData);
	}
	
	private static void sendError(final String tag, final int errorCode, final String errorMessage) {
		if (null == sFREContext) {
			return;
		}
		JSONObject jsonObject = new JSONObject();
		try {
			jsonObject.put("errorCode", errorCode);
			jsonObject.put("errorMessage", errorMessage);
		} catch (JSONException e) {
		}
		sFREContext.dispatchStatusEventAsync(tag, jsonObject.toString());
	}
	
	public static void requestProducts(String jsonData) {
		if (null == sFREContext) {
			Log.e(TAG, "Context is not set!");
			return;
		}
		if (null == sInstance) {
			sendError("RequestProductsError", 0, "MainActivity is null!");
			return;
		}
        if (null == sOuyaFacade) {
            sendError("RequestProductsError", 0, "requestProducts sOuyaFacade is null!");
            return;
        }
        if (sEnableLogging) {
            Log.d(TAG, "requestProducts");
        }
        if (null == sRequestProductsListener) {
            sendError("RequestProductsError", 0, "requestProducts: sRequestGamerInfoListener is null");
            return;
        }
		JSONArray jsonArray = null;
		try {
			jsonArray = new JSONArray(jsonData);
		} catch (JSONException e) {
            sendError("RequestProductsError", 0, "requestProducts Failed to create product list!");
            return;
        }		
		ArrayList<Purchasable> products = new ArrayList<Purchasable>();
        try {
            for (int i = 0; i < jsonArray.length(); ++i) {
                String identifier = jsonArray.getString(i);
                Purchasable purchasable = new Purchasable(identifier);
                products.add(purchasable);
            }
        } catch (JSONException e) {
            sendError("RequestProductsError", 0, "requestProducts Failed to create product list!");
            return;
        }
        sOuyaFacade.requestProductList(sInstance, products, sRequestProductsListener);
	}
	
	public static void requestPurchase(String identifier) {
		if (null == sFREContext) {
			Log.e(TAG, "Context is not set!");
			return;
		}
		if (null == sInstance) {
			sendError("RequestReceiptsError", 0, "MainActivity is null!");
			return;
		}
		if (null == sOuyaFacade) {
            sendError("RequestReceiptsError", 0, "requestProducts sOuyaFacade is null!");
            return;
        }
		if (null == sRequestPurchaseListener) {
            sendError("RequestPurchaseError", 0, "requestPurchase: sRequestPurchaseListener is null");
            return;
        }
        Purchasable purchasable = new Purchasable(identifier);
        sOuyaFacade.requestPurchase(sInstance, purchasable, sRequestPurchaseListener);
	}
	
	public static void requestReceipts() {
		if (null == sFREContext) {
			Log.e(TAG, "Context is not set!");
			return;
		}
		if (null == sInstance) {
			sendError("RequestReceiptsError", 0, "MainActivity is null!");
			return;
		}
		if (null == sOuyaFacade) {
            sendError("RequestReceiptsError", 0, "requestProducts sOuyaFacade is null!");
            return;
        }
		if (null == sRequestReceiptsListener) {
            sendError("RequestReceiptsError", 0, "requestReceipts: sRequestReceiptsListener is null");
            return;
        }
        sOuyaFacade.requestReceipts(sInstance, sRequestReceiptsListener);
	}
	
	public static void requestGamerInfo() {
		if (null == sFREContext) {
			Log.e(TAG, "Context is not set!");
			return;
		}
		if (null == sInstance) {
			sendError("RequestGamerInfoError", 0, "MainActivity is null!");
			return;
		}
		if (null == sOuyaFacade) {
            sendError("RequestGamerInfoError", 0, "requestProducts sOuyaFacade is null!");
            return;
        }
        if (sEnableLogging) {
            Log.i(TAG, "requestGamerInfo");
        }
        if (null == sRequestGamerInfoListener) {
            sendError("RequestGamerInfoError", 0, "requestGamerInfo: sRequestGamerInfoListener is null");
            return;
        }
        sOuyaFacade.requestGamerInfo(sInstance, sRequestGamerInfoListener);
	}
	
	public static String getGameData(String key) {
		if (null == sFREContext) {
			Log.e(TAG, "Context is not set!");
			return null;
		}
		if (null == sInstance) {
			sendError("GetGameData", 0, "getGameData: MainActivity is null!");
			return null;
		}
		if (null == sOuyaFacade) {
            sendError("GetGameData", 0, "getGameData: sOuyaFacade is null!");
            return null;
        }
        if (sEnableLogging) {
            Log.i(TAG, "GetGameData");
        }
        if (null == key) {
            sendError("GetGameData", 0, "getGameData: key is null");
            return null;
        }
        return sOuyaFacade.getGameData(key);
	}
	
	public static void putGameData(String key, String val) {
		if (null == sFREContext) {
			Log.e(TAG, "Context is not set!");
			return;
		}
		if (null == sInstance) {
			sendError("PutGameData", 0, "putGameData: MainActivity is null!");
			return;
		}
		if (null == sOuyaFacade) {
            sendError("PutGameData", 0, "putGameData sOuyaFacade is null!");
            return;
        }
        if (sEnableLogging) {
            Log.i(TAG, "PutGameData");
        }
        if (null == key) {
            sendError("PutGameData", 0, "putGameData: key is null");
            return;
        }
		if (null == val) {
            sendError("PutGameData", 0, "putGameData: value is null");
            return;
        }
        sOuyaFacade.putGameData(key, val);
	}
	
	public static void shutdown() {
		
		if (null != sOuyaFacade) {
			sOuyaFacade.shutdown();
			sOuyaFacade = null;
		}
		
		// with multiple activities, make sure application exits
		android.os.Process.killProcess(android.os.Process.myPid());		
	}	
}
