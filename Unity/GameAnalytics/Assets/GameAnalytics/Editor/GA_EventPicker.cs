using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_EventPicker : EditorWindow {
	
	public delegate void EventPickerDelegate(GA_EventPicker sender);
	public event EventPickerDelegate OnPicked;
	public event EventPickerDelegate OnClosed;

	public List<bool> Selected;
	public List<string> AvailableEvents;
	private bool closeOnNextUpdate = false;
	
	private string searchString = "";

	private Vector2 scrollPosition = new Vector2();
	
    void OnGUI ()
	{
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Search:", GUILayout.Width(60));
		GUI.SetNextControlName("SearchBox");
		searchString = EditorGUILayout.TextField(searchString);
		GUI.FocusControl("SearchBox");
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		for(int i=0;i<AvailableEvents.Count;i++)
		{
			string eventName = AvailableEvents[i];
			if (eventName.ToLower().Contains(searchString.ToLower()))
			{
				GUILayout.BeginHorizontal();
				Selected[i] = EditorGUILayout.Toggle(Selected[i],GUILayout.Width(20));
				EditorGUILayout.LabelField(eventName);
				GUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Select"))
		{
			if (OnPicked != null)
				OnPicked(this);
			
			Close();
		}
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
    }
	
	void OnDestroy()
	{
		if(OnClosed != null)
			OnClosed(this);
	}
	
	void Update()
	{
		if (closeOnNextUpdate)
		{
			if(OnClosed != null)
				OnClosed(this);
			
			Close();
		}
	}
	
	void OnLostFocus()
	{
        closeOnNextUpdate = true;
    }
}
