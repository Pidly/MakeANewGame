using UnityEngine;
using System.Collections;

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

	[HideInInspector]
	public int previousScore = 0;

	private float score = 0.0f;
	private static float highScore = 0.0f;
	private bool hasSaved = false;
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
		if(Application.loadedLevelName != titleScreenName){
			if (GameObject.FindGameObjectWithTag ("Player") == null) {
				gameOver = true;
			}

			if (gameOver) {
				saveHighScore();
				previousScore = (int)score;


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
	}

	void saveHighScore(){
		PlayerPrefs.SetInt ("HighScore", (int)highScore);
		PlayerPrefs.Save();
	}

	void loadHighScore(){
		highScore = PlayerPrefs.GetInt ("HighScore");
	}

	void OnGUI(){
		if(Application.loadedLevelName != titleScreenName){
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
