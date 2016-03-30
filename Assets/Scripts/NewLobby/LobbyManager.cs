using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

	static public LobbyManager s_singleton;
	private NewDiscoveryScript discovery;
	RoomInfoScript roomInfo;
	// default address and port ensures we will not be hosting with the wrong info after being a client
	private string defaultAddress;
	private int defaultPort;
	public string gameName = "";
	public string gamePass = ""; // just broadcast true/false, not the actual password
	public int curNumPlayers = 0;
	public const int playerLimit = 4;
	public string passwordRequired = "false";

	void Awake(){
		discovery = GetComponent<NewDiscoveryScript> ();
	}

	void Start(){
		// singleton
		s_singleton = this;

		this.defaultAddress = this.networkAddress;
		this.defaultPort = this.networkPort;

		roomInfo = GameObject.Find ("RoomInfo").GetComponent<RoomInfoScript>();
		Debug.Assert(roomInfo, "RoomeInfo not found");
		if(roomInfo.role == RoomInfoScript.Role.Host){
			this.networkAddress = this.defaultAddress;
			this.networkPort = this.defaultPort;
			this.curNumPlayers = 1; // 1 for the host's player
			this.gameName = roomInfo.gamename;
			this.gamePass = roomInfo.password;
			this.passwordRequired = (this.gamePass == "" ? "false" : "true");
			Debug.LogError("broadcasting game " + this.gameName);
			this.Host ();
		}
		else if(roomInfo.role == RoomInfoScript.Role.Player){
			this.networkAddress = roomInfo.address;
			this.networkPort = roomInfo.port;
			this.Join ();
		}

	}

	// Use this for initialization
	public void Host(){
		StartHost ();
	}

	public void Join(){
		StartClient ();
	}

	private void updateBroadcastMessage() {
		// NetworkManager|host|port|gameName|password?|numPlayers|playerLimit
		// gameName is last in case user input messes with colon delimiter
		string message = string.Format("Comanned|{0}|{1}|{2}|{3}|{4}|{5}",
			networkAddress, networkPort.ToString(), this.gameName, passwordRequired, 
			curNumPlayers.ToString(), playerLimit.ToString());
		discovery.broadcastData = message;
	}

	public override void OnStartHost ()
	{
		discovery.Initialize ();
		//string addressInfo = string.Format("{0}:{1}", this.networkAddress, this.networkPort.ToString()); 
		updateBroadcastMessage();
		discovery.StartAsServer ();
	}

//	public override void OnStartClient(NetworkClient client){
//		discovery.Initialize ();
//		discovery.StartAsClient ();
//		// TODO: hide UI stuff here
//	}

	public override void OnStopHost(){
		discovery.StopBroadcast ();
		this.gameName = "";
		this.gamePass = "";
		base.OnStopHost();
	}
//	public override void OnStopClient()
//	{
//		discovery.StopBroadcast();
//		// TODO: show ui stuff here
//	}

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

	public override void OnServerConnect(NetworkConnection conn) {
		Debug.Log ("a player has connected");
		this.curNumPlayers++;
		updateBroadcastMessage();
		base.OnServerConnect(conn);
	}

	public override void OnServerDisconnect(NetworkConnection conn) {
		Debug.LogError("player with connectionId " + conn.connectionId.ToString() + " disconnected");
		// update the room count
		this.curNumPlayers--;
		updateBroadcastMessage();

		// update the room slots
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
