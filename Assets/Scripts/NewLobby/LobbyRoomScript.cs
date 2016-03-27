using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

// object created any lobby player is created
public class LobbyRoomScript : MonoBehaviour {

	// hook room ui to lobbyplayer to let the server know the ui interactions
	public NetworkLobbyPlayer lobbyPlayer;

	// keep track of number of players
	public int playersInLobby = 0;
	Text playerCountText;

	// wire up components here
	void Awake(){
		playerCountText = GameObject.Find ("playersCount").GetComponent<Text> ();
		if (playerCountText == null)
			Debug.LogError ("cannot find lobby player count text");
	}

	// Use this for initialization class fields
	void Start () {
		DontDestroyOnLoad (this);
		playerCountText.text = "0";
	}

	// update text for printing number of players in room
	public void updatePlayerCountText(){
		playerCountText.text = playersInLobby.ToString ();
	}

	public void addPlayerCount(){
		playersInLobby++;
		updatePlayerCountText ();
	}

	// displaying/hiding panel
	public void togglePanel(string panelName){
		Debug.Log ("toggling panel: " + panelName);
		var panel = GameObject.Find (panelName).GetComponent<CanvasGroup>();
		panel.alpha = (panel.alpha == 1)?0:1;
		panel.interactable = !panel.interactable;
		panel.blocksRaycasts = !panel.blocksRaycasts;
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



}
