using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManagerScript : NetworkManager {

	// keep track of spawn points
    private Transform[] spawnPoints;

	// wire up game manager
	public GameObject gameManager;

	void Awake(){
		gameManager = GameObject.Find ("GameManager");
	}

	// Use this for initialization
	public void Join () {
		StartClient ();
	}

	public void Host () {
		StartHost ();
	}

	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		GameManager.Role role = gameManager.GetComponent<GameManager> ().getRoleSelection ();
		if (role == GameManager.Role.Engineer)
			return;
		
		base.OnClientSceneChanged (conn);
	}
		
	public override void OnServerConnect(NetworkConnection conn) {
		Debug.Log ("player connected");
	}
		
}
