/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;

[CustomEditor(typeof(GA_SystemTracker))]
public class GA_SystemTrackerInspector : Editor
{
	private GUIContent _documentationLink		= new GUIContent("Help", "Opens the GameAnalytics Unity3D package documentation page in your browser. https://beta.gameanalytics.com/docs/unity3d.html");
	private GUIContent _useForSubsequentLevels	= new GUIContent("Use For Subsequent Levels", "Do NOT destroy this GA_SystemTracker when loading a new level. By enabling this option you can simply add a single GA_SystemTracker to the first scene in your game, and it will be used for all levels (NOTE: you can only ever have one GA_SystemTracker per scene, so if you enable this option you should not add others in subsequent scenes).");
	private GUIContent _qaSystemSpecs			= new GUIContent("Submit System Info", "Submit system information to the GameAnalytics server when the game starts.");
	private GUIContent _qaFpsAverage			= new GUIContent("Submit Average FPS", "Submit the average frames per second.");
	private GUIContent _qaFpsCritical			= new GUIContent("Submit Critical FPS", "Submit a message whenever the frames per second falls below a certain threshold. The location of the Track Target will be used for critical FPS events.");
	private GUIContent _qaFpsCriticalThreshold	= new GUIContent("FPS<", "Frames per second threshold.");
	private GUIContent _qaFpsCriticalInterval 	= new GUIContent("Critical FPS Interval","The interval in seconds for which to track critical FPS. If the FPS in this interval is below the threshold a message will be submitted.");
	private GUIContent _qaErrorHandling			= new GUIContent("Error Handling", "Defines error and exception messages to be submitted to the GameAnalytics server.");
	private GUIContent _qaMaxErrorCount			= new GUIContent("Max Error Count", "The maximum number of errors and exceptions tracked per session. It is a good idea to keep this number relatively low, so as to not submit a huge number of repeating exceptions.");
	private GUIContent _qaSubmitErrors			= new GUIContent("Submit Errors", "Submit error and exception messages to the GameAnalytics server. Useful for getting relevant data when the game crashes, etc.");
	private GUIContent _qaErrorSubmitStackTrace	= new GUIContent("Submit Stack Trace", "Automatic error and exception messages sent to the GameAnalytics server will include the error stack trace.");
	private GUIContent _qaErrorSystemSpecs		= new GUIContent("Submit System Info", "Automatic error and exception messages sent to the GameAnalytics server will include system information.");
	private GUIContent _gdSceneChange			= new GUIContent("Submit Level Transitions", "Submit special event to the GameAnalytics server whenever a new level (scene) is loaded.");
	private GUIContent _guiEnabled				= new GUIContent("GUI enabled", "Enable GUI for submitting feedback and bug reports. The location of the Track Target will be used for feedback events.");
	//private GUIContent _guiAllowScreenshot		= new GUIContent("Take Screenshot", "If enabled the player will be able to include a screenshot when submitting feedback and bug reports (This feature is not yet fully implemented).");
	
	override public void OnInspectorGUI ()
	{
		GA_SystemTracker ga = target as GA_SystemTracker;
		
		EditorGUI.indentLevel = 1;
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("GameAnalytics system tracker",EditorStyles.largeLabel);
		
		if (GUILayout.Button(_documentationLink, GUILayout.MaxWidth(60)))
		{
			Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Basic component for tracking system info, errors, fps, etc.",EditorStyles.miniLabel);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_useForSubsequentLevels, GUILayout.Width(150));
	    ga.UseForSubsequentLevels = EditorGUILayout.Toggle("", ga.UseForSubsequentLevels);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_gdSceneChange, GUILayout.Width(150));
	    ga.IncludeSceneChange = EditorGUILayout.Toggle("", ga.IncludeSceneChange);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
	
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_qaSystemSpecs, GUILayout.Width(150));
	    ga.IncludeSystemSpecs = EditorGUILayout.Toggle("", ga.IncludeSystemSpecs);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_qaFpsAverage, GUILayout.Width(150));
	    ga.SubmitFpsAverage = EditorGUILayout.Toggle("", ga.SubmitFpsAverage);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_qaFpsCritical, GUILayout.Width(150));
	    ga.SubmitFpsCritical = EditorGUILayout.Toggle("", ga.SubmitFpsCritical, GUILayout.Width(35));
		GUI.enabled = ga.SubmitFpsCritical;
	    GUILayout.Label(_qaFpsCriticalThreshold, GUILayout.Width(35));
		
		int tmpFpsCriticalThreshold = 0;
		if (int.TryParse(EditorGUILayout.TextField(ga.FpsCriticalThreshold.ToString(), GUILayout.Width(36)), out tmpFpsCriticalThreshold))
		{
			ga.FpsCriticalThreshold = Mathf.Max(Mathf.Min(tmpFpsCriticalThreshold, 99), 5);
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_qaFpsCriticalInterval, GUILayout.Width(150));
	    ga.FpsCirticalSubmitInterval = EditorGUILayout.IntField("", ga.FpsCirticalSubmitInterval, GUILayout.Width(36));
		ga.FpsCirticalSubmitInterval = Mathf.Max(1,ga.FpsCirticalSubmitInterval);
		
		GUI.enabled = true;
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
		
		ga.ErrorFoldOut = EditorGUILayout.Foldout(ga.ErrorFoldOut, _qaErrorHandling);
		GUILayout.EndHorizontal();
		
		if (ga.ErrorFoldOut)
		{
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaSubmitErrors, GUILayout.Width(150));
		    ga.SubmitErrors = EditorGUILayout.Toggle("", ga.SubmitErrors);
			GUILayout.EndHorizontal();
			
			GUI.enabled = ga.SubmitErrors;
							
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaErrorSubmitStackTrace, GUILayout.Width(150));
		    ga.SubmitErrorStackTrace = EditorGUILayout.Toggle("", ga.SubmitErrorStackTrace);
			GUILayout.EndHorizontal();
							
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaErrorSystemSpecs, GUILayout.Width(150));
		    ga.SubmitErrorSystemInfo = EditorGUILayout.Toggle("", ga.SubmitErrorSystemInfo);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label("", GUILayout.Width(27));
		    GUILayout.Label(_qaMaxErrorCount, GUILayout.Width(150));
			
			int tmpMaxErrorCount = 0;
			if (int.TryParse(EditorGUILayout.TextField(ga.MaxErrorCount.ToString(), GUILayout.Width(36)), out tmpMaxErrorCount))
			{
				ga.MaxErrorCount = Mathf.Max(Mathf.Min(tmpMaxErrorCount, 99), 1);
			}
			GUILayout.EndHorizontal();
			
			GUI.enabled = true;
		}
		
		
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_guiEnabled, GUILayout.Width(150));
	    ga.GuiEnabled = EditorGUILayout.Toggle("", ga.GuiEnabled);
		GUILayout.EndHorizontal();
		
		/*GUILayout.BeginHorizontal();
	    GUILayout.Label("", GUILayout.Width(7));
	    GUILayout.Label(_guiAllowScreenshot, GUILayout.Width(150));
	    ga.GuiAllowScreenshot = EditorGUILayout.Toggle("", ga.GuiAllowScreenshot);
		GUILayout.EndHorizontal();*/
		
		EditorGUILayout.Space();
		
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
}