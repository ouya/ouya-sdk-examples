package tv.ouya.sdk.android.example.externalstorageexample;

import java.io.File;

import android.media.AudioManager;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.app.Activity;
import android.content.Context;
import tv.ouya.console.api.OuyaController;

public class MainActivity extends Activity {
	
	private Button m_btnGetExternalStorageApp = null;
	private TextView m_txtGetExternalStorageApp = null;
	
	private Button m_btnGetExternalStoragePublicDirectory = null;
	private TextView m_txtGetExternalStoragePublicDirectory = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);		
		setContentView(R.layout.activity_main);
		
		m_btnGetExternalStorageApp = (Button)findViewById(R.id.btnGetExternalStorageApp);
		m_txtGetExternalStorageApp = (TextView)findViewById(R.id.txtGetExternalStorageApp);
		
		m_btnGetExternalStoragePublicDirectory = (Button)findViewById(R.id.btnGetExternalStoragePublicDirectory);
		m_txtGetExternalStoragePublicDirectory = (TextView)findViewById(R.id.txtGetExternalStoragePublicDirectory);
	}

	@Override
	protected void onStart() {
		super.onStart();
		
		m_btnGetExternalStorageApp.setText("Environment.getExternalStorageApp");		
		UpdateResultGetExternalStorageApp("<-- hit the button");		
		m_btnGetExternalStorageApp.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				File file = Environment.getExternalStorageDirectory();
				if (null == file) {
					UpdateResultGetExternalStorageApp("file is null");					
				} else {
					UpdateResultGetExternalStorageApp("file: "+ file.getAbsolutePath());
				}
			}
		});
		
		m_btnGetExternalStoragePublicDirectory.setText("Environment.getExternalStoragePublicDirectory");
		UpdateResultGetExternalStoragePublicDirectory("<-- hit the button");		
		m_btnGetExternalStoragePublicDirectory.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				File file = Environment.getExternalStoragePublicDirectory("");
				if (null == file) {
					UpdateResultGetExternalStoragePublicDirectory("file is null");					
				} else {
					UpdateResultGetExternalStoragePublicDirectory("file: "+ file.getAbsolutePath());
				}
			}
		});
	}
	
	private void UpdateResultGetExternalStorageApp(String result) {
		m_txtGetExternalStorageApp.setText("result: " + result);
	}
	
	private void UpdateResultGetExternalStoragePublicDirectory(String result) {
		m_txtGetExternalStoragePublicDirectory.setText("result: " + result);
	}

}