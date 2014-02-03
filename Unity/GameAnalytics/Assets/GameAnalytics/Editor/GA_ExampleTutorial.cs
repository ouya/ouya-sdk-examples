using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_ExampleTutorial : EditorWindow
{
	private GUIContent _nextTour				= new GUIContent(">", "Continue the GameAnalytics example tutorial.");
	private GUIContent _prevTour				= new GUIContent("<", "Go back to the previous page.");
	private GUIContent _backToStart				= new GUIContent("Back to page 1", "Go back to the first page.");
	private GUIContent _closeWizard				= new GUIContent("Close Tutorial", "Closes the GameAnalytics example tutorial tour.");
	
	private GUIContent _documentationLink		= new GUIContent("Documentation", "Opens the GameAnalytics Unity3D package documentation page in your browser. https://beta.gameanalytics.com/docs/unity3d.html");
	private GUIContent _setupWizard				= new GUIContent("GA Setup Wizard", "Opens the GameAnalytics Setup Wizard, which will guide you through the process of setting up the GA package.");
	
	private int _tourStep = 0;
	private int _lastStep = 6;
	
	void OnGUI ()
	{
		switch(_tourStep)
		{
		case 0:
			GUILayout.Label("GameAnalytics Tutorial - Getting started!", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("This tutorial will show you examples of how each feature of the GameAnalytics Package can be used in a simple game.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The example game here is a very simple \"pong\" variant, where the player must prevent the ball from hitting the bottom of the level.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The example game uses a special Game Key and Secret Key to prevent data collected from cluttering your game. To use GameAnalytics in your own scene you will have to set up these keys manually (run the GameAnalytics Setup Wizard for details).", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Press the play button and give the game a quick try before you continue!", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("If you need more information, such as instructions on how to set up the GA Package, we recommend you check out the GA Setup Wizard (Window > GameAnalytics > GA Setup Wizard), and the online documentation.", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Visit the online documentation:", EditorStyles.boldLabel);
			if (GUILayout.Button(_documentationLink, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
			}
			EditorGUILayout.EndHorizontal();
			
			break;
		case 1:
			GUILayout.Label("The GA_SystemTracker", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("We have added a GA_SystemTracker to the scene. You can do this by going to Window > GameAnalytics > Create GA_SystemTracker.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("We use the GA_SystemTracker to automatically track the level start, average and Critical FPS events and any errors and exceptions which might occur, as well as handling the GA GUI, which will allow users to submit feedback and bug reports.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can learn more about the GA_SystemTracker from the GA Setup Wizard or by visiting the online documentation.", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard - GA_SystemTracker:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
				wizard.SetTourStep(4);
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(106);
			
			break;
		case 2:
			GUILayout.Label("The GA_Tracker", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("We have added a GA_Tracker component to the Ball object in the scene. You can add this by going to Window > GameAnalytics > Add GA_Tracker to Object.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_Tracker is used to automatically track collision for the ball. Whenever the ball collides with anything in the scene a simple collision event is tracked. The GA_Tracker component can be used to automatically track a number of predefined events for the object this way.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_Tracker also sets the ball to Track Target - this means that events from the GA_SystemTracker, such as critical FPS and errors, will use the ball's transform position. This way a heatmap may be generated showing where these types of events occurred.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can learn more about the GA_Tracker from the GA Setup Wizard or by visiting the online documentation.", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard - GA_Tracker:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
				wizard.SetTourStep(5);
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(30);
			
			break;
		case 3:
			GUILayout.Label("Custom events", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("A number of GameAnalytics custom events have been defined for this small game. Custom events basically allow you to track anything you want! For convenience we have gathered all the custom events we have used in the GA_ExampleHighScore script.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The AddScore method adds a GA design event \"Score:scoreName\" whenever it collides with the ball.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GameOver method adds a GA design event \"GameOver\" whenever the ball collides with the ground section.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The HighScoreWindow method adds a GA user event when the user submits his new high score.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can learn more about using custom events from the GA Setup Wizard or by visiting the online documentation.", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard - Custom events:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
				wizard.SetTourStep(6);
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(32);
			
			break;
		case 4:
			GUILayout.Label("The GA_Status Window", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("If you have tried out the example game you can use the GA_Status Window to get an overview of the events collected and sent as message to our servers.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_Status Window can be opened through Window > GameAnalytics > Open GA_Status Window.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can learn more about the GA_Status Window from the GA Setup Wizard or by visiting the online documentation.", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard - GA_Status Window:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
				wizard.SetTourStep(3);
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(119);
			
			break;
		case 5:
			GUILayout.Label("Heatmaps", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("We put a GA_Heatmap object with previously collected data in the example scene. The \"Show heatmap\" option has now been enabled to show you the heatmap data (you can disabled it at any time from the GA_Heat Map Renderer inspector). Heatmap objects can be added from the menu Window > GameAnalytics > Create GA_Heatmap.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Heatmaps provide a 3D visualization of events, giving you an overview of where stuff is happening in your levels.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_Heatmap object in this scene shows data for the ball collision event automatically tracked by the GA_Tracker component.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can learn more about the heatmaps from the GA Setup Wizard or by visiting the online documentation.", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard - Heatmaps:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
				wizard.SetTourStep(7);
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(56);
			
			break;
		case 6:
			GUILayout.Label("Achievement earned: Tutorial Complete!", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("That's it for the example tutorial. You might want to check out the GA Setup Wizard if you haven't done so already (Window > GameAnalytics > GA Setup Wizard).", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can also find tons of detailed information and examples in our online documentation. Go check it out!", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA Setup Wizard:", EditorStyles.boldLabel);
			if (GUILayout.Button(_setupWizard, GUILayout.Width(150)))
			{
				GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard>();
				wizard.ShowUtility();
				wizard.position = new Rect(150, 150, 420, 340);
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Visit the online documentation:", EditorStyles.boldLabel);
			if (GUILayout.Button(_documentationLink, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(126);
			
			break;
		}
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		if (_tourStep > 0 && GUILayout.Button(_backToStart))
		{
			_tourStep = 0;
		}
		if (_tourStep > 0 && GUILayout.Button(_prevTour))
		{
			_tourStep--;
			MoveToNewStepAction();
		}
		if ((_tourStep == 0 || _tourStep == _lastStep) && GUILayout.Button(_closeWizard))
		{
			Close();
		}
		if (_tourStep < _lastStep && GUILayout.Button(_nextTour))
		{
			_tourStep++;
			MoveToNewStepAction();
		}
		EditorGUILayout.EndHorizontal();
    }
	
	private void MoveToNewStepAction()
	{
		if (Application.loadedLevelName.Equals("ga_example"))
		{
			if (_tourStep == 1)
			{
				Selection.activeObject = GameObject.Find("GA_SystemTracker");
			}
			else if (_tourStep == 2)
			{
				Selection.activeObject = GameObject.Find("Ball");
			}
			else if (_tourStep == 5)
			{
				GameObject hm = GameObject.Find("GA_HeatMap_Ball_Collisions");
				Selection.activeObject = hm;
				
				if (hm.GetComponent<GA_HeatMapRenderer>().BillBoard != null)
					hm.GetComponent<GA_HeatMapRenderer>().BillBoard.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}
}