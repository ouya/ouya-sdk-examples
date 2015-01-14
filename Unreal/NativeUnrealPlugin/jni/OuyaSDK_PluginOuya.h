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

#ifndef __PLUGIN_OUYA_H__
#define __PLUGIN_OUYA_H__

//#if PLATFORM_ANDROID

#include <jni.h>
#include <string>
#include <vector>

class CallbacksInitOuyaPlugin;
class CallbacksRequestGamerInfo;
class CallbacksRequestProducts;
class CallbacksRequestPurchase;
class CallbacksRequestReceipts;

namespace OuyaSDK
{
	class PluginOuya
	{
	public:

		static int InitJNI(JavaVM* jvm);

		void AsyncInitOuyaPlugin(const std::string& jsonData, CallbacksInitOuyaPlugin* callbacks);

		void AsyncOuyaRequestGamerInfo(CallbacksRequestGamerInfo* callbacks);

		void AsyncOuyaRequestProducts(const std::vector<std::string>& productIds, CallbacksRequestProducts* callbacks);

		void AsyncOuyaRequestPurchase(const std::string& purchasable, CallbacksRequestPurchase* callbacks);

		void AsyncOuyaRequestReceipts(CallbacksRequestReceipts* callbacks);

		static bool updateSafeArea(float amount);

		static bool shutdown();

	private:

		// cached references
		static JavaVM* _jvm;
		static jclass jc_IUnrealOuyaActivity;
		static jclass jc_UnrealOuyaPlugin;
		static jclass jc_AsyncCppInitOuyaPlugin;
		static jclass jc_AsyncCppOuyaRequestGamerInfo;
		static jclass jc_AsyncCppOuyaRequestProducts;
		static jclass jc_AsyncCppOuyaRequestPurchase;
		static jclass jc_AsyncCppOuyaRequestReceipts;
	};
}

//#endif

#endif
