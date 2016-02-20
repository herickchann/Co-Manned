using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour {
	
	// loads the lobby screen
	public void joinLobby () {
		SceneManager.LoadScene("GameLobbyScreen");
	}
}
