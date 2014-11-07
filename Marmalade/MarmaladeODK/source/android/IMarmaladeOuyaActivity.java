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
import android.os.Bundle;
import com.ideaworks3d.marmalade.LoaderActivity;

public class IMarmaladeOuyaActivity
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

	protected static CallbacksInitOuyaPlugin m_callbacksInitOuyaPlugin = null;
	public static CallbacksInitOuyaPlugin GetCallbacksInitOuyaPlugin()
	{
		return m_callbacksInitOuyaPlugin;
	}
	public static void SetCallbacksInitOuyaPlugin(CallbacksInitOuyaPlugin callback)
	{
		m_callbacksInitOuyaPlugin = callback;
	}

	protected static CallbacksRequestGamerInfo m_callbacksRequestGamerInfo = null;
	public static CallbacksRequestGamerInfo GetCallbacksRequestGamerInfo()
	{
		return m_callbacksRequestGamerInfo;
	}
	public static void SetCallbacksRequestGamerInfo(CallbacksRequestGamerInfo callback)
	{
		m_callbacksRequestGamerInfo = callback;
	}

	protected static CallbacksRequestProducts m_callbacksRequestProducts = null;
	public static CallbacksRequestProducts GetCallbacksRequestProducts()
	{
		return m_callbacksRequestProducts;
	}
	public static void SetCallbacksRequestProducts(CallbacksRequestProducts callback)
	{
		m_callbacksRequestProducts = callback;
	}

	protected static CallbacksRequestPurchase m_callbacksRequestPurchase = null;
	public static CallbacksRequestPurchase GetCallbacksRequestPurchase()
	{
		return m_callbacksRequestPurchase;
	}
	public static void SetCallbacksRequestPurchase(CallbacksRequestPurchase callback)
	{
		m_callbacksRequestPurchase = callback;
	}

	protected static CallbacksRequestReceipts m_callbacksRequestReceipts = null;
	public static CallbacksRequestReceipts GetCallbacksRequestReceipts()
	{
		return m_callbacksRequestReceipts;
	}
	public static void SetCallbacksRequestReceipts(CallbacksRequestReceipts callback)
	{
		m_callbacksRequestReceipts = callback;
	}
}
