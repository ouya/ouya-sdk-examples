/// <summary>
/// The inspector for GA_HeatMapDataFilter.
/// </summary>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(GA_HeatMapDataFilter))]
public class GA_HeatMapInspector : Editor
{
	override public void OnInspectorGUI ()
	{
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		
		if(heatmap==null)
			return;
		
		if (!EditorUtility.IsPersistent (target ))
		{
			PrefabUtility.DisconnectPrefabInstance(heatmap.gameObject);
		}
		
		EditorGUI.indentLevel = 1;
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(" ", GUILayout.Width(155)); //layout hack to align.
		if(GUILayout.Button("Update index", GUILayout.Height(30)))
		{
			heatmap.UpdateIndexData();
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Current index:");
		EditorGUILayout.LabelField((heatmap.AvailableBuilds!=null&&heatmap.AvailableAreas!=null&&heatmap.AvailableEvents!=null?/*heatmap.AvailableBuilds.Count+" build(s). "+*/heatmap.AvailableAreas.Count+" area(s). "+heatmap.AvailableEvents.Count+" event(s).":"No index data loaded"));
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		//Version
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Select for all builds:");
		heatmap.IgnoreBuilds = EditorGUILayout.Toggle(heatmap.IgnoreBuilds);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Build:");
		
		if (heatmap.IgnoreBuilds)
			EditorGUILayout.Popup(0, new string[] { "All" });
		else
			heatmap.CurrentBuildIndex = EditorGUILayout.Popup(heatmap.CurrentBuildIndex, heatmap.AvailableBuilds.ToArray());

		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		//Area
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Area:");
		
		heatmap.CurrentAreaIndex = EditorGUILayout.Popup(heatmap.CurrentAreaIndex, heatmap.AvailableAreas.ToArray());

		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Events:", GUILayout.Width(155));
		
			
		if(heatmap.AvailableEvents.Count>0)
		{
			//Setting some easy to understand label values for the select button
			int selectedEventsCount = heatmap.CurrentEventFlag.Count(b=>b);
			string btnEventsLabel = "Mixed ("+selectedEventsCount+")";
			if(selectedEventsCount==1)
				btnEventsLabel = heatmap.AvailableEvents[heatmap.CurrentEventFlag.FindIndex(b=>b)];
			if(selectedEventsCount==0)
				btnEventsLabel = "None";
			
			if(GUILayout.Button(btnEventsLabel,EditorStyles.popup))
			{
				GA_EventPicker events = ScriptableObject.CreateInstance<GA_EventPicker>();
				events.AvailableEvents = heatmap.AvailableEvents;
				events.Selected = heatmap.CurrentEventFlag;
				events.ShowUtility();
				events.OnPicked += HandleEventsOnPicked;
				events.OnClosed += HandleEventsOnPicked;
			}
		}
		else
		{
			// no events to select from. No need to make the button
			EditorGUILayout.LabelField("No events");
		}
		
		GUILayout.EndHorizontal();
		
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Select for all dates:");
		heatmap.IgnoreDates = EditorGUILayout.Toggle(heatmap.IgnoreDates);
		GUILayout.EndHorizontal();
		
#if UNITY_EDITOR
		//Start Date
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Start Date:", GUILayout.Width(155));
		string startDateDisplay = heatmap.IgnoreDates?"All":heatmap.StartDateTime.Value.ToString("yyyy-MM-dd");
		
		if(GUILayout.Button(startDateDisplay,EditorStyles.popup))
		{
			GA_DatePicker date = ScriptableObject.CreateInstance<GA_DatePicker>();
			date.CurrentDate = heatmap.StartDateTime.Value;
			date.ShowUtility();
			date.OnPicked += HandleStartDateOnPicked;
			date.OnClosed += HandleStartDateOnClosed;
		}

		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		//End Date
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("End Date:", GUILayout.Width(155));
		string endDateDisplay = heatmap.IgnoreDates?"All":heatmap.EndDateTime.Value.ToString("yyyy-MM-dd");

		if(GUILayout.Button(endDateDisplay,EditorStyles.popup))
		{
			GA_DatePicker date = ScriptableObject.CreateInstance<GA_DatePicker>();
			date.CurrentDate = heatmap.EndDateTime.Value;
			date.ShowUtility();
			date.OnPicked += HandleEndDateOnPicked;
			date.OnClosed += HandleEndDateOnClosed;
		}

		GUILayout.EndHorizontal();
#endif
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField(" ", GUILayout.Width(155));
		if(heatmap.Loading)
		{
			//GA_TODO: We never know how much data we are downloading right now. With a better index or a GetCount method we could estimate the download time.
			/* //Progress bar doesn't really contain any info when we don't know how much data we are downloading
			 * GUILayout.Box("",GUILayout.Height(50),GUILayout.Width(200));.
			 * if(Event.current.type == EventType.Repaint)
			{
				var lastrect = GUILayoutUtility.GetLastRect();
				EditorGUI.ProgressBar(lastrect,heatmap.LoadProgress,"Downloading");
			}*/
			EditorGUILayout.HelpBox("Downloading data. Please wait.",MessageType.Info);
		}
		else
		{
			if (heatmap.DownloadingData)
			{
				EditorGUILayout.HelpBox("Downloading data. Please wait.",MessageType.Info);
			}
			else if (heatmap.BuildingHeatmap)
			{
				EditorGUILayout.HelpBox("Building heatmap. " + heatmap.BuildHeatmapPercentage + "%",MessageType.Info);
			}
			else if(GUILayout.Button("Download heatmap",GUILayout.Height (30)))
			{
				if (heatmap.DataContainer == null)
				{
					GA.Log("Downloading heatmap data. This might take some time..");
					heatmap.DownloadUpdate();
				}
				else
				{
					GA_HeatmapCombineTypePicker combine = ScriptableObject.CreateInstance<GA_HeatmapCombineTypePicker>();
					combine.ShowUtility();
					Vector2 pos = EditorGUIUtility.GUIToScreenPoint(new Vector2(GUILayoutUtility.GetLastRect().x + 150, Event.current.mousePosition.y - 25));
					combine.position = new Rect(pos.x, pos.y, 300, 145);
					combine.OnPicked += HandleCombineOnPicked;
				}
			}
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Current dataset:");
		EditorGUILayout.LabelField((heatmap.GetData()!=null?+heatmap.GetData().Count+" samples":"No data loaded"));
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Redownload in build:");
		heatmap.RedownloadDataOnPlay = EditorGUILayout.Toggle(heatmap.RedownloadDataOnPlay);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
	}

	void HandleCombineOnPicked (GA_HeatmapCombineTypePicker sender)
	{
		sender.OnPicked -= HandleCombineOnPicked;
		
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		heatmap.SetCombineHeatmapType(sender.CombineType);
		GA.Log("Downloading heatmap data. This might take some time..");
		heatmap.DownloadUpdate();
		
		Repaint();
	}
	
	void HandleEventsOnPicked (GA_EventPicker sender)
	{
		sender.OnClosed -= HandleEventsOnPicked;
		sender.OnPicked -= HandleEventsOnPicked;
		
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		heatmap.CurrentEventFlag = sender.Selected;
		Repaint();
	}
	
	void HandleEndDateOnPicked (GA_DatePicker sender)
	{
		sender.OnPicked -= HandleStartDateOnPicked;
		sender.OnClosed -= HandleStartDateOnClosed;
		
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		heatmap.EndDateTime = sender.CurrentDate;
		
		sender.Close();
		Repaint();
	}
	
	void HandleEndDateOnClosed (GA_DatePicker sender)
	{
		sender.OnPicked -= HandleStartDateOnPicked;
		sender.OnClosed -= HandleStartDateOnClosed;
		
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		heatmap.EndDateTime = sender.CurrentDate;
		//already closed
		Repaint();
	}
	
	void HandleStartDateOnPicked (GA_DatePicker sender)
	{
		sender.OnPicked -= HandleStartDateOnPicked;
		sender.OnClosed -= HandleStartDateOnClosed;
		
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		heatmap.StartDateTime = sender.CurrentDate;
		sender.Close();
		Repaint();
	}
	void HandleStartDateOnClosed (GA_DatePicker sender)
	{
		sender.OnPicked -= HandleStartDateOnPicked;
		sender.OnClosed -= HandleStartDateOnClosed;
		
		GA_HeatMapDataFilter heatmap = target as GA_HeatMapDataFilter;
		heatmap.StartDateTime = sender.CurrentDate;
		//already closed
		Repaint();
	}

}