package tv.ouya.examples.android.leapjs;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;

import android.app.Activity;
import android.os.Bundle;
import android.webkit.WebView;

/**
 * An example full-screen activity that shows and hides the system UI (i.e.
 * status bar and navigation/system bar) with user interaction.
 * 
 * @see SystemUiHider
 */
public class MainActivity extends Activity {
	
	private WebView mWebView = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_main);
		
		mWebView = (WebView)findViewById(R.id.videoView1);
		mWebView.getSettings().setJavaScriptEnabled(true);
		
		String content = "";
		try {
	        InputStream input = getResources().openRawResource(R.raw.leap_index);
	        Reader is = new BufferedReader(new InputStreamReader(input, "windows-1252"));
            int size = input.available();
            byte[] buffer = new byte[size];
            input.read(buffer);
            input.close();
            content = new String(buffer);
        } catch (IOException e) {
            e.printStackTrace();
        }

		mWebView.loadDataWithBaseURL("file:///android_res/raw/", content, "text/html", "UTF-8", null);
	}
}
