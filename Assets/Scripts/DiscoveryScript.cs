using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using AssemblyCSharp;

public class DiscoveryScript : NetworkDiscovery {

	// Game Lobby UI reference is set in Start()
	public GameDiscoveryScreen DiscoveryUI;

	void Start () {
		// register the Discovery UI
		GameObject DiscoveryUIEmpty = GameObject.Find("/DiscoveryUIEmpty");
		Debug.Assert(DiscoveryUIEmpty);
		GameDiscoveryScreen DiscoveryScreenUI = DiscoveryUIEmpty.GetComponent<GameDiscoveryScreen>();
		Debug.Assert(DiscoveryScreenUI);
		this.DiscoveryUI = DiscoveryScreenUI;
	}

	// NetworkManager:host:port:gameName:password?:numPlayers:playerLimit
	// encoded in NetworkManager.updateBroadcastMessage()
	public override void OnReceivedBroadcast(string fromAddress, string data) {
		Debug.Log("fromAddress = " + fromAddress + " and data = " + data);
		// push data to UI
		string[] gameData = data.Split(':');
		int timeStamp = (int)(System.DateTime.Now.Ticks / 10000);
		bool passwordProtected = (gameData[4] == "true") ? true : false;
		int numPlayers;
		int playerLimit;
		if (int.TryParse(gameData[5], out numPlayers) && int.TryParse(gameData[6], out playerLimit)) {
			// update UI with gameInfo
			DiscoveredGameInfo gameInfo = new DiscoveredGameInfo(gameData[1], gameData[2], timeStamp,
				gameData[3], passwordProtected, numPlayers, playerLimit);
			this.DiscoveryUI.addGameInfo(gameInfo);
		} else {
			Debug.LogError("Failed to parse broadcast message with data = " + data);
			Debug.Log(gameData[5]);
			Debug.Log(gameData[6]);
		}
		base.OnReceivedBroadcast(fromAddress, data);
	}

}