using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_HeatmapCombineTypePicker : EditorWindow
{
	public delegate void CombinePickerDelegate(GA_HeatmapCombineTypePicker sender);
	public event CombinePickerDelegate OnPicked;
	
	public GA_HeatMapDataFilter.CombineHeatmapType CombineType = GA_HeatMapDataFilter.CombineHeatmapType.None;
	
	private bool closeOnNextUpdate = false;
	
    void OnGUI ()
	{
		position = new Rect(position.x, position.y, 300, 145);
		
		GUILayout.Label("What would you like to do with the new heatmap data?");
		
		EditorGUILayout.Space();
		
		if (GUILayout.Button("Replace old data"))
		{
			CombineType = GA_HeatMapDataFilter.CombineHeatmapType.None;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		if (GUILayout.Button("Add to old data"))
		{
			CombineType = GA_HeatMapDataFilter.CombineHeatmapType.Add;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		if (GUILayout.Button("Subtract from old data"))
		{
			CombineType = GA_HeatMapDataFilter.CombineHeatmapType.Subtract;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		if (GUILayout.Button("Subtract from old data (no negative)"))
		{
			CombineType = GA_HeatMapDataFilter.CombineHeatmapType.SubtractZero;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		if (GUILayout.Button("Cancel download"))
		{
			Close();
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
