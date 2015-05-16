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
import android.os.Bundle;

public class IOuyaActivity
{
	// save reference to the activity
	protected static Activity m_activity = null;
	public static Activity GetActivity()
	{
		return m_activity;
	}
	public static void SetActivity(Activity activity)
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

	// save reference to the testFacade
	protected static CoronaOuyaFacade m_coronaOuyaFacade = null;
	public static CoronaOuyaFacade GetCoronaOuyaFacade()
	{
		return m_coronaOuyaFacade;
	}
	public static void SetCoronaOuyaFacade(CoronaOuyaFacade coronaOuyaFacade)
	{
		m_coronaOuyaFacade = coronaOuyaFacade;
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
	
	protected static CallbacksRequestGamerInfo m_callbacksRequestGamerInfo = null;
	public static CallbacksRequestGamerInfo GetCallbacksRequestGamerInfo()
	{
		return m_callbacksRequestGamerInfo;
	}
	public static void SetCallbacksRequestGamerInfo(CallbacksRequestGamerInfo callbacks)
	{
		m_callbacksRequestGamerInfo = callbacks;
	}
	
	protected static CallbacksRequestProducts m_callbacksRequestProducts = null;
	public static CallbacksRequestProducts GetCallbacksRequestProducts()
	{
		return m_callbacksRequestProducts;
	}
	public static void SetCallbacksRequestProducts(CallbacksRequestProducts callbacks)
	{
		m_callbacksRequestProducts = callbacks;
	}
	
	protected static CallbacksRequestPurchase m_callbacksRequestPurchase = null;
	public static CallbacksRequestPurchase GetCallbacksRequestPurchase()
	{
		return m_callbacksRequestPurchase;
	}
	public static void SetCallbacksRequestPurchase(CallbacksRequestPurchase callbacks)
	{
		m_callbacksRequestPurchase = callbacks;
	}
	
	protected static CallbacksRequestReceipts m_callbacksRequestReceipts = null;
	public static CallbacksRequestReceipts GetCallbacksRequestReceipts()
	{
		return m_callbacksRequestReceipts;
	}
	public static void SetCallbacksRequestReceipts(CallbacksRequestReceipts callbacks)
	{
		m_callbacksRequestReceipts = callbacks;
	}
	
	protected static CallbacksOuyaInput m_callbacksOuyaInput = null;
	public static CallbacksOuyaInput GetCallbacksOuyaInput()
	{
		return m_callbacksOuyaInput;
	}
	public static void SetCallbacksOuyaInput(CallbacksOuyaInput callbacks)
	{
		m_callbacksOuyaInput = callbacks;
	}

	protected static CallbacksInitOuyaPlugin m_callbacksInitOuyaPlugin = null;
	public static CallbacksInitOuyaPlugin GetCallbacksInitOuyaPlugin()
	{
		return m_callbacksInitOuyaPlugin;
	}
	public static void SetCallbacksInitOuyaPlugin(CallbacksInitOuyaPlugin callbacks)
	{
		m_callbacksInitOuyaPlugin = callbacks;
	}
}