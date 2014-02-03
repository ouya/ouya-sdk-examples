//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

/// <summary>
/// This script adds the NGUI menu options to the Unity Editor.
/// </summary>

static public class NGUIMenu
{
	/// <summary>
	/// Same as SelectedRoot(), but with a log message if nothing was found.
	/// </summary>

	static public GameObject SelectedRoot ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot();

		if (go == null)
		{
			Debug.Log("No UI found. You can create a new one easily by using the UI creation wizard.\nOpening it for your convenience.");
			CreateUIWizard();
		}
		return go;
	}

	[MenuItem("NGUI/Attach a Collider #&c")]
	static public void AddCollider ()
	{
		GameObject go = Selection.activeGameObject;

		if (NGUIEditorTools.WillLosePrefab(go))
		{
			if (go != null)
			{
				NGUIEditorTools.RegisterUndo("Add Widget Collider", go);
				NGUITools.AddWidgetCollider(go);
			}
			else
			{
				Debug.Log("You must select a game object first, such as your button.");
			}
		}
	}

	static Vector3 Round (Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	static void MakePixelPerfect (Transform t)
	{
		UIWidget w = t.GetComponent<UIWidget>();

		if (w != null)
		{
			w.MakePixelPerfect();
		}
		else
		{
			t.localPosition = Round(t.localPosition);
			t.localScale = Round(t.localScale);

			for (int i = 0, imax = t.childCount; i < imax; ++i)
			{
				MakePixelPerfect(t.GetChild(i));
			}
		}
	}

	[MenuItem("NGUI/Make Pixel Perfect #&p")]
	static void PixelPerfectSelection ()
	{
		if (Selection.activeTransform == null)
		{
			Debug.Log("You must select an object in the scene hierarchy first");
			return;
		}
		foreach (Transform t in Selection.transforms) MakePixelPerfect(t);
	}

	[MenuItem("NGUI/Create a Panel")]
	static public void AddPanel ()
	{
		GameObject go = SelectedRoot();

		if (NGUIEditorTools.WillLosePrefab(go))
		{
			NGUIEditorTools.RegisterUndo("Add a child UI Panel", go);

			GameObject child = new GameObject(NGUITools.GetName<UIPanel>());
			child.layer = go.layer;

			Transform ct = child.transform;
			ct.parent = go.transform;
			ct.localPosition = Vector3.zero;
			ct.localRotation = Quaternion.identity;
			ct.localScale = Vector3.one;

			child.AddComponent<UIPanel>();
			Selection.activeGameObject = child;
		}
	}

	[MenuItem("NGUI/Create a Widget")]
	static public void CreateWidgetWizard ()
	{
		EditorWindow.GetWindow<UICreateWidgetWizard>(false, "Widget Tool", true);
	}

	[MenuItem("NGUI/Create a New UI")]
	static public void CreateUIWizard ()
	{
		EditorWindow.GetWindow<UICreateNewUIWizard>(false, "UI Tool", true);
	}

	[MenuItem("NGUI/Panel Tool")]
	static public void OpenPanelWizard ()
	{
		EditorWindow.GetWindow<UIPanelTool>(false, "Panel Tool", true);
	}

	[MenuItem("NGUI/Camera Tool")]
	static public void OpenCameraWizard ()
	{
		EditorWindow.GetWindow<UICameraTool>(false, "Camera Tool", true);
	}

	[MenuItem("NGUI/Font Maker #&f")]
	static public void OpenFontMaker ()
	{
		EditorWindow.GetWindow<UIFontMaker>(false, "Font Maker", true);
	}

	[MenuItem("NGUI/Atlas Maker #&m")]
	static public void OpenAtlasMaker ()
	{
		EditorWindow.GetWindow<UIAtlasMaker>(false, "Atlas Maker", true);
	}
}