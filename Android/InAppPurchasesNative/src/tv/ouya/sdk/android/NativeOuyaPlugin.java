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

package tv.ouya.sdk.android;


import android.app.Activity;
import android.content.Intent;
import android.util.Log;
import android.view.WindowManager;
import android.widget.FrameLayout;

import java.io.File;

import tv.ouya.console.api.*;

public class NativeOuyaPlugin
{
	private static final String TAG = "NativeOuyaPlugin";

	// the developer id is sent from native code
	private static String m_developerId = "";

	// For debugging enable logging for testing
	private static final Boolean m_enableDebugLogging = false;
	
	public NativeOuyaPlugin() {
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
					Log.i(TAG, "NativeOuyaPlugin.Initialize: IOuyaActivity.GetActivity() is null");
				}
				return;
			}

			if (null == IOuyaActivity.GetNativeOuyaFacade())
			{
				//wait to read the application key
				if (null == IOuyaActivity.GetApplicationKey())
				{
				}
				else
				{
					if (m_enableDebugLogging)
					{
						Log.i(TAG, "Initialize: constructing NativeOuyaFacade");
					}

					NativeOuyaFacade nativeOuyaFacade =
						new NativeOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), m_developerId, IOuyaActivity.GetApplicationKey());
					
					//make facade accessible by activity
					IOuyaActivity.SetNativeOuyaFacade(nativeOuyaFacade);

					//Log.i(TAG, "NativeOuyaPlugin.Initialize: OuyaGameObject send SendIAPInitComplete");
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
				Log.i(TAG, "NativeOuyaPlugin.fetchGamerUUID");
			}

			if (null == IOuyaActivity.GetNativeOuyaFacade())
			{
				Log.i(TAG, "NativeOuyaPlugin.fetchGamerUUID: NativeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "NativeOuyaPlugin.fetchGamerUUID: NativeOuyaFacade is valid");
				}
				
				if (m_developerId == "") {
					Log.i(TAG, "NativeOuyaPlugin.m_developerId is not set");
				} else {
					if (m_enableDebugLogging) {
						Log.i(TAG, "NativeOuyaPlugin.m_developerId valid: " + m_developerId);
					}
				}
				
				IOuyaActivity.GetNativeOuyaFacade().fetchGamerUUID();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "NativeOuyaPlugin: fetchGamerUUID exception: " + ex.toString());
		}
	}

	public static void getProductsAsync()
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "NativeOuyaPlugin.getProductsAsync");
			}

			if (null == IOuyaActivity.GetNativeOuyaFacade())
			{
				Log.i(TAG, "NativeOuyaPlugin.getProductsAsync: NativeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "NativeOuyaPlugin.getProductsAsync: NativeOuyaFacade is valid");
				}
				IOuyaActivity.GetNativeOuyaFacade().requestProducts();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "NativeOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static void clearGetProductList()
	{
		try
		{
			if (m_enableDebugLogging) {
				Log.i(TAG, "clearGetProductList");
			}
		
			NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST.clear();
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
			for (Purchasable purchasable : NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST)
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
				NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST.add(newPurchasable);
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
			for (Purchasable purchasable : NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				++count;
			}
			Log.i(TAG, "debugProductList NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST has " + count + " elements");
			for (Purchasable purchasable : NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				Log.i(TAG, "debugProductList NativeOuyaFacade.PRODUCT_IDENTIFIER_LIST has: " + purchasable.getProductId());
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
		
			if (null == IOuyaActivity.GetNativeOuyaFacade())
			{
				Log.i(TAG, "NativeOuyaPlugin.requestPurchaseAsync: NativeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "NativeOuyaPlugin.requestPurchaseAsync: NativeOuyaFacade is valid");
				}
				Product product = new Product();
				product.setIdentifier(identifier);
				IOuyaActivity.GetNativeOuyaFacade().requestPurchase(product);
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
				Log.i(TAG, "NativeOuyaPlugin.getReceiptsAsync");
			}

			if (null == IOuyaActivity.GetNativeOuyaFacade())
			{
				Log.i(TAG, "NativeOuyaPlugin.getReceiptsAsync: NativeOuyaFacade is null");
			}
			else
			{
				if (m_enableDebugLogging) {
					Log.i(TAG, "NativeOuyaPlugin.getReceiptsAsync: NativeOuyaFacade is valid");
				}
				IOuyaActivity.GetNativeOuyaFacade().requestReceipts();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "NativeOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}
}