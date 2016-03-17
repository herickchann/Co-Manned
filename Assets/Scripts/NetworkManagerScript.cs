using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManagerScript : NetworkManager {

	public NetworkDiscovery discovery;

	// current game broadcast details
	public string gameName = "";
	public string gamePass = ""; // just broadcast true/false, not the actual password
	public int numPlayers = 0;
	public const int playerLimit = 4;
	public string passwordRequired = "false";

	private string defaultNetworkHost;
	private int defaultNetworkPort;

	void Start () {
		this.defaultNetworkHost = networkAddress;
		this.defaultNetworkPort = networkPort;
	}

	public void enterLobby() {
		discovery.Initialize(); // discovery must be initialized
		discovery.StartAsClient(); // start listening as soon as we enter lobby
	}

	private void updateBroadcastMessage() {
	// NetworkManager:host:port:gameName:password?:numPlayers:playerLimit
		// gameName is last in case user input messes with colon delimiter
		string message = string.Format("NetworkManager:{0}:{1}:{2}:{3}:{4}:{5}",
			networkAddress, networkPort.ToString(), this.gameName, passwordRequired, 
			numPlayers.ToString(), playerLimit.ToString());
		discovery.broadcastData = message;
	}

	public void initBroadcastMsg() {
		// reset connection info to defaults
		networkAddress = this.defaultNetworkHost;
		networkPort = this.defaultNetworkPort;
		this.passwordRequired = (this.gamePass == "" ? "false" : "true");
		this.numPlayers++; // 1 for this player
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
		Debug.Assert(this.gameName != "", "Game name not set - please call setGameNamePass");
		discovery.StopBroadcast(); // stop listening for other games (assuming we are already listening)
		discovery.Initialize(); // discovery must be initialized
		initBroadcastMsg();
		discovery.StartAsServer(); // start broadcasting
		maxConnections = playerLimit;
		base.OnStartHost();
	}

	public override void OnStopHost() {
		Debug.Log("OnStopHost called");
		discovery.StopBroadcast();
		this.gameName = "";
		this.gamePass = "";
		base.OnStopHost();
	}

	public override void OnStartClient(NetworkClient client) {
		Debug.Log("OnStartClient called");
	}

	public override void OnStopClient() {
		Debug.Log("OnStopClient called");
	}

	public override void OnServerConnect(NetworkConnection conn) {
		Debug.Log ("player connected");
		this.numPlayers++;
		updateBroadcastMessage();
		base.OnServerConnect(conn);
	}

	public override void OnServerDisconnect (NetworkConnection conn) {
		Debug.Log ("player disconnected");
		this.numPlayers--;
		updateBroadcastMessage();
		base.OnServerDisconnect(conn);
	}

	public override void OnClientDisconnect(NetworkConnection conn) {
		// load offline scene
		base.OnClientDisconnect(conn);
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
        var player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		Debug.Log("Player controller id = " + playerControllerId.ToString());
        /*//Setup Camera
        GameObject camera = GameObject.FindWithTag("MainCamera");
        if (camera != null) {
            PilotCamera followScript = camera.GetComponent("PilotCamera") as PilotCamera;

            if (followScript != null) {
                followScript.player = player;
            }
        }*/
		Debug.Log("player added");
    }
}
