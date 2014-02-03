using UnityEngine;
using System.Collections;

public class GA_ExampleManager : MonoBehaviour
{
	private string _exampleGameKey = "0c3c6e8c8896556a947a1f7c06c5df06";
	private string _exampleSecretKey = "f0e52c8b2422b39c0e3ed4be19792b6d2c81ef0a";
	
	void Start()
	{
		if (!GA.SettingsGA.SendExampleGameDataToMyGame)
		{
			GA.API.Submit.SetupKeys(_exampleGameKey, _exampleSecretKey);
			GA.Log("Changed GameAnalytics Game Key and Secret Key for this example game. To send example game data to your own game enable Get Example Game Data under GA_Settings > Debug");
		}
		else
		{
			GA.Log("Sending example game data to your game. To stop sending example game data to your own game disable Get Example Game Data under GA_Settings > Debug");
		}
	}
	
	void OnGUI()
	{
		if (Time.time < 5)
		{
			if (!GA.SettingsGA.SendExampleGameDataToMyGame)
				GUI.Box(new Rect(Screen.width / 2 - 220, Screen.height / 2 - 12, 440, 24), "Example Game Data will NOT be sent to your game (see log for details).");
			else
				GUI.Box(new Rect(Screen.width / 2 - 220, Screen.height / 2 - 12, 440, 24), "Example Game Data WILL be sent to your game (see log for details).");
		}
	}
}
