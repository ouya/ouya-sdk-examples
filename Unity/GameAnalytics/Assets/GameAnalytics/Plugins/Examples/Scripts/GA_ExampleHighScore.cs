using UnityEngine;
using System.Collections;

public class GA_ExampleHighScore : MonoBehaviour
{
	public GameObject BallPrefab;
	
	private static bool GAMEOVER = false;
	private static int SCORE = 0;
	private static int HIGHSCORE = 0;
	
	private int _genderIndex;
	private int _birthYear;
	private int _friendCount;
	
	private GUIContent[] genderComboBoxList = new GUIContent[] { new GUIContent("-"), new GUIContent("Female"), new GUIContent("Male") };
	private ComboBox genderComboBoxControl = new ComboBox();
	
	private Vector3 _startingPosition;
	
	void Start()
	{
		_startingPosition = transform.position;
	}
	
	void OnGUI()
	{
		if (GAMEOVER)
		{
			GUI.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 110, 200, 220), HighScoreWindow, "GAME OVER");
		}
			
		GUI.Label(new Rect(10, Screen.height - 70, 200, 20), "SCORE: " + SCORE);
	}
	
	public static void GameOver(Vector3 position)
	{
		if (PlayerPrefs.HasKey("GAExampleScore"))
			HIGHSCORE = PlayerPrefs.GetInt("GAExampleScore", 0);
		
		GAMEOVER = true;
		
		GA.API.Design.NewEvent("GameOver", position);
		
		GA_Queue.ForceSubmit();
	}
	
	public static void AddScore(int score, string scoreName, Vector3 position)
	{
		SCORE += score;
		
		GA.API.Design.NewEvent("Score:" + scoreName, 10, position);
	}
	
	private void HighScoreWindow(int windowID)
	{
		GUILayout.BeginHorizontal();
		if (SCORE > HIGHSCORE)
			GUILayout.Label("OLD HIGHSCORE:");
		else
			GUILayout.Label("HIGHSCORE:");
		GUILayout.Label(HIGHSCORE.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.Space(10);
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("NEW SCORE:");
		GUILayout.Label(SCORE.ToString());
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("BIRTH YEAR:");
		int birthyearParsed = 0;
		if (int.TryParse(GUILayout.TextField(_birthYear!=0?_birthYear.ToString():"", GUILayout.Width(50)), out birthyearParsed))
			_birthYear = birthyearParsed;
		else
			_birthYear = 0;
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("FRIEND COUNT:");
		int friendsParsed = 0;
		if (int.TryParse(GUILayout.TextField(_friendCount!=0?_friendCount.ToString():"", GUILayout.Width(50)), out friendsParsed))
			_friendCount = friendsParsed;
		else
			_friendCount = 0;
		GUILayout.EndHorizontal();
		
		GUILayout.Label("GENDER:");
		_genderIndex = genderComboBoxControl.List(new Rect(115, 130, 75, 20), genderComboBoxList[_genderIndex], genderComboBoxList, GUI.skin.button);
		
		GUILayout.Space(10);
		
		GUI.enabled = SCORE > HIGHSCORE;
		if (GUILayout.Button("SAVE", GUILayout.Width(100)))
		{
			HIGHSCORE = SCORE;
			PlayerPrefs.SetInt("GAExampleScore", SCORE);
			PlayerPrefs.Save();
			
			GA_User.Gender gender = GA_User.Gender.Unknown;
			if (_genderIndex == 1)
				gender = GA_User.Gender.Female;
			else if (_genderIndex == 2)
				gender = GA_User.Gender.Male;
			
			GA.API.User.NewUser(gender, _birthYear, _friendCount);
			GA_Queue.ForceSubmit();
		}
		GUI.enabled = true;
		if (GUILayout.Button("RESTART", GUILayout.Width(100)))
		{
			GAMEOVER = false;
			SCORE = 0;
			
			GameObject ball = Instantiate(BallPrefab, new Vector3(0, 3, 0), Quaternion.identity) as GameObject;
			ball.name = "Ball";
			
			transform.position = _startingPosition;
			rigidbody.velocity = Vector3.zero;
		}
		
		if (!genderComboBoxControl.IsShowingList())
		{
			GUI.DrawTexture(new Rect(126, 156, 55, 55), GA.SettingsGA.Logo);
		}
		
		GUI.FocusWindow(0);
	}
}
