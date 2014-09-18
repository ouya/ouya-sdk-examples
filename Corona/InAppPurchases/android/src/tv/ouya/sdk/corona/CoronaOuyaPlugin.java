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
import android.util.Log;
import android.view.WindowManager;
import android.widget.FrameLayout;

import java.io.File;

import tv.ouya.console.api.*;

public class CoronaOuyaPlugin
{
	private static final String TAG = "OuyaCoronaPlugin";

	// the developer id is sent from Corona
	private static String m_developerId = "";

	// ENABLE IAP mode this is for testing/debugging to turn off IAP
	private static Boolean m_enableIAP = true;

	// For debugging enable logging for testing
	private static Boolean m_enableDebugLogging = true;
	
	public CoronaOuyaPlugin() {
	}

	// most of the java functions that are called, need the ouya facade initialized
	public static void InitializeTest()
	{
		try		
		{
			if (null == IOuyaActivity.GetActivity())
			{
				if (m_enableDebugLogging)
				{
					Log.w(TAG, "CoronaOuyaPlugin.InitializeTest: IOuyaActivity.GetActivity() is null");
				}
				return;
			}

			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				//wait to read the application key
				if (null == IOuyaActivity.GetApplicationKey())
				{
				}
				else
				{
					if (m_enableDebugLogging)
					{
						Log.i(TAG, "InitializeTest: Corona has initialized, constructing CoronaOuyaFacade");
					}
					
					CoronaOuyaFacade coronaOuyaFacade =
						new CoronaOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), m_developerId, IOuyaActivity.GetApplicationKey());
					
					//make facade accessible by activity
					IOuyaActivity.SetCoronaOuyaFacade(coronaOuyaFacade);

					Log.i(TAG, "CoronaOuyaPlugin.Initialization Complete.");
				}
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "InitializeTest: InitializeTest exception: " + ex.toString());
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
			//Log.i(TAG, "setDeveloperId developerId: " + developerId);
			m_developerId = developerId;
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "setDeveloperId exception: " + ex.toString());
		}
		return "";
	}

	public static void requestGamerInfo()
	{
		try
		{
			Log.i(TAG, "CoronaOuyaPlugin.requestGamerInfo");

			if (!m_enableIAP)
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestGamerInfo IAP is disabled");
				return;
			}

			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestGamerInfo: CoronaOuyaFacade is null");
			}
			else
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestGamerInfo: CoronaOuyaFacade is valid");
				
				if (m_developerId == "") {
					Log.i(TAG, "CoronaOuyaPlugin.m_developerId is not set");
				} else {
					Log.i(TAG, "CoronaOuyaPlugin.m_developerId valid: " + m_developerId);
				}
				
				IOuyaActivity.GetCoronaOuyaFacade().requestGamerInfo();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "CoronaOuyaPlugin: requestGamerInfo exception: " + ex.toString());
		}
	}

	public static void getProductsAsync()
	{
		try
		{
			Log.i(TAG, "CoronaOuyaPlugin.getProductsAsync");

			if (!m_enableIAP)
			{
				Log.i(TAG, "CoronaOuyaPlugin.getProductsAsync IAP is disabled");
				return;
			}

			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.getProductsAsync: CoronaOuyaFacade is null");
			}
			else
			{
				Log.i(TAG, "CoronaOuyaPlugin.getProductsAsync: CoronaOuyaFacade is valid");
				IOuyaActivity.GetCoronaOuyaFacade().requestProducts();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "CoronaOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static void clearGetProductList()
	{
		try
		{
			Log.i(TAG, "clearGetProductList");
		
			CoronaOuyaFacade.PRODUCT_IDENTIFIER_LIST.clear();
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
			Log.i(TAG, "addGetProduct productId: " + productId);
		
			boolean found = false;
			for (Purchasable purchasable : CoronaOuyaFacade.PRODUCT_IDENTIFIER_LIST)
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
				CoronaOuyaFacade.PRODUCT_IDENTIFIER_LIST.add(newPurchasable);
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
			for (Purchasable purchasable : CoronaOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				++count;
			}
			Log.i(TAG, "debugProductList TestOuyaFacade.PRODUCT_IDENTIFIER_LIST has " + count + " elements");
			for (Purchasable purchasable : CoronaOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				Log.i(TAG, "debugProductList TestOuyaFacade.PRODUCT_IDENTIFIER_LIST has: " + purchasable.getProductId());
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
			Log.i(TAG, "requestPurchaseAsync identifier: " + identifier);
		
			if (!m_enableIAP)
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestPurchaseAsync IAP is disabled");
				return "";
			}

			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestPurchaseAsync: CoronaOuyaFacade is null");
			}
			else
			{
				Log.i(TAG, "CoronaOuyaPlugin.requestPurchaseAsync: CoronaOuyaFacade is valid");
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
			Log.i(TAG, "CoronaOuyaPlugin.getReceiptsAsync");

			if (!m_enableIAP)
			{
				Log.i(TAG, "CoronaOuyaPlugin.getReceiptsAsync IAP is disabled");
				return;
			}

			if (null == IOuyaActivity.GetCoronaOuyaFacade())
			{
				Log.i(TAG, "CoronaOuyaPlugin.getReceiptsAsync: CoronaOuyaFacade is null");
			}
			else
			{
				Log.i(TAG, "CoronaOuyaPlugin.getReceiptsAsync: CoronaOuyaFacade is valid");
				IOuyaActivity.GetCoronaOuyaFacade().requestReceipts();
			}
		}
		catch (Exception ex) 
		{
			Log.i(TAG, "CoronaOuyaPlugin: getProductsAsync exception: " + ex.toString());
		}
	}
}