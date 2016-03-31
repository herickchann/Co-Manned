using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


// this is where we will keep track information of all players
public class GlobalDataHook : NetworkBehaviour {

	public GlobalDataController globalData;

	void Start(){
	}

	// returning specific param based on team
	public int getParam(GameManager.Team team, GlobalDataController.Param param){
		LobbyManager manager = GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ();
		if (isServer) {
			globalData = NetworkServer.FindLocalObject (manager.globalDataId).GetComponent<GlobalDataController>();
		} else {
			globalData = ClientScene.FindLocalObject (manager.globalDataId).GetComponent<GlobalDataController>();
		}

		return globalData.getParam (team, param);	
	}
		
	// set param to the input amount for specific team
	[Command]
	public void CmdSetParam(GameManager.Team team, GlobalDataController.Param param, int amount){
		LobbyManager manager = GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ();
		globalData = NetworkServer.FindLocalObject (manager.globalDataId).GetComponent<GlobalDataController>();
		globalData.setParam (team, param, amount);
	}

	public void setParam(GameManager.Team team, GlobalDataController.Param param, int amount){
		LobbyManager manager = GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ();
		if (isServer) {
			globalData = NetworkServer.FindLocalObject (manager.globalDataId).GetComponent<GlobalDataController>();
			globalData.setParam (team, param, amount);
		} else {
			CmdSetParam (team, param, amount);
		}

	}
				
}
