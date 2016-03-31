using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using AssemblyCSharp;

public class NewDiscoveryScript : NetworkDiscovery {

	public MainRoomUIScript mainRoomUI;

	void Awake(){
		showGUI = false;
	}

	void Start () {
		// register the room's UI so we can push broadcast data to it
		MainRoomUIScript roomUI = GameObject.Find("/UIEmpty").GetComponent<MainRoomUIScript>();
		//Debug.Assert(roomUI); - UI attachment is not neccesary if we are using discovery for broadcasting
		this.mainRoomUI = roomUI;
	}
		
	// Comanned|host|port|gameName|password?|numPlayers|playerLimit
	// encoded in NetworkManager.updateBroadcastMessage()
	public override void OnReceivedBroadcast(string fromAddress, string data) {
		Debug.Log("fromAddress = " + fromAddress + " and data = " + data);
		// push data to UI
		string[] gameData = data.Split('|');
		int timeStamp = (int)(System.DateTime.Now.Ticks / 10000);
		bool passwordProtected = (gameData[4] == "true") ? true : false;
		int portNum;
		int numPlayers;
		int playerLimit;
		if (int.TryParse(gameData[2], out portNum) &&
			int.TryParse(gameData[5], out numPlayers) &&
			int.TryParse(gameData[6], out playerLimit)) {
			// update UI with gameInfo
			DiscoveredGameInfo gameInfo = new DiscoveredGameInfo(fromAddress, portNum, timeStamp,
				gameData[3], passwordProtected, numPlayers, playerLimit);
			this.mainRoomUI.addGameInfo(gameInfo);
		} else {
			Debug.LogError("Failed to parse broadcast message with data = " + data);
			Debug.Log(gameData[2]);
			Debug.Log(gameData[5]);
			Debug.Log(gameData[6]);
		}
		base.OnReceivedBroadcast(fromAddress, data);
	}
}
