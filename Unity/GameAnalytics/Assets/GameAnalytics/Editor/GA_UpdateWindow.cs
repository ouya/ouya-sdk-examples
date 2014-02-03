using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_UpdateWindow : EditorWindow
{
	private GUIContent _close					= new GUIContent("Skip", "Skip this version.");
	private GUIContent _download				= new GUIContent("Download Page", "Open the GameAnalytics download support page.");
	private GUIContent _assetStore				= new GUIContent("AssetStore", "Open Unity Asset Store page in a browser window.");
	
	private static string NewVersion = "";
	private static string Changes = "";
	private Vector2 _scrollPos;
	
	void OnGUI ()
	{
		GUILayout.BeginHorizontal();
		
		GUILayout.Label(GA.SettingsGA.Logo);
		
		GUILayout.BeginVertical();
		
		GUILayout.Label("A new version of the GameAnalytics Unity SDK is available");
		
		EditorGUILayout.Space();
		
		GUILayout.Label("Currently installed version: " + GA_Settings.VERSION);
		GUILayout.Label("Newest version: " + NewVersion);
		
		EditorGUILayout.Space();
		
		GUILayout.Label("Changes:");
		
		EditorGUILayout.Space();
		
		//EditorGUILayout.BeginVertical();
		
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width (320), GUILayout.Height (160));
		GUILayout.Label(new GUIContent(Changes), EditorStyles.wordWrappedLabel);
		EditorGUILayout.EndScrollView();
		
		//EditorGUILayout.EndVertical();
		
		EditorGUILayout.Space();
		
		GUILayout.Label("You can download the newest version from the Unity AssetStore, or from the GameAnalytics download support page.", EditorStyles.wordWrappedMiniLabel, GUILayout.MaxWidth(320));
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button(_assetStore, GUILayout.MaxWidth(115)))
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/#/content/6755");
		}
		
		if (GUILayout.Button(_download, GUILayout.MaxWidth(115)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/DownloadSetup");
		}
		
		if (GUILayout.Button(_close, GUILayout.MaxWidth(72)))
		{
			EditorPrefs.SetString("ga_skip_version", NewVersion);
			Close();
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
    }
	
	public static void SetNewVersion (string newVersion)
	{
		NewVersion = newVersion;
	}
	
	public static string GetNewVersion ()
	{
		return NewVersion;
	}
	
	public static void SetChanges (string changes)
	{
		Changes = changes;
	}
	
	public static string UpdateStatus (string currentVersion)
	{
		if (NewVersion.Equals(currentVersion))
			return "";
		else if (NewVersion.Equals(string.Empty))
			return "";
		else
			return "New update available";
	}
}