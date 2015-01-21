using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance{
		get{
			if(_instance == null){
				GameObject gameManager = new GameObject("GameManager");
				_instance = gameManager.AddComponent<GameManager>();
			}
			return _instance;	
		}
	}
	private static GameManager _instance;

	public float pointsPerUnitTravelled = 1.0f;
	public float gameSpeed = 10.0f;
	public string titleScreenName = "TitleScreen";
	public string highScoresScreenName = "HighScores";

	private bool hasSaved = false;

	[HideInInspector]
	public int previousScore = 0;

	private float score = 0.0f;
	private static float highScore = 0.0f;

	private List<int> highScorez = new List<int>();

	private bool gameOver = false;

	// Use this for initialization
	void Start () {
		if (_instance != this) {
			if (_instance == null) {
				_instance = this;
			} else {
				Destroy (gameObject);		
			}
		}
		loadHighScore();
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName != titleScreenName && Application.loadedLevelName != highScoresScreenName){
			if (GameObject.FindGameObjectWithTag ("Player") == null) {
				gameOver = true;
			}

			if (gameOver) {
				if(!hasSaved)
				{
					saveHighScore();
					previousScore = (int)score;
					hasSaved = true;
				}

				if(!IsMobile()){
					if(Input.anyKeyDown){
						Application.LoadLevel(titleScreenName);
					}
				}
				else{
					foreach(Touch touch in Input.touches){
						if(touch.phase == TouchPhase.Began){
							Application.LoadLevel(titleScreenName);
						}
					}
				}
			}
			if (!gameOver) {
				score += pointsPerUnitTravelled * gameSpeed * Time.deltaTime;
				if(score > highScore) {
					highScore = score;			
				}
			}
		}
		else{
			resetGame();
			//reset shit for the next game.
		}
	}

	void resetGame(){
		score = 0.0f;
		gameOver = false;
		hasSaved = false;
	}

	void saveHighScore(){
		highScorez.Add((int)this.score);
		highScorez.Sort();

		Debug.Log ("High Score: " + (int)this.score);
		Debug.Log ("Full score list:");
		foreach (int currentScore in highScorez){
			Debug.Log("Score: " + currentScore);
		}
		//int highSlot = -1;
		int scoreIndex = 0;
		for(int i = highScorez.Count-1; i > 0; i--) {
			//Debug.Log("Scores: " + highScorez[i]);
			PlayerPrefs.SetInt("HighScore"+scoreIndex.ToString(), (int)(highScorez[i]));
			scoreIndex++;
		}

		while (highScorez.Count > 5) {
			highScorez.RemoveAt(0);		
		}
		/*
		if (highScorez.Count > 5) {
			int difference = highScorez.Count - 5;
			highScorez.RemoveRange(5, difference);
		}
		*/

		PlayerPrefs.Save();
	}

	void loadHighScore(){
		int count = 0;
		while (highScorez.Count < 5) {
			highScorez.Add(0);	
			count++;
			if(count >= 5)
				break;
		}

		for(int i = 0; i < highScorez.Count; i++) {
			highScorez[i] = PlayerPrefs.GetInt("HighScore"+i.ToString());
			//highScorez.Insert(i, (PlayerPrefs.GetInt("HighScore"+i.ToString())));
		}
	}

	void OnGUI(){
		if(Application.loadedLevelName != titleScreenName && Application.loadedLevelName != highScoresScreenName){
			int currentHighScore = (int)highScore;

			GUILayout.Label ("Score: " + ((int)score).ToString());
			GUILayout.Label ("Highscore: " + currentHighScore.ToString ());

			if (gameOver) {
				GUILayout.Label("Game Over! Press any key to quit!");		
			}
		}
	}

	public static bool IsMobile(){
		if(Application.platform == RuntimePlatform.IPhonePlayer ||
		   Application.platform == RuntimePlatform.Android ||
		   Application.platform == RuntimePlatform.BlackBerryPlayer ||
		   Application.platform == RuntimePlatform.MetroPlayerARM){
			return true;
		}
		else{
			return false;
		}
	}
}
