package tv.ouya.examples.android.soundpoolexample;

import android.media.AudioManager;
import android.media.SoundPool;
import android.os.Bundle;
import android.app.Activity;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.Button;

public class MainActivity extends Activity {
	
	private static final String TAG = "MainActivity"; 
	
	private Button btnBoom = null;
	private Button btnOuch = null;
	
	private static SoundPool mSoundPool = null;
	
	private static int mSoundBoom = -1;
	private static int mSoundOuch = -1;
	
	private static final int SOUND_BOOM = 1;
	private static final int SOUND_OUCH = 2;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		// get the buttons
		btnBoom = (Button)findViewById(R.id.btnBoom);
		btnOuch = (Button)findViewById(R.id.btnOuch);
				
		// load the sound pool
		mSoundPool = new SoundPool(5, AudioManager.STREAM_MUSIC, 0);
		
		// load the resources
		mSoundBoom = mSoundPool.load(this, R.raw.boom, 1);
		mSoundOuch = mSoundPool.load(this, R.raw.ouch, 1);
		
		// set up events
		
		btnBoom.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	Log.i(TAG, "Clicked on Boom");
                PlaySound(SOUND_BOOM, 1, 1, 0, 0, 1);
            }
        });
		
		btnOuch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	Log.i(TAG, "Clicked on Ouch");
                PlaySound(SOUND_OUCH, 1, 1, 0, 0, 1);
            }
        });
	}
	
	public static void PlaySound(int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate) {
		
		if (null == mSoundPool ||
				-1 == mSoundBoom ||
				-1 == mSoundOuch) {
			Log.e(TAG, "Sound pool not initialized");
			return; //not initialized
		}
		
		switch (soundId) {
			case SOUND_BOOM:
				Log.i(TAG, "Play Sound: SOUND_BOOM");
				mSoundPool.play(mSoundBoom, leftVolume, rightVolume, priority, loop, rate);
				break;
			case SOUND_OUCH:
				Log.i(TAG, "Play Sound: SOUND_OUCH");
				mSoundPool.play(mSoundOuch, leftVolume, rightVolume, priority, loop, rate);
				break;
		}
	}
}
