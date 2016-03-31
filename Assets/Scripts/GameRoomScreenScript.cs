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
	public Button ClearSelection;
	public Button PlayButton;
	public Button LeaveButton;
	public CanvasGroup PilotPanel;
	public CanvasGroup EngineerPanel;
	public Text RoleText;

	private Button[] teamRoleButtons;
	private string [] roleNameArray;
	public string[] unameArray; // gets push to by lobby player

	Color redColour = new Color(1f, 0.125f, 0f, 1f);
	Color blueColour = new Color(0f, 0.5f, 1f, 1f);

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

		updateImage(GameManager.Team.None, GameManager.Role.None);
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

	public void updateImage(GameManager.Team team, GameManager.Role role){
		// show selection
		Color teamColour;
		string teamRoleString = "";
		if (team == GameManager.Team.Red) {
			teamColour = redColour;
			teamRoleString += "Red ";
		} else if (team == GameManager.Team.Blue) {
			teamColour = blueColour;
			teamRoleString += "Blue ";
		} else {
			teamColour = Color.white;
		}

		if (role == GameManager.Role.Pilot) {
			foreach (Image img in PilotPanel.GetComponentsInChildren<Image>()) {
				img.color = teamColour;
			}
			PilotPanel.alpha = 1;
			EngineerPanel.alpha = 0;
			teamRoleString += "Pilot";
		} else if (role == GameManager.Role.Engineer) {
			foreach (Image img in EngineerPanel.GetComponentsInChildren<Image>()) {
				img.color = teamColour;
			}
			PilotPanel.alpha = 0;
			EngineerPanel.alpha = 1;
			teamRoleString += "Engineer";
		} else {
			PilotPanel.alpha = 0;
			EngineerPanel.alpha = 0;
		}

		if (teamRoleString == "") {
			RoleText.text = "Please select a team / role";
		} else {
			RoleText.text = "Playing as " + teamRoleString;
		}
	}

	public void showRedPilot() {
		updateImage(GameManager.Team.Red, GameManager.Role.Pilot);
	}
	public void showRedEngineer() {
		updateImage(GameManager.Team.Red, GameManager.Role.Engineer);
	}
	public void showBluePilot() {
		updateImage(GameManager.Team.Blue, GameManager.Role.Pilot);
	}
	public void showBlueEngineer() {
		updateImage(GameManager.Team.Blue, GameManager.Role.Engineer);
	}
	public void showNone() {
		updateImage(GameManager.Team.None, GameManager.Role.None);
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

}
