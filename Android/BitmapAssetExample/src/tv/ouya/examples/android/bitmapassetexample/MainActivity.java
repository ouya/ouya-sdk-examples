package tv.ouya.examples.android.bitmapassetexample;

import java.io.IOException;
import java.io.InputStream;

import android.os.Bundle;
import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Log;
import android.widget.ImageView;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private static final String sBitmapAsset = "Subfolder/ExampleBitmap.png";
	
	private ImageView mImgBitmap = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		Log.i(TAG, "**********");
		Log.i(TAG, "**********");
		Log.i(TAG, "*onCreate*");
		Log.i(TAG, "**********");
		Log.i(TAG, "**********");
		
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		mImgBitmap = (ImageView)findViewById(R.id.imgBitmap);
		if (null == mImgBitmap) {
			Log.e(TAG, "Bitmap ImageView is null: "+sBitmapAsset);
			return;
		}
		
		InputStream is = null;
		try {
			is = getAssets().open(sBitmapAsset);
			if (null == is) {
				Log.e(TAG, "Bitmap input stream is null: "+sBitmapAsset);
				return;
			}
			Log.i(TAG, "Opened bitmap asset: "+sBitmapAsset);
		} catch (IOException e) {
			Log.e(TAG, "Failed to open bitmap asset: "+sBitmapAsset);
			e.printStackTrace();
			return;
		}
		
		Bitmap bitmap = BitmapFactory.decodeStream(is);
		if (null == bitmap) {
			Log.e(TAG, "Bitmap is null: "+sBitmapAsset);
			return;
		}
		Log.i(TAG, "Loaded bitmap: "+sBitmapAsset);
		
		try {
			is.close();
			Log.i(TAG, "Closed bitmap stream: "+sBitmapAsset);
		} catch (IOException e) {
			Log.e(TAG, "Failed to close bitmap stream: "+sBitmapAsset);
			e.printStackTrace();
			return;
		}
		
		Log.i(TAG, "Set bitmap ImageView: "+sBitmapAsset);
		mImgBitmap.setImageBitmap(bitmap);
	}
}
