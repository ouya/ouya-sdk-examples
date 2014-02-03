using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

[RequireComponent(typeof(GA_SpecialEvents))]
[RequireComponent(typeof(GA_Gui))]

[ExecuteInEditMode]
public class GA_SystemTracker : MonoBehaviour {

	#region public values
	
	public static GA_SystemTracker GA_SYSTEMTRACKER;
	
	public bool UseForSubsequentLevels = true;
	
	public bool IncludeSystemSpecs = true;
	public bool IncludeSceneChange = true;
	public bool SubmitErrors = true;
	public int MaxErrorCount = 10;
	public bool SubmitErrorStackTrace = true;
	public bool SubmitErrorSystemInfo = true;
	public bool SubmitFpsAverage = true;
	public bool SubmitFpsCritical = true;
	public int FpsCriticalThreshold = 30;
	public int FpsCirticalSubmitInterval = 10;
	public bool GuiEnabled;
	public bool GuiAllowScreenshot;

	public bool ErrorFoldOut = true;
	
	#endregion
	
	#region unity derived methods
	
	#if UNITY_EDITOR
	void OnEnable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += GA.HierarchyWindowCallback;
		
		if (Application.isPlaying)
			GA_SYSTEMTRACKER = this;
	}
	
	void OnDisable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI -= GA.HierarchyWindowCallback;
	}
	#endif
	
	public void Awake ()
	{
		if (!Application.isPlaying)
			return;
		
		if (GA_SYSTEMTRACKER != null)
		{
			// only one system tracker allowed per scene
			GA.LogWarning("Destroying dublicate GA_SystemTracker - only one is allowed per scene!");
			Destroy(gameObject);
			return;
		}
		GA_SYSTEMTRACKER = this;
	}
	
	/// <summary>
	/// Setup involving other components
	/// </summary>
	public void Start ()
	{
		if (!Application.isPlaying || GA_SYSTEMTRACKER != this)
			return;
		
		if (UseForSubsequentLevels)
			DontDestroyOnLoad(gameObject);
		
		GA_Gui gui = GetComponent<GA_Gui>();
		gui.GuiAllowScreenshot = GuiAllowScreenshot;
		gui.GuiEnabled = GuiEnabled;
		
		GA.API.Debugging.SubmitErrors = SubmitErrors;
		GA.API.Debugging.SubmitErrorStackTrace = SubmitErrorStackTrace;
		GA.API.Debugging.SubmitErrorSystemInfo = SubmitErrorSystemInfo;
		GA.API.Debugging.MaxErrorCount = MaxErrorCount;
		
		if(GA.API.Debugging.SubmitErrors) //Warning this registerLogCallback is slow because it saves the stacktraces
			Application.RegisterLogCallback(GA.API.Debugging.HandleLog);
		
		// Add system specs to the submit queue
		if (IncludeSystemSpecs)
		{
			List<Hashtable> systemspecs = GA.API.GenericInfo.GetGenericInfo("");
			
			foreach (Hashtable spec in systemspecs)
			{
				GA_Queue.AddItem(spec, GA_Submit.CategoryType.GA_Log, false);
			}
		}
	}
	
	void OnDestroy()
	{
		if (!Application.isPlaying)
			return;
		
		if (GA_SYSTEMTRACKER == this)
			GA_SYSTEMTRACKER = null;	
	}
	
	#endregion
}
