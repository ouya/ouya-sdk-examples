package tv.ouya.examples.android.safeareaexample;

import android.os.Bundle;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Point;
import android.util.Log;
import android.view.Display;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.view.WindowManager;
import android.widget.AbsoluteLayout;
import android.widget.FrameLayout;
import android.widget.SeekBar;
import android.widget.SeekBar.OnSeekBarChangeListener;
import android.widget.ToggleButton;

public class MainActivity extends Activity implements OnSeekBarChangeListener {
	
	private static final String TAG = "MainActivity";
	
	private FrameLayout frmView = null;
	
	private SeekBar sbSafeArea = null;
	
	private final float SEEK_MAX = 5;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		frmView = (FrameLayout)findViewById(R.id.frmView);
		sbSafeArea = (SeekBar)findViewById(R.id.sbSafeArea);
	}
	
    @Override
    protected void onStart() {
        super.onStart();
        
        sbSafeArea.setMax((int)SEEK_MAX);
		sbSafeArea.setProgress(sbSafeArea.getMax());
        
        sbSafeArea.setOnSeekBarChangeListener(this);
        
        updateSafeArea(1f);
    }
    
	@Override
	public void onProgressChanged(SeekBar arg0, int progress, boolean fromUser) {
		Log.i(TAG, "OnSeekBarChangeListener: "+progress/SEEK_MAX);
		updateSafeArea(progress/SEEK_MAX);
	}

	@Override
	public void onStartTrackingTouch(SeekBar arg0) {
	}

	@Override
	public void onStopTrackingTouch(SeekBar arg0) {
	}
	
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
	
	private void updateSafeArea(float progress) {
		
		// bring in by %
		float percent = 0.07f;
				
		float ratio = 1 - progress * percent;
		float halfRatio = 1 - progress * percent * 0.5f;
		
		float maxWidth = getDisplayWidth();
		float maxHeight = getDisplayHeight();
		
		LayoutParams layout = frmView.getLayoutParams();
		
		layout.width = (int)(maxWidth * ratio);
		layout.height = (int)(maxHeight * ratio);
		frmView.setLayoutParams(layout);
		
		frmView.setX(maxWidth - maxWidth * halfRatio);
		frmView.setY(maxHeight - maxHeight * halfRatio);
	}
}
