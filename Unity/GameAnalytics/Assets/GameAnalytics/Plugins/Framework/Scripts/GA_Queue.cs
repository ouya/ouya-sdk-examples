/// <summary>
/// This class handles the submit queue. Any data which should be sent to the Game Analytics servers should be added to the queue
/// using the AddItem method. All messages in the queue will be sent at every TIMER interval.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

public static class GA_Queue
{
	public delegate void EventSuccess();
	public static event EventSuccess OnSuccess;
	
	/// <summary>
	/// The maximum length of the submit queue. When messages are not submitted due to an error they are put back
	/// into the queue and another attempt at submitting them is made during the next submit interval. When the queue
	/// reaches the maximum length the oldest messages will be dropped.
	/// </summary>
	public static int MAXQUEUESIZE = 800;
	
	/// <summary>
	/// If true the game will automatically quit after data has been submitted to the server
	/// </summary>
	public static bool QUITONSUBMIT = false;
	
	#region private values
	
	/// <summary>
	/// A list containing all the messages which should be submitted to the GA server on the next submit
	/// </summary>
	private static List<GA_Submit.Item> _queue = new List<GA_Submit.Item>();
	
	/// <summary>
	/// A temporary list containing any new messages being recorded during a submit.
	/// These messages will be put back into the queue for the next submit
	/// </summary>
	private static List<GA_Submit.Item> _tempQueue = new List<GA_Submit.Item>();
	
	/// <summary>
	/// A list containing all the failed messages from a submit. These messages will be put
	/// back into the submit queue when another message is submitted succesfully
	/// </summary>
	private static List<GA_Submit.Item> _errorQueue = new List<GA_Submit.Item>();
	
	/// <summary>
	/// While true we are submitting messages to the GA server and new messages should therefore be put into the temporary queue
	/// </summary>
	private static bool _submittingData = false;
	
	/// <summary>
	/// The number of messages we have submitted to the GA server so far during any queue submit 
	/// </summary>
	private static int _submitCount = 0;
	
	/// <summary>
	/// set to true when the game cannot be found, to prevent us from sending messages constantly with no hope of success
	/// </summary>
	private static bool _endsubmit = false;
	
	#endregion
	
	#region public methods
	
	/// <summary>
	/// Add a new message to the submit queue. If we are in the middle of a queue submit we add the message to the temporary queue instead
	/// </summary>
	/// <param name="parameters">
	/// The message is a dictionary of parameters <see cref="Dictionary<System.String, System.Object>"/>
	/// </param>
	/// <param name="type">
	/// The GA service to send the message to (see GA_Submit) <see cref="GA_Submit.CategoryType"/>
	/// </param>
	/// <param name="stack">
	/// If true any identical messages in the queue will be merged/stacked as a single message, to save server load
	/// </param>
	public static void AddItem(Hashtable parameters, GA_Submit.CategoryType type, bool stack)
	{
		//No reason to add any more items if we have stopped submitting data or we are not supposed to submit in the first place
		if (_endsubmit || (Application.isEditor && !GA.SettingsGA.RunInEditorPlayMode))
		{
			return;
		}
		
		GA_Submit.Item item = new GA_Submit.Item
		{
			Type = type,
			Parameters = parameters,
			AddTime = Time.time
		};
		
		if (_submittingData)
		{
			if (stack && type == GA_Submit.CategoryType.GA_Log)
			{
				StackQueue(_tempQueue, item);
			}
			else
			{
				_tempQueue.Add(item);
			} 
		}
		else
		{
			if (stack && type == GA_Submit.CategoryType.GA_Log)
			{
				StackQueue(_queue, item);
			}
			else
			{
				_queue.Add(item);
			}
		}
	}
	
