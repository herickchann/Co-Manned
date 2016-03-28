using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkLobbyManager {

	static public LobbyManager s_singleton;
	public NetworkDiscovery discovery;

	// current game broadcast details
	public string gameName = "";
	public string gamePass = ""; // just broadcast true/false, not the actual password
	public int curNumPlayers = 0;
	public const int playerLimit = 4;
	public string passwordRequired = "false";

	private string defaultNetworkHost;
	private int defaultNetworkPort;

	void Awake(){
	}

	void Start(){
		// singleton
		s_singleton = this;
		this.defaultNetworkHost = networkAddress;
		this.defaultNetworkPort = networkPort;
	}

	// Use this for initialization
	public void Host(){
		StartHost ();
	}

	public void Join(){
		StartClient ();
	}

	public void startDiscovery() {
		Debug.Log("Starting discovery");
		discovery.Initialize(); // discovery must be initialized
		discovery.StartAsClient(); // start listening as soon as we enter lobby
	}

	private void updateBroadcastMessage() {
		// NetworkManager:host:port:gameName:password?:numPlayers:playerLimit
		// gameName is last in case user input messes with colon delimiter
		string message = string.Format("NetworkManager:{0}:{1}:{2}:{3}:{4}:{5}",
			networkAddress, networkPort.ToString(), this.gameName, passwordRequired, 
			curNumPlayers.ToString(), playerLimit.ToString());
		discovery.broadcastData = message;
	}

	public void initBroadcastMsg() {
		// reset connection info to defaults
		networkAddress = this.defaultNetworkHost;
		networkPort = this.defaultNetworkPort;
		this.passwordRequired = (this.gamePass == "" ? "false" : "true");
		this.curNumPlayers++; // 1 for this player
		updateBroadcastMessage();
		Debug.Log("Will broadcast with gameName: " + gameName + " and password: " + gamePass);
	}

	public void setGameNameAndPass(string gameName, string password) {
		this.gameName = gameName;
		this.gamePass = password;
		Network.incomingPassword = password;
	}

	public void setConnectionInfo(string hostAddr, string portNum) {
		// override defaults as client
		networkAddress = hostAddr;
		int port;
		Debug.Assert(int.TryParse(portNum, out port));
		networkPort = port;
	}

	public override void OnStartHost() {
		Debug.Log("OnStartHost called");
		Debug.Assert(this.gameName != "", "Game name/pass not set - please call setGameNamePass so we can broadcast it");
		discovery.StopBroadcast(); // stop listening for other games (assuming we are already listening)
		discovery.Initialize(); // discovery must be initialized
		initBroadcastMsg();
		discovery.StartAsServer(); // start broadcasting
		base.OnStartHost();
	}

	public override void OnStopHost() {
		Debug.Log("OnStopHost called");
		discovery.StopBroadcast();
		this.gameName = "";
		this.gamePass = "";
		base.OnStopHost();
	}

	public override void OnServerConnect (NetworkConnection conn) {
		Debug.Log ("player connected");
		this.curNumPlayers++;
		updateBroadcastMessage();
		base.OnServerConnect(conn);
	}

	public override void OnServerDisconnect (NetworkConnection conn) {
		Debug.Log ("player disconnected");
		this.curNumPlayers--;
		updateBroadcastMessage();
		base.OnServerDisconnect(conn);
	}

	// client stuff
	public override void OnLobbyClientEnter ()
	{
		Debug.Log ("client enterred");
		// hide host here
		base.OnLobbyClientEnter();
	}

	public override void OnLobbyClientConnect (NetworkConnection conn)
	{
		Debug.Log ("client connected");
		base.OnLobbyClientConnect(conn);	
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
