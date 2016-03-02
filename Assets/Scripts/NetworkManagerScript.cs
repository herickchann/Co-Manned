using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManagerScript : NetworkManager {

	// Use this for initialization
	public void Join () {
		StartClient ();
	}

	public void Host () {
		StartHost ();
	}

	public override void OnServerConnect(NetworkConnection conn){
		Debug.Log ("player connected");
	}
}
