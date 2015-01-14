/*
 * Copyright (C) 2012-2015 OUYA, Inc.
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

package tv.ouya.sdk.unreal;


import android.app.Activity;
import android.os.Bundle;
import java.util.*;
import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaMod;

public class IUnrealOuyaActivity
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

	// save reference to the facade
	protected static UnrealOuyaFacade m_unrealOuyaFacade = null;
	public static UnrealOuyaFacade GetUnrealOuyaFacade()
	{
		return m_unrealOuyaFacade;
	}
	public static void SetUnrealOuyaFacade(UnrealOuyaFacade unrealOuyaFacade)
	{
		m_unrealOuyaFacade = unrealOuyaFacade;
	}

	// save reference to the unreal ouya plugin
	protected static UnrealOuyaPlugin m_unrealOuyaPlugin = null;
	public static UnrealOuyaPlugin GetUnrealOuyaPlugin()
	{
		return m_unrealOuyaPlugin;
	}
	public static void SetUnrealOuyaPlugin(UnrealOuyaPlugin unrealOuyaPlugin)
	{
		m_unrealOuyaPlugin = unrealOuyaPlugin;
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
	
	protected static OuyaContent m_ouyaContent = null;
	public static OuyaContent GetOuyaContent()
	{
		return m_ouyaContent;
	}
	public static void SetOuyaContent(OuyaContent ouyaContent)
	{
		m_ouyaContent = ouyaContent;
	}

	protected static List<OuyaMod> m_ouyaContentInstalledResults = null;
	public static List<OuyaMod> GetOuyaContentInstalledResults()
	{
		return m_ouyaContentInstalledResults;
	}
	public static void SetOuyaContentInstalledResults(List<OuyaMod> results)
	{
		m_ouyaContentInstalledResults = results;
	}

	protected static List<OuyaMod> m_ouyaContentPublishedResults = null;
	public static List<OuyaMod> GetOuyaContentPublishedResults()
	{
		return m_ouyaContentPublishedResults;
	}
	public static void SetOuyaContentPublishedResults(List<OuyaMod> results)
	{
		m_ouyaContentPublishedResults = results;
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
	
	protected static CallbacksContentInit m_callbacksContentInit = null;
	public static CallbacksContentInit GetCallbacksContentInit()
	{
		return m_callbacksContentInit;
	}
	public static void SetCallbacksContentInit(CallbacksContentInit callback)
	{
		m_callbacksContentInit = callback;
	}
	
	protected static CallbacksContentSearchInstalled m_callbacksContentSearchInstalled = null;
	public static CallbacksContentSearchInstalled GetCallbacksContentSearchInstalled()
	{
		return m_callbacksContentSearchInstalled;
	}
	public static void SetCallbacksContentSearchInstalled(CallbacksContentSearchInstalled callback)
	{
		m_callbacksContentSearchInstalled = callback;
	}
	
	protected static CallbacksContentSearchPublished m_callbacksContentSearchPublished = null;
	public static CallbacksContentSearchPublished GetCallbacksContentSearchPublished()
	{
		return m_callbacksContentSearchPublished;
	}
	public static void SetCallbacksContentSearchPublished(CallbacksContentSearchPublished callback)
	{
		m_callbacksContentSearchPublished = callback;
	}
	
	protected static CallbacksContentSave m_callbacksContentSave = null;
	public static CallbacksContentSave GetCallbacksContentSave()
	{
		return m_callbacksContentSave;
	}
	public static void SetCallbacksContentSave(CallbacksContentSave callback)
	{
		m_callbacksContentSave = callback;
	}
	
	protected static CallbacksContentPublish m_callbacksContentPublish = null;
	public static CallbacksContentPublish GetCallbacksContentPublish()
	{
		return m_callbacksContentPublish;
	}
	public static void SetCallbacksContentPublish(CallbacksContentPublish callback)
	{
		m_callbacksContentPublish = callback;
	}
	
	protected static CallbacksContentUnpublish m_callbacksContentUnpublish = null;
	public static CallbacksContentUnpublish GetCallbacksContentUnpublish()
	{
		return m_callbacksContentUnpublish;
	}
	public static void SetCallbacksContentUnpublish(CallbacksContentUnpublish callback)
	{
		m_callbacksContentUnpublish = callback;
	}
	
	protected static CallbacksContentDelete m_callbacksContentDelete = null;
	public static CallbacksContentDelete GetCallbacksContentDelete()
	{
		return m_callbacksContentDelete;
	}
	public static void SetCallbacksContentDelete(CallbacksContentDelete callback)
	{
		m_callbacksContentDelete = callback;
	}
	
	protected static CallbacksContentDownload m_callbacksContentDownload = null;
	public static CallbacksContentDownload GetCallbacksContentDownload()
	{
		return m_callbacksContentDownload;
	}
	public static void SetCallbacksContentDownload(CallbacksContentDownload callback)
	{
		m_callbacksContentDownload = callback;
	}
}
