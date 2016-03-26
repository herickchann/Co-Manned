using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour {

	LobbyRoomScript roomScript;

	[SyncVar]
	public GameManager.Team team;
	[SyncVar]
	public GameManager.Role role;

	void Awake(){
		//button.GetComponent<Text> ().text = playerName;
		roomScript = GetComponent<LobbyRoomScript>();
	}

	[Command]
	public void CmdNewPlayer(string netid){
		RpcNewPlayer (netid);
	}

	[ClientRpc]
	public void RpcNewPlayer(string netid){
		Debug.Log("new player: "+ netid +" has joined");
	}


	void Start(){
		// hook lobby player to room for interaction
		if(isLocalPlayer){
			CmdNewPlayer (GetComponent<NetworkIdentity>().netId.ToString());
			GameObject.Find ("RoomManager").GetComponent<LobbyRoomScript> ().lobbyPlayer = GetComponent<NetworkLobbyPlayer> ();
			roomScript.togglePanel ("HostJoinPanel");
			roomScript.togglePanel ("TeamSelection");
		}
	}

	[ClientRpc]
	public void RpcLockSelection(string id, GameManager.Team selectedTeam, GameManager.Role selectedRole){
		Debug.Log ("CLIENT: user:[" + id +"] picked: " + GameManager.teamString(selectedTeam) + " " + GameManager.roleString(selectedRole));
		// TODO: lock corresponding button
	}


	[Command]
	public void CmdSetTeamInfo (string id, GameManager.Team selectedTeam, GameManager.Role selectedRole){
		Debug.Log ("SERVER updating team and role: user: ["+ id +"] picking: " + GameManager.teamString(selectedTeam) + " " + GameManager.roleString(selectedRole));

		// set player object team and role on the server
		team = selectedTeam;
		role = selectedRole;

		// let everyone else know what role has been picked
		RpcLockSelection (id, team, role);
	}


	// detects ui interaction, need to know everything what team/role the player picked
	public void setTeamInfo(GameManager.Team selectedTeam, GameManager.Role selectedRole){
		if (!isLocalPlayer){
			Debug.Log ("not client");
			return;
		}
		// let the server know
		CmdSetTeamInfo (GetComponent<NetworkIdentity>().netId.ToString(), selectedTeam, selectedRole);
	}


}
