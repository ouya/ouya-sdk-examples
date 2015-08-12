package tv.ouya.sdk;

import android.app.Activity;
import android.content.Intent;
import android.content.res.AssetManager;
import android.os.Bundle;
import android.util.Log;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaWebView;
import org.apache.cordova.PluginResult;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import tv.ouya.console.api.CancelIgnoringOuyaResponseListener;
import tv.ouya.console.api.GamerInfo;
import tv.ouya.console.api.OuyaFacade;
import tv.ouya.console.api.OuyaResponseListener;
import tv.ouya.console.api.Product;
import tv.ouya.console.api.Purchasable;
import tv.ouya.console.api.PurchaseResult;
import tv.ouya.console.api.Receipt;
import tv.ouya.examples.cordova.inapppurchases.MainActivity;

/**
 * This class echoes a string called from JavaScript.
 */
public class CordovaOuyaPlugin extends CordovaPlugin {

    private static final String TAG = CordovaOuyaPlugin.class.getSimpleName();

    private static final boolean sEnableLogging = true;

    private static CordovaOuyaPlugin sInstance = null;

    private static OuyaFacade sOuyaFacade = null;

    // listener for initialization
    private static CancelIgnoringOuyaResponseListener<Bundle> sInitCompletedListener = null;

    // listener for fetching gamer info
    private static CancelIgnoringOuyaResponseListener<GamerInfo> sRequestGamerInfoListener = null;

    // listener for getting products
    private static CancelIgnoringOuyaResponseListener<List<Product>> sRequestProductsListener = null;

    // listener for requesting purchase
    private OuyaResponseListener<PurchaseResult> sRequestPurchaseListener = null;

    // listener for getting receipts
    private static OuyaResponseListener<Collection<Receipt>> sRequestReceiptsListener = null;

    // the callback context for communicating with HTML5
    private static CallbackContext sCallbackOnGenericMotionEvent = null;
    private static CallbackContext sCallbackOnKeyUp = null;
    private static CallbackContext sCallbackOnKeyDown = null;
    private static CallbackContext sCallbackInitOuyaPlugin = null;
    private static CallbackContext sCallbackRequestGamerInfo = null;
    private static CallbackContext sCallbackRequestProducts = null;
    private static CallbackContext sCallbackRequestPurchase = null;
    private static CallbackContext sCallbackRequestReceipts = null;

