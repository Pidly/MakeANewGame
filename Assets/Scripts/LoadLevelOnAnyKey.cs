using UnityEngine;
using System.Collections;

public class LoadLevelOnAnyKey : MonoBehaviour {
	public string levelName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.IsMobile()){
			if(Input.anyKeyDown){
				Application.LoadLevel(levelName);
			}
		}
		else{
			foreach(Touch touch in Input.touches){
				if(touch.phase == TouchPhase.Began){
					Application.LoadLevel(levelName);
				}
			}
		}
	}
}
