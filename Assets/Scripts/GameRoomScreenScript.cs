using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameRoomScreenScript : MonoBehaviour {

	// drag and dropped from the editor
	public Text RoomName;
	public Button RedPilot;
	public Button RedEngineer;
	public Button BluePilot;
	public Button BlueEngineer;

	private Button[] teamRoleButtons;
	private string [] roleNameArray;
	public string[] unameArray; // gets push to by lobby player

	// Use this for initialization
	void Start () {
		RoomName.text = "Room: " + GameManager.instance.gameName;

		// access team role buttons by index
		teamRoleButtons = new Button[4];
		teamRoleButtons[0] = RedPilot;
		teamRoleButtons[1] = RedEngineer;
		teamRoleButtons[2] = BluePilot;
		teamRoleButtons[3] = BlueEngineer;

		// the roles to diplay on buttons
		roleNameArray = new string[4];
		roleNameArray[0] = "Red Pilot";
		roleNameArray[1] = "Red Engineer";
		roleNameArray[2] = "Blue Pilot";
		roleNameArray[3] = "Blue Engineer";

		unameArray = new string[4];
		for(int idx = 0; idx < GameRoomSlots.maxPlayers; idx++) {
			unameArray[idx] = "";
		}
	}

	// Update is called once per frame
	void Update () {
		// constantly update the UI based on the state of the game room
		for(int idx = 0; idx < GameRoomSlots.maxPlayers; idx++) {
			Button curButton = this.teamRoleButtons[idx];
			if (unameArray[idx] == "") {
				buttonAvailable(curButton, roleNameArray[idx] + "\n-----");
			} else {
				buttonBooked(curButton, roleNameArray[idx] + "\n" + unameArray[idx]);
			}
		}
	}

	public void buttonAvailable(Button b, string label) {
		b.interactable = true;
		Text btex = b.GetComponentInChildren<Text>();
		btex.text = label;
	}

	public void buttonBooked(Button b, string label) {
		b.interactable = false;
		Text btex = b.GetComponentInChildren<Text>();
		btex.text = label;
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
