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

import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.graphics.Bitmap;
import android.graphics.Point;
import android.util.Log;
import android.view.Display;
import android.view.ViewGroup.LayoutParams;
import android.view.View;
import android.view.WindowManager;
import android.widget.FrameLayout;
import com.unity3d.player.UnityPlayer;
import java.io.File;
import java.util.*;
import org.json.JSONArray;
import org.json.JSONObject;
import tv.ouya.console.api.*;
import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaMod;
import tv.ouya.console.api.content.OuyaModException;
import tv.ouya.console.api.content.OuyaModScreenshot;


public class OuyaUnityPlugin
{
	private static final String TAG = OuyaUnityPlugin.class.getSimpleName();

	// This initializes the Unity plugin - our OuyaUnityPlugin,
	// and it gets a generic reference to the activity
	public OuyaUnityPlugin(Activity currentActivity)
	{
	}

	// most of the java functions that are called, need the ouya facade initialized
	public static void initOuyaPlugin(final String jsonData)
	{
		Activity activity = IOuyaActivity.GetActivity();
		if (null != activity) {
			Runnable runnable = new Runnable()
			{
				public void run()
				{
					try
					{
						if (null == IOuyaActivity.GetActivity())
						{
							Log.e(TAG, "initOuyaPlugin: activity is null");
							IOuyaActivity.GetUnityPlayer().UnitySendMessage("OuyaGameObject", "OnFailureInitializePlugin", "Activity is missing");
							return;
						}

						if (null == IOuyaActivity.GetApplicationKey())
						{
							Log.e(TAG, "initOuyaPlugin: application key is null");
							IOuyaActivity.GetUnityPlayer().UnitySendMessage("OuyaGameObject", "OnFailureInitializePlugin", "Application key is missing");
							return;
						}

						Log.i(TAG, "initOuyaPlugin: Initializing plugin");

						Bundle developerInfo = new Bundle();

						developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, IOuyaActivity.GetApplicationKey());

						JSONArray jsonArray = new JSONArray(jsonData);
						for (int index = 0; index < jsonArray.length(); ++index) {
							JSONObject jsonObject = jsonArray.getJSONObject(index);
							String name = jsonObject.getString("key");
							String value = jsonObject.getString("value");
							if (null == name ||
								null == value) {
								continue;
							}
							if (name.equals("tv.ouya.product_id_list")) {
								String[] productIds = value.split(",");
								if (null == productIds) {
									continue;
								}
								developerInfo.putStringArray("tv.ouya.product_id_list", productIds);
							} else {
								developerInfo.putString(name, value);
							}
						}

						UnityOuyaFacade unityOuyaFacade =
							new UnityOuyaFacade(IOuyaActivity.GetActivity(),
								IOuyaActivity.GetSavedInstanceState(),
								developerInfo);

						IOuyaActivity.SetUnityOuyaFacade(unityOuyaFacade);
			
						Log.i(TAG, "initOuyaPlugin: OuyaGameObject send OnSuccessInitializePlugin");
						IOuyaActivity.GetUnityPlayer().UnitySendMessage("OuyaGameObject", "OnSuccessInitializePlugin", "");			
					}
					catch (Exception e)
					{
						e.printStackTrace();
						IOuyaActivity.GetUnityPlayer().UnitySendMessage("OuyaGameObject", "OnFailureInitializePlugin", "InitializePlugin exception");
					}
				}
			};
			activity.runOnUiThread(runnable);
		}
	}

	public static boolean isInitialized() {
		UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
		if (null == unityOuyaFacade) {
			return false;
		} else {
			return unityOuyaFacade.isInitialized();
		}
	}

	public static void putGameData(String key, String val)
	{
		//Log.i(TAG, "putGameData: key=" + key + " val=" + val);

		if (null == IOuyaActivity.GetUnityOuyaFacade())
		{
			Log.e(TAG, "putGameData: unityOuyaFacade is null");
		}
		else
		{
			IOuyaActivity.GetUnityOuyaFacade().putGameData(key, val);
		}
	}

	public static String getGameData(String key)
	{
		//Log.i(TAG, "getGameData");

		if (null == IOuyaActivity.GetUnityOuyaFacade())
		{
			Log.e(TAG, "getGameData: unityOuyaFacade is null");
			return "";
		}
		else
		{
			return IOuyaActivity.GetUnityOuyaFacade().getGameData(key);
		}
	}

	public static void requestGamerInfo()
	{
		try
		{
			//Log.i(TAG, "requestGamerInfo");
			if (null == IOuyaActivity.GetUnityOuyaFacade())
			{
				Log.e(TAG, "requestGamerInfo: unityOuyaFacade is null");
			}
			else
			{
				IOuyaActivity.GetUnityOuyaFacade().requestGamerInfo();
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "requestGamerInfo: exception=" + e.toString());
		}
	}

	public static void requestProducts(String jsonData)
	{
		try
		{
			//Log.i(TAG, "requestProducts");
			if (null == IOuyaActivity.GetUnityOuyaFacade())
			{
				Log.e(TAG, "requestProducts: unityOuyaFacade is null");
				return;
			}

			JSONArray jsonArray = new JSONArray(jsonData);

			ArrayList<Purchasable> products = new ArrayList<Purchasable>();

			for (int i = 0; i < jsonArray.length(); ++i) {
				String productId = jsonArray.getString(i);
				if (null != productId) {
					Purchasable purchasable = new Purchasable(productId);
					products.add(purchasable);
				}
			}

			IOuyaActivity.GetUnityOuyaFacade().requestProducts(products);
		}
		catch (Exception e)
		{
			Log.e(TAG, "requestProducts: exception=" + e.toString());
		}
	}

	public static void requestPurchase(String productId)
	{
		try
		{
			//Log.i(TAG, "requestPurchase productId: " + productId);
			if (null == IOuyaActivity.GetUnityOuyaFacade())
			{
				Log.e(TAG, "requestPurchase: unityOuyaFacade is null");
			}
			else
			{
				Product product = new Product(productId, "", 0, 0, "", 0, 0, "", "", Product.Type.ENTITLEMENT);
				IOuyaActivity.GetUnityOuyaFacade().requestPurchase(product);
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "requestPurchase: exception=" + e.toString());
		}
	}

	public static void requestReceipts()
	{
		try
		{
			//Log.i(TAG, "requestReceipts");
			if (null == IOuyaActivity.GetUnityOuyaFacade())
			{
				Log.e(TAG, "requestReceipts: unityOuyaFacade is null");
			}
			else
			{
				IOuyaActivity.GetUnityOuyaFacade().requestReceipts();
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "requestReceipts: exception=" + e.toString());
		}
	}

	public static boolean isRunningOnOUYASupportedHardware()
	{
		boolean result = false;
		try
		{
			//Log.i(TAG, "isRunningOnOUYASupportedHardware");
			if (null == IOuyaActivity.GetUnityOuyaFacade())
			{
				Log.e(TAG, "isRunningOnOUYASupportedHardware: unityOuyaFacade is null");
			}
			else
			{
				result = IOuyaActivity.GetUnityOuyaFacade().isRunningOnOUYASupportedHardware();
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "isRunningOnOUYASupportedHardware exception: " + e.toString());
		}
		return result;
	}

	private static int getDisplayWidth() {
		Activity activity = IOuyaActivity.GetActivity();
		WindowManager windowManager = activity.getWindowManager();
		Display display = windowManager.getDefaultDisplay();
		Point size = new Point();
		display.getSize(size);
		return size.x;
	}

	private static int getDisplayHeight() {
		Activity activity = IOuyaActivity.GetActivity();
		WindowManager windowManager = activity.getWindowManager();
		Display display = windowManager.getDefaultDisplay();
		Point size = new Point();
		display.getSize(size);
		return size.y;
	}

	private static void updateSafeArea(float progress) {
		// bring in by %
		float percent = 0.1f;
		float ratio = 1 - (1 - progress) * percent;
		float halfRatio = 1 - (1 - progress) * percent * 0.5f;
		float maxWidth = getDisplayWidth();
		float maxHeight = getDisplayHeight();
		Activity activity = IOuyaActivity.GetActivity();
		FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
		LayoutParams layout = content.getLayoutParams();
		layout.width = (int)(maxWidth * ratio);
		layout.height = (int)(maxHeight * ratio);
		content.setLayoutParams(layout);
		content.setX(maxWidth - maxWidth * halfRatio);
		content.setY(maxHeight - maxHeight * halfRatio);
	}

	public static void setSafeArea(final float percentage)
	{
		try
		{
			//Log.i(TAG, "setSafeArea: "+percentage);
			Activity activity = IOuyaActivity.GetActivity();
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

	public static void clearFocus()
	{
		try
		{
			//Log.i(TAG, "clearFocus");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						View view = activity.getCurrentFocus();
						if (null != view) {
							view.setFocusable(false);
							view.clearFocus();
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "clearFocus: exception=" + e.toString());
		}
	}


	public static OuyaContent getOuyaContent() {
		return IOuyaActivity.GetOuyaContent();
	}

	public static void saveOuyaMod(final OuyaMod ouyaMod, final OuyaMod.Editor editor) {
		try
		{
			//Log.i(TAG, "saveOuyaMod");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							unityOuyaFacade.saveOuyaMod(ouyaMod, editor);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "saveOuyaMod: exception=" + e.toString());
		}
	}

	public static void getOuyaContentInstalled() {
		try
		{
			//Log.i(TAG, "getOuyaContentInstalled");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						OuyaContent ouyaContent = IOuyaActivity.GetOuyaContent();
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							unityOuyaFacade.getOuyaContentInstalled();
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "getOuyaContentInstalled: exception=" + e.toString());
		}
	}

	public static OuyaMod[] getOuyaContentInstalledResults() {
		List<OuyaMod> result = IOuyaActivity.GetOuyaContentInstalledResults();
		if (null == result) {
			return null;
		}
		OuyaMod[] retVal = new OuyaMod[result.size()];
		result.toArray(retVal);
		return retVal;
	}

	public static void getOuyaContentPublished(final String sortMethod) {
		try
		{
			//Log.i(TAG, "getOuyaContentPublished");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						OuyaContent ouyaContent = IOuyaActivity.GetOuyaContent();
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							OuyaContent.SortMethod sort = OuyaContent.SortMethod.valueOf(sortMethod);
							//Log.i(TAG, "sortMethod="+sortMethod);
							unityOuyaFacade.getOuyaContentPublished(sort);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "getOuyaContentPublished: exception=" + e.toString());
		}
	}

	public static OuyaMod[] getOuyaContentPublishedResults() {
		List<OuyaMod> result = IOuyaActivity.GetOuyaContentPublishedResults();
		if (null == result) {
			Log.e(TAG, "getOuyaContentPublishedResults result is null."); 
			return null;
		}
		//Log.i(TAG, "getOuyaContentPublishedResults returning size="+result.size()); 
		OuyaMod[] retVal = new OuyaMod[result.size()];
		result.toArray(retVal);
		return retVal;
	}

	public static void contentDelete(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentDelete");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							unityOuyaFacade.contentDelete(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentDelete: exception=" + e.toString());
		}
	}

	public static void contentPublish(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentPublish");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							unityOuyaFacade.contentPublish(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentPublish: exception=" + e.toString());
		}
	}

	public static void contentUnpublish(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentUnpublish");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							unityOuyaFacade.contentUnpublish(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentUnpublish: exception=" + e.toString());
		}
	}

	public static void contentDownload(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentDownload");
			final Activity activity = IOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
						if (null == unityOuyaFacade) {
							Log.e(TAG, "unityOuyaFacade is null");
						} else {
							unityOuyaFacade.contentDownload(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentDownload: exception=" + e.toString());
		}
	}

	public static float getFloat(Float f) {
		if (null == f) {
			return 0;
		}
		return f.floatValue();
	}

	public static Bitmap[] getBitmapArray(List<Bitmap> list) {
		if (null == list) {
			return new Bitmap[0];
		}
		Bitmap[] retVal = new Bitmap[list.size()];
		list.toArray(retVal);
		return retVal;
	}

	public static OuyaModScreenshot[] getOuyaModScreenshotArray(List<OuyaModScreenshot> list) {
		if (null == list) {
			return new OuyaModScreenshot[0];
		}
		OuyaModScreenshot[] retVal = new OuyaModScreenshot[list.size()];
		list.toArray(retVal);
		return retVal;
	}

	public static String[] getStringArray(List<String> list) {
		if (null == list) {
			return new String[0];
		}
		String[] retVal = new String[list.size()];
		list.toArray(retVal);
		return retVal;
	}

	public static String getStringResource(String name) {
		final Activity activity = IOuyaActivity.GetActivity();
		if (null == activity) {
			return "";
		}
		Resources resources = activity.getResources();
		if (null == resources) {
			return "";
		}
		int id = resources.getIdentifier(name, "string", activity.getPackageName());
		if (id <= 0) {
			return "";
		}
		return resources.getString(id);
	}
}
