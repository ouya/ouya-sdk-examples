/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;

[CustomEditor(typeof(GA_Settings))]
public class GA_Inspector : Editor
{
	private GUIContent _myPageLink				= new GUIContent("My Page", "Opens your home on the GameAnalytics web page");
	private GUIContent _documentationLink		= new GUIContent("Help", "Opens the GameAnalytics Unity3D package documentation page in your browser.");
	private GUIContent _checkForUpdates			= new GUIContent("Download", "Opens the GameAnalytics Unity3D SDK downloads page.");
	private GUIContent _publicKeyLabel			= new GUIContent("Game Key", "Your GameAnalytics Game Key - copy/paste from the GA website.");
	private GUIContent _privateKeyLabel			= new GUIContent("Secret Key", "Your GameAnalytics Secret Key - copy/paste from the GA website.");
	private GUIContent _apiKeyLabel				= new GUIContent("API Key", "Your GameAnalytics API Key - copy/paste from the GA website. This key is used for retrieving data from the GA servers, f.x. when you want to generate heatmaps.");
	private GUIContent _heatmapSizeLabel		= new GUIContent("Heatmap Grid Size", "The size in Unity units of each heatmap grid space. Data visualized as a heatmap must use the same grid size as was used when the data was collected, otherwise the visualization will be wrong.");
	private GUIContent _build					= new GUIContent("Build", "The current version of the game. Updating the build name for each test version of the game will allow you to filter by build when viewing your data on the GA website.");
	//private GUIContent _useBundleVersion		= new GUIContent("Use Bundle Version", "Uses the Bundle Version from Player Settings instead of the Build field above (only works for iOS, Android, and Blackberry).");
	private GUIContent _debugMode				= new GUIContent("Debug Mode", "Show additional debug messages from GA in the unity editor console.");
	private GUIContent _sendExampleToMyGame		= new GUIContent("Get Example Game Data", "If enabled data collected while playing the example tutorial game will be sent to your game (using your game key and secret key). Otherwise data will be sent to a premade GA test game, to prevent it from polluting your data.");
	private GUIContent _runInEditor				= new GUIContent("Run In Editor Play Mode", "Submit data to the GameAnalytics server while playing your game in the Unity editor.");
	private GUIContent _customUserID			= new GUIContent("Custom User ID", "If enabled no data will be submitted until a custom user ID is provided. This is useful if you have your own log-in system, which ensures you have a unique user ID.");
	private GUIContent _interval				= new GUIContent("Data Submit Interval", "This option determines how often (in seconds) data is sent to GameAnalytics.");
	private GUIContent _allowRoaming			= new GUIContent("Submit While Roaming", "If enabled and using a mobile device (iOS or Android), data will be submitted to the GameAnalytics servers while the mobile device is roaming (internet connection via carrier data network).");
	private GUIContent _archiveData				= new GUIContent("Archive Data", "If enabled data will be archived when an internet connection is not available. When an internet connection is established again, any archived data will be sent. Not supported on: Webplayer, Google Native Client, Flash, Windows Store Apps.");
	private GUIContent _archiveMaxSize			= new GUIContent("Size<", "Indicates the maximum disk space used for archiving data in bytes.");
	private GUIContent _newSessionOnResume		= new GUIContent("New Session On Resume", "If enabled and using a mobile device (iOS or Android), a new play session ID will be generated whenever the game is resumed from background.");
	private GUIContent _basic					= new GUIContent("Basic", "This tab shows general options which are relevant for a wide variety of messages sent to GameAnalytics.");
	private GUIContent _debug					= new GUIContent("Debug", "This tab shows options which determine how the GameAnalytics SDK behaves in the Unity3D editor.");
	private GUIContent _preferences				= new GUIContent("Advanced", "This tab shows advanced and misc. options for the GameAnalytics SDK.");
	private GUIContent _autoSubmitUserInfo		= new GUIContent("Auto Submit User Info", "If enabled information about platform, device, os, and os version is automatically submitted at the start of each session.");
	
