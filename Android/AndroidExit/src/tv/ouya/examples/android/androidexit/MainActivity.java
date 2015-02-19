package tv.ouya.examples.android.androidexit;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	Button m_btnExit = null;
	
	private String getStringResource(String name) {
		int id = getResources().getIdentifier(name, "string", getPackageName());
		if (id <= 0) {
			return "";
		}
		return getResources().getString(id);
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		m_btnExit = (Button)findViewById(R.id.btnExit);
		
		m_btnExit.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					finish();
				}
			});
		
		Log.i(TAG, "Localized string: app_name="+getStringResource("app_name"));
		Log.i(TAG, "Localized string: exit="+getStringResource("exit"));
	}
}