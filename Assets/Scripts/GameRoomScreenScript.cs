using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameRoomScreenScript : MonoBehaviour {

	// drag and dropped from the editor
	public Text RoomName;
	public Button RedPilot;
	public Button RedEngineer;
	public Button BluePilot;
	public Button BlueEngineer;

	private Button[] role = new Button[4];

	// Use this for initialization
	void Start () {
		RoomName.text = "Room: " + GameManager.instance.gameName;
		role[0] = RedPilot;
		role[1] = RedEngineer;
		role[2] = BluePilot;
		role[3] = BlueEngineer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// TODO: re-write more cleanly
	private void excludeAllRoles() {
		foreach (Button button in this.role) {
			button.enabled = true;
			button.image.color = Color.white;
		}
	}

	public void selectRedPilot() {
		excludeAllRoles();
		role[0].enabled = false;
		role[0].image.color = Color.gray;
		GameManager.instance.role = GameManager.Role.RedPilot;
	}

	public void selectRedEngineer() {
		excludeAllRoles();
		role[1].enabled = false;
		role[1].image.color = Color.gray;
		GameManager.instance.role = GameManager.Role.RedEngineer;
	}

	public void selectBluePilot() {
		excludeAllRoles();
		role[2].enabled = false;
		role[2].image.color = Color.gray;
		GameManager.instance.role = GameManager.Role.BluePilot;
	}

	public void selectBlueEngineer() {
		excludeAllRoles();
		role[3].enabled = false;
		role[3].image.color = Color.gray;
		GameManager.instance.role = GameManager.Role.BlueEngineer;
	}

	public void playerReady () {
		GameManager.Role selection = GameManager.instance.role;
		if (selection == GameManager.Role.RedPilot || selection == GameManager.Role.BluePilot) {
			Debug.Log("Loading pilot mode...");
			SceneManager.LoadScene("pilot");
		} else if (selection == GameManager.Role.RedEngineer || selection == GameManager.Role.BlueEngineer) {
			Debug.Log("Loading engineer mode...");
			SceneManager.LoadScene("engineer");
		}
	}

	// returns to lobby screen
	public void backToLobby () {
		GameManager.instance.gameName = "";
		GameManager.instance.gamePass = "";
		SceneManager.LoadScene("GameLobbyScreen");
		// detach from game instance
	}
}
