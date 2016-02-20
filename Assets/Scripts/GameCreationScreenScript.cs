using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameCreationScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// returns to lobby screen
	public void backToLobby () {
		SceneManager.LoadScene("GameLobbyScreen");
	}

	// makes the game
	public void makeGame () {
		SceneManager.LoadScene("GameRoomScreen");
	}
}
