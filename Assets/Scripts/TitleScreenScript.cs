using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour {

	private string username = "";

	// loads the lobby screen
	public void joinLobby () {
		// gen random user name if none is given
		if (this.username == "") {
			Random.seed = (int)System.DateTime.Now.Ticks;
			int randNum = Random.Range(0, 9999);
			this.username = "user" + randNum.ToString("0000");
		}
		// do not commit username until they hit the join lobby button
		GameManager.instance.userName = this.username;
		SceneManager.LoadScene("GameLobbyScreen");
	}

	public void setUsername (string input) {
		//TODO: enforce string length?
		this.username = input;
	}
}