	private GUIStyle _orangeUpdateLabelStyle;
	private GUIStyle _orangeUpdateIconStyle;
	
	void OnEnable()
	{
		GA_Settings ga = target as GA_Settings;
		
		if(ga.UpdateIcon == null)
		{
			ga.UpdateIcon = (Texture2D)Resources.LoadAssetAtPath("Assets/GameAnalytics/Plugins/Examples/update_orange.png", typeof(Texture2D));
			
			if (ga.UpdateIcon == null)
				ga.UpdateIcon = (Texture2D)Resources.LoadAssetAtPath("Assets/Plugins/GameAnalytics/Examples/update_orange.png", typeof(Texture2D));
		}
		
		if(ga.Logo == null)
		{
			ga.Logo = (Texture2D)Resources.LoadAssetAtPath("Assets/GameAnalytics/Plugins/Examples/gaLogo.png", typeof(Texture2D));
			
			if (ga.Logo == null)
				ga.Logo = (Texture2D)Resources.LoadAssetAtPath("Assets/Plugins/GameAnalytics/Examples/gaLogo.png", typeof(Texture2D));
			
			//http://www.base64-image.de
			/*String d = "";
			d += "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABmJLR0QA/wD/AP+gvaeTAAAA";
			d += "CXBIWXMAAA7EAAAOxAGVKw4bAAAAB3RJTUUH3AcFDBE3zqglrQAACQdJREFUeNrtW2tIVGsXf";
			d += "mY7jcp4m+yoTdqYKWqFQtnlR2lFJGVxpEwIoThIHPDX15+v/oQdoiCC/DoQQSUxENSh7PJDHM";
			d += "wL6lEY00Og2dV7U15GHXHUue71/WjPsGf2zDR7nE7ZtGDjOPPenudda73vu961Jfh6IuEeBYC";
			d += "dADYDyAKQBiCJ+z6CK2sCMANgDMAAgNcA/gHwN/c9cc93L2EAwjmwFwG84A0+0OcF19Zmru2w";
			d += "7xG4DIAcQAWA524A2CWAd6/7nOtDzvX5zWUFgFgA/wUwHiTQ/pAxzvUZy43hm0g0gHIAg0Gcc";
			d += "bEaMciNIfrfBC4FsB1Azb8I/EtE1HBjkn5t8BEAfuO89bcA7ouIMW5sEV8LfAyASgBGXuffEj";
			d += "x5GIeRG2NMsMH/AuBPANbvYNa/pA1Wbqy/BBP8re9E5cWYxK1gkBDDsbkcwHsa459LMYcIzp6";
			d += "sywi8J3OoDMQxSjmPalyG4N1JMHJYRC2R23lL3XIE707CGIfJ7x1ezQ8A3p2EGn92jCu4rSX9";
			d += "gAQQh83n2SGWt7f/EcC7kzDIYXRxdvwj7e8AUnkBDdGiVCpx6NAh5OfnY+3atUhOTgYRQSKRY";
			d += "HZ2Fl1dXdDpdKirq4NWq3Wpu2bNGlRUVGDr1q1QqVSQSqVgGAY9PT0wGAy4desW2traAg3OgM";
			d += "P2O4D/AbC4F5LzjrSiZz8jI4PUajVNTk6S0Wgki8VCLMuSu5jNZlpcXKTZ2VkaGBigkpISZxt";
			d += "Hjx6l4eFhQR2bzUZWq5WqqqqCoQXjHFZBJKci0MYvX75M09PTZLVaBYN3kOCJDCKiM2fOONu5";
			d += "fv062Ww2l7r8eh8+fAiWSVS4R5bCeZEcUbN/9+5dMplMFIjMz8/TyZMnCQBFR0dTS0uLz/IWi";
			d += "4X27t0bDC14zmEGwxGwEUAe96Pftq9Wq1FaWorw8HAQiY9Z9vf3o7+/HwBQWFiItLQ0n+UZhs";
			d += "GePXuWGqglDutG/pcXxbJ57tw5MhqNAvXmq7zVaiWz2ezyWCwWstvtRERUXV1NERERBICqqqo";
			d += "8tuPeZm9vb7DM4CIAiZQjoEgMjenp6SgpKYFcLnd6eADOz3a7HUNDQ3j48CFqa2uxsLDweZmR";
			d += "ybBp0yYUFhYiPT0djY2NMJlMCA8PR0ZGBiQSibMNx2cAzvYlEgnWrVuHzMxMvHnzZqkHvSIA5";
			d += "wAgXqztX7t2zcVZuXvsuro6v9oJCwsjALR7927q6+sTzPjo6KigH7PZTGfPng3WviAeAH4VUz";
			d += "k9PZ06Ozu9qml9fb3oAZ0/f94joeXl5QIzs9vt1N7eHiwz+BUA/hBT6dSpU6TX673Ofmpqqqh";
			d += "ByOVyevTokYDQ1tZWSkhIoImJCQHJMzMzlJycHAwC/mC46yq/ZePGjYiPj/f4W11dHYaGhkQZ";
			d += "Yl5eHrKyspw27rB7jUaDiYkJ9Pb2gmVZF18gk8lw5MiRYES7shjurs6/CElEBFQqldPhuUttb";
			d += "a1zkP7KypUrYbPZoNfrwbKss357ezsAoLGx0UkAfxxFRUXBICBNyl1U+iWrV69GXFycy2zw5d";
			d += "WrVwJiJBIJFAoFEhMTBUCsVitYlsWTJ0+gUCiQnZ2NjIwMyGQyvH//HgDQ1NSEyspKZx0iAsM";
			d += "wyMnJQXx8PKamppZCQJKUu6X1r3RSEhQK78UHBgaEl4YyGUpLS1FWVgaz2Swgx2q1wmKxgGVZ";
			d += "6PV6rFq1Ct3d3VhcXAQAdHZ2Ym5uDgqFwrk8AkBkZCSKi4tRXV29FAIUAGDz12ns2rWLenp6v";
			d += "G5V5XK5oE5UVBTduXNH1Ba5vLycpFKps436+nrBWcJqtdK9e/eW6gRtjBi6GIbxaePetsMWi0";
			d += "XUtHR1dcFmszn/b2hoEAYtpVLk5OQgOnpp14IMl5zgl0xNTWFubs7r7ykpKZ434CIcY0dHh8C";
			d += "uGxoaXFYIx9+EhAQcPHhwKfhNDJeB4ZfMzMz4JCAzM1MAlr9V9qUx/OVvenrapdyLFy8wMzPj";
			d += "siUGgJiYGBQUFCyFgBmGi5j6JTqdDpOTk17VPS8vTwDWZDLh6tWrKC4uxuHDh1FUVITjx49Do";
			d += "9G4lHV81mq1zrODQ1iWRUdHh0cHm5ubC4ZhAiVgDAD+EuM4Ll265DW48fLlS7/aWL9+PTU1NQ";
			d += "nqv337lrKzsz3WOX36tMdAy+joKB04cCBQJ/gXg88JSX5LW1ubx+UOADZs2IBjx459sY3MzEz";
			d += "k5+cLvtdqtQL1d8izZ89cNM+hMfHx8di5c2egGvCawedsLL+ltbUVIyMjXm345s2b2L9/v/e4";
			d += "+4oV2LJlC8LCwgR1m5ubMT4+7rFeX18fBgcHBSYWGRmJvLy8QAn4h8HnVDTAzzS0+fl5PH36F";
			d += "AaDQXBmJyLExcXh/v37uHDhApKShJvM3Nxc7Nu3TzCbCwsLzuiQJ2FZFs3NzR6daEpKCrZt2y";
			d += "YGuAPr346lUFQ6W1RUFDU2Nnr0Aw7bNJvNNDs7S58+faIHDx6QRqOhjx8/ktFo9Bg87ejooKy";
			d += "sLJ/9lpWVeexzYWGBKisrA0m/YwIOieXn59PIyIjPiK9D7Ha7MwzmLVp85coVio6O9tlnYmKi";
			d += "1/5qamoCCok5VGJzIBHhgoIC0ul0HuN43gbqjSz+/YC3RyqVklar9dhed3c35eTkiIkGbeZHh";
			d += "V8C6OJFTf2SlpYWlJWV4fXr1y5HWV8bIL7fcMi7d++g0+m+2B/LsmhqavK4kVKpVNixY4c/ti";
			d += "/hsL4M2sVIZGQkqdVqMhgMZDabv2gS/KixIzqclJTkV1/bt2/3qkm3b98WfTEicbsaGwCQIPZ";
			d += "+gH+3d+LECRQXFyMtLQ1hYWHOBwDsdjvsdjsAYHx8HFqtFmq1WuDdfYlMJsPjx4+hVCqhVCpB";
			d += "RDCZTBgeHoZGo8GNGzdgMBh8zf4EFwSadydABuA/AC4HI9QSGxsLlUqF1NRUKJVK2Gw2jI2NY";
			d += "WBgAHq9HhMTE/hGcsbb5WhIXo//TJD4mSLzM0nKo4R8mhwQ4omSDgnpVFmHhHSyNJ+EkE2X55";
			d += "tDyL4wwXeMIfvKDH+JDNmXptx3jCH52pz72SFkX5yE21E6JF+ddZdl9/K05CuSsSxen/8/Sd3";
			d += "GJdqpTWgAAAAASUVORK5CYII=";
			ga.Logo = new Texture2D(1,1);
			ga.Logo.LoadImage(System.Convert.FromBase64String(d));*/
		}
		
		/*if (ga.UseBundleVersion)
		{
			ga.Build = PlayerSettings.bundleVersion;
		}*/
	}
	
