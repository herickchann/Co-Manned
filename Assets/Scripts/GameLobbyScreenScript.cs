using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLobbyScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// selected game
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void selectGame () {

	}

	// loads the game creation screen
	public void createGame () {
		SceneManager.LoadScene("GameCreationScreen");
	}

	// joins the selected game
	public void joinGame() {
		Debug.Log("Join Game Button pressed.");
	}
}
