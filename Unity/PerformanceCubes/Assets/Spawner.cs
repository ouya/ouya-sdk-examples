using System.Collections;
using System;
using UnityEngine;
using Random=UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	public int _NumberToSpawn = 100;

	public Vector3 _SpawnRange = new Vector3 (10, 10, 10);

	public GameObject _Prefab = null;

	private int _framesPerSecond = 0;
	private int _frameCount = 0;
	private DateTime _frameTimer = DateTime.MinValue;

	void Start ()
	{
		if (_Prefab)
		{
			for (int i = 0; i < _NumberToSpawn; ++i)
			{
				Vector3 pos;
				pos.x = Random.Range(-_SpawnRange.x, _SpawnRange.x);
				pos.y = Random.Range(-_SpawnRange.y, _SpawnRange.y);
				pos.z = Random.Range(-_SpawnRange.z, _SpawnRange.z);
				Quaternion rot = Quaternion.identity * Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
				Instantiate(_Prefab, pos, rot);
			}
		}
	}

	void OnGUI()
	{
		GUI.skin.label.fontSize = 72;
		GUI.color = Color.black;

		GUILayout.BeginVertical (GUILayout.Height (Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal (GUILayout.Width(Screen.width));
		GUILayout.FlexibleSpace();
		GUILayout.Label (string.Format ("Number of cubes: {0}", _NumberToSpawn), GUILayout.Width(1000));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal (GUILayout.Width(Screen.width));
		GUILayout.FlexibleSpace();
		GUILayout.Label (string.Format ("Frames per second: {0}", _framesPerSecond), GUILayout.Width(1000));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical ();
	}

	void Update ()
	{
		if (_frameTimer < DateTime.Now) {
			_frameTimer = DateTime.Now + TimeSpan.FromSeconds (1);
			_framesPerSecond = _frameCount;
			_frameCount = 0;
		}
		else
		{
			++_frameCount;
		}
	}
}