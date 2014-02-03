#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GA_Tracking : MonoBehaviour
{
	private static bool _eventSuccess = false;
	
	private static string _publicTestKey = "e49c92dcffcf2a73d7b66e8e102d92ec";
	private static string _privateTestKey = "fa54ce4fe3be6b49d6b0fe9a573380075d72ace5";
	
	void Start ()
	{
		List<GA_Submit.Item> queue = new List<GA_Submit.Item>();
		Hashtable parameters = new Hashtable() {
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "Platform" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message], GA.API.GenericInfo.GetSystem() },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID },
			{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
		};
		queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Log, Parameters = parameters});
		GA.API.Submit.SubmitQueue(queue, Submitted, SubmitError, true, _publicTestKey, _privateTestKey);
		
		if (FindObjectOfType(typeof(GA_Tracker)) != null)
		{
			List<GA_Submit.Item> queueTracker = new List<GA_Submit.Item>();
			Hashtable parametersTracker = new Hashtable() {
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "GA_Tracker" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
			};
			queueTracker.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Event, Parameters = parametersTracker});
			GA.API.Submit.SubmitQueue(queueTracker, Submitted, SubmitError, true, _publicTestKey, _privateTestKey);
		}
		
		if (FindObjectOfType(typeof(GA_SystemTracker)) != null)
		{
			List<GA_Submit.Item> queueSystemTracker = new List<GA_Submit.Item>();
			Hashtable parametersSystemTracker = new Hashtable() {
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "GA_SystemTracker" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
			};
			queueSystemTracker.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Event, Parameters = parametersSystemTracker});
			GA.API.Submit.SubmitQueue(queueSystemTracker, Submitted, SubmitError, true, _publicTestKey, _privateTestKey);
		}
		
		GA_Queue.OnSuccess += EventSuccess;
	}
	
	private static void Submitted(List<GA_Submit.Item> items, bool success)
	{
		
	}
	
	private static void SubmitError(List<GA_Submit.Item> items)
	{
		
	}
	
	private static void EventSuccess ()
	{
		if (!_eventSuccess)
		{
			_eventSuccess = true;
			
			List<GA_Submit.Item> queue = new List<GA_Submit.Item>();
			Hashtable parameters = new Hashtable() {
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID], "SubmitSuccess" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Level], "Unity Editor" },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.UserID], GA.API.GenericInfo.UserID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.SessionID], GA.API.GenericInfo.SessionID },
				{ GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Build], GA_Settings.VERSION }
			};
			queue.Add(new GA_Submit.Item() { AddTime = 0, Count = 1, Type = GA_Submit.CategoryType.GA_Event, Parameters = parameters});
			GA.API.Submit.SubmitQueue(queue, Submitted, SubmitError, true, _publicTestKey, _privateTestKey);
		}
	}
}

#endif