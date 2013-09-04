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
package tv.ouya.examples.android.testflightexample;

import android.app.Application;
import com.testflightapp.lib.TestFlight;


public class MyApplication extends Application {
	
	public final String YOUR_APP_TOKEN = "blahhhh";

    @Override
    public void onCreate() {
        super.onCreate();
        
        //Initialize TestFlight with your app token.
        try {
			TestFlight.takeOff(this, YOUR_APP_TOKEN);
			TestFlight.log("My Application Has Started…");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }	
}
