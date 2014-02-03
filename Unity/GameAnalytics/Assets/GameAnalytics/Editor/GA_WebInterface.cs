using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GA_WebInterface : MonoBehaviour
{
	[MenuItem ("Window/GameAnalytics/Export Mesh AssetBundle", false, 500)]
	static void Export ()
	{
		if (!UnityEditorInternal.InternalEditorUtility.HasPro()) {
			Debug.Log ("Building AssetBundles requires Unity Pro.");
			return;
		}
		
		if (Selection.gameObjects.Length == 0) {
			Debug.Log ("You must select at least one object to add to the asset bundle.");
			return;
		}
		
		if (GA.SettingsGA.GameKey == string.Empty) {
			Debug.Log ("To export mesh AssetBundles you must first select the GA_Settings object and enter your Game Key (found on GA's website under your game settings)");
			return;
		}
		
		// Bring up save panel
		string[] currentSceneSplit = EditorApplication.currentScene.Split('/');
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", currentSceneSplit[currentSceneSplit.Length - 1].Replace(".unity", ""), "unity3d");
		if (path.Length != 0)
		{
			Debug.Log ("Preparing to export mesh AssetBundle. This might take a while.");
			
			EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Might take a while", 0.1f);
			List<Transform> parents = new List<Transform> ();
			GameObject go = new GameObject ("GA_TMP_DONT_DELETE");
			foreach (GameObject selected in Selection.gameObjects)
			{
				EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Collecting " + parents.Count + 1 + "/" + Selection.gameObjects.Length, 0.1f);
				parents.Add (selected.transform.parent);
				selected.transform.parent = go.transform;
			}
			EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Creating temp prefab. Might take a while.", 0.1f);
			
			GameObject prefab = PrefabUtility.CreatePrefab ("Assets/GA_tmp.prefab", go);
			
			
			//Destroy all the things we don't need
			Debug.Log ("Stripping gameobjects from unecessary components");
			EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Stripping gameobjects from unecessary components.", 0.2f);
			
			StripGameObjects (prefab);
			Debug.Log ("Building asset bundle");
			EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Building and uploading AssetBundle.", 0.5f);
			
			// Build the resource file from the active selection.
			BuildPipeline.BuildAssetBundle (prefab, null, path);
			
			string[] splitPath = path.Split('/');
			UploadAssetBunble (path, splitPath[splitPath.Length - 1]);
			
			EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Cleaning up.", 0.9f);
			
			//IMPORTANT: If this code fails we will delete peoples scene. That would be sort of bad!
			int i = 0;
			foreach (GameObject selected in Selection.gameObjects) {
				
				selected.transform.parent = parents [i];
				i++;
				
			}
			//revert parents and prefabs
			Debug.Log ("Destroy temp. data");
			DestroyImmediate (prefab, true);
			DestroyImmediate (go, true);
			
			EditorUtility.DisplayProgressBar ("Exporting static mesh. Please wait...", "Done!", 1.0f);
			
			Debug.Log ("Finished exporting static mesh!");
			EditorUtility.ClearProgressBar ();
		} else {
			Debug.Log ("Export cancelled");
		}
	}
	
	static void UploadAssetBunble (string path, string fileName)
	{
		byte[] meshData = File.ReadAllBytes (path);
		
		if (!fileName.EndsWith(".unity3d"))
			fileName = fileName + ".unity3d";
		
		WWWForm form = new WWWForm ();
		
		form.AddField ("gamekey", GA.SettingsGA.GameKey);
		form.AddField ("authorization", GA.API.Submit.CreateMD5Hash(GA.SettingsGA.GameKey + fileName + GA.SettingsGA.SecretKey));
		form.AddBinaryData ("mesh", meshData, fileName, "application/vnd.unity");
		
		WWW w = new WWW ("https://go.gameanalytics.com/api/heatmap/mesh/upload", form);
		
		while (!w.isDone)
		{
			EditorUtility.DisplayProgressBar ("Uploading static mesh. Please wait...", "Might take a while", w.progress);
		}
		
		if (w.error != null)
		{
			Debug.LogWarning("Mesh Upload Error: " + w.error);
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
						Debug.LogWarning("Game not found! Have you input the correct game key in GA_Settings?");
					else if (msg.Equals("401"))
						Debug.LogWarning("Authorization failed! Have you input the correct secret key in GA_Settings?");
					else if (msg.Equals("400"))
						Debug.LogWarning("Failed to upload mesh.");
					else if (msg.Equals("503"))
						Debug.LogWarning("Service unavailable, please try again later!");
					else if (msg.Equals("409"))
						Debug.LogWarning("Mesh already exists! please try with a different name, or delete the existing mesh through the GameAnalytics > Delete AssetBundle Menu.");
				}
			}
			else
			{
				Debug.Log("Mesh Upload: Completed with no errors.");
			}
		}
		
		if (File.Exists(path))
		{
			Debug.Log ("Deleting local asset bundle file after upload");
			File.Delete(path);
		}
	}
	
	public static void StripGameObjects (GameObject prefab)
	{
		#if !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2
		
		List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
		BuildSpriteLayersList(prefab, spriteRenderers);
		
		if (spriteRenderers.Count > 0)
		{
			spriteRenderers = spriteRenderers.OrderBy(x => x.sortingLayerID).ThenBy(x => x.sortingOrder).ToList();
			
			for (int i = 0; i < spriteRenderers.Count; i++)
			{
				spriteRenderers[i].sortingLayerName = "Default";
				spriteRenderers[i].sortingOrder = i;
			}
		}
		
		#endif
		
		foreach (MonoBehaviour bh in prefab.GetComponentsInChildren<MonoBehaviour>()) {
			DestroyImmediate (bh);
		}
		foreach (Animation a in prefab.GetComponentsInChildren<Animation>()) {
			DestroyImmediate (a);
		}
		foreach (AudioSource a in prefab.GetComponentsInChildren<AudioSource>()) {
			DestroyImmediate (a);
		}
		foreach (Camera c in prefab.GetComponentsInChildren<Camera>()) {
			DestroyImmediate (c);
		}
		foreach (Rigidbody r in prefab.GetComponentsInChildren<Rigidbody>()) {
			DestroyImmediate (r);
		}
		foreach (Collider c in prefab.GetComponentsInChildren<Collider>()) {
			DestroyImmediate (c);
		}
	}
	
	#if !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2
	
	private static void BuildSpriteLayersList (GameObject go, List<SpriteRenderer> spriteRenderers)
	{
		SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
		if (sr != null)
		{
			spriteRenderers.Add(sr);
		}
		
		for (int i = 0; i < go.transform.childCount; i++)
		{
			BuildSpriteLayersList(go.transform.GetChild(i).gameObject, spriteRenderers);
		}
	}
	
	#endif
	
	[MenuItem ("Window/GameAnalytics/Delete AssetBundle Menu", false, 501)]
	static void OpenDeleteAssetMenu ()
	{
		GA_DeleteAssetBundleMenu deleteMenu = ScriptableObject.CreateInstance<GA_DeleteAssetBundleMenu>();
		deleteMenu.ShowUtility();
		deleteMenu.UpdateMeshList();
	}
}
