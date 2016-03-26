using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

	// wire up lobby room ui
	LobbyRoomScript lobby;

	static public LobbyManager s_singleton;

	void Awake(){
	}

	void Start(){
		// singleton
		s_singleton = this;

		// set up ui flow here
		//lobby.togglePanel("HostJoinPanel");
	}

	// Use this for initialization
	public void Host(){
		StartHost ();
	}

	public void Join(){
		StartClient ();
	}


	// client stuff
	public override void OnLobbyClientEnter ()
	{
		Debug.Log ("client enterred");
		// hide host here

	}

	public override void OnLobbyClientConnect (NetworkConnection conn)
	{
		Debug.Log ("client connected");
	}

	// for users to apply settings from their lobby player object to their in-game player object
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log ("passing information from lobby player to game player");
		var cc = lobbyPlayer.GetComponent<LobbyPlayerScript>();
		var player = gamePlayer.GetComponent<PilotMechController>();
		player.setTeam (cc.team);
		player.role = cc.role;

		return true;
	}
}
