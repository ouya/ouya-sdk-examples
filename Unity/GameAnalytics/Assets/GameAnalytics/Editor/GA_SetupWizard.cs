using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GA_SetupWizard : EditorWindow
{
	private GUIContent _nextTour				= new GUIContent(">", "Continue the GameAnalytics setup and introduction tour.");
	private GUIContent _prevTour				= new GUIContent("<", "Go back to the previous page.");
	private GUIContent _backToStart				= new GUIContent("Back to page 1", "Go back to the first page.");
	private GUIContent _closeWizard				= new GUIContent("Close Wizard", "Closes the GameAnalytics setup and introduction tour.");
	
	private GUIContent _myHomePage				= new GUIContent("GA account", "Opens your home page on the GameAnalytics website. If you have not created a GameAnalytics account yet you can do that here.");
	private GUIContent _documentationLink		= new GUIContent("Documentation", "Opens the GameAnalytics Unity3D package documentation page in your browser. https://beta.gameanalytics.com/docs/unity3d.html");
	private GUIContent _copyCode				= new GUIContent("Copy Code", "Copy the code to clipboard.");
	private GUIContent _openExampleGame			= new GUIContent("Example Game", "Opens the GameAnalytics example game scene.");
	
	private GUIContent _publicKeyLabel			= new GUIContent("Game Key", "Your GameAnalytics Game Key - copy/paste from the GA website.");
	private GUIContent _privateKeyLabel			= new GUIContent("Secret Key", "Your GameAnalytics Secret Key - copy/paste from the GA website.");
	private GUIContent _apiKeyLabel				= new GUIContent("API Key", "Your GameAnalytics API Key - copy/paste from the GA website.");
	
	private GUIContent _stepOne					= new GUIContent("Create GA account", "Create your GA account and add a game studio and game.");
	private GUIContent _stepTwo					= new GUIContent("Setup GA_Settings", "Copy/paste your Game Key and Secret Key from the website.");
	private GUIContent _stepThree				= new GUIContent("GA_Status overview", "The GA_Status window shows whether or not the settings have been setup correctly, and also shows the number of successful and failed messages sent.");
	private GUIContent _stepFour				= new GUIContent("System tracking", "Use the GA_SystemTracker for tracking system info, errors, performance, etc.");
	private GUIContent _stepFive				= new GUIContent("Object tracking", "Use the GA_Tracker to track predefined events for a game object.");
	private GUIContent _stepSix					= new GUIContent("Custom tracking", "Track your own custom events in the code.");
	private GUIContent _stepSeven				= new GUIContent("Heatmap visualization", "Visualize 3D heatmaps of your data in the Unity3D editor.");
	private GUIContent _stepEight				= new GUIContent("Heatmap histogram", "Explains how the histogram for heatmap datasets works.");
	
	private int _tourStep = 0;
	private int _lastStep = 9;
	
	void OnGUI ()
	{
		switch(_tourStep)
		{
		case 0:
			GUILayout.Label("Thank you for downloading the GameAnalytics Unity3D wrapper", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("By using GameAnalytics you will be able to find out what type of players play your game, how they play, and lots more!", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("This Setup Wizard will help you set up GameAnalytics, and teach you how to track system events, crashes, custom events, etc. You will also learn how to use heatmaps to visualize your data!", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.LabelField("Jump to Setup:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(_stepOne)) { _tourStep = 1; }
			if (GUILayout.Button(_stepTwo)) { _tourStep = 2; }
			if (GUILayout.Button(_stepThree)) { _tourStep = 3; }
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Jump to Tracking:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(_stepFour)) { _tourStep = 4; }
			if (GUILayout.Button(_stepFive)) { _tourStep = 5; }
			if (GUILayout.Button(_stepSix)) { _tourStep = 6; }
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Jump to Heatmaps:", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(_stepSeven)) { _tourStep = 7; }
			if (GUILayout.Button(_stepEight)) { _tourStep = 8; }
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Visit the online documentation:", EditorStyles.boldLabel);
			if (GUILayout.Button(_documentationLink, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			GUILayout.Space(4);
			
			break;
		case 1:
			GUILayout.Label("Step 1: Create your GA account and game", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The first thing you should do is create a GameAnalytics account on the GA website. Open the site using the button below, create and verify your GA account, and then you will be able to both create a game studio and add games to it.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Once you have created your first game on the website you will have what you need to continue this tour. If you have already created a GA account and a game you should skip this step.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("GA account creation website:", EditorStyles.boldLabel);
			if (GUILayout.Button(_myHomePage, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/CreateGameAnalytics");
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(6);
			
			EditorGUILayout.HelpBox("The GA website lets you view all the data you have collected in a multitude of ways. Along with the Design, Quality, Business, and User views, which give you quick overviews of these aspects of your game, the GA site also features the Explore tool, Funnels and Cohorts.\n\nUsing the Explore tool you can visualize a ton of standard matrices, such as DAU (daily average users) or ARPU (average revenue per user), or custom matrices based on your own custom events.\nFunnels give you an overview of how users progress through a sequence of events you define, and Cohorts help you keep track of player retention.", MessageType.Info, true);
			
			GUILayout.Space(4);
			
			break;
		case 2:
			GUILayout.Label("Step 2: Setup GA settings", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Now it's time to copy/paste your Game Key and Secret Key from the website to the fields below. These keys were generated when you created your game on the website. You can also find the keys under the game's settings from your home page.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Your keys are stored in the GA_Settings object, which also contains other useful settings - you can always find it under Window > GameAnalytics > Select GA_Settings.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("GA account home:", EditorStyles.boldLabel);
			if (GUILayout.Button(_myHomePage, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label(_publicKeyLabel, EditorStyles.boldLabel);
			GA.SettingsGA.GameKey = EditorGUILayout.TextField("", GA.SettingsGA.GameKey);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label(_privateKeyLabel, EditorStyles.boldLabel);
			GA.SettingsGA.SecretKey = EditorGUILayout.TextField("", GA.SettingsGA.SecretKey);
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.HelpBox("Your Game Key uniquely identifies your game, so the Unity Package knows where to store your data. Your Secret Key is used for security as message authorization, and makes sure that no-one else gets a hold of your data!", MessageType.Info, true);
			
			GUILayout.Space(31);
			
			break;
		case 3:
			GUILayout.Label("Step 3: The GA_Status Window", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("A quick tip before we start tracking tons of events; the GA_Status window shows whether the GameAnalytics Unity3D Package has been set up correctly. Both the Game Key and Secret Key must be set up to satisfy the GA_Status window. This window is accessible through Window > GameAnalytics > Open GA_Status.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_Status window also shows the status of messages sent to the server. You get an overview of the total number of messages sent successfully, and the total number of messages that failed. The GA_Status window also shows you the number of successful/failed messages for each message category.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("To make sure the GA Package is tracking the events you set up it might be a good idea to keep an eye on this window while you play your game. Remember that messages are clustered and sent to the server at certain intervals (you can set this interval on the GA_Settings object, under the Advanced tab).", EditorStyles.wordWrappedLabel);
			
			GUILayout.Space(46);
			
			break;
		case 4:
			GUILayout.Label("Step 4: The GA_SystemTracker Prefab (Optional)", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The rest of this tour is completely optional - if you got this far you are ready to track events. Now you just need to set up some events to track!", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_SystemTracker is useful for tracking system info, errors, performance, etc. To add it to a scene go to Window > GameAnalytics > Create GA_SystemTracker.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You can add a GA_SystemTracker to each of the scenes you want to track, or you can add a GA_SystemTracker to your first scene. Enable \"Use For Subsequent Levels\", and it will work for any scene that follows.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("It can be very useful to track how many levels players complete, and their completion times. The GA_SystemTracker will help you do this.\n\nThe GA_SystemTracker is also useful for gathering information on your players' system hardware. When you combine this with tracking game performance (average and Critical FPS events), you can get an overview of which system specifications have trouble running your game.\n\nYou can also use the GA_SystemTracker to track exceptions. This feature can generate a lot of data, but it can also be a great way of discovering bugs.", MessageType.Info, true);
			GUILayout.Space(7);
			
			break;
		case 5:
			GUILayout.Label("Step 5: The GA_Tracker Component (Optional)", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("The GA_Tracker component can be added to any game object in a scene from Window > GameAnalytics > Add GA_Tracker to Object. It allows you to easily set up auto-tracking of a bunch of simple, predefined events for that game object.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("The auto-tracked events available through the GA_Tracker component lets you set up some simple tracking for testing, etc. without having to code your own custom events.", MessageType.Info, true);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Using the GA_Tracker you can also enable Track Target for a game object. Some key events used by the GA_SystemTracker will use the location of the game object set up as Track Target - so only one GA_Tracker per scene can have Track Target enabled. Events include Submit Critical FPS, Submit Errors, and GA GUI events.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("You can put a GA_Tracker component on your player or camera object, and enable Track Target. This way you will know the location of the player when a Critical FPS event occurred - there is probably something causing bad performance in that area!", MessageType.Info, true);
			GUILayout.Space(18);
			
			break;
		case 6:
			GUILayout.Label("Step 6: Tracking your own custom events (Optional)", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It is of course possible to track your own custom events, such as when a player kills an enemy, picks up an item, or dies. Here are a few simple copy/paste code examples, but we recommend you check out the online documentation for a complete overview.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.LabelField("Design examples:", EditorStyles.boldLabel);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.SelectableLabel("GA.API.Design.NewEvent(\"Kill:Robot\", transform.position);", EditorStyles.textField, new GUILayoutOption[] { GUILayout.Height(19) });
			if (GUILayout.Button(_copyCode, GUILayout.MaxWidth(82)))
			{
				TextEditor te = new TextEditor();
				te.content = new GUIContent("GA.API.Design.NewEvent(\"Kill:Robot\", transform.position);");
				te.SelectAll();
				te.Copy();
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField("Quality example:", EditorStyles.boldLabel);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.SelectableLabel("GA.API.Quality.NewEvent(\"System:os\");", EditorStyles.textField, new GUILayoutOption[] { GUILayout.Height(19) });
			if (GUILayout.Button(_copyCode, GUILayout.MaxWidth(82)))
			{
				TextEditor te = new TextEditor();
				te.content = new GUIContent("GA.API.Quality.NewEvent(\"System:os\");");
				te.SelectAll();
				te.Copy();
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField("Business example:", EditorStyles.boldLabel);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.SelectableLabel("GA.API.Business.NewEvent(\"Buy:PlayerSkin\", \"USD\", 10);", EditorStyles.textField, new GUILayoutOption[] { GUILayout.Height(19) });
			if (GUILayout.Button(_copyCode, GUILayout.MaxWidth(82)))
			{
				TextEditor te = new TextEditor();
				te.content = new GUIContent("GA.API.Business.NewEvent(\"Buy:PlayerSkin\", \"USD\", 10);");
				te.SelectAll();
				te.Copy();
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField("User example:", EditorStyles.boldLabel);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.SelectableLabel("GA.API.User.NewUser(GA_User.Gender.Male, 1976, 7);", EditorStyles.textField, new GUILayoutOption[] { GUILayout.Height(19) });
			if (GUILayout.Button(_copyCode, GUILayout.MaxWidth(82)))
			{
				TextEditor te = new TextEditor();
				te.content = new GUIContent("GA.API.User.NewUser(GA_User.Gender.Male, 1976, 7);");
				te.SelectAll();
				te.Copy();
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Visit the online documentation:", EditorStyles.boldLabel);
			if (GUILayout.Button(_documentationLink, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/SupportDocu");
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(7);
			
			break;
		case 7:
			GUILayout.Label("Step 7A: 3D Heatmap data visualization (Optional)", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("In order to use the heatmap feature you must first copy/paste your API Key from the website to the field below. You can find the key under your game's settings from your home page. Your API key is stored in the GA_Settings object, under the Advanced tab.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("GA account home:", EditorStyles.boldLabel);
			if (GUILayout.Button(_myHomePage, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
		    GUILayout.Label(_apiKeyLabel, EditorStyles.boldLabel);
			GA.SettingsGA.ApiKey = EditorGUILayout.TextField("", GA.SettingsGA.ApiKey);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(3);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("To add a heatmap object to a scene go to Window > GameAnalytics > Create GA_Heatmap. Heatmaps. This allows you to visualize the data you have collected inside Unity3D.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("To use the GA_Heatmap prefab you first have to get your data from the GA servers. To do this click the Update Index button. This will update the available builds, areas, and events. After you select the appropriate values, which correspond to the event you want to visualize, you simply click the Download Heatmap button. If any data is present for the combination of values you have selected, you should now see a heatmap in your scene.", EditorStyles.wordWrappedLabel);
			
			GUILayout.Space(11);
			
			break;
		case 8:
			GUILayout.Label("Step 7B: 3D Heatmap data histogram (Optional)", EditorStyles.whiteLargeLabel);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("3D heatmaps come with a histogram of the downloaded dataset, which provides an overview of the data, and allows you to filter and focus on a smaller subset using the slider below the histogram.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("All data collected for heatmaps is aggregated into a grid. Each column in the histogram represents a frequency of occurrence for the event in a grid point. The height of the column indicates the number of grid points with this frequency. The column furthest to the left represents the lowest frequency, i.e. the number of grid points where this event only occurred a few times. The column furthest to the right represents the highest frequency.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Histogram scale switches between a linear and logarithmic view of the histogram, making it easier for you to focus on exactly the data you want.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Transparent render model gives a better overview of clustered data, while solid varies the size of each data point depending on the value. The overlay option allows you to view the heatmap through any other scene objects.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Point radius sets the size of each heatmap data point, and Show values will display the values of each data point (this can be bad for performance).", EditorStyles.wordWrappedLabel);
			
			GUILayout.Space(11);
			
			break;
		case 9:
			EditorGUILayout.LabelField("That's it!", EditorStyles.whiteLargeLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Of course, this tour only covered the basics of the Unity3D Package. For a complete overview, as well as examples, please visit our online documentation.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("You might also want to check out the example game included with the GA Package. You can find the scene in the Examples folder.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Once again, thank you for choosing GameAnalytics, and please do not hesitate to use the Support feature on our website (found on left side of the screen when you are logged in) to submit any feedback, or ask any questions you might have.", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Happy tracking!", EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Visit your GA home page:", EditorStyles.boldLabel);
			if (GUILayout.Button(_myHomePage, GUILayout.Width(150)))
			{
				Application.OpenURL("http://easy.gameanalytics.com/LoginGA");
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
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Open GA example game scene:", EditorStyles.boldLabel);
			if (GUILayout.Button(_openExampleGame, GUILayout.Width(150)))
			{
				if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
				{
					EditorApplication.OpenScene("Assets/GameAnalytics/Plugins/Examples/ga_example.unity");
					Close();
				}
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(11);
			
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
	
	public void SetTourStep(int step)
	{
		_tourStep = step;
		MoveToNewStepAction();
	}
	
	private void MoveToNewStepAction()
	{
		if (_tourStep == 2)
		{
			Selection.activeObject = GA.SettingsGA;
			GA.SettingsGA.CurrentInspectorState = GA_Settings.InspectorStates.Basic;
		}
		else if (_tourStep == 7)
		{
			Selection.activeObject = GA.SettingsGA;
			GA.SettingsGA.CurrentInspectorState = GA_Settings.InspectorStates.Pref;
		}
	}
}
