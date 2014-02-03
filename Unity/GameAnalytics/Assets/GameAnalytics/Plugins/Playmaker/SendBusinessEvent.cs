#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a business event message to the GameAnalytics server")]
	public class SendBusinessEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The event ID")]
		public FsmString EventID;
		
		[RequiredField]
		[Tooltip("Abbreviation of the currency used for the transaction. F.x. USD (U.S. Dollars)")]
		public FsmString Currency;
		
		[RequiredField]
		[Tooltip("The value of the transaction in the lowest currency unit. F.x. if currency is USD then amount should be in cent")]
		public FsmInt Amount;
		
		[Tooltip("The location of the event")]
		public FsmVector3 Position;
		
		public override void Reset()
		{
			EventID = new FsmString() { UseVariable = false };
			Currency = new FsmString() { UseVariable = false };
			Amount = new FsmInt() { UseVariable = false };
			Position = new FsmVector3() { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			if (!Position.IsNone)
				GA.API.Business.NewEvent(EventID.Value, Currency.Value, Amount.Value, Position.Value);
			else
				GA.API.Business.NewEvent(EventID.Value, Currency.Value, Amount.Value);
			
			Finish();
		}
	}
}

#endif