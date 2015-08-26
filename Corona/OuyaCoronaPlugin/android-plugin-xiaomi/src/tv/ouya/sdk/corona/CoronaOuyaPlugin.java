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
import android.content.res.Resources;
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
	private static final String TAG = CoronaOuyaPlugin.class.getSimpleName();

	private static final String VERSION = "1501.3";

	private static final boolean mEnableLogging = false;

	private static boolean sInitialized = false;

	// most of the java functions that are called, need the ouya facade initialized
	public static void initOuyaPlugin(String jsonData)
		throws Exception
	{
		try {
			if (sInitialized) {
				Log.i(TAG, "Corona Plugin Already Initialized");
			} else {
				Log.i(TAG, "VERSION="+VERSION);

				if (mEnableLogging) {
					Log.i(TAG, "initOuyaPlugin jsonData="+jsonData);
				}

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
					if (null == name ||
						null == value) {
						continue;
					}
					if (name.equals("tv.ouya.product_id_list")) {
						if (mEnableLogging) {
							Log.i(TAG, "key="+name);
						}
						String[] productIds = value.split(",");
						if (null == productIds) {
							continue;
						}
						if (mEnableLogging) {
							for (String productId : productIds) {
								Log.i(TAG, " value="+productId);
							}
						}
						developerInfo.putStringArray("tv.ouya.product_id_list", productIds);
					} else {
						if (mEnableLogging) {
							Log.i(TAG, "key="+name+" value="+value);
						}
						developerInfo.putString(name, value);
					}
				}

				//Log.i(TAG, "Developer info was set.");
				
				CoronaOuyaFacade coronaOuyaFacade =
					new CoronaOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), developerInfo);
				
				//make facade accessible by activity
				IOuyaActivity.SetCoronaOuyaFacade(coronaOuyaFacade);

				Log.i(TAG, "Corona Plugin Initialized.");

				sInitialized = true;
			}
		}
		catch (Exception e) {
			Log.e(TAG, "initOuyaPlugin exception: " + e.toString());
			throw e;
		}
	}

	public static void requestGamerInfo()
	{
		try {
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.e(TAG, "CoronaOuyaPlugin.requestGamerInfo: CoronaOuyaFacade is null");
			}
			else
			{
				
				IOuyaActivity.GetCoronaOuyaFacade().requestGamerInfo();
			}
		}
		catch (Exception ex) {
			Log.e(TAG, "CoronaOuyaPlugin: requestGamerInfo exception: " + ex.toString());
		}
	}

	public static void getProductsAsync(ArrayList<Purchasable> products)
	{
		try {
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.e(TAG, "CoronaOuyaPlugin.getProductsAsync: CoronaOuyaFacade is null");
			}
			else
			{
				IOuyaActivity.GetCoronaOuyaFacade().requestProducts(products);
			}
		}
		catch (Exception ex) {
			Log.e(TAG, "CoronaOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static String requestPurchaseAsync(String identifier)
	{
		try {
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.e(TAG, "CoronaOuyaPlugin.requestPurchaseAsync: CoronaOuyaFacade is null");
			}
			else
			{
				Product product = new Product(identifier, "", 0, 0, "", 0, 0, "", "", Product.Type.ENTITLEMENT);
				
				IOuyaActivity.GetCoronaOuyaFacade().requestPurchase(product);
			}
		}
		catch (Exception ex) {
			Log.e(TAG, "requestPurchaseAsync exception: " + ex.toString());
		}
		return "";
	}

	public static void getReceiptsAsync()
	{
		try {
			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.e(TAG, "CoronaOuyaPlugin.getReceiptsAsync: CoronaOuyaFacade is null");
			}
			else
			{
				IOuyaActivity.GetCoronaOuyaFacade().requestReceipts();
			}
		}
		catch (Exception ex) {
			Log.e(TAG, "CoronaOuyaPlugin: getReceiptsAsync exception: " + ex.toString());
		}
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

	public static String getDeviceHardwareName() {
		try {
			if (null == IOuyaActivity.GetCoronaOuyaFacade()) {
				Log.e(TAG, "CoronaOuyaPlugin.getDeviceHardwareName: CoronaOuyaFacade is null");
			} else {
				return IOuyaActivity.GetCoronaOuyaFacade().getDeviceHardwareName();
			}
		}
		catch (Exception ex) {
			Log.e(TAG, "CoronaOuyaPlugin: getDeviceHardwareName exception: " + ex.toString());
		}
		return "";	
	}

	public static boolean isAvailable() {
		return CoronaOuyaActivity.isAvailable();
	}
}
