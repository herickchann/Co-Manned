using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


// this is where we will keep track information of all players
public class GlobalData : NetworkBehaviour {

	// health information
	public int maxHealth = 100;
	[SyncVar]
	public int blueHealth = 70;
	[SyncVar]
	public int redHealth = 80;




	void Start(){
		blueHealth = 70;
		redHealth = 80;
	}

	// HEALTH: get health
	public int getHealth(GameManager.Team team){
		switch (team) {
		case GameManager.Team.Blue:
			return blueHealth;
		case GameManager.Team.Red:
			return redHealth;
		case GameManager.Team.None:
			return 0;
		default:
			return 0;
		}
	}

	// HEALTH: 1. tell server we need to update all globalData instances for players
	public void setHealth(GameManager.Team team, int amount){

		// client telling server to update health on ALL clients
		if (!isLocalPlayer)
			return;

		CmdAllUpdate (team, amount);
	}

	// HEALTH: 2. server tells all clients to update their instances
	[Command]
	public void CmdAllUpdate(GameManager.Team team, int amount){
		RpcAllUpdate (team, amount);
	}

	// HEALTH: 3. client then tell the server to update its own instane
	[ClientRpc]
	public void RpcAllUpdate(GameManager.Team team, int amount){
		if (!isClient)
			return;

		Debug.Log (GameManager.teamString(team) + "got hit");
		CmdUpdateHealth (team, amount);
	}

	// HEALTH: 4. server update individual instance
	[Command]
	public void CmdUpdateHealth(GameManager.Team team, int amount){
		int newHealth = 0;
		switch (team) {
		case GameManager.Team.Blue:
			blueHealth -= amount;
			newHealth = blueHealth;
			break;
		case GameManager.Team.Red:
			redHealth -= amount;
			newHealth = redHealth;
			break;
		case GameManager.Team.None:
			break;
		default:
			break;
		}
		RpcSendMsg (GameManager.teamString (team) + " got hit and got updated to: " + newHealth.ToString ());
	}
		
	[ClientRpc]
	public void RpcSendMsg(string message){
		Debug.Log (message);
	}


}
