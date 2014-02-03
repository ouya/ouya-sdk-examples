using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_HeatmapColorPresetPicker : EditorWindow
{
	public delegate void ColorPresetPickerDelegate(GA_HeatmapColorPresetPicker sender);
	public event ColorPresetPickerDelegate OnPicked;
	
	public GA_HeatMapRendererInspector.PresetColorTypes ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.None;
	
	private bool closeOnNextUpdate = false;
	
	private Vector2 scrollPosition = new Vector2();
	
    void OnGUI ()
	{
		position = new Rect(position.x, position.y, 180, 145);
		
		GUILayout.Label("Color preset for heatmap data:");
		
		EditorGUILayout.Space();
		
		Vector2 lastvect = new Vector2(GUILayoutUtility.GetLastRect().x, GUILayoutUtility.GetLastRect().y);
		
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;
		
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		
		if (GUILayout.Button("Yellow - Red"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.YellowRed;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("Blue - Red"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.BlueRed;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("Light Blue - Dark Blue"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.LightBlueDarkBlue;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("Magenta - Black"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.MagentaBlack;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("Cyan - Black"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.CyanBlack;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("Green - Black"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.GreenBlack;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("White - Black"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.WhiteBlack;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		if (GUILayout.Button("Salmon - Black"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.SalmonBlack;
			if (OnPicked != null)
				OnPicked(this);
			Close();
		}
		
		int add = 0;
		
		DrawColorBoxes(lastvect + new Vector2(135, -17 + add), lastvect + new Vector2(145, -17 + add), Color.yellow, Color.red, 10, 10, scrollPosition.y, 1);
		DrawColorBoxes(lastvect + new Vector2(135, 4 + add), lastvect + new Vector2(145, 4 + add), Color.blue, Color.red, 10, 10, scrollPosition.y, 2);
		DrawColorBoxes(lastvect + new Vector2(135, 25 + add), lastvect + new Vector2(145, 25 + add), new Color(0.75f, 0.75f, 1, 1), new Color(0, 0, 0.5f, 1), 10, 10, scrollPosition.y, 3);
		DrawColorBoxes(lastvect + new Vector2(135, 46 + add), lastvect + new Vector2(145, 46 + add), Color.magenta, Color.black, 10, 10, scrollPosition.y, 4);
		DrawColorBoxes(lastvect + new Vector2(135, 67 + add), lastvect + new Vector2(145, 67 + add), Color.cyan, Color.black, 10, 10, scrollPosition.y, 5);
		DrawColorBoxes(lastvect + new Vector2(135, 88 + add), lastvect + new Vector2(145, 88 + add), Color.green, Color.black, 10, 10, scrollPosition.y, 6);
		DrawColorBoxes(lastvect + new Vector2(135, 109 + add), lastvect + new Vector2(145, 109 + add), Color.white, Color.black, 10, 10, scrollPosition.y, 7);
		DrawColorBoxes(lastvect + new Vector2(135, 130 + add), lastvect + new Vector2(145, 130 + add), new Color(1, 0.5f, 0.5f, 1), Color.black, 10, 10, scrollPosition.y, 8);
		
		EditorGUILayout.EndScrollView();
		
		GUI.skin.button.alignment = TextAnchor.MiddleCenter;
		
		if (GUILayout.Button("Cancel"))
		{
			ColorPreset = GA_HeatMapRendererInspector.PresetColorTypes.None;
			Close();
		}
    }
	
	void DrawColorBoxes (Vector2 vect1, Vector2 vect2, Color col1, Color col2, int width, int height, float scrollPos, int number)
	{
		Debug.Log(scrollPos);
		if (scrollPos + 12 < 21 * number && scrollPos + 92 > 21 * number)
		{
			for (int i = 0; i < height; i++)
			{
				GA_GUIHelper.DrawLine(vect1 + new Vector2(0, i), vect1 + new Vector2(width, i), col1);
			}
			
			for (int i = 0; i < height; i++)
			{
				GA_GUIHelper.DrawLine(vect2 + new Vector2(0, i), vect2 + new Vector2(width, i), col2);
			}
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
