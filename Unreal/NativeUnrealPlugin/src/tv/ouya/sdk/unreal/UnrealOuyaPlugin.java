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

package tv.ouya.sdk.unreal;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Point;
import android.os.Bundle;
import android.util.Log;
import android.view.Display;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.FrameLayout;
import java.io.File;
import java.util.ArrayList;
import org.json.JSONArray;
import org.json.JSONObject;

import tv.ouya.console.api.*;

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
			Log.i(TAG, "InitializePlugin: exception: " + e.toString());
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
			Log.i(TAG, "InitializeContent: exception: " + e.toString());
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
		catch (Exception ex)
		{
			Log.i(TAG, "requestGamerInfo exception: " + ex.toString());
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
		catch (Exception ex)
		{
			Log.i(TAG, "getProductsAsync exception: " + ex.toString());
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
		catch (Exception ex)
		{
			Log.i(TAG, "requestPurchaseAsync: exception: " + ex.toString());
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
		catch (Exception ex)
		{
			Log.e(TAG, "getReceiptsAsync: exception: " + ex.toString());
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
}
