package tv.ouya.examples.android.soundpoolexample;

import android.media.AudioManager;
import android.media.SoundPool;
import android.os.Bundle;

import java.io.IOException;

import android.app.Activity;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.Button;

public class MainActivity extends Activity {
	
	private static final String TAG = "MainActivity";
	
	private static final String SOUND_1 = "tone.ogg";
	
	private Button btnBoom = null;
	private Button btnOuch = null;
	private Button btnTone = null;
	
	private SoundPool mSoundPool = null;
	
	private int mSoundBoom = -1;
	private int mSoundOuch = -1;
	private int mSoundTone = -1;
	
	private int mLastSound = -1;
	
	enum Sounds
	{
		Boom,
		Ouch,
		Tone
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		// get the buttons
		btnBoom = (Button)findViewById(R.id.btnBoom);
		btnOuch = (Button)findViewById(R.id.btnOuch);
		btnTone = (Button)findViewById(R.id.btnTone);
				
		// load the sound pool
		mSoundPool = new SoundPool(5, AudioManager.STREAM_MUSIC, 0);
		
		// load the resources
		mSoundBoom = mSoundPool.load(this, R.raw.boom, 1);
		mSoundOuch = mSoundPool.load(this, R.raw.ouch, 1);
		
		AssetManager assetManager = getApplicationContext().getAssets();
		
		AssetFileDescriptor afd = null;
		try {
			afd = assetManager.openFd(SOUND_1);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		if (null == afd) {
			Log.e(TAG, "Failed to load sound!");
			return;
		}
		
		mSoundTone = mSoundPool.load(afd, 1);
		
		try {
			afd.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		// set up events
		
		btnBoom.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	Log.i(TAG, "Clicked on Boom");
                PlaySound(Sounds.Boom, 1, 1, 0, 0, 1);
            }
        });
		
		btnOuch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	Log.i(TAG, "Clicked on Ouch");
                PlaySound(Sounds.Ouch, 1, 1, 0, 0, 1);
            }
        });
		
		btnTone.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	Log.i(TAG, "Clicked on Tone");
                PlaySound(Sounds.Tone, 1, 1, 0, -1, 1);
            }
        });
	}
	
	@Override
	protected void onDestroy() {
		
		if (mLastSound != -1) {
			mSoundPool.stop(mLastSound);
		}
		
		super.onDestroy();
	}

	public void PlaySound(Sounds sound, float leftVolume, float rightVolume, int priority, int loop, float rate) {
		
		if (null == mSoundPool ||
			-1 == mSoundBoom ||
			-1 == mSoundOuch) {
			Log.e(TAG, "Sound pool not initialized");
			return; //not initialized
		}
		
		if (mLastSound != -1) {
			mSoundPool.stop(mLastSound);
			mLastSound = -1;
		}
		
		switch (sound) {
			case Boom:
				Log.i(TAG, "Play Sound: Boom");
				mSoundPool.play(mSoundBoom, leftVolume, rightVolume, priority, loop, rate);
				break;
			case Ouch:
				Log.i(TAG, "Play Sound: Ouch");
				mSoundPool.play(mSoundOuch, leftVolume, rightVolume, priority, loop, rate);
				break;
			case Tone:
				Log.i(TAG, "Play Sound: Tone");
				mLastSound = mSoundPool.play(mSoundTone, leftVolume, rightVolume, priority, loop, rate);
				break;
		}
	}
}
