#if false

using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("GameAnalytics")]
	[Tooltip("Sends a quality event message to the GameAnalytics server")]
	public class SendQualityEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The event ID")]
		public FsmString EventID;
		
		[Tooltip("The event value")]
		public FsmString Message;
		
		[Tooltip("The location of the event")]
		public FsmVector3 Position;
		
		public override void Reset()
		{
			EventID = new FsmString() { UseVariable = false };
			Message = new FsmString() { UseVariable = true };
			Position = new FsmVector3() { UseVariable = true };
		}
		
		public override void OnEnter()
		{
			if (!Message.IsNone && !Position.IsNone)
				GA.API.Quality.NewEvent(EventID.Value, Message.Value, Position.Value);
			else if (!Message.IsNone)
				GA.API.Quality.NewEvent(EventID.Value, Message.Value);
			else if (!Position.IsNone)
				GA.API.Quality.NewEvent(EventID.Value, Position.Value);
			else
				GA.API.Quality.NewEvent(EventID.Value);
			
			Finish();
		}
	}
}

#endif