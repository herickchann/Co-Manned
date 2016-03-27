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
	public NetworkLobbyPlayer lobbyPlayer;

	private Button[,] teamRoles;

	// wire up network manager
	public GameObject networkManager;

	// Use this for initialization
	void Start () {

		// set up network manager
		networkManager = GameObject.Find("NetworkManager");
		if(networkManager == null){
			Debug.Log ("Error: cannot find network manager");
		}

		RoomName.text = "Room: " + GameManager.instance.gameName;
		// length-1 to account for None
		int numTeams = System.Enum.GetValues(typeof(GameManager.Team)).Length - 1;
		int numRoles = System.Enum.GetValues(typeof(GameManager.Role)).Length - 1;
		this.teamRoles = new Button[numTeams, numRoles];
		// access via this.teamRoles[team, role]
		this.teamRoles[(int)GameManager.Team.Red, (int)GameManager.Role.Pilot] = RedPilot;
		this.teamRoles[(int)GameManager.Team.Red, (int)GameManager.Role.Engineer] = RedEngineer;
		this.teamRoles[(int)GameManager.Team.Blue,(int)GameManager.Role.Pilot] = BluePilot;
		this.teamRoles[(int)GameManager.Team.Blue,(int)GameManager.Role.Engineer] = BlueEngineer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void lockButton(GameManager.Team team, GameManager.Role role) {
		Button selectedButton = this.teamRoles[(int)team,(int)role];
		selectedButton.enabled = false;
	}

	public void unlockButton(GameManager.Team team, GameManager.Role role) {
		Button selectedButton = this.teamRoles[(int)team,(int)role];
		selectedButton.enabled = true;
	}

	// team info interaction for lobby player
	public void selectRedPilot() {
		lobbyPlayer.GetComponent<LobbyPlayerScript>().setTeamInfo (GameManager.Team.Red, GameManager.Role.Pilot);
	}

	public void selectRedEngineer() {
		lobbyPlayer.GetComponent<LobbyPlayerScript>().setTeamInfo (GameManager.Team.Red, GameManager.Role.Engineer);
	}

	public void selectBluePilot() {
		lobbyPlayer.GetComponent<LobbyPlayerScript>().setTeamInfo (GameManager.Team.Blue, GameManager.Role.Pilot);
	}

	public void selectBlueEngineer() {
		lobbyPlayer.GetComponent<LobbyPlayerScript>().setTeamInfo (GameManager.Team.Blue, GameManager.Role.Engineer);
	}

	public void playerReady () {
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
