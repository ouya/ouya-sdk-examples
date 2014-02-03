/// <summary>
/// This class handles sending data to the Game Analytics servers.
/// JSON data is sent using a MD5 hashed authorization header, containing the JSON data and private key
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
//using LitJson;
using System.Linq;

#if !UNITY_FLASH && !UNITY_WP8 && !UNITY_METRO
using System.Security.Cryptography;
#endif

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public class GA_Submit
{	
	/// <summary>
	/// Handlers for success and fail during submit to the GA server
	/// </summary>
	public delegate void SubmitSuccessHandler(List<Item> items, bool success);
	public delegate void SubmitErrorHandler(List<Item> items);
	
	/// <summary>
	/// Types of services on the GA server
	/// </summary>
	public enum CategoryType { GA_User, GA_Event, GA_Log, GA_Purchase, GA_Error }
	
	/// <summary>
	/// An item is a message (parameters) and the category (GA service) the message should be sent to
	/// </summary>
	public struct Item
	{
		public CategoryType Type;
		public Hashtable Parameters;
		public float AddTime;
		public int Count;
	}
	
	/// <summary>
	/// All the different types of GA services
	/// </summary>
	public Dictionary<CategoryType, string> Categories;
	
	#region private values
	
	private string _publicKey;
	private string _privateKey;
	private string _baseURL = "://api.gameanalytics.com";
	private string _version = "1";
	
	#endregion
	
	#region public methods
	
	/// <summary>
	/// Sets the users public and private keys for the GA server
	/// </summary>
	/// <param name="publicKey">
	/// The public key which identifies this users game <see cref="System.String"/>
	/// </param>
	/// <param name="privateKey">
	/// The private key used to encode messages <see cref="System.String"/>
	/// </param>
	public void SetupKeys(string publicKey, string privateKey)
	{
		_publicKey = publicKey;
		_privateKey = privateKey;
		
		Categories = new Dictionary<CategoryType, string>()
		{
			{ CategoryType.GA_User, "user" },
			{ CategoryType.GA_Event, "design" },
			{ CategoryType.GA_Log, "quality" },
			{ CategoryType.GA_Purchase, "business" },
			{ CategoryType.GA_Error, "error" }
		};
	}
	
	/// <summary>
	/// Devides a list of messages into categories and calls Submit to send the messages to the GA servers.
	/// </summary>
	/// <param name="item">
	/// The list of messages (queue) <see cref="Item"/>
	/// </param>
	/// <param name="successEvent">
	/// If successful this will be fired <see cref="SubmitSuccessHandler"/>
	/// </param>
	/// <param name="errorEvent">
	/// If an error occurs this will be fired <see cref="SubmitErrorHandler"/>
	/// </param>
	public void SubmitQueue(List<Item> queue, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent, bool gaTracking, string pubKey, string priKey)
	{
		if ((_publicKey.Equals("") || _privateKey.Equals("")) && (pubKey.Equals("") || priKey.Equals("")))
		{
			if (!gaTracking)
				GA.LogError("Game Key and/or Secret Key not set. Open GA_Settings to set keys.");
			
			return;
		}
		
		//GA_TODO: Optimize by moving dictionary outside this fucntion. Submit is called often
		Dictionary<CategoryType, List<Item>> categories = new Dictionary<CategoryType, List<Item>>();
		
		/* Put all the items in the queue into a list containing only the messages of that category type.
		 * This way we end up with a list of items for each category type */
		foreach (Item item in queue)
		{
			if (categories.ContainsKey(item.Type))
			{
				
				/* If we already added another item of this type then remove the UserID, SessionID, and Build values if necessary.
				 * These values only need to be present in each message once, since they will be the same for all items */
				
				/* TODO: below not supported yet in API (exclude information)
			 	* activate once redundant data can be trimmed */
				
				/*
				if (item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
					item.Parameters.Remove(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]);
				
				if (item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID]))
					item.Parameters.Remove(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID]);
				
				if (item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build]))
					item.Parameters.Remove(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build]);
				*/
				
				/* TODO: remove below when API supports exclusion of data */
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID);
				
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID]))
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID);

				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build]))
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.SettingsGA.Build);
				
				categories[item.Type].Add(item);
			}
			else
			{
				/* If we did not add another item of this type yet, then add the UserID, SessionID, and Build values if necessary.
				 * These values only need to be present in each message once, since they will be the same for all items */
				
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID);
				
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID]))
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID);
				
				if (!item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build]))
					item.Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA.SettingsGA.Build);
				
				categories.Add(item.Type, new List<Item> { item });
			}
		}
		
		GA.RunCoroutine(Submit(categories, successEvent, errorEvent, gaTracking, pubKey, priKey));
	}
	
	/// <summary>
	/// Takes a dictionary with a item list for each category type. All items in each category are submitted together to the GA server.
	/// </summary>
	/// <param name="item">
	/// The list of items, each holding a message and service type <see cref="Item"/>
	/// </param>
	/// <param name="successEvent">
	/// If successful this will be fired <see cref="SubmitSuccessHandler"/>
	/// </param>
	/// <param name="errorEvent">
	/// If an error occurs this will be fired <see cref="SubmitErrorHandler"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator Submit(Dictionary<CategoryType, List<Item>> categories, SubmitSuccessHandler successEvent, SubmitErrorHandler errorEvent, bool gaTracking, string pubKey, string priKey)
	{
		if (pubKey.Equals(""))
			pubKey = _publicKey;
		
		if (priKey.Equals(""))
			priKey = _privateKey;
		
		//For each existing category, submit a message containing all the items of that category type
		foreach (KeyValuePair<CategoryType, List<Item>> kvp in categories)
		{
			List<Item> items = kvp.Value;
			
			if (items.Count == 0)
			{
				yield break;
			}
			
			//Since all the items must have the same category (we make sure they do below) we can get the category from the first item
			CategoryType serviceType = items[0].Type;
			string url = GetURL(Categories[serviceType], pubKey);
			
			//Make sure that all items are of the same category type, and put all the parameter collections into a list
			List<Hashtable> itemsParameters = new List<Hashtable>();
			
			for (int i = 0; i < items.Count; i++)
			{
				if (serviceType != items[i].Type)
				{
					GA.LogWarning("GA Error: All messages in a submit must be of the same service/category type.");
					if (errorEvent != null)
					{
						errorEvent(items);
					}
					yield break;
				}
				
				// if user ID is missing from the item add it now (could f.x. happen if custom user id is enabled,
				// and the item was added before the custom user id was provided)
				if (!items[i].Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]))
					items[i].Parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID);
				else if (items[i].Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]] == null)
					items[i].Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID]] = GA.API.GenericInfo.UserID;
				
				Hashtable parameters;
				
				if (items[i].Count > 1)
				{
					/* so far we don't do anything special with stacked messages - we just send a single message
					 * GA_TODO: stacked messages should be handle correctly.*/
					parameters = items[i].Parameters;
				}
				else
				{
					parameters = items[i].Parameters;
				}
	
				itemsParameters.Add(parameters);
			}
			
			//Make a JSON array string out of the list of parameter collections
			string json = DictToJson(itemsParameters);
			
			/* If we do not have access to a network connection (or we are roaming (mobile devices) and GA_static_api.Settings.ALLOWROAMING is false),
			 * and data is set to be archived, then archive the data and pretend the message was sent successfully */
			if  (GA.SettingsGA.ArchiveData && !gaTracking && !GA.SettingsGA.InternetConnectivity)
			{
				if (GA.SettingsGA.DebugMode)
				{
					GA.Log("GA: Archiving data (no network connection).");
				}
				
				GA.API.Archive.ArchiveData(json, serviceType);
				if (successEvent != null)
				{
					successEvent(items, true);
				}
				yield break;
			}
			else if (!GA.SettingsGA.InternetConnectivity)
			{
				if (!gaTracking)
					GA.LogWarning("GA Error: No network connection.");
				
				if (errorEvent != null)
				{
					errorEvent(items);
				}
				yield break;
			}
			
			//Prepare the JSON array string for sending by converting it to a byte array
			byte[] data = Encoding.UTF8.GetBytes(json);
			
			WWW www = null;
			
			#if !UNITY_WP8 && !UNITY_METRO
			
			//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
			Hashtable headers = new Hashtable();
			headers.Add("Authorization", CreateMD5Hash(json + priKey));
			//headers.Add("Content-Length", data.Length);
			
			//Try to send the data
			www = new WWW(url, data, headers);
			
			#else
			
			//Set the authorization header to contain an MD5 hash of the JSON array string + the private key
			
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", CreateMD5Hash(json + priKey));
			//headers.Add("Content-Length", data.Length.ToString());
			
			//Try to send the data
			www = new WWW(url, data, headers);
			
			#endif
			
			#if !UNITY_FLASH && !UNITY_WP8 && !UNITY_METRO
			//Set thread priority low
			www.threadPriority = ThreadPriority.Low;
			#endif
			
			//Wait for response
			yield return www;
			
			if (GA.SettingsGA.DebugMode && !gaTracking)
			{
				GA.Log("GA URL: " + url);
				GA.Log("GA Submit: " + json);
				GA.Log("GA Hash: " + CreateMD5Hash(json + priKey));
			}
			
			try
			{
				if (www.error != null && !CheckServerReply(www))
				{
					throw new Exception(www.error);
				}
				
				//Get the JSON object from the response
				Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
				
				//If the response contains the key "status" with the value "ok" we know that the message was sent and recieved successfully
				if ((returnParam != null &&
				    returnParam.ContainsKey("status") && returnParam["status"].ToString().Equals("ok")) ||
					CheckServerReply(www))
				{
					if (GA.SettingsGA.DebugMode && !gaTracking)
					{
						GA.Log("GA Result: " + www.text);
					}
					
					if (successEvent != null)
					{
						successEvent(items, true);
					}
				}
				else
				{
					/* The message was not sent and recieved successfully: Stop submitting all together if something
					 * is completely wrong and we know we will not be able to submit any messages at all..
					 * Such as missing or invalid public and/or private keys */
					if (returnParam != null &&
					    returnParam.ContainsKey("message") && returnParam["message"].ToString().Equals("Game not found") &&
						returnParam.ContainsKey("code") && returnParam["code"].ToString().Equals("400"))
					{
						if (!gaTracking)
							GA.LogWarning("GA Error: " + www.text + " (NOTE: make sure your Game Key and Secret Key match the keys you recieved from the Game Analytics website. It might take a few minutes before a newly added game will be able to recieve data.)");
						
						//An error event with a null parameter will stop the GA wrapper from submitting messages
						if (errorEvent != null)
						{
							errorEvent(null);
						}
					}
					else
					{
						if (!gaTracking)
							GA.LogWarning("GA Error: " + www.text);
						
						if (errorEvent != null)
						{
							errorEvent(items);
						}
					}
				}
			}
			catch (Exception e)
			{
				if (!gaTracking)
					GA.LogWarning("GA Error: " + e.Message);
				
				/* If we hit one of these errors we should not attempt to send the message again
				 * (if necessary we already threw a GA Error which may be tracked) */
				if (e.Message.Contains("400 Bad Request"))
				{
					//An error event with a null parameter will stop the GA wrapper from submitting messages
					if (errorEvent != null)
					{
						errorEvent(null);
					}
				}
				else
				{
					if (errorEvent != null)
					{
						errorEvent(items);
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Gets the base url to the GA server
	/// </summary>
	/// <param name="inclVersion">
	/// Should the version be included? <see cref="System.Boolean"/>
	/// </param>
	/// <returns>
	/// A string representing the base url (+ version if inclVersion is true) <see cref="System.String"/>
	/// </returns>
	public string GetBaseURL(bool inclVersion)
	{
		if (inclVersion)
			return GetUrlStart() + _baseURL + "/" + _version;
		
		return GetUrlStart() + _baseURL;
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
	public string GetURL(string category, string pubKey)
	{
		return GetUrlStart() + _baseURL + "/" + _version + "/" + pubKey + "/" + category;
	}
	
	private string GetUrlStart()
	{
		if (Application.absoluteURL.StartsWith("https"))
			return "https";
		else
			return "http";
	}
	
	/// <summary>
	/// Encodes the input as a MD5 hash
	/// </summary>
	/// <param name="input">
	/// The input we want encoded <see cref="System.String"/>
	/// </param>
	/// <returns>
	/// The MD5 hash encoded result of input <see cref="System.String"/>
	/// </returns>
	public string CreateMD5Hash(string input)
	{
		#if !UNITY_FLASH && !UNITY_WP8 && !UNITY_METRO
		
		// Gets the MD5 hash for input
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] data = Encoding.UTF8.GetBytes(input);
		byte[] hash = md5.ComputeHash(data);
		// Transforms as hexa
		string hexaHash = "";
		foreach (byte b in hash) {
			hexaHash += String.Format("{0:x2}", b);
		}
		// Returns MD5 hexa hash as string
		return hexaHash;
		
		#elif UNITY_WP8 || UNITY_METRO
		
		byte[] data = Encoding.UTF8.GetBytes(input);
		byte[] hash = MD5Core.GetHash(data);
		// Transforms as hexa
		string hexaHash = "";
		foreach (byte b in hash) {
			hexaHash += String.Format("{0:x2}", b);
		}
		// Returns MD5 hexa hash as string
		return hexaHash;
		
		#else
		
		return MD5Wrapper.Md5Sum(input);
		
		#endif
	}
	
	/// <summary>
	/// Encodes the input as a MD5 hash
	/// </summary>
	/// <param name="input">
	/// The input we want encoded <see cref="System.String"/>
	/// </param>
	/// <returns>
	/// The MD5 hash encoded result of input <see cref="System.String"/>
	/// </returns>
	/*public string CreateMD5Hash(byte[] input)
	{
		#if !UNITY_FLASH
		
		// Gets the MD5 hash for input
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] hash = md5.ComputeHash(input);
		// Transforms as hexa
		string hexaHash = "";
		foreach (byte b in hash) {
			hexaHash += String.Format("{0:x2}", b);
		}
		// Returns MD5 hexa hash as string
		return hexaHash;
		
		#else
		
		return MD5Wrapper.Md5Sum(input.ToString());
		
		#endif
	}*/
	
	
	/// <summary>
	/// Encodes the input as a sha1 hash
	/// </summary>
	/// <param name="input">
	/// The input we want to encoded <see cref="System.String"/>
	/// </param>
	/// <returns>
	/// The sha1 hash encoded result of input <see cref="System.String"/>
	/// </returns>
	#if !UNITY_FLASH && !UNITY_WP8 && !UNITY_METRO
	public string CreateSha1Hash(string input)
	{
		// Gets the sha1 hash for input
		SHA1 sha1 = new SHA1CryptoServiceProvider();
		byte[] data = Encoding.UTF8.GetBytes(input);
		byte[] hash = sha1.ComputeHash(data);
		// Returns sha1 hash as string
		return Convert.ToBase64String(hash);
	}
	
	public string GetPrivateKey()
	{
		return _privateKey;
	}
	
	#endif
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Check if a reply from the server was accepted. All response codes from 200 to 299 are accepted.
	/// </summary>
	/// <param name="www">
	/// The www object which contains response headers
	/// </param>
	/// <returns>
	/// Return true if response code is from 200 to 299. Otherwise returns false.
	/// </returns>
	public bool CheckServerReply(WWW www)
	{
		try
		{
			if (www.error != null)
			{
				string errStart = www.error.Substring(0, 3);
				if (errStart.Equals("201") || errStart.Equals("202") || errStart.Equals("203") || errStart.Equals("204") || errStart.Equals("205") || errStart.Equals("206"))
					return true;
			}
			
			if (!www.responseHeaders.ContainsKey("STATUS"))
				return false;
			
			string status = www.responseHeaders["STATUS"];
			
			string[] splitStatus = status.Split(' ');
			
			int responseCode;
			
			if (splitStatus.Length > 1 && int.TryParse(splitStatus[1], out responseCode))
			{
				if (responseCode >= 200 && responseCode < 300)
					return true;
			}
			
			return false;
		}
		catch
		{
			return false;
		}
	}
	
	#endregion
	
	/// <summary>
	/// Dicts to json. This function is 35% faster than the LitJson library.
	/// </summary>
	/// <returns>
	/// The to json.
	/// </returns>
	/// <param name='list'>
	/// List.
	/// </param>
	public static string DictToJson(List<Hashtable> list)
	{
		string b = "[";
		int d = 0;
		int c = 0;
		foreach(var dict in list)
		{
			b += '{';
			c = 0;
			foreach(var key in dict.Keys)
			{
				c++;
				b += "\""+key+"\":\""+dict[key]+"\"";
				if(c<dict.Keys.Count)
					b += ',';
			}
			b += '}';
			d++;
			if(d<list.Count)
				b += ',';
		}
		b += "]";
		return b;
	}
}