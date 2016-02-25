using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameCreationScreenScript : MonoBehaviour {

	private string gameName;
	private string gamePass;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setGameName (string input) {
		this.gameName = input;
	}

	public void setGamePass (string input) {
		this.gamePass = input;
	}

	// returns to lobby screen
	public void backToLobby () {
		SceneManager.LoadScene("GameLobbyScreen");
	}

	// makes the game
	public void makeGame () {
		GameManager.instance.gameName = this.gameName;
		GameManager.instance.gamePass = this.gamePass;
		SceneManager.LoadScene("GameRoomScreen");
	}
}
