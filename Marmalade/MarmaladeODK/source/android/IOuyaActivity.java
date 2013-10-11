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
import android.os.Bundle;
import com.ideaworks3d.marmalade.LoaderActivity;

public class IOuyaActivity
{
	// save reference to the activity
	protected static LoaderActivity m_activity = null;
	public static LoaderActivity GetActivity()
	{
		return m_activity;
	}
	public static void SetActivity(LoaderActivity activity)
	{
		m_activity = activity;
	}

	// save reference to the bundle
	protected static Bundle m_savedInstanceState = null;
	public static Bundle GetSavedInstanceState()
	{
		return m_savedInstanceState;
	}
	public static void SetSavedInstanceState(Bundle savedInstanceState)
	{
		m_savedInstanceState = savedInstanceState;
	}

	// save reference to the facade
	protected static MarmaladeOuyaFacade m_marmaladeOuyaFacade = null;
	public static MarmaladeOuyaFacade GetMarmaladeOuyaFacade()
	{
		return m_marmaladeOuyaFacade;
	}
	public static void SetMarmaladeOuyaFacade(MarmaladeOuyaFacade marmaladeOuyaFacade)
	{
		m_marmaladeOuyaFacade = marmaladeOuyaFacade;
	}

	// save reference to the marmalade ouya plugin
	protected static MarmaladeOuyaPlugin m_marmaladeOuyaPlugin = null;
	public static MarmaladeOuyaPlugin GetMarmaladeOuyaPlugin()
	{
		return m_marmaladeOuyaPlugin;
	}
	public static void SetMarmaladeOuyaPlugin(MarmaladeOuyaPlugin marmaladeOuyaPlugin)
	{
		m_marmaladeOuyaPlugin = marmaladeOuyaPlugin;
	}

	/*
	* The application key. This is used to decrypt encrypted receipt responses. This should be replaced with the
	* application key obtained from the OUYA developers website.
	*/
	protected static byte[] m_applicationKey = null;
	public static byte[] GetApplicationKey()
	{
		return m_applicationKey;
	}
	public static void SetApplicationKey(byte[] applicationKey)
	{
		m_applicationKey = applicationKey;
	}

	protected static CallbacksFetchGamerUUID m_callbacksFetchGamerUUID = null;
	public static CallbacksFetchGamerUUID GetCallbacksFetchGamerUUID()
	{
		return m_callbacksFetchGamerUUID;
	}
	public static void SetCallbacksFetchGamerUUID(CallbacksFetchGamerUUID callbacksFetchGamerUUID)
	{
		m_callbacksFetchGamerUUID = callbacksFetchGamerUUID;
	}

	protected static CallbacksRequestProducts m_callbacksRequestProducts = null;
	public static CallbacksRequestProducts GetCallbacksRequestProducts()
	{
		return m_callbacksRequestProducts;
	}
	public static void SetCallbacksRequestProducts(CallbacksRequestProducts callbacksRequestProducts)
	{
		m_callbacksRequestProducts = callbacksRequestProducts;
	}

	protected static CallbacksRequestPurchase m_callbacksRequestPurchase = null;
	public static CallbacksRequestPurchase GetCallbacksRequestPurchase()
	{
		return m_callbacksRequestPurchase;
	}
	public static void SetCallbacksRequestPurchase(CallbacksRequestPurchase callbacksRequestPurchase)
	{
		m_callbacksRequestPurchase = callbacksRequestPurchase;
	}

	protected static CallbacksRequestReceipts m_callbacksRequestReceipts = null;
	public static CallbacksRequestReceipts GetCallbacksRequestReceipts()
	{
		return m_callbacksRequestReceipts;
	}
	public static void SetCallbacksRequestReceipts(CallbacksRequestReceipts callbacksRequestReceipts)
	{
		m_callbacksRequestReceipts = callbacksRequestReceipts;
	}
}