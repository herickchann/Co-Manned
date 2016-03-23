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

	private void updateButtonUIArray() {
		// reset all buttons to unselected state
		foreach (Button button in this.teamRoles) {
			button.enabled = true;
			button.image.color = Color.white;
		}
		// darken just the user clicked button
		GameManager.Team myTeam = GameManager.instance.teamSelection;
		GameManager.Role myRole = GameManager.instance.roleSelection;
		Button selectedButton = this.teamRoles[(int)myTeam,(int)myRole];
		selectedButton.enabled = false;
		selectedButton.image.color = Color.gray;
	}

	public void setRoleScene (GameManager.Role role){
		if (networkManager != null) {
			string roleScene = (role == GameManager.Role.Engineer)?"engineer":"pilot";
			//networkManager.GetComponent<NetworkManagerScript>().onlineScene = roleScene;
		}
	}

	public void selectRedPilot() {
		GameManager.instance.teamSelection = GameManager.Team.Red;
		GameManager.instance.roleSelection = GameManager.Role.Pilot;
		setRoleScene (GameManager.Role.Pilot);
		updateButtonUIArray();
	}

	public void selectRedEngineer() {
		GameManager.instance.teamSelection = GameManager.Team.Red;
		GameManager.instance.roleSelection = GameManager.Role.Engineer;
		setRoleScene (GameManager.Role.Engineer);
		updateButtonUIArray();
	}

	public void selectBluePilot() {
		GameManager.instance.teamSelection = GameManager.Team.Blue;
		GameManager.instance.roleSelection = GameManager.Role.Pilot;
		setRoleScene (GameManager.Role.Pilot);
		updateButtonUIArray();
	}

	public void selectBlueEngineer() {
		GameManager.instance.teamSelection = GameManager.Team.Blue;
		GameManager.instance.roleSelection = GameManager.Role.Engineer;
		Debug.Log ("selectd blue engineer");
		setRoleScene (GameManager.Role.Engineer);
		updateButtonUIArray();
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