	/// <summary>
	/// At every timer interval we submit the next batch of messages to the GA server
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public static IEnumerator SubmitQueue()
	{
		while (!_endsubmit)
		{

			while (GA.SettingsGA.CustomUserID && GA.API.GenericInfo.UserID == string.Empty)
			{
				GA.LogWarning("GameAnalytics: User ID not set. No data will be sent until Custom User ID is set.");
				yield return new WaitForSeconds(10f);
			}
			
			//If we're still submitting data then wait half a second and try again
			while (_submittingData)
			{
				yield return new WaitForSeconds(0.5f);
			}
			
			//If we have internet connection then add any archived data to the submit queue
			if (GA.SettingsGA.ArchiveData && GA.SettingsGA.InternetConnectivity)
			{
				List<GA_Submit.Item> archivedItems = GA.API.Archive.GetArchivedData();
				
				if (archivedItems != null && archivedItems.Count > 0)
				{
					foreach (GA_Submit.Item item in archivedItems)
					{
						GA_Queue.AddItem(item.Parameters, item.Type, false);
					}
				
					if (GA.SettingsGA.DebugMode)
					{
						GA.Log("GA: Network connection detected. Adding archived data to next submit queue.");
					}
				}
			}
			
			ForceSubmit();
			
			//Wait for the next timer interval before we try to submit again
			yield return new WaitForSeconds(GA.SettingsGA.SubmitInterval);
		}
	}
	
	/// <summary>
	/// Submit any queued up data immediately. Useful when you want to control when data is submitted.
	/// </summary>
	public static void ForceSubmit()
	{
		GA_SpecialEvents.SubmitAverageFPS();
		
		//If we have something to submit and we have not stopped submitting completely then we start submitting data
		if (_queue.Count > 0 && !_submittingData && !_endsubmit)
		{
			_submittingData = true;
			
			GA.Log("GameAnalytics: Queue submit started");
			
			GA.API.Submit.SubmitQueue(_queue, Submitted, SubmitError, false, string.Empty, string.Empty);
		}
	}
	
	/// <summary>
	/// No more data will be sent after this timer interval
	/// </summary>
	public static void EndSubmit()
	{
		GA.Log("GA: Ending all data submission after this timer interval");
		
		_endsubmit = true;
	}
	
	#endregion
	
	#region private methods
	
