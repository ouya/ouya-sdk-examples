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

package com.ODK;


import android.app.Activity;
import android.content.Intent;
import android.util.Log;
import android.view.WindowManager;
import android.widget.FrameLayout;

import java.io.File;

import tv.ouya.console.api.*;

public class MarmaladeOuyaPlugin
{
	private static final String TAG = "MarmaladeOuyaPlugin";

	// the developer id is sent from marmalade code
	private static String m_developerId = "";

	// For debugging enable logging for testing
	private static final Boolean m_enableDebugLogging = false;

	public MarmaladeOuyaPlugin() {
	}

	// most of the java functions that are called, need the ouya facade initialized
	public static void Initialize()
	{
		try
		{
			if (null == IOuyaActivity.GetActivity())
			{
				if (m_enableDebugLogging)
				{
					Log.i(TAG, "MarmaladeOuyaPlugin.Initialize: IOuyaActivity.GetActivity() is null");
				}
				return;
			}

			if (null == IOuyaActivity.GetMarmaladeOuyaFacade())
			{
				//wait to read the application key
				if (null == IOuyaActivity.GetApplicationKey())
				{
				}
				else
				{
					if (m_enableDebugLogging)
					{
						Log.i(TAG, "Initialize: constructing MarmaladeOuyaFacade");
					}

					MarmaladeOuyaFacade marmaladeOuyaFacade =
						new MarmaladeOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), m_developerId, IOuyaActivity.GetApplicationKey());

					//make facade accessible by activity
					IOuyaActivity.SetMarmaladeOuyaFacade(marmaladeOuyaFacade);

					Log.i(TAG, "MarmaladeOuyaPlugin.Initialize: Complete");
				}
			}
		}
		catch (Exception ex)
		{
			Log.i(TAG, "Initialize: Exception: " + ex.toString());
		}
	}

	public static String getDeveloperId()
	{
		return m_developerId;
	}

	public static String setDeveloperId(String developerId)
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "setDeveloperId developerId: " + developerId);
			}
			m_developerId = developerId;
		}
		catch (Exception ex)
		{
			Log.i(TAG, "setDeveloperId exception: " + ex.toString());
		}
		return "";
	}

	public static void fetchGamerUUID()
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "MarmaladeOuyaPlugin.fetchGamerUUID");
			}

			if (null == IOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.i(TAG, "MarmaladeOuyaPlugin.fetchGamerUUID: MarmaladeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "MarmaladeOuyaPlugin.fetchGamerUUID: MarmaladeOuyaFacade is valid");
				}

				if (m_developerId == "") {
					Log.i(TAG, "MarmaladeOuyaPlugin.m_developerId is not set");
				} else {
					if (m_enableDebugLogging) {
						Log.i(TAG, "MarmaladeOuyaPlugin.m_developerId valid: " + m_developerId);
					}
				}

				IOuyaActivity.GetMarmaladeOuyaFacade().fetchGamerUUID();
			}
		}
		catch (Exception ex)
		{
			Log.i(TAG, "MarmaladeOuyaPlugin: fetchGamerUUID exception: " + ex.toString());
		}
	}

	public static void getProductsAsync()
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "MarmaladeOuyaPlugin.getProductsAsync");
			}

			if (null == IOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.i(TAG, "MarmaladeOuyaPlugin.getProductsAsync: MarmaladeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "MarmaladeOuyaPlugin.getProductsAsync: MarmaladeOuyaFacade is valid");
				}
				IOuyaActivity.GetMarmaladeOuyaFacade().requestProducts();
			}
		}
		catch (Exception ex)
		{
			Log.i(TAG, "MarmaladeOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static void clearGetProductList()
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "clearGetProductList");
			}

			MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST.clear();
		}
		catch (Exception ex)
		{
			Log.i(TAG, "clearGetProductList exception: " + ex.toString());
		}
	}

	public static void addGetProduct(String productId)
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "addGetProduct productId: " + productId);
			}

			boolean found = false;
			for (Purchasable purchasable : MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				//Log.i(TAG, "addGetProduct " + purchasable.getProductId() + "==" + productId);
				if (purchasable.getProductId().equals(productId))
				{
					//Log.i(TAG, "addGetProduct equals: " + purchasable.getProductId() + "==" + productId + "=" + purchasable.getProductId().equals(productId));
					found = true;
					break;
				}
			}
			if (found)
			{
				//Log.i(TAG, "addGetProduct found productId: " + productId);
			}
			else
			{
				//Log.i(TAG, "addGetProduct added productId: " + productId);
				Purchasable newPurchasable = new Purchasable(new String(productId));
				MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST.add(newPurchasable);
			}
		}
		catch (Exception ex)
		{
			Log.i(TAG, "addGetProduct exception: " + ex.toString());
		}
	}

	public static void debugGetProductList()
	{
		try
		{
			int count = 0;
			for (Purchasable purchasable : MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				++count;
			}
			Log.i(TAG, "debugProductList MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST has " + count + " elements");
			for (Purchasable purchasable : MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				Log.i(TAG, "debugProductList MarmaladeOuyaFacade.PRODUCT_IDENTIFIER_LIST has: " + purchasable.getProductId());
			}
		}
		catch (Exception ex)
		{
			Log.i(TAG, "debugProductList exception: " + ex.toString());
		}
	}

	public static String requestPurchaseAsync(String identifier)
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "requestPurchaseAsync identifier: " + identifier);
			}

			if (null == IOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.i(TAG, "MarmaladeOuyaPlugin.requestPurchaseAsync: MarmaladeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "MarmaladeOuyaPlugin.requestPurchaseAsync: MarmaladeOuyaFacade is valid");
				}
				Product product = new Product();
				product.setIdentifier(identifier);
				IOuyaActivity.GetMarmaladeOuyaFacade().requestPurchase(product);
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
			if (m_enableDebugLogging) {
				Log.i(TAG, "MarmaladeOuyaPlugin.getReceiptsAsync");
			}

			if (null == IOuyaActivity.GetMarmaladeOuyaFacade())
			{
				Log.i(TAG, "MarmaladeOuyaPlugin.getReceiptsAsync: MarmaladeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "MarmaladeOuyaPlugin.getReceiptsAsync: MarmaladeOuyaFacade is valid");
				}
				IOuyaActivity.GetMarmaladeOuyaFacade().requestReceipts();
			}
		}
		catch (Exception ex)
		{
			Log.i(TAG, "MarmaladeOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}
}