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

#ifndef __PLUGIN_OUYA_H__
#define __PLUGIN_OUYA_H__

#include <string>
#include <vector>

class _jclass;
class CallbacksRequestGamerInfo;
class CallbacksRequestProducts;
class CallbacksRequestPurchase;
class CallbacksRequestReceipts;

typedef struct _JNIEnv JNIEnv;
typedef _jclass* jclass;

namespace OuyaSDK
{
	class PluginOuya
	{
	public:

		PluginOuya();

		void FindClass(JNIEnv* env, const std::string& className, jclass* jc);

		void CacheClasses(JNIEnv* env);

		void SetApp(struct android_app* app);

		void AsyncInitOuyaPlugin(const std::string& jsonData);

		void AsyncOuyaRequestGamerInfo();

		void AsyncOuyaRequestProducts(const std::vector<std::string>& productIds);

		void AsyncOuyaRequestPurchase(const std::string& purchasable);

		void AsyncOuyaRequestReceipts();

	private:

		// cached references
		jclass jc_IMarmaladeOuyaActivity;
		jclass jc_AsyncCppInitOuyaPlugin;
		jclass jc_AsyncCppOuyaRequestGamerInfo;
		jclass jc_AsyncCppOuyaRequestProducts;
		jclass jc_AsyncCppOuyaRequestPurchase;
		jclass jc_AsyncCppOuyaRequestReceipts;
	};
}

#endif