	override public void OnInspectorGUI ()
	{
		GA_Settings ga = target as GA_Settings;
		
		
		EditorGUI.indentLevel = 1;
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		

		GUILayout.Label(ga.Logo);
		
		GUILayout.BeginVertical();
		
		GUILayout.Label("GameAnalytics Unity SDK v." + GA_Settings.VERSION);
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button(_myPageLink, GUILayout.MaxWidth(75)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
		}
		if (GUILayout.Button(_documentationLink, GUILayout.MaxWidth(75)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
		}
		if (GUILayout.Button(_checkForUpdates, GUILayout.MaxWidth(75)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/DownloadSetup");
		}
		
		GUILayout.EndHorizontal();
		
		string updateStatus = GA_UpdateWindow.UpdateStatus(GA_Settings.VERSION);
		
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		
		if (!updateStatus.Equals(string.Empty))
		{
			if (_orangeUpdateLabelStyle == null)
			{
				_orangeUpdateLabelStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
				_orangeUpdateLabelStyle.normal.textColor = new Color(0.875f, 0.309f, 0.094f);
			}
			if (_orangeUpdateIconStyle == null)
			{
				_orangeUpdateIconStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
			}
			
			if (GUILayout.Button(ga.UpdateIcon, _orangeUpdateIconStyle, GUILayout.MaxWidth(17)))
			{
				OpenUpdateWindow();
			}
			
			GUILayout.Label(updateStatus, _orangeUpdateLabelStyle);
		}
		else
			GUILayout.Label("");
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		
		
		//Hints
		/*ga.DisplayHints = EditorGUILayout.Foldout(ga.DisplayHints,"Show Hints");
		if (ga.DisplayHints)
		{
			ga.DisplayHintsScrollState = GUILayout.BeginScrollView(ga.DisplayHintsScrollState, GUILayout.Height (100));
		
			List<GA_Settings.HelpInfo> helpInfos = ga.GetHelpMessageList();
			foreach(GA_Settings.HelpInfo info in helpInfos)
			{
				MessageType msgType = ConvertMessageType(info.MsgType);
				EditorGUILayout.HelpBox(info.Message, msgType);
			}
		
			GUILayout.EndScrollView();
		}*/
		
		//Tabs
		GUILayout.BeginHorizontal();
		
		GUIStyle activeTabStyle = new GUIStyle(EditorStyles.miniButtonMid);
		GUIStyle activeTabStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
		GUIStyle activeTabStyleRight = new GUIStyle(EditorStyles.miniButtonRight);
		
		activeTabStyle.normal = EditorStyles.miniButtonMid.active;
		activeTabStyleLeft.normal = EditorStyles.miniButtonLeft.active;
		activeTabStyleRight.normal = EditorStyles.miniButtonRight.active;
		
		GUIStyle inactiveTabStyle = new GUIStyle(EditorStyles.miniButtonMid);
		GUIStyle inactiveTabStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
		GUIStyle inactiveTabStyleRight = new GUIStyle(EditorStyles.miniButtonRight);
		
		if (GUILayout.Button(_basic, ga.CurrentInspectorState==GA_Settings.InspectorStates.Basic?activeTabStyleLeft:inactiveTabStyleLeft))
		{
			ga.CurrentInspectorState = GA_Settings.InspectorStates.Basic;
		}
		
		if (GUILayout.Button(_debug, ga.CurrentInspectorState==GA_Settings.InspectorStates.Debugging?activeTabStyle:inactiveTabStyle))
		{
			ga.CurrentInspectorState = GA_Settings.InspectorStates.Debugging;
		}
		if (GUILayout.Button(_preferences,ga.CurrentInspectorState==GA_Settings.InspectorStates.Pref?activeTabStyleRight:inactiveTabStyleRight))
		{
			ga.CurrentInspectorState = GA_Settings.InspectorStates.Pref;
		}
		GUILayout.EndHorizontal();
		
		if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Basic)
		{
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_publicKeyLabel, GUILayout.Width(75));
			ga.GameKey = EditorGUILayout.TextField("", ga.GameKey);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_privateKeyLabel, GUILayout.Width(75));
			ga.SecretKey = EditorGUILayout.TextField("", ga.SecretKey);
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
		
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_build, GUILayout.Width(75));
			ga.Build = EditorGUILayout.TextField("", ga.Build);
			GUILayout.EndHorizontal();
			
