/// <summary>
/// Add to a game object or prefab to set up Game Analytic's automatic event tracking.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class GA_Tracker : MonoBehaviour
{
	public enum GAEventType { BreadCrumb, Start, OnDestroy, OnMouseDown, OnLevelWasLoaded, OnTriggerEnter, OnCollisionEnter, OnControllerColliderHit }
	
	public static Dictionary<GAEventType, string> EventTooltips = new Dictionary<GAEventType, string>()
	{
		{ GAEventType.BreadCrumb, "Send an event every time interval. Good for generating heatmaps of player position/movement in your levels." },
		{ GAEventType.Start, "Send an event when the Start method is run. Use this for tracking spawning of the object" },
		{ GAEventType.OnDestroy, "Send an event when the OnDestroy method is run. Use this for tracking \"death\" of the object." },
		{ GAEventType.OnMouseDown, "Send an event when the OnMouseDown method is run. Use this for tracking when the player performs a click/touch on the object." },
		{ GAEventType.OnLevelWasLoaded, "Send an event when the OnLevelWasLoaded method is run. Use this for tracking when a new level is loaded." },
		{ GAEventType.OnTriggerEnter, "Send an event when the OnTriggerEnter method is run. Use this for tracking when something (f.x. the player) enters a trigger area." },
		{ GAEventType.OnCollisionEnter, "Send an event when the OnCollisionEnter method is run. Use this for tracking when objects collide." },
		{ GAEventType.OnControllerColliderHit, "Send an event when the OnControllerColliderHit method is run. Use this for tracking when a controller hits a collider while performing a Move." }
	};
	
	[SerializeField]
	public List<GAEventType> TrackedEvents = new List<GAEventType>();
	
	public bool TrackedEventsFoldOut = false;
	
	public bool TrackTarget = false;
	
	public bool ShowGizmo = true;
	
	public float BreadCrumbTrackInterval = 1.0f;
	
	private static bool _trackTargetAlreadySet;
	
	private float _lastBreadCrumbTrackTime;
	
	/*void Awake()
	{
		if (_trackCounter == null)
		{
			_trackCounter = new Dictionary<string, int>[Enum.GetValues(typeof(GAEventType)).Length];
			for (int i = 0; i < _trackCounter.Length; i++)
			{
				_trackCounter[i] = new Dictionary<string, int>();
			}
		}
	}*/
	
#if UNITY_EDITOR
	void OnEnable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += GA.HierarchyWindowCallback;
	}
	
	void OnDisable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI -= GA.HierarchyWindowCallback;
	}
#endif
	
	void Start()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.Start))
		{
			GA.API.Design.NewEvent("Start:"+gameObject.name, transform.position);
		}
		
		if (TrackTarget)
		{
			GA.SettingsGA.TrackTarget = transform;
			if (_trackTargetAlreadySet)
			{
				GA.LogWarning("You should only set the Track Target of GA_Tracker once per scene");
			}
			_trackTargetAlreadySet = true;
		}
	}
	
	void Update()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.BreadCrumb))
		{
			if (Time.time > _lastBreadCrumbTrackTime + BreadCrumbTrackInterval)
			{
				_lastBreadCrumbTrackTime = Time.time;
				GA.API.Design.NewEvent("BreadCrumb:"+gameObject.name, transform.position);
			}
		}
	}
	
	void OnDestroy()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.OnDestroy))
		{
			GA.API.Design.NewEvent("OnDestroy:"+gameObject.name, transform.position);
		}
	}
	
	void OnMouseDown()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.OnMouseDown))
		{
			GA.API.Design.NewEvent("OnMouseDown:"+gameObject.name, transform.position);
		}
	}
	
	public void OnLevelWasLoaded ()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.OnLevelWasLoaded))
		{
			GA.API.Design.NewEvent("OnLevelWasLoaded:"+gameObject.name, transform.position);
		}
	}
	
	public void OnTriggerEnter ()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.OnTriggerEnter))
		{
			GA.API.Design.NewEvent("OnTriggerEnter:"+gameObject.name, transform.position);
		}
	}
	
	public void OnCollisionEnter ()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.OnCollisionEnter))
		{
			GA.API.Design.NewEvent("OnCollisionEnter:"+gameObject.name, transform.position);
		}
	}
	
	public void OnControllerColliderHit ()
	{
		if (!Application.isPlaying)
			return;
		
		if (TrackedEvents.Contains(GAEventType.OnControllerColliderHit))
		{
			GA.API.Design.NewEvent("OnControllerColliderHit:"+gameObject.name, transform.position);
		}
	}
	
	public System.Array GetEventValues()
	{
		return Enum.GetValues(typeof(GAEventType));
	}
	
	void OnDrawGizmos()
	{
		if (ShowGizmo)
			Gizmos.DrawIcon(transform.position, "gaLogo", true);
	}
}