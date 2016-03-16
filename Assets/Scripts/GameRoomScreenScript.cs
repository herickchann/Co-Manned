using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameRoomScreenScript : NetworkBehaviour {

	// drag and dropped from the editor
	public Text RoomName;
	// all these are just held here for the GameRoomPlayer who updates the buttons
	public Button RPButton; // red pilot
	public Button REButton; // red engineer
	public Button BPButton; // blue pilot
	public Button BEButton; // blue engineer
	// Launch / Ready button ?
	// store usernames to be rendered in the UI
	public string RPuname = "";
	public string REuname = "";
	public string BPuname = "";
	public string BEuname = "";

	// Use this for initialization
	void Start () {
		RoomName.text = "Room: " + GameManager.instance.gameName;
		Debug.Log("Game Room created");
	}
	
	// Update is called once per frame
	void Update () {
		// the buttons are constantly refreshed incase another user has booked a button
		// until more abstraction needed, we will hardcode UI updates
		// red pilot
		if (this.RPuname != "") {
			buttonBooked(RPButton, "Red Pilot\n" + this.RPuname);
		} else {
			buttonAvailable(RPButton, "Red Pilot\n---");
		}
		// red engineer
		if (this.REuname != "") {
			buttonBooked(REButton, "Red Engineer\n" + this.REuname);
		} else {
			buttonAvailable(REButton, "Red Engineer\n---");
		}
		// blue pilot
		if (this.BPuname != "") {
			buttonBooked(BPButton, "Blue Pilot\n" + this.BPuname);
		} else {
			buttonAvailable(BPButton, "Blue Pilot\n---");
		}
		// blue engineer
		if (this.BEuname != "") {
			buttonBooked(BEButton, "Blue Engineer\n" + this.BEuname);
		} else {
			buttonAvailable(BEButton, "Blue Engineer\n---");
		}
	}

	public void buttonAvailable(Button b, string label) {
		b.enabled = true;
		b.image.color = Color.white;
		Text btex = b.GetComponentInChildren<Text>();
		btex.text = label;
	}

	public void buttonBooked(Button b, string label) {
		b.enabled = false;
		b.image.color = Color.gray;
		Text btex = b.GetComponentInChildren<Text>();
		btex.text = label;
	}

	public void playerReady () {
		// if not ready nothing happens
		GameManager.Role myRole = GameManager.instance.roleSelection;
		if (myRole == GameManager.Role.Pilot) {
			Debug.Log("Loading pilot mode...");
			SceneManager.LoadScene("pilot");
		} else if (myRole == GameManager.Role.Engineer) {
			Debug.Log("Loading engineer mode...");
			SceneManager.LoadScene("engineer");
		} // None selection does nothing
	}

	// returns to lobby screen
	public void backToLobby () {
		GameManager.instance.gameName = "";
		GameManager.instance.gamePass = "";
		GameManager.instance.teamSelection = GameManager.Team.None;
		GameManager.instance.roleSelection = GameManager.Role.None;
		SceneManager.LoadScene("GameLobbyScreen");
		// detach from game instance
	}
}
