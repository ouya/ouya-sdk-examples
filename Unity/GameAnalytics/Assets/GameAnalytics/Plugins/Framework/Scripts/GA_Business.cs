/// <summary>
/// This class handles business events, such as ingame purchases.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public class GA_Business 
{
	#region public methods
	
	public  void NewEvent(string eventName, string currency, int amount, Vector3 trackPosition)
	{
		CreateNewEvent(eventName, currency, amount, trackPosition.x, trackPosition.y, trackPosition.z);
	}
	
	public  void NewEvent(string eventName, string currency, int amount, float x, float y, float z)
	{
		CreateNewEvent(eventName, currency, amount, x, y, z);
	}
	
	public  void NewEvent(string eventName, string currency, int amount)
	{
		CreateNewEvent(eventName, currency, amount, null, null, null);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Used for player purchases
	/// </summary>
	/// <param name="businessID">
	/// The business identifier. F.x. "Rocket Launcher Upgrade" <see cref="System.String"/>
	/// </param>
	/// <param name="currency">
	/// Abbreviation of the currency used for the transaction. F.x. USD (U.S. Dollars) <see cref="System.String"/>
	/// </param>
	/// <param name="amount">
	/// The value of the transaction in the lowest currency unit. F.x. if currency is USD then amount should be in cent <see cref="System.Int32"/>
	/// </param>
	private  void CreateNewEvent(string eventName, string currency, int amount, float? x, float? y, float? z)
	{
		Hashtable parameters = new Hashtable()
		{
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], eventName },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Currency], currency },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Amount], amount.ToString() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], GA.SettingsGA.CustomArea.Equals(string.Empty)?Application.loadedLevelName:GA.SettingsGA.CustomArea }
		};
		
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
		
		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_Purchase, false);
	}
	
	#endregion
}