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

package com.mycompany.simpleluaextension;

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
	protected static TestOuyaFacade m_testOuyaFacade = null;
	public static TestOuyaFacade GetTestOuyaFacade()
	{
		return m_testOuyaFacade;
	}
	public static void SetTestOuyaFacade(TestOuyaFacade testOuyaFacade)
	{
		m_testOuyaFacade = testOuyaFacade;
	}
	
	// save reference to the ouya corona plugin
	protected static OuyaCoronaPlugin m_ouyaCoronaPlugin = null;
	public static OuyaCoronaPlugin GetOuyaCoronaPlugin()
	{
		return m_ouyaCoronaPlugin;
	}
	public static void SetOuyaCoronaPlugin(OuyaCoronaPlugin ouyaCoronaPlugin)
	{
		m_ouyaCoronaPlugin = ouyaCoronaPlugin;
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
}