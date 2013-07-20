/*
 * Copyright (C) 2009 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package tv.ouya.android.setresolutions;

import android.app.Activity;
import android.content.Intent;
import android.view.View;
import android.view.View.OnLayoutChangeListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.os.Bundle;


public class ActivityMain extends Activity
{
	View m_contentView = null;
	TextView m_tvMainHeader = null;
	Button m_btn480p = null;
	Button m_btn720p = null;
	Button m_btn1080p = null;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        
        setContentView(R.layout.main);
        
        m_contentView = getWindow().getDecorView().findViewById(android.R.id.content);
        m_tvMainHeader = (TextView)findViewById(R.id.tvMainHeader);
        m_btn480p = (Button)findViewById(R.id.button480p);
        m_btn720p = (Button)findViewById(R.id.button720p);
        m_btn1080p = (Button)findViewById(R.id.button1080p);
        
        m_contentView.addOnLayoutChangeListener(new OnLayoutChangeListener() {

            @Override
            public void onLayoutChange(View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight,
                    int oldBottom) {
                // its possible that the layout is not complete in which case
                // we will get all zero values for the positions, so ignore the event
                if (left == 0 && top == 0 && right == 0 && bottom == 0) {
                    return;
                }

               // Do what you need to do with the height/width since they are now set
                LayoutParams layoutParams = m_contentView.getLayoutParams();
                m_tvMainHeader.setText("Main Layout @ " + layoutParams.width + "x" + layoutParams.height);
            }
        });
        
        m_btn480p.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent openIntent = new Intent("tv.ouya.android.setresolutions.RESOLUION480P");
		        startActivity(openIntent);				
			}
		});
        
        m_btn720p.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				//Intent openIntent = new Intent("tv.ouya.android.setresolutions.RESOLUION720P");
				Intent openIntent = new Intent("tv.ouya.android.setresolutions.RESOLUIONNATIVE");
		        startActivity(openIntent);
			}
		});
        
        m_btn1080p.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent openIntent = new Intent("tv.ouya.android.setresolutions.RESOLUION1080P");
		        startActivity(openIntent);				
			}
		});
    }
    
    @Override
    public void onResume()
    {
    	super.onResume();
    	LayoutParams layoutParams = m_contentView.getLayoutParams();
    	m_tvMainHeader.setText("Main Layout @ " + layoutParams.width + "x" + layoutParams.height);
    }
}
