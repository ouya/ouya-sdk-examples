using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class GA_Menu : MonoBehaviour
{
	[MenuItem ("Window/GameAnalytics/Select GA_Settings", false, 0)]
	static void SelectGASettings ()
	{
		Selection.activeObject = GA.SettingsGA;
	}
	
	[MenuItem ("Window/GameAnalytics/GA Setup Wizard", false, 100)]
	static void SetupAndTour ()
	{
		GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard> ();
		wizard.ShowUtility ();
		wizard.position = new Rect (150, 150, 420, 340);
	}
	
	[MenuItem ("Window/GameAnalytics/GA Example Tutorial", false, 101)]
	static void ExampleTutorial ()
	{
		GA_ExampleTutorial tutorial = ScriptableObject.CreateInstance<GA_ExampleTutorial> ();
		tutorial.ShowUtility ();
		tutorial.position = new Rect (150, 150, 420, 340);
	}
	
	[MenuItem ("Window/GameAnalytics/Create GA_SystemTracker", false, 200)]
	static void AddGASystemTracker ()
	{
		if (FindObjectOfType (typeof(GA_SystemTracker)) == null) {
			GameObject go = new GameObject("GA_SystemTracker");
			go.AddComponent<GA_Gui>();
			go.AddComponent<GA_SpecialEvents>();
			go.AddComponent<GA_SystemTracker>();
			Selection.activeObject = go;
		} else {
			GA.LogWarning ("A GA_SystemTracker already exists in this scene - you should never have more than one per scene!");
		}
	}
	
	[MenuItem ("Window/GameAnalytics/Create GA_Heatmap", false, 201)]
	static void AddHeatMap ()
	{
		GameObject go = new GameObject("GA_HeatMap");
		go.AddComponent<GA_HeatMapRenderer>();
		go.AddComponent<GA_HeatMapDataFilter>();
		Selection.activeObject = go;
	}
	
	[MenuItem ("Window/GameAnalytics/Add GA_Tracker to Object", false, 202)]
	static void AddGATracker ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<GA_Tracker> () == null) {
				Selection.activeGameObject.AddComponent<GA_Tracker> ();
			} else {
				GA.LogWarning ("That object already contains a GA_Tracker component.");
			}
		} else {
			GA.LogWarning ("You must select the gameobject you want to add the GA_Tracker component to.");
		}
	}
	
	[MenuItem ("Window/GameAnalytics/Open GA_Status Window", false, 300)]
	static void OpenGAStatus ()
	{
		GA_Status status = ScriptableObject.CreateInstance<GA_Status> ();
		status.Show ();
	}
	
	[MenuItem ("Window/GameAnalytics/PlayMaker/Toggle Scripts", false, 400)]
	static void TogglePlayMaker ()
	{
		bool enabled = false;
		bool fail = false;
		
		string searchText = "#if false";
		string replaceText = "#if true";
		
		string filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendBusinessEvent.cs";
		string filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendBusinessEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendDesignEvent.cs";
		filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendDesignEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendQualityEvent.cs";
		filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendQualityEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendUserEvent.cs";
		filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendUserEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		AssetDatabase.Refresh();
		
		if (fail)
			Debug.Log("Failed to toggle PlayMaker Scripts.");
		else if (enabled)
			Debug.Log("Enabled PlayMaker Scripts.");
		else
			Debug.Log("Disabled PlayMaker Scripts.");
	}
	
	private static bool ReplaceInFile (string filePath, string searchText, string replaceText)
	{
		bool enabled = false;
		
		StreamReader reader = new StreamReader (filePath);
		string content = reader.ReadToEnd ();
		reader.Close ();
		
		if (content.Contains(searchText))
		{
			enabled = true;
			content = Regex.Replace (content, searchText, replaceText);
		}
		else
		{
			enabled = false;
			content = Regex.Replace (content, replaceText, searchText);
		}

		StreamWriter writer = new StreamWriter (filePath);
		writer.Write (content);
		writer.Close ();
		
		return enabled;
	}
	
	[MenuItem ("Window/GameAnalytics/Facebook/Toggle Scripts", false, 401)]
	static void ToggleFacebook ()
	{
		bool enabled = false;
		bool fail = false;
		
		string searchText = "#if false";
		string replaceText = "#if true";
		
		string filePath = Application.dataPath + "/GameAnalytics/Plugins/Framework/Scripts/GA_FacebookSDK.cs";
		string filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Framework/Scripts/GA_FacebookSDK.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		AssetDatabase.Refresh();
		
		if (fail)
			Debug.Log("Failed to toggle Facebook Scripts.");
		else if (enabled)
			Debug.Log("Enabled Facebook Scripts.");
		else
			Debug.Log("Disabled Facebook Scripts.");
	}
	
	[MenuItem ("Window/GameAnalytics/Folder Structure/Switch to JS", false, 600)]
	static void JsFolders ()
	{
		if (!Directory.Exists(Application.dataPath + "/GameAnalytics/Plugins/"))
		{
			Debug.LogWarning("Folder structure incompatible, did you already switch to JS folder structure, or have you manually changed the folder structure?");
			return;
		}
		
		if (!Directory.Exists(Application.dataPath + "/Plugins/"))
			AssetDatabase.CreateFolder("Assets", "Plugins");
		if (!Directory.Exists(Application.dataPath + "/Plugins/GameAnalytics"))
			AssetDatabase.CreateFolder("Assets/Plugins", "GameAnalytics");
		
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/Android", "Assets/Plugins/GameAnalytics/Android");
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/Components", "Assets/Plugins/GameAnalytics/Components");
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/Examples", "Assets/Plugins/GameAnalytics/Examples");
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/Framework", "Assets/Plugins/GameAnalytics/Framework");
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/iOS", "Assets/Plugins/GameAnalytics/iOS");
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/Playmaker", "Assets/Plugins/GameAnalytics/Playmaker");
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Plugins/WebPlayer", "Assets/Plugins/GameAnalytics/WebPlayer");
		
		if (!Directory.Exists(Application.dataPath + "/Editor/"))
			AssetDatabase.CreateFolder("Assets", "Editor");
		
		AssetDatabase.MoveAsset("Assets/GameAnalytics/Editor", "Assets/Editor/GameAnalytics");
		
		AssetDatabase.DeleteAsset("Assets/GameAnalytics/Plugins");
		AssetDatabase.DeleteAsset("Assets/GameAnalytics/Editor");
		AssetDatabase.DeleteAsset("Assets/GameAnalytics");
		
		AssetDatabase.Refresh();
	}
	
	[MenuItem ("Window/GameAnalytics/Folder Structure/Revert to original", false, 601)]
	static void RevertFolders ()
	{
		if (!Directory.Exists(Application.dataPath + "/Plugins/GameAnalytics/"))
		{
			Debug.LogWarning("Folder structure incompatible, are you already using original folder structure, or have you manually changed the folder structure?");
			return;
		}
		
		if (!Directory.Exists(Application.dataPath + "/GameAnalytics/"))
			AssetDatabase.CreateFolder("Assets", "GameAnalytics");
		if (!Directory.Exists(Application.dataPath + "/GameAnalytics/Plugins"))
			AssetDatabase.CreateFolder("Assets/GameAnalytics", "Plugins");
		
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/Android", "Assets/GameAnalytics/Plugins/Android");
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/Components", "Assets/GameAnalytics/Plugins/Components");
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/Examples", "Assets/GameAnalytics/Plugins/Examples");
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/Framework", "Assets/GameAnalytics/Plugins/Framework");
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/iOS", "Assets/GameAnalytics/Plugins/iOS");
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/Playmaker", "Assets/GameAnalytics/Plugins/Playmaker");
		AssetDatabase.MoveAsset("Assets/Plugins/GameAnalytics/WebPlayer", "Assets/GameAnalytics/Plugins/WebPlayer");
		
		AssetDatabase.MoveAsset("Assets/Editor/GameAnalytics", "Assets/GameAnalytics/Editor");
		
		AssetDatabase.DeleteAsset("Assets/Plugins/GameAnalytics");
		AssetDatabase.DeleteAsset("Assets/Editor/GameAnalytics");
		
		Debug.Log("Project must be reloaded when reverting folder structure.");
		EditorApplication.OpenProject(Application.dataPath.Remove(Application.dataPath.Length - "Assets".Length, "Assets".Length));
	}
}