			/*GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_useBundleVersion, GUILayout.Width(150));
			ga.UseBundleVersion = EditorGUILayout.Toggle("", ga.UseBundleVersion);
			GUILayout.EndHorizontal();*/
		}
		
		if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Debugging)
		{
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_debugMode, GUILayout.Width(150));
			ga.DebugMode = EditorGUILayout.Toggle("", ga.DebugMode);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_runInEditor, GUILayout.Width(150));
		    ga.RunInEditorPlayMode = EditorGUILayout.Toggle("", ga.RunInEditorPlayMode);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_sendExampleToMyGame, GUILayout.Width(150));
		    ga.SendExampleGameDataToMyGame = EditorGUILayout.Toggle("", ga.SendExampleGameDataToMyGame);
			GUILayout.EndHorizontal();
		}
		
		if(ga.CurrentInspectorState == GA_Settings.InspectorStates.Pref)
		{
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_apiKeyLabel, GUILayout.Width(75));
			ga.ApiKey = EditorGUILayout.TextField("", ga.ApiKey);
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_heatmapSizeLabel, GUILayout.Width(150));
			GUILayout.EndHorizontal();
			
			#if UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0
			GUILayout.Space(-15);
			#endif
			
			ga.HeatmapGridSize = EditorGUILayout.Vector3Field("", ga.HeatmapGridSize);
			if (ga.HeatmapGridSize != Vector3.one)
			{
				EditorGUILayout.HelpBox("Editing the heatmap grid size must be done BEFORE data is submitted, and you must use the same grid size when setting up your heatmaps. Otherwise the heatmap data will be incorrectly displayed.", MessageType.Warning);
			}
			
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_interval, GUILayout.Width(150));
			
			int tmpTimer = 0;
			if (int.TryParse(EditorGUILayout.TextField(ga.SubmitInterval.ToString(), GUILayout.Width(38)), out tmpTimer))
			{
				ga.SubmitInterval = Mathf.Max(Mathf.Min(tmpTimer, 999), 1);
			}
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_customUserID, GUILayout.Width(150));
		    ga.CustomUserID = EditorGUILayout.Toggle("", ga.CustomUserID);
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_archiveData, GUILayout.Width(150));
		    ga.ArchiveData = EditorGUILayout.Toggle("", ga.ArchiveData, GUILayout.Width(36));
			GUI.enabled = ga.ArchiveData;
			GUILayout.Label(_archiveMaxSize, GUILayout.Width(40));
			
			int tmpMaxArchiveSize = 0;
			if (int.TryParse(EditorGUILayout.TextField(ga.ArchiveMaxFileSize.ToString(), GUILayout.Width(48)), out tmpMaxArchiveSize))
			{
				ga.ArchiveMaxFileSize = Mathf.Max(Mathf.Min(tmpMaxArchiveSize, 2000), 0);
			}
			
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_allowRoaming, GUILayout.Width(150));
		    ga.AllowRoaming = EditorGUILayout.Toggle("", ga.AllowRoaming);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_newSessionOnResume, GUILayout.Width(150));
		    ga.NewSessionOnResume = EditorGUILayout.Toggle("", ga.NewSessionOnResume);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(7));
		    GUILayout.Label(_autoSubmitUserInfo, GUILayout.Width(150));
		    ga.AutoSubmitUserInfo = EditorGUILayout.Toggle("", ga.AutoSubmitUserInfo);
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
		}
		
		if (GUI.changed)
		{
            EditorUtility.SetDirty(ga);
        }
	}
	
	private MessageType ConvertMessageType(GA_Settings.MessageTypes msgType)
	{
		switch (msgType)
		{
			case GA_Settings.MessageTypes.Error:
				return MessageType.Error;
			case GA_Settings.MessageTypes.Info:
				return MessageType.Info;
			case GA_Settings.MessageTypes.Warning:
				return MessageType.Warning;
			default:
				return MessageType.None;
		}
	}
	
	public static void CheckForUpdates ()
	{
		WWW www = new WWW("https://s3.amazonaws.com/download.gameanalytics.com/sdk_status/current.json");
		GA_ContinuationManager.StartCoroutine(CheckWebUpdate(www), () => www.isDone);
	}
	
	private static void GetUpdateChanges ()
	{
		WWW www = new WWW("https://s3.amazonaws.com/download.gameanalytics.com/sdk_status/latest_changes/unity.txt");
		GA_ContinuationManager.StartCoroutine(CheckWebChanges(www), () => www.isDone);
	}
	
	private static IEnumerator<WWW> CheckWebUpdate (WWW www)
	{
		yield return www;
		
		try {
			Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
			string newVersion = ((Hashtable)returnParam["unitysdk"])["version"].ToString();
			
			GA_UpdateWindow.SetNewVersion(newVersion);
			if (newVersion != GA_Settings.VERSION)
			{
				GetUpdateChanges();
			}
		}
		catch {}
	}
	
	private static IEnumerator<WWW> CheckWebChanges (WWW www)
	{
		yield return www;
		
		try {
			GA_UpdateWindow.SetChanges(www.text);
			
			string skippedVersion = EditorPrefs.GetString("ga_skip_version", "");
			
			if (!skippedVersion.Equals(GA_UpdateWindow.GetNewVersion()))
			{
				OpenUpdateWindow();
			}
		}
		catch {}
	}
	
	private static void OpenUpdateWindow ()
	{
		GA_UpdateWindow updateWindow = ScriptableObject.CreateInstance<GA_UpdateWindow> ();
		updateWindow.ShowUtility ();
		updateWindow.position = new Rect (150, 150, 420, 340);
	}
}