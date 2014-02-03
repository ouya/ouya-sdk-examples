using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_Status : EditorWindow
{
	private GUIContent _resetButton 			= new GUIContent("Reset Values", "Resets all the \"Messages Sent\" values.");
	private GUIContent _selectSettingsButton 	= new GUIContent("Select GA_Settings", "Select the GA_Settings object.");
	
	void OnGUI ()
	{
		GUILayout.Label("GameAnalytics Status", EditorStyles.whiteLargeLabel);
		
		GUILayout.Space(5);
		
		GUILayout.Label("Setup Status", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Game Key inserted:", GUILayout.Width(145));
		GUI.enabled = false;
		GUILayout.Toggle(GA.SettingsGA.GameKey != "", "");
		GUI.enabled = true;
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Secret Key inserted:", GUILayout.Width(145));
		GUI.enabled = false;
		GUILayout.Toggle(GA.SettingsGA.SecretKey != "", "");
		GUI.enabled = true;
		GUILayout.EndHorizontal();
		
		if (GUILayout.Button(_selectSettingsButton, GUILayout.Width(163)))
		{
			Selection.activeObject = GA.SettingsGA;
		}
		
		GUILayout.Space(5);
		
		GUILayout.Label("Events Sent", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Total Succeeded:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.TotalMessagesSubmitted.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Total Failed:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.TotalMessagesFailed.ToString());
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Design Succeeded:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.DesignMessagesSubmitted.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Design Failed:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.DesignMessagesFailed.ToString());
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Quality Succeeded:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.QualityMessagesSubmitted.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Quality Failed:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.QualityMessagesFailed.ToString());
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Error Succeeded:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.ErrorMessagesSubmitted.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Error Failed:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.ErrorMessagesFailed.ToString());
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Business Succeeded:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.BusinessMessagesSubmitted.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("Business Failed:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.BusinessMessagesFailed.ToString());
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("User Succeeded:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.UserMessagesSubmitted.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
	    GUILayout.Label("User Failed:", GUILayout.Width(145));
		GUILayout.Label(GA.SettingsGA.UserMessagesFailed.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.Space(3);
		
		if (GUILayout.Button(_resetButton, GUILayout.Width(163)))
		{
			GA.SettingsGA.TotalMessagesSubmitted = 0;
			GA.SettingsGA.TotalMessagesFailed = 0;
			
			GA.SettingsGA.DesignMessagesSubmitted = 0;
			GA.SettingsGA.DesignMessagesFailed = 0;
			GA.SettingsGA.QualityMessagesSubmitted = 0;
			GA.SettingsGA.QualityMessagesFailed = 0;
			GA.SettingsGA.ErrorMessagesSubmitted = 0;
			GA.SettingsGA.ErrorMessagesFailed = 0;
			GA.SettingsGA.BusinessMessagesSubmitted = 0;
			GA.SettingsGA.BusinessMessagesFailed = 0;
			GA.SettingsGA.UserMessagesSubmitted = 0;
			GA.SettingsGA.UserMessagesFailed = 0;
			
			EditorUtility.SetDirty(GA.SettingsGA);
		}
    }
	
	void OnInspectorUpdate()
	{
		EditorUtility.SetDirty(GA.SettingsGA);
		Repaint();
	}
}
