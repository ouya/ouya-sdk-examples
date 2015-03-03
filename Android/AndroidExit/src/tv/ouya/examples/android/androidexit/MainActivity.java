package tv.ouya.examples.android.androidexit;

import android.app.Activity;
import android.content.res.Resources;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.FrameLayout;
import tv.ouya.console.api.*;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
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
		
		FrameLayout content = (FrameLayout)findViewById(android.R.id.content);
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
		
		if (OuyaFacade.isRunningOnOUYASupportedHardware(this)) {
			Log.d(TAG, getStringResource("is_running_on_ouya_supported_hardware"));
		} else {
			Log.d(TAG, getStringResource("not_running_on_ouya_supported_hardware"));
		}
	}
}