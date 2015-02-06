/*
 * Copyright (C) 2012-2015 OUYA, Inc.
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

package tv.ouya.examples.unreal.unrealcommunitycontent;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import tv.ouya.console.api.content.OuyaContent;
import tv.ouya.console.api.content.OuyaContent.SaveListener;
import tv.ouya.console.api.content.OuyaMod;
import tv.ouya.sdk.OuyaInputView;
import tv.ouya.sdk.unreal.AsyncCppInitOuyaPlugin;
import tv.ouya.sdk.unreal.AsyncCppOuyaContentInit;
import tv.ouya.sdk.unreal.IUnrealOuyaActivity;
import tv.ouya.sdk.unreal.UnrealOuyaFacade;
import tv.ouya.sdk.unreal.UnrealOuyaPlugin;
import android.os.Bundle;
import android.os.Handler;
import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.Bitmap.CompressFormat;
import android.graphics.Bitmap.Config;
import android.graphics.BitmapFactory;
import android.util.Log;
import android.view.Menu;

public class MainActivity extends Activity {
	
	private static final String TAG = MainActivity.class.getSimpleName();
	
	private OuyaInputView mInputView = null;
	
	private Bitmap mScreenshot = null;
	
	private OuyaMod mOuyaMod = null;
	private OuyaMod mOuyaMod2 = null;

	static {
		System.loadLibrary("native-activity");
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		mInputView = new OuyaInputView(this);
		
		try {
            mScreenshot = BitmapFactory.decodeStream(getAssets().open("screenshot.jpg"));
        } catch(IOException e) {
            throw new RuntimeException(e);
        }
		
		String jsonData = null;
		
		try {
			JSONArray jsonArray = new JSONArray();
			JSONObject jsonObject = new JSONObject();
			jsonObject.put("key", "tv.ouya.developer_id");
			jsonObject.put("value", "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
			jsonArray.put(0, jsonObject);
			jsonData = jsonArray.toString();
			
			IUnrealOuyaActivity.SetActivity(this);
			
			AsyncCppInitOuyaPlugin.invoke(jsonData);
			
			AsyncCppOuyaContentInit.invoke();
			
			// expected to succeed
			testSave2();
			
			// expected to succeed
			testPublish2();
			
			// expected to succeed
			testSearchInstalled2();
			
			// expected to succeed
			testSearchPublished2();
			
			// expected to succeed
			testUnpublish2();

			// expected to succeed
			testDownload2();
			
			// expected to succeed
			//testDelete2();
			
			// expected to fail
			//testSave();
			
			// expected to fail without valid saved OuyaMod
			//testDownload();
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	@Override
	public void onDestroy() {
		super.onDestroy();
		mInputView.shutdown();
	}
	
	void testSave() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized()) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testSave();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{
			
					try {
						OuyaContent ouyaContent = IUnrealOuyaActivity.GetOuyaContent();
						Log.i(TAG, "OuyaContent is initialize="+ouyaContent.isInitialized());
						Log.i(TAG, "OuyaContent is available="+ouyaContent.isAvailable());
						mOuyaMod = ouyaContent.create();
						if (null == mOuyaMod) {
							Log.e(TAG, "OUYA Mod is null");
							return;
						}
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "Unreal OUYA Facade is null");
							return;
						}
						OuyaMod.Editor editor = mOuyaMod.edit();
						if (null == editor) {
							Log.e(TAG, "OUYA Mod Editor is null");
							return;
						}
						unrealOuyaFacade.saveOuyaMod(mOuyaMod, editor);
						Log.i(TAG, "Save invoked");
					} catch (Exception e) {
						Log.e(TAG, "Save failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testDownload() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized() ||
			null == mOuyaMod) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testDownload();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{			
					try {
						OuyaContent ouyaContent = IUnrealOuyaActivity.GetOuyaContent();
						Log.i(TAG, "OuyaMod isInstalled="+mOuyaMod.isInstalled());
						Log.i(TAG, "OuyaMod isDownloading="+mOuyaMod.isDownloading());
						UnrealOuyaPlugin.contentDownload(mOuyaMod);
						Log.i(TAG, "Download invoked");
					} catch (Exception e) {
						Log.e(TAG, "Download failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testSave2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized()) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testSave2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{			
					try {
						OuyaContent ouyaContent = IUnrealOuyaActivity.GetOuyaContent();
						Log.i(TAG, "OuyaContent is initialize="+ouyaContent.isInitialized());
						Log.i(TAG, "OuyaContent is available="+ouyaContent.isAvailable());
						mOuyaMod2 = ouyaContent.create();
						if (null == mOuyaMod2) {
							Log.e(TAG, "OUYA Mod is null");
							return;
						}
						UnrealOuyaFacade unrealOuyaFacade = IUnrealOuyaActivity.GetUnrealOuyaFacade();
						if (null == unrealOuyaFacade) {
							Log.e(TAG, "Unreal OUYA Facade is null");
							return;
						}
						OuyaMod.Editor editor = mOuyaMod2.edit();
						if (null == editor) {
							Log.e(TAG, "OUYA Mod Editor is null");
							return;
						}
						editor.setCategory(java.util.UUID.randomUUID().toString());
						editor.setDescription(java.util.UUID.randomUUID().toString());
						editor.setMetadata(java.util.UUID.randomUUID().toString());
						editor.setTitle(java.util.UUID.randomUUID().toString());
						editor.addTag(java.util.UUID.randomUUID().toString());
						final OutputStream os = editor.newFile(java.util.UUID.randomUUID().toString());
						try {
			                os.write(java.util.UUID.randomUUID().toString().getBytes());
			            } catch(IOException e) {
			                Log.e(TAG, "Unable to write file");
			            }
						try {
							os.close();
						} catch (IOException e) {
							Log.e(TAG, "Unable to close file");
						}
						if (null == mScreenshot)
						{
							Log.e(TAG, "Screenshot is null");
						} else {
							editor.addScreenshot(mScreenshot);
						}
						Bitmap bitmap = Bitmap.createBitmap(1920, 1080, Config.ARGB_8888);
						int x = 0;
						int y = 0;
						int alpha = 1;
						int red = 1;
						int green = 1;
						int blue = 1;
						int color = Color.argb(alpha, red, green, blue);
						bitmap.setPixel(x, y, color);
						ByteArrayOutputStream stream = new ByteArrayOutputStream();
						bitmap.compress(CompressFormat.PNG, 100, stream);
						editor.addScreenshot(bitmap);
						if (stream.size() >= 0)
						{
							byte[] buffer = stream.toByteArray();
							if (null == buffer) {
								Log.e(TAG, "Buffer is null");
							}
						}
						stream.close();
						editor.save(new SaveListener() {

							@Override
							public void onError(OuyaMod ouyaMod, int code,
									String reason) {
								Log.e(TAG, "Failed to save OuyaMod code="+code+" reason="+reason);
							}

							@Override
							public void onSuccess(OuyaMod ouyaMod) {
								Log.i(TAG, "-----------------");
								Log.i(TAG, "-----------------");
								Log.i(TAG, "-----------------");
								Log.i(TAG, "-----------------");
								Log.i(TAG, "isDownloading="+ouyaMod.isDownloading());
								Log.i(TAG, "isInstalled="+ouyaMod.isInstalled());
								Log.i(TAG, "isPublished="+ouyaMod.isPublished());
								List<String> filenames = ouyaMod.getFilenames();
								Log.i(TAG, "Found files count="+filenames.size());
								for (int count = 0; count < 5; ++count) {
									for (String filename : filenames) {
										Log.i(TAG, "Found: "+filename);
										InputStream is = ouyaMod.openFile(filename);
										byte[] buffer = new byte[10000];
										int bytesRead = 0;
										try {
											bytesRead = is.read(buffer);
										} catch (IOException e1) {
											// TODO Auto-generated catch block
											e1.printStackTrace();
										}
										Log.i(TAG, "Read bytes="+bytesRead);
										byte[] contents = new byte[bytesRead];
										System.arraycopy(buffer, 0, contents, 0, bytesRead);							
										Log.i(TAG, "Contents="+new String(contents));
										try {
											is.close();
										} catch (IOException e) {
											// TODO Auto-generated catch block
											e.printStackTrace();
										}
									}
								}
								
								unitTestNative(ouyaMod);
							}
							
						});
						UnrealOuyaPlugin.saveOuyaMod(editor, mOuyaMod2);						
						Log.i(TAG, "Save2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "Save2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testPublish2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized() ||
			null == mOuyaMod2) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testPublish2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{
					try {
						UnrealOuyaPlugin.contentPublish(mOuyaMod2);
						Log.i(TAG, "Publish2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "Publish2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testSearchInstalled2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized() ||
			null == mOuyaMod2) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testSearchInstalled2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{
					try {
						UnrealOuyaPlugin.getOuyaContentInstalled();
						Log.i(TAG, "SearchInstalled2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "SearchInstalled2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testSearchPublished2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized()) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testSearchPublished2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{
					try {
						UnrealOuyaPlugin.getOuyaContentPublished(OuyaContent.SortMethod.CreatedAt.toString());
						Log.i(TAG, "SearchPublished2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "SearchPublished2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testUnpublish2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized() ||
			null == mOuyaMod2 ||
			!mOuyaMod2.isPublished()) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testUnpublish2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{
					try {
						UnrealOuyaPlugin.contentUnpublish(mOuyaMod2);
						Log.i(TAG, "Unpublish2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "Unpublish2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testDownload2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized() ||
			null == mOuyaMod2) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testDownload2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{			
					try {
						Log.i(TAG, "OuyaMod isInstalled="+mOuyaMod2.isInstalled());
						Log.i(TAG, "OuyaMod isDownloading="+mOuyaMod2.isDownloading());
						UnrealOuyaPlugin.contentDownload(mOuyaMod2);
						Log.i(TAG, "Download2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "Download2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	void testDelete2() {
		if (null == IUnrealOuyaActivity.GetOuyaContent() ||
			!IUnrealOuyaActivity.GetOuyaContent().isInitialized() ||
			null == mOuyaMod2) {
			final Handler handler = new Handler();
			handler.postDelayed(new Runnable() {
				@Override
				public void run() {
					testDelete2();
				}
			}, 1000);
		} else {			
			Runnable runnable = new Runnable()
			{
				public void run()
				{			
					try {
						Log.i(TAG, "OuyaMod isInstalled="+mOuyaMod2.isInstalled());
						UnrealOuyaPlugin.contentDelete(mOuyaMod2);
						Log.i(TAG, "Delete2 invoked");
					} catch (Exception e) {
						Log.e(TAG, "Delete2 failed");
						e.printStackTrace();
					}
				}
			};			
			runOnUiThread(runnable);
		}		
	}
	
	public native void unitTestNative(OuyaMod ouyaMod);
}
