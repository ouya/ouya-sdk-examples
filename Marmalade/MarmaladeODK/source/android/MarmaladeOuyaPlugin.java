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

package tv.ouya.sdk.marmalade;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.WindowManager;
import android.widget.FrameLayout;
import java.io.File;
import java.util.ArrayList;
import org.json.JSONArray;
import org.json.JSONObject;

import tv.ouya.console.api.*;

public class MarmaladeOuyaPlugin
{
	private static final String TAG = MarmaladeOuyaPlugin.class.getSimpleName();

	public MarmaladeOuyaPlugin() {
	}

	// most of the java functions that are called, need the ouya facade initialized
	public static void InitializePlugin(String jsonData)
		throws Exception
	{
		try		
		{
			if (null == IMarmaladeOuyaActivity.GetActivity())
			{
				Log.e(TAG, "InitializePlugin: activity is null");
				return;
			}

			//wait to read the application key
			if (null == IMarmaladeOuyaActivity.GetApplicationKey())
			{
				Log.e(TAG, "InitializePlugin: application key is null");
				return;
			}

			Bundle developerInfo = new Bundle();

			developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, IMarmaladeOuyaActivity.GetApplicationKey());

			JSONArray jsonArray = new JSONArray(jsonData);
			for (int index = 0; index < jsonArray.length(); ++index) {
				JSONObject jsonObject = jsonArray.getJSONObject(index);
				String name = jsonObject.getString("key");
				String value = jsonObject.getString("value");
				developerInfo.putString(name, value);
			}

			MarmaladeOuyaFacade marmaladeOuyaFacade =
				new MarmaladeOuyaFacade(IMarmaladeOuyaActivity.GetActivity(),
					IMarmaladeOuyaActivity.GetSavedInstanceState(),
					developerInfo);
					
			//make facade accessible by activity
			IMarmaladeOuyaActivity.SetMarmaladeOuyaFacade(marmaladeOuyaFacade);

			Log.i(TAG, "InitializePlugin Complete.");
		}
		catch (Exception e) 
		{
			Log.i(TAG, "InitializePlugin: exception: " + e.toString());
			throw e;
		}
	}
	
	public static void requestGamerInfo()
	{
		try
		{
			Log.i(TAG, "requestGamerInfo");

			if (null == IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.i(TAG, "requestGamerInfo: MarmaladeOuyaFacade is null");
				return;
			}

			IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade().requestGamerInfo();
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
			Log.i(TAG, "MarmaladeOuyaPlugin.getProductsAsync");

			if (null == IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.e(TAG, "getProductsAsync: MarmaladeOuyaFacade is null");
				return;
			}

			IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade().requestProducts(products);
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
		
			if (null == IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.e(TAG, "requestPurchaseAsync: MarmaladeOuyaFacade is null");
				return;
			}

			IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade().requestPurchase(product);
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

			if (null == IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.e(TAG, "getReceiptsAsync: MarmaladeOuyaFacade is null");
				return;
			}

			IMarmaladeOuyaActivity.GetMarmaladeOuyaFacade().requestReceipts();
		}
		catch (Exception ex) 
		{
			Log.e(TAG, "getReceiptsAsync: exception: " + ex.toString());
		}
	}
}
