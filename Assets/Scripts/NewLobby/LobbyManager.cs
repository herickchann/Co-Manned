using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

	static public LobbyManager s_singleton;

	void Awake(){
	}

	void Start(){
		// singleton
		s_singleton = this;
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

	public override void OnServerDisconnect(NetworkConnection conn) {
		Debug.LogError("player with connectionId " + conn.connectionId.ToString() + " disconnected");
		GameObject GameRoomObj =  GameObject.Find("/GameRoomSlots");
		if (GameRoomObj == null) return; // not in game room, do nothing
		GameRoomSlots gameRoomSlots = GameRoomObj.GetComponent<GameRoomSlots>();
		Debug.Assert(gameRoomSlots, "Game Room not found");
		Debug.Assert(conn.clientOwnedObjects.Count == 1, "Player owns more than one player object?");
		foreach(NetworkInstanceId netiid in conn.clientOwnedObjects) {
			// assume the game room uses netIds as keys
			gameRoomSlots.releaseId(netiid.Value.ToString());
		}
		base.OnServerDisconnect(conn);
	}

	// for users to apply settings from their lobby player object to their in-game player object
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log ("passing information from lobby player to game player");
		var cc = lobbyPlayer.GetComponent<LobbyPlayerScript>();
		var player = gamePlayer.GetComponent<PilotMechController>();
		player.setTeamInfo (cc.team, cc.role);

		return true;
	}
}
