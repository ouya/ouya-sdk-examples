/// <summary>
/// This class handles any GUI related to the Game Analytics service, such as user feedback
/// </summary>

using UnityEngine;
using System.Collections;

public class GA_Gui : MonoBehaviour
{
	public enum WindowType { None, MessageTypeWindow, FeedbackWindow, BugWindow };
	
	
	[HideInInspector]
	public bool  GuiEnabled;
	[HideInInspector]
	public bool GuiAllowScreenshot;
	
	// feedback/bug report windows
	private WindowType _windowType = WindowType.None;
	
	private bool _popUpError = false;
	
	private bool _takeScreenshot = false;
	private Texture2D _screenshot;
	
	private Rect _messageTypeWindowRect;
	private Rect _feedbackWindowRect;
	private Rect _bugWindowRect;
	
	private string _topic = "";
	private string _details = "";
	

	private Texture2D keyTexture;
	
	void Start ()
	{
		
		_messageTypeWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 75, 400, 150);
		
		if (GuiAllowScreenshot)
		{
			_feedbackWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 170, 400, 340);
			_bugWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 170, 400, 340);
		}
		else
		{
			_feedbackWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
			_bugWindowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
		}
	}
	
	void OnGUI ()
	{
		Texture2D onNormalSkin = GUI.skin.window.onNormal.background;
		GUI.skin.window.onNormal.background = GUI.skin.window.normal.background;
		
		// feedback/bug report windows
		if (GuiEnabled || _windowType != WindowType.None)
		{
			switch (_windowType)
			{
				case WindowType.None:
					if (GUI.Button(new Rect(Screen.width - 55, Screen.height - 55, 50, 50), GA.SettingsGA.Logo))
					{
						_windowType = WindowType.MessageTypeWindow;
					}
					break;
				case WindowType.MessageTypeWindow:
					_messageTypeWindowRect = GUI.Window(0, _messageTypeWindowRect, MessageTypeWindow, "");
					break;
				case WindowType.FeedbackWindow:
					if (_takeScreenshot)
						StartCoroutine(TakeScreenshot());
					else
						_feedbackWindowRect = GUI.Window(0, _feedbackWindowRect, FeedbackWindow, "");
					break;
				case WindowType.BugWindow:
					if (_takeScreenshot)
						StartCoroutine(TakeScreenshot());
					else
						_bugWindowRect = GUI.Window(0, _bugWindowRect, BugWindow, "");
					break;
			}
		}
		
		
		GUI.skin.window.onNormal.background = onNormalSkin;
	}
	
	#region feedback/bug report windows
	
	void MessageTypeWindow(int windowID)
	{
		GUI.Label(new Rect(10, 15, 380, 50), "Help improve this game by submitting feedback/bug reports");
		
		if (GUI.Button(new Rect(10, 50, 185, 90), "Feedback"))
		{
			_windowType = WindowType.FeedbackWindow;
			GUI.FocusControl("TopicField");
		}
		if (GUI.Button(new Rect(205, 50, 185, 90), "Bug Report"))
		{
			_windowType = WindowType.BugWindow;
			GUI.FocusControl("TopicField");
		}
    }
	
	void FeedbackWindow(int windowID)
	{
		int heightAdd = 0;
		
		GUI.Label(new Rect(10, 15, 380, 50), "Submit feedback");
		
		GUI.Label(new Rect(10, 50, 380, 20), "Topic*");
		GUI.SetNextControlName("TopicField");
		_topic = GUI.TextField(new Rect(10, 70, 380, 20), _topic, 50);
		
		GUI.Label(new Rect(10, 100, 380, 20), "Details*");
		_details = GUI.TextArea(new Rect(10, 120, 380, 130), _details, 400);
		
		if (GuiAllowScreenshot)
		{
			heightAdd = 40;
			
			if (GUI.Button(new Rect(10, 260, 130, 25), "Take Screenshot"))
			{
				_takeScreenshot = true;
			}
			
			if (_screenshot != null)
			{
				GUI.Label(new Rect(192, 262, 198, 20), "Screenshot added.");
				GUI.DrawTexture(new Rect(150, 256, 32, 32), _screenshot);
			}
			else
			{
				GUI.Label(new Rect(150, 262, 240, 20), "Screenshot not added.");
			}
		}
		
		if (GUI.Button(new Rect(10, 260 + heightAdd, 185, 30), "Cancel"))
		{
			_topic = "";
			_details = "";
			_windowType = WindowType.None;
			_screenshot = null;
		}
		if (GUI.Button(new Rect(205, 260 + heightAdd, 185, 30), "Submit"))
		{
			if (_topic.Length > 0 && _details.Length > 0)
			{
				Vector3 target = Vector3.zero;
				if (GA.SettingsGA.TrackTarget != null)
					target = GA.SettingsGA.TrackTarget.position;
				
				GA.API.Quality.NewEvent("Feedback:"+_topic, _details, target.x, target.y, target.z);
				
				_topic = "";
				_details = "";
				_windowType = WindowType.None;
				_screenshot = null;
			}
			else
			{
				// GA_TODO: warning _topic and details must be filled out
			}
		}
    }
	
	void BugWindow(int windowID)
	{
		int heightAdd = 0;
		
		if (_popUpError)
			GUI.Label(new Rect(10, 10, 385, 50), "Oops! It looks like an error just occurred! Please fill out this form with as many details as possible (what you were doing, etc.).");
		else
			GUI.Label(new Rect(10, 15, 380, 50), "Submit bug report");
		
		GUI.Label(new Rect(10, 50, 380, 20), "Topic*");
		GUI.SetNextControlName("TopicField");
		_topic = GUI.TextField(new Rect(10, 70, 380, 20), _topic, 50);
		
		GUI.Label(new Rect(10, 100, 380, 20), "Details*");
		_details = GUI.TextArea(new Rect(10, 120, 380, 130), _details, 400);
		
		if (GuiAllowScreenshot)
		{
			heightAdd = 40;
			
			if (GUI.Button(new Rect(10, 260, 130, 25), "Take Screenshot"))
			{
				_takeScreenshot = true;
			}
			
			if (_screenshot != null)
			{
				GUI.Label(new Rect(192, 262, 198, 20), "Screenshot added.");
				GUI.DrawTexture(new Rect(150, 256, 32, 32), _screenshot);
			}
			else
			{
				GUI.Label(new Rect(150, 262, 240, 20), "Screenshot not added.");
			}
		}
		
		if (GUI.Button(new Rect(10, 260 + heightAdd, 185, 30), "Cancel"))
		{
			_topic = "";
			_details = "";
			_windowType = WindowType.None;
			_popUpError = false;
			_screenshot = null;
		}
		if (GUI.Button(new Rect(205, 260 + heightAdd, 185, 30), "Submit"))
		{
			if (_topic.Length > 0 && _details.Length > 0)
			{
				Vector3 target = Vector3.zero;
				if (GA.SettingsGA.TrackTarget != null)
					target = GA.SettingsGA.TrackTarget.position;
				
				if (_popUpError)
				{
					GA.API.Quality.NewEvent("Crash Report:"+_topic, _details, target);
				}
				else
					GA.API.Quality.NewEvent("Bug Report:"+_topic, _details, target);
				
				_topic = "";
				_details = "";
				_windowType = WindowType.None;
				_popUpError = false;
				_screenshot = null;
			}
			else
			{
				// _topic and details must be filled out
			}
		}
    }
	
	IEnumerator TakeScreenshot()
	{
		// wait for graphics to render
        yield return new WaitForEndOfFrame();
 
        // create a new texture
        _screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        _screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _screenshot.Apply();
		
		_takeScreenshot = false;
	}
	
	#endregion

	
	public void OpenBugGUI()
	{
		_windowType = WindowType.BugWindow;
		_popUpError = true;
		
		if (GuiAllowScreenshot)
			_takeScreenshot = true;
	}
}