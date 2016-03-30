using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour {

	public float gearRotationSpeed = 50f;
	public Transform gearTransform;
	private string username = "";

	void Update () {
		gearTransform.Rotate(Vector3.back, gearRotationSpeed * Time.deltaTime);
	}

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
		SceneManager.LoadScene("NewMain");
	}

	public void setUsername (string input) {
		//TODO: enforce string length?
		this.username = input;
	}
}
