#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a user event message to the GameAnalytics server")]
	public class SendUserEvent : FsmStateAction
	{
		[Tooltip("The player's gender")]
		public GA_User.Gender Gender;
		
		[Tooltip("The year the player was born")]
		public FsmInt BirthYear;
		
		[Tooltip("The number of friends in the player's network.")]
		public FsmInt FriendCount;
		
		public override void Reset()
		{
			Gender = GA_User.Gender.Unknown;
			BirthYear = new FsmInt() { UseVariable = true };
			FriendCount = new FsmInt() { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			if (!BirthYear.IsNone && !FriendCount.IsNone)
				GA.API.User.NewUser(Gender, BirthYear.Value, FriendCount.Value);
			else if (!BirthYear.IsNone)
				GA.API.User.NewUser(Gender, BirthYear.Value, null);
			else if (!FriendCount.IsNone)
				GA.API.User.NewUser(Gender, null, FriendCount.Value);
			else
				GA.API.User.NewUser(Gender, null, null);
			
			Finish();
		}
	}
}

#endif