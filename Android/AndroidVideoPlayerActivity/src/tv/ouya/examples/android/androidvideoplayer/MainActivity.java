package tv.ouya.examples.android.androidvideoplayer;

import tv.ouya.examples.android.androidvideoplayer.R;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

/**
 * An example full-screen activity that shows and hides the system UI (i.e.
 * status bar and navigation/system bar) with user interaction.
 * 
 * @see SystemUiHider
 */
public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		Log.i(TAG, "Activity started");
		
		setContentView(R.layout.layout_main);
		
		Button btnPlayVideo = (Button)findViewById(R.id.btnPlayVideo);
		btnPlayVideo.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				String path = "android.resource://"+getPackageName()+"/"+R.raw.video;
				PlayFullScreenMovie(path, false);
			}
		});
		
		Button btnPlayVideoControls = (Button)findViewById(R.id.btnPlayVideoControls);
		btnPlayVideoControls.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				String path = "android.resource://"+getPackageName()+"/"+R.raw.video;
				PlayFullScreenMovie(path, true);
			}
		});
		
		Button btnExit = (Button)findViewById(R.id.btnExit);
		btnExit.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				finish();
			}
		});
	}
	
	public void PlayFullScreenMovie(String path, boolean showControls) {
		Intent openIntent = new Intent(getPackageName()+".ACTION1");
		openIntent.putExtra("PLAY_VIDEO", path);
		openIntent.putExtra("SHOW_CONTROLS", Boolean.toString(showControls));
        startActivity(openIntent);
        
        //leave the main activity running
	}
}
