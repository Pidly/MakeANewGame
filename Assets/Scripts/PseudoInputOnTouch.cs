using UnityEngine;
using System.Collections;

public class PseudoInputOnTouch : MonoBehaviour {
	public enum PseudoInputDirection {Left, Right}
	public PseudoInputDirection direction;

	void Touched(){
		if (direction == PseudoInputDirection.Left) {
			PseudoInput.Instance.leftPressed = true;		
		}

		if(direction == PseudoInputDirection.Right){
			PseudoInput.Instance.rightPressed = true;
		}
	}
}
