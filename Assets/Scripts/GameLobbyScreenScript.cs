using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLobbyScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
