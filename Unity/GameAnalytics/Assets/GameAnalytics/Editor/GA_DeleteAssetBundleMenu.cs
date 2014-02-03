using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_DeleteAssetBundleMenu : EditorWindow {
	
	private Vector2 scrollPosition = new Vector2();
	
	private List<string> meshNames = new List<string>();
	private List<string> meshSizes = new List<string>();
	private bool[] meshSelected;
	
	private bool _loaded = false;
	private bool _error = false;
	
	private bool closeOnNextUpdate = false;
	
    void OnGUI ()
	{
		EditorGUILayout.LabelField("Asset Bundle Meshes:");
		if (_loaded && !_error)
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			for(int i = 0; i < meshSelected.Length; i++)
			{
				GUILayout.BeginHorizontal();
				meshSelected[i] = EditorGUILayout.Toggle(meshSelected[i], GUILayout.Width(20));
				EditorGUILayout.LabelField(meshNames[i] + " [" + meshSizes[i] + "]");
				GUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
		}
		else if (_error)
		{
			EditorGUILayout.LabelField("No meshes found for your game key.");
		}
		else
		{
			EditorGUILayout.LabelField("Loading meshes..");
		}
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Delete Selected"))
		{
			DeleteMeshes();
		}
		if (GUILayout.Button("Close Menu"))
		{
			Close();
		}
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
    }
	
	private void DeleteMeshes ()
	{
		_loaded = false;
		
		if (meshSelected == null)
			return;
		
		for (int i = 0; i < meshSelected.Length; i++)
		{
			if (meshSelected[i])
			{
				WWWForm form = new WWWForm ();
	
				form.AddField ("gamekey", GA.SettingsGA.GameKey);
				form.AddField ("authorization", GA.API.Submit.CreateMD5Hash(GA.SettingsGA.GameKey + meshNames[i] + ".unity3d" + GA.SettingsGA.SecretKey));
				form.AddField ("meshname", meshNames[i] + ".unity3d");
				
				WWW w = new WWW ("https://go.gameanalytics.com/api/heatmap/mesh/delete", form);
				
				while (!w.isDone) { }
				
				if (w.error != null)
				{
					Debug.LogWarning("Failed to delete mesh: " + meshNames[i]);
				}
				else
				{
					Hashtable data = (Hashtable)GA_MiniJSON.JsonDecode(w.text);
					
					ArrayList errorArray = data["errors"] as ArrayList;
					
					if (errorArray.Count > 0)
					{
						foreach (Hashtable ht in errorArray)
						{
							string msg = (string)ht["message"];
							if (msg.Equals("404"))
								Debug.LogWarning("Game key or mesh not found! Have you input the correct game key in GA_Settings?");
							else if (msg.Equals("401"))
								Debug.LogWarning("Authorization failed! Have you input the correct secret key in GA_Settings?");
							else if (msg.Equals("400"))
								Debug.LogWarning("Failed to delete mesh.");
							else if (msg.Equals("503"))
								Debug.LogWarning("Service unavailable, please try again later!");
						}
					}
					else
					{
						Debug.Log("Deleted mesh: " + meshNames[i]);
					}
				}
			}
		}
		UpdateMeshList();
	}
	
	public void UpdateMeshList ()
	{
		WWWForm form = new WWWForm ();

		form.AddField ("gamekey", GA.SettingsGA.GameKey);
		form.AddField ("authorization", GA.API.Submit.CreateMD5Hash(GA.SettingsGA.GameKey + GA.SettingsGA.SecretKey));
		
		WWW w = new WWW ("https://go.gameanalytics.com/api/heatmap/meshes", form);

		while (!w.isDone) { }

		if (w.error != null)
		{
			Debug.LogWarning("Failed to load mesh names " + w.error);
		}
		else
		{
			try
			{
				Hashtable data = (Hashtable)GA_MiniJSON.JsonDecode(w.text);
				
				ArrayList errorArray = data["errors"] as ArrayList;
				ArrayList meshArray = data["results"] as ArrayList;
				
				if (errorArray.Count > 0 || meshArray.Count == 0)
				{
					foreach (Hashtable ht in errorArray)
					{
						string msg = (string)ht["message"];
						if (msg.Equals("404"))
							Debug.LogWarning("Game key not found! Have you input the correct game key in GA_Settings?");
						else if (msg.Equals("401"))
							Debug.LogWarning("Authorization failed! Have you input the correct secret key in GA_Settings?");
						else if (msg.Equals("400"))
							Debug.LogWarning("Failed to load meshes.");
						else if (msg.Equals("503"))
							Debug.LogWarning("Service unavailable, please try again later!");
					}
					
					_error = true;
				}
				else
				{
					meshNames = new List<string>();
					meshSizes = new List<string>();
					foreach (Hashtable ht in meshArray)
					{
						meshNames.Add(((string)ht["name"]).Replace(".unity3d", ""));
						meshSizes.Add(((string)ht["size"]));
					}
					
					meshSelected = new bool[meshNames.Count];
				}
			}
			catch
			{
				_error = true;
			}
			
			_loaded = true;
		}
	}
	
	void Update()
	{
		if (closeOnNextUpdate)
		{
			Close();
		}
	}
	
	void OnLostFocus()
	{
        closeOnNextUpdate = true;
    }
}