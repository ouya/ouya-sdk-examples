package tv.ouya.examples.android.inputview;

import tv.ouya.sdk.OuyaInputView;
import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;

public class MainActivity extends Activity {
	
	private OuyaInputView mInputView = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		mInputView = new OuyaInputView(this, getApplicationContext());
	}
}
