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


public class AsyncCppOuyaContentInit {

	public static void invoke()
			throws Exception {

		// store callbakcs
		IUnrealOuyaActivity.SetCallbacksContentInit(new CallbacksContentInit());		
		IUnrealOuyaActivity.SetCallbacksContentDelete(new CallbacksContentDelete());
		IUnrealOuyaActivity.SetCallbacksContentDownload(new CallbacksContentDownload());
		IUnrealOuyaActivity.SetCallbacksContentPublish(new CallbacksContentPublish());
		IUnrealOuyaActivity.SetCallbacksContentSave(new CallbacksContentSave());
		IUnrealOuyaActivity.SetCallbacksContentSearchInstalled(new CallbacksContentSearchInstalled());
		IUnrealOuyaActivity.SetCallbacksContentSearchPublished(new CallbacksContentSearchPublished());
		IUnrealOuyaActivity.SetCallbacksContentUnpublish(new CallbacksContentUnpublish());

		// invoke service
		UnrealOuyaPlugin.InitializeContent();
	}
}