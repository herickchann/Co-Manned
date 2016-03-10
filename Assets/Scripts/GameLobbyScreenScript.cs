using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLobbyScreenScript : MonoBehaviour {

	public Text UserName;
	public CanvasGroup JoinGameMaskPanel;

	void Start () {
		UserName.text = "Username: " + GameManager.instance.userName;
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
