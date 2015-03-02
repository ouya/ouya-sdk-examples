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

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.WindowManager;
import android.widget.FrameLayout;
import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import org.json.JSONArray;
import org.json.JSONObject;
import tv.ouya.console.api.*;

public class CoronaOuyaPlugin
{
	private static final String TAG = "OuyaCoronaPlugin";

	// most of the java functions that are called, need the ouya facade initialized
	public static void initOuyaPlugin(String jsonData)
		throws Exception
	{
		try		
		{
			if (null == IOuyaActivity.GetActivity())
			{
				throw new Exception("Activity is not set");
			}

			if (null == IOuyaActivity.GetApplicationKey())
			{
				throw new Exception("Signing key is not set");
			}

			Bundle developerInfo = new Bundle();

	        developerInfo.putByteArray(OuyaFacade.OUYA_DEVELOPER_PUBLIC_KEY, IOuyaActivity.GetApplicationKey());
	        
	        JSONArray jsonArray = new JSONArray(jsonData);
			for (int index = 0; index < jsonArray.length(); ++index) {
				JSONObject jsonObject = jsonArray.getJSONObject(index);
				String name = jsonObject.getString("key");
				String value = jsonObject.getString("value");
				//Log.i(TAG, "key="+key+" value="+value);
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

			//Log.i(TAG, "Developer info was set.");
			
			CoronaOuyaFacade coronaOuyaFacade =
				new CoronaOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), developerInfo);
			
			//make facade accessible by activity
			IOuyaActivity.SetCoronaOuyaFacade(coronaOuyaFacade);

			Log.i(TAG, "Corona Plugin Initialized.");
		}
		catch (Exception e) 
		{
			Log.i(TAG, "initOuyaPlugin exception: " + e.toString());
			throw e;
		}
	}

	public static void requestGamerInfo()
	{
		try
		{
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestGamerInfo: CoronaOuyaFacade is null");
			}
			else
			{
				
				IOuyaActivity.GetCoronaOuyaFacade().requestGamerInfo();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "CoronaOuyaPlugin: requestGamerInfo exception: " + ex.toString());
		}
	}

	public static void getProductsAsync(ArrayList<Purchasable> products)
	{
		try
		{
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.getProductsAsync: CoronaOuyaFacade is null");
			}
			else
			{
				IOuyaActivity.GetCoronaOuyaFacade().requestProducts(products);
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "CoronaOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static String requestPurchaseAsync(String identifier)
	{
		try
		{
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestPurchaseAsync: CoronaOuyaFacade is null");
			}
			else
			{
				Product product = new Product(identifier, "", 0, 0, "", 0, 0, "", "", Product.Type.ENTITLEMENT);
				
				IOuyaActivity.GetCoronaOuyaFacade().requestPurchase(product);
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "requestPurchaseAsync exception: " + ex.toString());
		}
		return "";
	}

	public static void getReceiptsAsync()
	{
		try
		{
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.getReceiptsAsync: CoronaOuyaFacade is null");
			}
			else
			{
				IOuyaActivity.GetCoronaOuyaFacade().requestReceipts();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "CoronaOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}
}