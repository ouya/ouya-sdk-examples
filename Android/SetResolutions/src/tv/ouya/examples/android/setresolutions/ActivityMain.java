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
package tv.ouya.examples.android.setresolutions;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.graphics.PixelFormat;
import android.graphics.Point;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Display;
import android.view.View;
import android.view.View.OnLayoutChangeListener;
import android.view.ViewGroup.LayoutParams;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.os.Bundle;


public class ActivityMain extends Activity
{
	Context m_context = null;
	View m_contentView = null;
	TextView m_tvMainHeader = null;
	Button m_btn480p = null;
	Button m_btn720p = null;
	Button m_btn1080p = null;
	Button m_btnNative = null;
	Boolean m_keepWorking = true;
	
	int getDisplayWidth() {
		WindowManager windowManager = getWindowManager();
		Display display = windowManager.getDefaultDisplay();
		Point size = new Point();
		display.getSize(size);
		return size.x;
	}
	
	int getDisplayHeight() {
		WindowManager windowManager = getWindowManager();
		Display display = windowManager.getDefaultDisplay();
		Point size = new Point();
		display.getSize(size);
		return size.y;
	}
	
	void setResolutionLabels() {
		Runnable myRunnable = new Runnable() {
			public void run()
			{
                m_tvMainHeader.setText("Main Layout @ " + getDisplayWidth() + "x" + getDisplayHeight());
			}
		};
		
		runOnUiThread(myRunnable);
	}
	
	void changeResolution(final int width, final int height) {
		m_contentView.setScaleX((float)(getDisplayWidth()) / (float)(width));
		m_contentView.setScaleY((float)(getDisplayHeight()) / (float)(height));
	}
	
	@Override
	public View onCreateView (String name, Context context, AttributeSet attrs) {
		Log.i("Main", "onCreateView:" + name);
		m_context = context;
		return super.onCreateView(name, context, attrs);
	}
	
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
        m_btnNative = (Button)findViewById(R.id.buttonNative);
        
        m_btn480p.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				changeResolution(640, 480);
			}
		});
        
        m_btn720p.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				changeResolution(1280, 720);
			}
		});
        
        m_btn1080p.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				changeResolution(1920, 1080);
			}
		});
        
        m_btnNative.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent openIntent = new Intent("tv.ouya.examples.android.setresolutions.RESOLUIONNATIVE");
		        startActivity(openIntent);
			}
		});
    }
    
    @Override
    public void onResume() {
    	super.onResume();
    	
    	m_keepWorking = true;
    	Thread timer = new Thread() {
			public void run() {
				while (m_keepWorking) {
					setResolutionLabels();
					try {
						sleep(5000);
					} catch (InterruptedException e) {
						return;
					}					
				}
			}
		};
		
		timer.start();
    }
    
    @Override
    public void onPause() {
    	super.onPause();
    	m_keepWorking = false;    	
    }
}
