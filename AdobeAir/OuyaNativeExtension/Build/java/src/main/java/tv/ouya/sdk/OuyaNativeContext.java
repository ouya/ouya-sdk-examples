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

package tv.ouya.sdk;

import com.adobe.fre.FREContext;
import com.adobe.fre.FREFunction;
import java.util.HashMap;
import java.util.Map;

public class OuyaNativeContext extends FREContext {
	
	private static final String TAG = OuyaNativeContext.class.getSimpleName();
	
	@Override
	public void dispose() {
	}
	
	@Override
	public Map<String, FREFunction> getFunctions() {
		Map<String, FREFunction> map = new HashMap<String, FREFunction>();
		
		map.put("ouyaInit", new OuyaNativeFunctionInit());
		map.put("ouyaIsAnyConnected", new OuyaNativeFunctionIsAnyConnected());
		map.put("ouyaIsConnected", new OuyaNativeFunctionIsConnected());
		map.put("ouyaGetAxis", new OuyaNativeFunctionGetAxis());
		map.put("ouyaGetAnyButton", new OuyaNativeFunctionGetAnyButton());
		map.put("ouyaGetAnyButtonDown", new OuyaNativeFunctionGetAnyButtonDown());
		map.put("ouyaGetAnyButtonUp", new OuyaNativeFunctionGetAnyButtonUp());
		map.put("ouyaGetButton", new OuyaNativeFunctionGetButton());
		map.put("ouyaGetButtonDown", new OuyaNativeFunctionGetButtonDown());
		map.put("ouyaGetButtonUp", new OuyaNativeFunctionGetButtonUp());
		map.put("ouyaClearButtonStatesPressedReleased", new OuyaNativeFunctionClearButtonStatesPressedReleased());
		map.put("ouyaGetTrackpadX", new OuyaNativeFunctionGetTrackpadX());
		map.put("ouyaGetTrackpadY", new OuyaNativeFunctionGetTrackpadY());
		map.put("ouyaGetTrackpadDown", new OuyaNativeFunctionGetTrackpadDown());
		
		map.put("ouyaLogInfo", new OuyaNativeFunctionLogInfo());
		map.put("ouyaLogError", new OuyaNativeFunctionLogError());
		
		map.put("ouyaToggleInputLogging", new OuyaNativeFunctionToggleInputLogging());
		
		return map;
	}
}
