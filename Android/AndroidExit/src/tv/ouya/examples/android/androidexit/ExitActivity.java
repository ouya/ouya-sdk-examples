package tv.ouya.examples.android.androidexit;

import android.app.Activity;
import android.content.res.Resources;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

public class ExitActivity extends Activity {
	
	private static final String TAG = ExitActivity.class.getSimpleName();
	
	Button m_btnExit = null;
	
	private String getStringResource(String name) {
		Resources resources = getResources();
		if (null == resources) {
			return "";
		}
		int id = resources.getIdentifier(name, "string", getPackageName());
		if (id <= 0) {
			return "";
		}
		return resources.getString(id);
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		View content = (View)findViewById(android.R.id.content);
		if (null != content) {
			Log.d(TAG, "disable screensaver");
			content.setKeepScreenOn(true);
		}
		
		m_btnExit = (Button)findViewById(R.id.btnExit);
		
		m_btnExit.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					finish();
				}
			});
		
		Log.d(TAG, "Localized string: app_name="+getStringResource("app_name"));
		Log.d(TAG, "Localized string: exit="+getStringResource("exit"));
		
		Log.i(TAG, "onCreate");
	}

	@Override
	protected void onPause() {
		if (isFinishing()) {
			Log.i(TAG, "onPause: Game is finishing...");
		} else {
			Log.i(TAG, "onPause");
		}
		super.onPause();
	}

	@Override
	protected void onResume() {
		Log.i(TAG, "onResume");
		super.onResume();
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		Log.i(TAG, "onDestroy");
	}
}
