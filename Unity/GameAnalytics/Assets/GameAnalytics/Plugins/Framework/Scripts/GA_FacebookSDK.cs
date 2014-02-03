using UnityEngine;
using System;

#if false

using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;

public class GA_FacebookSDK : MonoBehaviour
{
	private string _userID = "";
	private int _friendCount = -1;
	private string _gender = "";

	private bool _fbCalled = false;
	private bool _userUpdated = false;
	private float _logOnTime;

	void Update ()
	{
		if(!_fbCalled && FB.IsLoggedIn)
		{
			_fbCalled = true;
			_userID = FB.UserId;
			_logOnTime = Time.time;
			FB.API("/me", HttpMethod.GET, LogCallbackMe);
			FB.API("/me/friends", HttpMethod.GET, LogCallbackFriends);
		}
		else if (!_userUpdated && _userID != "")
		{
			if (_logOnTime > Time.time + 10 || (_gender != "" && _friendCount > -1))
			{
				_userUpdated = true;

				GA_User.Gender gender = GA_User.Gender.Unknown;

				if (_gender.Equals("male"))
				{
					gender = GA_User.Gender.Male;
				}
				else if (_gender.Equals("female"))
				{
					gender = GA_User.Gender.Female;
				}

				if (_friendCount > -1)
					GA.API.User.NewUser(gender, null, _friendCount, null, null, null, null, null, null, null, null, null, null, null, null, null, _userID);
				else
					GA.API.User.NewUser(gender, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, _userID);
			}
		}
	}

	void LogCallbackMe (FBResult response)
	{
		try {
			var dict = Json.Deserialize(response.Text) as Dictionary<string,object>;
			_gender = (string)dict["gender"];
		}
		catch (Exception e)
		{
			GA.API.Error.NewEvent(GA_Error.SeverityType.error, e.Message);
		}
	}
	
	void LogCallbackFriends (FBResult response)
	{
		try {
			var dict = Json.Deserialize(response.Text) as Dictionary<string,object>;
			
			var friendList = new List<object>();
			friendList = (List<object>)(dict["data"]);
			_friendCount = friendList.Count;
		}
		catch (Exception e)
		{
			GA.API.Error.NewEvent(GA_Error.SeverityType.error, e.Message);
		}
	}

	public static void FacebookPayment (FBResult response)
	{
		try {
			bool error = false;

			if (response.Error != null && !response.Error.Equals(""))
			{
				var dictError = Json.Deserialize(response.Error) as Dictionary<string,object>;

				if (dictError.ContainsKey("code") && dictError.ContainsKey("body"))
				{
					var dictErrorBody = (Dictionary<string,object>)(dictError["body"]);
					if (dictErrorBody.ContainsKey("error"))
					{
						var dictErrorBodyError = (Dictionary<string,object>)(dictErrorBody["error"]);
						error = true;
						GA.API.Error.NewEvent(GA_Error.SeverityType.critical, (string)dictErrorBodyError["message"]);
					}
				}
			}

			if (!error)
			{
				var dict = Json.Deserialize(response.Text) as Dictionary<string,object>;

				if (((string)dict["status"]).Equals("completed"))
				{
					string currency = (string)dict["currency"];
					float amount = float.Parse((string)dict["amount"]);
					float quantity = float.Parse((string)dict["quantity"]);

					if (quantity > 0)
					{
						GA.API.Business.NewEvent("FacebookPurchase", currency, (int)(amount * quantity * 100));
					}
				}
			}
		}
		catch (Exception e)
		{
			GA.API.Error.NewEvent(GA_Error.SeverityType.error, e.Message);
		}
	}
}

#else

public class GA_FacebookSDK : MonoBehaviour
{
}

#endif