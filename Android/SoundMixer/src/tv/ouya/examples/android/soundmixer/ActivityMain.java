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
package tv.ouya.examples.android.soundmixer;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.widget.TextView;
import android.media.AudioManager;
import android.os.Bundle;


public class ActivityMain extends Activity
{
	TextView m_tvVolumeAlarm = null;	
	TextView m_tvVolumeDtmf = null;
	TextView m_tvVolumeMusic = null;
	TextView m_tvVolumeNotification = null;
	TextView m_tvVolumeRing = null;
	TextView m_tvVolumeSystem = null;
	TextView m_tvVolumeVoiceCall = null;
	
	boolean m_waitToExit = true;
	
	final String TAG = "SoundMixer";
	
	private void SetVolume(String description, TextView tv, int stream) {
		AudioManager audioMgr =
			(AudioManager)getSystemService(Context.AUDIO_SERVICE);
		int maxVolume = audioMgr.getStreamMaxVolume(stream);
		int volume = audioMgr.getStreamVolume(stream);
		tv.setText(description + " max="+maxVolume+" volume="+volume);
	}
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        
        setContentView(R.layout.layout_main);
        
        m_tvVolumeAlarm = (TextView)findViewById(R.id.tvVolumeAlarm);	
    	m_tvVolumeDtmf = (TextView)findViewById(R.id.tvVolumeDtmf);
    	m_tvVolumeMusic = (TextView)findViewById(R.id.tvVolumeMusic);
    	m_tvVolumeNotification = (TextView)findViewById(R.id.tvVolumeNotification);
    	m_tvVolumeRing = (TextView)findViewById(R.id.tvVolumeRing);
    	m_tvVolumeSystem = (TextView)findViewById(R.id.tvVolumeSystem);
    	m_tvVolumeVoiceCall = (TextView)findViewById(R.id.tvVolumeVoiceCall);
		
		Thread timer = new Thread() {
			public void run() {
				while (m_waitToExit) {
					Runnable runnable = new Runnable()
					{
						public void run()
						{
							SetVolume("Alarm", m_tvVolumeAlarm, AudioManager.STREAM_ALARM);
							SetVolume("Dtmf", m_tvVolumeDtmf, AudioManager.STREAM_DTMF);
							SetVolume("Music", m_tvVolumeMusic, AudioManager.STREAM_MUSIC);
							SetVolume("Notification", m_tvVolumeNotification, AudioManager.STREAM_NOTIFICATION);
							SetVolume("Ring", m_tvVolumeRing, AudioManager.STREAM_RING);
							SetVolume("System", m_tvVolumeSystem, AudioManager.STREAM_SYSTEM);
							SetVolume("Voice Call", m_tvVolumeVoiceCall, AudioManager.STREAM_VOICE_CALL);
						}
					};
					
					runOnUiThread(runnable);
					
					try {
						Thread.sleep(50);
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}
        };
        timer.start();
	}

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		
		m_waitToExit = false;
	}
    
    
}