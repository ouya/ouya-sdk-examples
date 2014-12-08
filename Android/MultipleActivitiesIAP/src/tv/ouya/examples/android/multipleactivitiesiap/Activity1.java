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

import java.util.Collection;

import tv.ouya.console.api.Receipt;
import android.content.Intent;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.os.Bundle;


public class Activity1 extends ActivityCommon
{
	Button m_btnActivity = null;
	Button m_btnRequestReceipts = null;
	TextView m_txtStatus = null;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        
        setContentView(R.layout.layout_activity1);
        
        m_btnActivity = (Button)findViewById(R.id.btnActivity);
        m_btnRequestReceipts = (Button)findViewById(R.id.btnRequestReceipts);
        m_txtStatus = (TextView)findViewById(R.id.txtStatus);
        
        m_txtStatus.setText("Status: Loaded "+getClass().getSimpleName());
        
        m_btnActivity.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent openIntent = new Intent(getPackageName()+".ACTION2");
		        startActivity(openIntent);
		        finish();
			}
		});
        
        m_btnRequestReceipts.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				m_txtStatus.setText("Status: Requesting Receipts...");
				requestReceipts();
			}
		});
    }
    
    // override this method in the activity to show in the text status field
    @Override
    protected void onSuccessRequestReceipts(Collection<Receipt> receipts) {
    	m_txtStatus.setText("requestReceipts onSuccess: received "+receipts.size() + " receipts");
 	}

 	// override this method in the activity to show in the text status field
    @Override
    protected void onFailureRequestReceipts(int errorCode, String errorMessage, Bundle optionalData) {
    	m_txtStatus.setText("requestReceipts onFailure: errorCode="+errorCode+" errorMessage="+errorMessage);
 	}
}
