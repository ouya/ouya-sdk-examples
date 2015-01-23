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

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Point;
import android.os.Bundle;
import android.util.Log;
import android.view.Display;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.FrameLayout;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import tv.ouya.console.api.*;
import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaMod;
import tv.ouya.console.api.content.OuyaModScreenshot;

public class UnrealOuyaPlugin
{
	private static final String TAG = UnrealOuyaPlugin.class.getSimpleName();

	public UnrealOuyaPlugin() {
	}

	// most of the java functions that are called, need the ouya facade initialized
	public static void InitializePlugin(String jsonData)
		throws Exception
	{
		try
		{
			if (null == IUnrealOuyaActivity.GetActivity())
			{
				Log.e(TAG, "InitializePlugin: activity is null");
				return;
			}

			//wait to read the application key
			if (null == IUnrealOuyaActivity.GetApplicationKey())
			{
				Log.e(TAG, "InitializePlugin: application key is null");
				return;
			}

			Bundle developerInfo = new Bundle();

			developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, IUnrealOuyaActivity.GetApplicationKey());

			JSONArray jsonArray = new JSONArray(jsonData);
			for (int index = 0; index < jsonArray.length(); ++index) {
				JSONObject jsonObject = jsonArray.getJSONObject(index);
				String name = jsonObject.getString("key");
				String value = jsonObject.getString("value");
				developerInfo.putString(name, value);
			}

			UnrealOuyaFacade unrealOuyaFacade =
				new UnrealOuyaFacade(IUnrealOuyaActivity.GetActivity(),
					IUnrealOuyaActivity.GetSavedInstanceState(),
					developerInfo);

			//make facade accessible by activity
			IUnrealOuyaActivity.SetUnrealOuyaFacade(unrealOuyaFacade);

			Log.i(TAG, "InitializePlugin Complete.");
		}
		catch (Exception e)
		{
			Log.e(TAG, "InitializePlugin: exception");
			e.printStackTrace();
			throw e;
		}
	}

	public static void InitializeContent()
		throws Exception
	{
		try
		{
			if (null == IUnrealOuyaActivity.GetUnrealOuyaFacade())
			{
				Log.e(TAG, "InitializeContent: facade is null");
				return;
			}			

			IUnrealOuyaActivity.GetUnrealOuyaFacade().initContent();

			Log.i(TAG, "InitializeContent Complete.");
		}
		catch (Exception e)
		{
			Log.e(TAG, "InitializeContent: exception");
			e.printStackTrace();
			throw e;
		}
	}

	public static void requestGamerInfo()
	{
		try
		{
			Log.i(TAG, "requestGamerInfo");

			if (null == IUnrealOuyaActivity.GetUnrealOuyaFacade())
			{
				Log.i(TAG, "requestGamerInfo: UnrealOuyaFacade is null");
				return;
			}

			IUnrealOuyaActivity.GetUnrealOuyaFacade().requestGamerInfo();
		}
		catch (Exception e)
		{
			Log.e(TAG, "requestGamerInfo exception");
			e.printStackTrace();
		}
	}

	public static void requestProductListAsync(ArrayList<Purchasable> products)
	{
		try
		{
			Log.i(TAG, "UnrealOuyaPlugin.getProductsAsync");

			if (null == IUnrealOuyaActivity.GetUnrealOuyaFacade())
			{
				Log.e(TAG, "getProductsAsync: UnrealOuyaFacade is null");
				return;
			}

			IUnrealOuyaActivity.GetUnrealOuyaFacade().requestProducts(products);
		}
		catch (Exception e)
		{
			Log.e(TAG, "getProductsAsync exception");
			e.printStackTrace();
		}
	}

	public static void requestPurchaseAsync(Product product)
	{
		try
		{
			Log.i(TAG, "requestPurchaseAsync identifier: " + product.getIdentifier());

			if (null == IUnrealOuyaActivity.GetUnrealOuyaFacade())
			{
				Log.e(TAG, "requestPurchaseAsync: UnrealOuyaFacade is null");
				return;
			}

			IUnrealOuyaActivity.GetUnrealOuyaFacade().requestPurchase(product);
		}
		catch (Exception e)
		{
			Log.e(TAG, "requestPurchaseAsync: exception");
			e.printStackTrace();
		}
	}

	public static void getReceiptsAsync()
	{
		try
		{
			Log.i(TAG, "getReceiptsAsync");

			if (null == IUnrealOuyaActivity.GetUnrealOuyaFacade())
			{
				Log.e(TAG, "getReceiptsAsync: UnrealOuyaFacade is null");
				return;
			}

			IUnrealOuyaActivity.GetUnrealOuyaFacade().requestReceipts();
		}
		catch (Exception e)
		{
			Log.e(TAG, "getReceiptsAsync: exception");
			e.printStackTrace();
		}
	}

	private static int getDisplayWidth() {
		Activity activity = IUnrealOuyaActivity.GetActivity();
		if (null == activity)
		{
			Log.e(TAG, "getDisplayWidth: activity is null");
			return 0;
		}
		WindowManager windowManager = activity.getWindowManager();
		Display display = windowManager.getDefaultDisplay();
		Point size = new Point();
		display.getSize(size);
		return size.x;
	}
	private static int getDisplayHeight() {
		Activity activity = IUnrealOuyaActivity.GetActivity();
		if (null == activity)
		{
			Log.e(TAG, "getDisplayHeight: activity is null");
			return 0;
		}
		WindowManager windowManager = activity.getWindowManager();
		Display display = windowManager.getDefaultDisplay();
		Point size = new Point();
		display.getSize(size);
		return size.y;
	}

	public static boolean updateSafeArea(final float progress) {
		final Activity activity = IUnrealOuyaActivity.GetActivity();
		if (null == activity)
		{
			Log.e(TAG, "updateSafeArea: activity is null");
			return false;
		}
		Runnable runnable = new Runnable() {
			public void run() {
				// bring in by %
				float percent = 0.1f;
				float ratio = 1 - (1 - progress) * percent;
				float halfRatio = 1 - (1 - progress) * percent * 0.5f;
				float maxWidth = getDisplayWidth();
				float maxHeight = getDisplayHeight();
				FrameLayout content = (FrameLayout)activity.findViewById(android.R.id.content);
				ViewGroup.LayoutParams layout = content.getLayoutParams();
				layout.width = (int)(maxWidth * ratio);
				layout.height = (int)(maxHeight * ratio);
				content.setLayoutParams(layout);
				content.setX(maxWidth - maxWidth * halfRatio);
				content.setY(maxHeight - maxHeight * halfRatio);
			}
		};
		activity.runOnUiThread(runnable);
		return true;
	}

	public static boolean shutdown() {
		final Activity activity = IUnrealOuyaActivity.GetActivity();
		if (null == activity)
		{
			Log.e(TAG, "shutdown: activity is null");
			return false;
		}
		Runnable runnable = new Runnable() {
			public void run() {
				activity.finish();
			}
		};
		activity.runOnUiThread(runnable);
		return true;
	}
	
	public static OuyaContent getOuyaContent() {
		return IUnrealOuyaActivity.GetOuyaContent();
	}

	public static void saveOuyaMod(final OuyaMod.Editor editor, final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "saveOuyaMod");
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							unrealOuyaFacade.saveOuyaMod(ouyaMod, editor);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "saveOuyaMod: exception");
			e.printStackTrace();
		}
	}

	public static void getOuyaContentInstalled() {
		try
		{
			//Log.i(TAG, "getOuyaContentInstalled");
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						OuyaContent ouyaContent = IUnrealOuyaActivity.GetOuyaContent();
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							unrealOuyaFacade.getOuyaContentInstalled();
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "getOuyaContentInstalled: exception");
			e.printStackTrace();
		}
	}

	public static OuyaMod[] getOuyaContentInstalledResults() {
		List<OuyaMod> result = IUnrealOuyaActivity.GetOuyaContentInstalledResults();
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
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							OuyaContent.SortMethod sort = OuyaContent.SortMethod.valueOf(sortMethod);
							//Log.i(TAG, "sortMethod="+sortMethod);
							unrealOuyaFacade.getOuyaContentPublished(sort);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "getOuyaContentPublished: exception");
			e.printStackTrace();
		}
	}

	public static OuyaMod[] getOuyaContentPublishedResults() {
		List<OuyaMod> result = IUnrealOuyaActivity.GetOuyaContentPublishedResults();
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
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							unrealOuyaFacade.contentDelete(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentDelete: exception");
			e.printStackTrace();
		}
	}

	public static void contentPublish(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentPublish");
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							unrealOuyaFacade.contentPublish(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentPublish: exception");
			e.printStackTrace();
		}
	}

	public static void contentUnpublish(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentUnpublish");
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							unrealOuyaFacade.contentUnpublish(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentUnpublish: exception");
			e.printStackTrace();
		}
	}

	public static void contentDownload(final OuyaMod ouyaMod) {
		try
		{
			//Log.i(TAG, "contentDownload");
			final Activity activity = IUnrealOuyaActivity.GetActivity();
			if (null != activity) {
				Runnable runnable = new Runnable()
				{
					public void run()
					{
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "unrealOuyaFacade is null");
						} else {
							unrealOuyaFacade.contentDownload(ouyaMod);
						}
					}
				};
				activity.runOnUiThread(runnable);			
			}
		}
		catch (Exception e)
		{
			Log.e(TAG, "contentDownload: exception");
			e.printStackTrace();
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
}
