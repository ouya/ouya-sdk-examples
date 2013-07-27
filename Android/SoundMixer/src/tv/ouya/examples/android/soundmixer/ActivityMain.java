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
import android.content.Intent;
import android.util.Log;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import android.media.AudioManager;
import android.os.Bundle;


public class ActivityMain extends Activity
{
	ImageButton m_btnDownAlarm = null;
	ImageButton m_btnDownDtmf = null;
	ImageButton m_btnDownMusic = null;
	ImageButton m_btnDownNotification = null;
	ImageButton m_btnDownRing = null;
	ImageButton m_btnDownSystem = null;
	ImageButton m_btnDownVoiceCall = null;

	ImageButton m_btnUpAlarm = null;
	ImageButton m_btnUpDtmf = null;
	ImageButton m_btnUpMusic = null;
	ImageButton m_btnUpNotification = null;
	ImageButton m_btnUpRing = null;
	ImageButton m_btnUpSystem = null;
	ImageButton m_btnUpVoiceCall = null;

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
	
	private void SetupButtonDown(ImageButton button, final int stream) {
		button.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				AudioManager audioMgr =
					(AudioManager)getSystemService(Context.AUDIO_SERVICE);
				int volume = audioMgr.getStreamVolume(stream);
				volume = Math.max(volume - 1, 0);
				audioMgr.setStreamVolume(stream, volume, 0);
			}
		});		
	}
	
	private void SetupButtonUp(ImageButton button, final int stream) {
		button.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				AudioManager audioMgr =
					(AudioManager)getSystemService(Context.AUDIO_SERVICE);
				int maxVolume = audioMgr.getStreamMaxVolume(stream);
				int volume = audioMgr.getStreamVolume(stream);
				volume = Math.min(volume + 1, maxVolume);
				audioMgr.setStreamVolume(stream, volume, 0);
			}
		});		
	}

    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.layout_main);

        m_btnDownAlarm = (ImageButton)findViewById(R.id.btnDownAlarm);
        m_btnDownDtmf = (ImageButton)findViewById(R.id.btnDownDtmf);
        m_btnDownMusic = (ImageButton)findViewById(R.id.btnDownMusic);
        m_btnDownNotification = (ImageButton)findViewById(R.id.btnDownNotification);
        m_btnDownRing = (ImageButton)findViewById(R.id.btnDownRing);
        m_btnDownSystem = (ImageButton)findViewById(R.id.btnDownSystem);
        m_btnDownVoiceCall = (ImageButton)findViewById(R.id.btnDownVoiceCall);

        m_btnUpAlarm = (ImageButton)findViewById(R.id.btnUpAlarm);
        m_btnUpDtmf = (ImageButton)findViewById(R.id.btnUpDtmf);
        m_btnUpMusic = (ImageButton)findViewById(R.id.btnUpMusic);
        m_btnUpNotification = (ImageButton)findViewById(R.id.btnUpNotification);
        m_btnUpRing = (ImageButton)findViewById(R.id.btnUpRing);
        m_btnUpSystem = (ImageButton)findViewById(R.id.btnUpSystem);
        m_btnUpVoiceCall = (ImageButton)findViewById(R.id.btnUpVoiceCall);

        m_tvVolumeAlarm = (TextView)findViewById(R.id.tvVolumeAlarm);
    	m_tvVolumeDtmf = (TextView)findViewById(R.id.tvVolumeDtmf);
    	m_tvVolumeMusic = (TextView)findViewById(R.id.tvVolumeMusic);
    	m_tvVolumeNotification = (TextView)findViewById(R.id.tvVolumeNotification);
    	m_tvVolumeRing = (TextView)findViewById(R.id.tvVolumeRing);
    	m_tvVolumeSystem = (TextView)findViewById(R.id.tvVolumeSystem);
    	m_tvVolumeVoiceCall = (TextView)findViewById(R.id.tvVolumeVoiceCall);
    	
    	
        SetupButtonDown(m_btnDownAlarm, AudioManager.STREAM_ALARM);
        SetupButtonDown(m_btnDownDtmf, AudioManager.STREAM_DTMF);
        SetupButtonDown(m_btnDownMusic, AudioManager.STREAM_MUSIC);
        SetupButtonDown(m_btnDownNotification, AudioManager.STREAM_NOTIFICATION);
        SetupButtonDown(m_btnDownRing, AudioManager.STREAM_RING);
        SetupButtonDown(m_btnDownSystem, AudioManager.STREAM_SYSTEM);
        SetupButtonDown(m_btnDownVoiceCall, AudioManager.STREAM_VOICE_CALL);

        SetupButtonUp(m_btnUpAlarm, AudioManager.STREAM_ALARM);
        SetupButtonUp(m_btnUpDtmf, AudioManager.STREAM_DTMF);
        SetupButtonUp(m_btnUpMusic, AudioManager.STREAM_MUSIC);
        SetupButtonUp(m_btnUpNotification, AudioManager.STREAM_NOTIFICATION);
        SetupButtonUp(m_btnUpRing, AudioManager.STREAM_RING);
        SetupButtonUp(m_btnUpSystem, AudioManager.STREAM_SYSTEM);
        SetupButtonUp(m_btnUpVoiceCall, AudioManager.STREAM_VOICE_CALL);
    	

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