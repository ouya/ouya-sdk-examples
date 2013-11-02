package tv.ouya.sdk.android.example.externalstorageexample;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

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
	
	private Button m_btnCreateFile = null;
	private TextView m_txtCreateFile = null;
	
	private Button m_btnReadFile = null;
	private TextView m_txtReadFile = null;
	
	private Button m_btnDeleteFile = null;
	private TextView m_txtDeleteFile = null;
	
	private final String DATA_FILE = "ExternalStorageExample.txt";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);		
		setContentView(R.layout.activity_main);
		
		m_btnGetExternalStorageApp = (Button)findViewById(R.id.btnGetExternalStorageApp);
		m_txtGetExternalStorageApp = (TextView)findViewById(R.id.txtGetExternalStorageApp);
		
		m_btnGetExternalStoragePublicDirectory = (Button)findViewById(R.id.btnGetExternalStoragePublicDirectory);
		m_txtGetExternalStoragePublicDirectory = (TextView)findViewById(R.id.txtGetExternalStoragePublicDirectory);
		
		m_btnCreateFile = (Button)findViewById(R.id.btnCreateFile);
		m_txtCreateFile = (TextView)findViewById(R.id.txtCreateFile);
		
		m_btnReadFile = (Button)findViewById(R.id.btnReadFile);
		m_txtReadFile = (TextView)findViewById(R.id.txtReadFile);
		
		m_btnDeleteFile = (Button)findViewById(R.id.btnDeleteFile);
		m_txtDeleteFile = (TextView)findViewById(R.id.txtDeleteFile);
	}

	@Override
	protected void onStart() {
		super.onStart();
		
		m_btnGetExternalStorageApp.setText("Environment.getExternalStorageApp");		
		UpdateResultGetExternalStorageApp("<-- hit the button");		
		m_btnGetExternalStorageApp.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				GetExternalStorageDirectory();
			}
		});
		
		m_btnGetExternalStoragePublicDirectory.setText("Environment.getExternalStoragePublicDirectory");
		UpdateResultGetExternalStoragePublicDirectory("<-- hit the button");		
		m_btnGetExternalStoragePublicDirectory.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				GetExternalStoragePublicDirectory("");
			}
		});
		
		m_btnCreateFile.setText("Create file on App storage device");
		UpdateResultCreateFile("<-- hit the button");		
		m_btnCreateFile.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				File file = GetExternalStorageDirectory();
				if (file.exists())
				{
					File newFile = new File(file.getAbsolutePath() + File.separatorChar + GetRelativeDataFilePath());
					new File(newFile.getParent()).mkdirs();
					if (!newFile.exists()) {
						try {
							newFile.createNewFile();
						} catch (IOException e) {
							UpdateResultCreateFile("Could not create file: " + GetRelativeDataFilePath() + " " + e.toString());
							return;
						}					
					}
					
					FileWriter fw;
					try {
						fw = new FileWriter(newFile.getAbsoluteFile());						
						BufferedWriter bw = new BufferedWriter(fw);
						String contents = "Hello External Storage!";
						bw.write(contents);
						bw.close();
						fw.close();
						UpdateResultCreateFile("Wrote data file: " + GetRelativeDataFilePath() + " contents: " + contents);
					} catch (IOException e) {
						UpdateResultCreateFile("Could not write file: " + GetRelativeDataFilePath() + " " + e.toString());
						return;
					}
				}
				else
				{
					UpdateResultCreateFile("GetExternalStorageDirectory File does not exist");
				}
			}
		});
		
		m_btnReadFile.setText("Read file on App storage device");
		UpdateResultReadFile("<-- hit the button");		
		m_btnReadFile.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				File file = GetExternalStorageDirectory();
				if (file.exists())
				{
					File newFile = new File(file.getAbsolutePath() + File.separatorChar + GetRelativeDataFilePath());
					new File(newFile.getParent()).mkdirs();
					if (newFile.exists()) {
						FileReader fr;
						try {
							fr = new FileReader(newFile.getAbsoluteFile());						
							BufferedReader br = new BufferedReader(fr);
							String contents = br.readLine();
							br.close();
							fr.close();
							UpdateResultReadFile("Read data file: " + GetRelativeDataFilePath() + " contents: " + contents);
						} catch (IOException e) {
							UpdateResultReadFile("Could not read file: " + GetRelativeDataFilePath() + " " + e.toString());
							return;
						}
					} else {
						UpdateResultReadFile("Data file does not exist: " + GetRelativeDataFilePath());
					}
				}
				else
				{
					UpdateResultReadFile("GetExternalStorageDirectory File does not exist");
				}
			}
		});
		
		m_btnDeleteFile.setText("Delete file on App storage device");
		UpdateResultDeleteFile("<-- hit the button");		
		m_btnDeleteFile.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				File file = GetExternalStorageDirectory();
				if (file.exists())
				{
					File newFile = new File(file.getAbsolutePath() + File.separatorChar + GetRelativeDataFilePath());
					new File(newFile.getParent()).mkdirs();
					if (newFile.exists()) {
						newFile.delete();
						UpdateResultDeleteFile("Data file deleted: " + GetRelativeDataFilePath());
					} else {
						UpdateResultDeleteFile("Data file does not exist: " + GetRelativeDataFilePath());
					}
				}
				else
				{
					UpdateResultDeleteFile("GetExternalStorageDirectory File does not exist");
				}
			}
		});
	}
	
	private String GetRelativeDataFilePath() {
		Context context = getApplicationContext();
		String packageName = context.getPackageName();
		return "Android" + File.separatorChar + "data" + File.separatorChar + packageName + File.separatorChar + DATA_FILE;
	}
	
	private File GetExternalStorageDirectory() {
		File file = Environment.getExternalStorageDirectory();
		if (null == file) {
			UpdateResultGetExternalStorageApp("file is null");					
		} else {
			UpdateResultGetExternalStorageApp("file: "+ file.getAbsolutePath());
		}
		return file;
	}
	
	private File GetExternalStoragePublicDirectory(String type) {
		File file = Environment.getExternalStoragePublicDirectory(type);
		if (null == file) {
			UpdateResultGetExternalStoragePublicDirectory("file is null");					
		} else {
			UpdateResultGetExternalStoragePublicDirectory("file: "+ file.getAbsolutePath());
		}
		return file;
	}
	
	private void UpdateResultGetExternalStorageApp(String result) {
		m_txtGetExternalStorageApp.setText("result: " + result);		
	}
	
	private void UpdateResultGetExternalStoragePublicDirectory(String result) {
		m_txtGetExternalStoragePublicDirectory.setText("result: " + result);
	}
	
	private void UpdateResultCreateFile(String result) {
		m_txtCreateFile.setText("result: " + result);
	}
	
	private void UpdateResultReadFile(String result) {
		m_txtReadFile.setText("result: " + result);
	}
	
	private void UpdateResultDeleteFile(String result) {
		m_txtDeleteFile.setText("result: " + result);
	}

}