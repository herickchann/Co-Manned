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

	// this function is called assuming that an address is picked
	// pick a location and transition to game room scene
	public void PickedLocation(){
		// pause the broadcast
		discovery.StopBroadcast ();

		// set the room info
		roomInfo.role = RoomInfoScript.Role.Player;
		roomInfo.parseAddressInfo (discovery.getRoom());

		SceneManager.LoadScene ("GameRoomScreen");
	}

	public void CreateRoom(){
		discovery.StopBroadcast ();
		roomInfo.role = RoomInfoScript.Role.Host;
		SceneManager.LoadScene ("GameRoomScreen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
}
