using UnityEngine;
using System.Collections;

public class SceneHandlerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("loaded back to game room");
		GameObject[] junk = GameObject.FindGameObjectsWithTag ("GameRoomObjects");
		for(int i=0; i<junk.Length; ++i){
			Destroy (junk[i]);
		}
	}

}
