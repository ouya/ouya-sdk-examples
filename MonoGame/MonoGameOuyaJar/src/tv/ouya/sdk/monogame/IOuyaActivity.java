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

package tv.ouya.sdk.monogame;


import android.app.Activity;
import android.app.NativeActivity;
import android.os.Bundle;

public class IOuyaActivity
{
	// save reference to the activity
	protected static NativeActivity m_activity = null;
	public static NativeActivity GetActivity()
	{
		return m_activity;
	}
	public static void SetActivity(NativeActivity activity)
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

	// save reference to the MonoGameFacade
	protected static MonoGameOuyaFacade m_monoGameOuyaFacade = null;
	public static MonoGameOuyaFacade GetMonoGameOuyaFacade()
	{
		return m_monoGameOuyaFacade;
	}
	public static void SetMonoGameOuyaFacade(MonoGameOuyaFacade monoGameOuyaFacade)
	{
		m_monoGameOuyaFacade = monoGameOuyaFacade;
	}

	// save reference to the MonoGame ouya plugin
	protected static MonoGameOuyaPlugin m_monoGameOuyaPlugin = null;
	public static MonoGameOuyaPlugin GetMonoGameOuyaPlugin()
	{
		return m_monoGameOuyaPlugin;
	}
	public static void SetMonoGameOuyaPlugin(MonoGameOuyaPlugin monoGameOuyaPlugin)
	{
		m_monoGameOuyaPlugin = monoGameOuyaPlugin;
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