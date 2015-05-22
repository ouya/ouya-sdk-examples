package tv.ouya.examples.android.androidvideoplayer;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.widget.MediaController;
import android.widget.TextView;
import android.widget.VideoView;

public class VideoActivity extends Activity {
	
	private static final String TAG = VideoActivity.class.getSimpleName();
	
	private VideoView mVideoView = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		Log.i(TAG, "Activity started");

		setContentView(R.layout.layout_video);
		
		mVideoView = (VideoView)findViewById(R.id.videoView1);
		mVideoView.setFocusable(false);
		
		Bundle extras = getIntent().getExtras();
		if (extras != null) {
		    String path = extras.getString("PLAY_VIDEO");
		    
		    if (null == path) {
		    	Log.e(TAG, "Path is null!");
		    	return;
		    }
		    
		    Log.i(TAG, "Playing video: "+path);
			Uri uri = Uri.parse(path); 
			
			mVideoView.setVideoURI(uri);
			
			String showControls = extras.getString("SHOW_CONTROLS");
			
			if (showControls.equals("true")) {
				// show controls
				mVideoView.setMediaController(new MediaController(this));
			} else {			
				// hide controls
				mVideoView.setMediaController(null);
			}
			
			mVideoView.requestFocus();
			mVideoView.start();
		}
	}

	@Override
	public boolean dispatchKeyEvent(KeyEvent event) {
		Log.i(TAG, "Key was detected!");
		finish();
		return super.dispatchKeyEvent(event);
	}
	
	
}
