using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameRoomScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playerReady () {
		Debug.Log("Player signalled they are ready");
	}

	// returns to lobby screen
	public void backToLobby () {
		SceneManager.LoadScene("GameLobbyScreen");
		// detach from game instance
	}
}
