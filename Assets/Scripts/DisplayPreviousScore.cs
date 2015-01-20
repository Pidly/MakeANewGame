using UnityEngine;
using System.Collections;

public class DisplayPreviousScore : MonoBehaviour {
	public string label;

	// Use this for initialization
	void Start () {
		TextMesh textMesh = GetComponent<TextMesh>();
		label = textMesh.text;
		textMesh.text = label + GameManager.Instance.previousScore;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
