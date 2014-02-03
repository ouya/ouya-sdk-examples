#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a design event message to the GameAnalytics server")]
	public class SendDesignEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The event ID")]
		public FsmString EventID;
		
		[Tooltip("The event value")]
		public FsmFloat EventValue;
		
		[Tooltip("The location of the event")]
		public FsmVector3 Position;
		
		public override void Reset()
		{
			EventID = new FsmString() { UseVariable = false };
			EventValue = new FsmFloat() { UseVariable = true };
			Position = new FsmVector3() { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			if (!EventValue.IsNone && !Position.IsNone)
				GA.API.Design.NewEvent(EventID.Value, EventValue.Value, Position.Value);
			else if (!EventValue.IsNone)
				GA.API.Design.NewEvent(EventID.Value, EventValue.Value);
			else if (!Position.IsNone)
				GA.API.Design.NewEvent(EventID.Value, Position.Value);
			else
				GA.API.Design.NewEvent(EventID.Value);
			
			Finish();
		}
	}
}

#endif