using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DiscoveryManager : NetworkManager {

	private NewDiscoveryScript discovery;
	private RoomInfoScript roomInfo;

	// Use this for initialization
	void Start () {
		// hook up components
		discovery = GetComponent<NewDiscoveryScript> ();
		roomInfo = GameObject.Find ("RoomInfo").GetComponent<RoomInfoScript>();

		// when client joins the scene, it immediately listens for addresses
		discovery.Initialize ();
		discovery.StartAsClient ();
	}

	// this function is calld assuming the roomInfo has been set
	// transition to the game room scene - the lobby manager will handle whatever is in the roomInfo
	public void startGame() {
		// stop broadcast
		discovery.StopBroadcast ();
		// check that the room info has been assigned (if not, this means the room info has not been set)
		Debug.Assert(roomInfo.role != RoomInfoScript.Role.None, "Room Info Role not set");
		// load the next scene
		SceneManager.LoadScene ("GameRoomScreen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
}