    @Override
    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
        super.initialize(cordova, webView);
        if (sEnableLogging) {
            Log.i(TAG, "Initialize plugin");
        }
        sInstance = this;
    }

    @Override
    public boolean execute(String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
        JSONObject result = null;
        if (sEnableLogging) {
            Log.i(TAG, "********************* execute action=" + action);
        }
        if (action.equals("setCallbackOnGenericMotionEvent")) {
            sCallbackOnGenericMotionEvent = callbackContext;
            PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
            pluginResult.setKeepCallback(true);
            sCallbackOnGenericMotionEvent.sendPluginResult(pluginResult);
            return true;
        } else if (action.equals("setCallbackOnKeyUp")) {
            sCallbackOnKeyUp = callbackContext;
            PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
            pluginResult.setKeepCallback(true);
            sCallbackOnKeyUp.sendPluginResult(pluginResult);
            return true;
        } else if (action.equals("setCallbackOnKeyDown")) {
            sCallbackOnKeyDown = callbackContext;
            PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
            pluginResult.setKeepCallback(true);
            sCallbackOnKeyDown.sendPluginResult(pluginResult);
            return true;
        } else if (action.equals("initOuyaPlugin")) {
            sCallbackInitOuyaPlugin = callbackContext;
            JSONArray jsonArray = null;
            if (args.length() > 0) {
                try {
                    jsonArray = new JSONArray(args.get(0).toString());
                } catch (Exception e) {
                    result = createError(0, "initOuyaPlugin failed to read argument!");
                    sCallbackInitOuyaPlugin.error(result);
                    return true;
                }
            } else {
                result = createError(0, "initOuyaPlugin arg1 is null!");
                sCallbackInitOuyaPlugin.error(result);
                return true;
            }
            initOuyaPlugin(jsonArray);
            return true;
        } else if (action.equals("requestGamerInfo")) {
            sCallbackRequestGamerInfo = callbackContext;
            requestGamerInfo();
            return true;
        } else if (action.equals("requestProducts")) {
            sCallbackRequestProducts = callbackContext;
            JSONArray jsonArray = null;
            if (args.length() > 0) {
                try {
                    jsonArray = new JSONArray(args.get(0).toString());
                } catch (Exception e) {
                    result = createError(0, "requestProducts failed to read argument!");
                    sCallbackRequestProducts.error(result);
                    return true;
                }
            } else {
                result = createError(0, "requestProducts arg1 is null!");
                sCallbackRequestProducts.error(result);
                return true;
            }
            requestProducts(jsonArray);
            return true;
        } else if (action.equals("requestPurchase")) {
            sCallbackRequestPurchase = callbackContext;
            String identifier = null;
            if (args.length() > 0) {
                try {
                    Log.i(TAG, "requestPurchase identifier="+args.getString(0));
                    identifier = args.getString(0);
                } catch (Exception e) {
                    result = createError(0, "requestPurchase failed to read argument!");
                    sCallbackRequestPurchase.error(result);
                    return true;
                }
            } else {
                result = createError(0, "requestPurchase arg1 is null!");
                sCallbackRequestPurchase.error(result);
                return true;
            }
            requestPurchase(identifier);
            return true;
        } else if (action.equals("requestReceipts")) {
            sCallbackRequestReceipts = callbackContext;
            requestReceipts();
            return true;
        }
        return false;
    }

    public static void onGenericMotionEvent(final int playerNum, final int axis, final float val) {
        if (null == sCallbackOnGenericMotionEvent) {
            if (sEnableLogging) {
                Log.e(TAG, "sCallbackOnGenericMotionEvent is null!");
            }
            return;
        }
        if (null == sInstance) {
            if (sEnableLogging) {
                Log.e(TAG, "sInstance is null!");
            }
            return;
        }
        sInstance.cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("playerNum", playerNum);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("axis", axis);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("val", val);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, jsonObject.toString());
                pluginResult.setKeepCallback(true);
                sCallbackOnGenericMotionEvent.sendPluginResult(pluginResult); // Thread-safe.
            }
        });
    }

    public static void onKeyUp(final int playerNum, final int button) {
        if (null == sCallbackOnKeyUp) {
            if (sEnableLogging) {
                Log.e(TAG, "sCallbackOnKeyUp is null!");
            }
            return;
        }
        if (null == sInstance) {
            if (sEnableLogging) {
                Log.e(TAG, "sInstance is null!");
            }
            return;
        }
        sInstance.cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("playerNum", playerNum);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("button", button);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, jsonObject.toString());
                pluginResult.setKeepCallback(true);
                sCallbackOnKeyUp.sendPluginResult(pluginResult); // Thread-safe.
            }
        });
    }

    public static void onKeyDown(final int playerNum, final int button) {
        if (null == sCallbackOnKeyDown) {
            if (sEnableLogging) {
                Log.e(TAG, "sCallbackOnKeyDown is null!");
            }
            return;
        }
        if (null == sInstance) {
            if (sEnableLogging) {
                Log.e(TAG, "sInstance is null!");
            }
            return;
        }
        sInstance.cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                JSONObject jsonObject = new JSONObject();
                try {
                    jsonObject.put("playerNum", playerNum);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                try {
                    jsonObject.put("button", button);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, jsonObject.toString());
                pluginResult.setKeepCallback(true);
                sCallbackOnKeyDown.sendPluginResult(pluginResult); // Thread-safe.
            }
        });
    }

    private static JSONObject createError(final int errorCode, final String errorMessage) {
        if (sEnableLogging) {
            Log.e(TAG, "errorCode="+errorCode+" errorMessage="+errorMessage);
        }
        JSONObject result = new JSONObject();
        try {
            result.put("errorCode", errorCode);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        try {
            result.put("errorMessage", errorMessage);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return result;
    }

    public void initOuyaPlugin(JSONArray jsonArray) {
        JSONObject result = null;
        if (null == jsonArray) {
            result = createError(0, "initOuyaPlugin jsonArray is null!");
            sCallbackInitOuyaPlugin.error(result);
            return;
        }

        if (sEnableLogging) {
            Log.i(TAG, "initOuyaPlugin jsonArray=" + jsonArray.toString());
        }

        Bundle developerInfo = new Bundle();

        try {
            for (int index = 0; index < jsonArray.length(); ++index) {
                JSONObject jsonObject = jsonArray.getJSONObject(index);
                String name = jsonObject.getString("key");
                String value = jsonObject.getString("value");
                if (null == name || null == value) {
                    continue;
                }
                if (name.equals("tv.ouya.product_id_list")) {
                    String[] productIds = value.split(",");
                    if (null == productIds) {
                        continue;
                    }
                    developerInfo.putStringArray("tv.ouya.product_id_list", productIds);
                } else {
                    Log.i(TAG, "initOuyaPlugin name="+name+" value="+value);
                    developerInfo.putString(name, value);
                }
            }
        } catch (JSONException e) {
            e.printStackTrace();

            result = createError(0, "initOuyaPlugin Failed to read JSON data!");
            sCallbackInitOuyaPlugin.error(result);
            return;
        }

        byte[] applicationKey = null;

        // load the application key from assets
        try {
            AssetManager assetManager = cordova.getActivity().getAssets();
            InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
            applicationKey = new byte[inputStream.available()];
            inputStream.read(applicationKey);
            inputStream.close();
        } catch (IOException e) {
            e.printStackTrace();
            result = createError(0, "Failed to load signing key!");
            sCallbackInitOuyaPlugin.error(result);
            return;
        }

        if (null == applicationKey) {
            result = createError(0, "Failed to load signing key!");
            sCallbackInitOuyaPlugin.error(result);
            return;
        }

        developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, applicationKey);

        sInitCompletedListener = new CancelIgnoringOuyaResponseListener<Bundle>() {
            @Override
            public void onSuccess(Bundle info) {
                if (sEnableLogging) {
                    Log.i(TAG, "sInitCompletedListener: onSuccess");
                }
                PluginResult pluginResult = new PluginResult(PluginResult.Status.OK, "");
                pluginResult.setKeepCallback(true);
                sCallbackInitOuyaPlugin.sendPluginResult(pluginResult);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
                if (sEnableLogging) {
                    Log.i(TAG, "sInitCompletedListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
                }
                JSONObject result = createError(errorCode, errorMessage);
                sCallbackInitOuyaPlugin.error(result);
            }
        };

        sRequestGamerInfoListener = new CancelIgnoringOuyaResponseListener<GamerInfo>() {
            @Override
            public void onSuccess(GamerInfo info) {
                Log.i(TAG, "sRequestGamerInfoListener: onSuccess");
                JSONObject result = new JSONObject();
                try {
                    result.put("username", info.getUsername());
                } catch (JSONException e) {
                    result = createError(0, "Failed to set username!");
                    sCallbackRequestGamerInfo.error(result);
                    return;
                }
                try {
                    result.put("uuid", info.getUuid());
                } catch (JSONException e) {
                    result = createError(0, "Failed to set uuid!");
                    sCallbackRequestGamerInfo.error(result);
                    return;
                }
                sCallbackRequestGamerInfo.success(result);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
                Log.i(TAG, "sRequestGamerInfoListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
                JSONObject result = createError(errorCode, errorMessage);
                sCallbackRequestGamerInfo.error(result);
            }
        };

        sRequestProductsListener = new CancelIgnoringOuyaResponseListener<List<Product>>() {
            @Override
            public void onSuccess(final List<Product> products) {
                Log.i(TAG, "sRequestProductsListener: onSuccess");
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
                    JSONObject error = createError(0, "Failed to create results!");
                    sCallbackRequestProducts.error(error);
                    return;
                }
                sCallbackRequestProducts.success(result);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
                Log.i(TAG, "sRequestProductsListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
                JSONObject result = createError(errorCode, errorMessage);
                sCallbackRequestProducts.error(result);
            }
        };

        sRequestPurchaseListener = new OuyaResponseListener<PurchaseResult>() {

            @Override
            public void onSuccess(PurchaseResult purchaseResult) {
                Log.i(TAG, "sRequestPurchaseListener: onSuccess");
                JSONObject result = new JSONObject();
                try {
                    result.put("productIdentifier", purchaseResult.getProductIdentifier());
                } catch (JSONException e) {
                    result = createError(0, "Failed to set productIdentifier!");
                    sCallbackRequestPurchase.error(result);
                    return;
                }
                sCallbackRequestPurchase.success(result);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
                Log.i(TAG, "sRequestPurchaseListener: onFailure errorCode=" + errorCode + " errorMessage=" + errorMessage);
                JSONObject result = createError(errorCode, errorMessage);
                sCallbackRequestPurchase.error(result);
            }

            @Override
            public void onCancel() {
                Log.i(TAG, "sRequestPurchaseListener: onCancel");
                JSONObject result = createError(0, "Purchase was cancelled!");
                sCallbackRequestPurchase.error(result);
            }
        };

        sRequestReceiptsListener = new OuyaResponseListener<Collection<Receipt>>() {

            @Override
            public void onSuccess(Collection<Receipt> receipts) {
                Log.i(TAG, "requestReceipts onSuccess: received " + receipts.size() + " receipts");
                JSONArray result = new JSONArray();
                try {
                    int i = 0;
                    for (Receipt receipt : receipts) {
                        JSONObject jsonObject = new JSONObject();
                        jsonObject.put("currency", receipt.getCurrency());
                        jsonObject.put("identifier", receipt.getIdentifier());
                        jsonObject.put("localPrice", receipt.getLocalPrice());
                        result.put(i, jsonObject);
                        ++i;
                    }
                } catch (JSONException e) {
                    JSONObject error = createError(0, "Failed to create results!");
                    sCallbackRequestReceipts.error(error);
                    return;
                }
                sCallbackRequestReceipts.success(result);
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
                Log.e(TAG, "requestReceipts onFailure: errorCode=" + errorCode + " errorMessage=" + errorMessage);
                JSONObject result = createError(errorCode, errorMessage);
                sCallbackRequestReceipts.error(result);
            }

            @Override
            public void onCancel() {
                Log.i(TAG, "requestReceipts onCancel");
                JSONObject result = createError(0, "Request Receipts was cancelled!");
                sCallbackRequestReceipts.error(result);
            }
        };

        sOuyaFacade = OuyaFacade.getInstance();
        sOuyaFacade.registerInitCompletedListener(sInitCompletedListener);
        sOuyaFacade.init(cordova.getActivity(), developerInfo);
    }

    public static boolean processActivityResult(final int requestCode, final int resultCode, final Intent data) {
        if (null == sOuyaFacade) {
            return false;
        }
        return sOuyaFacade.processActivityResult(requestCode, resultCode, data);
    }

    public void requestGamerInfo() {
        JSONObject result = null;
        if (null == sOuyaFacade) {
            result = createError(0, "requestGamerInfo sOuyaFacade is null!");
            sCallbackRequestGamerInfo.error(result);
            return;
        }

        if (sEnableLogging) {
            Log.i(TAG, "requestGamerInfo");
        }

        if (null == sRequestGamerInfoListener) {
            result = createError(0, "requestGamerInfo: sRequestGamerInfoListener is null");
            sCallbackRequestGamerInfo.error(result);
            return;
        }
        sOuyaFacade.requestGamerInfo(cordova.getActivity(), sRequestGamerInfoListener);
    }

    public void requestProducts(JSONArray jsonArray) {
        JSONObject result = null;
        if (null == sOuyaFacade) {
            result = createError(0, "requestProducts sOuyaFacade is null!");
            sCallbackRequestProducts.error(result);
            return;
        }

        if (sEnableLogging) {
            Log.i(TAG, "requestProducts");
        }

        if (null == sRequestProductsListener) {
            result = createError(0, "requestProducts: sRequestGamerInfoListener is null");
            sCallbackRequestProducts.error(result);
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
            result = createError(0, "requestProducts Failed to create product list!");
            sCallbackRequestProducts.error(result);
            return;
        }
        sOuyaFacade.requestProductList(cordova.getActivity(), products, sRequestProductsListener);
    }

    protected void requestPurchase(String identifier) {
        if (null == sRequestPurchaseListener) {
            JSONObject result = createError(0, "requestPurchase: sRequestPurchaseListener is null");
            sCallbackRequestProducts.error(result);
            return;
        }
        Purchasable purchasable = new Purchasable(identifier);
        sOuyaFacade.requestPurchase(cordova.getActivity(), purchasable, sRequestPurchaseListener);
    }

    protected void requestReceipts() {
        if (null == sRequestReceiptsListener) {
            JSONObject result = createError(0, "requestReceipts: sRequestReceiptsListener is null");
            sCallbackRequestReceipts.error(result);
            return;
        }
        sOuyaFacade.requestReceipts(cordova.getActivity(), sRequestReceiptsListener);
    }
}
