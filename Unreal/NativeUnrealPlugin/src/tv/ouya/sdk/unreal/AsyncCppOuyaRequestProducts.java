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

import android.util.Log;
import java.util.ArrayList;
import tv.ouya.console.api.*;


public class AsyncCppOuyaRequestProducts {

	static final String LOG_TAG = AsyncCppOuyaRequestProducts.class.getSimpleName();

	public static void invoke(String[] productArray) {

		Log.i(LOG_TAG, "Java: AsyncCppOuyaRequestProducts");

		ArrayList<Purchasable> products = new ArrayList<Purchasable>();

		for (String productId : productArray) {
			Purchasable purchasable = new Purchasable(productId);
			products.add(purchasable);
		}

		CallbacksRequestProducts callbacks = new CallbacksRequestProducts();

		// store for access
		IUnrealOuyaActivity.SetCallbacksRequestProducts(callbacks);

		// invoke service
		UnrealOuyaPlugin.requestProductListAsync(products);
	}
}