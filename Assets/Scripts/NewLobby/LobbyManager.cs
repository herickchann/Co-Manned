using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkLobbyManager {

	static public LobbyManager s_singleton;
	private NewDiscoveryScript discovery;
	RoomInfoScript roomInfo;
	// default address and port ensures we will not be hosting with the wrong info after being a client
	private string defaultAddress;
	private int defaultPort;
	public string gameName = "";
	public string gamePass = ""; // just broadcast true/false, not the actual password
	public const int playerLimit = 4;
	public string passwordRequired = "false";
    public GameObject redMech;
    public GameObject blueMech;
    public GameObject redEng;
    public GameObject blueEng;

	public bool isFirstMatch = true; // a flag to prevent duplicate connections

	public NetworkInstanceId globalDataId;

	void Awake(){
		
		if(Application.platform==RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.platform==RuntimePlatform.tvOS)
		{
			runInBackground=false;
		}

		if(s_singleton == null){
			s_singleton = this;
		}
		else{
			Destroy(gameObject);
		}
	}

	void Start(){

		discovery = GetComponent<NewDiscoveryScript> ();
		 
		// singleton
		s_singleton = this;

		this.defaultAddress = this.networkAddress;
		this.defaultPort = this.networkPort;

		roomInfo = GameObject.Find ("RoomInfo").GetComponent<RoomInfoScript>();
		Debug.Assert(roomInfo, "RoomeInfo not found");
		//Debug.LogError("isFirstMatch = " + isFirstMatch.ToString());
		if (isFirstMatch) { // would use isNetworkActive, but that does not work
			if(roomInfo.role == RoomInfoScript.Role.Host){
				this.networkAddress = this.defaultAddress;
				this.networkPort = this.defaultPort;
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
			isFirstMatch = false;
		} else {
			Debug.LogError("Not the first match - using existing connection");
		}
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) { // for dev purposes, hitting space will start the game
			ServerChangeScene("pilot");
		}
		if (Input.GetKeyDown(KeyCode.Return)) {
			ServerChangeScene("ResultScreen");
		}
		if (Input.GetKeyDown(KeyCode.Backspace)) {
			ServerChangeScene("GameRoomScreen");
		}
	}

	// Use this for initialization
	public void Host(){
		if (isNetworkActive) {
			Debug.LogError("Network is active");
			return;
		}
		StartHost ();
	}

	public void Join(){
		if (isNetworkActive) return;
		StartClient ();
	}

	public void startGame() {
		discovery.StopBroadcast();
		//CheckReadyToBegin(); // starts if all clients are ready - does not work
		ServerChangeScene("pilot");
	}

	public void endGame() {
		ServerChangeScene("ResultScreen");
	}

	private void updateBroadcastMessage() {
		// NetworkManager|host|port|gameName|password?|numPlayers|playerLimit
		// gameName is last in case user input messes with colon delimiter
		string message = string.Format("Comanned|{0}|{1}|{2}|{3}|{4}|{5}",
			networkAddress, networkPort.ToString(), this.gameName, passwordRequired, 
			Network.connections.Length.ToString(), playerLimit.ToString());
		discovery.broadcastData = message;
	}

	public override void OnStartHost ()
	{
		Debug.LogError("OnStartHost was called");
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
		if(discovery.running){
			discovery.StopBroadcast ();
		}
			
		this.gameName = "";
		this.gamePass = "";
		//base.OnStopHost();
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

		Debug.Log ("spawn objects called");
		NetworkServer.SpawnObjects ();
	}

	public override void OnLobbyClientConnect (NetworkConnection conn)
	{
		Debug.Log ("client connected");
	}

	public override void OnServerConnect(NetworkConnection conn) {
		Debug.Log ("a player has connected");
		//updateBroadcastMessage();
		base.OnServerConnect(conn);
	}

	public override void OnServerDisconnect(NetworkConnection conn) {
		Debug.LogError("player with connectionId " + conn.connectionId.ToString() + " disconnected");
		//updateBroadcastMessage();

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
    public override bool OnLobbyServerSceneLoadedForPlayer (GameObject lobbyPlayer, GameObject gamePlayer) {
		//Debug.LogError("Active Scene = " + SceneManager.GetActiveScene().name);
        Debug.Log("passing information from lobby player to game player");
        var cc = lobbyPlayer.GetComponent<LobbyPlayerScript>();
        if (cc.role == GameManager.Role.Pilot) { 
            var player = gamePlayer.GetComponent<PilotMechController>();
            player.setTeamInfo(cc.team, cc.role);
        } else if(cc.role == GameManager.Role.Engineer) {
            var player = gamePlayer.GetComponent<MechBehaviour>();
            player.setTeamInfo(cc.team, cc.role);
        }
        return true;
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        var startPos = startPositions;
        int rand = Random.Range(0, startPos.Count);
        Transform pos = startPos[rand];
        GameObject newGamePlayer = null;
        GameManager.Team team = GameManager.Team.None;
        GameManager.Role role = GameManager.Role.None;
        
        foreach(NetworkInstanceId netid in conn.clientOwnedObjects) {
            GameObject gameRoomSlots = GameObject.Find("GameRoomSlots");
			gameRoomSlots.GetComponent<GameRoomSlots>().lookupTeamRole(netid.Value.ToString(), out team, out role);
            Debug.LogError(netid.Value.ToString() + " " + team + " " + role);
		}

        if (GameManager.teamString(team) == "red" && GameManager.roleString(role) == "pilot") {
            newGamePlayer = (GameObject) Instantiate(redMech, pos.position, pos.rotation);
        } else if (GameManager.teamString(team) == "blue" && GameManager.roleString(role) == "pilot") {
            newGamePlayer = (GameObject) Instantiate(blueMech, pos.position, pos.rotation);
        } else if (GameManager.teamString(team) == "red" && GameManager.roleString(role) == "engineer") { 
            newGamePlayer = (GameObject) Instantiate(redEng, new Vector3(-200, -202, -200), Quaternion.identity);
        } else if (GameManager.teamString(team) == "blue" && GameManager.roleString(role) == "engineer") { 
            newGamePlayer = (GameObject) Instantiate(blueEng, new Vector3(200, -202, 200), Quaternion.identity);
        } else {
            Debug.LogError("GTFO");
        }
        return newGamePlayer;
    }
}
