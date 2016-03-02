using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ComannedNetworkManager : NetworkManager {

	// Use this for initialization
	void Join () {
		StartClient ();
	}

	// Update is called once per frame
	void Host () {
		StartHost ();
	}
}
