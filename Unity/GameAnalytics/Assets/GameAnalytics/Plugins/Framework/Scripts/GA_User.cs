/// <summary>
/// This class handles user events. Anything connected to the player, such as age, country, gender, etc.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public class GA_User
{
	public enum Gender { Unknown, Male, Female }
	
	#region public methods
	
	public void NewUser(Gender gender, int? birth_year, int? friend_count)
	{
		CreateNewUser(gender, birth_year, friend_count, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
	}
	
	public void NewUser(Gender gender, int? birth_year, int? friend_count, string ios_id, string android_id, string platform, string device, string os, string osVersion, string sdk)
	{
		CreateNewUser(gender, birth_year, friend_count, ios_id, android_id, platform, device, os, osVersion, sdk, null, null, null, null, null, null, null);
	}
	
	public void NewUser(Gender gender, int? birth_year, int? friend_count, string ios_id, string android_id, string platform, string device, string os, string osVersion, string sdk, string installPublisher, string installSite, string installCampaign, string installAdgroup, string installAd, string installKeyword, string facebookID)
	{
		CreateNewUser(gender, birth_year, friend_count, ios_id, android_id, platform, device, os, osVersion, sdk, installPublisher, installSite, installCampaign, installAdgroup, installAd, installKeyword, facebookID);
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// Adds information about the user/player
	/// </summary>
	/// <param name="gender">
	/// The gender of the user. If the gender is unknown information will not be submitted.
	/// </param>
	/// <param name="birth_year">
	/// The year the user was born. Set to "null" if unknown.
	/// </param>
	/// <param name="country">
	/// The ISO2 country code the user is playing from. See: http://en.wikipedia.org/wiki/ISO_3166-2. Set to "null" if unknown.
	/// </param>
	/// <param name="state">
	/// The code of the country state the user is playing from. Set to "null" if unknown.
	/// </param>
	/// /// <param name="friend_count">
	/// The number of friends in the user's network. Set to "null" if unknown.
	/// </param>
	private void CreateNewUser(Gender gender, int? birth_year, int? friend_count, string ios_id, string android_id, string platform, string device, string os, string osVersion, string sdk, string installPublisher, string installSite, string installCampaign, string installAdgroup, string installAd, string installKeyword, string facebookID)
	{
		Hashtable parameters = new Hashtable();
		
		if (gender == Gender.Male)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Gender], 'M');
		}
		else if (gender == Gender.Female)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Gender], 'F');
		}
		
		if (birth_year.HasValue && birth_year.Value != 0)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Birth_year], birth_year.ToString());
		}
		
		if (friend_count.HasValue)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Friend_Count], friend_count.ToString());
		}
		
		if (ios_id != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Ios_id], ios_id);
		}
		
		if (android_id != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Android_id], android_id);
		}
		
		if (platform != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Platform], platform);
		}
		
		if (device != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Device], device);
		}
		
		if (os != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Os], os);
		}
		
		if (osVersion != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.OsVersion], osVersion);
		}
		
		if (sdk != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Sdk], sdk);
		}
		
		if (installPublisher != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallPublisher], installPublisher);
		}
		
		if (installSite != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallSite], installSite);
		}
		
		if (installCampaign != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallCampaign], installCampaign);
		}
		
		if (installAdgroup != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallAdgroup], installAdgroup);
		}
		
		if (installAd != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallAd], installAd);
		}
		
		if (installKeyword != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.InstallKeyword], installKeyword);
		}

		if (facebookID != null)
		{
			parameters.Add(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.FacebookID], facebookID);
		}

		if (parameters.Count == 0)
		{
			GA.LogWarning("GA: No data to send with NewUser event; event will not be added to queue");
			return;
		}

		GA_Queue.AddItem(parameters, GA_Submit.CategoryType.GA_User, false);
	}
	
	#endregion
}