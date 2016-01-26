package tv.ouya.examples.android.oggloop;

import java.io.IOException;

import android.media.MediaPlayer;
import android.os.Bundle;
import android.app.Activity;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.view.View;
import tv.ouya.examples.android.oggloop.R;

public class MainActivity extends Activity {
	
	private static final String TAG = "MainActivity";
	
	private static final String SOUND_1 = "tone.ogg";
	
	private MediaPlayer mMediaPlayer = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		View content = (View)findViewById(android.R.id.content);
        if (null != content) {
            // Disable screensaver
            content.setKeepScreenOn(true);
        }
		
		// create media player
        mMediaPlayer = new MediaPlayer();
        
        playSound(mMediaPlayer, SOUND_1);
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		
		if (null != mMediaPlayer) {
			if (mMediaPlayer.isPlaying()) {
				mMediaPlayer.stop();
			}
			mMediaPlayer.release();
    	}
	}
	
	private void playSound(MediaPlayer mp, String asset) {
		if (null == mp) {
			return;
		}
		
		AssetManager assetManager = getApplicationContext().getAssets();
		
		AssetFileDescriptor afd = null;
		try {
			afd = assetManager.openFd(asset);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		if (null == afd) {
			return;
		}
		
		mp.reset();
		
		try {
			mp.setDataSource(afd.getFileDescriptor(),
					afd.getStartOffset(),
					afd.getLength());
		} catch (IllegalArgumentException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalStateException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		try {
			mp.prepare();
		} catch (IllegalStateException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		try {
			afd.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		mp.setLooping(true);
		
		mp.start();
	}
}
