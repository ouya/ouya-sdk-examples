/*
 * Copyright (C) 2012 OUYA, Inc.
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

package tv.ouya.examples.android.virtualcontroller;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.*;
import android.widget.TextView;
import tv.ouya.console.api.OuyaController;
import tv.ouya.sdk.OuyaInputView;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
    OuyaInputView mInputView = null;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        
        mInputView = new OuyaInputView(this);
    }
    
	@Override
    public void onDestroy()
    {
    	super.onDestroy();
    	mInputView.shutdown();
    }
}
