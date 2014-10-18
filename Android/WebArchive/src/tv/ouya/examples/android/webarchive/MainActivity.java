/*
 * Copyright (C) 2012-2014 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package tv.ouya.examples.android.webarchive;


import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.webkit.WebView;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

public class MainActivity extends Activity
{
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private WebView mWebView = null;
    
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.layout_main);
        mWebView = (WebView)findViewById(R.id.wvContent);
        
        Context context = getApplicationContext();

		Log.i(TAG, "DataDir="+context.getApplicationInfo().dataDir);

		String dataDir = context.getApplicationInfo().dataDir;

		String startupUrl = "file://"+dataDir+"/index.html";

		byte[] buffer = new byte[1024];
		try {
			InputStream inputStream = getResources().openRawResource(R.raw.web_archive);
			ZipInputStream zis = new ZipInputStream(inputStream);
			ZipEntry ze = zis.getNextEntry();
			while (ze != null) {
				String filePath = ze.getName();
				Log.i(TAG, "Found="+ze.getName());
				File newFile = new File(dataDir + File.separator + filePath);
				
				if (ze.isDirectory()) {
					//Log.i(TAG, "Directory="+ze.getName());
					if (!newFile.exists()) {
						newFile.mkdirs();
					}
				} else {
					//Log.i(TAG, "Destination="+newFile.getAbsoluteFile());
					if (!newFile.isDirectory()) {
						String parentFolder = newFile.getParent();
						//Log.i(TAG, "Parent Folder="+parentFolder);
						if (!(new File(parentFolder).exists())) {
							//Log.i(TAG, "Creating Parent Folder="+parentFolder);
							new File(parentFolder).mkdirs();
						}
						if (newFile.exists()) {
							newFile.delete();
						}
		                if (!newFile.getAbsoluteFile().exists()) {
		                	try {
							    FileOutputStream fos = new FileOutputStream(newFile.getAbsoluteFile());
							    int len;
							    while ((len = zis.read(buffer)) > 0) {
								    fos.write(buffer, 0, len);
							    }
							    fos.flush();
							    fos.close();
		                	} catch (Exception e) {
		                		e.printStackTrace();
		                		Log.e(TAG, "Failed to create file: "+newFile.getAbsoluteFile());
		                	}
		                }
					}
				}
				ze = zis.getNextEntry();
			}
			zis.close();
			inputStream.close();
		} catch (IOException e) {
			e.printStackTrace();
        }
		
		mWebView.loadUrl(startupUrl);
    }
}
