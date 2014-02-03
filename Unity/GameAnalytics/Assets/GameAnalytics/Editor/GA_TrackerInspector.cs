/// <summary>
/// The inspector for the GA prefab.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;

[CustomEditor(typeof(GA_Tracker))]
public class GA_TrackerInspector : Editor
{
	private GUIContent _eventFoldOut = new GUIContent("Auto-tracked Events", "The events you can track automatically.");
	private GUIContent _trackTarget = new GUIContent("Track Target", "Use this objects location when tracking default events, such as critical FPS events. Only one object per scene can be set to track target.");
	private GUIContent _showGizmo = new GUIContent("Editor Scene Gizmo", "Show a GA gizmo in the scene view for this GA_Tracker.");
	
	override public void OnInspectorGUI ()
	{
		GA_Tracker gaTracker = target as GA_Tracker;
		
		bool showGizmo = gaTracker.ShowGizmo;
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(7));
		GUILayout.Label(_showGizmo, GUILayout.Width(150));
		gaTracker.ShowGizmo = EditorGUILayout.Toggle("", gaTracker.ShowGizmo);
		GUILayout.EndHorizontal();
		
		if (gaTracker.ShowGizmo != showGizmo)
		{
			EditorApplication.RepaintHierarchyWindow();
			SceneView.RepaintAll();
		}
		
		bool trackTargetValue = gaTracker.TrackTarget;
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(7));
		GUILayout.Label(_trackTarget, GUILayout.Width(150));
		trackTargetValue = EditorGUILayout.Toggle("", gaTracker.TrackTarget);
		GUILayout.EndHorizontal();
		
		if (trackTargetValue && !gaTracker.TrackTarget)
		{
			GA_Tracker[] gaTrackers = FindObjectsOfType(typeof(GA_Tracker)) as GA_Tracker[];
			foreach (GA_Tracker gaT in gaTrackers)
			{
				gaT.TrackTarget = false;
			}
		}
		gaTracker.TrackTarget = trackTargetValue;
		
		gaTracker.TrackedEventsFoldOut = EditorGUILayout.Foldout(gaTracker.TrackedEventsFoldOut, _eventFoldOut);
		
		if (gaTracker.TrackedEventsFoldOut)
		{
			System.Array eventValues = gaTracker.GetEventValues();
			
			for (int i = 0; i < eventValues.Length; i++)
			{
				GA_Tracker.GAEventType eventType = (GA_Tracker.GAEventType)eventValues.GetValue(i);
				
				bool eventSelected = gaTracker.TrackedEvents.Contains(eventType);
				
				GUILayout.BeginHorizontal();
			    GUILayout.Label("", GUILayout.Width(27));
			    GUILayout.Label(new GUIContent(eventType.ToString(), GA_Tracker.EventTooltips[eventType]), GUILayout.Width(150));
				eventSelected = EditorGUILayout.Toggle("", eventSelected, GUILayout.Width(27));
				
				if (i == 0)
				{
					gaTracker.BreadCrumbTrackInterval = EditorGUILayout.FloatField(gaTracker.BreadCrumbTrackInterval, GUILayout.Width(27));
					gaTracker.BreadCrumbTrackInterval = Mathf.Max(1.0f, gaTracker.BreadCrumbTrackInterval);
				}
				
				GUILayout.EndHorizontal();
				
				if (eventSelected && !gaTracker.TrackedEvents.Contains(eventType))
					gaTracker.TrackedEvents.Add(eventType);
				
				if (!eventSelected && gaTracker.TrackedEvents.Contains(eventType))
					gaTracker.TrackedEvents.Remove(eventType);
			}
		}
	}
}