	/// <summary>
	/// If we already have the same message item in the queue we simply increase the message count instead of adding a duplicate.
	/// </summary>
	/// <param name='queue'>
	/// Queue.
	/// </param>
	/// <param name='item'>
	/// Item.
	/// </param>
	private static void StackQueue(List<GA_Submit.Item> queue, GA_Submit.Item item)
	{
		bool stacked = false;
		for (int i = 0; i < queue.Count; i++)
		{
			if (!stacked && queue[i].Type == GA_Submit.CategoryType.GA_Log && item.Type == GA_Submit.CategoryType.GA_Log &&
				queue[i].Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID]) &&
				item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID]) &&
				queue[i].Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID]].Equals(item.Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.EventID]]) &&
				queue[i].Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message]) &&
				item.Parameters.ContainsKey(GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message]) &&
				queue[i].Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message]].Equals(item.Parameters[GA_ServerFieldTypes.Fields[GA_ServerFieldTypes.FieldType.Message]]))
			{
				stacked = true;
				queue[i] = new GA_Submit.Item { AddTime = queue[i].AddTime, Parameters = queue[i].Parameters, Type = queue[i].Type, Count = Mathf.Max(2, queue[i].Count + 1) };
			}
		}
		if (!stacked)
		{
			queue.Add(item);
		}
	}
	
	/// <summary>
	/// This is called once for every successful message submitted. We count the number of messages submitted and when
	/// we are done submitting everything in the queue we put everything in the temporary queue back into the queue
	/// </summary>
	private static void Submitted(List<GA_Submit.Item> items, bool success)
	{
		_submitCount += items.Count;
		
		// Update GA_Status
		if (success)
		{
			if (OnSuccess != null)
				OnSuccess();
			
			GA.SettingsGA.TotalMessagesSubmitted += items.Count;
			
			foreach (GA_Submit.Item it in items)
			{
				switch (it.Type)
				{
				case GA_Submit.CategoryType.GA_Event:
					GA.SettingsGA.DesignMessagesSubmitted++;
					break;
				case GA_Submit.CategoryType.GA_Log:
					GA.SettingsGA.QualityMessagesSubmitted++;
					break;
				case GA_Submit.CategoryType.GA_Error:
					GA.SettingsGA.ErrorMessagesSubmitted++;
					break;
				case GA_Submit.CategoryType.GA_Purchase:
					GA.SettingsGA.BusinessMessagesSubmitted++;
					break;
				case GA_Submit.CategoryType.GA_User:
					GA.SettingsGA.UserMessagesSubmitted++;
					break;
				}
			}
		}
		
		/* When all items have either been submitted successfully or errors have been stored in the error queue,
		 * we can return to collecting new events normally */
		if (_submitCount >= _queue.Count)
		{
			if (GA.SettingsGA.DebugMode)
			{
				GA.Log("GA: Queue submit over");
			}
			
			//If we were told to quit after this submit then quit
			if (QUITONSUBMIT)
			{
				Application.Quit();
			}
			
			//Put the items collected while we were submitting in to the queue and clear the temporary queue
			_queue = _tempQueue;
			_tempQueue = new List<GA_Submit.Item>();
			
			//Do not attempt to re-submit errored messages until a message has been submitted successfully
			if (success)
			{
				_queue.AddRange(_errorQueue);
				_errorQueue = new List<GA_Submit.Item>();
			}
			
			//Reset counters and go back to normal event collection
			_submitCount = 0;
			_submittingData = false;
		}
	}
	
	/// <summary>
	/// This is called once for every failed message (an error occurs during submit). We put the message into the temporary queue - this way
	/// it will be put back into the queue at the end of the queue submit and on the next queue submit we attempt to submit the message again
	/// </summary>
	/// <param name="item">
	/// The failed message <see cref="GA_Submit.Item"/>
	/// </param>
	private static void SubmitError(List<GA_Submit.Item> items)
	{
		//If items are null we should stop submitting data after this timer interval because the game cannot be found
		if (items == null)
		{
			GA.Log("GA: Ending all data submission after this timer interval");
			
			_endsubmit = true;
			return;
		}
		else
		{
			// Update GA_Status
			GA.SettingsGA.TotalMessagesFailed += items.Count;
			
			foreach (GA_Submit.Item it in items)
			{
				switch (it.Type)
				{
				case GA_Submit.CategoryType.GA_Event:
					GA.SettingsGA.DesignMessagesFailed++;
					break;
				case GA_Submit.CategoryType.GA_Log:
					GA.SettingsGA.QualityMessagesFailed++;
					break;
				case GA_Submit.CategoryType.GA_Error:
					GA.SettingsGA.ErrorMessagesFailed++;
					break;
				case GA_Submit.CategoryType.GA_Purchase:
					GA.SettingsGA.BusinessMessagesFailed++;
					break;
				case GA_Submit.CategoryType.GA_User:
					GA.SettingsGA.UserMessagesFailed++;
					break;
				}
			}
			
			// Check to see if we have access to a network connection - if this is the cause of the error future submits will take it into account
			GA.RunCoroutine(GA.SettingsGA.CheckInternetConnectivity(false));
		}
		
		_errorQueue.AddRange(items);
		
		/* If the number of error messages exceeds the limit we sort the errors so game design events
		 * are the first to be discarded. Then we remove the access error messages */
		if (_errorQueue.Count > MAXQUEUESIZE)
		{
			_errorQueue.Sort(new ItemComparer());
			_errorQueue.RemoveRange(MAXQUEUESIZE, _errorQueue.Count - MAXQUEUESIZE);
		}
		
		Submitted(items, false);
	}
	
	#endregion
}

class ItemComparer : IComparer<GA_Submit.Item>
{
	public int Compare(GA_Submit.Item item1, GA_Submit.Item item2)
	{
		if (item1.Type != GA_Submit.CategoryType.GA_Event &&
			item2.Type == GA_Submit.CategoryType.GA_Event)
		{
			return 1;
		}
		else if (item2.Type != GA_Submit.CategoryType.GA_Event &&
				 item1.Type == GA_Submit.CategoryType.GA_Event)
		{
			return -1;
		}
		
		float n1 = item1.AddTime;
		float n2 = item2.AddTime;
		
		if (n1 < n2)
		{
			return 1;
		}
		else if (n1 == n2)
		{
			return 0;
		}
		else
		{
			return -1;
		}
	}
}
