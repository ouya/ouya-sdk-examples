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
package tv.ouya.examples.android.multipleactivitiesiap;


import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;


public class ActivityInit extends Activity
{
    // bare-bones switch to first activity   
    @Override
    public void onStart()
    {
    	super.onStart();
    	
    	Intent openIntent = new Intent(this.getPackageName()+".ACTION1");
        startActivity(openIntent);
        finish();
    }
}
