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

	void Start () {
		discovery.Initialize(); // discovery must be initialized
		discovery.StartAsClient(); // start listening as soon as we enter lobby
	}

	// Use this for initialization
	/*public void Join () {
		//StartClient();
		//discovery.StopBroadcast();
		//discovery.showGUI = false;
	}*/

	/*public void Host () {
		discovery.StartAsServer();
		//StartHost();
	}*/

	private void updateBroadcastMessage() {
	// NetworkManager:host:port:gameName:password?:numPlayers:playerLimit
		// gameName is last in case user input messes with colon delimiter
		string message = string.Format("NetworkManager:{0}:{1}:{2}:{3}:{4}:{5}",
		networkAddress, networkPort.ToString(), this.gameName, passwordRequired, 
			numPlayers.ToString(), playerLimit.ToString());
		discovery.broadcastData = message;
	}

	public void startBroadcast(string gameName, string gamePass) {
		this.gameName = gameName;
		this.gamePass = gamePass;
		this.passwordRequired = (this.gamePass == "" ? "false" : "true");
		this.numPlayers++; // 1 for this player
		updateBroadcastMessage();
		discovery.StartAsServer(); // start broadcasting
		Debug.Log("Broadcasting with gameName: " + gameName + " and password: " + gamePass);
	}

	public override void OnStartHost() {
		Debug.Log("OnStartHost called");
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

	public override void OnClientDisconnect (NetworkConnection conn) {
		Debug.Log ("player disconnected");
		this.numPlayers--;
		updateBroadcastMessage();
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
