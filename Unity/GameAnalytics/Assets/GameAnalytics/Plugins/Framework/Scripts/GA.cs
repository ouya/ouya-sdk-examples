using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class GA { 
	
	public static GA_GameObjectManager _GA_controller;
	
	private static GA_Settings _settings; 
	
	public static GA_Settings SettingsGA
	{
		get{
			if( _settings == null)
			{
				InitAPI ();
			}
			return _settings;
		}
		private set{ _settings = value; }
	}
	
	public static GA_GameObjectManager GA_controller
	{
		get{
			if(_GA_controller == null)
			{
				var ga = new GameObject("GA_Controller");
				_GA_controller =  ga.AddComponent<GA_GameObjectManager>();
			}
			return _GA_controller;
		}
		private set{ _GA_controller = value; }
	}
	
	public class GA_API
	{
		public GA_Quality Quality = new GA_Quality();
		public GA_Error Error = new GA_Error();
		public GA_Design Design = new GA_Design();
		public GA_Business Business = new GA_Business();
		public GA_GenericInfo GenericInfo = new GA_GenericInfo();
		public GA_Debug Debugging = new GA_Debug();
		public GA_Archive Archive = new GA_Archive();
#if UNITY_EDITOR || !UNITY_FLASH
		public GA_Request Request = new GA_Request();	
#endif
		public GA_Submit Submit = new GA_Submit();
		public GA_User User = new GA_User();
	}
	
	private static GA_API api = new GA_API();
	public static GA_API API
	{ 
		get{
			if(GA.SettingsGA == null)
			{
				InitAPI ();
			}
			return api;
		}
		private set{}
	}

	private static void InitAPI ()
	{
		try
		{
			_settings = (GA_Settings)Resources.Load("GameAnalytics/GA_Settings",typeof(GA_Settings));
			
			#if UNITY_EDITOR
	 		if (_settings == null)
			{
				//If the settings asset doesn't exist, then create it. We require a resources folder
				if(!Directory.Exists(Application.dataPath+"/Resources"))
				{
					Directory.CreateDirectory(Application.dataPath+"/Resources");
				}
				if(!Directory.Exists(Application.dataPath+"/Resources/GameAnalytics"))
				{
					Directory.CreateDirectory(Application.dataPath+"/Resources/GameAnalytics");
					Debug.LogWarning("GameAnalytics: Resources/GameAnalytics folder is required to store settings. it was created ");
				}
				
				var asset = ScriptableObject.CreateInstance<GA_Settings>();
				//some hack to mave the asset around
				string path = AssetDatabase.GetAssetPath (Selection.activeObject);
				if (path == "") 
				{
					path = "Assets";
				}  
				else if (Path.GetExtension (path) != "") 
				{
					path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
				}
				string uniquePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/GameAnalytics/GA_Settings.asset");
				AssetDatabase.CreateAsset(asset, uniquePath);
				if(uniquePath != "Assets/Resources/GameAnalytics/GA_Settings.asset")
					GA.Log("GameAnalytics: The path Assets/Resources/GameAnalytics/GA_Settings.asset used to save the settings file is not available.");
				AssetDatabase.SaveAssets ();
				Debug.LogWarning("GameAnalytics: Settings file didn't exist and was created");
				Selection.activeObject = asset;
				
				//save reference
				_settings =	asset;
			}
			#endif
			GA.InitializeQueue(); //will also start a coroutine sending messages to the server if needed
		}
		catch (Exception e)
		{
			Debug.Log("Error getting GA_Settings in InitAPI: " + e.Message);
		}
	}
	 
	/// <summary>
	/// Setup involving other components
	/// </summary>
	private static void InitializeQueue ()
	{
		GA.API.Submit.SetupKeys(GA.SettingsGA.GameKey, GA.SettingsGA.SecretKey);
		
		if(!Application.isPlaying)
			return; // no need to setup anything else, if we are in the editor and not playing
		
		if (GA.API.GenericInfo.UserID == "" && !GA.SettingsGA.CustomUserID)
		{
			Debug.LogWarning("GA UserID not set. No data will be sent.");
			return;
		}
		
		GA.RunCoroutine(GA.SettingsGA.CheckInternetConnectivity(true));
	}
	
	/// <summary>
	/// Starts a new coroutine for the specified method, using the StartCoroutine Unity function.
	/// This is used to run the submits to the GameAnalytics server in a seperate routine.
	/// </summary>
	/// <param name="routine">
	/// The method to start in the new coroutine <see cref="IEnumerator"/>
	/// </param>
	/// <returns>
	/// The new coroutine <see cref="Coroutine"/>
	/// </returns>
	public static void RunCoroutine(IEnumerator routine)
	{
		 RunCoroutine(routine,()=>true); //Default coroutine
	}
	
	public static void RunCoroutine(IEnumerator routine,Func<bool> done)
	{
		if(!Application.isPlaying && Application.isEditor)
		{
#if UNITY_EDITOR
			GA_ContinuationManager.StartCoroutine(routine,done);
#endif
		}
		else
		{
			GA_controller.RunCoroutine(routine);
		}
	}
	
	public static void Log(object msg)
	{
		if(GA.SettingsGA.DebugMode)
			Debug.Log(msg);
	}
	
	public static void LogWarning(object msg)
	{
		Debug.LogWarning(msg);
	}
	
	public static void LogError(object msg)
	{
		Debug.LogError(msg);
	}
	
	#if UNITY_EDITOR
	
	public static void HierarchyWindowCallback (int instanceID, Rect selectionRect)
	{
		GameObject go = (GameObject)EditorUtility.InstanceIDToObject(instanceID);
		if (go != null && (go.GetComponent<GA_Tracker>() != null || go.GetComponent<GA_SystemTracker>() != null || go.GetComponent<GA_HeatMapDataFilter>() != null))
		{
			float addX = 0;
			if (go.GetComponent("PlayMakerFSM") != null)
				addX = selectionRect.height + 2;
			
			if (GA.SettingsGA.Logo == null)
			{
				GA.SettingsGA.Logo = (Texture2D)Resources.LoadAssetAtPath("Assets/GameAnalytics/Plugins/Examples/gaLogo.png", typeof(Texture2D));
				if (GA.SettingsGA.Logo == null)
					GA.SettingsGA.Logo = (Texture2D)Resources.LoadAssetAtPath("Assets/Plugins/GameAnalytics/Examples/gaLogo.png", typeof(Texture2D));
			}
			
			Graphics.DrawTexture(new Rect(GUILayoutUtility.GetLastRect().width - selectionRect.height - 5 - addX, selectionRect.y, selectionRect.height, selectionRect.height), GA.SettingsGA.Logo);
		}
	}
	
	#endif
}
