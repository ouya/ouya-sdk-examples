#if UNITY_EDITOR || !UNITY_FLASH

/// <summary>
/// This class handles receiving data from the Game Analytics servers.
/// JSON data is sent using a MD5 hashed authorization header, containing the JSON data and private key. Data received must be in JSON format also.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
//using LitJson;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public class GA_Request
{
	/// <summary>
	/// Handlers for success and fail regarding requests sent to the GA server
	/// </summary>
	public delegate void SubmitSuccessHandler(RequestType requestType, Hashtable returnParam, SubmitErrorHandler errorEvent);
	public delegate void SubmitErrorHandler(string message);
	
	/// <summary>
	/// Types of requests that can be made to the GA server
	/// </summary>
	public enum RequestType { GA_GetHeatmapGameInfo, GA_GetHeatmapData }
	
	/// <summary>
	/// All the different types of requests
	/// </summary>
	public Dictionary<RequestType, string> Requests = new Dictionary<RequestType, string>()
	{
		{ RequestType.GA_GetHeatmapGameInfo, "game" },
		{ RequestType.GA_GetHeatmapData, "heatmap" }
	};
	
	#region private values
	
	private string _baseURL = "http://data-api.gameanalytics.com";
	
	#endregion
	
	#region public methods
	
	public WWW RequestGameInfo(SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{ 
		string game_key = GA.SettingsGA.GameKey;
		
		string requestInfo = "game_key=" + game_key + "&keys=area%7Cevent_id%7Cbuild";
		
		requestInfo = requestInfo.Replace(" ", "%20");
		
		//Get the url with the request type
		string url = GetURL(Requests[RequestType.GA_GetHeatmapGameInfo]);
		
		url += "/?" + requestInfo;
		
		WWW www = null;
		
		#if !UNITY_WP8 && !UNITY_METRO
		
		//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
		Hashtable headers = new Hashtable();
		headers.Add("Authorization", GA.API.Submit.CreateMD5Hash(requestInfo + GA.SettingsGA.ApiKey));
		
		//Try to send the data
		www = new WWW(url, new byte[] { 0 }, headers);
		
		#else
		
		//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", GA.API.Submit.CreateMD5Hash(requestInfo + GA.SettingsGA.ApiKey));
		
		//Try to send the data
		www = new WWW(url, new byte[] { 0 }, headers);
		
		#endif
		
		GA.RunCoroutine(Request(www, RequestType.GA_GetHeatmapGameInfo, successEvent, errorEvent),()=>www.isDone);
		
		return www;
	}
	
	public WWW RequestHeatmapData(List<string> events, string area, string build, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		return RequestHeatmapData(events, area, build, null, null, successEvent, errorEvent);
	}
	
	public WWW RequestHeatmapData(List<string> events, string area, string build, DateTime? startDate, DateTime? endDate, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		string game_key = GA.SettingsGA.GameKey;
		string event_ids = "";
		
		for (int i = 0; i < events.Count; i++)
		{
			if (i == events.Count - 1)
				event_ids += events[i];
			else
				event_ids += events[i] + "|";
		}
		
		string requestInfo = "game_key=" + game_key + "&event_ids=" + event_ids + "&area=" + area;
		
		if (!build.Equals(""))
			requestInfo += "&build=" + build;
		
		requestInfo = requestInfo.Replace(" ", "%20");

		if (startDate.HasValue && endDate.HasValue)
		{
			DateTime startDT = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
			DateTime endDT = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 0, 0, 0);
			
			requestInfo += "&start_ts=" + DateTimeToUnixTimestamp(startDT) + "&end_ts=" + DateTimeToUnixTimestamp(endDT);
		}
		
		//Get the url with the request type
		string url = GetURL(Requests[RequestType.GA_GetHeatmapData]);
		
		url += "/?" + requestInfo;
		
		WWW www = null;
		
		#if !UNITY_WP8 && !UNITY_METRO
		
		//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
		Hashtable headers = new Hashtable();
		headers.Add("Authorization", GA.API.Submit.CreateMD5Hash(requestInfo + GA.SettingsGA.ApiKey));
		
		//Try to send the data
		www = new WWW(url, new byte[] { 0 }, headers);
		
		#else
		
		//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", GA.API.Submit.CreateMD5Hash(requestInfo + GA.SettingsGA.ApiKey));
		
		//Try to send the data
		www = new WWW(url, new byte[] { 0 }, headers);

		#endif
		
		GA.RunCoroutine(Request(www, RequestType.GA_GetHeatmapData, successEvent, errorEvent),()=>www.isDone);
				
		return www;
	}
	
	public static double DateTimeToUnixTimestamp(DateTime dateTime)
	{
		return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
	}
	
	#endregion
	
	#region private methods
	
	private IEnumerator Request(WWW www, RequestType requestType, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent)
	{
		yield return www;
		
		GA.Log("GameAnalytics: URL " + www.url);
		
		try
		{
			if (www.error != null)
			{
				throw new Exception(www.error);
			}
			
			//Get the JSON object from the response
			string text = www.text;
			text = text.Replace("null","0");
			Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(text);
			
			if (returnParam != null)
			{
				
				GA.Log("GameAnalytics: Result: " + text);
				
			
				if (successEvent != null)
				{
					successEvent(requestType, returnParam, errorEvent);
				}
			}
			else
			{
				throw new Exception(text);
			}
		}
		catch (Exception e)
		{
			if (errorEvent != null)
			{
				errorEvent(e.Message);
			}
		}
	}
	
	/// <summary>
	/// Gets the url on the GA server matching the specific service we are interested in
	/// </summary>
	/// <param name="category">
	/// Determines the GA service/category <see cref="System.String"/>
	/// </param>
	/// <returns>
	/// A string representing the url matching our service choice on the GA server <see cref="System.String"/>
	/// </returns>
	private string GetURL(string category)
	{
		return _baseURL + "/" + category;
	}
	
	#endregion
}

#endif