// This class handles game design events, such as kills, deaths, high scores, etc.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public class GA_Design
{
	#region public methods
	
	/// <summary>
	/// Creates a new event
	/// </summary>
	/// <param name='eventName'>
	/// A event string you define
	/// </param>
	/// <param name='eventValue'>
	/// A value of the event, can be null
	/// </param>
	/// <param name='trackPosition'>
	/// Position that the event is tied to.
	/// </param>S
	public void NewEvent(string eventName, float? eventValue, Vector3 trackPosition)
	{
		CreateNewEvent(eventName, eventValue, trackPosition.x, trackPosition.y, trackPosition.z);
	}
	
	/// <summary>
	/// Creates a new event...
	/// </summary>
	/// <param name='eventName'>
	/// A event string you define
	/// </param>
	/// <param name='trackPosition'>
	/// Position that the event is tied to.
	/// </param>S
	public void NewEvent(string eventName, Vector3 trackPosition)
	{
		CreateNewEvent(eventName, null, trackPosition.x, trackPosition.y, trackPosition.z);
	}
	
	public void NewEvent(string eventName, float? eventValue, float x, float y, float z)
	{
		CreateNewEvent(eventName, eventValue, x, y, z);
	}
	
	public void NewEvent(string eventName, float x, float y, float z)
	{
		CreateNewEvent(eventName, null, x, y, z);
	}
	
	public void NewEvent(string eventName, float eventValue)
	{
		CreateNewEvent(eventName, eventValue, null, null, null);
	}
	
	public void NewEvent(string eventName)
	{
		CreateNewEvent(eventName, null, null, null, null);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Adds a custom event to the submit queue (see GA_Queue)
	/// </summary>
	/// <param name="eventName">
	/// Identifies the event so this should be as descriptive as possible. PickedUpAmmo might be a good event name. EventTwo is a bad event name! <see cref="System.String"/>
	/// </param>
	/// <param name="eventValue">
	/// A value relevant to the event. F.x. if the player picks up some shotgun ammo the eventName could be "PickedUpAmmo" and this value could be "Shotgun". This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	/// <param name="x">
	/// The x coordinate of the event occurence. This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	/// <param name="y">
	/// The y coordinate of the event occurence. This can be null <see cref="System.Nullable<System.Single>"/>
	/// </param>
	private void CreateNewEvent(string eventName, float? eventValue, float? x, float? y, float? z)
	{
		Hashtable parameters = new Hashtable()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], GA.SettingsGA.CustomArea.Equals(string.Empty)?Application.loadedLevelName:GA.SettingsGA.CustomArea }
		};
		
		if (eventValue.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Value], eventValue.ToString());
		}
		
		if (x.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.X], (x*GA.SettingsGA.HeatmapGridSize.x).ToString());
		}
		
		if (y.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Y], (y*GA.SettingsGA.HeatmapGridSize.y).ToString());
		}
		
		if (z.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Z], (z*GA.SettingsGA.HeatmapGridSize.z).ToString());
		}

		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Event, false);
	}
	
	#endregion
}