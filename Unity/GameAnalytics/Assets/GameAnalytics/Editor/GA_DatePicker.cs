using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_DatePicker : EditorWindow {
	
	public delegate void DatePickerDelegate(GA_DatePicker sender);
	public event DatePickerDelegate OnPicked;
	public event DatePickerDelegate OnClosed;
	private List<string> Days;
    public System.DateTime CurrentDate;
	
	private bool closeOnNextUpdate = false;
	
    void OnGUI ()
	{
		position = new Rect(position.x, position.y, 250, 200);
		
		GUILayout.BeginArea( new Rect( 0, 7, 250, 200 ) );
		
		//Year
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("<", GUILayout.Width(65)))
			CurrentDate = CurrentDate.AddYears(-1);
		
		GUILayout.FlexibleSpace();
		GUILayout.Label(CurrentDate.ToString("yyyy"), EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button(">", GUILayout.Width(65)))
			CurrentDate = CurrentDate.AddYears(1);
		GUILayout.EndHorizontal();
		
		//Month
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("<", GUILayout.Width(65)))
			CurrentDate = CurrentDate.AddMonths(-1);
		
		GUILayout.FlexibleSpace();
		GUILayout.Label(CurrentDate.ToString("MMMM"), EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button(">", GUILayout.Width(65)))
			CurrentDate = CurrentDate.AddMonths(1);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		//Day
		int numDays = System.DateTime.DaysInMonth(CurrentDate.Year,CurrentDate.Month);
		Days = new List<string>();
		for(int i=1;i<numDays+1;i++)
		{
			Days.Add(i.ToString());
		}
       	int newDay = GUILayout.SelectionGrid(CurrentDate.Day-1, Days.ToArray(), 7)+1;
		int diff = newDay-CurrentDate.Day;

		CurrentDate = CurrentDate.AddDays(diff);
		
		if (numDays <= 28)
		{
			GUILayout.Space(21);
		}
		
		EditorGUILayout.Space();
		
		if (GUILayout.Button("Select"))
		{
			if (OnPicked != null)
				OnPicked(this);
		}
		
		GUILayout.EndArea();
    }
	
	void OnDestroy()
	{
		if (OnClosed != null)
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
